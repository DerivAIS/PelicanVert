using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Markit
{
    public class MarkitSmile
    {


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


        // EXPIRY DATE
        #region Expiry Date (as DateTime)
        protected DateTime _expiryDate;
        public DateTime expiryDate
        {
            get
            {
                return _expiryDate;
            }

            protected set
            {
                _expiryDate = value;
            }

        }

        #endregion



        // DATA -- SMILE FOR GIVEN EXPIRY
        #region Data (as Dict of double,double)
        protected Dictionary<Double, Double> _data;
        public Dictionary<Double, Double> data
        {
            get
            {
                if (_data == null)
                {
                    _data = new Dictionary<Double, Double>();
                }
                return _data;
            }

            protected set
            {
                _data = value;
            }

        }


        #endregion


        #endregion




        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        // public MarkitSmile() { }


        // Constructor 2 : From given date
        public MarkitSmile(DateTime pricingDate, DateTime expiryDate)
        {
            this.pricingDate = pricingDate;
            this.expiryDate = expiryDate;
        }


        // Constructor 3 : For given date & content
        public MarkitSmile(DateTime pricingDate, DateTime expiryDate, Dictionary<Double, Double> data)
        {
            this.pricingDate = pricingDate;
            this.expiryDate = expiryDate;
            this.data = data;
        }

        #endregion



        // ************************************************************
        // METHODS : SETTING DATA
        // ************************************************************

        #region

        internal void SetData(DateTime pricingDate, DateTime expiryDate, Double argStrike, Double argVolatility)
        {

            if (DateTime.Compare(pricingDate, this.pricingDate) != 0)
            {
                throw new System.ArgumentException("DateException", "Data point's pricing date does not match this vol smile's pricing date.");
            }

            if (DateTime.Compare(expiryDate, this.expiryDate) != 0)
            {
                throw new System.ArgumentException("DateException", "Data point's expiry date does not match this vol smile's expiry date.");
            }

            if (this.data.ContainsKey(argStrike))
            {
                // Console.WriteLine("Warning : Duplicate strike/moneyness detected in volatility smile. Skipping.");
                return;
            }


            this.data[argStrike] = argVolatility;


        }


        #endregion



        // ************************************************************
        // METHODS : ACCESSING DATA
        // ************************************************************

        #region

        public List<double> Strikes() {
            return _data.Keys.ToList();
        }


        // ACCESS VOLATILITY POINT
        internal Double GetVolatility(Double moneyness)
        {

            if (data.ContainsKey(moneyness)) { return data[moneyness]; }

            else if ((data.Keys.Max() > moneyness) && (data.Keys.Min() < moneyness)) { return Interpolate(moneyness); }

            else if (data.Keys.Max() < moneyness) { return ExtrapolateHigh(moneyness); }

            else { return ExtrapolateLow(moneyness); }
            
        }

        private Double Interpolate(Double moneyness)
        {
            Double prevStrike = Double.NegativeInfinity;
            Double nextStrike = Double.NegativeInfinity;

            foreach (Double d in data.Keys)
            {
                // Check for lower bound
                if (((d - moneyness) < (d - prevStrike)) && (d < moneyness))
                {
                    prevStrike = d;
                }

                // Check for lower bound
                if (((d - moneyness) < (d - nextStrike)) && (d > moneyness))
                {
                    nextStrike = d;
                }

            }


            if ((prevStrike < Double.PositiveInfinity) && (nextStrike < Double.PositiveInfinity))
            {
                return LinearStrikeInterpolation(moneyness, prevStrike, nextStrike);
            }

            else
            {
                throw new System.ArgumentException("DataUnavailable", "Markit Smile cannot compute volatility interpolation.");
            }
        
        }

        private Double LinearStrikeInterpolation(double moneyness, double prevStrike, double nextStrike)
        {
            return data[prevStrike] + (data[nextStrike] - data[prevStrike]) / (nextStrike - prevStrike) * (moneyness - prevStrike);
        }

        private double ExtrapolateLow(double moneyness)
        {
            Console.WriteLine("Warning: Extrapolation towards low moneyness not implemented. Returning lowest volatility value.");
            return data[data.Keys.Min()];
        }

        private double ExtrapolateHigh(double moneyness)
        {
            Console.WriteLine("Warning: Extrapolation towards high moneyness not implemented. Returning highest volatility value.");
            return data[data.Keys.Max()];
        }

        #endregion


        // ************************************************************
        // INDEXOR 
        // ************************************************************

        #region

        public Double this[Double moneyness]
        {

            get
            {
                if (moneyness > 5.0) { moneyness *= 0.01; }
                return GetVolatility(moneyness);
            }


        }

        #endregion


    }
}
