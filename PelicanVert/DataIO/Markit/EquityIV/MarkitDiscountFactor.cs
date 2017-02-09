using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Markit
{
    public class MarkitDiscountFactor : QLyx.Markit.Generic.MarkitTermStructure
    {


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        //public MarkitDiscountFactor() { }


        // Constructor 2 : From given date
        public MarkitDiscountFactor(DateTime pricingDate) : base(pricingDate: pricingDate) { }


        // Constructor 3 : For given date & content
        public MarkitDiscountFactor(DateTime pricingDate, Dictionary<DateTime, Double> data) : base(pricingDate: pricingDate, data: data) { }


        #endregion



        // ************************************************************
        // METHOD (BASE CLASS OVERRIDE) 
        // ************************************************************

        #region
            
        public List<double> ZeroRates()
        {
            List<double> res = new List<double>();
            double yearFrac = 0.0;
            double zeroRate = 0.0;

            foreach (DateTime d in _data.Keys)
            {
                yearFrac = dayCounter_.yearFraction(pricingDate, d);
                zeroRate = -1 * Math.Log(_data[d]) / yearFrac;
                if (yearFrac == 0.0) { zeroRate = 0.0; }

                res.Add(zeroRate);
            }


            return res;
        }


        #endregion


        // ************************************************************
        // METHOD (BASE CLASS OVERRIDE) 
        // ************************************************************

        #region

        public double ZeroRateAtMaturity()
        {
            
            double yearFrac = 0.0;
            double zeroRate = 0.0;

            KeyValuePair<DateTime, double> kvp = _data.LastOrDefault();
            
            yearFrac = dayCounter_.yearFraction(pricingDate, kvp.Key);
            zeroRate = -1 * Math.Log(kvp.Value) / yearFrac;
            if (yearFrac == 0.0) { zeroRate = 0.0; }
            
            return zeroRate;
        }



        public double ZeroRate(DateTime maturityDate)
        {

            double yearFrac = 0.0;
            double zeroRate = 0.0;

            double DF = this[maturityDate];


            yearFrac = dayCounter_.yearFraction(pricingDate, maturityDate);
            zeroRate = -1 * Math.Log(DF) / yearFrac;
            if (yearFrac == 0.0) { zeroRate = 0.0; }

            return zeroRate;
        }


        public double ShortRate()
        {

            double yearFrac = 0.0;
            double shortRate = 0.0;

            KeyValuePair<DateTime, double> kvp = _data.FirstOrDefault();

            yearFrac = dayCounter_.yearFraction(pricingDate, kvp.Key);
            shortRate = -1 * Math.Log(kvp.Value) / yearFrac;
            if (yearFrac == 0.0) { shortRate = 0.0; }

            return shortRate;

        }

        protected override Double Extrapolate(DateTime dt)
        {
            
            double T_extrapol = dayCounter_.yearFraction(_pricingDate, dt);
            double zeroRate = this.ZeroRateAtMaturity();

            return Math.Exp(-1 * zeroRate * T_extrapol);
        }


        #endregion

        // ************************************************************
        // METHODS : EXTRACTING TS DATA FOR QLNET
        // ************************************************************

        #region

        // EXTRACTING RISK FREE TERM STRUCTURE
        public QLyx.DataIO.Markit.QLNet_Results GetZeroRateTermStructure()
        {
            double yearFrac = 0.0;
            Dictionary<DateTime, double> zeroRates = new Dictionary<DateTime, double>();

            foreach (DateTime d in _data.Keys) {
                yearFrac = dayCounter_.yearFraction(pricingDate, d);
                zeroRates[d] = -1 * Math.Log(_data[d]) / yearFrac;
                if (yearFrac == 0.0) { zeroRates[d] = 0.0; }
            }

            return new QLyx.DataIO.Markit.QLNet_Results(zeroRates);
        }


        // EXTRACTING RISK FREE TERM STRUCTURE -- FORWARD
        public QLyx.DataIO.Markit.QLNet_Results GetForwardRateTermStructure()
        {
            
            Dictionary<DateTime, double> fwdRates = new Dictionary<DateTime, double>();
            DateTime date = _data.Keys.FirstOrDefault();
            DateTime prev_date = _data.Keys.FirstOrDefault();

            double yearFrac = dayCounter_.yearFraction(new QLNet.Date(pricingDate), new QLNet.Date(_data.Keys.FirstOrDefault()));
            
            fwdRates[date] = Math.Log(_data.FirstOrDefault().Value) / yearFrac;

            for (int t = 1; t < _data.Count()-1; t++)
            {
                date = _data.Keys.ElementAt(t);
                yearFrac = dayCounter_.yearFraction(new QLNet.Date(prev_date), new QLNet.Date(date));

                double continuous_rate = Math.Log(_data[prev_date] / _data[date]) / yearFrac;
                fwdRates[prev_date] = continuous_rate;
                // fwdRates[prev_date] = (1/yearFrac) * Math.Exp(continuous_rate*yearFrac) - 1.0;
                prev_date = date;
               
            }

            return new QLyx.DataIO.Markit.QLNet_Results(fwdRates);
        }


        #endregion




    }

}
