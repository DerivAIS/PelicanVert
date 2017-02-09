using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using System.Collections;

namespace QLyx.DataIO
{



    public class InterestRate_Table : GenericDatabaseTable
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        // THIS NEEDS TO BE A DICT

        protected new Dictionary<DateTime, InterestRate_Line> _internalData = new Dictionary<DateTime, InterestRate_Line>();
        public new Dictionary<DateTime, InterestRate_Line> internalData
        {
            get
            {
                return _internalData;
            }

            protected set
            {
                _internalData = value;
            }
        }


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        // Constructor : Generic
        public InterestRate_Table() { }

        // Constructor : From List
        public InterestRate_Table(List<InterestRate_Line> MultipleLines) 
        {
            foreach (InterestRate_Line singleLine in MultipleLines)
            {
                internalData[singleLine.Date] = singleLine;
            }
        }


        // ************************************************************
        // METHODS
        // ************************************************************


        public void AddFromList(List<InterestRate_Line> myList)
        {
            foreach (InterestRate_Line myLine in myList)
            {
                _internalData[myLine.Date] = myLine;
            }
        }



        // ************************************************************
        // OUTPUT TO LIST OF DATABASE LINES
        // ************************************************************


        public List<InterestRate_Line> ToList()
        {
            return this.internalData.Values.ToList();
        }

        // ************************************************************
        // INDEXORS
        // ************************************************************


        public InterestRate_Line this[DateTime dt]
        {
            get
            {
                return _internalData[dt];
            }

            set
            {
                _internalData[dt] = value;
            }
        }





        // ************************************************************
        // ENUMERATOR-RELATED METHODS
        // ************************************************************

        #region Enumerators


        private int Position = -1;


        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }



        public bool MoveNext()
        {

            if (Position < _internalData.Count - 1)
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
                return _internalData.ElementAt(Position);
            }
        }

        #endregion






        // ************************************************************
        // CASTS
        // ************************************************************

        #region Casts


        public static explicit operator InterestRate_Table(List<InterestRate_Line> rawResults)
        {

            // Create Table from Constructor
            InterestRate_Table myTable = new InterestRate_Table(rawResults);
            return myTable;
        }


        #endregion






    }


}
