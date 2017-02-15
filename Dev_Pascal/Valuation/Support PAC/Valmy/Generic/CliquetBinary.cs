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

namespace Pascal.Valuation
{
    public class BermudeanCliquetBinaryOption
    {


        // ************************************************************
        // PROPERTIES - INFRASTUCTURE
        // ************************************************************

        #region Infrastructure

        // Le référentiel de la database
        private ReferenceManager _referentiel = ReferenceManager.Factory;
        private ReferenceManager referentiel()
        {
            if (_referentiel == null) { _referentiel = ReferenceManager.Factory; }
            return _referentiel;
        }
        
        // Accès au référentiel de la database
        private IDtoken GetToken(int referenceDBID)
        {
            return _referentiel.Identify(new DBID(referenceDBID));
        }
        
        // Accès aux données de la base via le helper
        protected myFrame EndOfDay(int referenceDBID, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            IDtoken thisToken = GetToken(referenceDBID);
            return EndOfDay(thisToken, startDate, endDate);
        }

        protected myFrame EndOfDay(IDtoken referenceToken, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            if (startDate == DateTime.MinValue) { startDate = _strikeDate.AddDays(-31); }
            if (endDate == DateTime.MinValue) { endDate = _valuationDate; }

            HistoricalDataRequest myRequest = new HistoricalDataRequest(referenceToken, startDate, endDate);
            return _localDatabase.GetEODPrices(myRequest);
        }
        
        // Helper pour la DB locale
        DatabaseHelper _localDatabase = new DatabaseHelper();
        
        // Market Data (Markit)
        protected Markit_Equity_IV _marketData;
        protected MarkitSurface marketData(DateTime valuationDate)
        {
            if (_marketData == null) { _marketData = Markit_Equity_IV.Instance(_underlying); }
            return _marketData[valuationDate];
        }

        #endregion


        // ************************************************************
        // PROPERTIES - UNDERLYING
        // ************************************************************

        #region Underlying

        // Underlying
        protected MarkitEquityUnderlying _underlying;

        // Underlying value on strike date (the strike level)
        protected double _spotAtStrike = Double.NaN;
        public double strikeLevel() {
            if (_spotAtStrike == Double.NaN) { _spotAtStrike = underlying(_strikeDate); }
            return _spotAtStrike;
        }

        // Identification
        protected DBID underlyingID() { return new DBID((int)_underlying); }

        // Accessing the Time series
        protected myFrame _underlying_timeSeries;
        protected double underlying(DateTime date)
        {
            if (_underlying_timeSeries == null) { SetUnderlyingTimeSeries(); }
            return (double)_underlying_timeSeries[date]["Close"];
        }

        // Setting the Time series
        protected void SetUnderlyingTimeSeries()
        {
            _underlying_timeSeries = EndOfDay(underlyingID());
        }

