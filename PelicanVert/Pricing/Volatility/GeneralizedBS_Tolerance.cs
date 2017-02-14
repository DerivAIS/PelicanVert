
using System;

namespace QLNet
{

    public class GeneralizedBlackScholesProcessTolerance : GeneralizedBlackScholesProcess
    {

        public GeneralizedBlackScholesProcessTolerance(Handle<Quote> x0, Handle<YieldTermStructure> dividendTS,
           Handle<YieldTermStructure> riskFreeTS, Handle<BlackVolTermStructure> blackVolTS, IDiscretization1D disc = null)
            : base(x0, dividendTS, riskFreeTS, blackVolTS, disc)
        {
            x0_ = x0;
            riskFreeRate_ = riskFreeTS;
            dividendYield_ = dividendTS;
            blackVolatility_ = blackVolTS;
            updated_ = false;

            x0_.registerWith(update);
            riskFreeRate_.registerWith(update);
            dividendYield_.registerWith(update);
            blackVolatility_.registerWith(update);
        }
      
        public override double diffusion(double t, double x)
        {
            return localVolatility().link.localVol(t, x, true);
        }
       

        public new Handle<LocalVolTermStructure> localVolatility()
        {
            if (!updated_)
            {
                isStrikeIndependent_ = true;

                // constant Black vol?
                BlackConstantVol constVol = blackVolatility().link as BlackConstantVol;
                if (constVol != null)
                {
                    // ok, the local vol is constant too.
                    localVolatility_.linkTo(new LocalConstantVol(constVol.referenceDate(),
                       constVol.blackVol(0.0, x0_.link.value()),
                       constVol.dayCounter()));
                    updated_ = true;
                    return localVolatility_;
                }

                // ok, so it's not constant. Maybe it's strike-independent?
                BlackVarianceCurve volCurve = blackVolatility().link as BlackVarianceCurve;
                if (volCurve != null)
                {
                    // ok, we can use the optimized algorithm
                    localVolatility_.linkTo(new LocalVolCurve(new Handle<BlackVarianceCurve>(volCurve)));
                    updated_ = true;
                    return localVolatility_;
                }

                // ok, so it's strike-dependent. Never mind.
                localVolatility_.linkTo(new LocalVolSurfaceTolerance(blackVolatility_, riskFreeRate_, dividendYield_,
                   x0_));
                updated_ = true;
                isStrikeIndependent_ = false;
                return localVolatility_;
            }
            else
            {
                return localVolatility_;
            }
        }
        
        private Handle<Quote> x0_;
        private Handle<YieldTermStructure> riskFreeRate_, dividendYield_;
        private Handle<BlackVolTermStructure> blackVolatility_;
        private RelinkableHandle<LocalVolTermStructure> localVolatility_ = new RelinkableHandle<LocalVolTermStructure>();
        private bool updated_, isStrikeIndependent_;
    }
}
