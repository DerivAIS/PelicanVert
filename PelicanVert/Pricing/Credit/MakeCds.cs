

///////  Marc RAYGOT - 2017   ///////


using System;

namespace QLNet
{
    // helper class
    // This class provides a more comfortable way to instantiate standard market CDS.
    public class MakeCreditDefaultSwap
    {
        private Period cdsTenor_;
        private double cdsSpread_;
        private double recoveryRate_;

        private Handle<DefaultProbabilityTermStructure> probabilityTermStructure_;
        private Handle<YieldTermStructure> discountingTermStructure_;
        IPricingEngine engine_;

        private Protection.Side side_;
        private double nominal_;
        private BusinessDayConvention cdsConvention_;
        private DateGeneration.Rule cdsRule_;
        private DayCounter cdsDayCount_;
        private Date effectiveDate_;
        private Calendar cdsCalendar_;



        public MakeCreditDefaultSwap(Period cdsTenor, double cdsSpread)
        {
            cdsTenor_ = cdsTenor;
            cdsSpread_ = cdsSpread;
            recoveryRate_ = 0.4;

            cdsCalendar_ = new NullCalendar();
            nominal_ = 100.0;
            side_ = Protection.Side.Buyer;

            cdsConvention_ = BusinessDayConvention.ModifiedFollowing;
            cdsRule_ = DateGeneration.Rule.Backward;
            cdsDayCount_ = new  Actual365Fixed();
        }

       
        public MakeCreditDefaultSwap withType(Protection.Side type)
        {
            side_ = type;
            return this;
        }

        public MakeCreditDefaultSwap withEffectiveDate(Date effectiveDate)
        {
            effectiveDate_ = effectiveDate;
            return this;
        }

        public MakeCreditDefaultSwap withRecoveryRate(double recoveryRate)
        {
            recoveryRate_ = recoveryRate;
            return this;
        }

        public MakeCreditDefaultSwap withEngine(Handle<DefaultProbabilityTermStructure> probabilityTermStructure, Handle<YieldTermStructure> discountingTermStructure)
        {
            discountingTermStructure_ = discountingTermStructure;
            probabilityTermStructure_ = probabilityTermStructure;

            engine_ = new MidPointCdsEngine(probabilityTermStructure, recoveryRate_, discountingTermStructure);
            return this;
        }

        public MakeCreditDefaultSwap withPricingEngine(IPricingEngine engine)
        {
            engine_ = engine;
            return this;
        }

        public MakeCreditDefaultSwap withFixedLegCalendar(Calendar cal)
        {
            cdsCalendar_ = cal;
            return this;
        }
        public MakeCreditDefaultSwap withFixedLegConvention(BusinessDayConvention bdc)
        {
            cdsConvention_ = bdc;
            return this;
        }
   



        // CDS creator
        public static implicit operator CreditDefaultSwap(MakeCreditDefaultSwap o) { return o.value(); }
        
        public CreditDefaultSwap value()
        {
            Date startDate;
            //double lastCdsPayementtime;

            if (effectiveDate_ != null)
                startDate = effectiveDate_;
            else
            {
                Date refDate = Settings.evaluationDate();
                startDate = cdsCalendar_.adjust(refDate, BusinessDayConvention.Following);
            }

            
            // Standard CDS calendar dates
            Date startDateSchedule = new Date(20, Month.March, startDate.year());
            Date endDateSchedule = new Date(20, Month.December, startDate.year()+50);
            Period cdsSchedulePeriode = new Period(3, TimeUnit.Months);

            Schedule standardSchedule = new Schedule(startDateSchedule,
                                         endDateSchedule,
                                         cdsSchedulePeriode,
                                         cdsCalendar_,
                                         cdsConvention_,
                                         cdsConvention_,
                                         cdsRule_,
                                         false);



            Date endDate = new Date();
            /*
            // compute the last coverred date : the previous quaterly date (mod 0.25)

            double extratime = (1-discountingTermStructure_.link.timeFromReference(new Date(1, Month.Jan, startDate.year()+1)))% 0.25;
            lastCdsPayementtime = discountingTermStructure_.link.timeFromReference(startDate + cdsTenor_) - extratime;
            endDate = cdsCalendar_.advance(startDate, (int)(lastCdsPayementtime*365.0), TimeUnit.Days);
            */
            Date previousDate = startDate;

            foreach(Date date in standardSchedule.dates())
            {
                if (date > startDate + cdsTenor_)
                {
                    endDate = previousDate;
                    break;
                }
                previousDate = date;
            }

            Schedule cdsSchedule = new Schedule(startDate, endDate,
                                   cdsSchedulePeriode, cdsCalendar_,
                                   cdsConvention_, cdsConvention_,
                                   cdsRule_, false);

            DayCounter cdsDayCount = cdsDayCount_;
         
                CreditDefaultSwap cds = new CreditDefaultSwap(side_, nominal_, cdsSpread_, cdsSchedule, cdsConvention_, cdsDayCount);

            if (engine_ == null)
            {
                throw new Exception("No engine set for CDS");
            }
            else
                cds.setPricingEngine(engine_);

            return cds;
        }
    }
}
