using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// External custom package
using QLNet;

// Internal custom package
using QLyx.InterestRates;

namespace QLyx.Credit
{

    public class myEurCDSCurve
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        // Recovery
        private double recovery = 0.4;

        // Survival Vector
        private double[,] survivalMatrix;
        protected SortedDictionary<DateTime, double> survivalProbabilities = new SortedDictionary<DateTime, double>();

        // Hazard Matrix
        private double[,] hazardMatrix;
        protected SortedDictionary<DateTime, double> hazardRates = new SortedDictionary<DateTime, double>();

        // Cumulative Hazard Matrix
        private double[,] cumulativeHazardMatrix;
        protected SortedDictionary<DateTime, double> cumulativeHazardRates = new SortedDictionary<DateTime, double>();

        // CDS Spreads
        /*
        protected SortedDictionary<DateTime, double> CdsSpreads = new SortedDictionary<DateTime, double>()
        {
            {new DateTime(2017, 03, 04), 0.06670}, 
            {new DateTime(2018, 03, 04), 0.08800}, 
            {new DateTime(2019, 03, 04), 0.10640},
            {new DateTime(2020, 03, 04), 0.10670}, 
            {new DateTime(2021, 03, 04), 0.10800},
            {new DateTime(2023, 03, 04), 0.10470},
            {new DateTime(2026, 03, 04), 0.10140},

        };
        */




        // ************************************************************
        // INSTANCE PROPERTIES -- TESTING PURPOSES
        // ************************************************************

        // *** DELETE THIS SECTION ***

        // FOR TESTING PURPOSES
        private double[,] CDSSpreads = myEurCDSCurve.createCDSCurve();

        // FOR TESTING PURPOSES
        static double[,] createCDSCurve()
        {
            double[,] cds = new double[7, 1];

            // Format: [Tenor, Entity Nb]

            // RALLYE
            cds[0, 0] = 667;
            cds[1, 0] = 880;
            cds[2, 0] = 1064;
            cds[3, 0] = 1067;
            cds[4, 0] = 1080;
            cds[5, 0] = 1047;
            cds[6, 0] = 1014;

            return cds;
        }

        // FOR TESTING PURPOSES
        public double df(double argument)
        {
            return 1.0;
        }

        // *** DELETE THIS SECTION ***




        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************


        // Constructor 1: Generic
        public myEurCDSCurve() { }


        // Constructor 2: with Pricing Date and Reference Entity
        public myEurCDSCurve(string referenceEntity, DateTime argPricingDate)
        {
            this.referenceEntity = referenceEntity;
            this.PricingDate = argPricingDate;
            //InitializeCdsCurve();

        }





        // ************************************************************
        // INSTANCE PROPERTIES -- GENERICS
        // ************************************************************
        #region Generic properties


        // CDS SPREAD ELEMENTS (List of myCDS)
        #region

        // Indexé sur la maturité du CDS
        protected SortedDictionary<DateTime, myCDS> _cdsElements = new SortedDictionary<DateTime, myCDS>();
        public SortedDictionary<DateTime, myCDS> cdsElements
        {
            get
            {
                if (_cdsElements.Count() == 0) { InitializeCdsCurve(this.PricingDate); }
                return _cdsElements;
            }
            protected set { _cdsElements = value; }
        }


        public void InitializeCdsCurve(DateTime argDate)
        {
            Date pricingDate = new Date(argDate);

            foreach (string tenor in CdsCurveTenors.Keys)
            {
                Date thisMaturityDate = CdsCurveCalendar.advance(pricingDate, CdsCurveTenors[tenor], CdsCurveBDC, false);
                myCDS thisCDS = new myCDS(referenceEntity, PricingDate, CdsCurveTenors[tenor], CdsCurveCurrency);
                _cdsElements[(DateTime)thisMaturityDate] = thisCDS;
            }
        }

        #endregion


        // PRICING DATE
        #region Pricing Date

        protected DateTime _PricingDate = DateTime.Today.AddDays(-1);
        public DateTime PricingDate
        {
            get { return _PricingDate; }
            protected set { _PricingDate = value; }
        }

