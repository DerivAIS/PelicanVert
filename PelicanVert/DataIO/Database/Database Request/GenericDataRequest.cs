using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO
{
    public class GenericDataRequest
    {



        // ************************************************************
        // CONSTRUCTOR
        // ************************************************************

        public GenericDataRequest() { }

        public GenericDataRequest(IDtoken Id_, List<string> Fields_, DateTime StartDate_, DateTime EndDate_, 
            TimeUnit Periodicity_ = TimeUnit.Days, string Source_ = "Bloomberg") 
        {
            // Set the id token
            id = Id_;

            // Set the fields requested
            fields = Fields_;

            // Set start and end dates
            startDate = StartDate_;
            endDate = EndDate_;

            // Set the data periodicity/frequency (optional)
            periodicity = Periodicity_;

            // Set the favorite source
            source = Source_;

        }



        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        // IDTOKEN
        #region Securities identification

        protected IDtoken _id;
        public IDtoken id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        #endregion


        // START DATE
        #region Start Date (inclusive)

        protected DateTime _startDate = new DateTime(2013, 12, 31);
        public DateTime startDate
        {
            get { return _startDate; }
            protected set { _startDate = value; }
        }

        #endregion


        // END DATE
        #region End Date (inclusive)

        protected DateTime _endDate = DateTime.Today;
        public DateTime endDate
        {
            get { return _endDate; }
            protected set { _endDate = value; }
        }

        #endregion


        // PERIODICITY
        #region Data frequency

        protected TimeUnit _periodicity = TimeUnit.Days;
        public TimeUnit periodicity
        {
            get { return _periodicity; }
            protected set { _periodicity = value; }
        }

        #endregion


        // FIELDS
        #region Securities identification

        protected List<string> _fields = new List<string>();
        public List<string> fields
        {
            get { return _fields; }
            protected set { _fields = value; }
        }

        #endregion


        // DATA SOURCE (PREFERED)
        #region Data Source

        protected string _source;
        public string source
        {
            get { return _source; }
            protected set { _source = value; }
        }

        #endregion







    }
}
