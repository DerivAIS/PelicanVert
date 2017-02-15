using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom packages
using QLNet;

namespace QLyx.InterestRates
{

    public class myRateCurve : IEnumerable, IEnumerator
    {


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        #region Instance properties and accessors


        // PRICING DATE
        #region

        protected DateTime _pricingDate = DateTime.Today;
        public DateTime pricingDate
        {
            get { return _pricingDate; }
            protected set { _pricingDate = value; }
        }

        #endregion


        // CURVE SETTLEMENT DATE
        #region

        protected DateTime _settlementDate;
        public DateTime settlementDate
        {
            get { return _settlementDate; }
            protected set { _settlementDate = value; }
        }

        #endregion


        // CURVE CALENDAR 
        #region Calendar

        //protected Calendar _CurveCalendar = new TARGET(); -- Old, maintenant dans constructeur
        protected Calendar _CurveCalendar = new TARGET();
        public Calendar CurveCalendar
        {
            get { return _CurveCalendar; }
            protected set { _CurveCalendar = value; }
        }

        #endregion


        // CURVE CURRENCY 
        #region Calendar

        protected Currency _CurveCurrency;
        public Currency CurveCurrency
        {
            get { return _CurveCurrency; }
            protected set { _CurveCurrency = value; }
        }

        #endregion


        // CURVE BUSINESS DAY CONVENTION 
        #region Business Day Convention

        protected BusinessDayConvention _CurveBusinessDayConvention = BusinessDayConvention.Unadjusted;
        public BusinessDayConvention CurveBusinessDayConvention
        {
            get { return _CurveBusinessDayConvention; }
            protected set { _CurveBusinessDayConvention = value; }
        }

        #endregion


        // CURVE BUSINESS DAY CONVENTION 
        #region Day Count Convention

        protected DayCounter _CurveDayCounter = new Actual360();
        public DayCounter CurveDayCounter
        {
            get { return _CurveDayCounter; }
            protected set { _CurveDayCounter = value; }
        }

        #endregion


        // FORWARD START (DAYS)
        #region Fixing Days / Forward Start Days

        protected int _CurveForwardStart;
        public int CurveForwardStart
        {
            get { return _CurveForwardStart; }
            protected set { this._CurveForwardStart = value; }
        }


        #endregion



        // YIELD TERM STRUCTURE
        #region Yield Term Structure -- from QLNet

        protected YieldTermStructure _yieldTermStructure;
        public YieldTermStructure yieldTermStructure
        {
            get
            {
                if (_yieldTermStructure == null) { this.setYieldTermStructure(); }
                return _yieldTermStructure;
            }
            protected set { _yieldTermStructure = value; }
        }

        #endregion



        // DISCOUNTING TERM STRUCTURE
        #region Discounting Term Structure -- from QLNet

        protected RelinkableHandle<YieldTermStructure> _discountingTermStructure;
        public RelinkableHandle<YieldTermStructure> discountingTermStructure
        {
            get
            {
                if (_discountingTermStructure == null)
                { this.setDiscountingTermStructure(); }

                this._discountingTermStructure.linkTo(this.yieldTermStructure);
                return this._discountingTermStructure;
            }
            protected set { _discountingTermStructure = value; }
        }

        #endregion



        // FORECASTING TERM STRUCTURE
        #region Forecasting Term Structure -- from QLNet

        protected RelinkableHandle<YieldTermStructure> _forecastingTermStructure;
        public RelinkableHandle<YieldTermStructure> forecastingTermStructure
        {
            get { return _forecastingTermStructure; }
            protected set { _forecastingTermStructure = value; }
        }

        #endregion



        // DISCOUNTING BOND ENGINE
        #region Discounting Engine for Bootstrapping -- from QLNet

        protected DiscountingBondEngine _discountingEngine;
        public DiscountingBondEngine discountingEngine
        {
            get
            {
                if (_discountingEngine == null) { this.setDiscountingEngine(); }
                return _discountingEngine;
            }
            protected set { _discountingEngine = value; }
        }

