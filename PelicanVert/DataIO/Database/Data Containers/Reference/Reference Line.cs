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


    public class IDtoken : GenericDatabaseLine
    {


        // ************************************************************
        // PROPERTIES -- ASSOCIATED TABLE
        // ************************************************************

        #region

        // Table
        public new myDB Table = myDB.Reference;

        #endregion



        // ************************************************************
        // PROPERTIES -- TO COMPLY WITH INTERFACE
        // ************************************************************

        #region Irrelevant in this case : to comply with interface

        // Date
        public new DateTime Date { get; set; }

        #endregion


        // ************************************************************
        // PROPERTIES -- KEYS
        // ************************************************************

        #region DBID

        protected DBID _DBID;
        public int DBID
        {
            get
            {
                return _DBID.ToInt();
            }

            protected set
            {
                _DBID = new DBID(value);
            }
        }


        public override void SetDBID(DBID dbid)
        {
            this._DBID = dbid;
        }



        #endregion


        // ************************************************************
        // PROPERTIES -- DATA -- SOURCES
        // ************************************************************

        #region Sources

        // SOURCE 1
        public string Source1 { get; set; }

        // SOURCE 2
        public string Source2 { get; set; }

        // SOURCE 3
        public string Source3 { get; set; }

        // SOURCE 4
        public string Source4 { get; set; }

        // SOURCE 5
        public string Source5 { get; set; }

        #endregion


        // ************************************************************
        // PROPERTIES -- DATA -- EXTERNAL IDENTIFIERS
        // ************************************************************

        #region External Identifiers


        // BLOOMBERG
        public string Bloomberg { get; set; }

        // REUTERS
        public string Reuters { get; set; }

        // SOPHIS
        public string Sophis { get; set; }

        // MSD
        public string MSD { get; set; }

        // MARKET MAP
        public string MarketMap { get; set; }

        // MARKIT
        public string Markit { get; set; }

        // FRED
        public string FRED { get; set; }

        // YAHOO
        public string Yahoo { get; set; }

        // GOOGLE
        public string Google { get; set; }

        // ISIN
        public string ISIN { get; set; }


        #endregion



        // ************************************************************
        // PROPERTIES -- DATA -- CURRENCY, COUNTRY, NAME (STRING)
        // ************************************************************

        #region Descriptive 


        // Currency ISO code (string)
        public string Currency { get; set; }

        // Country (string)
        public string Country { get; set; }

        // Timezone (string)
        public string Timezone { get; set; }

        // Name
        public string Name { get; set; }

        // Asset Class
        public string AssetClass { get; set; }


        #endregion



        // ************************************************************
        // PROPERTIES -- DATA -- FINANCIAL REFERENCE ASSETS/INDICES
        // ************************************************************

        #region Financial Reference


        // SECTOR

        // Sector (string)
        public string SectorName { get; set; }

        // Sector (int)
        public string SectorID { get; set; }



        // SUB-SECTOR

        // Subsector (string)
        public string SubsectorName { get; set; }

        // Subsector (int)
        public string SubsectorID { get; set; }



        // BENCHMARK

        // Benchmark (string)
        public int BenchmarkName { get; set; }

        // Benchmark ID (int)
        public int BenchmarkID { get; set; }



        // UNDERLYING

        // Underlying (string)
        public int UnderlyingName { get; set; }

        // Underlying ID (int)
        public int UnderlyingID { get; set; }
        

        #endregion


        // ************************************************************
        // PROPERTIES -- DATA -- EXCHANGES
        // ************************************************************

        #region Exchange(s)


        // EXCHANGE 1
        public string Exchange1 { get; set; }

        // EXCHANGE 2
        public string Exchange2 { get; set; }

        // EXCHANGE 3
        public string Exchange3 { get; set; }

        // EXCHANGE 4
        public string Exchange4 { get; set; }

        // EXCHANGE 5
        public string Exchange5 { get; set; }


        #endregion


        // ************************************************************
        // PROPERTIES -- DATA -- CALENDARS
        // ************************************************************

        #region Calendar(s)


        // Calendar 1
        public string Calendar1 { get; set; }

        // Calendar 2
        public string Calendar2 { get; set; }

        // Calendar 3
        public string Calendar3 { get; set; }

        // Calendar 4
        public string Calendar4 { get; set; }

        // Calendar 5
        public string Calendar5 { get; set; }


        #endregion



        // ************************************************************
        // PROPERTIES -- DATA -- BOOLEAN
        // ************************************************************

        #region Booleans


        // INDEX? 
        public bool isIndex { get; set; }

        // OPTION?
        public bool isOption { get; set; }

        // ETF?
        public bool isETF { get; set; }

        // MUTUAL FUND? 
        public bool isMutualFund { get; set; }

        // ADR?
        public bool isADR { get; set; }


        #endregion



        // ************************************************************
        // PROPERTIES -- DATA -- OPTION RELATED
        // ************************************************************

        #region Option related

        // Option spacing
        public double OptionSpacing { get; set; }

        // Option type
        public string ListedOptionType { get; set; }


        #endregion



        // ************************************************************
        // PROPERTIES -- DATA -- TABLE MANAGEMENT
        // ************************************************************

        #region Table Management --> Matched on the existing tables


        // Last Update of the EQUITY table in myDB
        public DateTime HistoryTableLastUpdate { get; set; }

        // Last Update of the INTEREST RATE table in myDB
        public string HistoryTable { get; set; }

        // Number of times the algo has tried to update un-successfully
        public int HistoryRetry { get; set; }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors


        // Constructor 1 : Generic
        public IDtoken() { }

        // Constructor 2 : Keys only
        public IDtoken(int argDBID)
        {
            DBID = argDBID;
        }

        
        #endregion


        // ************************************************************
        // METHODS -- FACILITATE mySQL MANAGEMENT
        // ************************************************************


        #region

  
        public override List<string> GetDataFields()
        {
            return new List<string>() { 
                
                "Source1", "Source2", "Source3", "Source4", "Source5",              // sources
                "Exchange1", "Exchange2", "Exchange3", "Exchange4", "Exchange5",    // exchanges
                "Bloomberg", "Reuters", "Sophis", "MSD", "MarketMap",               // external id's (1 of 2)
                "Markit", "FRED", "Yahoo", "Google", "ISIN",                        // external id's (2 of 2)
                "Currency", "Country", "Name", "Timezone", "AssetClass",            // descriptive
                "SectorName", "SectorID", "SubsectorName", "SubsectorID",           // sector & subsector
                "BenchmarkName", "BenchmarkID", "UnderlyingName", "UnderlyingID",   // descriptive
                "Calendar1", "Calendar2", "Calendar3", "Calendar4", "Calendar5",    // calendars
                "isIndex", "isOption", "isETF", "isADR", "isMutualFund",            // booleans
                "OptionSpacing", "ListedOptionType",                                // option-related
                "HistoryTableLastUpdate", "HistoryTable", "HistoryRetry"            // DB management-related
            
            };
        }



        public override List<string> GetKeyFields()
        {
            return new List<string>() { 

                "DBID"

            };
        }


        public DBID GetKey()
        {
            return _DBID;
        }


        public override string GetTable()
        {
            return this.Table.ToString();
        }


        public List<string> GetExternalIdentifiers()
        {
            return new List<string>() 
            {
                "Bloomberg", "Reuters", "Sophis", "MSD", "MarketMap",
                "Markit", "FRED", "Yahoo", "Google", "ISIN" 
            };
        }


        public override List<string> GetAllFields()
        {
            return new List<string>() { 
                
                "DBID",                                                             // Keys

                "Source1", "Source2", "Source3", "Source4", "Source5",              // sources
                "Exchange1", "Exchange2", "Exchange3", "Exchange4", "Exchange5",    // exchanges
                "Bloomberg", "Reuters", "Sophis", "MSD", "MarketMap",               // external id's (1 of 2)
                "Markit", "FRED", "Yahoo", "Google", "ISIN",                        // external id's (2 of 2)
                "Currency", "Country", "Name", "Timezone", "AssetClass",            // descriptive
                "SectorName", "SectorID", "SubsectorName", "SubsectorID",           // sector & subsector
                "BenchmarkName", "BenchmarkID", "UnderlyingName", "UnderlyingID",   // descriptive
                "Calendar1", "Calendar2", "Calendar3", "Calendar4", "Calendar5",    // calendars
                "isIndex", "isOption", "isETF", "isADR", "isMutualFund",            // booleans
                "OptionSpacing", "ListedOptionType",                                // option-related
                "HistoryTableLastUpdate", "HistoryTable", "HistoryRetry"            // DB management-related
            
            };
        }


        #endregion



        // ************************************************************
        // MY OWN SPECIAL INDEXER...
        // ************************************************************

        #region Indexer


        public new object this[string propertyName]
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

        #endregion


    }

}
