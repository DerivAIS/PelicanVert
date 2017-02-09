using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using QLyx.Containers;

using QLNet;

namespace QLyx.Equities
{

    public class EquityIndex : IEquityIndex
    {


        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************

        static public myDB _referenceDBtable = myDB.Equity;



        // ************************************************************
        // DATA READER
        // ************************************************************

        private DataReader myDataReader = new DataReader();



        // ************************************************************
        // HISTORICAL DATA AND ACCESSORS
        // ************************************************************

        #region Historical data

        protected myFrame _historicalData;
        public myFrame historicalData
        {
            get
            {
                if (_historicalData == null) { this.setHistoricalData(); }
                return _historicalData;
            }

            protected set
            {
                _historicalData = value;
            }
        }


        private void setHistoricalData()
        {
            _historicalData = myDataReader.EndOfDay(DBID);
        }

        #endregion



        // ************************************************************
        // INDEX COMPOSITION
        // ************************************************************


        // COMPOSITION
        #region INDEX Composition

        // Dict of : KEY -> DBID (string), VALUE -> Equity object

        protected Dictionary<string, Equity> _indexComposition;
        public Dictionary<string, Equity> indexComposition
        {
            get { return _indexComposition; }
            protected set
            {
                this._indexComposition = value;
            }
        }

        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES - FINANCIAL - DIVIDEND
        // ************************************************************

        #region Property -- Dividend


        // DIVIDEND YIELD CURVE -- HISTORICAL
        #region Historical Dividend YIELD Curve

        protected DividendCurve _historicalDividendYield;
        public DividendCurve historicalDividendYield
        {
            get { return _historicalDividendYield; }
            protected set
            {
                this._historicalDividendYield = value;
            }
        }

        #endregion


        // DIVIDEND POINTS CURVE -- HISTORICAL
        #region Dividend POINTS Curve

        protected DividendCurve _historicalDividendPoints;
        public DividendCurve historicalDividendPoints
        {
            get { return _historicalDividendPoints; }
            protected set
            {
                this._historicalDividendPoints = value;
            }
        }

        #endregion


        // DIVIDEND YIELD CURVE -- FORECAST
        #region Historical Dividend YIELD Curve

        protected DividendCurve _forecastDividendYield;
        public DividendCurve forecastDividendYield
        {
            get { return _forecastDividendYield; }
            protected set
            {
                this._forecastDividendYield = value;
            }
        }

        #endregion


        // DIVIDEND POINTS CURVE -- FORECAST
        #region Dividend POINTS Curve

        protected DividendCurve _forecastDividendPoints;
        public DividendCurve forecastDividendPoints
        {
            get { return _forecastDividendPoints; }
            protected set
            {
                this._forecastDividendPoints = value;
            }
        }

        #endregion



        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES - CONTEXTUAL
        // ************************************************************


        #region Properties (contextual)

        // PRICING DATE
        #region Pricing Date

        protected DateTime _pricingDate;
        public DateTime pricingDate
        {
            get { return _pricingDate; }
            protected set
            {
                this._pricingDate = value;
            }
        }

        #endregion


        // CALENDAR
        #region Calendar

        protected Calendar _calendar;
        public Calendar calendar
        {
            get { return _calendar; }
            protected set
            {
                this._calendar = value;
            }
        }


        #endregion


        // CURRENCY
        #region Currency

        protected string _currencyName;
        public string currencyName
        {
            get { return _currencyName; }
            protected set
            {
                this._currencyName = value;
            }
        }

       
        #endregion


        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES - INFRASTRUCTURE
        // ************************************************************

        #region Properties (infrastructure)

        // DBID - ID TOKEN
        #region DBID

        protected IDtoken _DBID;
        public IDtoken DBID
        {
            get { return _DBID; }
            protected set
            {
                this._DBID = value;
            }
        }


        #endregion


  
        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors


        // Constructor 1 : Generic
        public EquityIndex() { }


        // Constructor 2 : Identified only from DBID (int)
        public EquityIndex(int argDBID)
            : base()
        {
            IDtoken myDBID = TokenFactory.New(argDBID);
            this.DBID = myDBID;
        }

        // Constructor 3 : Identified only from IDToken
        public EquityIndex(IDtoken idToken)
            : base()
        {
            this.DBID = idToken;
        }

