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
    public class myRateCurveEURv3m : myRateCurve
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        static List<myRate> staticRateElements = new List<myRate> 
                    { 
                    new myEuribor1W(),
                    new myEuribor1M(),
                    new myEuribor3M(),
                    new myEuribor6M(),
                    new myEuribor12M(),
                    new myEurSwapRate2Yv3m(),
                    new myEurSwapRate3Yv3m(),
                    new myEurSwapRate4Yv3m(),
                    new myEurSwapRate5Yv3m(),      
                    new myEurSwapRate6Yv3m(), 
                    new myEurSwapRate7Yv3m(),
                    new myEurSwapRate8Yv3m(),
                    new myEurSwapRate9Yv3m(),
                    new myEurSwapRate10Yv3m(),
                    new myEurSwapRate15Yv3m(),                    
                    new myEurSwapRate20Yv3m(),
                    new myEurSwapRate25Yv3m(),
                    new myEurSwapRate30Yv3m()
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
        public myRateCurveEURv3m() { }



        // Constructor 2 : Basic constructor 
        public myRateCurveEURv3m(DateTime argPricingDate)

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