        #endregion


        // RATE ELEMENTS (List of MyRates)
        #region


        protected List<myRate> _rateElements = new List<myRate>();
        public List<myRate> rateElements
        {
            get { return _rateElements; }
            protected set { _rateElements = value; }
        }

        #endregion


        // RATE HELPERS (from QL type)
        #region


        protected List<RateHelper> _rateHelpers = new List<RateHelper>();
        public List<RateHelper> rateHelpers
        {
            get
            {
                // if (_rateHelpers.Count() == 0) { this.buildRateHelpers(); }
                return _rateHelpers;
            }
            protected set { _rateHelpers = value; }
        }


        #endregion


        // DAY COUNTER (DCC)
        #region Day Count Convention

        protected DayCounter _RateCurveDayCounter = new ActualActual(ActualActual.Convention.ISDA);
        public DayCounter RateCurveDayCounter
        {
            get { return _RateCurveDayCounter; }
            protected set { this._RateCurveDayCounter = value; }
        }


        #endregion


        // TOLERANCE 
        #region Tolerance

        protected double _RateCurveTolerance = 1.0e-15;
        public double RateCurveTolerance
        {
            get { return _RateCurveTolerance; }
        }

        #endregion


        // ENUMERATOR POSITION PROPERTY
        #region Enumerator Position
        private int _Position = -1;
        #endregion


        // DISCOUNT FACTORS (PRE-COMPUTED)
        #region


        protected Dictionary<DateTime, double> _discountFactors = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> discountFactors
        {
            get
            {
                return _discountFactors;
            }
            protected set { _discountFactors = value; }
        }


        #endregion


        // ZERO YIELDS (PRE-COMPUTED)
        #region


        protected Dictionary<DateTime, double> _zeroYields = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, double> zeroYields
        {
            get
            {
                return _zeroYields;
            }
            protected set { _zeroYields = value; }
        }


        #endregion


        #endregion



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region Constructors
            

        // Constructor 1 : Generic
        public myRateCurve() { }


        // Constructor 2 : Generic with date
        public myRateCurve(DateTime pricingDate,
                            List<myRate> argRateElements,
                            Currency Currency_,
                            Calendar Calendar_,
                            BusinessDayConvention BDC_,
                            DayCounter DCC_,
                            int FixingDays = 0)
        {

            // Set the calendar
            this.CurveCalendar = Calendar_;

            // Set the BDC
            this.CurveBusinessDayConvention = BDC_;

            // Set the DCC
            this.CurveDayCounter = DCC_;

            // Set the fixing delay
            this.CurveForwardStart = FixingDays;

            // Set the currency
            this.CurveCurrency = Currency_;

            // Set the pricing date
            this.pricingDate = pricingDate;

            //Set the settlement date
            this.settlementDate = this.CurveCalendar.advance(this.pricingDate, this.CurveForwardStart, TimeUnit.Days);

            // Set the rates objects
            this.rateElements = argRateElements;


        }

        
        #endregion



        // ************************************************************
        // METHODS -- SET PROPERTIES
        // ************************************************************

        #region Set Properties Methods


        // SET YIELD TERM STRUCTURE
        #region Yield Term Structure

        // SET YIELD TERM STRUCTURE
        public void setYieldTermStructure()
        {
            this.setYieldTermStructure(this.pricingDate);
        }


        // SET YIELD TERM STRUCTURE
        public void setYieldTermStructure(DateTime argPricingDate)
        {

            if (rateHelpers.Count() == 0) { buildRateHelpers(argPricingDate); }

            Date QLsettlementDate = this.CurveCalendar.adjust(new Date(argPricingDate), this.CurveBusinessDayConvention);

            this.yieldTermStructure =
                new PiecewiseYieldCurve<Discount, LogLinear>(QLsettlementDate, this.rateHelpers,
                                                                this.RateCurveDayCounter, new List<Handle<Quote>>(),
                                                                new List<Date>(), this.RateCurveTolerance);

        }


