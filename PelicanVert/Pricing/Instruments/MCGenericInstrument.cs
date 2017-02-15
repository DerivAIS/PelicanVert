


///////  Marc RAYGOT - 2017   ///////





using System.Collections.Generic;
using System.Linq;
using System;

namespace QLNet
{
      ////////////////////////////////////////////////////////////////////////////////////
     ///////////////////////////////////  ENGINE MAKER   ////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

        #region MonteCarlo
    public class MakeMGenericInstrument<RNG> where RNG : IRSG, new()
        {
          protected StochasticProcess1D process_;
          protected bool antithetic_;
          protected int? steps_, stepsPerYear_, samples_, maxSamples_;
          protected double? tolerance_;
          protected bool brownianBridge_;
          protected ulong seed_;

          public MakeMGenericInstrument(StochasticProcess1D process)
          {
             process_ = process;
             antithetic_ = true;
             steps_ = null;
             stepsPerYear_ = null;
             samples_ = null;
             maxSamples_ = null;
             tolerance_ = null;
             brownianBridge_ = false;
             seed_ = 0;


          }

          // named parameters


          public MakeMGenericInstrument<RNG> withSteps(int steps)
          {
              steps_ = steps;
              return this;
          }
          public MakeMGenericInstrument<RNG> withStepsPerYear(int steps)
          {
              stepsPerYear_ = steps;
              return this;
          }
          public MakeMGenericInstrument<RNG> withSamples(int samples)
          {
              Utils.QL_REQUIRE(tolerance_ == null, () => "tolerance already set");
              samples_ = samples;
              return this;
          }
          public MakeMGenericInstrument<RNG> withAbsoluteTolerance(double tolerance)
          {
              Utils.QL_REQUIRE(samples_ == null, () => "number of samples already set");
              Utils.QL_REQUIRE(new RNG().allowsErrorEstimate != 0, () =>
                 "chosen random generator policy does not allow an error estimate");
              tolerance_ = tolerance;
              return this;
          }
          public MakeMGenericInstrument<RNG> withMaxSamples(int samples)
          {
              maxSamples_ = samples;
              return this;
          }
          public MakeMGenericInstrument<RNG> withSeed(ulong seed)
          {
              seed_ = seed;
              return this;
          }
          public MakeMGenericInstrument<RNG> withBrownianBridge(bool brownianBridge = true)
          {
              brownianBridge_ = brownianBridge;
              return this;
          }


          // conversion to pricing engine
          public IPricingEngine value()
          {
              return new MCGenericInstrument<RNG>(process_, 
                                           antithetic_, 
                                           steps_, 
                                           stepsPerYear_, 
                                           samples_, 
                                           maxSamples_, 
                                           tolerance_, 
                                           brownianBridge_, 
                                           seed_);
          }


        }

          ////////////////////////////////////////////////////////////////////////////////////
         ///////////////////////////////////  ENGINE   //////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////

        public class MCGenericInstrument<RNG> : MCGenericInstrument<RNG, GenericInstrument.MCGenericEngineBase>
            where RNG : IRSG, new()
        {
            public MCGenericInstrument(StochasticProcess1D process,
                    bool antithetic,
                    int? steps,
                    int? stepsPerYear,
                    int? samples,
                    int? maxSamples,
                    double? tolerance,
                    bool brownianBridge,
                    ulong seed)
                : base(process, antithetic, steps, stepsPerYear, samples, maxSamples, tolerance, brownianBridge, seed) { }
        }


