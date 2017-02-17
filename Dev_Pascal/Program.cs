using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom
using QLNet;

// Internal Custom QLYX
using QLyx.DataIO.Markit;
using QLyx.InterestRates;
using QLyx.Utilities;

// Internal Custom DEV
using Pascal.Valuation;
using Pascal.Pricing;
using Pascal.Pricing.Bonds;
using Pascal.Pricing.Underlyings;

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

            DateTime strikeDate = new DateTime(2014, 06, 18);

            
            //FixedDivIndex FSEURE = new FixedDivIndex(PricingUnderlying.IND1EDFI, MarkitEquityUnderlying.Eurostoxx, 0, 0, 0.5, 1.00);
            //var sdsd = FSEURE.impliedVolatilitySurface(pricingDate);


            double couponLevel = 0.077;
            double barrierLevel = 0.70;
            double cliquetLevel = 1.21;

            BermudeanCliquetBinaryOption<IND1EDFI> opt = new BermudeanCliquetBinaryOption<IND1EDFI>(strikeDate, dates, couponLevel, barrierLevel, cliquetLevel, new TARGET(), 
                new Actual365Fixed(), BusinessDayConvention.Preceding);

            double px = opt.NPV(pricingDate);
            // double probaDown = opt.binaire().inspout("AvgMid");



            throw new NotImplementedException();

            BondPricingInstrument btp = new BondPricingInstrument(BondReferential.BTP_525_01Nov2029, new DateTime(2029, 11, 01), 
                100.0, 5.25, new Period(6, TimeUnit.Months), "EUR", new Italy(), new Actual365Fixed(), BusinessDayConvention.ModifiedPreceding);

            double pxBondClean = btp.cleanPrice(pricingDate);
            double pxBondDirty = btp.dirtyPrice(pricingDate);
            double assetSwapSpread = btp.assetSwapSpread(pricingDate);

            btp.couponsPV_vs3m(pricingDate);

            Console.ReadKey();

        }
    }
}
