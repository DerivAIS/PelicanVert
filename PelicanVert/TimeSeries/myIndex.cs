using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Containers
{

    public class Index
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        #region

        // DATA
        protected SortedSet<DateTime> _data;
        public SortedSet<DateTime> data
        {
            get { return _data; }
            protected set { _data = value; }
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


        // DATETIME COUNT
        protected int _length;
        public int length
        {
            get { return _length; }
            protected set { _length = value; }
        }



        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public Index() { }


        // Constructor 2 : From other hashset
        public Index(SortedSet<DateTime> argHash)
        {
            // Set the internal data
            this.data = argHash;

            // Update start datetime
            this.startDate = this.data.First();

            // Update end datetime
            this.endDate = this.data.Last();

            // Count the number of points
            this.length = this.data.Count();

        }


        #endregion



        // ************************************************************
        // COMMON METHODS 
        // ************************************************************

        #region

        // COUNT
        public int count()
        {
            return this.data.Count();
        }

        #endregion





    }
}



