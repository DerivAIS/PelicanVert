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


    public class myDepositRateEUROIS : myDepositRate
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        // DATABASE TABLE
        //static public string _referenceDBtable = "INTERESTRATE";
        static public myDB _referenceDBtable = myDB.InterestRate;



        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************




        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myDepositRateEUROIS() { }


        // Constructor 2 : Full fledged

        public myDepositRateEUROIS(DateTime PricingDate_,
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

    public class myEONIAOIS : myDepositRateEUROIS
    {
        public myEONIAOIS()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EONIA Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }

        public myEONIAOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "EONIA Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }
    }


    public class myOISswap1W : myDepositRateEUROIS
    {
        public myOISswap1W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUSEW1Z Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap1W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUSEW1Z Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap2W : myDepositRateEUROIS
    {
        public myOISswap2W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUSEW2Z Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap2W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "EUSEW2Z Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap1M : myDepositRateEUROIS
    {
        public myOISswap1M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEA Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap1M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEA Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap2M : myDepositRateEUROIS
    {
        public myOISswap2M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEB Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap2M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEB Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap3M : myDepositRateEUROIS
    {
        public myOISswap3M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEC Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap3M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEC Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap4M : myDepositRateEUROIS
    {
        public myOISswap4M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(4, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWED Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap4M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(4, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWED Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap5M : myDepositRateEUROIS
    {
        public myOISswap5M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(5, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEE Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap5M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(5, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEE Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap6M : myDepositRateEUROIS
    {
        public myOISswap6M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEF Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap6M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEF Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap7M : myDepositRateEUROIS
    {
        public myOISswap7M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(7, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEG Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap7M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(7, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEG Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap8M : myDepositRateEUROIS
    {
        public myOISswap8M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(8, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEH Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap8M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(8, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEH Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap9M : myDepositRateEUROIS
    {
        public myOISswap9M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEI Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap9M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEI Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap10M : myDepositRateEUROIS
    {
        public myOISswap10M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(10, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEJ Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap10M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(10, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEJ Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myOISswap11M : myDepositRateEUROIS
    {
        public myOISswap11M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(11, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEK Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myOISswap11M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(11, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "EUSWEK Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }




    #endregion

}


