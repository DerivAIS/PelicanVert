using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using QLyx.InterestRates;

namespace QLyx.Equities
{
    class EquityIndexOption : IEquity
    {



        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************

        // string _referenceDBtable = "EQUITYINDEXOPTION";
        static public myDB _referenceDBtable = myDB.Equity;


        // ************************************************************
        // CLASS PROPERTIES - FINANCIAL
        // ************************************************************

        #region Properties (financial)
        // UNDERLYING (MUST BE OF TYPE EQUITY INDEX)
        #region Equity Underlying

        protected EquityIndex _underlying;
        public EquityIndex underlying
        {
            get { return _underlying; }
            protected set
            {
                this._underlying = value;
            }
        }


        #endregion


        // STRIKE
        #region Strike

        protected double _strike;
        public double strike
        {
            get { return _strike; }
            protected set
            {
                if (value < 0.0) { throw new System.ArgumentException("_currency", "Negative strike not allowed for Equity Option"); }
                this._strike = value;
            }
        }

        #endregion


        // UNDERLYING INDEX LEVEL AT STRIKE DATE
        #region Underlying Strike INDEX Level

        protected double _underlyingValueAtStrike;
        public double underlyingValueAtStrike
        {
            get { return _underlyingValueAtStrike; }
            protected set
            {
                this._underlyingValueAtStrike = value;
            }
        }

        #endregion


        // OPTION TYPE (PUT / CALL)
        #region Option Type

        protected string _optionType;
        public string optionType
        {
            get { return _optionType; }
            protected set
            {
                this._optionType = value;
            }
        }

        #endregion


        // RATE CURVE

        /*/
        #region Rate Curve

        protected RateCurve _rateCurve;
        public RateCurve rateCurve
        {
            get { return _rateCurve; }
            protected set
            {
                this._rateCurve = value;
            }
        }

        #endregion
        */

        // IMPLIED VOLATILITY
        #region Implied Volatility

        protected double _impliedVolatility;
        public double impliedVolatility
        {
            get { return _impliedVolatility; }
            protected set
            {
                this._impliedVolatility = value;
            }
        }

        #endregion


        // DATES (MATURITY, PRICING, STRIKE)
        #region Dates


        // MATURITY
        #region Maturity

        protected DateTime _maturity;
        public DateTime maturity
        {
            get { return _maturity; }
            protected set
            {
                this._maturity = value;
            }
        }

        #endregion


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


        // STRIKE DATE
        #region Strike Date

        protected DateTime _strikeDate;
        public DateTime strikeDate
        {
            get { return _strikeDate; }
            protected set
            {
                this._strikeDate = value;
            }
        }

        #endregion

        #endregion


        // CALENDAR
        #region Calendar

        protected string _calendar;
        public string calendar
        {
            get { return _calendar; }
            protected set
            {
                this._calendar = value;
            }
        }


        #endregion


        // CURRENCY
        #region CURRENCY

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


        // EXCHANGE
        #region EXCHANGE

        protected string _exchange;
        public string exchange
        {
            get { return _exchange; }
            protected set
            {
                this._exchange = value;
            }
        }


        #endregion


        #endregion



        // ************************************************************
        // CLASS PROPERTIES - INFRASTRUCTURE
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


        // CACHE MANAGER
        #region Cache

        protected CacheManager _cache;
        public CacheManager cache
        {
            get { return _cache; }
            protected set
            {
                this._cache = value;
            }
        }


        #endregion


        #endregion


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors


        // Constructor 1 : Generic
        public EquityIndexOption() { }


        // Constructor 2 : Identified only from DBID
        public EquityIndexOption(string argDBID)
            : base()
        {
            IDtoken myDBID = TokenFactory.New(argDBID);
            this.DBID = myDBID;
        }




        #endregion




        // ************************************************************
        // METHODS TO ACCESS HISTORICAL PRICE
        // ************************************************************

        // @TODO : to be implemented
        public double HistoricalPrice(DateTime argDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }

        // @TODO : to be implemented
        public double HistoricalPrice(DateTime argStartDate, DateTime argEndDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }

        // @TODO : to be implemented
        public double HistoricalBidAskPrices(DateTime argDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }

        // @TODO : to be implemented
        public double HistoricalBidAskPrices(DateTime argStartDate, DateTime argEndDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }

        // @TODO : to be implemented
        public double HistoricalIntervalPrices(DateTime argDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }

        // @TODO : to be implemented
        public double HistoricalIntervalPrices(DateTime argStartDate, DateTime argEndDate)
        {
            // Request to Market Data Manager
            // NOT IMPLEMENTED YET

            return 1.0;

        }


    }
}