        #endregion



        // SET DISCOUNTING ENGINE
        #region Discounting Engine

        // NO DATE ARGUMENT
        public void setDiscountingEngine()
        {
            this.setDiscountingEngine(this.pricingDate);
        }


        // SET DISCOUNTING ENGINE WITH DATE
        public void setDiscountingEngine(DateTime argPricingDate)
        {
            this._discountingEngine = new DiscountingBondEngine(discountingTermStructure);
        }


        #endregion



        // SET DISCOUNTING TERM STRUCTURE
        #region Discounting Term Strucutre

        // SET DISCOUNTING TERM STRUCTURE
        public void setDiscountingTermStructure()
        {
            this.discountingTermStructure = new RelinkableHandle<YieldTermStructure>();
        }



        // SET FORECASTING TERM STRUCTURE
        public void setForecastingTermStructure()
        {
            RelinkableHandle<YieldTermStructure> forecastingTermStructure = new RelinkableHandle<YieldTermStructure>();
        }


        #endregion



        // SET THE BOND RATE HELPERS
        #region Bond Rate Helpers


        // SET THE BOND RATE HELPERS
        public void buildRateHelpers()
        {
            this.buildRateHelpers(this.pricingDate);
        }

        public void buildRateHelpers(DateTime argPricingDate)
        {

            // Build the RateHelper List -- Deposits
            foreach (myDepositRate r in this.rateElements.OfType<myDepositRate>())
            {
                try
                {
                    this.rateHelpers.Add(r.getHelper(argPricingDate)); 
                }

                catch { Console.WriteLine("Deposit rate helper skipped in building myRateCurve instance."); }
            }


            // Build the RateHelper List -- Swaps
            foreach (mySwapRate r in this.rateElements.OfType<mySwapRate>())
            {
                // this.rateHelpers.Add(r.getHelper(argPricingDate)); 
                try
                {
                    this.rateHelpers.Add(r.getHelper(argPricingDate)); 
                }

                catch { Console.WriteLine("Swap rate helper skipped in building myRateCurve instance."); }
            }


        }

        #endregion



        #endregion




        // ************************************************************
        // METHODS -- DISCOUNT FACTORS, ZERO RATES & FORWARD RATES
        // ************************************************************

        #region DF, Zero Rates, Forward Rates

        // COMPUTE THE DISCOUNT FACTOR
        public double getDiscountFactor(DateTime Maturity)
        {

            if (this.discountFactors.Keys.Contains(Maturity)) { return this.discountFactors[Maturity]; }

            else
            {
                // Set a zero coupon bond
                ZeroCouponBond zeroCouponBond = new ZeroCouponBond(
                            this.CurveForwardStart,
                            this.CurveCalendar,                     // new UnitedStates(UnitedStates.Market.GovernmentBond)
                            100.0,                                  // 100.0
                            new Date(Maturity),                  // new Date(15, Month.August, 2013),
                            this.CurveBusinessDayConvention,        // Following
                            100.0,                                  // 116.92
                            new Date());                            //new Date(15, Month.August, 2003));




                // Set the QL evaluation date
                Settings.setEvaluationDate(new Date(this.pricingDate));

                // Load the rate helpers
                this.buildRateHelpers();

                // Link TS used for discounting to Yield TS from Rate Elements
                this.discountingTermStructure.linkTo(this.yieldTermStructure);

                // Set the QL pricing engine
                zeroCouponBond.setPricingEngine(this.discountingEngine);

                // Compute DF
                this.discountFactors[Maturity] = zeroCouponBond.NPV();

                // Return
                return this.discountFactors[Maturity];

            }
        }

