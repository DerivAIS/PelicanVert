using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// External custom packages
using QLNet;

// Internal custom packages
using QLyx.DataIO;


namespace QLyx.Credit
{
    public class myCDS
    {



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors


        // Constructor 1 : All arguments (with maturity date)
        public myCDS(string argReferenceEntity, DateTime argPricingDate, DateTime argIssueDate,
            DateTime argMaturityDate, Currency argCurrency)
        {

            // Set dates
            this.PricingDate = argPricingDate;
            this.IssueDate = argIssueDate;
            this.MaturityDate = argMaturityDate;

            // Set Reference Entity
            this.referenceEntity = argReferenceEntity;

            // Set the currency
            this.currency = argCurrency;


        }



        // Constructor 2 : All arguments (with tenor)
        public myCDS(string argReferenceEntity, DateTime argPricingDate, DateTime argIssueDate,
            Period argTenor, Currency argCurrency)
        {

            // Set dates
            this.PricingDate = argPricingDate;
            this.IssueDate = argIssueDate;

            // Set Tenor
            this.tenor = argTenor;
            this.SetMaturityDateFromTenor(argIssueDate);

            // Set Reference Entity
            this.referenceEntity = argReferenceEntity;

            // Set the currency
            this.currency = argCurrency;

        }



        // Constructor 3 : Few arguments (issue date implied to be the pricing date)
        public myCDS(string argReferenceEntity, DateTime argPricingDate,
            Period argTenor, Currency argCurrency)
            : this(argReferenceEntity, argPricingDate, argPricingDate, argTenor, argCurrency) { }



        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- DATES
        // ************************************************************

        #region Instance Properties -- Dates


        // PRICING DATE
        #region Pricing Date

        protected DateTime _PricingDate = DateTime.Today.AddDays(-1);
        public DateTime PricingDate
        {
            get { return _PricingDate; }
            protected set { _PricingDate = value; }
        }

        #endregion


        // ISSUE DATE
        #region Issue Date

        protected DateTime _IssueDate = new DateTime();
        public DateTime IssueDate
        {
            get
            {
                if (this._IssueDate == DateTime.MinValue) { this.SetCDSIssueDate(); }
                return _IssueDate;
            }
            protected set { _IssueDate = value; }
        }


        private void SetCDSIssueDate()
        {
            this._IssueDate = this.PricingDate;
            Console.WriteLine("Warning : Issue date taken as Pricing date for CDS.");
        }

        #endregion


        // MATURITY DATE
        #region Maturity Date

        protected DateTime _MaturityDate = new DateTime();
        public DateTime MaturityDate
        {
            get
            {
                if (this._MaturityDate == DateTime.MinValue) { this.SetCDSMaturityDate(); }
                return _MaturityDate;
            }
            protected set { _MaturityDate = value; }
        }


        private void SetCDSMaturityDate()
        {
            throw new NotImplementedException();
        }


        private void SetMaturityDateFromTenor(DateTime argIssueDate)
        {
            this._MaturityDate = calendar.advance(IssueDate, tenor, busDayConv, false);
        }


        #endregion



        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- CDS SPECIFICS
        // ************************************************************

        #region Instance Properties -- CDS specifics

        // SPREAD
        #region Credit Spread

        protected double _spread;
        public double spread
        {
            get
            {
                return _spread;
            }

            protected set
            {
                this._spread = value;
            }

        }

        public void SetSpread(double argSpread)
        {
            this._spread = argSpread;
        }


        #endregion


        // RECOVERY
        #region Recovery

        protected double _recovery = 0.40;
        public double recovery
        {
            get
            {
                return _recovery;
            }

            protected set
            {
                this._recovery = value;
            }

        }


        #endregion


        // REFERENCE ENTITY
        #region Reference Entity

        protected string _referenceEntity;
        public string referenceEntity
        {
            get
            {
                return _referenceEntity;
            }

            protected set
            {
                this._referenceEntity = value;
            }

        }


        #endregion


        // CURRENCY
        #region Currency

        protected Currency _currency;
        public Currency currency
        {
            get { return _currency; }
            protected set { _currency = value; }
        }

        #endregion


        // TENOR
        #region Tenor

        protected Period _tenor;

        public Period tenor
        {
            get
            {
                return _tenor;
            }

            protected set { _tenor = value; }
        }

        // TODO check the format returned by ToString method of QLNet Period object
        public string CDSPeriodString
        {
            get
            {
                string rawString = _tenor.ToString();
                return rawString;
            }

        }

        #endregion


        // PMT SCHEDULE
        #region Schedule

        protected Schedule _schedule;
        public Schedule schedule
        {
            get
            {
                if (this._schedule == null) { this.SetCDSSChedule(); }
                return _schedule;
            }
            protected set
            {
                _schedule = value;
            }
        }



        protected void SetCDSSChedule()
        {
            //this._schedule = new Schedule(issueDate, maturity, new Period(frequency), calendar,
            //                      convention, convention, DateGeneration.Rule.Forward, false);
        }

        #endregion


        // FIXING DAYS
        #region Fixing Days

        protected int _fixingDays;
        public int fixingDays
        {
            get { return _fixingDays; }
            protected set { this._fixingDays = value; }
        }


        #endregion


        // BUSINESS DAY CONVENTION
        #region Business Day Convention

        protected BusinessDayConvention _busDayConv;
        public BusinessDayConvention busDayConv
        {
            get { return _busDayConv; }
            protected set { this._busDayConv = value; }
        }



        #endregion


        // DAY COUNT CONVENTION
        #region Day Count Convention

        protected DayCounter _dayCountConv;
        public DayCounter dayCountConv
        {
            get { return _dayCountConv; }
            protected set { this._dayCountConv = value; }
        }


        #endregion


        // CALENDAR
        #region Calendar

        protected Calendar _calendar = new TARGET();
        public Calendar calendar
        {
            get { return _calendar; }
            protected set { this._calendar = value; }

        }

        #endregion

        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- INFRASTRUCTURE
        // ************************************************************

        #region Instance Properties -- Infrastructure

        // MARKET DATA HELPER
        /*
        #region Market Data Helper

        protected DataManager _MarketData = new DataManager();
        public DataManager MarketData
        {
            get { return _MarketData; }
        }

        #endregion
        */


        // DBID - ID TOKEN
        #region DBID

        protected IDtoken _DBID;
        public IDtoken DBID
        {
            get { return _DBID; }
            protected set
            {
                this._DBID = value;
            }
        }

        #endregion



        #endregion




        // ************************************************************
        // METHODS
        // ************************************************************

        public double GetLevel(DateTime argDate)
        {
            //return DataReader.CDS[referenceEntity, argDate];
            return 0.0100;
        }


    }
}
