using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO.Markit;
using QLyx.Instruments.Derivatives;

using QLNet;

namespace QLyx.Simulation
{
    public class AutocallStrategy : MarkitStrategy<AutocallHelper>
    {

        // ************************************************************
        // PROPERTIES    
        // ************************************************************

        // Initial Strategy Level
        public double initialLevel() { return _initialLevel; }
        protected double _initialLevel = 100.0;


        // NPV : MtM value of the strategy
        protected Dictionary<DateTime, double> _NPV = new Dictionary<DateTime, double>(); // ------------------> THIS IS NOT FILLED AS VALUED ARE PRODUCED @TODO

        
        // Cash Account : Represents the amount of cash (not invested) available
        protected CashManager _bankAccount;
        

        // Portfolio
        protected Dictionary<Guid, AutocallPosition> _activePositions = new Dictionary<Guid, AutocallPosition>();
        protected Dictionary<Guid, AutocallPosition> _closedPositions = new Dictionary<Guid, AutocallPosition>();
        protected List<Guid> _positionsToBeClosed = new List<Guid>();


        // Ramp-up properties
        protected bool isRampUpPeriod = true;


        // Instrument specific parameter set
        protected AutocallHelper _instrumentHelper;


        // ************************************************************
        // CONSTRUCTORS      
        // ************************************************************

        // Constructors
        public AutocallStrategy(double initialLevel, SimulationParameters simulationParameters, 
            AutocallHelper instrumentHelper) : base(simulationParameters, instrumentHelper)
        {
            // Set the rebalancing calendar
            _rebalCalendar.Add(_simulationParameters.startDate());
            _rebalCalendar.Add(_simulationParameters.endDate());

            // Set instrument helper
            _instrumentHelper = instrumentHelper;

            // Set the initial strategy level as an initial amount of cash
            _bankAccount = new CashManager(_simulationParameters.startDate(), initialLevel, 0.0);

        }



        // ************************************************************
        // INITIALIZATION AND RAMP-UP        
        // ************************************************************

        #region Initialization and ramp-up period management

        public override void Initialize(DateTime date)
        {
            // do something, sanity checks, fill forward/backward, etc.
        }


        protected void RampUp(DateTime date, MarkitSurface marketData)
        {

            // Count number of active positions
            int openPositions = _activePositions.Count();

            // Check to see if ramp-up is over.
            if (openPositions == _simulationParameters.maxNumberInstruments())
            {
                isRampUpPeriod = false;
                Console.WriteLine("Ramp-up period completed on : {0}", date);
                return;
            }

            // Check if slot is available for new investment
            if (openPositions < _simulationParameters.maxNumberInstruments())
            {

                // Display
                Console.WriteLine("Number of positions available for investment: {0}", _simulationParameters.maxNumberInstruments() - openPositions);

                // Always invest only once in ramp-up period
                Console.WriteLine("Investing...");
                Invest(date, Weight(date),  marketData);
            }

            // This case should never be reached... means we're above max nb of positions
            else
            {
                throw new ArgumentException("RampUpException", "Ramp up failure.");
            }
        }

        #endregion



        // ************************************************************
        // ASSET ALLOCATION 
        // ************************************************************

        #region Weight calculation for rebalancing

        public double Weight(DateTime date) // <-----------------------------------------> THIS NEEDS TO BE IMPLEMENTED PROPERLY !!  @ TODO
        {

            double cash_NPV = _bankAccount.currentBalance();
            double investment_NPV = 0.0;

            foreach (KeyValuePair<Guid, AutocallPosition> kvp in _activePositions)
            {
                investment_NPV += kvp.Value.MostRecentValuation();
            }

            double totalValue = investment_NPV + cash_NPV;
            Console.WriteLine("TOTAL STRATEGY VALUE : {0}", totalValue);

            return totalValue  / _simulationParameters.maxNumberInstruments();
        }


        #endregion



        // ************************************************************
        // INVESTMENT 
        // ************************************************************

        #region Investment after ramp-up

        public virtual void Update(DateTime date, MarkitSurface marketData)
        {

            Console.WriteLine("     Updating a portfolio of {0} autocalls.", _activePositions.Count());

            // Update the cash interest rate
            _bankAccount.UpdateRate(date, marketData.ShortRate());

            // 1. Regarder si on arrive à maturité sur un instrument. 
            // Si oui, se demander si on restrike et dans quelles conditions
            foreach (KeyValuePair<Guid, AutocallPosition> deriv in _activePositions)
            {
                // Case 1 : At Maturity
                if (deriv.Value.isRedemption(date))
                {
                    Event_Maturity(date, deriv, marketData.impliedSpot);
                }

                // Case 2 : Not Maturity, but on a coupon date
                else if (deriv.Value.isRecallDate(date))
                {
                    Event_Recall(date, deriv, marketData);
                }

                // Case 3 : MtM valuations of existing products
                else
                {
                    // Valuation is handle as a single block later
                    // NoEvent(date, deriv, marketData);
                }
            }

            // 2. Clean up the active positions
            CleanUp();

            // 3. Value all remaining products
            ComputeNPV(date, marketData);
            

            // 4. Réinvestir (aux dates de rebalancement)
            if (_rebalCalendar.Contains(date))
            {

                // Display
                Console.WriteLine("          >> Rebalancing date detected: {0}.", date);
                
                // During ramp-up
                if (isRampUpPeriod)
                {
                    Console.WriteLine("          >> Ramp-up period found on {0}.", date);
                    RampUp(date, marketData);
                }

                // After ramp-up
                else
                {
                    Allocate(date, marketData);
                }
            }
        }


