using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLNet;


namespace Autocall
{
    class Autocall
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

            double underlying = 100;
            double dividendYield = 0.035;
            double riskFreeRate = 0.01;
            double intensity = 0.02;
            double volatility = 0.20;

            Handle<YieldTermStructure> flatTermStructure = new Handle<YieldTermStructure>(new FlatForward(settlementDate, riskFreeRate, dayCounter));
            Handle<DefaultProbabilityTermStructure> flatHazardStructure = new Handle<DefaultProbabilityTermStructure>(new FlatHazardRate(settlementDate, intensity, dayCounter));

            Period forwardStart = new Period(1, TimeUnit.Days);
            DayCounter swFixedLegDayCounter = new Thirty360(Thirty360.Thirty360Convention.European);
            IborIndex swFloatingLegIndex = new Euribor6M();


            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(underlying));
            Handle<YieldTermStructure> flatDividendTS = new Handle<YieldTermStructure>(new FlatForward(settlementDate, dividendYield, dayCounter));
            Handle<BlackVolTermStructure> flatVolTS = new Handle<BlackVolTermStructure>(new BlackConstantVol(settlementDate, calendar, volatility, dayCounter));

            Console.WriteLine("Underlying price = " + underlying);
            Console.WriteLine("Risk-free interest rate = {0:0.00%}", riskFreeRate);
            Console.WriteLine("Dividend yield = {0:0.00%}", dividendYield);
            Console.WriteLine("Flat Volatility = {0:0.00%}", volatility);
            Console.Write("\n");

            ////////////////  VOL SURFACE  //////////////////////////////////////////////

            List<Date> datesVol = new InitializedList<Date>();
            List<double> strikesVol = new InitializedList<double>();
            double spotATP = 100;
            Date StartDateVol = settlementDate + new Period(1, TimeUnit.Months);
            Matrix blackVolMatrix = new Matrix(5, 5, 0.2);


            List<double> timesVol = new InitializedList<double>();
            for (int i = 0; i < blackVolMatrix.rows(); i++)
                timesVol.Add(i + 1);

            for (int j = 0; j < blackVolMatrix.columns(); j++)
            {
                datesVol.Add(StartDateVol + new Period(j, TimeUnit.Years));
                for (int i = 0; i < blackVolMatrix.rows(); i++)
                    blackVolMatrix[i, j] = 0.2 + Math.Pow(2.5, (i)) / 100;
            }
            for (int j = 0; j < blackVolMatrix.columns(); j++)
                strikesVol.Add(spotATP * (1 - (double)((int)(blackVolMatrix.rows() / 2)) / 10) + spotATP * 0.1 * j);



            BlackVarianceSurface mySurface = new BlackVarianceSurface(settlementDate,
                                                                    calendar,
                                                                    datesVol,
                                                                    strikesVol,
                                                                    blackVolMatrix,
                                                                    dayCounter);

            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(mySurface);

            GeneralizedBlackScholesProcessTolerance bsmProcessVolSurface = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatTermStructure, mySurfaceH);
            GeneralizedBlackScholesProcessTolerance bsmProcess = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatTermStructure, flatVolTS);


            ////////////////  INSTRUMENT  //////////////////////////////////////////////

            double tolerance = 0.2;

            List<Date> fixingdates = new InitializedList<Date>();
            double coupon = 0.05;
            double barrierlvl = 0.6;
            for (int i = 1; i <= 4; i++)
                fixingdates.Add(settlementDate + new Period(i, TimeUnit.Years));

            GenericAutocall myGenericAutocall = new GenericAutocall(fixingdates, coupon, barrierlvl, underlying);
            IPricingEngine mcengine = new MakeMGenericInstrument<PseudoRandom>(bsmProcessVolSurface)
                                                                            .withAbsoluteTolerance(tolerance)
                                                                            .withStepsPerYear(52)
                                                                            .withSeed(50)
                                                                            .value();
            myGenericAutocall.setPricingEngine(mcengine);


            ////////////////  Printing Results  //////////////////////////////////////////////

            Console.WriteLine("NPV Generic Autocall = {0:0.0000}", myGenericAutocall.NPV());
            Console.WriteLine("Err = {0:0.0000%}", myGenericAutocall.errorEstimate() / myGenericAutocall.NPV());
            Console.WriteLine("Samples = {0}", myGenericAutocall.samples());
            Console.Write("\n");

            for (int i = 0; i < 4; i++)
                Console.WriteLine("ProbaCall {1} = {0:0.0000%}", myGenericAutocall.inspout("ProbaCall " + i), i + 1);
            Console.WriteLine("ProbaMid = {0:0.0000%}", myGenericAutocall.inspout("ProbaMid"));
            Console.WriteLine("probaDown = {0:0.0000%}", myGenericAutocall.inspout("ProbaDown"));
            Console.WriteLine("AvgDown = {0:0.0000%}", myGenericAutocall.inspout("AvgDown") / myGenericAutocall.inspout("ProbaDown"));
            Console.Write("\n");


            // End test
            Console.WriteLine(" \nRun completed in {0}", DateTime.Now - timer);
            Console.WriteLine();

            Console.Write("Press any key to continue ...");
            Console.ReadKey();


        }
    }
}
