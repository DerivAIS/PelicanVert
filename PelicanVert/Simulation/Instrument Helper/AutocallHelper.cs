using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;
using QLyx.Instruments.Derivatives;
using QLyx.DataIO.Markit;

namespace QLyx.Simulation
{
    public class AutocallHelper : InstrumentHelper
    {


        // ************************************************************
        // PROPERTIES   
        // ************************************************************

        // PAYOFF DESCRIPTION

        // Recall Moneyness
        public double recall_Moneyness() { return _recall_Moneyness; }
        protected double _recall_Moneyness = 1.00;
        
        // PDI Moneyness
        public double PDI_moneyness() { return _PDI_moneyness; }
        protected double _PDI_moneyness = 0.60;
        
        // Number of constatation (recall) dates
        public int numberOfConstatations() { return _numberOfConstatations; }
        protected int _numberOfConstatations = 5;

        // Period bewteen consecutive constatation (recall) dates
        public Period constatationPeriod() { return _constatationPeriod; }
        protected Period _constatationPeriod = new Period(1, TimeUnit.Years);

        // Forward Strike Delay
        public Period forwardStrikeDelay() { return _forwardStrikeDelay; }
        protected Period _forwardStrikeDelay = new Period(0, TimeUnit.Days);

        // Nominal
        public double nominal() { return _nominal; }
        protected double _nominal = 1.0;


        // PRICING ENGINE SETTINGS

        // Number of samples in Monte Carlo
        public int samples() { return _samples; }
        protected int _samples = 10000;

        public int timeSteps() { return _timeSteps; }
        protected int _timeSteps = 5;

        public ulong seed() { return _seed; }
        protected ulong _seed = 42;

        public bool antithetic() { return _antitetic; }
        protected bool _antitetic = true;

        public bool brownianBridge() { return _brownianBridge; }
        protected bool _brownianBridge = false;
        
        public double couponTolerance() { return _couponTolerance; }
        protected double _couponTolerance = 0.0001;



        // OTHER PARAMETERS

        // Instrument template corresponding to this parameter set
        public Type instrumentTemplate() { return typeof(myAutocall); }

        // Upfront Margin cost (as percentage of notional)
        public double upfrontMargin() { return _upfrontMargin; }
        protected double _upfrontMargin = 0.025;

        // Calendar and Dates management
        public Calendar calendar() { return _calendar; }
        protected Calendar _calendar = new TARGET();

        // Forward Strike Delay
        public BusinessDayConvention businessDayConvention() { return _businessDayConvention; }
        protected BusinessDayConvention _businessDayConvention = BusinessDayConvention.Unadjusted;



        // HELPER PROPERTIES

        // Current Date
        public DateTime pricingDate() { return _pricingDate; }
        protected DateTime _pricingDate;

        public MarkitSurface currentMarketData() { return _currentMarketData; }
        protected MarkitSurface _currentMarketData;


        // ************************************************************
        // CONSTRUCTORS   
        // ************************************************************

        public AutocallHelper(double nominal, double recall_Moneyness, double PDI_Moneyness, Period constatationPeriod, int numberOfConstatations, double upfrontMargin,
                                    Calendar calendar, BusinessDayConvention businessDayConvention) : base()
        {
            // Nominal
            if (nominal < 0.0) { }
            _nominal = nominal;
            
            // Barriers
            _recall_Moneyness = recall_Moneyness;
            _PDI_moneyness = PDI_Moneyness;

            // Calendar management
            _numberOfConstatations = numberOfConstatations;
            _constatationPeriod = constatationPeriod;

            // Upfront trading margin/cost
            _upfrontMargin = upfrontMargin;

            //Calendar and Dates management
            _calendar = calendar;
            _businessDayConvention = businessDayConvention;
        }


        public AutocallHelper(double recall_Moneyness, double PDI_Moneyness, Period constatationPeriod, int numberOfConstatations, double upfrontMargin,
                            Calendar calendar, BusinessDayConvention businessDayConvention) : this(1.0, recall_Moneyness, PDI_Moneyness, constatationPeriod, 
                                numberOfConstatations, upfrontMargin, calendar, businessDayConvention)
        {
        }

        public AutocallHelper(double recall_Moneyness, double PDI_Moneyness, Period constatationPeriod, int numberOfConstatations, 
                                double upfrontMargin, SimulationParameters simulationParameters) : base()
        {
            // Barriers
            _recall_Moneyness = recall_Moneyness;
            _PDI_moneyness = PDI_Moneyness;

            // Calendar management
            _numberOfConstatations = numberOfConstatations;
            _constatationPeriod = constatationPeriod;

            // Upfront trading margin/cost
            _upfrontMargin = upfrontMargin;

            //Calendar and Dates management
            _calendar = simulationParameters.calendar();
            _businessDayConvention = simulationParameters.businessDayConvention();
        }


        // ************************************************************
        // METHODS   
        // ************************************************************


        // Set current date and data
        protected void SetLocalData(DateTime pricingDate, MarkitSurface marketData)
        {
           if (pricingDate != marketData.informationDate) { Console.WriteLine("Warning : Market data provided does not match pricing date."); }

            _pricingDate = pricingDate;
            _currentMarketData = marketData;
        }


        // Instrument

        public myAutocall Instrument(DateTime d, double notional, MarkitSurface marketData)
        {
            // Display
            Console.WriteLine("Creation of a new instrument requested on {0}", d);

            // Set current date and market data locally
            SetLocalData(d, marketData);
            Console.WriteLine("Local data set with information date {0}", marketData.informationDate);

            // return private method
            return GetAutocall(notional: notional);
        }

