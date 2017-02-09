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
    public class AutocallPosition : InvestmentPosition<myAutocall>
    {

        // ************************************************************
        // PROPERTIES      
        // ************************************************************

        // Properties 
        protected DateTime _lastConstatationDate;
        protected int _settlementDelay; // not used yet

        // Instrument specific parameter set
        public AutocallHelper helper() { return _helper; }
        protected AutocallHelper _helper;


        // ************************************************************
        // CONSTRUCTOR      
        // ************************************************************

        // Constructors
        public AutocallPosition(myAutocall autocallInstrument, AutocallHelper autocallHelper, Guid identification) 
            : this(autocallInstrument, autocallHelper, false, identification) { }


        public AutocallPosition(myAutocall autocallInstrument, AutocallHelper autocallHelper, bool shortAllowed, Guid identification) 
            : base(autocallInstrument, shortAllowed, identification)
        {
            _helper = autocallHelper;
        }


        // ************************************************************
        // METHODS      
        // ************************************************************

        // Method
        public bool isRedemption(DateTime d)
        {
            return _instrument.isMaturity(d);
        }

        public double RedemptionValue(double underlyingLevel)
        {
            return _instrument.MaturityValue(underlyingLevel);
        }

        public bool isRecallDate(DateTime d)
        {
            return _instrument.isRecallDate(d);
        }

        public bool isRecall(double spot, DateTime d)
        {
            return _instrument.isRecall(spot, d);
        }

        public double RecallPayment(double underlyingLevel, DateTime date)
        {
            return _instrument.RecallPayment(underlyingLevel, date);
        }

        public double NPV(DateTime pricingDate, MarkitSurface marketData)
        {

            IPricingEngine engine = helper().Engine(pricingDate, marketData);
            _instrument.setPricingEngine(engine);
            double npv = _instrument.NPV();

            Console.WriteLine("          >> NPV : {0}", npv);
            //Console.WriteLine("          >> NPV : {0}", npv.ToString("P", System.Globalization.CultureInfo.InvariantCulture));
            _NPV[pricingDate] = npv;

            return npv;
        }

        public double Parallel_NPV(DateTime pricingDate, MarkitSurface marketData, IPricingEngine engine)
        {

            _instrument.setPricingEngine(engine);
            double npv = _instrument.NPV();

            Console.WriteLine("NPV {0}", npv.ToString("P", System.Globalization.CultureInfo.InvariantCulture));
            _NPV[pricingDate] = npv;

            return npv;
        }
    }
}
