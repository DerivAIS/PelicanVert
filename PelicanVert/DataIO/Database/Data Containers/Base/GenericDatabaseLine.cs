using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

// External Custom Packages
using BBCOMM = Bloomberglp.Blpapi;

namespace QLyx.DataIO
{

    public abstract class GenericDatabaseLine : IGenericDatabaseLine
    {

        public DateTime Date { get; set; } // virtual keyword is new...

        public myDB Table { get; set; } // virtual keyword is new...

        public virtual List<string> GetAllFields() { return new List<string>(); }
        public virtual List<string> GetDataFields() { return new List<string>(); }
        public virtual List<string> GetKeyFields() { return new List<string>(); }
        public virtual string GetTable() { return ""; }
        public virtual DateTime GetDate() { return new DateTime(); }
        public virtual void SetDate(DateTime myDate) { throw new System.ArgumentException("SetDateException", "Generic database line 'SetDate' must be overridden."); }
        public virtual void SetDBID(DBID myId) { throw new System.ArgumentException("SetDBIDException", "Generic database line 'SetDBID' must be overridden."); }

        public virtual object this[string propertyName]
        {

            get
            {
                PropertyInfo property = GetType().GetProperty(propertyName);
                return property.GetValue(this, null);
            }

            set
            {
                PropertyInfo property = GetType().GetProperty(propertyName);
                property.SetValue(this, value, null);
            }

        }

        public virtual Dictionary<string, int> SetFromBloomberg(ref BBCOMM.Element myElement, Dictionary<string, int> skipFields)
        {
            throw new System.ArgumentException("SetFromBloombergException", "Generic database line 'SetFromBloomberg' must be overridden.");
            return new Dictionary<string, int>();
        }




    }




}
