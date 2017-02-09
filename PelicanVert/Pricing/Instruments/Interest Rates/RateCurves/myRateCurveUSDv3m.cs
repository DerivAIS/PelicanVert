using System;
using System.Collections;
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
    class myRateCurveUSD : myRateCurve
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************


        static List<myRate> staticRateElements = new List<myRate> 
                    { 
                    new myUSLibor1W(),
                    new myUSLibor1M(),
                    new myUSLibor3M(),
                    new myUSLibor6M(),
                    new myUSLibor12M(),
                    new myUsdSwapRate2Y(),
                    new myUsdSwapRate3Y(),
                    new myUsdSwapRate4Y(),
                    new myUsdSwapRate5Y(),      
                    new myUsdSwapRate6Y(), 
                    new myUsdSwapRate7Y(),
                    new myUsdSwapRate8Y(),
                    new myUsdSwapRate9Y(),
                    new myUsdSwapRate10Y(),
                    new myUsdSwapRate15Y(),                    
                    new myUsdSwapRate20Y(),
                    new myUsdSwapRate25Y(),
                    new myUsdSwapRate30Y()
                    };

        static Calendar staticCalendar = new UnitedStates();

        static Currency staticCurrency = new USDCurrency();

        static BusinessDayConvention staticBDC = BusinessDayConvention.Unadjusted;

        static DayCounter staticDayCounter = new Actual360();

        static int staticFixingDays = 0;




        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        // n/a



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region


        // Constructor 1 : Generic
        public myRateCurveUSD() { }



        // Constructor 2 : Basic constructor 
        public myRateCurveUSD(DateTime argPricingDate)

            : base(pricingDate: argPricingDate,
                    argRateElements: staticRateElements,
                    Currency_: staticCurrency,
                    Calendar_: staticCalendar,
                    BDC_: staticBDC,
                    DCC_: staticDayCounter,
                    FixingDays: staticFixingDays) { }


        #endregion










    }
}
