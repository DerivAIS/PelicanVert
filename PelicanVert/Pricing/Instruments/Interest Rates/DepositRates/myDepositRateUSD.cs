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
    
    public class myDepositRateUSD : myDepositRate
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        // DATABASE TABLE
        // static public string _referenceDBtable = "INTERESTRATE"; -- OLD
        static public myDB _referenceDBtable = myDB.InterestRate;


        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************




        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myDepositRateUSD() { }


        // Constructor 2 : Full fledged

        public myDepositRateUSD(DateTime PricingDate_,
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
                        DepoCurrency_: "USD"
                ) { }



        #endregion


    }



    // ************************************************************
    // CLASS WRAPPERS (e.g. EONIA, TN, USDIBOR1M, etc.)
    // ************************************************************

    #region

    public class myFedFunds : myDepositRateUSD
    {
        public myFedFunds()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "US00O/N Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }

        public myFedFunds(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "US00O/N Index"), FixingDays_: 0, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }
    }


    public class myUSTN : myDepositRateUSD
    {
        public myUSTN()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "USDR2T Index"), FixingDays_: 1, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }

        public myUSTN(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Days), argDBID_: TokenFactory.New(Bloomberg: "USDR2T Index"), FixingDays_: 1, BDC_: BusinessDayConvention.Following, DayCounter_: new Actual360()) { }
    }


    public class myUSLibor1W : myDepositRateUSD
    {
        public myUSLibor1W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "US0001W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myUSLibor1W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "US0001W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myUSLibor2W : myDepositRateUSD
    {
        public myUSLibor2W()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "US0002W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }

        public myUSLibor2W(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Weeks), argDBID_: TokenFactory.New(Bloomberg: "US0002W Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Actual360()) { }
    }


    public class myUSLibor1M : myDepositRateUSD
    {
        public myUSLibor1M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0001M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor1M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(1, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0001M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    public class myUSLibor2M : myDepositRateUSD
    {
        public myUSLibor2M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0002M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor2M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(2, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0002M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    public class myUSLibor3M : myDepositRateUSD
    {
        public myUSLibor3M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0003M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor3M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(3, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0003M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    public class myUSLibor6M : myDepositRateUSD
    {
        public myUSLibor6M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0006M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor6M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(6, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0006M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    public class myUSLibor9M : myDepositRateUSD
    {
        public myUSLibor9M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0009M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor9M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(9, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0009M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }


    public class myUSLibor12M : myDepositRateUSD
    {
        public myUSLibor12M()
            : base(PricingDate_: DateTime.Today, Period_: new Period(12, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0012M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }

        public myUSLibor12M(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Period_: new Period(12, TimeUnit.Months), argDBID_: TokenFactory.New(Bloomberg: "US0012M Index"), FixingDays_: 2, BDC_: BusinessDayConvention.ModifiedFollowing, DayCounter_: new Thirty360()) { }
    }




    #endregion

}


