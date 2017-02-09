using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QLyx.DataIO.Markit
{
    public class MarkitVolatilityReader 
    {


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        // FIXED PROPERTIES
        #region Basic properties

        private string partialFilePath_old = @"F:\MARK-LYX-QMG\DValo\_Documents\Markit Vol\CSV\";
        private string partialFilePath_new = @"F:\MARK-LYX-QMG\DValo\_Documents\Markit Vol\";

        // Properties
        protected static readonly Dictionary<string, string> MarkitFileNames = new Dictionary<string, string>() {
                            {"SPX", "Lyxor_ExchangeCloseSPUS_EQVol_Index_Exchange_" },
                            {"STOXX", "Lyxor_ExchangeCloseSTOXX_EQVol_Index_Exchange_" },
                            {"LSE", "Lyxor_ExchangeCloseLSE_EQVol_Index_Exchange_"},
                            {"SWIND", "Lyxor_ExchangeCloseSWIND_EQVol_Index_Exchange_"},
                            {"OSK", "Lyxor_ExchangeCloseOSK_EQVol_Index_Exchange_"},
                            {"TK", "Lyxor_ExchangeCloseTK_EQVol_Index_Exchange_"},
                            {"NYSE", "Lyxor_ExchangeCloseNYSEIND_EQVol_Index_Exchange_" }};


        protected static readonly Dictionary<MarkitEquityUnderlying, string> MarkitFileUnderlyingNames = new Dictionary<MarkitEquityUnderlying, string>() {
                            {MarkitEquityUnderlying.Eurostoxx, "EURO STOXX 50" },
                            {MarkitEquityUnderlying.Eurostoxx_TR, "EURO STOXX 50 TOTAL RETURN" },
                            {MarkitEquityUnderlying.SP_500, "S&P 500" },
                            {MarkitEquityUnderlying.SP_500_TR, "S&P 500 TOTAL RETURN" },
                            {MarkitEquityUnderlying.EPRA, "EPRA" },
                            {MarkitEquityUnderlying.FTSE_100, "FTSE 100" },
                            {MarkitEquityUnderlying.CAC40, "CAC 40" },
                            {MarkitEquityUnderlying.SMI, "SMI" },
                            {MarkitEquityUnderlying.NIKKEI_225, "NIKKEI 225" },
                            {MarkitEquityUnderlying.TOPIX, "TOPIX" }};

        private string fileExtension = ".csv";

        private MarkitEquityUnderlying _underlying;

        protected string filePath_old() {

            switch (_underlying)
            {
                case MarkitEquityUnderlying.Eurostoxx:
                    Console.WriteLine("Reading Markit Eurostoxx50 Excess Return volatility data.");
                    return System.IO.Path.Combine(partialFilePath_old, "SX5E");

                case MarkitEquityUnderlying.SP_500:
                    Console.WriteLine("Reading Markit S&P 500 Excess Return volatility data.");
                    return System.IO.Path.Combine(partialFilePath_old, "SPX");

                default:
                    Console.WriteLine("Markit equity volatility data not available for underlying.");
                    throw new System.ArgumentException("DataUnavailable", "Markit equity volatility data not available for underlying");


            }
        }


        protected string filePath_new()
        {

            switch (_underlying)
            {
                case MarkitEquityUnderlying.Eurostoxx:
                    Console.WriteLine("Reading Markit Eurostoxx50 Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["STOXX"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.Eurostoxx_TR:
                    Console.WriteLine("Reading Markit Eurostoxx50 Total Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["STOXX"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.SP_500:
                    Console.WriteLine("Reading Markit S&P500 Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["SPUS"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.SP_500_TR:
                    Console.WriteLine("Reading Markit S&P500 Total Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["SPUS"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.EPRA:
                    Console.WriteLine("Reading Markit EPRA volatility data.");
                    return (partialFilePath_new + MarkitFileNames["LSE"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.FTSE_100:
                    Console.WriteLine("Reading Markit FTSE100 (UKX) Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["LSE"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.CAC40:
                    Console.WriteLine("Reading Markit CAC40 Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["NYSEIND"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.SMI:
                    Console.WriteLine("Reading Markit Swiss SMI Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["SWIND"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.NIKKEI_225:
                    Console.WriteLine("Reading Markit Nikkei 225 Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["OSK"] + dateString() + fileExtension);

                case MarkitEquityUnderlying.TOPIX:
                    Console.WriteLine("Reading Markit Topix Excess Return volatility data.");
                    return (partialFilePath_new + MarkitFileNames["TK"] + dateString() + fileExtension);


                default:
                    Console.WriteLine("Markit equity volatility data not available for underlying.");
                    throw new System.ArgumentException("DataUnavailable", "Markit equity volatility data not available for underlying");

            }
        }

        protected string filePath() {
            if (isNewFileFormat) { return filePath_new(); }
            return filePath_old();
        }




        int retryCount = 0;
        int maxRetry= 5;

        bool isNewFileFormat = false;

        #endregion


        // DATA -- ACTUAL SURFACE SERIES
        #region Volatility Date (dict of MarkitSurface)

        protected Dictionary<DateTime, MarkitSurface> _readerData;
        public Dictionary<DateTime, MarkitSurface> readerData
        {
            get
            {
                if (_readerData == null) { _readerData = new Dictionary<DateTime, MarkitSurface>(); }
                return _readerData;
            }

            protected set
            {
                _readerData = value;
            }

        }

        #endregion


        // Current Date
        #region Current Valuation Date (as DateTime)

        protected DateTime _currentDate;
        public DateTime currentDate
        {
            get
            {
                return _currentDate;
            }

            protected set
            {
                _currentDate = value;
            }

        }

        #endregion


        // ************************************************************
        // METHODS : ACCESSORS
        // ************************************************************

        public string dateString(DateTime dt) {

            string year = dt.Year.ToString();

            string month = dt.Month.ToString();
            if (dt.Month < 10) { month = "0" + month; }

            string day = dt.Day.ToString();
            if (dt.Day < 10) { day = "0" + day; }

            return (year + month + day);
        }

        public string dateString()
        {
            return dateString(_currentDate);
        }
        

        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************


        #region

        // Constructor 1 : Generic
        // public MarkitVolatilityReader() { }

        public MarkitVolatilityReader(MarkitEquityUnderlying underlying) { _underlying = underlying; }

        

        #endregion



        // ************************************************************
        // METHODS : LOADING / PARSING
        // ************************************************************

        private void LoadCSV(string fileName, bool isNewFileFormat = false)
        {
            if (isNewFileFormat)
            {
                LoadCSV_new(fileName);
            }
            else {
                LoadCSV_old(fileName);
            }
        }
        

        #region *** MARKIT OLD FORMAT *** Method for loading and parsing

        private void LoadCSV_old(string fileName)
        {

            int Row = 0;
            List<string> headers = new List<string>();
            string path = filePath_old();
            StreamReader sr = new StreamReader(filePath_old() + fileName + fileExtension);
            var lines = new List<string[]>();


            // Headers
            headers = this.ParseHeader(sr.ReadLine().Split(';').ToList());
            

            // Data
            while ((!sr.EndOfStream) && (retryCount <= maxRetry))
            {

                List<string> Line = sr.ReadLine().Split(',').ToList();
                List<double> singleLine = new List<double>();

                if (Check_OldFormat(Line)) { Parse_OldFormat(Line); }
                else { retryCount++; }

                //Console.WriteLine(Row);

                Row++;

            }


            sr.Close();
            
            Console.WriteLine("Finished loading Markit Volatility data ({0}).", readerData.FirstOrDefault().Key.Year );

        }

        private bool Check_OldFormat(List<string> singleLine)
        {
            if (singleLine[1].Count() > 0) { return true; }
            return false;
        }

        private void Parse_OldFormat(List<string> line)
        {

               
            DateTime thisPricingDate = this.ParseDate(line[1]);
            MarkitSurface thisSurface = this[thisPricingDate];

            thisSurface.SetName(line[3]);
            thisSurface.SetImpliedSpot(line[9]);
            thisSurface.SetReferenceLevel(line[6]);

            DateTime thisForwardDate = this.ParseDate(line[7]);

            thisSurface.SetForward(thisPricingDate, thisForwardDate, line[11]);
            thisSurface.SetDiscountFactor(thisPricingDate, thisForwardDate, line[12]);
            //thisSurface.SetDividend(thisPricingDate, thisForwardDate, line[xyz]);


            thisSurface.SetVolatility(thisPricingDate, thisForwardDate, line[8], line[10]);


        }


        #endregion


        #region *** MARKIT NEW FORMAT *** Method for loading and parsing
        private void LoadCSV_new(string fileName)
        {

            int Row = 0;
            List<string> headers = new List<string>();

            string path = filePath_new();
            if(!CheckFileExistance(path))
            {
                QLyx.Utilities.MarkitEquityVolatilityFileIO.CopyAllFiles(_currentDate);
            }

            StreamReader sr = new StreamReader(path);
            var lines = new List<string[]>();

            // Get the underlying to look for in the file (many indices per file)
            string underlyingCode = MarkitFileUnderlyingNames[_underlying];


            // Headers
            headers = this.ParseHeader_new(sr.ReadLine().Split(',').ToList());


            // Data
            while ((!sr.EndOfStream) && (retryCount <= maxRetry))
            {

                List<string> Line = sr.ReadLine().Split(',').ToList();
                List<double> singleLine = new List<double>();

                if (Check_NewFormat(Line, underlyingCode)) { Parse_NewFormat(Line); }
                else { retryCount++; }

                //Console.WriteLine(Row);

                Row++;

            }


            sr.Close();

            Console.WriteLine("Finished loading Markit Volatility data ({0}).", readerData.FirstOrDefault().Key.Year);

        }


        private bool CheckFileExistance(string targetPath)
        {
            return System.IO.File.Exists(targetPath);
        }

        private bool Check_NewFormat(List<string> singleLine, string underlyingCode)
        {
            if ((singleLine[1].Count() > 0) && (singleLine[2] == underlyingCode)) { return true; }
            return false;
        }

        private void Parse_NewFormat(List<string> line)
        {


            DateTime thisPricingDate = this.ParseDate_new(line[0]);
            MarkitSurface thisSurface = this[thisPricingDate];
            thisSurface.IsNewFormat();

            thisSurface.SetName(line[3]);
            thisSurface.SetImpliedSpot(line[5]);
            thisSurface.SetReferenceLevel(line[5]);

            DateTime thisForwardDate = this.ParseDate_new(line[6]);

            thisSurface.SetForward(thisPricingDate, thisForwardDate, line[12]);
            thisSurface.SetDiscountFactor(thisPricingDate, thisForwardDate, line[10]);
            thisSurface.SetDividend(thisPricingDate, thisForwardDate, line[13]);


            thisSurface.SetVolatility(thisPricingDate, thisForwardDate, line[7], line[9]);


        }


        #endregion



        private DateTime ParseDate(string DateString)
        {

            DateTime output = new DateTime();

            

                string rawDate = DateString.Split(' ')[0];
                string[] DDMMYYYY = rawDate.Split('/');

                if (DDMMYYYY.Count() == 3)
                {

                    int day = 0;
                    Int32.TryParse(DDMMYYYY[0].ToString(), out day);

                    int month = 0;
                    Int32.TryParse(DDMMYYYY[1].ToString(), out month);

                    int year = 0;
                    Int32.TryParse(DDMMYYYY[2].ToString(), out year);

                    output = new DateTime(year, month, day);

            }



            return output;

        }


        private DateTime ParseDate_new(string DateString)
        {

            DateTime output = new DateTime();



            string rawDate = DateString.Split(' ')[0];
            string[] DDMMYYYY = rawDate.Split('/');

            if (DDMMYYYY.Count() == 3)
            {

                int day = 0;
                Int32.TryParse(DDMMYYYY[2].ToString(), out day);

                int month = 0;
                Int32.TryParse(DDMMYYYY[1].ToString(), out month);

                int year = 0;
                Int32.TryParse(DDMMYYYY[0].ToString(), out year);

                output = new DateTime(year, month, day);

            }



            return output;

        }

        private List<string> ParseHeader(List<string> HeadersStringList)
        {

            List<string> output = new List<string>();

            foreach (string d in HeadersStringList)
            {
                if (!(d.Count() == 0))
                {
                    output.Add(d);
                }
            }

            return output;

            
        }

        
        private List<string> ParseHeader_new(List<string> HeadersStringList)
        {

            List<string> output = new List<string>();

            foreach (string d in HeadersStringList)
            {
                if (!(d.Count() == 0))
                {
                    output.Add(d);
                }
            }

            return output;


        }



        // ************************************************************
        // METHODS : SELECTING FILENAME / CONSTRUCTING PATH
        // ************************************************************
        #region

        // ENTRY

        public String BuildFileName(String indexTicker, DateTime requestedDate, bool isNewFileFormat = false)
        {
            if (isNewFileFormat) { return BuildFileName_new(indexTicker, requestedDate); }
            else { return BuildFileName_old(indexTicker, requestedDate); }
        }


        public String BuildFileName(String indexTicker, bool isNewFileFormat = false)
        {
            if (isNewFileFormat) { return BuildFileName_new(indexTicker); }
            else { return BuildFileName_old(indexTicker); }
        }


        // OLD

        public String BuildFileName_old(String indexTicker, DateTime requestedDate)
        {
            return indexTicker + "_" + requestedDate.Year.ToString();
        }


        public String BuildFileName_old(String indexTicker)
        {
            return indexTicker;
        }


        // NEW

        public String BuildFileName_new(String indexTicker, DateTime requestedDate)
        {
            //string ticker = _underlying.ToString();
            return dateString();
            
        }


        public String BuildFileName_new(String indexTicker)
        {
            return indexTicker;
        }

        #endregion




        // ************************************************************
        // METHODS : ACCESSING DATA
        // ************************************************************
        #region


        public void Load(String indexTicker, DateTime requestedDate, bool isNewFileFormat = false)
        {
            String myFile = BuildFileName(indexTicker, requestedDate);
            LoadCSV(myFile, isNewFileFormat);
        }


        public void Load(String indexTicker, bool isNewFileFormat = false)
        {
            String myFile = BuildFileName(indexTicker);
            LoadCSV(myFile, isNewFileFormat);
        }



        public Dictionary<DateTime, MarkitSurface> Get(String indexTicker, DateTime requestedDate, bool isNewFileFormat = false)
        {
            _currentDate = requestedDate;
            
            String myFile = BuildFileName(indexTicker, requestedDate, isNewFileFormat);
            LoadCSV(myFile, isNewFileFormat);
            return readerData;
        }

        public Dictionary<DateTime, MarkitSurface> Get(String indexTicker, bool isNewFileFormat = false)
        {
            String myFile = BuildFileName(indexTicker, isNewFileFormat);
            LoadCSV(myFile, isNewFileFormat);
            return readerData;
        }




        #endregion


        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public MarkitSurface this[DateTime i]
        {

            get
            {
                if (!readerData.ContainsKey(i))
                {
                    readerData[i] = new MarkitSurface(i);
                }
 
                return readerData[i];
                
            }

            protected set
            {
                readerData[i] = value;
            }

        }

        #endregion



    }


   

    }
