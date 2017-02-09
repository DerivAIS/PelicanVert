using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO
{
    public static class TokenFactory
    {

        public static IDtoken New(int DBID)
        {
            if (DBID < 1) { throw new System.ArgumentException("TokenFactory", "Invalid DBID provided to factory."); }

            ReferenceManager _referenceManager = ReferenceManager.Factory;
            return _referenceManager.Identify(DBID);
        }



        public static IDtoken New(string Bloomberg = "", string Reuters = "", string Sophis = "",
                                        string MSD = "", string MarketMap = "", string Markit = "",
                                        string FRED = "", string Yahoo = "", string Google = "", string ISIN = "")
        {

            Dictionary<string, string> _extIds = new Dictionary<string, string>()
            {

            {"Bloomberg",   Bloomberg.ToUpper()}, // Careful with the upper/lower case...
            {"Reuters",     Reuters},
            {"Sophis",      Sophis},
            {"MSD",         MSD},
            {"MarketMap",   MarketMap},
            {"Markit",      Markit},    
            {"FRED",        FRED},                    
            {"Yahoo",       Yahoo},
            {"Google",      Google},
            {"ISIN",        ISIN}

            };


            // Create container for potential matching lines
            List<IDtoken> results = new List<IDtoken>();

            // Load Reference Manager singleton
            ReferenceManager _referenceManager = ReferenceManager.Factory;

            // Retreive all matching lines
            foreach (string source in _extIds.Keys)
            {
                if (!(_extIds[source] == ""))
                {
                    List<IDtoken> tmp = _referenceManager.GetAllLinesForString(source, _extIds[source]);
                    results = results.Union(tmp).ToList();
                }

            }


            // To make sure this matches uniquely a security in the reference table 
            if (!(results.Count == 1)) { throw new System.ArgumentException("TokenFactory", "External IDs match multiple assets, unable to identify."); }


            // Return the matching line
            return results.FirstOrDefault();


        }


        public static IDtoken GenerateDBID(DBID ReferenceDBID, string Bloomberg = "", string Reuters = "", string Sophis = "",
                                 string MSD = "", string MarketMap = "", string Markit = "",
                                 string FRED = "", string Yahoo = "", string Google = "", string ISIN = "")
        {

            Dictionary<string, string> _extIds = new Dictionary<string, string>()
            {

            {"Bloomberg",   Bloomberg.ToUpper()}, // Careful with the upper/lower case...
            {"Reuters",     Reuters},
            {"Sophis",      Sophis},
            {"MSD",         MSD},
            {"MarketMap",   MarketMap},
            {"Markit",      Markit},    
            {"FRED",        FRED},                    
            {"Yahoo",       Yahoo},
            {"Google",      Google},
            {"ISIN",        ISIN}

            };


            // Create container for potential matching lines
            // List<IDtoken> results = new List<IDtoken>();

            // Load Reference Manager (singleton)
            ReferenceManager _referenceManager = ReferenceManager.Factory;

            // Get a new DBID from Reference Manager (= max of column DBID + 1)
            DBID newDBID = _referenceManager.GetNewDBID();

            // Replace DBID from existing token
            IDtoken refToken = TokenFactory.New(ReferenceDBID);

            // New token for output
            IDtoken newToken = new IDtoken();
            newToken.SetDBID(newDBID);

            // Replace all sources
            foreach (string source in _extIds.Keys)
            {
                newToken[source] = _extIds[source];
            }

            // Return the new token
            return newToken;

        }

    }
}
