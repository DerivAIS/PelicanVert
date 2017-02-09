using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// External Custom Packages
using BBCOMM = Bloomberglp.Blpapi;


namespace QLyx.DataIO
{


    public class EquityVolatility_Line : GenericDatabaseLine
    {


        // ************************************************************
        // PROPERTIES -- ASSOCIATED TABLE
        // ************************************************************

        #region


        // Table
        public new myDB Table = myDB.EquityVolatility;


        #endregion


        // ************************************************************
        // PROPERTIES -- KEYS
        // ************************************************************

        #region

        // Pricing Date
        public new DateTime Date { get; set; }

        // Ticker
        public DBID DBID { get; set; }

        // Moneyness
        public double Moneyness { get; set; }

        // Maturity
        public DateTime Maturity { get; set; }

        #endregion



        // ************************************************************
        // PROPERTIES -- DATA
        // ************************************************************

        #region Data fields

        // Implied Volatility BID
        public double? ImpVolBid { get; set; }

        // Implied Volatility ASK
        public double? ImpVolAsk { get; set; }

        // Implied Volatility MID
        public double? ImpVolMid { get; set; }

        // Reference Spot
        public double? RefSpot { get; set; }
        
        // Forward
        public double? Forward { get; set; }

        // Implied Dividend
        public double? ImpDiv { get; set; }

        // Discount Factor
        public double? DiscountFactor { get; set; }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        public EquityVolatility_Line() { }


        public EquityVolatility_Line(DateTime currentDate)
        {
            Date = currentDate;
        }


        public EquityVolatility_Line(DateTime currentDate, DBID argDBID)
        {
            Date = currentDate;
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
                
                "ImpVolBid", 
                "ImpVolAsk",
                "ImpVolAsk",
                "RefSpot", 
                "Forward",
                "ImpDiv", 
                "DiscountFactor" };
        }


        public override List<string> GetKeyFields()
        {
            return new List<string>() { 
                
                "Date", 
                "Underlying",
                "Maturity",
                "Moneyness" };
        }


        public override string GetTable()
        {
            return this.Table.ToString();
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
            { "ImpVolBid", ImpVolBid }, { "ImpVolAsk", ImpVolAsk},
            { "ImpVolMid", ImpVolMid }, { "RefSpot", RefSpot},
            { "Forward", Forward }, { "ImpDiv", ImpDiv},
            { "DiscountFactor", DiscountFactor }
            };

        }

        #endregion


    }

}
