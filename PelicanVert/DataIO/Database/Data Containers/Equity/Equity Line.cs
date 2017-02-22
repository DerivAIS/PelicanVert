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


    public class Equity_Line : GenericDatabaseLine
    {


        // ************************************************************
        // PROPERTIES -- ASSOCIATED TABLE
        // ************************************************************

        #region

        // Table
        public new myDB Table = myDB.Equity;

        #endregion



        // ************************************************************
        // PROPERTIES -- KEYS
        // ************************************************************

        #region

        // Date
        public new DateTime Date { get; set; }

        // Ticker
        public int DBID { get; set; }


        #endregion


        // ************************************************************
        // PROPERTIES -- DATA
        // ************************************************************

        #region Data fields

        // Open
        public double? Open { get; set; }

        // High
        public double? High { get; set; }

        // Low
        public double? Low { get; set; }

        // Close
        public double? Close { get; set; }

        // Volume
        public double? Volume { get; set; }

        // Bid
        public double? Bid { get; set; }

        // Ask
        public double? Ask { get; set; }

        // AdjustedClose
        public double? AdjustedClose { get; set; }

        #endregion


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public Equity_Line() { }

        // Constructor 2 : Date Only
        public Equity_Line(DateTime currentDate)
        {
            Date = currentDate;
        }

        // Constructor 3 : Keys only
        public Equity_Line(DateTime currentDate, DBID ticker)
        {
            Date = currentDate;
            DBID = ticker.ToInt();
        }

        // Constructor 4 : Keys + Data
        public Equity_Line(DateTime currentDate, DBID ticker, double argOpen, 
                                double argHigh, double argLow, double argClose, double argBid, 
                                double argAsk, double argVolume, double argAdjClose)
        {
            
            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            Open = argOpen;
            High = argHigh;
            Low = argLow;
            Close = argClose;
            Bid = argBid;
            Ask = argAsk;
            Volume = argVolume;    
            AdjustedClose = argAdjClose;
            
        }

        // Constructor 5 : From Dict of nullable doubles
        public Equity_Line(DateTime currentDate, DBID ticker, Dictionary<String, Double?> data)
        {

            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            Open = data["Open"];
            High = data["High"];
            Low = data["Low"];
            Close = data["Close"];
            Bid = data["Bid"];
            Ask = data["Ask"];
            Volume = data["Volume"];
            AdjustedClose = data["AdjustedClose"];

        }

        #endregion


        // ************************************************************
        // BLOOMBERG
        // ************************************************************

        #region

        private static readonly string OPEN = "PX_OPEN";
        private static readonly string HIGH = "PX_HIGH";
        private static readonly string LOW = "PX_LOW";
        private static readonly string CLOSE = "PX_LAST";
        private static readonly string BID = "BID";
        private static readonly string ASK = "ASK";
        private static readonly string VOLUME = "VOLUME";
        private static readonly string ADJCLOSE = "TOT_RET_INDEX_NET_DVDS";


        private static readonly int MAX_RETRY = 10;

        // added override to kill warning
        public override Dictionary<string, int> SetFromBloomberg(ref BBCOMM.Element myElement, Dictionary<string, int> skipFields)
        {


            if (skipFields[OPEN] < MAX_RETRY) { SetOpen(ref myElement, ref skipFields); };
            if (skipFields[HIGH] < MAX_RETRY) { SetHigh(ref myElement, ref skipFields); };
            if (skipFields[LOW] < MAX_RETRY) { SetLow(ref myElement, ref skipFields); };
            if (skipFields[CLOSE] < MAX_RETRY) { SetClose(ref myElement, ref skipFields); };
            if (skipFields[BID] < MAX_RETRY) { SetBid(ref myElement, ref skipFields); };
            if (skipFields[ASK] < MAX_RETRY) { SetAsk(ref myElement, ref skipFields); };
            if (skipFields[VOLUME] < MAX_RETRY) { SetVolume(ref myElement, ref skipFields); };
            if (skipFields[ADJCLOSE] < MAX_RETRY) { SetAdjClose(ref myElement, ref skipFields); };

            return skipFields;

        }

        protected void SetOpen(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Open = (double)myElement.GetElementAsFloat64(OPEN);
            }

            catch
            {
                // Some log ?
                skipFields[OPEN] += 1;
                Open = Double.NaN;
            }

        }
        protected void SetHigh(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                High = (double)myElement.GetElementAsFloat64(HIGH);
            }

            catch
            {
                // Some log ?
                skipFields[HIGH] += 1;
                High = Double.NaN;
            }

        }
        protected void SetLow(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Low = (double)myElement.GetElementAsFloat64(LOW);
            }

            catch
            {
                // Some log ?
                skipFields[LOW] += 1;
                Low = Double.NaN;
            }

        }
        protected void SetClose(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Close = (double)myElement.GetElementAsFloat64(CLOSE);
            }

            catch
            {
                // Some log ?
                skipFields[CLOSE] += 1;
                Close = Double.NaN;
            }

        }
        protected void SetBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Bid = (double)myElement.GetElementAsFloat64(BID);
            }

            catch
            {
                // Some log ?
                skipFields[BID] += 1;
                Bid = Double.NaN;
            }

        }
        protected void SetAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Ask = (double)myElement.GetElementAsFloat64(ASK);
            }

            catch
            {
                // Some log ?
                skipFields[ASK] += 1;
                Ask = Double.NaN;
            }

        }
        protected void SetVolume(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Volume = (double)myElement.GetElementAsFloat64(VOLUME);
            }

            catch
            {
                // Some log ?
                skipFields[VOLUME] += 1;
                Volume = Double.NaN;
            }

        }
        protected void SetAdjClose(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                Volume = (double)myElement.GetElementAsFloat64(VOLUME);
            }

            catch
            {
                // Some log ?
                skipFields[ADJCLOSE] += 1;
                Volume = Double.NaN;
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
                
                "Open", 
                "High",
                "Low",
                "Close", 
                "Volume",
                "Bid",                 
                "Ask", 
                "AdjustedClose" 
            
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

                // Keys
                "Date",
                "DBID",

                // Values
                "Open", 
                "High",
                "Low",
                "Close", 
                "Volume",
                "Bid",                 
                "Ask", 
                "AdjustedClose" 

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

            return new Dictionary<String, Double?>() 
            { 
            { "Open", Open }, { "High", High },
            { "Low", Low }, { "Close", Close },
            { "Bid", Bid }, { "Ask", Ask },
            { "Volume", Volume }, { "AdjustedClose", AdjustedClose }
            };

        }

        #endregion



        // ************************************************************
        // SANITY CHECKS
        // ************************************************************

        public bool isNull()
        {
            if (Open == null && Close == null && Low == null && High == null) { return true; }
            return false;
        }


    }

}
