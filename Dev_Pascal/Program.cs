using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom
using QLNet;

// Internal Custom
using QLyx.DataIO.Markit;
using Pascal.Valuation;
using QLyx.InterestRates;
using QLyx.Utilities;

namespace Dev_Pascal
{
    class Program
    {

        static void Main(string[] args)
        {


            // *********************************************************************************
            // INITIAL DISPLAY : PASCAL DEV
            // *********************************************************************************

            Console.WriteLine("************************************");
            Console.WriteLine("  Running Project : Pascal (dev)");
            Console.WriteLine("************************************");

            DateTime pricingDate = new DateTime(2017, 02, 10);


            // *********************************************************************************
            // PASCAL DEV : STRATEGIES SGI (STRANGLE SPX & SX5E)
            // *********************************************************************************

            // SGIXESPE strangle_SX5E = new SGIXESPE(pricingDate);
            // strangle_SX5E.Results();

            // SGIXESPU strangle_SPX = new SGIXESPU(pricingDate);
            // strangle_SPX.Results();


            // *********************************************************************************
            // INITIAL DISPLAY : PASCAL DEV
            // *********************************************************************************

            Period freq = new Period(1, TimeUnit.Years);

            List<DateTime> dates = new List<DateTime>();
            for (int i = 1; i <= 10; i++) {
                dates.Add((pricingDate.ToDate() + freq*i).ToDateTime());
            }

            DateTime strikeDate = (pricingDate.ToDate() - freq).ToDateTime();

            // COUPONS DU BOND
            /*
            EUR_Coupon_Stream cpn = new EUR_Coupon_Stream(1.0, dates);
            cpn.PV_v3m(pricingDate);
            double px = cpn.PV_v3m(pricingDate);
            */
            
            BermudeanCliquetBinaryOption_v2 opt = new BermudeanCliquetBinaryOption_v2(strikeDate, dates, MarkitEquityUnderlying.Eurostoxx_TR, 
                0.077, 0.70, 1.21, 0.70, new TARGET(), new Actual365Fixed(), BusinessDayConvention.Preceding);

            double px = opt.NPV(pricingDate);
            //double probaDown = opt.binaire().inspout("AvgMid");

            Console.ReadKey();

        }
    }
}
