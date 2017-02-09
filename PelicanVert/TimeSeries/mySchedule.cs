using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Containers
{
    public class mySchedule : IEnumerable, IEnumerator
    {


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        // INTERNAL CONTAINER
        protected Dictionary<DateTime, double> _cashflows;
        public Dictionary<DateTime, double> cashflows
        {
            get
            {
                if (this._cashflows == null)
                {
                    this._cashflows = new Dictionary<DateTime, double>();
                }
                return _cashflows;
            }

            protected set { _cashflows = value; }
        }


        // DATES
        #region Dates

        // START DATE 
        protected DateTime _startDate;
        public DateTime startDate
        {
            get { return _startDate; }
            protected set { _startDate = value; }
        }


        // END DATE
        protected DateTime _endDate;
        public DateTime endDate
        {
            get { return _endDate; }
            protected set { _endDate = value; }
        }


        #endregion


        // ENUMERATOR PROPERTIES 
        private int Position = -1;





        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public mySchedule() { }


        // Constructor 2 : Specifying start and end dates
        public mySchedule(DateTime argStartDate, DateTime argEndDate)
        {
            this.startDate = argStartDate;
            this.endDate = argEndDate;

        }



        // Constructor 3 : Specified all cashflows
        public mySchedule(Dictionary<DateTime, double> argCashflows)
        {
            this.cashflows = argCashflows;
            this.startDate = argCashflows.Keys.Min();
            this.endDate = argCashflows.Keys.Max();

        }


        #endregion






        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region Square brackets indexers

        public double this[DateTime i]
        {
            get
            {
                if (this._cashflows.Keys.Contains(i))
                {
                    return this.cashflows[i];
                }

                else if ((this._cashflows.Keys.Max() - i).TotalDays >= 0)
                {
                    return this.linearInterpolation(i);
                }

                else
                {
                    return this.flatExtrapolation(i);
                }


            }
            protected set { this.cashflows[i] = value; }
        }

        #endregion



        // ************************************************************
        // GENERIC METHODS
        // ************************************************************

        #region Generic Methods

        // Count
        public int Count()
        {
            if (this.cashflows == null) { return 0; }
            return this.cashflows.Count();
        }


        // EXTRAPOLATION : just the last value propagated
        public double flatExtrapolation(DateTime i)
        {
            return this.cashflows[this.cashflows.Keys.Max()];
        }


        // INTERPOLATION : linear scheme
        private double linearInterpolation(DateTime argDate)
        {

            // Order the source
            this.cashflows.OrderByDescending(pair => pair.Value);

            // Sets beginDate to the FIRST date in the discountFactor dict
            var e = this.cashflows.GetEnumerator();
            e.MoveNext();
            DateTime beginDate = e.Current.Key;

            // Sets beginDate to the SECOND date in the dividendCurve dict
            e.MoveNext();
            DateTime endDate = e.Current.Key;

            // loop to get the argDate falling between begin and end dates
            while (argDate > endDate)
            {
                e.MoveNext();
                beginDate = endDate;
                endDate = e.Current.Key;

            }

            // Linear interpolation
            double ans = (this.cashflows[endDate] - this.cashflows[beginDate]) / ((endDate - beginDate).TotalDays) * ((argDate - beginDate).TotalDays) + this.cashflows[beginDate];

            // Return
            return ans;


        }

        #endregion



        // ************************************************************
        // ENUMERATOR-RELATED METHODS
        // ************************************************************

        #region Enumerators


        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }



        public bool MoveNext()
        {

            if (Position < cashflows.Count - 1)
            {
                ++Position;
                return true;
            }
            return false;
        }



        public void Reset()
        {
            Position = -1;
        }



        public object Current
        {
            get
            {
                DateTime key = this.cashflows.Keys.ElementAt(Position);
                return key;
            }
        }



        #endregion


    }
}