        protected myAutocall GetAutocall(double notional)
        {
            // This will sove for the coupon rate returning an upfront value at Par minus upfront fees
            return new myAutocall(pricingDate(), GetRecallSchedule(), GetPayoff(notional));
        }

        protected myAutocall GetAutocall(double notional, double couponRate)
        {
            // This will sove for the coupon rate returning an upfront value at Par minus upfront fees
            return new myAutocall(pricingDate(), GetRecallSchedule(), GetPayoff(notional: notional, couponRate: couponRate));
        }


        // Recall dates
        private AutocallExercise GetRecallSchedule()
        {
            // Declare
            List<Date> recallDates = new List<Date>();

            // Compute Strike Date
            Date strikeDate = calendar().adjust(new Date(_pricingDate) + forwardStrikeDelay(), businessDayConvention());

            // Setup dates
            for (int i = 1; i <= numberOfConstatations(); i++)
            {
                Date recallDate = strikeDate + i * constatationPeriod();
                recallDates.Add(calendar().adjust(recallDate, businessDayConvention()));
            }

            //Return 
            return new AutocallExercise(recallDates);
        }


        // Payoff (solve for coupon rate)
        private AutocallPayoff GetPayoff(double notional)
        {
            // Declare
            double couponRate = 0.0;

            // Solve for coupon rate
            couponRate = SolveCouponRate(upfrontMargin());

            //Return from internal parameters
            return GetPayoff(notional, couponRate);
        }

        // Payoff (solve for coupon rate)
        private AutocallPayoff GetPayoff(double notional, double couponRate)
        {
            //Return from internal parameters
            return new AutocallPayoff(notional, currentMarketData().impliedSpot, couponRate, recall_Moneyness(), PDI_moneyness());
        }


        private double SolveCouponRate(double upfrontMargin = 0.0)
        {

            // Display
            Console.WriteLine("Solving for the coupon rate on {0}", pricingDate());
            double targetPrice = 1.0 - upfrontMargin;


            // Price with high level of coupon (upper bound)
            double highCoupon = 0.10;
            myAutocall AC_high = GetAutocall(1.0, highCoupon);
            AC_high.setPricingEngine(Engine(pricingDate(), currentMarketData()));
            double px_high = AC_high.NPV();
            Console.WriteLine("Found a high price of {0}", px_high.ToString("P", System.Globalization.CultureInfo.InvariantCulture) );


            // price with low level of coupon (lower bound)
            double lowCoupon = 0.00;
            myAutocall AC_low = GetAutocall(1.0, lowCoupon);
            AC_low.setPricingEngine(Engine(pricingDate(), currentMarketData()));
            double px_low = AC_low.NPV();
            Console.WriteLine("Found a low price of {0}", px_low.ToString("P", System.Globalization.CultureInfo.InvariantCulture));

            // Coupon rate linear estimation
            double estimation = lowCoupon + (highCoupon - lowCoupon) / (px_high - px_low) * (targetPrice - px_low);
            Console.WriteLine("Coupon rate estimated at {0}", estimation.ToString("P", System.Globalization.CultureInfo.InvariantCulture));

            // Return
            return estimation;

        }


        protected GeneralizedBlackScholesProcess GetStochasticProcess(DateTime d, MarkitSurface marketData)
        {
            // Spot level
            Handle<Quote> spotLevel = new Handle<Quote>(new SimpleQuote(marketData.impliedSpot));

            // Risk free rate (term structure)
            Handle<YieldTermStructure> riskFreeTS = marketData.riskFree_FwdCrv();

            // Dividend yield (term structure)
            Handle<YieldTermStructure> dividendTS = marketData.dividend_FwdCrv();

            // Volatility ATM Forward (term structure)
            BlackVarianceCurve atmf_vol_curve = marketData.ATMF_Vol_TS(d);
            Handle<BlackVolTermStructure> volatilityTS = new Handle<BlackVolTermStructure>(atmf_vol_curve);

            // Return
            return new GeneralizedBlackScholesProcess(spotLevel, dividendTS, riskFreeTS, volatilityTS);
        }



        protected void SetEngine(myAutocall autocallInstrument, DateTime d, MarkitSurface marketData)
        {

            // Get stochastic process
            var stochasticProcess = GetStochasticProcess(d, marketData);

            // Set properties
            IPricingEngine monteCarloEngine = new MakeMCEuropeanAutocallEngine<PseudoRandom>(stochasticProcess)
                                            .withSteps(timeSteps())
                                            .withAntitheticVariate(antithetic())
                                            .withBrownianBridge(brownianBridge())
                                            .withSamples(samples())
                                            .withSeed(seed())
                                            .value();

            // Apply engine to instrument
            autocallInstrument.setPricingEngine(monteCarloEngine);
        }

        public IPricingEngine Engine(DateTime d, MarkitSurface marketData)
        {

            // Get stochastic process
            var stochasticProcess = GetStochasticProcess(d, marketData);

            // Set properties
            IPricingEngine monteCarloEngine = new MakeMCEuropeanAutocallEngine<PseudoRandom>(stochasticProcess)
                                            .withSteps(timeSteps())
                                            .withAntitheticVariate(antithetic())
                                            .withBrownianBridge(brownianBridge())
                                            .withSamples(samples())
                                            .withSeed(seed())
                                            .value();

            // Apply engine to instrument
            return monteCarloEngine;
        }



    }
}
