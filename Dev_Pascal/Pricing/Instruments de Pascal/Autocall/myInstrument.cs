using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace Pascal.Instruments.Derivatives
{
    public class myAutocall : QLyx.Simulation.SimulationInstrument
    {
        // ************************************************************
        // ***** PROPERTIES *****
        // ************************************************************

        protected double? delta_, gamma_, theta_, vega_, rho_;

        protected double? recallLevel_, finalBarrierLevel_, CouponRate_, spotAtStrikeDate_, Coefficient_;

        public AutocallPayoff payoff() { return payoff_; }
        protected AutocallPayoff payoff_;
                
        public AutocallExercise exercise() { return exercise_; }
        protected AutocallExercise exercise_;



        // ************************************************************
        // **** CONSTRUCTORS ****
        // ************************************************************

        public myAutocall() : base() { }

        public myAutocall(Date valuationDate, AutocallExercise exercise, AutocallPayoff payoff) : base()
        {

            // Assign dates
            base.valuationDate_ = valuationDate;
            Settings.setEvaluationDate(valuationDate);

            // Assign payoff and exercise
            exercise_ = exercise;
            payoff_ = payoff;

            // @TODO : valuation date chould not be here, this is not part of the instrument itself. Belongs to valuation methods... @TODO

        }



        // ************************************************************
        // ****** METHODS -- OVERRIDDEN METHODS
        // ************************************************************

        #region QLNet methods 

        /// <summary>  
        ///  Attaches the Monte Carlo pricing engine to the autocall instrument.
        /// </summary> 
        new public void setPricingEngine(IPricingEngine e)
        {
            if (engine_ != null) engine_.unregisterWith(update);
            engine_ = e;
            if (engine_ != null) engine_.registerWith(update);

            update(); 
        }

        /// <summary>  
        ///  Performs the valuation of the instruments. Returns Net Present Value (NPV).
        /// </summary> 
        new public double NPV()
        {              
            calculate();
            if (NPV_ == null) throw new ArgumentException("NPV not provided");
            return NPV_.GetValueOrDefault();
        }


        /// <summary>  
        ///  Calls the valuation methods from base the class.
        /// </summary> 
        protected override void calculate()
        {
            if (isExpired())
            {
                setupExpired();
                calculated_ = true;
            }
            else
            {
                base.calculate();
            }
        }

        /// <summary>  
        ///  Calls the valuation methods from base the class.
        /// </summary> 
        public override bool isExpired() { return exercise_.lastDate() < Settings.evaluationDate(); }

        protected override void performCalculations()
        {
            if (engine_ == null) throw new ArgumentException("No pricing engine set.");

            engine_.reset();
            setupArguments(engine_.getArguments());

            engine_.getArguments().validate();
            engine_.calculate();
            fetchResults(engine_.getResults());
        }

        /// <summary>  
        ///  Setup pricing arguments (Payoff and Exercise objects).
        /// </summary> 
        public override void setupArguments(IPricingEngineArguments args)
        // public override void setupArguments(IPricingEngineArguments args)
        {
            myAutocall.Arguments arguments = args as myAutocall.Arguments;

            if (arguments == null)
                throw new ApplicationException("wrong argument type");

            arguments.payoff = payoff_;
            arguments.exercise = exercise_;
        }


        /// <summary>  
        ///  Sets the pricing results locally.
        /// </summary> 
        public override void fetchResults(IPricingEngineResults r)
        {
            base.fetchResults(r);

            Results results = r as Results;
            if (results == null)
                throw new ApplicationException("No greeks returned from pricing engine");
            
            delta_ = results.delta;
            gamma_ = results.gamma;
            theta_ = results.theta;
            vega_ = results.vega;
            rho_ = results.rho;
            
        }

        #endregion


        #region Methods necessary for simulations

        public bool isMaturity(DateTime d)
        {
            DateTime lastDate = new DateTime(exercise().lastDate().serialNumber());
            if (d == lastDate) { return true; }
            return false;
        }


        public double MaturityValue(double spot)
        {
            Console.WriteLine("Deterministic payoff check : maturity / redemption payment on {0}");

            double yield = spot / payoff_.spotAtStrikeDate();
            int nbSteps = exercise_.dates().Count();

            if (yield >= payoff_.recallMoneyness()) { return payoff_.nominal() * (1 + (1 + nbSteps) * payoff_.CouponRate()); }
            else if (yield >= payoff_.finalBarrierMoneyness()) { return payoff_.nominal(); }
            else { return payoff_.nominal() * yield; }
        }

        #endregion



        // ************************************************************
        // METHODS -- DETERMINISTIC CHECKS RELATING TO PAYOFFS
        // ************************************************************


        #region Deterministic checks (for simulations)

        public double RecallPayment(double spot, DateTime d)
        {
            Console.WriteLine("Deterministic payoff check : coupon payment on {0} given recall.", d);

            double yield = spot / payoff_.spotAtStrikeDate();
            int elapsedPeriods = exercise_.dates().IndexOf(d);

            return payoff_.nominal() * (1 + (1 + elapsedPeriods) * payoff_.CouponRate()); 
        }


        public bool isRecallDate(DateTime d)        
        {
            DateTime thisDate;

            foreach (Date dd in exercise_.dates())
            {
                //thisDate = new DateTime(dd.serialNumber());
                thisDate = new  DateTime(dd.year(), dd.month(), dd.Day);
                if (d == thisDate) { return true; }
            }

            return false;
        }


        public bool isRecall(double spot, DateTime d)
        {
            if ((spot / payoff_.spotAtStrikeDate()) >= payoff_.recallMoneyness()) { return true; }
            return false;
        }

        #endregion


        // ************************************************************
        // ****** SUB CLASSES : ARGUMENTS, RESULTS & ENGINE
        // ************************************************************

        //! basic %option %arguments
        public class Arguments : IPricingEngineArguments
        {
            public AutocallPayoff payoff;

            public AutocallExercise exercise;

            public Arguments() { }

            public virtual void validate()
            {
                if (payoff == null) throw new ApplicationException("no redemption payoff given");
                if (exercise == null) throw new ApplicationException("no exercise given");
            }
            
        }
        

        //! %Results from single-asset option calculation
        public new class Results : Instrument.Results           // Added the new to hide object below 
        {
            public double? delta, gamma, theta, vega, rho, dividendRho;
            public double? itmCashProbability, deltaForward, elasticity, thetaPerDay, strikeSensitivity;

            public override void reset()
            {
                base.reset();

                // Greeks::reset();
                delta = gamma = theta = vega = rho = dividendRho = null;

                // MoreGreeks::reset();
                //itmCashProbability = deltaForward = elasticity = thetaPerDay = strikeSensitivity = null;
            }
        }


        public class Engine : GenericEngine<myAutocall.Arguments, myAutocall.Results>
        {
        }
    }
}
