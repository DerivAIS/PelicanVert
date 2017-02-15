using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom packages
using QLNet;

// Internal custom packages
using QLyx.DataIO;



namespace QLyx.InterestRates
{
    public abstract class myDepositRate : myRate
    {


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        // Attention : Moved up to myRate class !!!!!!!!!!!!!!!!!!!!!
        /*
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
        */


        // PERIOD
        #region DepoPeriod

        protected Period _DepoPeriod;
        public Period DepoPeriod
        {
            get { return _DepoPeriod; }
            protected set { _DepoPeriod = value; }
        }


        #endregion


        // FIXING DAYS
        #region Fixing Days

        protected int _DepoFixingDays;
        public int DepoFixingDays
        {
            get { return _DepoFixingDays; }
            protected set { this._DepoFixingDays = value; }
        }


        #endregion


        // BUSINESS DAY CONVENTION
        #region Business Day Convention

        protected BusinessDayConvention _DepoBusDayConv;
        public BusinessDayConvention DepoBusDayConv
        {
            get { return _DepoBusDayConv; }
            protected set { this._DepoBusDayConv = value; }
        }



        #endregion


        // DAY COUNT CONVENTION
        #region Day Count Convention

        protected DayCounter _DepoDayCounter;
        public DayCounter DepoDayCounter
        {
            get { return _DepoDayCounter; }
            protected set { this._DepoDayCounter = value; }
        }


        #endregion


        // CALENDAR
        #region Calendar

        protected Calendar _DepoCalendar = new TARGET();
        public Calendar DepoCalendar
        {
            get { return _DepoCalendar; }
        }

        #endregion


        // END OF MONTH
        #region End of Month (Boolean)

        protected bool _EndOfMonth = true;
        public bool EndOfMonth
        {
            get { return _EndOfMonth; }
            protected set { _EndOfMonth = value; }
        }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public myDepositRate() { }


        // Constructor 2 : Full fledged
        public myDepositRate(DateTime PricingDate_,
                            Period DepoPeriod_,
                            IDtoken argDBID_,
                            int DepoFixingDays_,
                            BusinessDayConvention DepoBusDayConv_,
                            DayCounter DepoDayCounter_,
                            string DepoCurrency_)
            : base(PricingDate_: PricingDate_, Currency_: DepoCurrency_, _IdToken: argDBID_)
        {
            //this.PricingDate = PricingDate_;      // moved up to base class (myRate)
            this.DepoPeriod = DepoPeriod_;
            //this.DBID = argDBID_;                 // moved up to base class (myRate)
            this.DepoFixingDays = DepoFixingDays_;
            this.DepoBusDayConv = DepoBusDayConv_;
            this.DepoDayCounter = DepoDayCounter_;
            //this.DepoCurrency = DepoCurrency_;    // moved up to base class (myRate)

        }


        #endregion




        // ************************************************************
        // METHODS
        // ************************************************************

        #region Methods

        public RateHelper getHelper()
        {
            return this.getHelper(this.PricingDate);
        }
        
        public RateHelper getHelper(DateTime argPricingdate)
        {

            // Extract the quote
            Double? rate = base.Mid(argPricingdate); // changed to nullable type @TODO insert try/catch in rate curve class
            if (rate == null) 
            {
                Console.WriteLine("Interest Rate Helper: Invalid Quote.");
                throw new System.ArgumentException("InterestRateQuoteException", "Invalid quote provided to RateHelper."); 
            }

            Quote rateQuote = new SimpleQuote(rate);

            // Return rate helper
            return new DepositRateHelper(new Handle<Quote>(rateQuote), 
                                            this.DepoPeriod,
                                            this.DepoFixingDays,
                                            this.DepoCalendar,
                                            BusinessDayConvention.ModifiedFollowing,
                                            this.EndOfMonth,
                                            this.DepoDayCounter);
        }

        #endregion




    }
}
