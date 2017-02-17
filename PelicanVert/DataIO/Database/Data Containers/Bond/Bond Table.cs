using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using System.Collections;

namespace QLyx.DataIO
{



    public class Bond_Table : GenericDatabaseTable
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        // THIS NEEDS TO BE A DICT

        protected new Dictionary<DateTime, Bond_Line> _internalData = new Dictionary<DateTime, Bond_Line>();
        public new Dictionary<DateTime, Bond_Line> internalData
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
        public Bond_Table() { }

        // Constructor : From List
        public Bond_Table(List<Bond_Line> MultipleLines) 
        {
            foreach (Bond_Line singleLine in MultipleLines)
            {
                internalData[singleLine.Date] = singleLine;
            }
        }


        // ************************************************************
        // METHODS
        // ************************************************************


        public void AddFromList(List<Bond_Line> myList)
        {
            foreach(Bond_Line myLine in myList)
            {
                _internalData[myLine.Date] = myLine;
            }
        }

        // ************************************************************
        // OUTPUT TO LIST OF DATABASE LINES
        // ************************************************************


        public List<Bond_Line> ToList()
        {
            return this.internalData.Values.ToList();
        }


        // ************************************************************
        // INDEXORS
        // ************************************************************


        public new Bond_Line this[DateTime dt]  
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



        public new void Reset()
        {
            Position = -1;
        }



        public new object Current
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


        public static explicit operator Bond_Table(List<Bond_Line> rawResults)
        {
            // Create Table from Constructor
            return new Bond_Table(rawResults);
        }


        #endregion





    }


}
