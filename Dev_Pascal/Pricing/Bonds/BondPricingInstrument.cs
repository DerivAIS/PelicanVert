using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

// internal custom
using QLyx.DataIO;
using QLyx.DataIO.Markit;
using QLyx.Utilities;
using QLyx.Containers;
using QLyx.InterestRates;

using Pascal.Pricing;

namespace Pascal.Pricing.Bonds
{
    public class BondPricingInstrument
    {

        // ************************************************************
        // PROPERTIES - INFRASTUCTURE
        // ************************************************************

        #region Infrastructure

        // Bond identification
        protected DBID bondID() { return new DBID((int)_bondID); }
        protected BondReferential _bondID;

        // Le référentiel de la database
        protected ReferenceManager _referentiel = ReferenceManager.Factory;
        protected ReferenceManager referentiel()
        {
            if (_referentiel == null) { _referentiel = ReferenceManager.Factory; }
            return _referentiel;
        }

        // Accès au référentiel de la database
        protected IDtoken GetToken(int referenceDBID)
        {
            return _referentiel.Identify(new DBID(referenceDBID));
        }

        // Accès aux données de la base via le helper
        protected myFrame EndOfDay(int referenceDBID, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            IDtoken thisToken = GetToken(referenceDBID);
            return EndOfDay(thisToken, startDate, endDate);
        }

        protected myFrame EndOfDay(IDtoken referenceToken, DateTime startDate = new DateTime(), DateTime endDate = new DateTime())
        {
            if (startDate == DateTime.MinValue) { startDate = _historyStartDate; }
            if (endDate == DateTime.MinValue) { endDate = _historyEndDate; }

            HistoricalDataRequest myRequest = new HistoricalDataRequest(referenceToken, startDate, endDate);
            return _localDatabase.GetEODPrices(myRequest);
        }

        // Helper pour la DB locale
        DatabaseHelper _localDatabase = new DatabaseHelper();

        #endregion


        // ************************************************************
        // PROPERTIES - HISTORICAL TIME SERIES - PRICES/YIELDS/SPREADS
        // ************************************************************

        #region History / Backward

        // Underlying
        protected MarkitEquityUnderlying _underlying;

        // Setting the Time series
        protected void SetBondTimeSeries()
        {
            _timeSeries = EndOfDay(bondID());
        }

        // Accessing the Time series
        protected myFrame _timeSeries;
        public double cleanPrice(DateTime date, string BidMidAsk = "Mid")
        {
            if (_timeSeries == null) { SetBondTimeSeries(); }
            return (double)_timeSeries[date]["CleanPrice"+BidMidAsk];
        }
        public double dirtyPrice(DateTime date, string BidMidAsk = "Mid")
        {
            if (_timeSeries == null) { SetBondTimeSeries(); }
            return (double)_timeSeries[date]["DirtyPrice" + BidMidAsk];
        }
        public double yieldToMaturity(DateTime date, string BidMidAsk = "Mid")
        {
            if (_timeSeries == null) { SetBondTimeSeries(); }
            return (double)_timeSeries[date]["YieldToMaturity" + BidMidAsk];
        }
        public double assetSwapSpread(DateTime date)
        {
            if (_timeSeries == null) { SetBondTimeSeries(); }
            return (double)_timeSeries[date]["AssetSwapSpreadMid"];
        }

        #endregion


        // ************************************************************
        // PROPERTIES - DATES
        // ************************************************************

        #region Infrastructure

        // History Start Date
        protected DateTime _historyStartDate = new DateTime(2013, 12, 31);

        // History End Date
        protected DateTime _historyEndDate = DateTime.Today.AddDays(-1);

        // Maturity Date
        protected DateTime _maturityDate;

        #endregion


        // ************************************************************
        // PROPERTIES - BOND CARACTERISTICS
        // ************************************************************

        #region Bond caracteristics

        // Nominal
        protected double _nominal = 1.0;

        // Coupon Rate
        protected double _couponRate;

        // Coupon spacing (in between coupon payments)
        protected Period _couponSpacing;

        // Bond currency
        protected string _currency;


        #endregion


        // ************************************************************
        // PROPERTIES - COUPON SCHEDULE
        // ************************************************************

        #region Coupon Schedule management

        // Rate curve to perform calculations
        protected string _curveDescription;

        // Coupon schedule
        protected Dictionary<DateTime, double> coupons_Schedule(DateTime valuationDate) {
            if (_couponSchedule == null) { SetCouponSchedule(valuationDate); }
            return _couponSchedule;
        }
        protected Dictionary<DateTime, double> _couponSchedule;

        // Discount Factors
        protected Dictionary<DateTime, double> coupons_DF(DateTime valuationDate)
        {
            if (_couponDF == null) { ComputePV(valuationDate); }
            return _couponDF;
        }
        protected Dictionary<DateTime, double> _couponDF = new Dictionary<DateTime, double>();


