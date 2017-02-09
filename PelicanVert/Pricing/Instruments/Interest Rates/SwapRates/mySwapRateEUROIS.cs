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


    abstract class mySwapRateEUROIS : mySwapRate
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
        public mySwapRateEUROIS() { }


        // Constructor 2 : Normal usage

        public mySwapRateEUROIS(DateTime PricingDate_,
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



    class myEurSwapRate1YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate1YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE1 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate1YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(1, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE1 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }


    class myEurSwapRate2YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate2YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE2 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate2YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(2, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE2 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }


    class myEurSwapRate3YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate3YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE3 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate3YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(3, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE3 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate4YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate4YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE4 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate4YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(4, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE4 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate5YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate5YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE5 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate5YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(5, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE5 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate6YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate6YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE6 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate6YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(6, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE6 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate7YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate7YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE7 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate7YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(7, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE7 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate8YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate8YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE8 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate8YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(8, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE8 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate9YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate9YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE9 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate9YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(9, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE9 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate10YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate10YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE10 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate10YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(10, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE10 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate11YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate11YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE11 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate11YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(11, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE11 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate12YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate12YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE12 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate12YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(12, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE12 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate15YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate15YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE15 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate15YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(15, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE15 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate20YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate20YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE20 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate20YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(20, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE20 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate25YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate25YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE25 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate25YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(25, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE25 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate30YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate30YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE30 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate30YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(30, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE30 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate35YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate35YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE35 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate35YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(35, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE35 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate40YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate40YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE40 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate40YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(40, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE40 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate45YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate45YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE45 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate45YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(45, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE45 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }

    class myEurSwapRate50YOIS : mySwapRateEUROIS
    {
        public myEurSwapRate50YOIS()
            : base(PricingDate_: DateTime.Today, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE50 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }

        public myEurSwapRate50YOIS(DateTime argPricingDate)
            : base(PricingDate_: argPricingDate, Frequency_: Frequency.Annual, Period_: new Period(50, TimeUnit.Years), argDBID_: TokenFactory.New(Bloomberg: "EUSWE50 Curncy"), ForwardStart_: new Period(2, TimeUnit.Days), SwapFixedLegBDC_: BusinessDayConvention.ModifiedFollowing, SwapFixedLegDayCounter_: new Thirty360(), SwapFloatingLegIndex_: new Eonia()) { }
    }




    #endregion

}


