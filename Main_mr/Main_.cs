using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;


namespace PelicanVert
{
    class MainPricer
    {

        public double MyFunc(double Xvalue)
        {
            return Xvalue * Xvalue;
        }

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
            double underlying    = 100.0;
            double dividendYield = 0.035;
            double riskFreeRate  = 0.01;
            double intensity     = 0.02;
            double volatility    = 0.20;

            Handle<YieldTermStructure> flatRfTermStructure = new Handle<YieldTermStructure>(new FlatForward(settlementDate, riskFreeRate, dayCounter));
            Handle<DefaultProbabilityTermStructure> flatHazardStructure = new Handle<DefaultProbabilityTermStructure>(new FlatHazardRate(settlementDate, intensity, dayCounter));

            Period forwardStart = new Period(1, TimeUnit.Days);
            DayCounter swFixedLegDayCounter = new Thirty360(Thirty360.Thirty360Convention.European);
            IborIndex swFloatingLegIndex = new Euribor6M();


            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(underlying));
            Handle<YieldTermStructure> flatDividendTS = new Handle<YieldTermStructure>(new FlatForward(settlementDate, dividendYield, dayCounter));
            Handle<BlackVolTermStructure> flatVolTS = new Handle<BlackVolTermStructure>(new BlackConstantVol(settlementDate, calendar, volatility, dayCounter));
            GeneralizedBlackScholesProcessTolerance bsmProcess = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatRfTermStructure, flatVolTS);


            Console.WriteLine("Underlying price = " + underlying);
            Console.WriteLine("Risk-free interest rate = {0:0.00%}", riskFreeRate);
            Console.WriteLine("Dividend yield = {0:0.00%}", dividendYield);
            Console.WriteLine("Volatility = {0:0.00%}", volatility);
            Console.Write("\n");

            ////////////////  SIMPLEX  //////////////////////////////////////////////
            /*
            CostFunction corstFunction = new CostFunction();

            List<double> X = new InitializedList<double>();
            X.Add(0.0);
            Vector vectX = new Vector (X);

            List<double> Dir = new InitializedList<double>();
            Dir.Add(1);
            Vector vectDir = new Vector (Dir);

            Constraint constraint = new PositiveConstraint();
            constraint
            Vector initValues = new Vector();

            Problem myProb = new Problem(corstFunction, constraint, initValues);
            */

            ////////////////  SURFACE  //////////////////////////////////////////////

            /*
            freeArbSVI testSurface = new freeArbSVI(strikesVol, timesVol, spotATP, flatTermStructure, flatDividendTS, blackVolMatrix,50);

            testSurface.matricesBuildingForwardMoneyness();
            testSurface.matricesBuildingTotalVariance();
            testSurface.matricesBuildingBSPrices();
            testSurface.splincalculation();
            testSurface.matricesBuildingA();
            testSurface.matricesBuildingB();
            */

            //Console.WriteLine("value [0,0] = {0}", blackVolMatrix[1,1]);









            // End test
            Console.WriteLine(" \nRun completed in {0}", DateTime.Now - timer);
            Console.WriteLine();

            Console.Write("Press any key to continue ...");
            Console.ReadKey();

        }
    }
}