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

namespace Pascal.Valuation
{

    public class EquityIndexStrangleStrategy : IEquityIndexStrangleStrategy // where T : EquityIndex
    {


        // ************************************************************
        // PROPERTIES - INFRASTUCTURE
        // ************************************************************

        #region Infrastructure
            
        // Le référentiel de la database
        private ReferenceManager referentiel(){
            if (_referentiel == null) { _referentiel = ReferenceManager.Factory; }
            return _referentiel;
        }
        private ReferenceManager _referentiel = ReferenceManager.Factory;


        // Accès au référentiel de la database
        private IDtoken GetToken(int referenceDBID) {
            return _referentiel.Identify(new DBID(referenceDBID));
        }


        // Accès aux données de la base via le helper
        private myFrame EndOfDay(int referenceDBID, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            IDtoken thisToken = GetToken(referenceDBID);
            return EndOfDay(thisToken, startDate, endDate);
        }

        private myFrame EndOfDay(IDtoken referenceToken, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            if (startDate == DateTime.MinValue) { startDate = optionTradeDates().LastOrDefault().ToDateTime(); }
            if (endDate == DateTime.MinValue) { endDate = optionTradeDates().FirstOrDefault().ToDateTime(); }
       
            HistoricalDataRequest myRequest = new HistoricalDataRequest(referenceToken, startDate, endDate);
            return _localDatabase.GetEODPrices(myRequest);
        }

        // Helper pour la DB locale
        DatabaseHelper _localDatabase = new DatabaseHelper();


        // ID des différents elements
        private Dictionary<int, DBID> strangleMTM_Tokens = new Dictionary<int, DBID>();
  
        // Valeur MtM des différents strangles (contrepartie)
        private Dictionary<int, double> strangleMTM_CounterpartyPrices = new Dictionary<int, double>();
        private Dictionary<int, double> strangleMTM_LyxorPrices = new Dictionary<int, double>();
        private Dictionary<int, double> strangleMTM_thresholds = new Dictionary<int, double>();

        // Identification
        private DBID _callStrikeID; 
        private DBID _putStrikeID; 
        private DBID _underlyingID; 

        #endregion


        // ************************************************************
        // PROPERTIES - FINANCIAL
        // ************************************************************

        #region Financial Properties

        // Underlying
        protected MarkitEquityUnderlying _underlying;

        //Number of strangles (of call/put pairs)
        protected int _numberStrangles;

        //Spacing between strike of each strangle
        protected Period _spacing;

        // Calendar
        protected Calendar _calendar = new NullCalendar();

        // Calendar
        protected DayCounter _dayCounter = new Actual365Fixed();

        // Business Day Convention  
        protected BusinessDayConvention _bdc = BusinessDayConvention.Preceding;

        // Underlying type
        // Type _underlyingType = typeof(T);

        // Valuation Date
        protected DateTime _valuationDate;
        protected DateTime valuationDate {

            get {
                return _valuationDate;
            }

            set {
                _valuationDate = Adjust(value);
            }

        }

        // Option trade dates
        List<Date> _optionTradeDates;

        // Option expiry dates
        //List<Date> _optionExpiryDates;

        // Call options (indexed on strike date)
        // protected Dictionary<DateTime, VanillaOption> _callOptions = new Dictionary<DateTime, VanillaOption>();
        protected Dictionary<Guid, OptionPosition> _callOptions = new Dictionary<Guid, OptionPosition>();

        // Put options (indexed on strike date)
        // protected Dictionary<DateTime, VanillaOption> _putOptions = new Dictionary<DateTime, VanillaOption>();
        protected Dictionary<Guid, OptionPosition> _putOptions = new Dictionary<Guid, OptionPosition>();

        // Market Data (Markit)
        protected Markit_Equity_IV _marketData;
        protected MarkitSurface marketData(DateTime valuationDate) {
            if (_marketData == null) { _marketData = Markit_Equity_IV.Instance(_underlying); }
            return _marketData[valuationDate];
        }

        // Call options NPV (indexed on strike date)
        protected Dictionary<Guid, double> _call_NPV = new Dictionary<Guid, double>();

        // Put options NPV (indexed on strike date)
        protected Dictionary<Guid, double> _put_NPV = new Dictionary<Guid, double>();

