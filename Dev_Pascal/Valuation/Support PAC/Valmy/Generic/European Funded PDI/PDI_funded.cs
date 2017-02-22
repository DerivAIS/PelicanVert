using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// external custom
using QLNet;

// internal custom
using QLyx.DataIO;
using QLyx.DataIO.Markit;
using QLyx.Utilities;
using QLyx.Containers;

using Pascal.Pricing.Engines;
using Pascal.Pricing.Exercises;
using Pascal.Pricing.Instruments;
using Pascal.Pricing.Underlyings;

namespace Pascal.Valuation
{
    public class European_Funded_PDI<T> where T : UnderlyingIndex
    {


        // ************************************************************
        // PROPERTIES - UNDERLYING
        // ************************************************************

        #region Underlying

        // Underlying id
        // protected MarkitEquityUnderlying _underlyingMarkitID;

        // Underlying
        protected T _underlying;

        protected T underlying()
        {
            // if (_underlying == null) { SetUnderlyingIndex(); }
            if (_underlying == null)
            {
                var type = typeof(T);
                _underlying = (T)Activator.CreateInstance(type);
            }
            return _underlying;
        }

        /*
        protected void SetUnderlyingIndex()
        {
            _underlying = new UnderlyingIndex(_underlyingMarkitID, _dayCounter, _bdc);
        }
        */

        // Forward request to underlying index object
        public double strikeLevel()
        {
            return underlying().spot(_strikeDate);
        }

        // Forward request to underlying index object
        public double spot(DateTime valuationDate)
        {
            return underlying().spot(valuationDate);
        }

        #endregion


        // ************************************************************
        // PROPERTIES - CALENDAR AND DATES
        // ************************************************************

        #region Calendar, Day counter, Dates, etc.

        // Calendar
        protected Calendar _calendar = new NullCalendar();

        // Calendar
        protected DayCounter _dayCounter = new Actual365Fixed();

        // Business Day Convention  
        protected BusinessDayConvention _bdc = BusinessDayConvention.Preceding;

        // Settlement delay
        protected int settlementDelay = 0;

        // Observation date
        List<DateTime> _observationDate; 

        // Pricing Date
        protected DateTime _valuationDate;

        // Pricing Date
        protected DateTime _strikeDate;

        #endregion


        // ************************************************************
        // PROPERTIES - OPTIONS DEFINITION
        // ************************************************************

        #region Option definition
            
        protected Kernel_FundedPDI _kernel;
        public Kernel_FundedPDI kernel()
        {
            if (_kernel == null) { SetKernel(); }
            return _kernel;
        }

        protected void SetKernel()
        {
            // Get spot level at strike on a historical basis
            double thisStrike = underlying().spot(_strikeDate);

            // Instanciate
            _kernel = new Kernel_FundedPDI(_observationDate.ToDateList(), _strikeMoneyness, _barrierMoneyness, 
                thisStrike, _leverage_down, _underlying.fixDiv(), _underlying.drift());
        }

        protected GeneralizedBlackScholesProcess process(DateTime valuationDate)
        {
            return underlying().stochasticProcess(valuationDate);
        }


        #endregion


        // ************************************************************
        // PROPERTIES - STRIKE MONEYNESS
        // ************************************************************

        #region Strike moneyness

        // Barrier moneyness
        protected double _barrierMoneyness;

        // Strike moneyness
        protected double _strikeMoneyness = double.NaN;

        // Downside leverage (if barrier is triggered)
        protected double _leverage_down = 1.0;


        #endregion



        // ************************************************************
        // PROPERTIES - MONTE CARLO PARAMETERS
        // ************************************************************

        #region Monte Carlo Parameters

        private int _nbSamples = 0;
        public int nbSamples()
        {
            if (_nbSamples == 0) { _nbSamples = 10000; }
            return _nbSamples;
        }


        private int _nbSteps = 0;
        public int nbSteps()
        {
            // if (_nbSteps == 0) { _nbSteps = 12 * _exercise.dates().Count(); }
            if (_nbSteps == 0) { _nbSteps = 100; }
            return _nbSteps;
        }

        #endregion


        // ************************************************************
        // CONSTRUCTOR -- MONEYNESS (INCLUDES A STRIKE DATE)
        // ************************************************************

        public European_Funded_PDI(DateTime strikeDate, List<DateTime> observationDates, double strikeMoneyness, double barrierMoneyness, double leverageDown,
            Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
        {

            // Dates
            _strikeDate = strikeDate;
            _observationDate = observationDates;

            // Calendar
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

            // Option caracteristics
            _strikeMoneyness = strikeMoneyness;
            _barrierMoneyness = barrierMoneyness;
            _leverage_down = leverageDown;
           
        }

        public European_Funded_PDI(DateTime strikeDate, DateTime observationDate, double strikeMoneyness, double barrierMoneyness, double leverageDown,
            Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)

            : this(strikeDate, new List<DateTime>() { observationDate },  strikeMoneyness,  barrierMoneyness,  leverageDown, calendar,  dayCounter,  bdc)

        {  }

        // ************************************************************
        // METHODS -- DATE ADJUSTEMENT
        // ************************************************************

            
            #region Date adjustment

        protected DateTime Adjust(DateTime dateTime)
        {
            Date d = _calendar.adjust(dateTime.ToDate(), _bdc);
            return d.ToDateTime();
        }

        protected Date Adjust(Date date)
        {
            return _calendar.adjust(date, _bdc);
        }

        protected List<DateTime> Adjust(List<DateTime> dateList)
        {
            List<DateTime> res = new List<DateTime>();
            foreach (DateTime dt in dateList)
            {
                res.Add(Adjust(dt));
            }
            return res;
        }

        #endregion


        // ************************************************************
        // METHODS -- PRICING ENGINE --> MERCI MARC !
        // ************************************************************

        #region Pricing Engine

        private IPricingEngine PricingEngine(DateTime valuationDate)
        {
            var _process = process(valuationDate);

            return new MakeMGenericInstrument<PseudoRandom>(_process).withAbsoluteTolerance(0.1)
                                                                   .withStepsPerYear(12)
                                                                   .withSeed(42)
                                                                   .value();
        }


        protected void setPricingEngine(IPricingEngine engine)
        {
            kernel().setPricingEngine(engine);
        }


        #endregion


        // ************************************************************
        // METHODS -- NPV
        // ************************************************************

        #region Computational methods for NPV

        public double NPV(DateTime pricingDate)
        {

            // Set the pricing date for QLNet and locally
            Settings.setEvaluationDate(pricingDate.ToDate());
            _valuationDate = pricingDate;

            setPricingEngine(PricingEngine(pricingDate));

            // Price
            return kernel().NPV();
        }


        #endregion



    }


}
