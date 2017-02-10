using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// external custom
using QLNet;

// internal custom
using QLyx.Equities;
using QLyx.DataIO;
using QLyx.DataIO.Markit;
using QLyx.Utilities;
using QLyx.Containers;

namespace Pascal.Valuation
{

    public class EquityIndexStrangleStrategy<T> : IEquityIndexStrangleStrategy where T : EquityIndex
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
        private myFrame EndOfDay(int referenceDBID)
        {
            IDtoken thisToken = GetToken(referenceDBID);
            return EndOfDay(thisToken);
        }

        private myFrame EndOfDay(IDtoken referenceToken)
        {
            //DateTime startDate = _optionTradeDates.LastOrDefault();
            //DateTime endDate = _optionTradeDates.FirstOrDefault();

            DateTime startDate = optionTradeDates().LastOrDefault();
            DateTime endDate = optionTradeDates().FirstOrDefault();

            HistoricalDataRequest myRequest = new HistoricalDataRequest(referenceToken, startDate, endDate);
            return _localDatabase.GetEODPrices(myRequest);
        }

        DatabaseHelper _localDatabase = new DatabaseHelper();


        // ID des différents elements
        private Dictionary<int, DBID> strangleMTM_Tokens = new Dictionary<int, DBID>() {{0, new DBID(70)},
                                                                                    {1, new DBID(71)},
                                                                                    {2, new DBID(72)},
                                                                                    {3, new DBID(73)},
                                                                                    {4, new DBID(74)},
                                                                                    {5, new DBID(75)},
                                                                                    {6, new DBID(76)},
                                                                                    {7, new DBID(77)},
                                                                                    {8, new DBID(78)},
                                                                                    {9, new DBID(79)}};

        
        private int _callStrikeID = 68; // SGPXESD1
        private int _putStrikeID = 69; // SGPXESD2 
        private int _underlyingID = 61; // SX5E

        #endregion


        // ************************************************************
        // PROPERTIES - FINANCIAL
        // ************************************************************

        #region Financial Properties

        //Number of strangles (of call/put pairs)
        protected int _numberStrangles = 10;

        //Spacing between strike of each strangle
        protected Period _spacing = new Period(1, TimeUnit.Days);

        // Calendar
        protected Calendar _calendar = new TARGET();

        // Calendar
        protected DayCounter _dayCounter = new Actual365Fixed();

        // Business Day Convention  
        protected BusinessDayConvention _bdc = BusinessDayConvention.Preceding;

        // Underlying type
        Type _underlyingType = typeof(T);