        public class MCGenericInstrument<RNG, GenericEngine> : McSimulation<SingleVariate, RNG, Statistics>, IPricingEngine
            where GenericEngine : IPricingEngine, new()
            where RNG : IRSG, new()
        {
            protected StochasticProcess1D process_;
            private bool antithetic_;
            private int? steps_, stepsPerYear_, samples_, maxSamples_;
            private double? tolerance_;
            private bool brownianBridge_;
            private ulong seed_;


            public MCGenericInstrument(StochasticProcess1D process, 
                                bool antithetic, 
                                int? steps, 
                                int? stepsPerYear,
                                int? samples,
                                int? maxSamples,
                                double? tolerance,
                                bool brownianBridge,
                                ulong seed)
                : base(false, false)
            {
             process_ = process;
             antithetic_ = antithetic;
             steps_ = steps;
             stepsPerYear_ = stepsPerYear;
             samples_ = samples;
             maxSamples_ = maxSamples;
             tolerance_ = tolerance;
             brownianBridge_ = brownianBridge;
             seed_ = seed;

             Utils.QL_REQUIRE(steps != null || stepsPerYear != null, () => "no time steps provided");
             Utils.QL_REQUIRE(steps == null || stepsPerYear == null, () =>
                        "both time steps and time steps per year were provided");
             if (steps != null)
                 Utils.QL_REQUIRE(steps > 0, () => "timeSteps must be positive, " + steps + " not allowed");
             if (stepsPerYear != null)
                 Utils.QL_REQUIRE(stepsPerYear > 0, () =>
                 "timeStepsPerYear must be positive, " + stepsPerYear + " not allowed");


             process_.registerWith(update);
            }

    #endregion
         public virtual void calculate()
         {
            base.calculate(tolerance_, samples_, maxSamples_);
            results_.value = mcModel_.sampleAccumulator().mean();
            results_.samples = (double)mcModel_.sampleAccumulator().samples();
            //results_.additionalResults = mcModel_.sampleAccumulator().
            //results_.additionalResults.Add("test", 0.1);
            if (new RNG().allowsErrorEstimate != 0)
                results_.errorEstimate = mcModel_.sampleAccumulator().errorEstimate();
        }

      

        #region TimeGrid
            // McSimulation implementation
        protected override TimeGrid timeGrid()
        {
            Dictionary<string,List<Date>> datesDico = arguments_.datesDico;
            List<string> keyList = new List<string>();
            keyList = new List<string>(datesDico.Keys);

            Date maturity = arguments_.maturity;
            List<double> mandatorytime = new InitializedList<double>();

            foreach (string Key in keyList)
            {
                foreach (Date date in datesDico[Key])
                {
                    mandatorytime.Add(process_.time(date));
                }
            }
                
            mandatorytime.Sort();
            
            for (int i = 1 ; i < mandatorytime.Count; i++)
            {
                if (mandatorytime[i] == mandatorytime[i-1])
                    mandatorytime.Remove(mandatorytime[i]);
            }
            
            double t = process_.time(maturity);
            int steps = (int)(stepsPerYear_ * t);

            return new TimeGrid(mandatorytime, mandatorytime.Count, steps);
        }

        #endregion

        #region PathPricer
        protected override IPathGenerator<IRNG> pathGenerator()
        {
            int dimensions = process_.factors();
            TimeGrid grid = timeGrid();
            IRNG generator = (IRNG)new RNG().make_sequence_generator(dimensions * (grid.size() - 1), seed_);
            return new PathGenerator<IRNG>(process_, grid, generator, brownianBridge_);
        }


         ///////////////////////////////////  PATH PRICER   /////////////////////////////////


         protected override PathPricer<IPath> pathPricer()
        {
           
            Dictionary<string,List<double>> indexDico = arguments_.indexDico;
            Dictionary<string,List<Date>> datesDico = arguments_.datesDico;
            Func<Dictionary<string, List<double>>, Dictionary<string, List<double>>, Handle<YieldTermStructure>, Path, double> scriptDico = arguments_.scriptDico;
            
            Dictionary<string, List<double>> timesDico = new Dictionary<string, List<double>>();
            List<double> tempTime = new InitializedList<double>();

            GeneralizedBlackScholesProcess process = process_ as GeneralizedBlackScholesProcess;


            List<string>  keyList = new List<string>(datesDico.Keys);

            foreach (string Key in keyList)
            {
                tempTime = new InitializedList<double>();
                foreach (Date date in datesDico[Key])
                {
                    tempTime.Add(process_.time(date));
                }
                timesDico.Add(Key,tempTime);
                
            }

            return new GenericInstrumentPathPricer(scriptDico, timesDico, indexDico, process.riskFreeRate());
        }



        public class GenericInstrumentPathPricer : PathPricer<IPath>
        {


            protected Dictionary<string,List<double>> timesDico_;
            protected Dictionary<string,List<double>> indexDico_;
            protected Func<Dictionary<string,List<double>>, Dictionary<string,List<double>>, Handle<YieldTermStructure>, Path, double> scriptDico_;

            protected Handle<YieldTermStructure> discountTS_;
            

            public GenericInstrumentPathPricer(Func<Dictionary<string,List<double>>, Dictionary<string,List<double>>, Handle<YieldTermStructure>, Path, double> scriptDico,
                                               Dictionary<string,List<double>> timesDico, 
                                               Dictionary<string,List<double>> indexDico, 
                                               Handle<YieldTermStructure> discountTS)
                {
                    timesDico_ = timesDico;         
                    indexDico_ = indexDico;
                    discountTS_ = discountTS;
                    scriptDico_ = scriptDico;
                }


                 public double value(IPath path)
                {
                    return scriptDico_(timesDico_,indexDico_,discountTS_,path as Path);
                }

        }
        #endregion
        /*
        protected override double controlVariateValue()
        {
            AnalyticHestonHullWhiteEngine controlPE = controlPricingEngine() as AnalyticHestonHullWhiteEngine;
            if (controlPE == null)
                throw new Exception("engine does not provide control variation pricing engine");

            OneAssetOption.Arguments controlArguments = controlPE.getArguments() as VanillaOption.Arguments;
            if (controlArguments == null)
                throw new Exception("engine is using inconsistent arguments");

            controlPE.setupArguments(arguments_);
            controlPE.calculate();

            OneAssetOption.Results controlResults = controlPE.getResults() as VanillaOption.Results;
            if (controlResults == null)
                throw new Exception("engine returns an inconsistent result type");

            return controlResults.value.GetValueOrDefault();
        }*/


        #region PricingEngine

        protected GenericInstrument.Arguments arguments_ = new GenericInstrument.Arguments();
        protected GenericInstrument.Results results_ = new GenericInstrument.Results();

        public IPricingEngineArguments getArguments() { return arguments_; }
        public IPricingEngineResults getResults() { return results_; }
        public void reset() { results_.reset(); }

        // observable interface
        private readonly WeakEventSource eventSource = new WeakEventSource();
        public event Callback notifyObserversEvent
        {
            add { eventSource.Subscribe(value); }
            remove { eventSource.Unsubscribe(value); }
        }

        public void registerWith(Callback handler) { notifyObserversEvent += handler; }
        public void unregisterWith(Callback handler) { notifyObserversEvent -= handler; }
        protected void notifyObservers()
        {
            eventSource.Raise();
        }

        public void update() { notifyObservers(); }
      
        #endregion
    }
}
