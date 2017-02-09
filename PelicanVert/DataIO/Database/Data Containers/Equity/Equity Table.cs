using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using System.Collections;

namespace QLyx.DataIO
{



    public class Equity_Table : GenericDatabaseTable
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        // THIS NEEDS TO BE A DICT

        protected new Dictionary<DateTime, Equity_Line> _internalData = new Dictionary<DateTime, Equity_Line>();
        public new Dictionary<DateTime, Equity_Line> internalData
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
        public Equity_Table() { }

        // Constructor : From List
        public Equity_Table(List<Equity_Line> MultipleLines) 
        {
            foreach (Equity_Line singleLine in MultipleLines)
            {
                internalData[singleLine.Date] = singleLine;
            }
        }


        // ************************************************************
        // METHODS
        // ************************************************************


        public void AddFromList(List<Equity_Line> myList)
        {
            foreach(Equity_Line myLine in myList)
            {
                _internalData[myLine.Date] = myLine;
            }
        }

        // ************************************************************
        // OUTPUT TO LIST OF DATABASE LINES
        // ************************************************************


        public List<Equity_Line> ToList()
        {
            return this.internalData.Values.ToList();
        }


        // ************************************************************
        // INDEXORS
        // ************************************************************


        public new Equity_Line this[DateTime dt]  
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


        public static explicit operator Equity_Table(List<Equity_Line> rawResults)
        {

            // Create Table from Constructor
            return new Equity_Table(rawResults);

        }


        #endregion





    }


}
