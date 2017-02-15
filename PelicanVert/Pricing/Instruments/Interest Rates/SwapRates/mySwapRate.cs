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
    public class mySwapRate : myRate
    {


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        // Moved up to myRate class (base class)
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
        #region Swap Currency

        protected string _SwapCurrency;
        public string SwapCurrency
        {
            get { return _SwapCurrency; }
            protected set { _SwapCurrency = value; }
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
        #region SwapPeriod

        protected Period _SwapPeriod;
        public Period SwapPeriod
        {
            get { return _SwapPeriod; }
            protected set { _SwapPeriod = value; }
        }


        #endregion


        // FORWARD START (DAYS)
        #region Fixing Days / Forward Start Days

        protected Period _ForwardStart;
        public Period ForwardStart
        {
            get { return _ForwardStart; }
            protected set { this._ForwardStart = value; }
        }


        #endregion


        // BUSINESS DAY CONVENTION
        #region Business Day Convention

        protected BusinessDayConvention _SwapFixedLegBDC;
        public BusinessDayConvention SwapFixedLegBDC
        {
            get { return _SwapFixedLegBDC; }
            protected set { this._SwapFixedLegBDC = value; }
        }


        #endregion


        // DAY COUNT CONVENTION
        #region Day Count Convention

        protected DayCounter _SwapFixedLegDayCount;
        public DayCounter SwapFixedLegDayCount
        {
            get { return _SwapFixedLegDayCount; }
            protected set { this._SwapFixedLegDayCount = value; }
        }


        #endregion


        // CALENDAR 
        #region Calendar

        protected Calendar _SwapCalendar = new TARGET();
        public Calendar SwapCalendar
        {
            get { return _SwapCalendar; }
        }

        #endregion


        // FREQUENCY
        #region Swap Rate Fixed Leg Frequency

        protected Frequency _SwapFixedLegFrequency;
        public Frequency SwapFixedLegFrequency
        {
            get { return _SwapFixedLegFrequency; }
            protected set { _SwapFixedLegFrequency = value; }
        }

        #endregion


        // SWAP FLOATING LEG INDEX (IBOR)
        #region Swap Rate Ref Index

        protected IborIndex _SwapFloatingLegIndex;
        public IborIndex SwapFloatingLegIndex
        {
            get { return _SwapFloatingLegIndex; }
            protected set { _SwapFloatingLegIndex = value; }
        }

        #endregion






        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public mySwapRate() { }


        // Constructor 2 : Normal usage
        /*
        public mySwapRate(DateTime PricingDate, string Currency, IDtoken argDBID)
        {
            this.PricingDate = PricingDate;
            this.Currency = Currency;
            this._DBID = argDBID;
        }

        */


        // Constructor 3 : Full fledged
        public mySwapRate(DateTime PricingDate_,
                            Frequency Frequency_,
                            Period Period_,
                            IDtoken argDBID_,
                            Period ForwardStart_,
                            BusinessDayConvention SwapFixedLegBDC_,
                            DayCounter SwapFixedLegDayCounter_,
                            IborIndex SwapFloatingLegIndex_,
                            string SwapCurrency_):base(PricingDate_: PricingDate_, Currency_: SwapCurrency_, _IdToken: argDBID_)
        {
            //this.PricingDate = PricingDate_;          // moved up to base class (myRate)
            this.SwapFixedLegFrequency = Frequency_;
            //this.DBID = argDBID_;                     // moved up to base class (myRate)
            this.ForwardStart = ForwardStart_;
            this.SwapFixedLegBDC = SwapFixedLegBDC_;
            this.SwapFixedLegDayCount = SwapFixedLegDayCounter_;
            this.SwapFloatingLegIndex = SwapFloatingLegIndex_;
            //this.SwapCurrency = SwapCurrency_;        // moved up to base class (myRate)
            this.SwapPeriod = Period_;

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

            Double? rate = base.Mid(argPricingdate); // changed to nullable type @TODO insert try/catch in rate curve class
            Quote rateQuote = new SimpleQuote(rate);

            // Return rate helper
            return new SwapRateHelper(new Handle<Quote>(rateQuote),  //Handle<Quote>(rateQuote)
                                        this.SwapPeriod,
                                        this.SwapCalendar,
                                        this.SwapFixedLegFrequency,
                                        this.SwapFixedLegBDC,
                                        this.SwapFixedLegDayCount,
                                        this.SwapFloatingLegIndex,
                                        new Handle<Quote>(),
                                        this.ForwardStart);
        }


        #endregion



    }
}
