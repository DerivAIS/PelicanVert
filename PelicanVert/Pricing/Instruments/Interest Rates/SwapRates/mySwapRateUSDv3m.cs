using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

// Internal custom packages
using QLyx.DataIO;


namespace QLyx.InterestRates
{


    abstract public class mySwapRateUSD : mySwapRate
    {


        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        // REF TABLE
        // static public string _referenceDBtable = "INTERESTRATE";
        static public myDB _referenceDBtable = myDB.InterestRate;


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        // n/a



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public mySwapRateUSD() { }


        // Constructor 2 : Normal usage

        public mySwapRateUSD(DateTime PricingDate_,
                                Frequency Frequency_,
                                Period Period_,
                                IDtoken argDBID_,
                                Period ForwardStart_,
                                BusinessDayConvention SwapFixedLegBDC_,
                                DayCounter SwapFixedLegDayCounter_,
                                IborIndex SwapFloatingLegIndex_)

            : base(
                        PricingDate_: PricingDate_,
                        Frequency_: Frequency_,
                        Period_: Period_,
                        argDBID_: argDBID_,
                        ForwardStart_: ForwardStart_,
                        SwapFixedLegBDC_: SwapFixedLegBDC_,
                        SwapFixedLegDayCounter_: SwapFixedLegDayCounter_,
                        SwapFloatingLegIndex_: SwapFloatingLegIndex_,
                        SwapCurrency_: "EUR"
                ) { }



        #endregion


    }



    // ************************************************************
    // CLASS WRAPPERS (e.g. EONIA, TN, EURIBOR1M, etc.)
    // ************************************************************

    #region



    public class myUsdSwapRate1Y : mySwapRateUSD
    {
        public myUsdSwapRate1Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA1 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate1Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA1 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }


    public class myUsdSwapRate2Y : mySwapRateUSD
    {
        public myUsdSwapRate2Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA2 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate2Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA2 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }


    public class myUsdSwapRate3Y : mySwapRateUSD
    {
        public myUsdSwapRate3Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA3 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate3Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA3 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate4Y : mySwapRateUSD
    {
        public myUsdSwapRate4Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA4 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate4Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA4 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate5Y : mySwapRateUSD
    {
        public myUsdSwapRate5Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA5 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate5Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA5 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate6Y : mySwapRateUSD
    {
        public myUsdSwapRate6Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA6 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate6Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA6 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate7Y : mySwapRateUSD
    {
        public myUsdSwapRate7Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA7 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate7Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA7 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate8Y : mySwapRateUSD
    {
        public myUsdSwapRate8Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA8 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate8Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA8 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate9Y : mySwapRateUSD
    {
        public myUsdSwapRate9Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA9 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate9Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA9 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate10Y : mySwapRateUSD
    {
        public myUsdSwapRate10Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA10 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate10Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA10 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate11Y : mySwapRateUSD
    {
        public myUsdSwapRate11Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA11 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate11Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA11 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate12Y : mySwapRateUSD
    {
        public myUsdSwapRate12Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA12 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate12Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA12 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate15Y : mySwapRateUSD
    {
        public myUsdSwapRate15Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA15 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate15Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA15 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate20Y : mySwapRateUSD
    {
        public myUsdSwapRate20Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA20 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate20Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA20 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate25Y : mySwapRateUSD
    {
        public myUsdSwapRate25Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA25 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate25Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA25 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate30Y : mySwapRateUSD
    {
        public myUsdSwapRate30Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA30 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate30Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA30 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate35Y : mySwapRateUSD
    {
        public myUsdSwapRate35Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA35 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate35Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA35 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate40Y : mySwapRateUSD
    {
        public myUsdSwapRate40Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA40 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate40Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA40 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate45Y : mySwapRateUSD
    {
        public myUsdSwapRate45Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA45 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate45Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA45 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }

    public class myUsdSwapRate50Y : mySwapRateUSD
    {
        public myUsdSwapRate50Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA50 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }

        public myUsdSwapRate50Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "USSA50 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new USDLibor(new Period(3, TimeUnit.Months))) { }
    }




    #endregion

}


