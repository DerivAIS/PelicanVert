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


    abstract class mySwapRateEUR : mySwapRate
    {


        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        // REF TABLE
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
        public mySwapRateEUR() { }


        // Constructor 2 : Normal usage

        public mySwapRateEUR(DateTime PricingDate_,
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



    class myEurSwapRate1Y : mySwapRateEUR
    {
        public myEurSwapRate1Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA1 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate1Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA1 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate2Y : mySwapRateEUR
    {
        public myEurSwapRate2Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA2 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate2Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA2 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate3Y : mySwapRateEUR
    {
        public myEurSwapRate3Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA3 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate3Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA3 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate4Y : mySwapRateEUR
    {
        public myEurSwapRate4Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA4 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate4Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA4 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate5Y : mySwapRateEUR
    {
        public myEurSwapRate5Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA5 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate5Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA5 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate6Y : mySwapRateEUR
    {
        public myEurSwapRate6Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA6 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate6Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA6 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate7Y : mySwapRateEUR
    {
        public myEurSwapRate7Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA7 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate7Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA7 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate8Y : mySwapRateEUR
    {
        public myEurSwapRate8Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA8 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate8Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA8 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate9Y : mySwapRateEUR
    {
        public myEurSwapRate9Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA9 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate9Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA9 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate10Y : mySwapRateEUR
    {
        public myEurSwapRate10Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA10 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate10Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA10 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate11Y : mySwapRateEUR
    {
        public myEurSwapRate11Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA11 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate11Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA11 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate12Y : mySwapRateEUR
    {
        public myEurSwapRate12Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA12 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate12Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA12 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate15Y : mySwapRateEUR
    {
        public myEurSwapRate15Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA15 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate15Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA15 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate20Y : mySwapRateEUR
    {
        public myEurSwapRate20Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA20 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate20Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA20 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate25Y : mySwapRateEUR
    {
        public myEurSwapRate25Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA25 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate25Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA25 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate30Y : mySwapRateEUR
    {
        public myEurSwapRate30Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA30 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate30Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA30 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate35Y : mySwapRateEUR
    {
        public myEurSwapRate35Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA35 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate35Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA35 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate40Y : mySwapRateEUR
    {
        public myEurSwapRate40Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA40 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate40Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA40 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate45Y : mySwapRateEUR
    {
        public myEurSwapRate45Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA45 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate45Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA45 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }

    class myEurSwapRate50Y : mySwapRateEUR
    {
        public myEurSwapRate50Y()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA50 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }

        public myEurSwapRate50Y(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSA50 Index"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Euribor6M()) { }
    }




    #endregion

}


