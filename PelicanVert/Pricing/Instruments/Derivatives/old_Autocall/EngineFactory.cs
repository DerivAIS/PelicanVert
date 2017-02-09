using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;
namespace QLyx.Instruments.Derivatives
{

    //! European option pricing engine using Monte Carlo simulation
    /*! \ingroup vanillaengines

        \test the correctness of the returned value is tested by
              checking it against analytic results.
    */
    public class MCEuropeanAutocallEngine<RNG, S> : MCAutocallEngine<SingleVariate, RNG, S>
        // public class MCEuropeanEngine<RNG, S> : MCVanillaEngine<SingleVariate, RNG, S>
        where RNG : IRSG, new()
        where S : IGeneralStatistics, new()
    {

        // constructor
        // MCEuropeanEngine --> MCEuropeanAutocallEngine
        public MCEuropeanAutocallEngine(GeneralizedBlackScholesProcess process, int timeSteps, int timeStepsPerYear,
                                bool brownianBridge, bool antitheticVariate,
                                int requiredSamples, double requiredTolerance, int maxSamples, ulong seed)
            : base(process, timeSteps, timeStepsPerYear, brownianBridge, antitheticVariate, false,
                   requiredSamples, requiredTolerance, maxSamples, seed)
        { }

        protected override PathPricer<IPath> pathPricer()
        {
            AutocallPayoff payoff = arguments_.payoff as AutocallPayoff;
            if (payoff == null)
                throw new ApplicationException("non-plain payoff given");

            AutocallExercise exercise = arguments_.exercise as AutocallExercise;
            if (payoff == null)
                throw new ApplicationException("non-plain payoff given");

            GeneralizedBlackScholesProcess process = process_ as GeneralizedBlackScholesProcess;
            if (process == null)
                throw new ApplicationException("Black-Scholes process required");

            // return new EuropeanAutocallPathPricer(payoff, exercise, process.riskFreeRate().link.discount(timeGrid().Last()));
            return new EuropeanAutocallPathPricer(payoff, exercise, process); //, process.riskFreeRate().link.discount(timeGrid().Last()));
        }
    }


    //! Monte Carlo European engine factory
    // template <class RNG = PseudoRandom, class S = Statistics>
    public class MakeMCEuropeanAutocallEngine<RNG> : MakeMCEuropeanAutocallEngine<RNG, Statistics> where RNG : IRSG, new()
        // MakeMCEuropeanEngine --> MakeMCEuropeanAutocallEngine
    {
        public MakeMCEuropeanAutocallEngine(GeneralizedBlackScholesProcess process) : base(process) { }
    }

    public class MakeMCEuropeanAutocallEngine<RNG, S> where RNG : IRSG, new() where S : IGeneralStatistics, new()
        // MakeMCEuropeanEngine --> MakeMCEuropeanAutocallEngine
    {
        private GeneralizedBlackScholesProcess process_;
        private bool antithetic_;
        private int steps_, stepsPerYear_, samples_, maxSamples_;
        private double tolerance_;
        private bool brownianBridge_;
        private ulong seed_;

        public MakeMCEuropeanAutocallEngine(GeneralizedBlackScholesProcess process)
        {
            process_ = process;
        }

        // named parameters
        public MakeMCEuropeanAutocallEngine<RNG, S> withSteps(int steps)
        {
            steps_ = steps;
            return this;
        }
        public MakeMCEuropeanAutocallEngine<RNG, S> withStepsPerYear(int steps)
        {
            stepsPerYear_ = steps;
            return this;
        }
        //public MakeMCEuropeanEngine withBrownianBridge(bool b = true);
        public MakeMCEuropeanAutocallEngine<RNG, S> withBrownianBridge(bool brownianBridge)
        {
            brownianBridge_ = brownianBridge;
            return this;
        }
        public MakeMCEuropeanAutocallEngine<RNG, S> withSamples(int samples)
        {
            if (tolerance_ != 0)
                throw new ApplicationException("tolerance already set");
            samples_ = samples;
            return this;
        }
        public MakeMCEuropeanAutocallEngine<RNG, S> withAbsoluteTolerance(double tolerance)
        {
            if (samples_ != 0)
                throw new ApplicationException("number of samples already set");
            if (new RNG().allowsErrorEstimate == 0)
                throw new ApplicationException("chosen random generator policy does not allow an error estimate");
            tolerance_ = tolerance;
            return this;
        }
        public MakeMCEuropeanAutocallEngine<RNG, S> withMaxSamples(int samples)
        {
            maxSamples_ = samples;
            return this;
        }
        public MakeMCEuropeanAutocallEngine<RNG, S> withSeed(ulong seed)
        {
            seed_ = seed;
            return this;
        }
        //public MakeMCEuropeanEngine withAntitheticVariate(bool b = true)
        public MakeMCEuropeanAutocallEngine<RNG, S> withAntitheticVariate(bool b)
        {
            antithetic_ = b;
            return this;
        }

        // conversion to pricing engine
        public IPricingEngine value()
        {
            if (steps_ == 0 && stepsPerYear_ == 0)
                throw new ApplicationException("number of steps not given");
            if (!(steps_ == 0 || stepsPerYear_ == 0))
                throw new ApplicationException("number of steps overspecified");
            return new MCEuropeanAutocallEngine<RNG, S>(process_, steps_, stepsPerYear_, brownianBridge_, antithetic_,
                                               samples_, tolerance_, maxSamples_, seed_);
        }
    }


    public class EuropeanAutocallPathPricer : PathPricer<IPath>
    {
        private AutocallPayoff payoff_;
        private AutocallExercise exercise_;
        private GeneralizedBlackScholesProcess process_;

        public EuropeanAutocallPathPricer(AutocallPayoff autocallPayoff, AutocallExercise autocallExercise, GeneralizedBlackScholesProcess process)
        {
            payoff_ = autocallPayoff;
            exercise_ = autocallExercise;
            //payoff_ = new AutocallPayoff(type, strike);
            process_ = process;
            //if (!(strike >= 0.0))
            //    throw new ApplicationException("strike less than zero not allowed");
        }

        public double value(IPath path) // this needs to be changed, we'll need more than the last value
        {
            if (!(path.length() > 0))
                throw new ApplicationException("the path cannot be empty");
            return payoff_.value(path as Path, exercise_, process_);
            // return payoff_.value((path as Path).back()) * discount_; // --> comes out directly discounted from payoff_.value()
        }
    }

}