        public double currentSpot() {
            return underlying(_valuationDate);
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

        #region Options definition *** PASCAL ***
       
         // Exercise
          protected PathDepExercise _exercise;
        public PathDepExercise exercise() {
            if (_exercise == null) { SetExercise(); }
            return _exercise;
        }

        protected void SetExercise()
        {
            // _exercise = new PathDepExercise(Exercise.Type.Bermudan, true);
            List<Date> _dates = new List<Date>();
            foreach (var dt in _observationDates)
            {
                _dates.Add(dt.ToDate());
            }
            _exercise = new PathDepExercise(_dates);
        }

        // Payoff
        protected CliquetBinarySequence_Payoff _payoff;
        public CliquetBinarySequence_Payoff payoff()
        {
            if (_payoff == null) { SetPayoff(); }
            return _payoff;
        }

        protected void SetPayoff()
        {
            _payoff = new CliquetBinarySequence_Payoff(1.0, strikeLevel(), _couponSchedule.Values.ToList(), 
                                                        _strikeMoneyness.Values.ToList(), _cliquetMoneyness.Values.ToList());
        }

        // Instrument
        protected CliquetBinarySequence_Instrument _instrument;
        public CliquetBinarySequence_Instrument instrument()
        {
            if (_instrument == null) { SetInstrument(); }
            return _instrument;
        }
        protected void SetInstrument()
        {
            var exer = exercise();
            var poff = payoff();
            _instrument = new CliquetBinarySequence_Instrument(_valuationDate, exer, poff);
        }

        #endregion


        #region *** MARC ***

        protected ScriptedBinaire _binaire;
        public ScriptedBinaire binaire() {
            if (_binaire == null) { SetScriptedBinaire(); }
            return _binaire;
        }

        protected void SetScriptedBinaire()
        {

            // Dates
            List<Date> _dates = new List<Date>();
            foreach (var dt in _observationDates)
            {
                _dates.Add(dt.ToDate());
            }

            _binaire = new ScriptedBinaire(_dates, _couponRate, _singleStrikeMoneyness, _singleCliquetMoneyness, _singleStrikeMoneyness, 154.0, currentSpot());

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
        public Dictionary<DateTime, double> coupons() {
            if (_couponSchedule.Count() == 0 || _couponSchedule == null) {
                Console.WriteLine("Binary Cliquet Option : Coupon schedule not set. Using the default coupon rate.");
                SetCouponsLevels(_couponRate);
            }
            return _couponSchedule;
        }

        // Set a unique coupon level for all observation dates (scalar)
        protected void SetCouponsLevels(double couponRate) {
            foreach (Date dt in _exercise.dates()) {
                _couponSchedule[dt.ToDateTime()] = couponRate;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetCouponsLevels(List<double> coupons)
        {
            int k = 0;
            foreach (Date dt in _exercise.dates())
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
            foreach (Date dt in _exercise.dates())
            {
                _strikeMoneyness[dt.ToDateTime()] = strikeMoneyness;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetStrikeLevels(List<double> strikeMoneyness)
        {
            int k = 0;
            foreach (Date dt in _exercise.dates())
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
            foreach (Date dt in _exercise.dates())
            {
                _cliquetMoneyness[dt.ToDateTime()] = cliquetMoneyness;
            }
        }

        // Set a coupon levels for all observation dates (vector)
        protected void SetCliquetLevels(List<double> cliquetMoneyness)
        {
            int k = 0;
            foreach (Date dt in _exercise.dates())
            {
                _cliquetMoneyness[dt.ToDateTime()] = cliquetMoneyness.IndexOf(k);
                k++;
            }
        }

        #endregion


        // ************************************************************
        // PROPERTIES - STOCHASTIC PROCESS
        // ************************************************************

        #region Stochastic Process

        // Process
        protected GeneralizedBlackScholesProcessTol _process;
        public GeneralizedBlackScholesProcessTol process() {
            if (_process == null) { SetStochasticProcess(_valuationDate); }
            return _process;
        }

        // Set the stochastic process
        protected void SetStochasticProcess(DateTime valuationDate)
        {
            
            // Sous-jacent : Spot de Bloomberg
            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(currentSpot()));

            // Yield Term Structure
            Handle<YieldTermStructure> RateTermStructure = marketData(_valuationDate).riskFree_FwdCrv();
      
            // Dividend Yield
            var DivTermStructure = marketData(_valuationDate).dividend_FwdCrv();
            
            // Volatility Surface
            Matrix volMat = marketData(_valuationDate).VolMatrix();
            List<Date> dates = marketData(_valuationDate).Dates();
            Calendar cal = marketData(_valuationDate).calendar();
            List<double> strikes = marketData(_valuationDate).Strikes(currentSpot());

            BlackVarianceSurface mySurface = new BlackVarianceSurface(_valuationDate, cal, dates,
                                                                    strikes, volMat, marketData(_valuationDate).dayCounter());

            // Surface Declaration
            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(mySurface);

            // Set the process locally 
            _process = new GeneralizedBlackScholesProcessTol(underlyingH, DivTermStructure, RateTermStructure, mySurfaceH);

        }

        #endregion




        // ************************************************************
        // PROPERTIES - MONTE CARLO PARAMETERS
        // ************************************************************

        #region Monte Carlo Parameters

        private int _nbSamples = 0;
        public int nbSamples() {
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
        // CONSTRUCTOR
        // ************************************************************

        public BermudeanCliquetBinaryOption(DateTime valuationDate, DateTime strikeDate, List<DateTime> observationDates, 
            MarkitEquityUnderlying underlying, double couponRate, double barrierMoneyness, double cliquetMoneyness, 
            Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc) {

            // Dates
            _valuationDate = valuationDate;
            _strikeDate = strikeDate;
            _observationDates = observationDates;

            // Underlying
            _underlying = underlying;

            // Calendar
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

            // option caracteristics
            _couponRate = couponRate;
            _singleStrikeMoneyness = barrierMoneyness;
            _singleCliquetMoneyness = cliquetMoneyness;
        }

        public BermudeanCliquetBinaryOption(DateTime strikeDate, List<DateTime> observationDates, MarkitEquityUnderlying underlying,
            double couponRate, double barrierMoneyness, double cliquetMoneyness, Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
            : this(new DateTime(), strikeDate, observationDates, underlying, couponRate, barrierMoneyness, cliquetMoneyness, calendar, dayCounter, bdc)
        { }


        public BermudeanCliquetBinaryOption(DateTime valuationDate, DateTime strikeDate, List<DateTime> observationDates,
             MarkitEquityUnderlying underlying, List<double> coupons, List<double> barrierMoneyness, List<double> cliquetMoneyness, 
             Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
        {

            // Dates
            _valuationDate = valuationDate;
            _strikeDate = strikeDate;
            _observationDates = observationDates;

            // Underlying
            _underlying = underlying;

            // Calendar
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

            // Set the option properties
            SetCouponsLevels(coupons);
            SetStrikeLevels(barrierMoneyness);
            SetCliquetLevels(cliquetMoneyness);

        }

        public BermudeanCliquetBinaryOption(DateTime strikeDate, List<DateTime> observationDates, MarkitEquityUnderlying underlying, 
            List<double> coupons, List<double> barrierMoneyness, List<double> cliquetMoneyness, Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
            : this(new DateTime(), strikeDate, observationDates, underlying, coupons, barrierMoneyness, cliquetMoneyness, calendar, dayCounter, bdc)
        { }


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
        // METHODS -- PRICING ENGINE -- PASCAL
        // ************************************************************

        #region Pricing Engine

        private IPricingEngine PricingEngine()
        {
            return new MakeMCPathDepEngine<PseudoRandom>(process()).withSteps(nbSteps())
                                                                   .withSamples(nbSamples())
                                                                   .withAntitheticVariate(true)
                                                                   .withBrownianBridge(false)
                                                                   .withSeed(42)
                                                                   .value();
        }

        protected void setPricingEngine(IPricingEngine engine) {
            instrument().setPricingEngine(engine);
        }

        
        #endregion



        // ************************************************************
        // METHODS -- PRICING ENGINE -- MARC
        // ************************************************************


        #region Pricing Engine

        private IPricingEngine PricingEngine_Marc()
        {
            return new MakeMGenericInstrument<PseudoRandom>(process()).withAbsoluteTolerance(0.1)
                                                                   .withStepsPerYear(12)
                                                                   .withSeed(42)
                                                                   .value();
        }


        protected void setPricingEngine_Marc(IPricingEngine engine)
        {
            binaire().setPricingEngine(engine);
        }


        #endregion


        

        // ************************************************************
        // METHODS -- NPV
        // ************************************************************

        #region Computational methods for NPV

        public double NPV(DateTime pricingDate) {

            // Set the pricing date for QLNet and locally
            Settings.setEvaluationDate(pricingDate.ToDate());
            _valuationDate = pricingDate;

            setPricingEngine(PricingEngine());

            // Price
            return instrument().NPV();
        }


        public double NPV_Marc(DateTime pricingDate)
        {

            // Set the pricing date for QLNet and locally
            Settings.setEvaluationDate(pricingDate.ToDate());
            _valuationDate = pricingDate;

            setPricingEngine_Marc(PricingEngine_Marc());

            // Price
            return binaire().NPV();
        }


        #endregion








    }


}