        protected void CleanUp()
        {
            foreach (Guid id in _positionsToBeClosed)
            {

                // Add to closed positions
                _closedPositions[id] = _activePositions[id];

                // Delete from active positions
                _activePositions.Remove(id);

            }

            // Reset list
            _positionsToBeClosed = new List<Guid>();

        }



        protected void ComputeNPV(DateTime date, MarkitSurface marketData)
        {
            Console.WriteLine("          >> Performing valuation for {0} autocalls.", _activePositions.Count());

            foreach (KeyValuePair<Guid, AutocallPosition> kvp in _activePositions)
            {
                kvp.Value.NPV(date, marketData);
            }
            
        }

        protected void Parallel_ComputeNPV(DateTime date, MarkitSurface marketData)
        {
            int k = 1;

            Console.WriteLine("Parallel Update of the NPV called:");

            List<AutocallPosition> positions = _activePositions.Values.ToList();
            Console.WriteLine("     >> Found {0} positions in portfolio.", _activePositions.Count());

            if (_activePositions.Count() > 0)
            {
                Parallel.ForEach(positions, (pos) => // foreach (KeyValuePair<Guid, AutocallPosition> kvp in _activePositions)
                {
                    Console.WriteLine("     >> Computing NPV for position: {0}", k);
                    IPricingEngine engine = _instrumentHelper.Engine(date, marketData);

                    pos.Parallel_NPV(date, marketData, engine);
                });
            }
        }


        protected void Allocate(DateTime date, MarkitSurface markitSurface)
        {

            // Count number of active positions
            int openPositions = _activePositions.Count();

            // Compute the weight
            double weight = Weight(date);

            // Check if slot is available for new investment
            if (openPositions < _simulationParameters.maxNumberInstruments())
            {
                // Count the number of slots available 
                int newInvestments = _simulationParameters.maxNumberInstruments() - openPositions;

                for(int k=0; k<newInvestments; k++)
                {
                    Invest(date, weight, markitSurface);
                }
            }
        }

        
        protected void Invest(DateTime date, double notional, MarkitSurface marketData)
        {

            // Get a new product for investment on date
            myAutocall newProduct = _instrumentHelper.Instrument(date, notional, marketData);
            Console.WriteLine("New autocall product constructed.");

            // Withdrawing from bank account to fund investment
            _bankAccount.Transaction(date, -1 * notional);

            // Add new investment to active positions
            Guid id = Guid.NewGuid();
            _activePositions[id] = new AutocallPosition(newProduct, _instrumentHelper ,id);
            Console.WriteLine("New autocall product added to the active positions under GUID: {0}", id);

        }
        
        #endregion



        // ************************************************************
        // EVENTS HANDLING
        // ************************************************************

        #region Events handling (coupons, maturity, etc.)

        protected void NoEvent(DateTime date, KeyValuePair<Guid, AutocallPosition> kvp, MarkitSurface marketData)
        {
            kvp.Value.NPV(date, marketData);
        }


        protected void Event_Recall(DateTime date, KeyValuePair<Guid, AutocallPosition> kvp, MarkitSurface marketData)
        {
            double spot = marketData.impliedSpot;
            // No recall on this recall (observation) date
            if (!kvp.Value.isRecall(spot, date)) {
                NoEvent(date, kvp, marketData);
            }

            // At this point the instrument is being recalled
            double pmt = 0.0;
            pmt = kvp.Value.RecallPayment(spot, date);

            // Adding the cash to the bank account
            _bankAccount.Transaction(date, pmt);

            // Close position
            kvp.Value.Close(date, pmt, true, true);

            // Add to the list of positions to be terminated
            _positionsToBeClosed.Add(kvp.Key);


        }
        

        protected void Event_Maturity(DateTime date, KeyValuePair<Guid, AutocallPosition> kvp, double spotLevel)
        {

            // Compute final payment
            double pmt = kvp.Value.RedemptionValue(spotLevel);
            DateTime settlementDate = date.AddDays(_simulationParameters.settlementDelay()); 

            // Adding the cash to the bank account
            _bankAccount.Transaction(settlementDate, pmt);

            // Close position
            kvp.Value.Close(date, pmt, true, true);

            // Add to the list of positions to be terminated
            _positionsToBeClosed.Add(kvp.Key);

        }


        #endregion

        

    }
}