        // Underlying time series
        protected myFrame _underlyingTimeSeries;
        protected double underlying(DateTime valuationDate) {
            if (_underlyingTimeSeries == null) { _underlyingTimeSeries = EndOfDay((int)_underlyingID); }
            return (double)_underlyingTimeSeries[valuationDate]["Close"];
        }

        #endregion


        // ************************************************************
        // ACCESSORS
        // ************************************************************

        #region Accessors
            
        // Underlying instance (lazy)
        // protected T underlying() { return (T)Activator.CreateInstance(_underlyingType); }

        // Trade dates
        // protected List<DateTime> optionTradeDateTime() { return _callOptions.Keys.ToList(); }
        
        protected List<Date> optionTradeDates()
        {
            if (_optionTradeDates == null) { SetTradeDates(); }
            return _optionTradeDates;
        }

        // Time unit and nb of periods relating to the spacing
        protected TimeUnit timeUnit() { return _spacing.units(); }
        protected int numberPeriod() { return _spacing.length(); }

        #endregion

        // ************************************************************
        // CONSTRUCTOR 
        // ************************************************************

        #region Constuctors
            
        // Generic
        //public EquityIndexStrangleStrategy() { }

        // No Date
        public EquityIndexStrangleStrategy(MarkitEquityUnderlying underlying, int numberStrangles, Period spacing, 
            Calendar calendar, DBID callStrikeDBID, DBID putStrikeDBID, DBID underlyingDBID, Dictionary<int, DBID> strangleMtM_DBID)
            : this(new DateTime(), underlying, numberStrangles, spacing, calendar, callStrikeDBID, putStrikeDBID, underlyingDBID, strangleMtM_DBID)
        { }

        // Full fledged
        public EquityIndexStrangleStrategy(DateTime valuationDate, MarkitEquityUnderlying underlying, int numberStrangles, 
            Period spacing, Calendar calendar, DBID callStrikeDBID, DBID putStrikeDBID, DBID underlyingDBID, Dictionary<int, DBID> strangleMtM_DBID)
        {

            // Set the date
            this.valuationDate = valuationDate;

            // Set the underlying
            _underlying = underlying;
            
            // Set the strategy properties
            _numberStrangles = numberStrangles;
            _spacing = spacing;
            _calendar = calendar;

            // Set the identification (to locate data in the DB)
            _callStrikeID = callStrikeDBID;
            _putStrikeID = putStrikeDBID;
            _underlyingID = underlyingDBID;
            strangleMTM_Tokens = strangleMtM_DBID;

        }

        #endregion


        // ************************************************************
        // METHODS - STRIKES COMPUTATION
        // ************************************************************

        #region Strike calculations


        private double callStrike(int k)
        {
            if ((k < 0) || (k >= _numberStrangles)) { throw new ArgumentException("CallStrikesMismatch", "Number of strikes does not match the number of options."); }
            if (_callStrikes.Count() == 0 || _callStrikes == null) { SetCallStrikes(); }
            return _callStrikes[k];
        }


        private double putStrike(int k)
        {
            if ((k < 0) || (k >= _numberStrangles)) { throw new ArgumentException("PutStrikesMismatch", "Number of strikes does not match the number of options."); }
            if (_putStrikes.Count() == 0 || _putStrikes == null) { SetPutStrikes(); }
            return _putStrikes[k];
        }

        private Dictionary<int, double> _callStrikes = new Dictionary<int, double>();
        private Dictionary<int, double> _putStrikes = new Dictionary<int, double>();


        private void SetCallStrikes()
        {
            int cnt = 0;
            myFrame data = EndOfDay((int)_callStrikeID);
            //if (data.data.Count() != _numberStrangles) { throw new ArgumentException("",""); }
            foreach (DateTime dt in _optionTradeDates) {
                double strike = (double)data[dt].data["Close"];
                _callStrikes[cnt] = strike;
                cnt += 1;
            }
        }


        private void SetPutStrikes()
        {
            int cnt = 0;
            myFrame data = EndOfDay((int)_putStrikeID);
            //if (data.data.Count() != _numberStrangles) { throw new ArgumentException("", ""); }
            foreach (DateTime dt in _optionTradeDates)
            {
                double strike = (double)data[dt].data["Close"];
                _putStrikes[cnt] = strike;
                cnt += 1;
            }
        }


        #endregion


        // ************************************************************
        // METHODS FOR SETTING UP THE PORTFOLIO OF OPTIONS
        // ************************************************************

