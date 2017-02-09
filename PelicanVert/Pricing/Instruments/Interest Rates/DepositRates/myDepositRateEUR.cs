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


    class myDepositRateEUR : myDepositRate // la super classe est vide
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        // DATABASE TABLE
        // static public string _referenceDBtable = "INTERESTRATE";
        static public myDB _referenceDBtable = myDB.InterestRate;


        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************




        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myDepositRateEUR() { }


        // Constructor 2 : Full fledged

        public myDepositRateEUR(DateTime PricingDate_,
                                Period Period_,
                                IDtoken argDBID_,
                                int FixingDays_,
                                BusinessDayConvention BDC_,
                                DayCounter DayCounter_)


            : base(
                        PricingDate_: PricingDate_,
                        DepoPeriod_: Period_,
                        argDBID_: argDBID_,
                        DepoFixingDays_: FixingDays_,
                        DepoBusDayConv_: BDC_,
                        DepoDayCounter_: DayCounter_,
                        DepoCurrency_: "EUR"
                ) { }



        #endregion


    }



    // ************************************************************
    // CLASS WRAPPERS (e.g. EONIA, TN, EURIBOR1M, etc.)
    // ************************************************************

    #region

    class myEONIA : myDepositRateEUR
    {
        public myEONIA()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EONIA Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }

        public myEONIA(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EONIA Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }
    }


    class myTomorrowNext : myDepositRateEUR
    {
        public myTomorrowNext()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EUDR2T Index"), FixingDays_: 1, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }

        public myTomorrowNext(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EUDR2T Index"), FixingDays_: 1, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }
    }


    class myEuribor1W : myDepositRateEUR
    {
        public myEuribor1W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUR001W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myEuribor1W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUR001W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    class myEuribor2W : myDepositRateEUR
    {
        public myEuribor2W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUR002W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myEuribor2W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUR002W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    class myEuribor1M : myDepositRateEUR
    {
        public myEuribor1M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR001M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor1M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR001M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    class myEuribor2M : myDepositRateEUR
    {
        public myEuribor2M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR002M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor2M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR002M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    class myEuribor3M : myDepositRateEUR
    {
        public myEuribor3M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR003M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor3M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR003M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    class myEuribor6M : myDepositRateEUR
    {
        public myEuribor6M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR006M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor6M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR006M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    class myEuribor9M : myDepositRateEUR
    {
        public myEuribor9M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR009M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor9M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR009M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    class myEuribor12M : myDepositRateEUR
    {
        public myEuribor12M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(12, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR012M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myEuribor12M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(12, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUR012M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }




    #endregion

}


