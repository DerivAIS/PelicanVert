using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.Markit.Generic
{
    public class MarkitTermStructure 
    {

        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        #region

        protected DayCounter dayCounter_ = new Actual365Fixed();
        
        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        #region

        // PRICING DATE
        #region Pricing Date (as DateTime)
        protected DateTime _pricingDate;
        public DateTime pricingDate
        {
            get
            {
                return _pricingDate;
            }

            protected set
            {
                _pricingDate = value;
            }

        }

        #endregion


        // DATA
        #region Data (as Dict of DateTime,double)
        protected Dictionary<DateTime, Double> _data;
        public Dictionary<DateTime, Double> data
        {
            get
            {
                if (_data == null) { _data = new Dictionary<DateTime, Double>(); }
                return _data;
            }

            protected set
            {
                _data = value;
            }

        }

        public List<Date> Dates()
        {
            List<Date> dates = new List<Date>();
            foreach (DateTime d in _data.Keys) { dates.Add(new Date(d)); }
            return dates;
        }

        public List<double> Points()
        {
            List<double> res = new List<double>();
            foreach (DateTime d in _data.Keys) { res.Add(_data[d]); }
            return res;
        }


        #endregion


        #endregion



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public MarkitTermStructure() { }


        // Constructor 2 : From given date
        public MarkitTermStructure(DateTime pricingDate)
        {
            this.pricingDate = pricingDate;
        }


        // Constructor 3 : For given date & content
        public MarkitTermStructure(DateTime pricingDate, Dictionary<DateTime, Double> data)
        {
            this.pricingDate = pricingDate;
            this.data = data;
        }


        #endregion



        // ************************************************************
        // METHODS 
        // ************************************************************

        #region

        internal void SetData(DateTime pricingDate, DateTime expiryDate, double thisDataPoint)
        {

            if (DateTime.Compare(pricingDate, this.pricingDate) != 0)
            {
                throw new System.ArgumentException("DateException", "Data point's pricing date does not match this term structure's pricing date.");
            }

            if (data.ContainsKey(expiryDate))
            {
                //Console.WriteLine("Warning: Duplicate data point detected while constructing term structure.");
                //throw new System.ArgumentException("DateException", "Data point already present in this term structure.");
                return;
            }

            data[expiryDate] = thisDataPoint;

        }

        #endregion

        // At Maturity KVP
        public KeyValuePair<DateTime, double> AtMaturity()
        {
            return _data.LastOrDefault();
        }


        // ACCESS DATA POINT
        internal Double GetDataPoint(DateTime dt)
        {

            if (data.ContainsKey(dt)) { return data[dt]; }

            else if ((data.Keys.Max() > dt) && (data.Keys.Min() < dt)) { return Interpolate(dt); }

            else if (data.Keys.Max() < dt) { return Extrapolate(dt); }

            else { return data[data.Keys.Min()]; }

        }


        private Double Interpolate(DateTime dt)
        {

            DateTime prevStrike = DateTime.MinValue;
            DateTime nextStrike = DateTime.MinValue;

            foreach (DateTime d in data.Keys)
            {
                // Check for lower bound
                if (((d - dt).TotalDays < (d - prevStrike).TotalDays) && (d < dt))
                {
                    prevStrike = d;
                }

                // Check for upper bound
                if (((d - dt).TotalDays < (d - nextStrike).TotalDays) && (d > dt))
                {
                    nextStrike = d;
                }

            }


            if ((prevStrike != DateTime.MinValue) && (nextStrike != DateTime.MinValue))
            {
                return LinearStrikeInterpolation(dt, prevStrike, nextStrike);
            }

            else
            {
                throw new System.ArgumentException("DataUnavailable", "Markit Smile cannot compute volatility interpolation.");
            }

        }


        private Double LinearStrikeInterpolation(DateTime dt, DateTime prevStrike, DateTime nextStrike)
        {
            return data[prevStrike] + (data[nextStrike] - data[prevStrike]) / (nextStrike - prevStrike).TotalDays * (dt - prevStrike).TotalDays;
        }



        protected virtual Double Extrapolate(DateTime dt)
        {
            return data[data.Keys.Max()];
        }



        // ************************************************************
        // METHODS : EXTRACTING TS DATA FOR QLNET
        // ************************************************************

        #region

        // EXTRACTING RISK FREE TERM STRUCTURE
        internal QLyx.DataIO.Markit.QLNet_Results QLNet_TS()
        {
            return new QLyx.DataIO.Markit.QLNet_Results(_data);
        }


        // EXTRACTING RISK FREE TERM STRUCTURE
        internal double ElementAt(int k)
        {
            return data.Values.ElementAt(k);
        }


        #endregion



        // ************************************************************
        // INDEXOR 
        // ************************************************************

        #region Indexor

        public Double this[DateTime dt]
        {

            get
            {
                return GetDataPoint(dt);
            }


        }

        #endregion

        
    }
}