        // Constructor 4 : Identified only from DBID (int) & Date
        public EquityIndex(int argDBID, DateTime pricingDate)
            : base()
        {
            IDtoken myDBID = TokenFactory.New(argDBID);
            this.pricingDate = pricingDate;
            this.DBID = myDBID;
        }

        // Constructor 5 : Identified only from IDToken & Date
        public EquityIndex(IDtoken idToken, DateTime pricingDate)
            : base()
        {
            this.pricingDate = pricingDate;
            this.DBID = idToken;
        }


        #endregion



        // ************************************************************
        // METHODS TO ACCESS HISTORICAL SINGLE PRICE POINTS 
        // ************************************************************


        // Accessing Bid, Ask, Mid
        #region Methods for : Bid / Ask / Mid

        public Double? Bid(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Bid"];
        }

        public Double? Ask(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Ask"];
        }

        public Double? Mid(DateTime argDate)
        {
            myElement elem = historicalData[argDate];

            Double? ask = elem["Ask"];
            Double? bid = elem["Bid"];
            Double? last = elem["Last"];

            if (ask != null && bid != null)
            {
                return (ask + bid) * 0.5;
            }

            else if (ask == null && bid == null && last == null)
            {
                return ask;
            }

            else
            {
                if (last != null) { return last; }
                else if (bid != null) { return bid; }
                else { return ask; }

            }
        }

        #endregion



        // Accessing OHLC, Volume, Adjusted Close
        #region Methods for : Open / High / Low / Close

        public Double? Open(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Open"];
        }

        public Double? High(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["High"];
        }

        public Double? Low(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Low"];
        }

        public Double? Close(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Close"];
        }

        public Double? AdjustedClose(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["AdjustedClose"];
        }

        public Double? Volume(DateTime argDate)
        {
            myElement elem = historicalData[argDate];
            return elem["Volume"];
        }

        #endregion



        // Robust method to get a Price
        #region 

        public Double? Price(DateTime argDate)
        {
            myElement elem = historicalData[argDate];

            Double? mid = this.Mid(argDate);
            Double? last = this.Mid(argDate);

            
            if (last != null)
            {
                return last;
            }

            else if (mid != null)
            {
                return mid;
            }

            else
            {
                return null;
            }
        }

        #endregion



    }



    // ************************************************************
    // DERIVED CLASSES
    // ************************************************************

    public class SX5E : EquityIndex 
    { 
        
        // Constructor 1 : Generic
        public SX5E() 
            : base(TokenFactory.New(Bloomberg: "SX5E INDEX")) 
        { }


        // Constructor 2 : Generic with date
        public SX5E(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5E INDEX"))
        { }


    }

    public class SX5T : EquityIndex
    {

        // Constructor 1 : Generic
        public SX5T()
            : base(TokenFactory.New(Bloomberg: "SX5T INDEX"))
        { }


        // Constructor 2 : Generic with date
        public SX5T(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5T INDEX"))
        { }


    }

    public class SX5EWGT : EquityIndex
    {

        // Constructor 1 : Generic
        public SX5EWGT()
            : base(TokenFactory.New(Bloomberg: "SX5EWGT INDEX"))
        { }


        // Constructor 2 : Generic with date
        public SX5EWGT(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5EWGT INDEX"))
        { }


    }

    public class SX5EFETR : EquityIndex
    {

        // Constructor 1 : Generic
        public SX5EFETR()
            : base(TokenFactory.New(Bloomberg: "SX5EFETR INDEX"))
        { }


        // Constructor 2 : Generic with date
        public SX5EFETR(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5EFETR INDEX"))
        { }


    }

    public class SX5EFEER : EquityIndex
    {

        // Constructor 1 : Generic
        public SX5EFEER()
            : base(TokenFactory.New(Bloomberg: "SX5EFEER INDEX"))
        { }


        // Constructor 2 : Generic with date
        public SX5EFEER(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5EFEER INDEX"))
        { }


    }

    public class SX5ED : EquityIndex
    {

        // Constructor 1 : Generic
        public SX5ED()
            : base(TokenFactory.New(Bloomberg: "SX5ED INDEX"))
        { }


        // Constructor 2 : Generic with date
        public SX5ED(DateTime pricingDate)
            : base(TokenFactory.New(Bloomberg: "SX5ED INDEX"))
        { }


    }


}

