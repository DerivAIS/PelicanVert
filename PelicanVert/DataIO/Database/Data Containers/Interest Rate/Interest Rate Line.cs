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


    public class InterestRate_Line : GenericDatabaseLine
    {


        // ************************************************************
        // PROPERTIES -- ASSOCIATED TABLE
        // ************************************************************

        #region

        // Table
        public new myDB Table = myDB.InterestRate;

        #endregion



        // ************************************************************
        // PROPERTIES -- KEYS
        // ************************************************************

        #region

        // Date
        public new DateTime Date { get; set; }

        public override DateTime GetDate()
        {
            return this.Date;
        }

        // DBID
        public int DBID { get; set; }


        #endregion


        // ************************************************************
        // PROPERTIES -- DATA
        // ************************************************************

        #region Data fields

        // bid
        public double? Bid { get; set; }

        // Ask
        public double? Ask { get; set; }

        // Last (required for EONIA)
        public double? Last { get; set; }

        #endregion


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public InterestRate_Line() { }

        // Constructor 2 : Date Only
        public InterestRate_Line(DateTime currentDate)
        {
            Date = currentDate;
        }

        // Constructor 3 : Keys only
        public InterestRate_Line(DateTime currentDate, DBID ticker)
        {
            Date = currentDate;
            DBID = ticker.ToInt();
        }

        // Constructor 4 : Keys + Data
        public InterestRate_Line(DateTime currentDate, DBID ticker, double Bid_, double Ask_, double Last_)
        {

            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            Bid = Bid_;       
            Ask = Ask_;
            Ask = Last_;

        }

        // Constructor 5 : From Dict of nullable doubles
        public InterestRate_Line(DateTime currentDate, DBID ticker, Dictionary<String, Double?> data)
        {

            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            Bid = data["Bid"];
            Ask = data["Ask"];
            Last = data["Last"];
            
        }


        #endregion



        // ************************************************************
        // BLOOMBERG
        // ************************************************************

        #region

        private static readonly string BID = "BID";
        private static readonly string ASK = "ASK";
        private static readonly string LAST = "PX_LAST";

        private static readonly int MAX_RETRY = 10;

        public override Dictionary<string, int> SetFromBloomberg(ref BBCOMM.Element myElement, Dictionary<string, int> skipFields)
        {

            if (skipFields[BID] < MAX_RETRY) { SetBid(ref myElement, ref skipFields); };
            if (skipFields[ASK] < MAX_RETRY) { SetAsk(ref myElement, ref skipFields); };
            if (skipFields[LAST] < MAX_RETRY) { SetLast(ref myElement, ref skipFields); };

            return skipFields;

        }

        protected void SetBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                // Warning : Bloomberg displays interest rates in percentage terms
                Bid = 0.01 * (double)myElement.GetElementAsFloat64(BID);
            }

            catch
            {
                // Some log ?
                skipFields[BID] += 1;
                Bid = 0.0;
            }

        }

        protected void SetAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                // Warning : Bloomberg displays interest rates in percentage terms
                Ask = 0.01 * (double)myElement.GetElementAsFloat64(ASK);
            }

            catch
            {
                // Some log ?
                skipFields[ASK] += 1;
                Ask = 0.0;
            }

        }

        protected void SetLast(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                // Warning : Bloomberg displays interest rates in percentage terms
                Last = 0.01 * (double)myElement.GetElementAsFloat64(LAST);
            }

            catch
            {
                // Some log ?
                skipFields[LAST] += 1;
                Last = 0.0;
            }

        }

        #endregion


        // ************************************************************
        // METHODS -- FACILITATE mySQL MANAGEMENT
        // ************************************************************


        #region


        public override List<string> GetDataFields()
        {
            return new List<string>() { 
                
                "Bid", "Ask", "Last" 
            
            };
        }


        public override List<string> GetKeyFields()
        {
            return new List<string>() { 

                "Date", 
                "DBID"

            };
        }

        public override List<string> GetAllFields()
        {
            return new List<string>() { 

                "Date", 
                "DBID",
                "Bid", 
                "Ask",
                "Last"

            };
        }


        public override string GetTable()
        {
            return this.Table.ToString();
        }


        public override void SetDate(DateTime myDate)
        {
            this.Date = myDate;
        }


        public override void SetDBID(DBID dbid)
        {
            this.DBID = dbid;
        }

        #endregion


        // ************************************************************
        // INDEXOR
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



        // ************************************************************
        // OUTPUT
        // ************************************************************

        #region Output


        public Dictionary<String, Double?> ToDict()
        {

            return new Dictionary<String, Double?>() { { "Bid", Bid }, { "Ask", Ask }, { "Last", Last } };

        }

        #endregion






    }

}
