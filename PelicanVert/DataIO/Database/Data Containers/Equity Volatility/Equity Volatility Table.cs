using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using System.Collections;
//using QLyx.DataIO.Data_Containers;

namespace QLyx.DataIO
{



    public class EquityVolatility_Table : GenericDatabaseTable
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        protected new Dictionary<DateTime, EquityVolatility_Line> _internalData = new Dictionary<DateTime, EquityVolatility_Line>();
        public new Dictionary<DateTime, EquityVolatility_Line> internalData
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
        public EquityVolatility_Table() { }

        // Constructor : From List
        public EquityVolatility_Table(List<EquityVolatility_Line> MultipleLines) 
        {
            foreach (EquityVolatility_Line singleLine in MultipleLines)
            {
                internalData[singleLine.Date] = singleLine;
            }
        }

        // ************************************************************
        // METHODS
        // ************************************************************


        public void AddFromList(List<EquityVolatility_Line> myList)
        {
            foreach (EquityVolatility_Line myLine in myList)
            {
                _internalData[myLine.Date] = myLine;
            }
        }



        // ************************************************************
        // OUTPUT TO LIST OF DATABASE LINES
        // ************************************************************


        public List<EquityVolatility_Line> ToList()
        {
            return this.internalData.Values.ToList();
        }



        // ************************************************************
        // INDEXORS
        // ************************************************************


        public EquityVolatility_Line this[DateTime dt]
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


        public static explicit operator EquityVolatility_Table(List<EquityVolatility_Line> rawResults)
        {

            // Create Table from Constructor
            return new EquityVolatility_Table(rawResults);

        }


        #endregion

    }


}
