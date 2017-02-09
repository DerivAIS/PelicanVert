using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.Containers;
using QLyx.DataIO;

namespace QLyx.InterestRates
{
    public abstract class myRate
    {


        // ************************************************************
        // DATA READER
        // ************************************************************

        // Data Reader
        private DataReader myDataReader = new DataReader();



        // ************************************************************
        // BASIC RATE DATA
        // ************************************************************

        // PRICING DATE
        #region Pricing Date

        protected DateTime _PricingDate = new DateTime();
        public DateTime PricingDate
        {
            get { return _PricingDate; }
            protected set { _PricingDate = value; }
        }

        #endregion


        // CURRENCY
        #region Currency

        protected string _DepoCurrency;
        public string DepoCurrency
        {
            get { return _DepoCurrency; }
            protected set { _DepoCurrency = value; }
        }

        #endregion


        // DBID -> ID TOKEN
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
        


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public myRate() { }


        // Constructor 2 : Common usage
        public myRate(DateTime PricingDate_, string Currency_, IDtoken _IdToken) 
        {
            this.PricingDate = PricingDate_;
            this.DBID = _IdToken;
            this.DepoCurrency = Currency_;
        }

        #endregion




        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public myElement this[DateTime i]
        {
            get { return historicalData[i]; }
        }

        #endregion




    }
}
