using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO.Markit
{
    public class MarkitSurface
    {

        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        #region Properties

        // INDEX NAME
        #region Index name (string)

        protected string _name;
        public string name
        {
            get
            {
                return _name;
            }

            protected set
            {
                _name = value;
            }

        }

        #endregion


        // IS MARKIT NEW FORMAT (JAN 2017 ONWARDS)
        #region isNewFormat (bool)

        protected Boolean _isNewFormat = false;
        public Boolean isNewFormat
        {
            get
            {
                return _isNewFormat;
            }

            protected set
            {
                _isNewFormat = value;
            }

        }

        #endregion


        // CALENDAR AND DAY COUNTER
        #region Calendar (QLNet.Calendar) and Day Counter (QLNet.DayCounter)

        public Calendar calendar() { return _calendar; }
        protected Calendar _calendar = new TARGET();

        public DayCounter dayCounter() { return _dayCounter; }
        protected DayCounter _dayCounter = new Actual365Fixed();

        public int settlementDays() { return _settlementDays; }
        protected int _settlementDays = 0;

        #endregion



        // PRICING DATE
        #region Information Date (as DateTime)
        protected DateTime _informationDate;
        public DateTime informationDate
        {
            get
            {
                return _informationDate;
            }

            protected set
            {
                _informationDate = value;
            }

        }

        #endregion


        // IMPLIED SPOT
        #region Spot (double)
        // Faire une référene à Forward(0) pour la cohérence
        protected double _impliedSpot;
        public double impliedSpot
        {
            get
            {
                return _impliedSpot;
            }

            protected set
            {
                _impliedSpot = value;
            }

        }

        #endregion


        // REFERENCE LEVEL
        #region Reference Level (double)
        protected double _referenceLevel;
        public double referenceLevel
        {
            get
            {
                return _referenceLevel;
            }

            protected set
            {
                _referenceLevel = value;
            }

        }

        #endregion


        // FORWARD
        #region Forward (MarkitForward)

        protected MarkitForward _forward;
        public MarkitForward forward
        {
            get
            {
                if (_forward == null)
                {
                    _forward = new MarkitForward(informationDate);
                    _forward.SetData(this.informationDate, this.informationDate, this.impliedSpot);
                }

                return _forward;
            }

            protected set
            {
                _forward = value;
            }

        }

        #endregion


        // DISCOUNT FACTOR
        #region Discount Factor (MarkitDiscountFactor)

        protected MarkitDiscountFactor _DF;
        public MarkitDiscountFactor DF
        {
            get
            {
                if (_DF == null)
                {
                    _DF = new MarkitDiscountFactor(informationDate);
                    _DF.SetData(this.informationDate, this.informationDate, 1.0);
                }

                return _DF;
            }

            protected set
            {
                _DF = value;
            }

        }

        #endregion


        // DIVIDEND 
        #region Dividend (as MarkitDividend)

        protected MarkitDividend _dividend;
        public MarkitDividend dividend
        {
            get
            {
                if (_dividend == null)
                {
                    // dividend is an explicit column in the Markit data file
                    if (_isNewFormat)
                    {
                        // Console.WriteLine("Warning : New dividend accessor in MarkitSurface not implemented.");
                        _dividend = new MarkitDividend(informationDate);
                    }
                    // dividend has to be computed from forwards points and interest rates
                    else
                    {
                        // PERFORM CALCULATION
                        _dividend = new MarkitDividend(informationDate, this.forward, this.DF, impliedSpot);
                    }
                }

                return _dividend;

            }

            protected set
            {
                _dividend = value;
            }

        }


        public List<double> Dividend_DF()
        {

            List<double> div_df = new List<double>();
            foreach (DateTime d in forward.Dates())
            {
                div_df.Add(DF[d] * forward[d] / impliedSpot);
            }

            return div_df;

        }

        #endregion


        // DATA -- ACTUAL SURFACE
        #region Volatility Surface (dict of MarkitSmile)


        protected Dictionary<DateTime, MarkitSmile> _data;
        public Dictionary<DateTime, MarkitSmile> data
        {
            get
            {
                if (_data == null)
                {
                    _data = new Dictionary<DateTime, MarkitSmile>();
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
        //public MarkitSurface() { }


        // Constructor 2 : Generic with valuation date
        public MarkitSurface(DateTime argDate)
        {
            informationDate = argDate;
        }

        #endregion



        // ************************************************************
        // METHODS : SETTING DATA
        // ************************************************************
        #region

        // ADDING INDEX NAME
        public void SetName(string v)
        {
            name = v;
        }

        // ADDING IMPLIED SPOT
        public void SetImpliedSpot(string argSpotString)
        {
            Double thisSpot = 0.0;
            Double.TryParse(argSpotString, out thisSpot);

            // Set spot level as property
            this.impliedSpot = thisSpot;

            // Set spot within forward term structure (at origin)
            this.forward.SetData(this.informationDate, this.informationDate, thisSpot);

        }

        public void SetImpliedSpot(Double argSpot)
        {
            this.impliedSpot = argSpot;
        }

        // ADDING REFERENCE LEVEL
        public void SetReferenceLevel(string argRefLevelString)
        {
            Double thisRef = 0.0;
            Double.TryParse(argRefLevelString, out thisRef);

            this.referenceLevel = thisRef;
        }

        public void SetReferenceLevel(Double argRefLevel)
        {
            this.referenceLevel = argRefLevel;
        }


        // ADDING FORWARD (SINGLE) DATA POINT 
        public void SetForward(DateTime pricingDate, DateTime expiryDate, string argFwdPt)
        {
            Double thisFwdPoint = 0.0;
            Double.TryParse(argFwdPt, out thisFwdPoint);

            this.forward.SetData(pricingDate, expiryDate, thisFwdPoint);

        }


        // ADDING DISCOUNT FACTOR (SINGLE) DATA POINT 
        public void SetDiscountFactor(DateTime pricingDate, DateTime expiryDate, string argDF)
        {
            Double thisDFpoint = 0.0;
            Double.TryParse(argDF, out thisDFpoint);

            this.DF.SetData(pricingDate, expiryDate, thisDFpoint);

        }


        // ADDING DIVIDEND (SINGLE) DATA POINT 
        public void SetDividend(DateTime pricingDate, DateTime expiryDate, string argDiv)
        {
            Double thisDivpoint = 0.0;
            Double.TryParse(argDiv, out thisDivpoint);

            dividend.SetData(pricingDate, expiryDate, thisDivpoint);

        }


        // ADDING VOLATILITY POINT
        public void SetVolatility(DateTime pricingDate, DateTime expiryDate, string argMoneyness, string argVolatility)
        {
            Double thisStrike = 0.0;
            Double.TryParse(argMoneyness, out thisStrike);

            Double thisVol = 0.0;
            Double.TryParse(argVolatility, out thisVol);

            if (!this.data.ContainsKey(expiryDate))
            {
                this.data[expiryDate] = new MarkitSmile(pricingDate, expiryDate);
            }

            // Strike given by Markit is 150 for 150%. Need to divide by 100.
            this.data[expiryDate].SetData(pricingDate, expiryDate, thisStrike * 0.01, thisVol);

        }

        public void IsNewFormat()
        {
            _isNewFormat = true;
        }

        #endregion


        // ************************************************************
        // METHODS : ACCESSING DATA
        // ************************************************************

        #region

        // ACCESSING SMILE
        public MarkitSmile GetSmile(DateTime expiryDate)
        {

            if (data.ContainsKey(expiryDate)) { return data[expiryDate]; }

            else if (DateTime.Compare(expiryDate, data.Keys.Max()) > 0) { return Extrapolate_LongEnd(expiryDate); }

            else if (DateTime.Compare(expiryDate, data.Keys.Min()) < 0) { return Extrapolate_ShortEnd(expiryDate); }

            else { return Interpolate(expiryDate); }

        }

        // ACCESSING STRIKES
        public List<double> Strikes(double refSpot)
        {
            var smile = _data.FirstOrDefault().Value.Strikes();
            List<double> result = new List<double>();
            foreach (double pt in smile) { result.Add(pt * refSpot); }
            return result;
        }

        public List<double> Strikes()
        {
            return Strikes(1.0);
        }

        protected int ATM_index()
        {
            return Strikes().IndexOf(1.0);
        }

        // ACCESSING DATES
        public List<Date> Dates()
        {
            List<Date> res = this.forward.Dates();
            res.Remove(_informationDate);
            return res;
        }

        // ACCESSING VOL MATRIX
        
        public Matrix VolMatrix()
        {
            int rows =  Strikes().Count();
            int cols = _data.Count();

            int r = 0;
            int c = 0;

            Matrix res = new Matrix(rows, cols, 0.0);

            foreach (DateTime dt in _data.Keys)
            {

                MarkitSmile smile = _data[dt];

                foreach (double strike in smile.Strikes())
                {
                    res[c, r] = smile[strike];
                    c++;
                }
                c = 0;
                r++;
            }

            return res;
        }

        public Matrix VolMatrix(double scalingLevel)
        {
            int rows = Strikes().Count();
            int cols = _data.Count();

            int r = 0;
            int c = 0;

            Matrix res = new Matrix(rows, cols, 0.0);

            foreach (DateTime dt in _data.Keys)
            {

                MarkitSmile smile = _data[dt];

                foreach (double strike in smile.Strikes())
                {
                    res[c, r] = smile[strike] * scalingLevel;
                    c++;
                }
                c = 0;
                r++;
            }

            return res;
        }



        public Matrix VolMatrix(double scalingLevel, double smileLevel)
        {
            int rows = Strikes().Count();
            int cols = _data.Count();

            int r = 0; // tenor - maturity
            int c = 0; // strike - moneyness

            Matrix tmp = new Matrix(rows, cols, 0.0);
            //Matrix res = new Matrix(rows, cols, 0.0);
            
            foreach (DateTime dt in _data.Keys)
            {
                MarkitSmile smile = _data[dt];
                double smileATM = smile.data[1.0];

                foreach (double strike in smile.Strikes())
                {
                    double atm_vol = smile[strike] * scalingLevel;
                    double smile_excess = (smile[strike] - smileATM);

                    tmp[c, r] = scalingLevel * ( smile[strike] + smileLevel * (smile[strike] - smileATM) );
                    c++;
                }
                c = 0;
                r++;
            }

            
            return tmp;

        }

        private MarkitSmile Extrapolate_ShortEnd(DateTime expiryDate)
        {
            return data[data.Keys.Min()];
        }

        private MarkitSmile Extrapolate_LongEnd(DateTime expiryDate)
        {
            return data[data.Keys.Max()];
        }

        private MarkitSmile Interpolate(DateTime expiryDate)
        {
            DateTime prevDate = new DateTime();
            DateTime nextDate = new DateTime();

            Double prevDate_days = Double.PositiveInfinity;
            Double nextDate_days = Double.PositiveInfinity;

            foreach (DateTime d in data.Keys)
            {
                // Check for lower bound
                if (((d - expiryDate).TotalDays < prevDate_days) && (DateTime.Compare(d, expiryDate) < 0))
                {
                    prevDate_days = (d - expiryDate).TotalDays;
                    prevDate = d;
                }

                // Check for upper bound
                if (((d - expiryDate).TotalDays < nextDate_days) && (DateTime.Compare(d, expiryDate) > 0))
                {
                    nextDate_days = (d - expiryDate).TotalDays;
                    nextDate = d;
                }

            }


            if ((prevDate != DateTime.MinValue) && (nextDate != DateTime.MinValue))
            {
                return LinearVarianceInterpolate(expiryDate, prevDate, nextDate);
            }

            else
            {
                throw new System.ArgumentException("DataUnavailable", "Markit Equity IV object does not contain the volatility data requested.");
            }
        }

        private MarkitSmile LinearVarianceInterpolate(DateTime expiryDate, DateTime prevDate, DateTime nextDate)
        {
            Dictionary<Double, Double> temp = new Dictionary<double, double>();

            foreach (Double strike in data[prevDate].data.Keys)
            {
                if (data[nextDate].data.ContainsKey(strike))
                {
                    double var_min = Math.Pow(data[prevDate][strike], 2);
                    double var_max = Math.Pow(data[nextDate][strike], 2);

                    temp[strike] = Math.Pow(var_min + (var_max - var_min) / (nextDate - prevDate).TotalDays * (expiryDate - prevDate).TotalDays, 0.5);
                }

                else { temp[strike] = data[prevDate][strike]; }
            }

            // @TODO : Does not cover the cases where there are additional strikes in the upper bound vector (smile).

            return new MarkitSmile(this.informationDate, expiryDate, temp);

        }

        #endregion


        // ************************************************************
        // METHODS : EXTRACTING TERM STRUCTURES FOR QLNET USAGE
        // ************************************************************

        #region


        // ***********************************
        // EXTRACTING RISK FREE TERM STRUCTURE
        // ***********************************
        
        public Handle<YieldTermStructure> riskFree_FwdCrv()
        {
            InterpolatedDiscountCurve<Cubic> crv = new InterpolatedDiscountCurve<Cubic>(DF.Dates(), DF.Points(), dayCounter());
            return new Handle<YieldTermStructure>(crv);
        }


        public Handle<YieldTermStructure> riskFree_flat()
        {
            double riskFreeRate = this.DF.ZeroRateAtMaturity();
            return new Handle<YieldTermStructure>(new FlatForward(new Date(informationDate), riskFreeRate, dayCounter()));
        }

        public Handle<YieldTermStructure> riskFree_flat(DateTime maturityDate)
        {
            double riskFreeRate = this.DF.ZeroRate(maturityDate);
            return new Handle<YieldTermStructure>(new FlatForward(new Date(informationDate), riskFreeRate, dayCounter()));
        }


        // ***********************************
        // EXTRACTING DIVIDEND TERM STRUCTURE
        // ***********************************

        public Handle<YieldTermStructure> dividend_FwdCrv()
        {

             YieldTermStructure crv;

            if (_isNewFormat == false)
            {
                QLNet_Results res = dividend.GetDividendDF_TermStructure(impliedSpot, forward.data, DF.data);
                crv = new InterpolatedDiscountCurve<Cubic>(res.timeStamps(), res.dataPoints(), dayCounter()); //InterpolatedDiscountCurve<Cubic> 
            }
            else {
                QLNet_Results res = dividend.GetDividendDF_TermStructure_NewFormat();
                crv = new InterpolatedZeroCurve<Linear>(res.timeStamps(), res.dataPoints(), _dayCounter, _calendar); //InterpolatedZeroCurve<Linear> 

                //(res.timeStamps(), res.dataPoints(), dayCounter());

            }



            return new Handle<YieldTermStructure>(crv);
        }


        public Handle<YieldTermStructure> dividendYield_flat()
        {
            double dividendYield = this.dividend.DivYieldAtMaturity(impliedSpot, forward.AtMaturity(), DF.AtMaturity());
            return new Handle<YieldTermStructure>(new FlatForward(new Date(informationDate), dividendYield, dayCounter()));
        }


        public Handle<YieldTermStructure> dividendYield_flat(DateTime maturityDate)
        {
            var fwd_dico = new Dictionary<DateTime, double>() { { maturityDate, forward[maturityDate] } };
            var df_dico = new Dictionary<DateTime, double>() { { maturityDate, DF[maturityDate] } };

            double dividendYield = this.dividend.DivYieldAtMaturity(impliedSpot, fwd_dico.FirstOrDefault(), df_dico.FirstOrDefault());
            return new Handle<YieldTermStructure>(new FlatForward(new Date(informationDate), dividendYield, dayCounter()));
        }



        // ************************************
        // EXTRACTING ATMF BLACK IMPL VOL CURVE
        // ************************************
        public BlackVarianceCurve ATMF_Vol_TS(DateTime valuationDate)
        //public ZeroRateTermStructure riskFree_TS(DateTime valuationDate)
        {
            List<Date> dates = new List<Date>();
            List<double> atmf_vols = new List<double>();
            double fwd_moneyness = 1.0;

            foreach (DateTime d in data.Keys)
            {
                // Setting date
                dates.Add(d);

                // Setting forward
                fwd_moneyness = forward[d] / impliedSpot;
                atmf_vols.Add(data[d].GetVolatility(fwd_moneyness));
            }

            BlackVarianceCurve crv = new BlackVarianceCurve(informationDate, dates, atmf_vols, _dayCounter, false);
            return crv;

        }




        // ************************************
        // EXTRACTING SHORT RATE
        // ************************************

        public double ShortRate()
        {
            return DF.ShortRate();
        }


        #endregion





        // ************************************************************
        // INDEXOR 
        // ************************************************************

        #region

        public MarkitSmile this[DateTime expiryDate]
        {
            get
            {
                return GetSmile(expiryDate);
            }
        }



        public MarkitSmile this[Date expiryDate]
        {
            get
            {
                DateTime expiryDateTime = new DateTime(expiryDate.year(), expiryDate.month(), expiryDate.Day);
                return GetSmile(expiryDateTime);
            }
        }


        


        #endregion



    }
}
