using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO.Markit;

using QLNet;

namespace QLyx.Simulation
{
    public class InvestmentStrategy<T> : IStrategy where T : InstrumentHelper
    {


        // ************************************************************
        // PROPERTIES  
        // ************************************************************

        // Generic Simulation parameter set
        protected SimulationParameters _simulationParameters;

        // Instrument specific parameter set
        protected InstrumentHelper _instrumentParameters;

        // Rebalancing dates
        protected List<DateTime> _rebalCalendar = new List<DateTime>() { };




        // ************************************************************
        // CONSTRUCTORS  
        // ************************************************************


        // Constructors
        public InvestmentStrategy(SimulationParameters simulationParameters, InstrumentHelper parameters)
        {
            _simulationParameters = simulationParameters;
        }



        // ************************************************************
        // METHODS  
        // ************************************************************


        // Initialize : Should be overridden by child classes
        public virtual void Initialize(DateTime date)
        {
            throw new NotImplementedException();
        }


        // Fill the Rebalancing calendar
        public virtual void SetRebalancing(DateTime firstRebalDate, Period normalPeriod, Period rampUpPeriod, int maxNumberInstruments)
        {

            // Set the normal calendar for rebalancing
            SetRebalancing_Normal(firstRebalDate, normalPeriod);

            if (rampUpPeriod != null)
            {
                SetRebalancing_RampUp(firstRebalDate, rampUpPeriod, maxNumberInstruments);
            }

        }


        // Fill the Rebalancing calendar (no special treatment of ramp-up period)
        public virtual void SetRebalancing(DateTime firstRebalDate, Period normalPeriod)
        {
            SetRebalancing(firstRebalDate, normalPeriod, null, 0);
        }


        // Fill the Rebalancing calendar (list of dates) 
        // *** NORMAL / STEADY-STATE ***
        public virtual void SetRebalancing_Normal(DateTime firstRebalDate, Period period)
        {

            TimeUnit timeUnit = period.units();
            int length = period.length();

            DateTime currentDate = firstRebalDate;
            DateTime localEndDate = _simulationParameters.endDate();

            int k = 0;

            while (currentDate < localEndDate)
            {
                Date proposedDate = new Date(firstRebalDate) + new Period(length * k, timeUnit);
                Date adjustedDate = _simulationParameters.calendar().adjust(proposedDate, _simulationParameters.businessDayConvention());
                DateTime rebalDateTime = new DateTime(adjustedDate.year(), adjustedDate.month(), adjustedDate.Day);

                // Add to the calendar
                if (!_rebalCalendar.Contains(rebalDateTime)) {
                    _rebalCalendar.Add(rebalDateTime);
                }

                // Increment
                currentDate = adjustedDate;
                k++;
            }

            if (currentDate > _simulationParameters.endDate())
            {
                _simulationParameters.SetEndDate(currentDate);
            }
        }


        // Fill the Rebalancing calendar (list of dates)
        // *** RAMP UP PERIOD ONLY ***
        public virtual void SetRebalancing_RampUp(DateTime firstRebalDate, Period period, int numberInstruments)
        {

            TimeUnit timeUnit = period.units();
            int length = period.length();

            DateTime currentDate = firstRebalDate;
            DateTime localEndDate = _simulationParameters.endDate();

            int k = 0;
            int rampUpPeriods = 0;

            while ((currentDate < localEndDate) && (rampUpPeriods <= numberInstruments))
            {
                Date proposedDate = new Date(firstRebalDate) + new Period(length * k, timeUnit);
                Date adjustedDate = _simulationParameters.calendar().adjust(proposedDate, _simulationParameters.businessDayConvention());
                DateTime rebalDateTime = new DateTime(adjustedDate.year(), adjustedDate.month(), adjustedDate.Day);

                // Add to the calendar
                if (!_rebalCalendar.Contains(rebalDateTime))
                {
                    _rebalCalendar.Add(rebalDateTime);
                    rampUpPeriods++;
                }

                // Increment
                currentDate = adjustedDate;
                k++;
            }

            if (currentDate > _simulationParameters.endDate())
            {
                _simulationParameters.SetEndDate(currentDate);
            }
        }


    }
}
