using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

using QLyx.DataIO.Connector;

namespace QLyx.DataIO
{

    // CACHE MANAGEMENT CLASS
    public sealed class ReferenceManager
    {
        
        // ************************************************************
        // PRIVATE CONSTRUCTORS & NESTED CLASS
        // ************************************************************

        #region Private constructors -- do not touch

        private ReferenceManager() {}

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly ReferenceManager instance = new ReferenceManager();


        }


        #endregion



        // ************************************************************
        // PADLOCK
        // ************************************************************

        #region Padlock

        static readonly object padlock = new object();

        #endregion



        // ************************************************************
        // FACTORY 
        // ************************************************************

        #region

        /// <summary>
        /// Method used ton instanciate the ReferenceManager class.
        /// </summary>
        public static ReferenceManager Factory { get { return Nested.instance; } }

        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- DELEGATES & SUPPORT OBJECTS
        // ************************************************************

        // DATABSE CONNECTOR
        #region Database Connector

        private myDB_Connector _SQLconnector = new myDB_Connector();
        public myDB_Connector SQLconnector
        {
            get
            {
                return _SQLconnector;
            }

            private set
            {
                _SQLconnector = value;
            }
        }

        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- REFERENCE TABLE
        // ************************************************************


        #region Reference Table

        private List<IDtoken> _referenceTable;
        public List<IDtoken> referenceTable
        {
            get
            {
                if (_referenceTable == null) { LoadReferenceTableInMemory(); }
                return _referenceTable;
            }

            private set
            {
                this._referenceTable = value;
            }
        }


        #endregion



        // ************************************************************
        // METHODS
        // ************************************************************


        private void LoadReferenceTableInMemory()
        {
            _referenceTable = SQLconnector.Reference();
        }
        
        public Dictionary<string, string> GetExternalIdentifiers(DBID DBID_)
        {
            myDB_Connector dbCom = new myDB_Connector();
            IDtoken refLine = dbCom.Reference(DBID_);

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string s in refLine.GetExternalIdentifiers())
            {

                result[s] = (string)refLine[s];
            }


            return result;

        }

        public List<IDtoken> GetAllLinesForString(string field, string value)
        {
            // Initialize a new list of results
            List<IDtoken> result = new List<IDtoken>();

            // Loop through the ref table in memory
            foreach (IDtoken line in referenceTable)
            {
                if ((string)line[field] == value) { result.Add(line); }
            }

            // Return results
            return result;

        }

     
        public IDtoken Identify(DBID dbid)
        {

            // IDtoken result = TokenFactory.New(dbid);

            // Loop through the ref table in memory
            foreach (IDtoken line in referenceTable)
            {
                if ((int)line["DBID"] == dbid.ToInt()) 
                { 
                    return line;
                    
                }
            }

            // Return results
            throw new System.ArgumentException("IDException", "Invalid DBID provided to reference manager."); 

        }

        public IDtoken Identify(int dbid)
        {
            return Identify(new DBID(dbid));
        }
     

        public object GetReferenceItem(DBID id, string key)
        {

            object result = new object();

            // Loop through the ref table in memory
            foreach (IDtoken line in referenceTable)
            {
                if (line["DBID"] == id) { result = line[key]; }
            }

            // Return results
            return result;

        }


        public DBID GetNewDBID()
        {
            int k = referenceTable.Count();
            return new DBID(k + 1);
        }



        public void InsertReferenceLine(IDtoken newRefLine)
        {

            // Loop through the ref table in memory
            foreach (IDtoken line in referenceTable)
            {
                //Console.WriteLine("item:{0}", (int)line.DBID);
                if ((int)line["DBID"] == newRefLine.DBID)
                { throw new System.ArgumentException("ReferenceManager", "DBID to be inserted is already in use."); }
            }

            referenceTable.Add(newRefLine);
            _SQLconnector.Insert<IDtoken>(newRefLine, new List<IDtoken>() { newRefLine }, includeDate: false);

        }



        /*
        public object PopItem(string key, bool remove)
        {
            lock (padlock)
            {
                //var res = base.Get(key);

                var res = "...";
                if (res != null)
                {
                    if (remove == true) { } /// base.Remove(key); }
                }
                else
                {
                    //WriteToLog("CachingProvider-GetItem: Don't contains key: " + key);
                }

                return res;
   
            }
                
                
        }

        */

    }


}