        // Valuation Date
        protected DateTime _valuationDate;

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
            if (_marketData == null) { _marketData = Markit_Equity_IV.Instance(MarkitEquityUnderlying.Eurostoxx); }
            return _marketData[valuationDate];
        }

        // Call options NPV (indexed on strike date)
        protected Dictionary<Guid, double> _call_NPV = new Dictionary<Guid, double>();

        // Put options NPV (indexed on strike date)
        protected Dictionary<Guid, double> _put_NPV = new Dictionary<Guid, double>();

        // Underlying time series
        protected myFrame _underlyingTimeSeries;
        protected double underlying(DateTime valuationDate) {
            if (_underlyingTimeSeries == null) { _underlyingTimeSeries = EndOfDay(_underlyingID); }
            return (double)_underlyingTimeSeries[valuationDate]["Close"];
        }

        #endregion


        // ************************************************************
        // ACCESSORS
        // ************************************************************

        #region Accessors
            
        // Underlying instance (lazy)
        protected T underlying() { return (T)Activator.CreateInstance(_underlyingType); }

        // Trade dates
        //protected List<DateTime> optionTradeDateTime() { return _callOptions.Keys.ToList(); }
        
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
        public EquityIndexStrangleStrategy() { }

        // No Date
        public EquityIndexStrangleStrategy(int numberStrangles, Period spacing, Calendar calendar)
            : this(new DateTime(), numberStrangles, spacing, calendar)
        {
        }

        // Full fledged
        public EquityIndexStrangleStrategy(DateTime valuationDate, int numberStrangles, Period spacing, Calendar calendar)
        {
            _valuationDate = valuationDate;
            _numberStrangles = numberStrangles;
            _spacing = spacing;
            _calendar = calendar;
        }

        #endregion


        // ************************************************************
        // METHODS - STRIKES COMPUTATION
        // ************************************************************

        #region Strike calculations


        private double callStrike(int k)
        {
            if ((k < 0) || (k >= _numberStrangles)) { throw new ArgumentException("", ""); }
            if (_callStrikes.Count() == 0 || _callStrikes == null) { SetCallStrikes(); }
            return _callStrikes[k];
        }


        private double putStrike(int k)
        {
            if ((k < 0) || (k >= _numberStrangles)) { throw new ArgumentException("", ""); }
            if (_putStrikes.Count() == 0 || _putStrikes == null) { SetPutStrikes(); }
            return _putStrikes[k];
        }

        private Dictionary<int, double> _callStrikes = new Dictionary<int, double>();
        private Dictionary<int, double> _putStrikes = new Dictionary<int, double>();


        private void SetCallStrikes()
        {
            int cnt = 0;
            myFrame data = EndOfDay(_callStrikeID);
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
            myFrame data = EndOfDay(_putStrikeID);
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

                DateTime expiry = _callOptions[id].exercise().dates().FirstOrDefault();
                double strikeMoneyness = _callOptions[id].strikeLevel / currentSpot;
                Settings.setEvaluationDate(valuationDate);
                _callOptions[id].setPricingEngine(GetEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
                _call_NPV[id] = _callOptions[id].NPV();
            }


            // *************************************
            // Puts options
            // *************************************
            foreach (Guid id in _putOptions.Keys)
            {

                DateTime expiry = _putOptions[id].exercise().dates().FirstOrDefault();
                double strikeMoneyness = _putOptions[id].strikeMoneyness;
                Settings.setEvaluationDate(valuationDate);
                _putOptions[id].setPricingEngine(GetEngine(_valuationDate, expiry, underlying(_valuationDate), strikeMoneyness));
                _put_NPV[id] = _putOptions[id].NPV();
            }
        }


        private IPricingEngine GetEngine(DateTime valuationDate, DateTime expiryDate, double currentSpot, double strikeMoneyness)
        {
            return new AnalyticEuropeanEngine(GetStochasticProcess(valuationDate, expiryDate, currentSpot, strikeMoneyness));
        }



        protected GeneralizedBlackScholesProcess GetStochasticProcess(DateTime valuationDate, DateTime expiryDate, double currentSpot, double strikeMoneyness)
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
            double volatility = marketData(valuationDate)[expiryDate][strikeMoneyness];
            var blackVol = new Handle<BlackVolTermStructure>(new BlackConstantVol(_valuationDate, _calendar, volatility, _dayCounter));

            // Return
            return new GeneralizedBlackScholesProcess(underlyingH, DivTermStructure, RateTermStructure, blackVol);

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
            OptionPosition optionPosition = new OptionPosition(Payoff(optionType, strikeLevel) as StrikedTypePayoff, Exercise(maturity));

            // Set internal properties:
            // Levels
            optionPosition.strikeLevel = strikeLevel;
            optionPosition.spotAtStrike = spotAtStrikeDate;
            // Dates
            optionPosition.expiryDate = maturity;
            optionPosition.tradeDate = tradeDate;

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
        // SUBCLASS (INTERNAL)
        // ************************************************************

        protected class OptionPosition {


            // Option
            public VanillaOption _option;


            // Dates
            public DateTime tradeDate;
            public DateTime expiryDate;


            // Levels
            public double strikeLevel;
            public double spotAtStrike;
            public double strikeMoneyness { get { return strikeLevel / spotAtStrike; } }
            public Exercise exercise() { return _option.exercise(); } 
            public Payoff payoff() { return _option.payoff(); }


            // Constructor
            public OptionPosition(StrikedTypePayoff payoff, Exercise exercise)
            {
                _option = new VanillaOption(payoff, exercise);
            }


            // Methods
            public void setPricingEngine(IPricingEngine engine) {
                _option.setPricingEngine(engine);
            }

            public double NPV()
            {
                Console.WriteLine("**************************************");
                Console.WriteLine("NPV for option traded : {0}", tradeDate);
                Console.WriteLine("**************************************");

                double prix_points_indice = _option.NPV();
                return prix_points_indice / spotAtStrike;
            }












        }
    }
}