        #endregion


        // CALENDAR
        #region Calendar

        protected Calendar _CdsCurveCalendar = new TARGET();
        public Calendar CdsCurveCalendar
        {
            get { return _CdsCurveCalendar; }
            protected set { this._CdsCurveCalendar = value; }

        }

        #endregion


        // BUSINESS DAY CONVENTION
        #region Business Day Convention

        protected BusinessDayConvention _CdsCurveBDC = BusinessDayConvention.ModifiedFollowing;
        public BusinessDayConvention CdsCurveBDC
        {
            get { return _CdsCurveBDC; }
            protected set { this._CdsCurveBDC = value; }

        }

        #endregion


        // CURRENCY
        #region Currency

        protected Currency _CdsCurveCurrency = new EURCurrency();
        public Currency CdsCurveCurrency
        {
            get { return _CdsCurveCurrency; }
            protected set { this._CdsCurveCurrency = value; }

        }

        #endregion


        // REFERENCE ENTITY
        #region Reference Entity

        protected string _referenceEntity;
        public string referenceEntity
        {
            get { return _referenceEntity; }
            protected set { this._referenceEntity = value; }

        }

        #endregion


        // STANDARD TENORS
        #region Standard Tenors

        protected SortedDictionary<string, Period> _CdsCurveTenors = new SortedDictionary<string, Period>();
        public SortedDictionary<string, Period> CdsCurveTenors
        {
            get
            {
                if (_CdsCurveTenors.Count() == 0) { _CdsCurveTenors = PopulateWithMarkitTenors(); }
                return _CdsCurveTenors;
            }
            protected set { this._CdsCurveTenors = value; }

        }


        private SortedDictionary<string, Period> PopulateWithMarkitTenors() // { "6M", new Period(6, TimeUnit.Months) },
        {
            return new SortedDictionary<string, Period>() 
                { 
                //{ "6M", new Period(6, TimeUnit.Months) }, // NOT WORKING... NEEDS EQUAL PERIODS
 
                { "1Y", new Period(1, TimeUnit.Years) },
                { "2Y", new Period(2, TimeUnit.Years) }, 
                { "3Y", new Period(3, TimeUnit.Years) }, 
                { "4Y", new Period(4, TimeUnit.Years) },
                { "5Y", new Period(5, TimeUnit.Years) }, 
                { "7Y", new Period(7, TimeUnit.Years) }, 
                { "10Y", new Period(10, TimeUnit.Years) }

                //{ "15Y", new Period(15, TimeUnit.Years) }, 
                //{ "20Y", new Period(20, TimeUnit.Years) }, 
                //{ "30Y", new Period(30, TimeUnit.Years) }

                };


        }

        #endregion


        // RATE CURVE
        #region Rate Curve

        protected myRateCurveEURv3m _rateCurve;
        public myRateCurveEURv3m rateCurve
        {
            get
            {
                if (_rateCurve == null) { InitializeRateCurve(); }
                return _rateCurve;
            }
            protected set { _rateCurve = value; }
        }

        private void InitializeRateCurve()
        {
            _rateCurve = new myRateCurveEURv3m(this.PricingDate);
        }

        public void ForceRateCurve(myRateCurveEURv3m argCurve)
        {
            _rateCurve = argCurve;
        }

        #endregion

        #endregion



        // ************************************************************
        // METHODS -- MY VERSION
        // ************************************************************

