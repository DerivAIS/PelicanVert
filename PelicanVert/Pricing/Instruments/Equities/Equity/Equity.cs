using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;

namespace QLyx.Equities
{
    public class Equity : IEquity
    {


        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************

        // string _referenceDBtable = "EQUITY";
        static public myDB _referenceDBtable = myDB.Equity;


        // ************************************************************
        // INSTANCE PROPERTIES - FINANCIAL
        // ************************************************************


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
        public Equity() { }


        // Constructor 2 : Identified only from DBID (and Table sourced locally)
        public Equity(string argDBID)
            : base()
        {
            IDtoken myDBID = TokenFactory.New(argDBID);
            this.DBID = myDBID;

        }


        #endregion




        // ************************************************************
        // METHODS TO ACCESS HISTORICAL PRICES SERIES
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