        // COMPUTE THE ZERO RATE
        public double getZeroYield(DateTime Maturity)
        {

            if (this.zeroYields.Keys.Contains(Maturity)) { return this.discountFactors[Maturity]; }

            else
            {
                // Set a zero coupon bond
                ZeroCouponBond zeroCouponBond = new ZeroCouponBond(
                            this.CurveForwardStart,
                            this.CurveCalendar,                     // new UnitedStates(UnitedStates.Market.GovernmentBond)
                            100.0,                                  // 100.0
                            new Date(Maturity),                  // new Date(15, Month.August, 2013),
                            this.CurveBusinessDayConvention,        // Following
                            100.0,                                  // 116.92
                            new Date());                            //new Date(15, Month.August, 2003));




                // Set the QL evaluation date
                Settings.setEvaluationDate(new Date(this.pricingDate));

                // Load the rate helpers
                this.buildRateHelpers();

                // Link TS used for discounting to Yield TS from Rate Elements
                this.discountingTermStructure.linkTo(this.yieldTermStructure);

                // Set the QL pricing engine
                zeroCouponBond.setPricingEngine(this.discountingEngine);

                // Compute Zero Yield
                this.zeroYields[Maturity] = zeroCouponBond.yield(this.CurveDayCounter, Compounding.Continuous, Frequency.Once);

                // Return
                return this.zeroYields[Maturity];

            }
        }

        // COMPUTE THE FORWARD RATE
        public double getZeroYield(DateTime FromDate, DateTime ToDate)
        {
            // Sanitize From Date 
            Date qlFromDate = this.CurveCalendar.adjust(new Date(FromDate));
            Date qlToDate = this.CurveCalendar.adjust(new Date(ToDate));

            // Set a zero coupon bond (from)
            ZeroCouponBond Begin_ZC = new ZeroCouponBond(
                            this.CurveForwardStart,
                            this.CurveCalendar,
                            100.0,
                            qlFromDate,
                            this.CurveBusinessDayConvention,
                            100.0,
                            new Date());


            // Set a zero coupon bond (to)
            ZeroCouponBond End_ZC = new ZeroCouponBond(
                            this.CurveForwardStart,
                            this.CurveCalendar,
                            100.0,
                            qlToDate,
                            this.CurveBusinessDayConvention,
                            100.0,
                            new Date());



            // Set the QL evaluation date
            Settings.setEvaluationDate(new Date(this.pricingDate));

            // Load the rate helpers
            this.buildRateHelpers();

            // Link TS used for discounting to Yield TS from Rate Elements
            this.discountingTermStructure.linkTo(this.yieldTermStructure);

            // Set the QL pricing engine
            Begin_ZC.setPricingEngine(this.discountingEngine);
            End_ZC.setPricingEngine(this.discountingEngine);

            // Compute Zero Yields
            this.zeroYields[FromDate] = Begin_ZC.yield(this.CurveDayCounter, Compounding.Continuous, Frequency.Once);
            this.zeroYields[ToDate] = End_ZC.yield(this.CurveDayCounter, Compounding.Continuous, Frequency.Once);

            // Compute Time Spans
            double nbDays = this.CurveDayCounter.yearFraction(new Date(FromDate), new Date(ToDate));
            double matuFrom = this.CurveDayCounter.yearFraction(new Date(this.pricingDate), new Date(FromDate));
            double matuTo = this.CurveDayCounter.yearFraction(new Date(this.pricingDate), new Date(ToDate));

            // Return Forward
            return (this.zeroYields[ToDate] * matuTo - this.zeroYields[FromDate] * matuFrom) / nbDays;

        }

        #endregion




        // ************************************************************
        // ENUMERATOR-RELATED METHODS
        // ************************************************************

        #region Enumerators


        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }



        public bool MoveNext()
        {

            if (_Position < this.rateElements.Count - 1)
            {
                ++_Position;
                return true;
            }
            return false;
        }



        public void Reset()
        {
            _Position = -1;
        }



        public object Current
        {
            get
            {
                // myRate rate = this.rateElements.ElementAt(Position);
                // return rate;

                return this.rateElements.ElementAt(_Position);
            }
        }



        #endregion



    }
}
