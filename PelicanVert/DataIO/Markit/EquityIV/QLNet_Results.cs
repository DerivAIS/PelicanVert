using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO.Markit
{

    // ************************************************************
    // QLNET EXTRACTION RESULTS
    // ************************************************************

    #region QLNet termstructure data class

    public class QLNet_Results
    {

        // Internal properties
        public List<Handle<Quote>> dataQuoteHandles() { return dataQuoteHandles_; }
        protected List<Handle<Quote>> dataQuoteHandles_ = new List<Handle<Quote>>();

        public List<double> dataPoints() { return dataPoints_; }
        protected List<double> dataPoints_ = new List<double>();

        public List<Date> timeStamps() { return timeStamps_; }
        protected List<Date> timeStamps_ = new List<Date>();


        // Constructors

        public QLNet_Results(Dictionary<DateTime, Double> thisData)
        {
            SetData(thisData);
        }


        protected void SetData(Dictionary<DateTime, Double> data)
        {


            foreach (KeyValuePair<DateTime, double> kvp in data)
            {
                timeStamps_.Add(new Date(kvp.Key));
                dataQuoteHandles_.Add(new Handle<Quote>(new SimpleQuote(kvp.Value)));
                dataPoints_.Add(kvp.Value);
            }
        }

    }

    #endregion



    // ************************************************************
    // RISK FREE PIECEWISE TERM STRUCTURE
    // ************************************************************

    #region QLNet termstructure data class

    public class ZeroRateTermStructure : YieldTermStructure
    {

        public override Date maxDate() {
            throw new NotImplementedException();
        }

        protected List<Handle<Quote>> zeroRates_ = new List<Handle<Quote>>(); //dayCounter_
        protected DayCounter dayCounter_ = new Actual365Fixed();

        public List<double> rateTimes()
        {
            if (rateTimes_.Count() == 0) { SetRateTimes(); }
            return rateTimes_;
        }

        private void SetRateTimes()
        {
            double yearFrac = 0.0;

            foreach (DateTime d in rateDates_)
            {
                yearFrac = dayCounter_.yearFraction(pricingDate_, d);
                rateTimes_.Add(yearFrac);
            }

 
        }

        protected List<double> rateTimes_ = new List<double>();

        protected List<Date> rateDates_ = new List<Date>();
        protected Date pricingDate_;


        #region Constructors
        /* Other constructors
        public ZeroRateTermStructure(DayCounter dc = null, List<Handle<Quote>> jumps = null, List<Date> jumpDates = null)
           : base(dc, jumps, jumpDates)
        { }

        public ZeroRateTermStructure(Date referenceDate, Calendar calendar = null, DayCounter dc = null,
            List<Handle<Quote>> jumps = null, List<Date> jumpDates = null)
           : base(referenceDate, calendar, dc, jumps, jumpDates)
        { }
        */

        public ZeroRateTermStructure(Date pricingDate, int settlementDays, Calendar calendar, DayCounter dc,
            List<Handle<Quote>> jumps = null, List<Date> jumpDates = null)
           : base(settlementDays, calendar, dc, jumps, jumpDates)
        {

            zeroRates_ = jumps;
            rateDates_ = jumpDates;

            pricingDate_ = pricingDate;

        }

        #endregion



        #region Calculations

        // This method must be implemented in derived classes to
        // perform the actual calculations. When it is called,
        // range check has already been performed; therefore, it
        // must assume that extrapolation is required.

        //! zero-yield calculation
        protected double zeroYieldImpl(double t)
        {
            Console.WriteLine("We're here ... ");
            throw new NotImplementedException();
            // if (rateTimes_.Contains(t)) { return zeroRates_.ElementAt( rateTimes_.IndexOf(t)) as double ; }

        }

        #endregion



        #region YieldTermStructure implementation

        /*! Returns the discount factor for the given date calculating it
            from the zero yield.
        */
        protected override double discountImpl(double t)
        {
            if (t == 0.0)     // this acts as a safe guard in cases where
                return 1.0;   // zeroYieldImpl(0.0) would throw.

            double r = zeroYieldImpl(t);
            return Math.Exp(-r * t);
        }

        #endregion


    }
    #endregion
}

