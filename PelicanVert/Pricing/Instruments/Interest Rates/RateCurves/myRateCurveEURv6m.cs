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
    public class myRateCurveEUR : myRateCurve
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
                    new myEurSwapRate2Y(),
                    new myEurSwapRate3Y(),
                    new myEurSwapRate4Y(),
                    new myEurSwapRate5Y(),      
                    new myEurSwapRate6Y(), 
                    new myEurSwapRate7Y(),
                    new myEurSwapRate8Y(),
                    new myEurSwapRate9Y(),
                    new myEurSwapRate10Y(),
                    new myEurSwapRate15Y(),                    
                    new myEurSwapRate20Y(),
                    new myEurSwapRate25Y(),
                    new myEurSwapRate30Y()
                    };

        static Calendar staticCalendar = new TARGET();

        static Currency staticCurrency = new EURCurrency();

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
        public myRateCurveEUR() { }



        // Constructor 2 : Basic constructor 
        public myRateCurveEUR(DateTime argPricingDate)

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
