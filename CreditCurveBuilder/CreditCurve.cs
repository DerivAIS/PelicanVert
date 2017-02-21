

///////  Marc RAYGOT - 2017   ///////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;


namespace PelicanVert
{
    class CreditCurveBuilder
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

           
            
            // Build Rate Curve /////////////////////////////////////
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

            RateHelper zc1y = new DepositRateHelper(zc1yRate,new Period(1, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc2y = new DepositRateHelper(zc2yRate, new Period(2, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc3y = new DepositRateHelper(zc3yRate,new Period(3, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc4y = new DepositRateHelper(zc4yRate,new Period(4, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc5y = new DepositRateHelper(zc5yRate,new Period(5, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc7y = new DepositRateHelper(zc7yRate,new Period(7, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc10y = new DepositRateHelper(zc10yRate,new Period(10, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc15y = new DepositRateHelper(zc15yRate,new Period(15, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc20y = new DepositRateHelper(zc20yRate,new Period(20, TimeUnit.Years), fixingDays,
                                             calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);
            RateHelper zc30y = new DepositRateHelper(zc30yRate,new Period(30, TimeUnit.Years), fixingDays,
                                            calendar, BusinessDayConvention.ModifiedFollowing,true, zcBondsDayCounter);

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

           
            
            // Build Credit Curve //////////////////////////////////////

            CreditDefaultSwapHelper cds1Y = new CreditDefaultSwapHelper(0.0003, new Period(1, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds2Y = new CreditDefaultSwapHelper(0.0009, new Period(2, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds3Y = new CreditDefaultSwapHelper(0.0015, new Period(3, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds4Y = new CreditDefaultSwapHelper(0.0021, new Period(4, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds5Y = new CreditDefaultSwapHelper(0.0028, new Period(5, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds7Y = new CreditDefaultSwapHelper(0.0043, new Period(7, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds10Y = new CreditDefaultSwapHelper(0.0061, new Period(10, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds15Y = new CreditDefaultSwapHelper(0.0063, new Period(15, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds20Y = new CreditDefaultSwapHelper(0.0068, new Period(20, TimeUnit.Years), zcTermStructureH);
            CreditDefaultSwapHelper cds30Y = new CreditDefaultSwapHelper(0.0066, new Period(30, TimeUnit.Years), zcTermStructureH);
        
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

            Console.WriteLine("Hazard and Discount Term Structures");
            Console.WriteLine(" \n");
            int width = 12;
            Console.Write("{0,-" + width + "}", "Tenor(y)");
            Console.Write("{0,-" + width + "}", "Hazard");
            Console.WriteLine("{0,-" + width + "}", "Discount");

            double[] tenors = { 1.0,2.0,3.0,4.0,5.0,6.0,7.0,10.0,15.0,20.0 };  // bug to solve : the last date is not in the availabele univers 
            foreach (double i in tenors)
            {
                Console.Write("{0,-" + width + ":0.00}", i+"Y");
                Console.Write("{0,-"+ width + ":0.00%}", defaultTSH.link.hazardRate(i));
                Console.WriteLine("{0,-" + width + ":0.00%}", zcTermStructureH.link.discount(i));
            }

            
            
            // End test
            Console.WriteLine(" \nRun completed in {0}", DateTime.Now - timer);
            Console.WriteLine();

            Console.Write("Press any key to continue ...");
            Console.ReadKey();

        }
    }
}