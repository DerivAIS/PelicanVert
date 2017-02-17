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


    public class Bond_Line : GenericDatabaseLine
    {


        // ************************************************************
        // PROPERTIES -- ASSOCIATED TABLE
        // ************************************************************

        #region

        // Table
        public new myDB Table = myDB.Bond;

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

        // CLEAN PX

        // Clean Price Bid
        public double? CleanPriceBid { get; set; }

        // Clean Price Ask
        public double? CleanPriceAsk { get; set; }

        // Clean Price Mid
        public double? CleanPriceMid { get; set; }


        // DIRTY PX

        // Clean Price Bid
        public double? DirtyPriceBid { get; set; }

        // Clean Price Ask
        public double? DirtyPriceAsk { get; set; }

        // Clean Price Mid
        public double? DirtyPriceMid { get; set; }


        // YIELDS & SPREADS

        // YTM Bid
        public double? YieldToMaturityBid { get; set; }

        // YTM Ask
        public double? YieldToMaturityAsk { get; set; }

        // YTM Mid
        public double? YieldToMaturityMid { get; set; }


        // ASSET SWAP SPREADS

        // Asset Swap Spread
        public double? AssetSwapSpreadBid { get; set; }

        // Asset Swap Spread
        public double? AssetSwapSpreadAsk { get; set; }
        

        // Asset Swap Spread
        public double? AssetSwapSpreadMid { get; set; }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1 : Generic
        public Bond_Line() { }

        // Constructor 2 : Date Only
        public Bond_Line(DateTime currentDate)
        {
            Date = currentDate;
        }

        // Constructor 3 : Keys only
        public Bond_Line(DateTime currentDate, DBID ticker)
        {
            Date = currentDate;
            DBID = ticker.ToInt();
        }

        // Constructor 4 : Keys + Data
        public Bond_Line(DateTime currentDate, DBID ticker, double CleanPriceBid_, double CleanPriceAsk_, double CleanPriceMid_,
                                                            double DirtyPriceBid_, double DirtyPriceAsk_, double DirtyPriceMid_,
                                                            double YieldToMaturityBid_, double YieldToMaturityAsk_, double YieldToMaturityMid_,
                                                            double AssetSwapSpreadBid_, double AssetSwapSpreadAsk_, double AssetSwapSpreadMid_)
                                                            // double CDS1Y_, double CDS3Y_, double CDS5Y_, double CDS7Y_, double CDS10Y_,

        {
            
            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            CleanPriceBid = CleanPriceBid_;
            CleanPriceAsk = CleanPriceAsk_;
            CleanPriceMid = CleanPriceMid_;

            DirtyPriceBid = DirtyPriceBid_;
            DirtyPriceAsk = DirtyPriceAsk_;
            DirtyPriceMid = DirtyPriceMid_;

            YieldToMaturityBid = YieldToMaturityBid_;
            YieldToMaturityAsk = YieldToMaturityAsk_;
            YieldToMaturityMid = YieldToMaturityMid_;

            AssetSwapSpreadBid = AssetSwapSpreadBid_;
            AssetSwapSpreadAsk = AssetSwapSpreadAsk_;
            AssetSwapSpreadMid = AssetSwapSpreadMid_;

        }

        // Constructor 5 : From Dict of nullable doubles
        public Bond_Line(DateTime currentDate, DBID ticker, Dictionary<String, Double?> data)
        {

            // Keys
            Date = currentDate;
            DBID = ticker.ToInt();

            // Data
            CleanPriceBid = data["CleanPriceBid"];
            CleanPriceAsk = data["CleanPriceAsk"];
            CleanPriceMid = data["CleanPriceMid"];

            DirtyPriceBid = data["DirtyPriceBid"];
            DirtyPriceAsk = data["DirtyPriceAsk"];
            DirtyPriceMid = data["DirtyPriceMid"];

            YieldToMaturityBid = data["YieldToMaturityBid"];
            YieldToMaturityAsk = data["YieldToMaturityAsk"];
            YieldToMaturityMid = data["YieldToMaturityMid"];

            //AssetSwapSpreadBid = data["AssetSwapSpreadBid"];
            AssetSwapSpreadMid = data["AssetSwapSpreadMid"];
            //AssetSwapSpreadAsk = data["AssetSwapSpreadAsk"];

        }

        #endregion


        // ************************************************************
        // BLOOMBERG
        // ************************************************************

        #region

        private static readonly string CLEAN_PX_BID = "PX_CLEAN_BID";
        private static readonly string CLEAN_PX_ASK = "PX_CLEAN_ASK";
        private static readonly string CLEAN_PX_MID = "PX_CLEAN_MID";

        private static readonly string DIRTY_PX_BID = "PX_DIRTY_BID";
        private static readonly string DIRTY_PX_ASK = "PX_DIRTY_ASK";
        private static readonly string DIRTY_PX_MID = "PX_DIRTY_MID";

        private static readonly string YTM_BID = "YLD_YTM_BID";
        private static readonly string YTM_ASK = "YLD_YTM_BID";
        private static readonly string YTM_MID = "YLD_YTM_BID";

        // private static readonly string ASW_SPREAD_BID = "ASSET_SWAP_SPD_BID";
        // private static readonly string ASW_SPREAD_ASK = "ASSET_SWAP_SPD_ASK";
        private static readonly string ASW_SPREAD_MID = "ASSET_SWAP_SPD_MID";

        private static readonly int MAX_RETRY = 10;



        // added override to kill warning
        public override Dictionary<string, int> SetFromBloomberg(ref BBCOMM.Element myElement, Dictionary<string, int> skipFields)
        {


            if (skipFields[CLEAN_PX_BID] < MAX_RETRY) { SetCleanPxBid(ref myElement, ref skipFields); };
            if (skipFields[CLEAN_PX_ASK] < MAX_RETRY) { SetCleanPxAsk(ref myElement, ref skipFields); };
            if (skipFields[CLEAN_PX_MID] < MAX_RETRY) { SetCleanPxMid(ref myElement, ref skipFields); };

            if (skipFields[DIRTY_PX_BID] < MAX_RETRY) { SetDirtyPxBid(ref myElement, ref skipFields); };
            if (skipFields[DIRTY_PX_ASK] < MAX_RETRY) { SetDirtyPxAsk(ref myElement, ref skipFields); };
            if (skipFields[DIRTY_PX_MID] < MAX_RETRY) { SetDirtyPxMid(ref myElement, ref skipFields); };

            if (skipFields[YTM_BID] < MAX_RETRY) { SetYTMBid(ref myElement, ref skipFields); };
            if (skipFields[YTM_ASK] < MAX_RETRY) { SetYTMAsk(ref myElement, ref skipFields); };
            if (skipFields[YTM_MID] < MAX_RETRY) { SetYTMMid(ref myElement, ref skipFields); };

            //if (skipFields[ASW_SPREAD_BID] < MAX_RETRY) { SetAssetSwapSpreadBid(ref myElement, ref skipFields); };
            //if (skipFields[ASW_SPREAD_ASK] < MAX_RETRY) { SetAssetSwapSpreadAsk(ref myElement, ref skipFields); };
            if (skipFields[ASW_SPREAD_MID] < MAX_RETRY) { SetAssetSwapSpreadMid(ref myElement, ref skipFields); };

            return skipFields;

        }


        #endregion

        // SET METHODS FOR CLEAN PRICES

        #region Set for Clean Prices


        protected void SetCleanPxBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CleanPriceBid = (double)myElement.GetElementAsFloat64(CLEAN_PX_BID);
            }

            catch
            {
                // Some log ?
                skipFields[CLEAN_PX_BID] += 1;
                CleanPriceBid = Double.NaN;
            }

        }
        protected void SetCleanPxAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CleanPriceAsk = (double)myElement.GetElementAsFloat64(CLEAN_PX_ASK);
            }

            catch
            {
                // Some log ?
                skipFields[CLEAN_PX_ASK] += 1;
                CleanPriceAsk = Double.NaN;
            }

        }
        protected void SetCleanPxMid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CleanPriceMid = (double)myElement.GetElementAsFloat64(CLEAN_PX_MID);
            }

            catch
            {
                // Some log ?
                skipFields[CLEAN_PX_MID] += 1;
                CleanPriceMid = Double.NaN;
            }

        }

        #endregion
        

        // SET METHODS FOR DIRTY PRICES

        #region Set for Dirty Prices

        protected void SetDirtyPxBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                DirtyPriceBid = (double)myElement.GetElementAsFloat64(DIRTY_PX_BID);
            }

            catch
            {
                // Some log ?
                skipFields[DIRTY_PX_BID] += 1;
                DirtyPriceBid = Double.NaN;
            }

        }
        protected void SetDirtyPxAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                DirtyPriceAsk = (double)myElement.GetElementAsFloat64(DIRTY_PX_ASK);
            }

            catch
            {
                // Some log ?
                skipFields[DIRTY_PX_ASK] += 1;
                DirtyPriceAsk = Double.NaN;
            }

        }
        protected void SetDirtyPxMid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                DirtyPriceMid = (double)myElement.GetElementAsFloat64(DIRTY_PX_MID);
            }

            catch
            {
                // Some log ?
                skipFields[DIRTY_PX_MID] += 1;
                DirtyPriceMid = Double.NaN;
            }

        }


        #endregion



        // SET METHODS FOR YTM

        #region Set for Yields to Maturity

        protected void SetYTMBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                YieldToMaturityBid = (double)myElement.GetElementAsFloat64(YTM_BID);
            }

            catch
            {
                // Some log ?
                skipFields[YTM_BID] += 1;
                YieldToMaturityBid = Double.NaN;
            }

        }
        protected void SetYTMAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                YieldToMaturityAsk = (double)myElement.GetElementAsFloat64(YTM_ASK);
            }

            catch
            {
                // Some log ?
                skipFields[YTM_ASK] += 1;
                YieldToMaturityAsk = Double.NaN;
            }

        }
        protected void SetYTMMid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                YieldToMaturityMid = (double)myElement.GetElementAsFloat64(YTM_MID);
            }

            catch
            {
                // Some log ?
                skipFields[YTM_MID] += 1;
                YieldToMaturityMid = Double.NaN;
            }

        }


        #endregion




        // SET METHODS FOR YTM
        /*
        #region Set for CDS levels

        protected void SetCDS1Y(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CDS1Y = (double)myElement.GetElementAsFloat64(CDS_1Y);
            }

            catch
            {
                // Some log ?
                skipFields[CDS_1Y] += 1;
                CDS1Y = Double.NaN;
            }

        }
        protected void SetCDS3Y(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CDS3Y = (double)myElement.GetElementAsFloat64(CDS_3Y);
            }

            catch
            {
                // Some log ?
                skipFields[CDS_3Y] += 1;
                CDS3Y = Double.NaN;
            }

        }
        protected void SetCDS5Y(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CDS5Y = (double)myElement.GetElementAsFloat64(CDS_5Y);
            }

            catch
            {
                // Some log ?
                skipFields[CDS_5Y] += 1;
                CDS5Y = Double.NaN;
            }

        }
        protected void SetCDS7Y(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CDS7Y = (double)myElement.GetElementAsFloat64(CDS_7Y);
            }

            catch
            {
                // Some log ?
                skipFields[CDS_7Y] += 1;
                CDS7Y = Double.NaN;
            }

        }
        protected void SetCDS10Y(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                CDS10Y = (double)myElement.GetElementAsFloat64(CDS_10Y);
            }

            catch
            {
                // Some log ?
                skipFields[CDS_10Y] += 1;
                CDS10Y = Double.NaN;
            }

        }

        #endregion
        */


        // SET METHODS FOR ASSET SWAP SPREAD

        #region Set for CDS levels

        /*
        protected void SetAssetSwapSpreadBid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                AssetSwapSpreadBid = (double)myElement.GetElementAsFloat64(ASW_SPREAD_BID);
            }

            catch
            {
                // Some log ?
                skipFields[ASW_SPREAD_BID] += 1;
                AssetSwapSpreadBid = Double.NaN;
            }

        }


        protected void SetAssetSwapSpreadAsk(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                AssetSwapSpreadAsk = (double)myElement.GetElementAsFloat64(ASW_SPREAD_ASK);
            }

            catch
            {
                // Some log ?
                skipFields[ASW_SPREAD_ASK] += 1;
                AssetSwapSpreadAsk = Double.NaN;
            }

        }

        */

        protected void SetAssetSwapSpreadMid(ref BBCOMM.Element myElement, ref Dictionary<string, int> skipFields)
        {
            try
            {
                AssetSwapSpreadMid = (double)myElement.GetElementAsFloat64(ASW_SPREAD_MID);
            }

            catch
            {
                // Some log ?
                skipFields[ASW_SPREAD_MID] += 1;
                AssetSwapSpreadMid = Double.NaN;
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

                "CleanPriceBid",
                "CleanPriceAsk",
                "CleanPriceMid",

                "DirtyPriceBid",
                "DirtyPriceAsk",
                "DirtyPriceMid",

                "YieldToMaturityBid",
                "YieldToMaturityAsk",
                "YieldToMaturityMid",

                //"AssetSwapSpreadBid",
                //"AssetSwapSpreadAsk",
                "AssetSwapSpreadMid"

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
                 "CleanPriceBid",
                "CleanPriceAsk",
                "CleanPriceMid",

                "DirtyPriceBid",
                "DirtyPriceAsk",
                "DirtyPriceMid",

                "YieldToMaturityBid",
                "YieldToMaturityAsk",
                "YieldToMaturityMid",

               // "AssetSwapSpreadBid",
               // "AssetSwapSpreadAsk",
                "AssetSwapSpreadMid"

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
            { "CleanPriceBid", CleanPriceBid }, { "CleanPriceAsk", CleanPriceAsk }, { "CleanPriceMid", CleanPriceMid },
            { "DirtyPriceBid", DirtyPriceBid }, { "DirtyPriceAsk", DirtyPriceAsk }, { "DirtyPriceMid", DirtyPriceMid },
            { "YieldToMaturityBid", YieldToMaturityBid }, { "YieldToMaturityAsk", YieldToMaturityAsk }, { "YieldToMaturityMid", YieldToMaturityMid },
            //{ "CDS1Y", CDS1Y }, { "CDS3Y", CDS3Y }, { "CDS5Y", CDS5Y }, { "CDS7Y", CDS7Y }, { "CDS10Y", CDS10Y },
            { "AssetSwapSpreadMid", AssetSwapSpreadMid } //, { "AssetSwapSpreadAsk", AssetSwapSpreadAsk }, { "AssetSwapSpreadBid", AssetSwapSpreadBid }
            };

        }

        #endregion




        // ************************************************************
        // SANITY CHECKS
        // ************************************************************

        public bool isNull()
        {
            if (CleanPriceMid == null && DirtyPriceMid == null && YieldToMaturityMid == null) { return true; }
            return false;
        }


    }

}
