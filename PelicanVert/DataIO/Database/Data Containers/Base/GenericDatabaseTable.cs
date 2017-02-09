using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO
{

    public class GenericDatabaseTable : IGenericDatabaseTable, IEnumerable, IEnumerator
    {

        // ************************************************************
        // INSTANCE PROPERTIES -- INTERNAL DATA
        // ************************************************************


        protected Dictionary<DateTime, GenericDatabaseLine> _internalData = new Dictionary<DateTime, GenericDatabaseLine>();
        public Dictionary<DateTime, GenericDatabaseLine> internalData
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
        // INDEXORS
        // ************************************************************


        public virtual GenericDatabaseLine this[DateTime dt]
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



        // ENUMERATOR PROPERTIES 
        private int Position = -1;


        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }



        public bool MoveNext()
        {

            if (Position < internalData.Count - 1)
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
                DateTime key = internalData.Keys.ElementAt(Position);
                return key;
            }
        }



        #endregion





    }



}
