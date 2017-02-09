using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO.Markit
{
    public class MarkitDividend : QLyx.Markit.Generic.MarkitTermStructure
    {

     

        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        //public MarkitDividend() { }


        // Constructor 2 : From given date
        public MarkitDividend(DateTime pricingDate) : base(pricingDate: pricingDate) { }


        // Constructor 3 : For given date & content
        public MarkitDividend(DateTime pricingDate, Dictionary<DateTime, Double> data) : base(pricingDate: pricingDate, data: data) { }

        // Constructor 4 : From the old Markit format
        public MarkitDividend(DateTime pricingDate, MarkitForward forward, MarkitDiscountFactor DF, double impliedSpot) : this(pricingDate)
        {

            Dictionary<DateTime, Double> data = new Dictionary<DateTime, double>();
            double div_rate_T = 0.0;
            double yearFraction = 0.0;

            foreach (DateTime d in forward.data.Keys)
            {
                yearFraction = dayCounter_.yearFraction(new Date(pricingDate), new Date(d));
                div_rate_T = -1 * Math.Log(DF[d] * forward[d] / impliedSpot) / yearFraction;
                data[d] = div_rate_T;
            }

            if (dayCounter_.yearFraction(new Date(pricingDate), new Date(data.FirstOrDefault().Key)) == 0.0)
            {
                data[pricingDate] = data.ElementAtOrDefault(1).Value;
            }

            base._data = data;

        }


        #endregion


        // ************************************************************
        // METHODS : EXTRACTING TS DATA FOR QLNET
        // ************************************************************

        #region

        public double DivYieldAtMaturity(double impliedSpot, KeyValuePair<DateTime, double> forwardAtMaturity, KeyValuePair<DateTime, double> DF_KVP_AtMaturity)
        {

            if (!(forwardAtMaturity.Key == DF_KVP_AtMaturity.Key))
            {
                throw new System.ArgumentException("MarkitDividend.DivYieldAtMaturity : Dates mismatch exception.");
            }

            double yearFrac = 0.0;
            double divYield = 0.0;
            
            yearFrac = dayCounter_.yearFraction(pricingDate, DF_KVP_AtMaturity.Key);
            divYield = -1 * Math.Log(forwardAtMaturity.Value * DF_KVP_AtMaturity.Value / impliedSpot) / yearFrac;
            if (yearFrac == 0.0) { divYield = 0.0; }

            return divYield;
        }

        

        // EXTRACTING RISK FREE TERM STRUCTURE
        public QLyx.DataIO.Markit.QLNet_Results GetDividendTermStructure()
        {
            
            return new QLyx.DataIO.Markit.QLNet_Results(data);
        }


        public QLNet_Results GetDividendDF_TermStructure(double impliedSpot, Dictionary<DateTime, double> forward, Dictionary<DateTime, double> riskFree_DF)
        {

            double yearFrac = 0.0;
            double DF_divYield = 0.0;

            Dictionary<DateTime, double> res = new Dictionary<DateTime, double>();

            foreach (DateTime d in forward.Keys)
            {

                if (!(riskFree_DF.Keys.Contains(d))) { throw new NotImplementedException(); }
                
                yearFrac = dayCounter_.yearFraction(pricingDate, d);

                // divYield = -1 * Math.Log(forward[d] * riskFree_DF[d] / impliedSpot) / yearFrac;
                DF_divYield = forward[d] * riskFree_DF[d] / impliedSpot;

                if (yearFrac == 0.0) { DF_divYield = 1.0; }
                res[d] = DF_divYield;

            }

            return new QLNet_Results(res);
        }




        public QLNet_Results GetDividendDF_TermStructure_NewFormat()
        {
            Dictionary<DateTime, Double> myData = new Dictionary<DateTime, double>();
            myData[_pricingDate] = 0.0;
            foreach (DateTime dt in _data.Keys) {
                myData[dt] = _data[dt];
            }
           
            return new QLNet_Results(myData);
        }


        #endregion



    }

}
