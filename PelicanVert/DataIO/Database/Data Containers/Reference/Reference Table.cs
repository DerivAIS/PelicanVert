using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using System.Collections;

namespace QLyx.DataIO
{



    public class Reference_Table : GenericDatabaseTable
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        protected Dictionary<DBID, IDtoken> _internalData = new Dictionary<DBID, IDtoken>();
        public Dictionary<DBID, IDtoken> internalData
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
        public Reference_Table() { }



        // ************************************************************
        // METHODS
        // ************************************************************


        public void AddFromList(List<IDtoken> myList)
        {
            foreach (IDtoken myLine in myList)
            {
                _internalData[myLine.GetKey()] = myLine;
            }
        }


        // ************************************************************
        // INDEXORS
        // ************************************************************


        public IDtoken this[DBID dt]
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

        /// <summary>
        /// Provides explicit cast of a Reference Table into a list of Reference Lines.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static explicit operator List<IDtoken>(Reference_Table x)
        {

            List<IDtoken> output = new List<IDtoken>();

            foreach(DBID i in x._internalData.Keys)
            {
                output.Add(x._internalData[i]);
            }

            return output;
        }


        /// <summary>
        /// Provides explicit cast of a list of Reference Lines into a Reference Table.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static explicit operator Reference_Table(List<IDtoken> x)
        {

            Reference_Table output = new Reference_Table();

            foreach (IDtoken line in x)
            {
                output._internalData[line.GetKey()] = line;
            }

            return output;
        }


        #endregion





    }


}
