using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using QLNet;

namespace QLyx.Simulation
{

    public class SimulationParameters : ISimulationParameters
    {


        // ************************************************************
        // PROPERTIES    
        // ************************************************************

        // Properties - Calendar
        public Calendar calendar() { return _calendar; }
        protected Calendar _calendar = new NullCalendar();
        
        public DayCounter dayCounter() { return _dayCounter; }
        protected readonly DayCounter _dayCounter = new Actual365Fixed();

        public BusinessDayConvention businessDayConvention() { return _bdc; }
        protected readonly BusinessDayConvention _bdc = BusinessDayConvention.Unadjusted;


        // Properties - Portfolio and Allocation
        public InitializationMethod initMethod() { return _initMethod; }
        protected readonly InitializationMethod _initMethod = InitializationMethod.Forward;

        public Period rebalPeriod() { return _rebalPeriod; }
        protected readonly Period _rebalPeriod = new Period(1, TimeUnit.Days);

        public Period rampUpRebalPeriod() { return _rampUpRebalPeriod; }
        protected readonly Period _rampUpRebalPeriod = new Period(1, TimeUnit.Days);


        // Properties - Dates
        public DateTime startDate() { return _startDate; }
        protected DateTime _startDate;

        public DateTime endDate() { return _endDate; }
        protected DateTime _endDate;

        public DateTime valuationDate() { return _valuationDate; }
        protected DateTime _valuationDate;



        // Properties - Financial
        public bool shortsAllowed() { return _shortsAllowed; }
        protected bool _shortsAllowed;

        public int maxNumberInstruments() { return _MaxNumberInstruments; }
        protected readonly int _MaxNumberInstruments = 10;

        public int settlementDelay() { return _settlementDelay; }
        protected readonly int _settlementDelay = 3;

 


        // ************************************************************
        // CONSTRUCTORS      
        // ************************************************************
        // public SimulationParameters() { }

        public SimulationParameters(DateTime startDate, DateTime endDate, Period rebalancingPeriod, Period rampUpRebalPeriod, Calendar calendar, 
            DayCounter dayCounter, BusinessDayConvention businessDayConvention, int maxNumberInstruments, bool shortsAllowed, InitializationMethod initMethod)
        {
            // Dates
            _startDate = startDate;
            _endDate = endDate;

            // Calendar & Conventions
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = businessDayConvention;

            // Rebalancing
            _rebalPeriod = rebalancingPeriod;
            _rampUpRebalPeriod = rampUpRebalPeriod;

            // Portfolio composition
            _MaxNumberInstruments = maxNumberInstruments;
            _shortsAllowed = shortsAllowed;
            _initMethod = initMethod;

        }



        // ************************************************************
        // DATES AND CALENDAR MANAGEMENT   
        // ************************************************************

        public void SetEndDate(DateTime newEndDate)
        {
            Console.WriteLine("Warning: Simulation end date changed to {0}.", newEndDate);
            _endDate = newEndDate;
        }

        public void SetStartDate(DateTime newStartDate)
        {
            Console.WriteLine("Warning: Simulation start date changed to {0}.", newStartDate);
            _startDate = newStartDate;
        }

        public void Date(DateTime newStartDate)
        {
            _startDate = newStartDate;
        }
    }




    public enum InitializationMethod {

        // Build the instruments portfolio as the simulation evolves
        Forward = 0,

        // Build the instruments portfolio as if the instruments 
        // had already been traded in the past. 
        //
        // Warning : Forces strategy start date to be the earliest 
        //           of the instrument creation. Data must be available.
        // 
        Backward = 1,

        // Artificially adds DAILY rebalancing dates at the beginning
        // of the simulation to perform a (fast) ramp-up of the portfolio
        DailyRampUp,

        // Artificially adds WEEKLY rebalancing dates at the beginning
        // of the simulation to perform a (fast) ramp-up of the portfolio
        WeeklyRampUp

    }




}
