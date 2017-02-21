

///////  Marc RAYGOT - 2017   ///////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;


namespace HestonAutocall
{
    class HestonAutocall
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
            double underlying = 4468.17;
            Handle<Quote> underlyingH = new Handle<Quote>(new SimpleQuote(underlying));

            // riskfree
            double riskFreeRate = 0.035;
            Handle<YieldTermStructure> flatTermStructure = new Handle<YieldTermStructure>(new FlatForward(settlementDate, riskFreeRate, dayCounter));

            // dividend
            double dividendYield = 0.0;
            double fixedDiv = 5.0;

            Handle<YieldTermStructure> flatDividendTS = new Handle<YieldTermStructure>(new FlatForward(settlementDate, dividendYield, dayCounter));
            Handle<YieldTermStructure> FixedDivTermStructure = new Handle<YieldTermStructure>(new FixedForward(settlementDate, fixedDiv, underlying, dayCounter));

            // vol surface
            //List<Date> datesVol = new InitializedList<Date>();
            //List<double> strikesVol = new InitializedList<double>();

            Date StartDateVol = settlementDate + new Period(1, TimeUnit.Months);


            List<int> maturityInDays = new InitializedList<int>() { 0, 13, 41, 90, 165, 256, 345, 524, 703 };
            List<Date> datesVol = new InitializedList<Date>();
            for (int d = 1; d < maturityInDays.Count; d++)
                datesVol.Add(calendar.advance(settlementDate, new Period(maturityInDays[d], TimeUnit.Days)));

            List<double> strikes = new InitializedList<double>() { 3400, 3600, 3800, 4000, 4200, 4400, 4500, 4600, 4800, 5000, 5200, 5400, 5600 };

            Matrix blackVolMatrix = new Matrix(maturityInDays.Count - 1, strikes.Count, 0.2);
            var vols = new InitializedList<double>() {
            0.6625, 0.4875, 0.4204, 0.3667, 0.3431, 0.3267, 0.3121, 0.3121,
            0.6007, 0.4543, 0.3967, 0.3511, 0.3279, 0.3154, 0.2984, 0.2921,
            0.5084, 0.4221, 0.3718, 0.3327, 0.3155, 0.3027, 0.2919, 0.2889,
            0.4541, 0.3869, 0.3492, 0.3149, 0.2963, 0.2926, 0.2819, 0.2800,
            0.4060, 0.3607, 0.3330, 0.2999, 0.2887, 0.2811, 0.2751, 0.2775,
            0.3726, 0.3396, 0.3108, 0.2781, 0.2788, 0.2722, 0.2661, 0.2686,
            0.3550, 0.3277, 0.3012, 0.2781, 0.2781, 0.2661, 0.2661, 0.2681,
            0.3428, 0.3209, 0.2958, 0.2740, 0.2688, 0.2627, 0.2580, 0.2620,
            0.3302, 0.3062, 0.2799, 0.2631, 0.2573, 0.2533, 0.2504, 0.2544,
            0.3343, 0.2959, 0.2705, 0.2540, 0.2504, 0.2464, 0.2448, 0.2462,
            0.3460, 0.2845, 0.2624, 0.2463, 0.2425, 0.2385, 0.2373, 0.2422,
            0.3857, 0.2860, 0.2578, 0.2399, 0.2357, 0.2327, 0.2312, 0.2351,
            0.3976, 0.2860, 0.2607, 0.2356, 0.2297, 0.2268, 0.2241, 0.2320};


            for (int i = 0; i < vols.Count; i++)
            {
                int testraw = (int)(i % (datesVol.Count));
                int testcol = (int)(i / (datesVol.Count));

                blackVolMatrix[testraw, testcol] = vols[i];

            }



            BlackVarianceSurface mySurface = new BlackVarianceSurface(settlementDate, calendar, datesVol,
                                                        strikes, Matrix.transpose(blackVolMatrix), dayCounter);

            Handle<BlackVolTermStructure> mySurfaceH = new Handle<BlackVolTermStructure>(mySurface);


            ////////////////  CALIBRATION  //////////////////////////////////////////////

            Period helperPeriod = new Period();

            //helpers
            List<CalibrationHelper> calibrationHelpers = new List<CalibrationHelper>();
            for (int k = 0; k < strikes.Count; k++)
            {
                for (int d = 0; d < datesVol.Count; d++)
                {
                    //int testc = datesVol[4]- settlementDate ;
                    //helperPeriod = new Period((int)(((double)maturityInDays[d] + 3.0) / 7.0), TimeUnit.Weeks);
                    helperPeriod = new Period(datesVol[d] - settlementDate, TimeUnit.Days);
                    calibrationHelpers.Add(new HestonModelHelper(helperPeriod,
                                                                 calendar,
                                                                 underlying,
                                                                 strikes[k],
                                                                 new Handle<Quote>(new SimpleQuote(blackVolMatrix[d, k])),
                                                                 flatTermStructure,
                                                                 flatDividendTS,
                                                                 CalibrationHelper.CalibrationErrorType.ImpliedVolError));

                }
            }


