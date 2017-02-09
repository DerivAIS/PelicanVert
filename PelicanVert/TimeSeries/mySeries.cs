using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLyx.Containers
{
    public class mySeries
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        #region

        // DATA
        protected SortedDictionary<DateTime, double> _data;
        public SortedDictionary<DateTime, double> data
        {
            get
            {
                if (this._data == null) { this._data = new SortedDictionary<DateTime, double>(); }
                return _data;
            }
            protected set { _data = value; }
        }


        // NAME
        protected string _name;
        public string name
        {
            get { return _name; }
            protected set { _name = value; }
        }


        // DATETIME OF FIRST DATA POINT
        protected DateTime _startDate;
        public DateTime startDate
        {
            get { return _startDate; }
            protected set { _startDate = value; }
        }


        // DATETIME OF LAST DATA POINT
        protected DateTime _endDate;
        public DateTime endDate
        {
            get { return _endDate; }
            protected set { _endDate = value; }
        }


        // LENGTH OF TIME mySeries (number of elements)
        protected int _length;
        public int length
        {
            get { return _length; }
            protected set { _length = value; }
        }


        // SORTED FLAG (bool to determine wether the mySeries has been sorted based on keys)
        protected bool _flagSorted;
        public bool flagSorted
        {
            get { return _flagSorted; }
            protected set { _flagSorted = value; }
        }


        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region


        // Constructor 1 : Generic
        public mySeries() { }


        // Constructor 2 : Direct load from Dict<DateTime, double>
        public mySeries(SortedDictionary<DateTime, double> argData)
        {

            // Set the data
            this.data = argData;

            // Order the data (implicitely sets start and end dates)
            this.sort();

        }



        #endregion



        // ************************************************************
        // COMMON METHODS 
        // ************************************************************

        #region


        // FILLNA WITH A VALUE
        public void fillNA_withValue(double argValue)
        {
            if (!this.flagSorted == true) { this.sort(); }

            foreach (DateTime dt in this.data.Keys)
            {
                if (Double.IsNaN(this.data[dt])) { this.data[dt] = argValue; }
            }

        }


        // ERASE
        public void erase()
        {
            foreach (DateTime dt in this.data.Keys)
            {
                this.data[dt] = 0.0;
            }
        }


        // SORT
        public void sort()
        {

            // Sort existing data
            // this.data.OrderByDescending(pair => pair.Key);
            this.data.OrderByDescending(pair => pair.Value);

            // Update start datetime
            this.startDate = this.data.First().Key;

            // Update end datetime
            this.endDate = this.data.Last().Key;

            // Count the number of points
            this.length = this.data.Count();

            // Set flag for sorted
            this.flagSorted = true;

        }


        // PUSHBACK
        public void pushback(DateTime dt, double dataPoint)
        {
            this.data[dt] = dataPoint;

        }


        // SHIFT (by 1)
        public mySeries shift()
        {

            // Make sure existing data is sorted
            if (!this.flagSorted == true) { this.sort(); }

            // Grab Enumerator from object
            var myEnum = this.data.GetEnumerator();

            // Grab 1st data point
            double currentVal = this.data.First().Value;

            // Create result Dict
            SortedDictionary<DateTime, double> shiftedData = new SortedDictionary<DateTime, double>();
            shiftedData[this.data.First().Key] = double.NaN;

            // Init
            myEnum.MoveNext();

            // Shift values
            while (myEnum.MoveNext())
            {
                shiftedData[myEnum.Current.Key] = currentVal;
                currentVal = myEnum.Current.Value;
            }

            return new mySeries(shiftedData);

        }


        // INDEX
        public SortedSet<DateTime> index()
        // previously hashset
        {
            SortedSet<DateTime> ans = new SortedSet<DateTime>(this.data.Keys);
            return ans;
        }


        // MERGE INDEX
        public SortedSet<DateTime> mergeIndex(mySeries argmySeries)
        // previously hashset
        {
            // http://www.dotnetperls.com/combine-keys
            SortedSet<DateTime> ans = new SortedSet<DateTime>(this.data.Keys);
            ans.UnionWith(argmySeries.data.Keys);
            return ans;
        }


        // MERGE mySeries
        public mySeries merge(mySeries argmySeries)
        {
            SortedDictionary<DateTime, double> ans = new SortedDictionary<DateTime, double>(this.data);
            foreach (DateTime dt in argmySeries.data.Keys.Where(x => !this.data.Keys.Contains(x)))
            {
                ans.Add(dt, Double.NaN);
            }

            return new mySeries(ans);
        }


        // CONCAT mySeries
        public mySeries concat(mySeries argmySeries)
        {
            SortedDictionary<DateTime, double> ans = new SortedDictionary<DateTime, double>(this.data);
            foreach (DateTime dt in argmySeries.data.Keys.Where(x => !this.data.Keys.Contains(x)))
            {
                ans.Add(dt, argmySeries.data[dt]);
            }

            return new mySeries(ans);
        }

        #endregion


        // ************************************************************
        // OVERRIDDEN OPERATORS 
        // ************************************************************

        #region

        // DIVISION

        public static mySeries operator /(mySeries num, mySeries den)
        {
            mySeries ansNum = num.merge(den);
            mySeries ansDen = den.merge(num);

            mySeries answer = new mySeries();

            foreach (DateTime dt in ansNum.data.Keys)
            {
                answer[dt] = ansNum[dt] / ansDen[dt];
            }

            return answer;

        }

        public static mySeries operator /(mySeries num, double den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in num.data.Keys)
            {
                answer[dt] = num[dt] / den;
            }

            return answer;

        }

        public static mySeries operator /(double num, mySeries den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in den.data.Keys)
            {
                answer[dt] = num / den[dt];
            }

            return answer;

        }


        // MULTIPLICATION

        public static mySeries operator *(mySeries num, mySeries den)
        {
            mySeries ansNum = num.merge(den);
            mySeries ansDen = den.merge(num);

            mySeries answer = new mySeries();

            foreach (DateTime dt in ansNum.data.Keys)
            {
                answer[dt] = ansNum[dt] * ansDen[dt];
            }

            return answer;

        }

        public static mySeries operator *(mySeries num, double den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in num.data.Keys)
            {
                answer[dt] = num[dt] * den;
            }

            return answer;

        }

        public static mySeries operator *(double num, mySeries den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in den.data.Keys)
            {
                answer[dt] = num * den[dt];
            }

            return answer;

        }


        // ADDITION

        public static mySeries operator +(mySeries num, mySeries den)
        {
            mySeries ansNum = num.merge(den);
            mySeries ansDen = den.merge(num);

            mySeries answer = new mySeries();

            foreach (DateTime dt in ansNum.data.Keys)
            {
                answer[dt] = ansNum[dt] + ansDen[dt];
            }

            return answer;

        }

        public static mySeries operator +(mySeries num, double den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in num.data.Keys)
            {
                answer[dt] = num[dt] + den;
            }

            return answer;

        }

        public static mySeries operator +(double num, mySeries den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in den.data.Keys)
            {
                answer[dt] = num + den[dt];
            }

            return answer;

        }



        // SUBTRACTION

        public static mySeries operator -(mySeries num, mySeries den)
        {
            mySeries ansNum = num.merge(den);
            mySeries ansDen = den.merge(num);

            mySeries answer = new mySeries();

            foreach (DateTime dt in ansNum.data.Keys)
            {
                answer[dt] = ansNum[dt] - ansDen[dt];
            }

            return answer;

        }

        public static mySeries operator -(mySeries num, double den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in num.data.Keys)
            {
                answer[dt] = num[dt] - den;
            }

            return answer;

        }

        public static mySeries operator -(double num, mySeries den)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in den.data.Keys)
            {
                answer[dt] = num - den[dt];
            }

            return answer;

        }



        #endregion



        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public double this[DateTime i]
        {
            get { return this.data[i]; }
            protected set { this.data[i] = value; }
        }

        #endregion



        // ************************************************************
        // MATH OPERATIONS
        // ************************************************************

        #region

        // EXPONENTIAL

        public mySeries EXP(double argExp)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in this.data.Keys)
            {
                answer[dt] = Math.Exp(argExp * this[dt]);
            }

            return answer;

        }

        public mySeries EXP()
        {
            return this.EXP(1.0);
        }


        // LOGARITHM LOG
        public mySeries LOG(double argExp)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in this.data.Keys)
            {
                answer[dt] = Math.Log10(this[dt]);
            }

            return answer;

        }


        // LOGARITHM LN
        public mySeries LN(double argExp)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in this.data.Keys)
            {
                answer[dt] = Math.Log(argExp * this[dt]);
            }

            return answer;

        }


        // POWERS
        public mySeries Power(double argExp)
        {

            mySeries answer = new mySeries();

            foreach (DateTime dt in this.data.Keys)
            {
                answer[dt] = Math.Pow(this[dt], argExp);
            }

            return answer;

        }

        public mySeries Sqrt()
        {
            return this.Power(0.5);
        }

        public mySeries Square()
        {
            return this.Power(2.0);
        }


        // CUMULATIVE SUM
        public double CumSum()
        {
            double answer = 0.0;

            foreach (DateTime dt in this.data.Keys)
            {
                answer += this[dt];
            }

            return answer;
        }

        #endregion



        

    }
}



