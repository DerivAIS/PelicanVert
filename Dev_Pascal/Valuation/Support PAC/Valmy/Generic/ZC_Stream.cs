using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

using QLyx.InterestRates;
using QLyx.Utilities;


namespace Pascal.Valuation
{

    public class EUR_Coupon_Stream
    {

        // ************************************************************
        // PROPERTIES - UNDERLYING
        // ************************************************************

        #region Rate curve

        // Date
        protected DateTime _valuationDate;

        // Rate curve to perform calculations
        protected string _curveDescription;

        // Coupon schedule
        protected Dictionary<DateTime, double> _couponSchedule = new Dictionary<DateTime, double>();

        // Discount Factors
        protected Dictionary<DateTime, double> _DF = new Dictionary<DateTime, double>();

        // Individual PV
        protected Dictionary<DateTime, double> _PV = new Dictionary<DateTime, double>();
        
        // Default Period for rate curve --> vs EUR3M
        protected Period _defaultPeriod = new Period(3, TimeUnit.Months);

        #endregion



        // ************************************************************
        // CONSTRUCTOR
        // ************************************************************


        public EUR_Coupon_Stream(List<double> couponList, List<DateTime> dateList)
        {
            if (couponList.Count() != dateList.Count()) { throw new ArgumentException("CouponDateMismatch", "Wrong number of coupons or dates provided."); }

            int k = 0;

            foreach (DateTime dt in dateList) {
                _couponSchedule[dt] = couponList.IndexOf(k);
                k++;
            }
        }

        public EUR_Coupon_Stream(double couponRate, List<DateTime> dateList)
        {
            foreach (DateTime dt in dateList)
            {
                _couponSchedule[dt] = couponRate;
            }
        }

        public EUR_Coupon_Stream(Dictionary<DateTime, double> couponSchedule)
        {
            _couponSchedule = couponSchedule;
        }


        // ************************************************************
        // METHODS
        // ************************************************************
        /*
        protected void SetRateCurve(Period period_)
        {
            // Check for valuation date
            if (_valuationDate == null ||_valuationDate == DateTime.MinValue)
            { throw new ArgumentException("DateException", "Valuation date not set. Unable to set rate curve."); }

            // Extract the key info
            int nb = period_.length();
            TimeUnit tu = period_.units();

            // Case 1 : Against 3 or 6 month floating rates
            if (tu == TimeUnit.Months)
            {
                if (nb == 3)
                {
                    _curve = new myRateCurveEURv3m(); // as myRateCurve;
                }
                else if (nb == 6)
                {
                    _curve = new myRateCurveEUR(); // as myRateCurve;
                }
            }

            // Case 1 : Against EONIA
            else if (tu == TimeUnit.Days)
            {
                if (nb == 1)
                {
                    _curve = new myRateCurveEUROIS(); // as myRateCurve;
                }
            }

            else
            {
                throw new NotImplementedException();
            }
        }
        */

        // Compute the present value (PV)

        // Public
        public double PV_v3m(DateTime valuationDate)
        {
            _curveDescription = "EURIBOR_3M";
            myRateCurveEURv3m rateCurve = new myRateCurveEURv3m(valuationDate);
            return Compute_PV(valuationDate, _defaultPeriod, rateCurve);
        }

        public double PV_v6m(DateTime valuationDate)
        {
            _curveDescription = "EURIBOR_6M";
            myRateCurveEUR rateCurve = new myRateCurveEUR(valuationDate);
            return Compute_PV(valuationDate, _defaultPeriod, rateCurve);
        }

        public double PV_OIS(DateTime valuationDate)
        {
            _curveDescription = "OIS_EONIA";
            myRateCurveEUROIS rateCurve = new myRateCurveEUROIS(valuationDate);
            return Compute_PV(valuationDate, _defaultPeriod, rateCurve);
        }


        // Poretcted

        protected void Compute_PV(DateTime valuationDate, myRateCurve rateCurve)
        {
            Compute_PV(valuationDate, _defaultPeriod, rateCurve);
        }

        protected double Compute_PV(DateTime valuationDate, Period curvePeriod, myRateCurve rateCurve)
        {
            _valuationDate = valuationDate;
            double cumSum = 0.0;

            foreach (DateTime dt in _couponSchedule.Keys)
            {
                _DF[dt] = 0.01 * rateCurve.getDiscountFactor(dt);
                _PV[dt] =  _DF[dt] * _couponSchedule[dt];
                cumSum += _PV[dt];
            }

            return cumSum;

        }


        // Adjust the dates

        protected DateTime Adjust(DateTime dateTime, myRateCurve rateCurve)
        {
            Date d = rateCurve.CurveCalendar.adjust(dateTime.ToDate(), rateCurve.CurveBusinessDayConvention);
            return d.ToDateTime();
        }

        protected List<DateTime> Adjust(List<DateTime> dateList, myRateCurve rateCurve)
        {
            List<DateTime> res = new List<DateTime>();
            foreach (DateTime dt in dateList) {
                res.Add(Adjust(dt, rateCurve));
            }
            return res;
        }



        // PV()


    }
}