        public void ComputeSurvivalProbabilities()
        {

            this.artificiallySetCdsElements();

            survivalProbabilities = new SortedDictionary<DateTime, double>();

            // Set discount factors
            Dictionary<DateTime, double> DF = new Dictionary<DateTime, double>();
            foreach (DateTime dt in cdsElements.Keys)
            {
                DF[dt] = rateCurve.getDiscountFactor(dt) / 100;
            }

            // Adding 1 for the current day
            DF[PricingDate] = 1.0;


            // Initialize temp variables
            int n = 0; int i = 0;
            survivalProbabilities[PricingDate] = 1.0;

            for (n = 1; n < cdsElements.Count(); n++)
            {

                // Current date
                date_n = this.cdsElements.ElementAt(n - 1).Key;



                double S_n = cdsElements.ElementAt(n - 1).Value.spread;
                double dt_nm1_n = (DF.ElementAt(n - 1).Key - DF.ElementAt(n - 2).Key).TotalDays / 365;
                double DF_n = DF.ElementAt(n - 1).Value;
                double alpha = DF_n * (1 - recovery + (0.5 * S_n * dt_nm1_n));


                // Computing Term 2
                double DF_1 = DF.ElementAt(1).Value;
                double dt_0_1 = (DF.ElementAt(1).Key - DF.ElementAt(0).Key).TotalDays / 365;
                double beta = DF_1 * ((1 - recovery) - (0.5 * S_n * dt_0_1));


                // Init for iter terms
                double cumSum1 = 0.0;
                double cumSum2 = 0.0;
                double cumSum3 = 0.0;


                // Computing Term 1
                for (i = 1; i < n; i++)
                {
                    double DF_i = DF.ElementAt(i - 1).Value;
                    double DF_next_i = DF.ElementAt(i).Value;

                    //double dt_previ_i = (cdsElements.ElementAt(i).Value.MaturityDate - cdsElements.ElementAt(i - 1).Value.MaturityDate).TotalDays / 365;
                    //double dt_i_nexti = (cdsElements.ElementAt(i + 1).Value.MaturityDate - cdsElements.ElementAt(i).Value.MaturityDate).TotalDays / 365;

                    // A MODIOFIER !!!!!!!!!!!!!!!
                    double dt_previ_i = (DF.ElementAt(i).Key - DF.ElementAt(i - 1).Key).TotalDays / 365;
                    double dt_i_nexti = (DF.ElementAt(i + 1).Key - DF.ElementAt(i).Key).TotalDays / 365;


                    double Ps_i = survivalProbabilities.ElementAt(i).Value;

                    cumSum1 += 0.5 * Ps_i * DF_i * dt_previ_i;
                    cumSum2 += 0.5 * Ps_i * DF_next_i * dt_i_nexti;
                    cumSum3 += Ps_i * (DF_next_i - DF_i);
                }


                // Assignement
                survivalProbabilities[date_n] = (1 / alpha) * (-1 * S_n * (cumSum1 + cumSum2) + (1 - recovery) * cumSum3 + beta);



            }
        }

        private void artificiallySetCdsElements()
        {

            Dictionary<DateTime, double> exampleDict = new Dictionary<DateTime, double>() 
                    {
                    //{new DateTime(2016, 09, 05), 0.03880},
                    {new DateTime(2017, 03, 06), 0.06670}, 
                    {new DateTime(2018, 03, 05), 0.08800}, 
                    {new DateTime(2019, 03, 04), 0.10640},
                    {new DateTime(2020, 03, 04), 0.10670}, 
                    {new DateTime(2021, 03, 04), 0.10800},
                    {new DateTime(2023, 03, 06), 0.10470},
                    {new DateTime(2026, 03, 04), 0.10140}
                    };

            foreach (DateTime dt in cdsElements.Keys)
            {
                cdsElements[dt].SetSpread(exampleDict[dt]);
            }

        }


        public void ComputeHazardRates()
        {
            // convert matrix of survival probabilities into two hazard rate matrices
            int rows = survivalMatrix.GetUpperBound(0);
            int cols = survivalMatrix.GetUpperBound(1) + 1;
            hazardMatrix = new double[rows, cols];
            cumulativeHazardMatrix = new double[rows, cols];
            int i = 0; int j = 0;
            //
            for (i = 0; i < rows; i++)
            {
                for (j = 0; j < cols; j++)
                {
                    cumulativeHazardMatrix[i, j] = -Math.Log(survivalMatrix[i + 1, j]);
                    if (i == 0) hazardMatrix[i, j] = cumulativeHazardMatrix[i, j];
                    if (i > 0) hazardMatrix[i, j] = (cumulativeHazardMatrix[i, j] - cumulativeHazardMatrix[i - 1, j]);
                }
            }
        }







        public DateTime date_n { get; set; }
    }
}

