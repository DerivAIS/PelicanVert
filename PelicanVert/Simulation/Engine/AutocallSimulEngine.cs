using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom
using QLNet;

// Internal custom
using QLyx.Instruments.Derivatives;
using QLyx.DataIO.Markit;


namespace QLyx.Simulation
{
    public class AutocallSimulationEngine : SimulationEngine<myAutocall>
    {


        // Properties
        protected MarkitEquityUnderlying _underlying;
        protected Markit_Equity_IV _database;
        protected new AutocallStrategy _strategy;


        // Instrument template
        Type _instrumentTemplate = typeof(myAutocall);


        // Constructor
        /* Old constructor
        public AutocallSimulationEngine(MarkitEquityUnderlying underlying, AutocallStrategy strategy, SimulationParameters parameters) 
            : base(parameters)
        {
            _underlying = underlying;
            _strategy = strategy;
            SetDatabase(underlying);
         
        }
        */

        public AutocallSimulationEngine(double initialLevel, MarkitEquityUnderlying underlying, SimulationParameters simulationParameters, AutocallHelper payoffParameters)
            : base(simulationParameters)
        {
            _underlying = underlying;
            _strategy = new AutocallStrategy(initialLevel, simulationParameters, payoffParameters);

            Initialize(underlying);
        }


        // ************************************************************
        // INITIALIZATION   
        // ************************************************************
        #region Setup & Initialization

        protected void Initialize(MarkitEquityUnderlying underlying)
        {
            // Setup link to Markit Data (aka the database)
            _database = Markit_Equity_IV.Instance(underlying);

            // Fill the rebalancing calendar
            _strategy.SetRebalancing(_parameters.startDate(), _parameters.rebalPeriod(), _parameters.rampUpRebalPeriod(), _parameters.maxNumberInstruments());
            
        }

        #endregion



        // ************************************************************
        // CORE SIMULATION   
        // ************************************************************
        #region Core simulation methods
        public void Run()
        {
            int totalObs = observations();

            _strategy.Initialize(_parameters.startDate());

            for (int i = 0; i <= totalObs; i++)
            {
                // Determine current date
                Date currentDate = _parameters.calendar().adjust(_parameters.startDate().AddDays(i), _parameters.businessDayConvention());
                DateTime currentDateTime = new DateTime(currentDate.year(), currentDate.month(), currentDate.Day);


                DisplayNewSimulationDate(currentDate);

                MarkitSurface markitSurface = _database[currentDateTime];
                Console.WriteLine("Market data found. Starting update...");

                _strategy.Update(currentDate, markitSurface);

            }

            Console.WriteLine("Simualtion completed.");

        }

        #endregion


        // ************************************************************
        // DISPLAY METHODS
        // ************************************************************
        #region Display methods


        protected void DisplayNewSimulationDate(DateTime d)
        {
            Console.WriteLine("");
            Console.WriteLine("*************************************************");
            Console.WriteLine("***** Simulation date : {0}", d);
            Console.WriteLine("*************************************************");
            Console.WriteLine("");
        }


        #endregion

    }
}