        public void SetOptionPortfolio()
        {

            // Initial declarations
            double _callStrike = 0.0;
            double _putStrike = 0.0;
            double _spotAtStrikeDate = 0.0;
            DateTime _expiry;
            int k = 0;

            // GET SPOT AT STRIKE DATE
            // myFrame underlyingLevel = EndOfDay(_underlyingID);

            // Set each option in the portfolio
            foreach (DateTime dt in optionTradeDates()) {

                // COMMON INFOS
                _expiry = ExpiryDate(dt);
                _spotAtStrikeDate = underlying(dt);

                // SET CALL
                Guid call_id = Guid.NewGuid();
                _callStrike = callStrike(k);
                _callOptions[call_id] = _Option(Option.Type.Call, _callStrike, _spotAtStrikeDate, _expiry, dt);

                // SET PUT
                Guid put_id = Guid.NewGuid();
                _putStrike = putStrike(k);
                _putOptions[put_id] = _Option(Option.Type.Put, _putStrike, _spotAtStrikeDate, _expiry, dt);

                // ITERATE
                k++;

            }
        }


        public void _compute_NPV()
        {
            _compute_NPV(_valuationDate);
        }

        public void _compute_NPV(DateTime valuationDate)
        {

            if ((_callOptions.Count() == 0) || (_putOptions.Count() == 0)) { SetOptionPortfolio(); }

            Settings.setEvaluationDate(valuationDate);

            _call_NPV = new Dictionary<Guid, double>();
            _put_NPV = new Dictionary<Guid, double>();

            double currentSpot = underlying(valuationDate);


            // *************************************
            // Calls options
            // *************************************
            foreach (Guid id in _callOptions.Keys) {

                DateTime expiry = _callOptions[id].exercise().dates().LastOrDefault();
                double strikeMoneyness = _callOptions[id]._strikeLevel / currentSpot;
                // Settings.setEvaluationDate(valuationDate);
                //_callOptions[id].setPricingEngine(PricingEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
                _call_NPV[id] = _callOptions[id].NPV(valuationDate, PricingEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
            }


            // *************************************
            // Puts options
            // *************************************
            foreach (Guid id in _putOptions.Keys)
            {

                DateTime expiry = _putOptions[id].exercise().dates().LastOrDefault();
                double strikeMoneyness = _putOptions[id].strikeMoneyness;
                // Settings.setEvaluationDate(valuationDate);
                // _putOptions[id].setPricingEngine(PricingEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
                _put_NPV[id] = _putOptions[id].NPV(valuationDate, PricingEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
            }
        }


        private IPricingEngine PricingEngine(DateTime valuationDate, DateTime expiryDate, double currentSpot, double strikeMoneyness)
        {
            return new AnalyticEuropeanEngine(GetStochasticProcess(valuationDate, expiryDate, currentSpot, strikeMoneyness));
        }



        protected GeneralizedBlackScholesProcessTolerance GetStochasticProcess(DateTime valuationDate, DateTime expiryDate, double currentSpot, double strikeMoneyness)
        {
            // Sous-jacent : Spot Markit
            // Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(marketData(_valuationDate).impliedSpot));

            // Sous-jacent : Spot Bloomberg
            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(currentSpot));

            // Yield Term Structure
            Handle<YieldTermStructure> RateTermStructure = marketData(_valuationDate).riskFree_FwdCrv();
            // Handle<YieldTermStructure> RateTermStructure = marketData(valuationDate).riskFree_flat(valuationDate);
            // Handle<YieldTermStructure> RateTermStructure = new Handle<YieldTermStructure>(new FlatForward(valuationDate, 0.00, _dayCounter));

            // Dividend Yield
            var DivTermStructure = marketData(_valuationDate).dividend_FwdCrv();
            // var DivTermStructure = new Handle<YieldTermStructure>(new FlatForward(valuationDate, 0.00, _dayCounter));

            // Volatility ATM curve
            // BlackVarianceCurve blackVol = marketData_valuationDate).ATMF_Vol_TS(_valuationDate);

            // Volatility Single vol point (flat forward)
            // double volatility = marketData(valuationDate)[expiryDate][strikeMoneyness];
            // var blackVol = new Handle<BlackVolTermStructure>(new BlackConstantVol(_valuationDate, _calendar, volatility, _dayCounter));

            // Volatility Surface
            
            Matrix volMat = marketData(_valuationDate).VolMatrix();
            List<Date> dates = marketData(_valuationDate).Dates();
            Calendar cal = marketData(_valuationDate).calendar();
            List<double> strikes = marketData(_valuationDate).Strikes(currentSpot); // double en argument correspondant au spot de pricing

            BlackVarianceSurface mySurface = new BlackVarianceSurface(_valuationDate,
                                                                    cal,
                                                                    dates,
                                                                    strikes,
                                                                    volMat,
                                                                    marketData(_valuationDate).dayCounter());

            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(mySurface);
            GeneralizedBlackScholesProcessTolerance bsmProcessVolSurface = new GeneralizedBlackScholesProcessTolerance(underlyingH, DivTermStructure, RateTermStructure, mySurfaceH);
            
            
            // Return
            //return new GeneralizedBlackScholesProcess(underlyingH, DivTermStructure, RateTermStructure, blackVol);
            return bsmProcessVolSurface;

        }

        // ************************************************************
        // METHODS FOR SETTING CALENDARS
        // ************************************************************

        #region Trade dates

        public void SetTradeDates(DateTime valuationDate)
        {

            // Initial setup (avoid null exception)
            if (_optionTradeDates == null) { _optionTradeDates = new List<Date>(); }

            // Set first date and add to the list of dates
            Date previousDate = new Date(valuationDate);
            _optionTradeDates.Add(previousDate);

            // Back fill the option sequence from valuation date (excluded) up to the max number allowed
            Date currentDate = previousDate;
            Period step = new Period(numberPeriod(), timeUnit());
            for (int i = 1; i < _numberStrangles; i++)
            {
                // Compute the previous date by taking 1 step
                currentDate = Adjust(previousDate - step);

                // Add to the list
                _optionTradeDates.Add(currentDate);

                // Iterate
                previousDate = currentDate;
            }
        }


        public void SetTradeDates()
        {
            if (_valuationDate == DateTime.MinValue) {
                throw new ArgumentException("NoValuationDate", "No valuation date give. Unable to build options' list.");
            }

            SetTradeDates(_valuationDate);

        }


        protected DateTime Adjust(DateTime dateTime)
        {
            Date d = _calendar.adjust(dateTime.ToDate(), _bdc);
            return d.ToDateTime();
        }


        protected Date Adjust(Date date)
        {
            return _calendar.adjust(date, _bdc);
        }


        #endregion

        // ************************************************************
        // METHODS FOR SETTING OPTION CARACTERISTICS
        // ************************************************************

        #region Option Caracteristics

        public virtual DateTime ExpiryDate(DateTime strikeDate)
        {
            Period optionLifeSpan = new Period(_numberStrangles, timeUnit());
            Date tmp = _calendar.advance(new Date(strikeDate), optionLifeSpan, _bdc);
            return tmp.ToDateTime();
        }
        

        private OptionPosition _Option(Option.Type optionType, double strikeLevel, double spotAtStrikeDate, DateTime maturity, DateTime tradeDate)
        {

            // Instanciate
            OptionPosition optionPosition = new OptionPosition(Payoff(optionType, strikeLevel) as StrikedTypePayoff, Exercise(maturity), tradeDate, spotAtStrikeDate, _dayCounter);

            // Set internal properties:
            // Levels
            optionPosition._strikeLevel = strikeLevel;
            optionPosition._spotAtStrike = spotAtStrikeDate;

            // Dates
            // optionPosition.expiryDate = maturity;
            // optionPosition.tradeDate = tradeDate;

            // Return
            return optionPosition;
        }


        private Payoff Payoff(Option.Type optionType, double strike)
        {
            return new PlainVanillaPayoff(optionType, strike);
        }


        private Exercise Exercise(DateTime maturity)
        {
            return new EuropeanExercise(new Date(maturity));
        }
        #endregion


        // ************************************************************
        // METHODS FOR COMPARAISON AND OUTPUT
        // ************************************************************

        private void Strangle_CounterpartyPrices(DateTime valuationDate)
        {
            // Loop through the various IDtoken to fetch SG's price
            foreach (int k in strangleMTM_Tokens.Keys) {

                myFrame MtM_strangle = EndOfDay(strangleMTM_Tokens[k], valuationDate, valuationDate);
                double valueMtM = (double)MtM_strangle[valuationDate]["Close"];
                strangleMTM_CounterpartyPrices[k] = valueMtM;
            }
        }
        

        private void Strangle_LyxorPrices(DateTime valuationDate){
            for (int k=0; k < _numberStrangles; k++) {

                // Set the prices for the strangle
                double px_strangle = _putOptions.ElementAt(k).Value.price() + _callOptions.ElementAt(k).Value.price();
                strangleMTM_LyxorPrices[k] = px_strangle;

                // Set the threshold
                double threshold = _putOptions.ElementAt(k).Value.threshold() + _callOptions.ElementAt(k).Value.threshold();
                strangleMTM_thresholds[k] = threshold;

            }
        }


        public void Results() {

            _compute_NPV();

            Strangle_CounterpartyPrices(_valuationDate);
            Strangle_LyxorPrices(_valuationDate);

            Console.WriteLine();
            Console.WriteLine("*******************************************************************************");
            Console.WriteLine("Pricing Date : {0}", _valuationDate.ToDate());
            Console.WriteLine("Item \t Lyxor \t SG  \t Gap \t Threshold \t OK/KO");
            Console.WriteLine();

            for (int k = 0; k < _numberStrangles; k++){

                double lyx_bps = Math.Round(10000 * strangleMTM_LyxorPrices[k], 2);
                double sg_bps = Math.Round(100 * strangleMTM_CounterpartyPrices[k],2);
                double gap_bps = Math.Round(Math.Abs(lyx_bps-sg_bps), 2);
                double th_bps = Math.Round(10000 * strangleMTM_thresholds[k],2);

                string OK_KO = "NOT OK";
                if (Math.Abs(sg_bps-lyx_bps) <= th_bps) { OK_KO = "OK"; }

                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t\t{5}", k+1, lyx_bps, sg_bps, gap_bps, th_bps, OK_KO);
                //Console.WriteLine("---------------------------------------------------");

            }

            Console.WriteLine("*******************************************************************************");

        }

        

        // ************************************************************
        // SUBCLASS (INTERNAL)
        // ************************************************************

        #region Sub Class : OptionPosition

        protected class OptionPosition {


            // Option
            public VanillaOption _option;

            // Valuation
            public double _price_points;
            public double _price_relative;
            public double _delta;
            public double _vega;

            // Calendar and Day Counters
            public DayCounter _dayCounter = new Actual365Fixed();
            
            // Dates
            public DateTime _tradeDate;
            public DateTime _expiryDate;
            public DateTime _pricingDate;


            // Levels
            public double _strikeLevel;
            public double _spotAtStrike;
            public double strikeMoneyness { get { return _strikeLevel / _spotAtStrike; } }
            public Exercise exercise() { return _option.exercise(); } 
            public Payoff payoff() { return _option.payoff(); }
            public double TTM() { return _dayCounter.yearFraction(_pricingDate, _expiryDate); }

            public double price() { return _price_relative; }


            // Threshold / Pricing policy
            double base_threshold = 0.0050;
            double delta_threshold = 0.0030;
            double vega_threshold = 0.0300;


            // Constructor
            public OptionPosition(StrikedTypePayoff payoff, Exercise exercise, DateTime TradeDate, double SpotAtStrikeDate, DayCounter dayCounter)
            {
                _option = new VanillaOption(payoff, exercise);
                _dayCounter = dayCounter;
                _expiryDate = exercise.lastDate();
                _spotAtStrike = SpotAtStrikeDate;
                _tradeDate = TradeDate;
            }


            // Methods
            public double NPV(DateTime pricingDate, IPricingEngine engine)
            {
                // Set the pricing date
                _pricingDate = pricingDate;
                Settings.setEvaluationDate(pricingDate);

                // Set the pricing engine
                _option.setPricingEngine(engine);

                _price_points = _option.NPV();
                _price_relative = _price_points / _spotAtStrike;

                return _price_relative;
            }


            // Threshold / Pricing Policy
            public double threshold() {

                double basic = Basic_Threshold();
                double delta = Delta_threshold();
                double vega = Vega_threshold();

                return (basic + delta + vega);
            }


            private double Basic_Threshold()
            {
                return Math.Sqrt(TTM()) * base_threshold;
            }


            private double Delta_threshold()
            {
                return _delta * TTM() * delta_threshold;
            }

            private double Vega_threshold()
            {
                return _vega * vega_threshold;
            }

            #endregion

            


        }
    }
}