            // starting data
            double v0 = 0.1;
            double kappa = 1.0;
            double theta = 0.1;
            double sigma = 0.5;
            double rho = -0.5;

            // model
            HestonProcess hestonProcess = new HestonProcess(flatTermStructure,
                                                            flatDividendTS,
                                                            underlyingH,
                                                            v0, kappa, theta, sigma, rho);

            HestonModel hestonmodel = new HestonModel(hestonProcess);

            AnalyticHestonEngine analyticHestonEngine = new AnalyticHestonEngine(hestonmodel);


            foreach (HestonModelHelper hmh in calibrationHelpers)
                hmh.setPricingEngine(analyticHestonEngine);


            // optimization
            double tolerance = 1.0e-8;
            LevenbergMarquardt optimizationmethod = new LevenbergMarquardt(tolerance, tolerance, tolerance);
            hestonmodel.calibrate(calibrationHelpers, optimizationmethod, new EndCriteria(400, 40, tolerance, tolerance, tolerance));

            double error = 0.0;
            List<double> errorList = new InitializedList<double>();


            ////////////////  CALIBRATION RESULTS  //////////////////////////////////////////////
            Console.WriteLine("Calbration :");
            Console.WriteLine("-----------");

            foreach (HestonModelHelper hmh in calibrationHelpers)
            {
                error += Math.Abs(hmh.calibrationError());
                errorList.Add(Math.Abs(hmh.calibrationError()));
            }

            Vector hestonParameters = hestonmodel.parameters();

            Console.WriteLine("v0    = {0:0.00%}", hestonParameters[4]);
            Console.WriteLine("kappa = {0:0.00%}", hestonParameters[1]);
            Console.WriteLine("theta = {0:0.00%}", hestonParameters[0]);
            Console.WriteLine("sigma = {0:0.00%}", hestonParameters[2]);
            Console.WriteLine("rho   = {0:0.00%}", hestonParameters[3]);
            Console.WriteLine();
            Console.WriteLine("Total error = {0:0.0000}", error);
            Console.WriteLine("Mean error  = {0:0.0000%}", error / (errorList.Count - 1));
            Console.WriteLine();

            int StepsPerYear = 52;
            double absoluteTolerance = 2.0;
            ulong mcSeed = 42;

            // MC Heston process
            HestonProcess calibratedHestonProcess = new HestonProcess(flatTermStructure,
                                                           flatDividendTS,
                                                           underlyingH,
                                                           hestonParameters[4],
                                                           hestonParameters[1],
                                                           hestonParameters[0],
                                                           hestonParameters[2],
                                                           hestonParameters[3]);

            // BS process
            GeneralizedBlackScholesProcessTolerance bsmProcess = new GeneralizedBlackScholesProcessTolerance(underlyingH, FixedDivTermStructure, flatTermStructure, mySurfaceH);

            ////////////////  ENGINES  /////////////////////////////////////////////////


            IPricingEngine mcHestonEngine = new MakeMCEuropeanHestonEngine<PseudoRandom, Statistics>(calibratedHestonProcess)
                                .withStepsPerYear(StepsPerYear)
                                .withAbsoluteTolerance(absoluteTolerance)
                                .withSeed(mcSeed)
                                .getAsPricingEngine();

            double absoluteTolerance2 = 0.1;
            IPricingEngine mcGenHestonEngine = new MakeMGenericHestonInstrument<PseudoRandom>(calibratedHestonProcess)
                               .withStepsPerYear(StepsPerYear)
                               .withAbsoluteTolerance(absoluteTolerance2)
                               .withSeed(mcSeed)
                               .value();

            double absoluteTolerance3 = 0.1;
            IPricingEngine mcengine = new MakeMGenericInstrument<PseudoRandom>(bsmProcess)
                                                                .withAbsoluteTolerance(absoluteTolerance3)
                                                                .withStepsPerYear(52)
                                                                .withSeed(50)
                                                                .value();


            ////////////////  PRICING  //////////////////////////////////////////////
            Console.WriteLine("Pricing Vanilla:");
            Console.WriteLine("---------------");


            Date maturity = new Date(17, Month.May, 2019);
            Exercise europeanExercise = new EuropeanExercise(maturity);

            Option.Type type = Option.Type.Call;
            double strike = underlying;
            StrikedTypePayoff payoff = new PlainVanillaPayoff(type, strike);
            VanillaOption europeanOption = new VanillaOption(payoff, europeanExercise);

