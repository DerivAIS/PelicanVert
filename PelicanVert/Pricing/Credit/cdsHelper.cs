

///////  Marc RAYGOT - 2017   ///////


using System;
using System.Collections.Generic;

namespace QLNet
{
    public class CreditDefaultSwapHelper : BootstrapHelper<DefaultProbabilityTermStructure>
    {

        public CreditDefaultSwapHelper(double cdsSpread,
                                       Period cdsTenor,
                                       Handle<YieldTermStructure> discount,
                                       Handle<DefaultProbabilityTermStructure> HazardHandle = null)
            : base(cdsSpread)
        {
            cdsTenor_ = cdsTenor;
            cdsSpread_ = cdsSpread;
            calendar_ = new TARGET();
            dayCount_ = new Actual365Fixed();
            discountHandle_ = discount ?? new Handle<YieldTermStructure>();
            HazardHandle_ = HazardHandle ?? new Handle<DefaultProbabilityTermStructure>();

            discountHandle_.registerWith(update);

            initializeDates();
        }

        protected void initializeDates()
        {
            
            cds_ = new MakeCreditDefaultSwap(cdsTenor_,cdsSpread_)
                                              .withEngine(hazardRelinkableHandle_, discountHandle_)
                                              .withType(Protection.Side.Buyer);
                                               
            earliestDate_ = cds_.protectionStartDate();
            maturityDate_ = earliestDate_ + cdsTenor_;
            latestDate_ = cds_.protectionEndDate();
            pillarDate_ = latestDate_;
            latestRelevantDate_ = latestDate_;
        }

        public override void setTermStructure(DefaultProbabilityTermStructure t)
        {
            // do not set the relinkable handle as an observer -
            // force recalculation when needed
            termStructureHandle_.linkTo(t, false);
            base.setTermStructure(t);
            hazardRelinkableHandle_.linkTo(hazardRelinkableHandle_.empty() ? t : hazardRelinkableHandle_, false);
        }

        public override double impliedQuote()
        {
            Utils.QL_REQUIRE(termStructure_ != null, () => "term structure not set");
            // we didn't register as observers - force calculation
            cds_.recalculate();                // it is from lazy objects

            double result = cds_.fairSpread();
            return result;
        }

        //! \name cdsHelper inspectors
        public double spread() { return cdsSpread_; }
        public CreditDefaultSwap cds() { return cds_; }

        protected Period cdsTenor_;
        protected Calendar calendar_;
        protected DayCounter dayCount_;
        protected CreditDefaultSwap cds_;
        // need to init this because it is used before the handle has any link, i.e. setTermStructure will be used after ctor
        protected RelinkableHandle<DefaultProbabilityTermStructure> termStructureHandle_ = new RelinkableHandle<DefaultProbabilityTermStructure>();
        protected double cdsSpread_;

        protected Handle<YieldTermStructure> discountHandle_;
        protected Handle<DefaultProbabilityTermStructure> HazardHandle_;
        protected RelinkableHandle<DefaultProbabilityTermStructure> hazardRelinkableHandle_ = new RelinkableHandle<DefaultProbabilityTermStructure>();
     
    }

}
