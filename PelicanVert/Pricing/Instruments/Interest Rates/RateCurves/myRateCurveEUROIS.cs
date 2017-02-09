using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom packages
using QLNet;

namespace QLyx.InterestRates
{
    class myRateCurveEUROIS : myRateCurve
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        static List<myRate> staticRateElements = new List<myRate> 
                    { 
                    new myOISswap1W(),
                    new myOISswap2W(),
                    new myOISswap1M(),
                    new myOISswap3M(),
                    new myOISswap6M(),
                    new myEurSwapRate1YOIS(),
                    new myEurSwapRate3YOIS(),
                    new myEurSwapRate4YOIS(),
                    new myEurSwapRate5YOIS(),      
                    new myEurSwapRate6YOIS(), 
                    new myEurSwapRate7YOIS(),
                    new myEurSwapRate8YOIS(),
                    new myEurSwapRate9YOIS(),
                    new myEurSwapRate10YOIS(),
                    new myEurSwapRate15YOIS(),                    
                    new myEurSwapRate20YOIS(),
                    new myEurSwapRate25YOIS(),
                    new myEurSwapRate30YOIS()
                    };

        static Calendar staticCalendar = new TARGET();

        static Currency staticCurrency = new EURCurrency();

        static BusinessDayConvention staticBDC = BusinessDayConvention.Unadjusted;

        static DayCounter staticDayCounter = new Actual360();

        static int staticCurveFixingDays = 0;




        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        // n/a



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region


        // Constructor 1 : Generic
        public myRateCurveEUROIS() { }



        // Constructor 2 : Basic constructor 
        public myRateCurveEUROIS(DateTime argPricingDate)

            : base(pricingDate: argPricingDate,
                    argRateElements: staticRateElements,
                    Currency_: staticCurrency,
                    Calendar_: staticCalendar,
                    BDC_: staticBDC,
                    DCC_: staticDayCounter,
                    FixingDays: staticCurveFixingDays) { }


        #endregion










    }
}
