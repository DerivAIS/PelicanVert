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


    abstract class mySwapRateEURv3m : mySwapRate
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
        public mySwapRateEURv3m() { }


        // Constructor 2 : Normal usage

        public mySwapRateEURv3m(DateTime PricingDate_,
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

    // Thirty360() --> Thirty360()

    // ************************************************************
    // CLASS WRAPPERS (e.g. EONIA, TN, EURIBOR1M, etc.)
    // ************************************************************

    #region



    class myEurSwapRate1Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate1Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW1V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate1Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW1V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }


    class myEurSwapRate2Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate2Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW2V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate2Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW2V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }


    class myEurSwapRate3Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate3Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW3V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate3Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW3V3 Curency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate4Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate4Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW4V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate4Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW4V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate5Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate5Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW5V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate5Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW5V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate6Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate6Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW6V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate6Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW6V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate7Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate7Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW7V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate7Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW7V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate8Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate8Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW8V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate8Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW8V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate9Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate9Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW9V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate9Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW9V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate10Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate10Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW10V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate10Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW10V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate11Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate11Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW11V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate11Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW11V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate12Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate12Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW12V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate12Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW12V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate15Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate15Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW15V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate15Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW15V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate20Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate20Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW20V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate20Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW20V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate25Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate25Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW25V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate25Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW25V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate30Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate30Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW30V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate30Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW30V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate35Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate35Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW35V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate35Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW35V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate40Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate40Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW40V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate40Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW40V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate45Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate45Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW45V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate45Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW45V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }

    class myEurSwapRate50Yv3m : mySwapRateEURv3m
    {
        public myEurSwapRate50Yv3m()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW50V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }

        public myEurSwapRate50Yv3m(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSW50V3 Currency"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor3M()) { }
    }




    #endregion

}