        // Individual PV
        protected Dictionary<DateTime, double> coupons_PV(DateTime valuationDate)
        {
            if (_couponPV == null) { ComputePV(valuationDate); }
            return _couponPV;
        }

        protected Dictionary<DateTime, double> _couponPV = new Dictionary<DateTime, double>();

        // Return the PV of all future coupons base on a curve
        protected double PV_allFutureCoupons(DateTime valuationDate)
        {
            throw new NotImplementedException();
        }

        private void SetCouponSchedule(DateTime valuationDate)
        {
            _couponSchedule = new Dictionary<DateTime, double>();

            Date currentDate = _maturityDate.ToDate();
            List<Date> couponDates = new List<Date>();
            couponDates.Add(currentDate);

            int i = 0;
            while (currentDate > valuationDate.ToDate()) {
                currentDate = Adjust(currentDate - _couponSpacing);
                couponDates.Add(currentDate);
            }

            for (int k = couponDates.Count()-1; k > 0 ; k--) {
                Date begin = couponDates.ElementAt(k);
                Date end = couponDates.ElementAt(k-1);
                double pmt = 0.01 * _nominal * _couponRate * _dayCounter.yearFraction(begin, end);
                _couponSchedule[end.ToDateTime()] = pmt;
            }
        }

        private void ComputePV(DateTime valuatioNDate)
        {
            throw new NotImplementedException();
        }

        // Default Period for rate curve --> vs EUR3M
        // protected Period _defaultPeriod = new Period(3, TimeUnit.Months);

        #endregion



        // ************************************************************
        // PROPERTIES - COUPON SCHEDULE
        // ************************************************************

        #region Coupon Schedule management

        // PV des coupons futurs
        public double couponsPV_vs3m(DateTime valuationDate)
        {
            return Compute_Coupons_PV(valuationDate, RateCurve(valuationDate, "3M"));
        }

        public double couponsPV_vs6m(DateTime valuationDate)
        {
            return Compute_Coupons_PV(valuationDate, RateCurve(valuationDate, "6M"));
        }

        public double couponsPV_vsOIS(DateTime valuationDate)
        {
            return Compute_Coupons_PV(valuationDate, RateCurve(valuationDate, "OIS"));
        }

        protected myRateCurve RateCurve(DateTime valuationDate, string CurveDescription)
        {
            string desc = CurveDescription.ToUpper();

            switch (desc)
            {

                case ("3M"):
                    return new myRateCurveEURv3m(valuationDate);

                case "6M":
                    return new myRateCurveEUR(valuationDate);

                case "OIS":
                    return new myRateCurveEUROIS(valuationDate);

                default:
                    throw new ArgumentException("RateCurveUnavailable", "Requested rate curve is not available.");

            }
        }

        protected double Compute_Coupons_PV(DateTime valuationDate, myRateCurve rateCurve)
        {
            double cumSum = 0.0;

            foreach (DateTime dt in coupons_Schedule(valuationDate).Keys)
            {
                _couponDF[dt] = 0.01 * rateCurve.getDiscountFactor(dt);
                _couponPV[dt] = _couponDF[dt] * _couponSchedule[dt];
                cumSum += _couponPV[dt];
            }

            return cumSum;

        }


        #endregion


        // ************************************************************
        // PROPERTIES - CALENDAR MANAGEMENT 
        // ************************************************************

        #region Calendar management

        // CALENDAR
        protected Calendar _calendar = new NullCalendar();

        // DAY COUNTER
        protected DayCounter _dayCounter = new Actual365Fixed();

        // BDC
        protected BusinessDayConvention _bdc = BusinessDayConvention.ModifiedFollowing;

        // ADJUST DATETIMES
        protected DateTime Adjust(DateTime dateTime)
        {
            Date d = _calendar.adjust(dateTime.ToDate(), _bdc);
            return d.ToDateTime();
        }

        // ADJUST DATES
        protected List<DateTime> Adjust(List<DateTime> dateList)
        {
            List<DateTime> res = new List<DateTime>();
            foreach (DateTime dt in dateList)
            {
                res.Add(Adjust(dt));
            }
            return res;
        }
        
        #endregion



        // ************************************************************
        // CONSTRUCTOR
        // ************************************************************

        #region Constructors

        public BondPricingInstrument(BondReferential bondID, DateTime maturity, double nominal, double couponRate, Period couponSpacing, string currency,
            Calendar calendar, DayCounter dayCounter, BusinessDayConvention bdc)
        {
            
            // Bond identification
            _bondID = bondID;

            // Maturity
            _maturityDate = maturity;

            _nominal = nominal;
            _couponRate = couponRate;
            _couponSpacing = couponSpacing;

            // Currency
            _currency = currency;

            // Calendar, Day Count & BDC
            _calendar = calendar;
            _dayCounter = dayCounter;
            _bdc = bdc;

        }

        #endregion



    }
}
