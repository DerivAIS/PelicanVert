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
    public class BermudeanCliquetBinaryOption<T> where T : UnderlyingIndex
    {
        

        // ************************************************************
        // PROPERTIES - UNDERLYING
        // ************************************************************

        #region Underlying

        // Underlying id
        // protected MarkitEquityUnderlying _underlyingMarkitID;

        // Underlying
        protected T _underlying;

        protected T underlying() {
            // if (_underlying == null) { SetUnderlyingIndex(); }
            if (_underlying == null) {
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

        // Constat dates
        List<DateTime> _observationDates = new List<DateTime>();

        // Pricing Date
        protected DateTime _valuationDate;

        // Pricing Date
        protected DateTime _strikeDate;

        #endregion


        // ************************************************************
        // PROPERTIES - OPTIONS DEFINITION
        // ************************************************************

        #region Option definition

        protected double _PDI_barrier;

        protected Kernel_BinaryCliquet _kernel;
        public Kernel_BinaryCliquet kernel()
        {
            if (_kernel == null) { SetKernel(); }
            return _kernel;
        }

        protected void SetKernel()
        {
            // Dates
            List<Date> _dates = new List<Date>();
            foreach (var dt in _observationDates)
            {
                _dates.Add(dt.ToDate());
            }

            
            double thisStrike = underlying().spot(_strikeDate);

            double PDI_barrier = _PDI_barrier; 

            // Instanciate
            _kernel = new Kernel_BinaryCliquet(_dates, _couponRate, _singleStrikeMoneyness,
                _singleCliquetMoneyness, PDI_barrier,thisStrike, _underlying.fixDiv(), _underlying.drift());

        }

        protected GeneralizedBlackScholesProcess process(DateTime valuationDate) {
            return underlying().stochasticProcess(valuationDate);
        }


        #endregion


        // ************************************************************
        // PROPERTIES - COUPONS
        // ************************************************************

        #region Options definition

        // Coupon rate (used if all binaries have the same payoffs)
        protected double _couponRate = double.NaN;

        // Coupons
        protected Dictionary<DateTime, double> _couponSchedule = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> coupons()
        {
            if (_couponSchedule.Count() == 0 || _couponSchedule == null)
            {
                Console.WriteLine("Binary Cliquet Option : Coupon schedule not set. Using the default coupon rate.");
                SetCouponsLevels(_couponRate);
            }
            return _couponSchedule;
        }

        // Set a unique coupon level for all observation dates (scalar)
        protected void SetCouponsLevels(double couponRate)
        {
            foreach (Date dt in _observationDates)
            {
                _couponSchedule[dt.ToDateTime()] = couponRate;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetCouponsLevels(List<double> coupons)
        {
            int k = 0;
            foreach (Date dt in _observationDates)
            {
                _couponSchedule[dt.ToDateTime()] = coupons.IndexOf(k);
                k++;
            }
        }

        #endregion


        // ************************************************************
        // PROPERTIES - STRIKE MONEYNESS
        // ************************************************************

        #region Strike moneyness

        // Single strike moneyness
        protected double _singleStrikeMoneyness = double.NaN;

        // Coupons
        protected Dictionary<DateTime, double> _strikeMoneyness = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> strikeMoneyness()
        {
            if (_strikeMoneyness.Count() == 0 || _strikeMoneyness == null)
            {
                Console.WriteLine("Binary Cliquet Option : Strike moneyness ill-defined. Setting from default (unique) strike moneyness.");
                SetStrikeLevels(_singleStrikeMoneyness);
            }
            return _strikeMoneyness;
        }

        // Set a unique coupon level for all observation dates (scalar)
        protected void SetStrikeLevels(double strikeMoneyness)
        {
            foreach (Date dt in _observationDates)
            {
                _strikeMoneyness[dt.ToDateTime()] = strikeMoneyness;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetStrikeLevels(List<double> strikeMoneyness)
        {
            int k = 0;
            foreach (Date dt in _observationDates)
            {
                _strikeMoneyness[dt.ToDateTime()] = strikeMoneyness.IndexOf(k);
                k++;
            }
        }

        #endregion


        // ************************************************************
        // PROPERTIES - CLIQUET MONEYNESS
        // ************************************************************

        #region Cliquet moneyness

        // Single strike moneyness
        protected double _singleCliquetMoneyness = double.NaN;

        // Coupons
        protected Dictionary<DateTime, double> _cliquetMoneyness = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> cliquetMoneyness()
        {
            if (_cliquetMoneyness.Count() == 0 || _cliquetMoneyness == null)
            {
                Console.WriteLine("Binary Cliquet Option : Cliquet moneyness ill-defined. Setting from default (unique) cliquet moneyness.");
                SetCliquetLevels(_singleCliquetMoneyness);
            }
            return _cliquetMoneyness;
        }

        // Set a unique coupon level for all observation dates (scalar)
        protected void SetCliquetLevels(double cliquetMoneyness)
        {
            foreach (Date dt in _observationDates)
            {
                _cliquetMoneyness[dt.ToDateTime()] = cliquetMoneyness;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetCliquetLevels(List<double> cliquetMoneyness)
        {
            int k = 0;
            foreach (Date dt in _observationDates)
            {
                _cliquetMoneyness[dt.ToDateTime()] = cliquetMoneyness.IndexOf(k);
                k++;
            }
        }

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

        public BermudeanCliquetBinaryOption(DateTime strikeDate, List<DateTime> observationDates, 
            double couponRate, double barrierMoneyness, double cliquetMoneyness, double PDI_Moneyness,
            Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
        {

            // Dates
            _strikeDate = strikeDate;
            _observationDates = observationDates;

            // Underlying
            // _underlying = underlying;

            // Calendar
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

            // option caracteristics
            _couponRate = couponRate;
            _singleStrikeMoneyness = barrierMoneyness;
            _singleCliquetMoneyness = cliquetMoneyness;
            _PDI_barrier = PDI_Moneyness;
        }

 
        public BermudeanCliquetBinaryOption(DateTime strikeDate, List<DateTime> observationDates, 
             List<double> coupons, List<double> barrierMoneyness, List<double> cliquetMoneyness, double PDI_Moneyness,
             Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
        {

            // Dates
            _strikeDate = strikeDate;
            _observationDates = observationDates;

            // Underlying
            // _underlying = underlying;

            // Calendar
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

            // Set the option properties
            SetCouponsLevels(coupons);
            SetStrikeLevels(barrierMoneyness);
            SetCliquetLevels(cliquetMoneyness);

            // PDI moneyness (as scalar, at maturity only)
            _PDI_barrier = PDI_Moneyness;

        }

        



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
