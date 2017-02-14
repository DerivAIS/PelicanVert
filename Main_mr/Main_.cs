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

            // set up dates  ////////////////////////////////////////
            Calendar calendar = new TARGET();
            Date todaysDate = new Date(15, Month.January, 2017);
            Date settlementDate = new Date(todaysDate);
            Settings.setEvaluationDate(todaysDate);
            DayCounter dayCounter = new Actual365Fixed();


            // market //////////////////////////////////////////////
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


            double zc1yRate = 0.002585;
            double zc2yRate = 0.005034;
            double zc3yRate = 0.008981;
            double zc4yRate = 0.012954;
            double zc5yRate = 0.016452;
            double zc7yRate = 0.021811;
            double zc10yRate = 0.027007;
            double zc15yRate = 0.031718;
            double zc20yRate = 0.033834;
            double zc30yRate = 0.035056;

            DayCounter zcBondsDayCounter = new Actual365Fixed();
            int fixingDays = 0;

            RateHelper zc1y = new DepositRateHelper(zc1yRate,
                                             new Period(1, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc2y = new DepositRateHelper(zc2yRate,
                                             new Period(2, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc3y = new DepositRateHelper(zc3yRate,
                                             new Period(3, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc4y = new DepositRateHelper(zc4yRate,
                                             new Period(4, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc5y = new DepositRateHelper(zc5yRate,
                                             new Period(5, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc7y = new DepositRateHelper(zc7yRate,
                                             new Period(7, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc10y = new DepositRateHelper(zc10yRate,
                                             new Period(10, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc15y = new DepositRateHelper(zc15yRate,
                                             new Period(15, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc20y = new DepositRateHelper(zc20yRate,
                                             new Period(20, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,
                                             true, zcBondsDayCounter);
            RateHelper zc30y = new DepositRateHelper(zc30yRate,
                                            new Period(30, TimeUnit.Years), fixingDays,
                                            calendar, BusinessDayConvention.ModifiedFollowing,
                                            true, zcBondsDayCounter);


            List<RateHelper> zcInstruments = new List<RateHelper>();
            zcInstruments.Add(zc1y);
            zcInstruments.Add(zc2y);
            zcInstruments.Add(zc3y);
            zcInstruments.Add(zc4y);
            zcInstruments.Add(zc5y);
            zcInstruments.Add(zc7y);
            zcInstruments.Add(zc10y);
            zcInstruments.Add(zc15y);
            zcInstruments.Add(zc20y);
            zcInstruments.Add(zc30y);

            double tolerance = 1.0e-15;

            YieldTermStructure zcTermStructure = new PiecewiseYieldCurve<Discount, LogLinear>(
                                                                   settlementDate, zcInstruments,
                                                                   zcBondsDayCounter,
                                                                   new List<Handle<Quote>>(),
                                                                   new List<Date>(),
                                                                   tolerance);


            Handle<YieldTermStructure> zcTermStructureH = new Handle<YieldTermStructure>(zcTermStructure);


            CreditDefaultSwapHelper cds1Y = new CreditDefaultSwapHelper(0.6405, new Period(1, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds2Y = new CreditDefaultSwapHelper(0.5956, new Period(2, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds3Y = new CreditDefaultSwapHelper(0.5511, new Period(3, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds4Y = new CreditDefaultSwapHelper(0.5144, new Period(4, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds5Y = new CreditDefaultSwapHelper(0.4894, new Period(5, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds7Y = new CreditDefaultSwapHelper(0.4511, new Period(7, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds10Y = new CreditDefaultSwapHelper(0.4156, new Period(10, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds15Y = new CreditDefaultSwapHelper(0.3815, new Period(15, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds20Y = new CreditDefaultSwapHelper(0.3657, new Period(20, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds30Y = new CreditDefaultSwapHelper(0.3506, new Period(30, TimeUnit.Years), zcTermStructureH);

            List<CreditDefaultSwapHelper> cdsInstruments = new List<CreditDefaultSwapHelper>();
            cdsInstruments.Add(cds1Y);
            cdsInstruments.Add(cds2Y);
            cdsInstruments.Add(cds3Y);
            cdsInstruments.Add(cds4Y);
            cdsInstruments.Add(cds5Y);
            cdsInstruments.Add(cds7Y);
            cdsInstruments.Add(cds10Y);
            cdsInstruments.Add(cds15Y);
            cdsInstruments.Add(cds20Y);
            cdsInstruments.Add(cds30Y);

            DefaultProbabilityTermStructure defaultTS = new PiecewiseHazardRateCurve<HazardRate, BackwardFlat>(
                                                                    settlementDate,
                                                                    cdsInstruments,
                                                                    dayCounter,
                                                                    new List<Handle<Quote>>(),
                                                                    new List<Date>(),
                                                                    tolerance);

            Handle<DefaultProbabilityTermStructure> defaultTSH = new Handle<DefaultProbabilityTermStructure>(defaultTS);

            double test1 = defaultTSH.link.hazardRate(0.9);
            double test2 = defaultTSH.link.hazardRate(1.9);
            double test3 = defaultTSH.link.hazardRate(2.9);
            double test4 = defaultTSH.link.hazardRate(3.9);
            double test5 = defaultTSH.link.hazardRate(4.9);
            double test7 = defaultTSH.link.hazardRate(6.9);
            double test10 = defaultTSH.link.hazardRate(9.9);
            double test15 = defaultTSH.link.hazardRate(14.9);
            double test20 = defaultTSH.link.hazardRate(19.9);
            double test30 = defaultTSH.link.hazardRate(29.5);

            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(underlying));
            //Handle<YieldTermStructure> flatTermStructure = new Handle<YieldTermStructure>(new FlatForward(settlementDate, riskFreeRate, dayCounter));
            Handle<YieldTermStructure> flatDividendTS = new Handle<YieldTermStructure>(new FlatForward(settlementDate, dividendYield, dayCounter));
            Handle<BlackVolTermStructure> flatVolTS = new Handle<BlackVolTermStructure>(new BlackConstantVol(settlementDate, calendar, volatility, dayCounter));

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

            //HazardRateCurve = myHazard = new HazardRateCurve(datesVol,flatTermStructure)


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

            BlackVarianceSurface mySurface = new BlackVarianceSurface(settlementDate,
                                                                      calendar,
                                                                      datesVol,
                                                                      strikesVol,
                                                                      blackVolMatrix,
                                                                      dayCounter);

            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(mySurface);



            GeneralizedBlackScholesProcessTolerance bsmProcessVolSurface = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatTermStructure, mySurfaceH);



            ////////////////  INSTRUMENT  //////////////////////////////////////////////


            // Autocall instrument //////////////////////////////////////////

            GeneralizedBlackScholesProcessTolerance bsmProcess = new GeneralizedBlackScholesProcessTolerance(underlyingH, flatDividendTS, flatTermStructure, flatVolTS);


            List<Date> fixingdates = new InitializedList<Date>();
            double coupon = 0.05;
            double barrierlvl = 0.6;
            for (int i = 1; i <= 4; i++)
                fixingdates.Add(settlementDate + new Period(i, TimeUnit.Years));

            GenericAutocall myGenericAutocall = new GenericAutocall(fixingdates, coupon, barrierlvl, underlying);
            IPricingEngine mcengine = new MakeMGenericInstrument<PseudoRandom>(bsmProcessVolSurface)
                                                                            .withAbsoluteTolerance(0.5)
                                                                            .withStepsPerYear(52)
                                                                            .withSeed(50)
                                                                            .value();
            myGenericAutocall.setPricingEngine(mcengine);

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