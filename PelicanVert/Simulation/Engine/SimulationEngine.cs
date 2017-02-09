using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO.Markit;

using QLNet;

namespace QLyx.Simulation
{
    public class SimulationEngine<T> : ISimulationEngine where T : SimulationInstrument
    {


        // ************************************************************
        // PROPERTIES  
        // ************************************************************

        // Properties -- Facilitation
        public Dictionary<DateTime, bool> _valuationCalendar = new Dictionary<DateTime, bool>();
        public BusinessDayConvention _bdc() { return _parameters.businessDayConvention(); }
        public DayCounter _dayCounter() { return _parameters.dayCounter(); }

        protected DateTime startDate() { return _parameters.startDate(); }
        protected DateTime endDate() { return _parameters.endDate(); }


        // Properties - Core
        protected InvestmentStrategy<AutocallHelper> _strategy;
        protected SimulationParameters _parameters;
        

        // ************************************************************
        // CONSTRUCTORS  
        // ************************************************************

        protected SimulationEngine(SimulationParameters parameters)
        {

            // Set the parameters for this simulation
            _parameters = parameters;

            // Perform local checks for dates consistency
            SetStartDate(_parameters.startDate());
            SetEndDate(_parameters.endDate());
            
        }


        // ************************************************************
        // DATES MANAGEMENT   
        // ************************************************************

        protected void SetStartDate(DateTime startDate)
        {
            // Adjust end date for calendar
            DateTime _startDate = _parameters.calendar().adjust(startDate, _parameters.businessDayConvention());

            if ((_startDate - startDate).TotalDays != 0)
            {
                Console.WriteLine("Warning : start date adjusted to ", _startDate);
                _parameters.SetStartDate(_startDate);
            }
        }

        protected void SetEndDate(DateTime endDate)
        {
            // Adjust end date for calendar
            DateTime _endDate = _parameters.calendar().adjust(endDate, _parameters.businessDayConvention());

            if ((_endDate - endDate).TotalDays != 0)
            {
                Console.WriteLine("Warning : start date adjusted to ", _endDate);
                _parameters.SetEndDate(_endDate);
            }

            // Sanity check
            if ((_endDate - _parameters.startDate()).TotalDays < 1)
            {
                throw new ArgumentException("DateMismatch", "End date must be on or after start date.");
            }

        }


        // Number of valuation days in the simulation
        public int observations() {
            return _parameters.dayCounter().dayCount(_parameters.startDate(), _parameters.endDate()) + 1;
        }

    }

    
}
