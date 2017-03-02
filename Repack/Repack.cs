
///////  Marc RAYGOT - 2017   ///////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;


namespace Repack
{
    class Repack
    {
        static void Main(string[] args)
        {

            DateTime timer = DateTime.Now;

            ////////////////  DATES  //////////////////////////////////////////////

            Calendar calendar = new TARGET();
            Date todaysDate = new Date(15, Month.January, 2017);
            Date settlementDate = new Date(todaysDate);
            Settings.setEvaluationDate(todaysDate);
            DayCounter dayCounter = new Actual365Fixed();


            ////////////////  MARKET  //////////////////////////////////////////////

            // Spot
            double underlying = 1.0;
            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(underlying));

            // Curves
            double riskFreeRate = 0.01;
            double dividendYield = 0.035;
            double cstVol = 0.2;
            double hazardRate = 0.02;

            Handle<YieldTermStructure> flatTermStructure = new Handle<YieldTermStructure>(new FlatForward(settlementDate, riskFreeRate, dayCounter));
            Handle<YieldTermStructure> flatDividendTS = new Handle<YieldTermStructure>(new FlatForward(settlementDate, dividendYield, dayCounter));
            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(new BlackConstantVol(settlementDate, calendar, cstVol, dayCounter));
            Handle<DefaultProbabilityTermStructure> defaultTSH = new Handle<DefaultProbabilityTermStructure>(new FlatHazardRate(settlementDate, hazardRate, dayCounter));

            // Process
            GeneralizedBlackScholesProcessTolerance bsmProcess = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatTermStructure, mySurfaceH);


            ////////////////  INSTRUMENT  //////////////////////////////////////////////

            List<Date> fixings = new InitializedList<Date>();
            for (int i = 1; i <= 8; i++)
                fixings.Add(settlementDate + new Period(i * 3, TimeUnit.Months));

            double couponBond = 0.0525;
            double couponBinary = 0.077;
            double barrierlvl = 0.7;
            double strike = 1.0;
            double recovery = 0.4;

            double absoluteTolerance = 0.0001;

            GenericScriptRepack repack = new GenericScriptRepack(fixings, couponBond, couponBinary, barrierlvl, strike, recovery);
            IPricingEngine mcengine = new MakeMCGenericScriptInstrument<PseudoRandom>(bsmProcess)
                                                                .withAbsoluteTolerance(absoluteTolerance)
                                                                .withStepsPerYear(52)
                                                                .withSeed(50)
                                                                .withCredit(defaultTSH)
                                                                .value();

            repack.setPricingEngine(mcengine);
            Console.WriteLine("Repack pricing = {0:0.0000}", repack.NPV());
            repack.inspout(5, true);

            ////////////////  END TEST  //////////////////////////////////////////////
            Console.WriteLine();
            Console.WriteLine(" \nRun completed in {0}", DateTime.Now - timer);
            Console.WriteLine();

            Console.Write("Press any key to continue ...");
            Console.ReadKey();





        }
    }
}