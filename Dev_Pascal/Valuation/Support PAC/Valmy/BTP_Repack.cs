using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.InterestRates;

using QLNet;
using QLyx.Utilities;
using QLyx.DataIO.Markit;

namespace Pascal.Valuation
{
    public class BTP_Repack
    {




        // ************************************************************
        // PROPERTIES -- FINANCIAL
        // ************************************************************

        // Bond and Bond Coupons
        private string _bondIdentification = "EC0659473 Corp";
        private double _couponRate = 0.0525;
        private Period _paymentPeriod = new Period(6, TimeUnit.Months);
        private double _notional = 100.0;
        private double _frequencyAdjustment = 0.5;
        private Dictionary<DateTime, double> _bondSchedule = new Dictionary<DateTime, double>();
        DateTime _bondMaturity = new DateTime(2029, 11, 01);


        // Equity Coupons
        private double _downsideLeverage = 1.43;
        private double _downsideBarrier = 0.70;
        private double _accumulatedCoupons = 0.2053;
        private MarkitEquityUnderlying _MarkitRefUnderlying = MarkitEquityUnderlying.Eurostoxx;


        // Rate Curve
        myRateCurveEURv3m _rateCurve;


        // Date
        private DateTime _valuationDate;
        private Calendar _calendar = new TARGET();
        private BusinessDayConvention _bdc = BusinessDayConvention.ModifiedFollowing;
        private DayCounter _dc = new Actual365Fixed();


        // ************************************************************
        // PROPERTIES -- INFRASTRUCTURE
        // ************************************************************

        // Market Data (Markit)
        protected Markit_Equity_IV _marketData;
        public Markit_Equity_IV marketData() {
            if (_marketData == null) { _marketData = Markit_Equity_IV.Instance(_MarkitRefUnderlying); }
            return _marketData;
        }



        // ************************************************************
        // ACCESSOR 
        // ************************************************************

        public Dictionary<DateTime, double> bondSchedule() {
            if (_bondSchedule.Count() == 0 || _bondSchedule == null)
            {
                SetBondCouponSchedule(_valuationDate);
            }
            return _bondSchedule;
        }


        // ************************************************************
        // CONSTRUCTOR 
        // ************************************************************

        // Generic
        public BTP_Repack() : base() { }

        // With Date
        public BTP_Repack(DateTime valuationDate) : base()
        {
            _valuationDate = valuationDate;
        }

        

        // ************************************************************
        // METHODS -- BOND SCHEDULE
        // ************************************************************


        public void SetBondCouponSchedule(DateTime valuationDate) {

            Date matDate = _bondMaturity.ToDate();
            Date thisDate = matDate;
            int k = 0;

            // Back fill the option sequence from maturity date (included)
            while (thisDate > valuationDate.ToDate())
            {
                thisDate = Adjust(matDate - k * _paymentPeriod);
                _bondSchedule[thisDate] = _couponRate * _frequencyAdjustment;
                // if (thisDate == matDate) { _bondSchedule[thisDate] += 1.0; }
                k++;
            }
        }


        public void SetBondCouponSchedule()
        {
            if (_valuationDate == DateTime.MinValue) { throw new ArgumentException("ValuationDateException", "No valuation date provided."); }
            SetBondCouponSchedule(_valuationDate);
        }

        
        public double NPV_BondCoupons_Markit()
        {

            Date currentDate = _valuationDate.ToDate();
            double yearFrac = 0.0;
            double NPV_BondCoupons = 0.0;
            double discount = 0.0;

            MarkitDiscountFactor DF = marketData()[_valuationDate].DF;

            foreach (DateTime d in bondSchedule().Keys) {
                yearFrac = _dc.yearFraction(currentDate, d.ToDate());
                discount = DF[d];
                NPV_BondCoupons += bondSchedule()[d] * discount;
            }

            return NPV_BondCoupons;
            
        }
        
        public double NPV_BondCoupons_QuantLib()
        {
            Date currentDate = _valuationDate.ToDate();

            EUR_Coupon_Stream cpn = new EUR_Coupon_Stream(_bondSchedule);
            cpn.PV_v3m(currentDate);
            double px = cpn.PV_v3m(currentDate);
            return px;

        }

        public double GetBondPrice()
        {
            return 0.0;
        }


        // ************************************************************
        // METHODS -- DATE ADJUSTMENT
        // ************************************************************

        protected DateTime Adjust(DateTime dateTime)
        {
            Date d = _calendar.adjust(dateTime.ToDate(), _bdc);
            return d.ToDateTime();
        }

        protected Date Adjust(Date date)
        {
            return _calendar.adjust(date, _bdc);
        }




    }
}