            // heston
            europeanOption.setPricingEngine(analyticHestonEngine);
            Console.Write("Heston pricing = {0:0.0000}", europeanOption.NPV());
            Console.WriteLine("  ->   {0:0.0000%}", europeanOption.NPV() / underlying);

            // Mc heston
            europeanOption.setPricingEngine(mcHestonEngine);
            Console.Write("HestMC pricing = {0:0.0000}", europeanOption.NPV());
            Console.Write("  ->   {0:0.0000%}", europeanOption.NPV() / underlying);
            Console.WriteLine("  tolerance   {0:0.0} / {1:0.00%}", absoluteTolerance, absoluteTolerance / underlying);

            // analytic bs
            europeanOption.setPricingEngine(new AnalyticEuropeanEngine(bsmProcess));
            Console.Write("BS pricing     = {0:0.0000}", europeanOption.NPV());
            Console.WriteLine("  ->   {0:0.0000%}", europeanOption.NPV() / underlying);


            Console.WriteLine();
            ////////////////  AUTOCALL HESTON //////////////////////////////////////////////

            List<Date> fixingdates = new InitializedList<Date>();
            double coupon = 0.05;
            double barrierlvl = 0.6;
            for (int i = 1; i <= 4; i++)
                fixingdates.Add(settlementDate + new Period(i, TimeUnit.Years));

            HestonGenericAutocall myGenericAutocallHT = new HestonGenericAutocall(fixingdates, coupon, barrierlvl, underlying);

            myGenericAutocallHT.setPricingEngine(mcGenHestonEngine);

            ////////////////  AUTOCALL BS //////////////////////////////////////////////



            for (int i = 1; i <= 4; i++)
                fixingdates.Add(settlementDate + new Period(i, TimeUnit.Years));

            GenericAutocall myGenericAutocallBS = new GenericAutocall(fixingdates, coupon, barrierlvl, underlying);

            myGenericAutocallBS.setPricingEngine(mcengine);




            ////////////////  Printing Results  //////////////////////////////////////////////
            Console.WriteLine("Pricing Autocall BS:");
            Console.WriteLine("--------------------");
            Console.WriteLine("NPV Generic Autocall = {0:0.0000}", myGenericAutocallBS.NPV());
            Console.WriteLine("Err = {0:0.0000%}", myGenericAutocallBS.errorEstimate() / myGenericAutocallBS.NPV());
            Console.WriteLine("Samples = {0}", myGenericAutocallBS.samples());
            Console.Write("\n");

            for (int i = 0; i < 4; i++)
                Console.WriteLine("ProbaCall {1} = {0:0.0000%}", myGenericAutocallBS.inspout("ProbaCall " + i), i + 1);
            Console.WriteLine("ProbaMid = {0:0.0000%}", myGenericAutocallBS.inspout("ProbaMid"));
            Console.WriteLine("probaDown = {0:0.0000%}", myGenericAutocallBS.inspout("ProbaDown"));
            Console.WriteLine("AvgDown/Proba = {0:0.0000%}", myGenericAutocallBS.inspout("AvgDown") / myGenericAutocallBS.inspout("ProbaDown"));

            Console.Write("\n");

            Console.WriteLine("Pricing Autocall Heston:");
            Console.WriteLine("-----------------------");
            Console.WriteLine("NPV Generic Autocall = {0:0.0000}", myGenericAutocallHT.NPV());
            Console.WriteLine("Err = {0:0.0000%}", myGenericAutocallHT.errorEstimate() / myGenericAutocallHT.NPV());
            Console.WriteLine("Samples = {0}", myGenericAutocallHT.samples());
            Console.Write("\n");

            for (int i = 0; i < 4; i++)
                Console.WriteLine("ProbaCall {1} = {0:0.0000%}", myGenericAutocallHT.inspout("ProbaCall " + i), i + 1);
            Console.WriteLine("ProbaMid = {0:0.0000%}", myGenericAutocallHT.inspout("ProbaMid"));
            Console.WriteLine("probaDown = {0:0.0000%}", myGenericAutocallHT.inspout("ProbaDown"));
            Console.WriteLine("AvgDown/Proba = {0:0.0000%}", myGenericAutocallHT.inspout("AvgDown") / myGenericAutocallHT.inspout("ProbaDown"));

            Console.Write("\n");






            ////////////////  END TEST  //////////////////////////////////////////////
            Console.WriteLine();
            Console.WriteLine(" \nRun completed in {0}", DateTime.Now - timer);
            Console.WriteLine();

            Console.Write("Press any key to continue ...");
            Console.ReadKey();



        }
    }
}