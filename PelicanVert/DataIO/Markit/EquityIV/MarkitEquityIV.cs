using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.Utilities;

namespace QLyx.DataIO.Markit
{

    public sealed class Markit_Equity_IV
    {


        // ************************************************************
        // STATIC METHODS
        // ************************************************************




        // ************************************************************
        // PRIVATE CONSTRUCTORS & NESTED CLASS
        // ************************************************************

        #region Private constructors -- do not touch

        private MarkitEquityUnderlying _underlying;

        private DateTime formatChangeDate = new DateTime(2016, 10, 25);

        private bool _isNewFileFormat = false;

        private Markit_Equity_IV(MarkitEquityUnderlying underlying)
        {
            _underlying = underlying;
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            // internal static readonly Markit_IV_SX5E instance = new Markit_IV_SX5E();
            internal static Markit_Equity_IV instance(MarkitEquityUnderlying underlying) { return new Markit_Equity_IV(underlying); }

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
        /// Method used ton instanciate the Markit Equity IV in-memory cache (singleton).
        /// </summary>
        public static Markit_Equity_IV Instance(MarkitEquityUnderlying underlying) { return Nested.instance(underlying); }

        #endregion


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        // FIXED PROPERTIES
        #region Fixed Properties

        private int maxLag = 5;

        #endregion


        // INSTANCE PROPERTIES
        #region Instance Properties

        // DATA
        #region Data (Dict of DateTime, MarkitSurface)

        // String : index name, DateTime : valuation date of surface
        private Dictionary<DateTime, MarkitSurface> _data;
        public Dictionary<DateTime, MarkitSurface> data
        {
            get
            {
                if (_data == null)
                {
                    _data = new Dictionary<DateTime, MarkitSurface>();
                }
                return _data;
            }

            private set
            {
                _data = value;
            }

        }

        #endregion



        // INDEX NAME
        #region Index Name (as String)

        private String _indexName;
        public String indexName
        {
            get
            {
                return _indexName;
            }

            private set
            {
                if (!(_indexName == null)) { throw new System.ArgumentException("ReadOnlyException", "Markit Equity IV object already linked to an index."); }
                _indexName = value;
            }

        }

        #endregion



        //CURRENT YEAR
        #region Current year (as Double)

        private Double _currentYear;
        public Double currentYear
        {
            get
            {
                return _currentYear;
            }

            private set
            {
                _currentYear = value;
            }

        }

        #endregion


        #endregion



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        //public Markit_IV() { }


        // Constructor 2 : Index name built-in

        /*
        public Markit_Equity_IV(String argIndexName)
        {
            indexName = argIndexName;
        }

        */

        #endregion




        // ************************************************************
        // METHODS : ACCESSING DATA
        // ************************************************************

        #region

        /// <summary>
        /// Method used ton instanciate the Markit Equity IV in-memory cache (singleton).
        /// </summary>
        private void LoadData(DateTime valuationDate)
        {
            // Skip if data already loaded
            if (valuationDate.Year == currentYear) { return; }

            // Clear old data (potential)
            _data = null;
            _currentYear = 0.0;

            // Load data using reader
            MarkitVolatilityReader myReader = new MarkitVolatilityReader(_underlying);
            data = myReader.Get(indexName, valuationDate);
            currentYear = valuationDate.Year;
        }

        /// <summary>
        /// Method used ton instanciate the Markit Equity IV in-memory cache (singleton).
        /// </summary>
        public void Load()
        {
      
            // Clear old data (potential)
            _data = null;
            _currentYear = 0.0;

            // Load data using reader
            MarkitVolatilityReader myReader = new MarkitVolatilityReader(_underlying);
            data = myReader.Get(indexName);
            currentYear = Double.NaN;
        }

        /// <summary>
        /// Method used to retrieve the MarkitSurface for a given date.
        /// </summary>
        private MarkitSurface Get(DateTime valuationDate)
        {

            // Not using this method, returns the new one
            if (_isNewFileFormat) { return Get_newFileFormat(valuationDate); }

            // First check if there is data in the object,
            // if not load data
            if (data.Count() == 0)
            {
                LoadData(valuationDate);
            }

            // If there is data, check if the year matches
            else if (valuationDate.Year != currentYear)
            {
                LoadData(valuationDate);
            }

            // Return available data
            if (data.ContainsKey(valuationDate))
            { return data[valuationDate]; }

            // Case where we are at the beginning of January 
            // (say 1st, for which there is no data in this file, 
            // but the past for past year is gone
            else if (valuationDate.Month == 1 && valuationDate.Day < maxLag)
            {
                return data.FirstOrDefault().Value;
            }

            else
            {

                int k = 1;

                // Check if previous dates are present (up to the admissible max lag)
                while (k < maxLag)
                {
                    if (data.ContainsKey(valuationDate.AddDays(-k))) { return data[valuationDate.AddDays(-k)]; }
                    k++;
                }
            }

            throw new System.ArgumentException("DataUnavailable", "Markit Equity IV object does not contain the volatility data requested."); 

        }


        /// <summary>
        /// Interpolated MarkitSurface for a date not present in the original data file.
        /// </summary>
        private MarkitSurface GetInterpolated(DateTime valuationDate)
        {
            throw new NotImplementedException();
        }


        #endregion
         

        /// <summary>
        /// Method used to retrieve the MarkitSurface for a given date (new format, single date).
        /// </summary>
        private MarkitSurface Get_newFileFormat(DateTime valuationDate)
        {
            if (data.ContainsKey(valuationDate))
            { return data[valuationDate]; }

            // Load data using reader
            MarkitVolatilityReader myReader = new MarkitVolatilityReader(_underlying);
            data = myReader.Get(indexName, valuationDate, true);
            currentYear = valuationDate.Year;

            return data[valuationDate];
        }


        // ************************************************************
        // ACCESSORS 
        // ************************************************************

            #region Public methods to access the data

            /// <summary>
            /// Provide the MarkitSurface object for a certain DateTime.
            /// </summary>
        public MarkitSurface this[DateTime valuationDate]
        {
            get
            {
                if (valuationDate > formatChangeDate) { _isNewFileFormat = true; }
                return Get(valuationDate);
            }
         
        }

        /// <summary>
        /// Provide the MarkitSurface object for a certain Date.
        /// </summary>
        public MarkitSurface this[QLNet.Date valuationDate]
        {
            get
            {
                DateTime valuationDateTime = valuationDate.ToDateTime();
                if (valuationDateTime > formatChangeDate) { _isNewFileFormat = true; }
                return Get(valuationDateTime);
            }

        }

        #endregion

        
    }
}
