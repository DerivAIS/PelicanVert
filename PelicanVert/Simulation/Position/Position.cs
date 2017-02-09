using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.Simulation
{

    public class InvestmentPosition<T> where T : SimulationInstrument
    {

        // Properties
        protected double _unitPurchasePrice;
        protected double _unitSellPrice;

        protected DateTime _investDate;
        protected DateTime _devestDate;

        protected Dictionary<DateTime, double> _numberUnits = new Dictionary<DateTime, double>();
        protected Dictionary<DateTime, double> _NPV = new Dictionary<DateTime, double>();

        protected T _instrument;
        protected bool _shortAllowed = false;

        protected double tradingCost_uf_fix;
        protected double tradingCost_uf_variable;

        protected Guid _id;



        // Constructors
        public InvestmentPosition(T instrument, Guid identification) : this(instrument, false, identification) { }

        public InvestmentPosition(T instrument, bool shortAllowed, Guid identification)
        {
            _instrument = instrument;
            _shortAllowed = shortAllowed;
            _id = identification;

        }



        // Methods
        public void Buy(DateTime date, double unitPurchasePrice, double numberUnits, bool includesFixCosts, bool includesVariableCosts)
        {
            _investDate = date;
            _unitPurchasePrice = unitPurchasePrice;

            int alreadyIncludesFix = includesFixCosts ? 0 : 1;
            int alreadyIncludesVariable = includesVariableCosts ? 0 : 1;

            _NPV[date] = ((numberUnits * unitPurchasePrice) * (1.0 + alreadyIncludesVariable * tradingCost_uf_variable)) + alreadyIncludesFix * tradingCost_uf_fix;

            if (numberUnits < 0.0 && !_shortAllowed) { throw new System.ArgumentException("ShortNotAllowed", "Shorts not allowed in this DerivativePosition."); }
            _numberUnits[date] = numberUnits;

        }

        public virtual void Buy(DateTime date, double unitPurchasePrice, double numberUnits)
        {
            Console.WriteLine("Trade order (buy) sent without specifying inclusion of trading costs. Adding costs.");
            Buy(date, unitPurchasePrice, numberUnits, false, false);
        }


        public virtual void Sell(DateTime date, double unitSellPrice, double numberUnits, bool includesFixCosts, bool includesVariableCosts)
        {

            int alreadyIncludesFix = includesFixCosts ? 0 : 1;
            int alreadyIncludesVariable = includesVariableCosts ? 0 : 1;

            _devestDate = date;
            _unitSellPrice = unitSellPrice;

            _NPV[date] = ((numberUnits * unitSellPrice) * (1.0 - alreadyIncludesVariable * tradingCost_uf_variable)) - alreadyIncludesFix * tradingCost_uf_fix;

            _numberUnits[date] = _numberUnits.Values.LastOrDefault() - numberUnits;
            if (_numberUnits[date] < 0.0 && !_shortAllowed) { throw new System.ArgumentException("ShortNotAllowed", "Shorts not allowed in this DerivativePosition."); }


        }

        public virtual void Sell(DateTime date, double unitPurchasePrice, double numberUnits)
        {
            Console.WriteLine("Trade order (sell) sent without specifying inclusion of trading costs. Adding costs.");
            Sell(date, unitPurchasePrice, numberUnits, false, false);
        }



        public virtual void Close(DateTime date, double unitSellPrice, bool includesFixCosts, bool includesVariableCosts)
        {
            Sell(date, unitSellPrice, _numberUnits.Values.LastOrDefault(), includesFixCosts, includesVariableCosts);
        }


        public virtual void Close(DateTime date, double unitSellPrice)
        {
            Console.WriteLine("Trade order (close) sent without specifying inclusion of trading costs. Adding costs.");
            Sell(date, unitSellPrice, _numberUnits.Values.LastOrDefault(), false, false);
        }


        public virtual double MostRecentValuation()
        {
            return _NPV.LastOrDefault().Value;
        }
    }
}