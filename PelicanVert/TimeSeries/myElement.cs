using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;

namespace QLyx.Containers
{
    public class myElement
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        

        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        #region


        // DATA
        protected Dictionary<String, Double?> _data;
        public Dictionary<String, Double?> data
        {
            get
            {
                if (_data == null) { _data = new Dictionary<String, Double?>(); }
                return _data;
            }

            protected set 
            { 
                _data = value; 
            }

        }


                

        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myElement() { }


        // Constructor 2 : all items from Dict
        public myElement(Dictionary<String, Double?> items)
        {
            data =  items;
        }


        #endregion


        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public Double? this[string i]
        {
            get { return this.data[i]; }
        }

        #endregion




    }
}



