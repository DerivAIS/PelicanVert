
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

        public override double x0()
        {
            return x0_.link.value();
        }

        /*! \todo revise extrapolation */
        public override double drift(double t, double x)
        {
            double sigma = diffusion(t, x);
            // we could be more anticipatory if we know the right dt for which the drift will be used
            double t1 = t + 0.0001;
            return riskFreeRate_.link.forwardRate(t, t1, Compounding.Continuous, Frequency.NoFrequency, true).rate()
                   - dividendYield_.link.forwardRate(t, t1, Compounding.Continuous, Frequency.NoFrequency, true).rate()
                   - 0.5 * sigma * sigma;
        }

        /*! \todo revise extrapolation */
        public override double diffusion(double t, double x)
        {
            return localVolatility().link.localVol(t, x, true);
        }

        public override double apply(double x0, double dx)
        {
            return x0 * Math.Exp(dx);
        }

        /*! \warning raises a "not implemented" exception.  It should
               be rewritten to return the expectation E(S) of
               the process, not exp(E(log S)).
        */
        public override double expectation(double t0, double x0, double dt)
        {
            localVolatility(); // trigger update
            if (isStrikeIndependent_)
            {
                // exact value for curves
                return x0 *
                       Math.Exp(dt * (riskFreeRate_.link.forwardRate(t0, t0 + dt, Compounding.Continuous,
                                                            Frequency.NoFrequency, true).value() -
                                 dividendYield_.link.forwardRate(
                                     t0, t0 + dt, Compounding.Continuous, Frequency.NoFrequency, true).value()));
            }
            else
            {
                Utils.QL_FAIL("not implemented");
                return 0;
            }
        }
        public override double stdDeviation(double t0, double x0, double dt)
        {
            localVolatility(); // trigger update
            if (isStrikeIndependent_)
            {
                // exact value for curves
                return Math.Sqrt(variance(t0, x0, dt));
            }
            else
            {
                return discretization_.diffusion(this, t0, x0, dt);
            }
        }
        public override double variance(double t0, double x0, double dt)
        {
            localVolatility(); // trigger update
            if (isStrikeIndependent_)
            {
                // exact value for curves
                return blackVolatility_.link.blackVariance(t0 + dt, 0.01) -
                       blackVolatility_.link.blackVariance(t0, 0.01);
            }
            else
            {
                return discretization_.variance(this, t0, x0, dt);
            }
        }
        public override double evolve(double t0, double x0, double dt, double dw)
        {
            localVolatility(); // trigger update
            if (isStrikeIndependent_)
            {
                // exact value for curves
                double var = variance(t0, x0, dt);
                double drift = (riskFreeRate_.link.forwardRate(t0, t0 + dt, Compounding.Continuous,
                                                         Frequency.NoFrequency, true).value() -
                              dividendYield_.link.forwardRate(t0, t0 + dt, Compounding.Continuous,
                                                          Frequency.NoFrequency, true).value()) *
                                 dt - 0.5 * var;
                return apply(x0, Math.Sqrt(var) * dw + drift);
            }
            else
                return apply(x0, discretization_.drift(this, t0, x0, dt) + stdDeviation(t0, x0, dt) * dw);
        }
        public override double time(Date d)
        {
            return riskFreeRate_.link.dayCounter().yearFraction(riskFreeRate_.link.referenceDate(), d);
        }
        public override void update()
        {
            updated_ = false;
            base.update();
        }
        
        public Handle<Quote> stateVariable()
        {
            return x0_;
        }
        public Handle<YieldTermStructure> dividendYield()
        {
            return dividendYield_;
        }
        public Handle<YieldTermStructure> riskFreeRate()
        {
            return riskFreeRate_;
        }
        public Handle<BlackVolTermStructure> blackVolatility()
        {
            return blackVolatility_;
        }

        public Handle<LocalVolTermStructure> localVolatility()
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
