using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

// External Custom Packages
using BbergAPI = Bloomberglp.Blpapi;

// Internal Custom Packages
using QLyx.Containers;
using QLyx.DataIO;


namespace QLyx.DataIO.Fetcher
{


    // ************************************************************
    // ENUM 
    // ************************************************************

    public enum E_PRICING_OPTION { PRICING_OPTION_PRICE, PRICING_OPTION_YIELD };
    public enum E_PERIODICITY_ADJUSTMENT { ACTUAL, CALENDAR, FISCAL };
    public enum E_PERIODICITY_SELECTION { DAILY, WEEKLY, MONTHLY, QUARTERLY, SEMI_ANNUALLY, YEARLY };
    public enum E_NON_TRADING_DAY_FILL_OPTION { NON_TRADING_WEEKDAYS, ALL_CALENDAR_DAYS, ACTIVE_DAYS_ONLY };
    public enum E_NON_TRADING_DAY_FILL_METHOD { PREVIOUS_VALUE, NIL_VALUE };




 
    public class BloombergFetcher<T> : Fetcher where T: GenericDatabaseLine
    {



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Full fledged
        public BloombergFetcher(        DBID Dbid,
                                        string Ticker,
                                        Dictionary<string, string> FieldNames,
                                        DateTime StartDate,
                                        DateTime EndDate,
                                        Dictionary<String, Double> Scaling,
                                        List<string> OverrideFields,
                                        List<string> OverrideValues,
                                        E_PRICING_OPTION PricingOption = E_PRICING_OPTION.PRICING_OPTION_PRICE,
                                        E_PERIODICITY_SELECTION PeriodicitySelection = E_PERIODICITY_SELECTION.DAILY,
                                        E_PERIODICITY_ADJUSTMENT PeriodicityAdjustment = E_PERIODICITY_ADJUSTMENT.ACTUAL,
                                        E_NON_TRADING_DAY_FILL_OPTION NonTradingDayFillOption = E_NON_TRADING_DAY_FILL_OPTION.ACTIVE_DAYS_ONLY,
                                        E_NON_TRADING_DAY_FILL_METHOD NonTradingDayFillMethod = E_NON_TRADING_DAY_FILL_METHOD.PREVIOUS_VALUE,
                                        string OverrideCurrency = "")
        {

            // Set the DBID for the security 
            _dbid = Dbid;

            // Set the security tickers
            _bloombergTicker = Ticker;

            // Set the fields to be queried
            _fieldNames = FieldNames;

            // Set the start date of time series
            _fromDate = StartDate;

            // Set the start date time series
            _toDate = EndDate;

            // Set the scaling to be applied
            _scaling = Scaling;

            // Set the override fields
            overrideFields = OverrideFields;

            // Set the override values
            overrideValues = OverrideValues;

            // Set the pricing option (price or yield)
            Option_PricingOption = PricingOption;

            // Set the periodicity (daily, weekly, monthly...)
            Option_PeriodicitySelection = PeriodicitySelection;

            // Set the periodicity adjustment (actual, calendar, fiscal)
            Option_PeriodicityAdjustement = PeriodicityAdjustment;

            // Set the NA fill option
            Option_NonTradingDayFillOption = NonTradingDayFillOption;

            // Set the NA fill method (previous data, etc.)
            Option_NonTradingDayFillMethod = NonTradingDayFillMethod;

            // Override the currency (do not use)
            Option_OverrideCurrency = OverrideCurrency;

            // Tell base which request type we're instaciating
            requestType = REQUEST_TYPE_HISTORICAL;


        }



        // Constructor 2 : Ticker, Fields, Start and End dates
        public BloombergFetcher(DBID Dbid, string Ticker, Dictionary<string, string> FieldNames, DateTime StartDate, DateTime EndDate, Dictionary<String, Double> Scaling)
            
            : this(Dbid: Dbid, Ticker: Ticker, FieldNames: FieldNames, StartDate: StartDate, EndDate: EndDate, Scaling : Scaling,
                                        OverrideFields: new List<string>(),
                                        OverrideValues: new List<string>(),
                                        PricingOption: E_PRICING_OPTION.PRICING_OPTION_PRICE,
                                        PeriodicitySelection: E_PERIODICITY_SELECTION.DAILY,
                                        PeriodicityAdjustment: E_PERIODICITY_ADJUSTMENT.ACTUAL,
                                        NonTradingDayFillOption: E_NON_TRADING_DAY_FILL_OPTION.ALL_CALENDAR_DAYS,
                                        NonTradingDayFillMethod: E_NON_TRADING_DAY_FILL_METHOD.PREVIOUS_VALUE,
                                        OverrideCurrency: "") { }




        // Constructor 3 : Ticker, Fields, Start and End dates, Periodicity
        public BloombergFetcher(DBID Dbid, string Ticker, Dictionary<string, string> FieldNames, DateTime StartDate, DateTime EndDate, Dictionary<String, Double> Scaling, E_PERIODICITY_SELECTION PeriodicitySelection)

            : this(Dbid: Dbid, Ticker: Ticker, FieldNames: FieldNames, StartDate: StartDate, EndDate: EndDate, Scaling: Scaling,
                                        OverrideFields: new List<string>(),
                                        OverrideValues: new List<string>(),
                                        PricingOption: E_PRICING_OPTION.PRICING_OPTION_PRICE,
                                        PeriodicitySelection: PeriodicitySelection, // --------- >> difference from Constructor 2
                                        PeriodicityAdjustment: E_PERIODICITY_ADJUSTMENT.ACTUAL,
                                        NonTradingDayFillOption: E_NON_TRADING_DAY_FILL_OPTION.ALL_CALENDAR_DAYS,
                                        NonTradingDayFillMethod: E_NON_TRADING_DAY_FILL_METHOD.PREVIOUS_VALUE,
                                        OverrideCurrency: "") { }



        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES (CONSTANTS)
        // ************************************************************
        #region

        protected readonly BbergAPI.Name SECURITY_DATA = new BbergAPI.Name("securityData");
        protected readonly BbergAPI.Name TICKER = new BbergAPI.Name("security");
        protected readonly BbergAPI.Name FIELD_DATA = new BbergAPI.Name("fieldData");

        protected readonly BbergAPI.Name FIELD_ID = new BbergAPI.Name("fieldId");
        protected readonly BbergAPI.Name VALUE = new BbergAPI.Name("value");
        protected readonly BbergAPI.Name OVERRIDES = new BbergAPI.Name("overrides");
        protected readonly BbergAPI.Name SECURITIES = new BbergAPI.Name("securities");
        protected readonly BbergAPI.Name FIELDS = new BbergAPI.Name("fields");
        protected readonly BbergAPI.Name SEQUENCE_NUMBER = new BbergAPI.Name("sequenceNumber");
        protected readonly BbergAPI.Name START_DATE = new BbergAPI.Name("startDate");
        protected readonly BbergAPI.Name END_DATE = new BbergAPI.Name("endDate");
        protected readonly BbergAPI.Name DATE = new BbergAPI.Name("date");

        protected readonly BbergAPI.Name PRICING_OPTION = new BbergAPI.Name("pricingOption");
        protected readonly BbergAPI.Name PERIODICITY_ADJUSTMENT = new BbergAPI.Name("periodicityAdjustment");
        protected readonly BbergAPI.Name PERIODICITY_SELECTION = new BbergAPI.Name("periodicitySelection");
        protected readonly BbergAPI.Name NON_TRADING_DAY_FILL_OPTION = new BbergAPI.Name("nonTradingDayFillOption");
        protected readonly BbergAPI.Name NON_TRADING_DAY_FILL_METHOD = new BbergAPI.Name("nonTradingDayFillMethod");
        protected readonly BbergAPI.Name OVERRIDE_CURRENCY = new BbergAPI.Name("currency");

        protected readonly string NOT_AVAILABLE = null; //"#N/A";

        protected readonly string SESSION_EXCEPTION = "Session not started";
        protected readonly string SERVICE_EXCEPTION = "Service not opened";
        protected readonly string REQUEST_TYPE_HISTORICAL = "HistoricalDataRequest";
        protected readonly string REFERENCE_DATA_SERVICE = "//blp/refdata";
        protected readonly string BLOOMBERG_DATE_FORMAT = "yyyyMMdd";

        protected E_PRICING_OPTION pricingOption;
        protected E_PERIODICITY_ADJUSTMENT periodicityAdjustment;
        protected E_PERIODICITY_SELECTION periodicitySelection;
        protected E_NON_TRADING_DAY_FILL_OPTION nonTradingDayFillOption;
        protected E_NON_TRADING_DAY_FILL_METHOD nonTradingDayFillMethod;

        protected string requestType;

        protected string overrideCurrency;

        #endregion



        // ************************************************************
        // CLASS PROPERTIES -- DEFAULT SETTINGS
        // ************************************************************

        #region

        protected E_PRICING_OPTION _Option_PricingOption = E_PRICING_OPTION.PRICING_OPTION_PRICE;
        public E_PRICING_OPTION Option_PricingOption
        {
            get
            {
                return _Option_PricingOption;
            }

            protected set
            {
                _Option_PricingOption = value;
            }
        }


        protected E_PERIODICITY_ADJUSTMENT _Option_PeriodicityAdjustement = E_PERIODICITY_ADJUSTMENT.ACTUAL;
        public E_PERIODICITY_ADJUSTMENT Option_PeriodicityAdjustement
        {
            get
            {
                return _Option_PeriodicityAdjustement;
            }

            protected set
            {
                _Option_PeriodicityAdjustement = value;
            }
        }


        protected E_PERIODICITY_SELECTION _Option_PeriodicitySelection = E_PERIODICITY_SELECTION.DAILY;
        public E_PERIODICITY_SELECTION Option_PeriodicitySelection
        {
            get
            {
                return _Option_PeriodicitySelection;
            }

            protected set
            {
                _Option_PeriodicitySelection = value;
            }
        }


        protected E_NON_TRADING_DAY_FILL_OPTION _Option_NonTradingDayFillOption = E_NON_TRADING_DAY_FILL_OPTION.ACTIVE_DAYS_ONLY;
        public E_NON_TRADING_DAY_FILL_OPTION Option_NonTradingDayFillOption
        {
            get
            {
                return _Option_NonTradingDayFillOption;
            }

            protected set
            {
                _Option_NonTradingDayFillOption = value;
            }
        }


        protected E_NON_TRADING_DAY_FILL_METHOD _Option_NonTradingDayFillMethod = E_NON_TRADING_DAY_FILL_METHOD.NIL_VALUE;
        public E_NON_TRADING_DAY_FILL_METHOD Option_NonTradingDayFillMethod
        {
            get
            {
                return _Option_NonTradingDayFillMethod;
            }

            protected set
            {
                _Option_NonTradingDayFillMethod = value;
            }
        }

        protected string _Option_OverrideCurrency = "";
        public string Option_OverrideCurrency
        {
            get
            {
                return _Option_OverrideCurrency;
            }

            protected set
            {
                _Option_OverrideCurrency = value;
            }
        }


        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES (BLOOMBERG API VARIABLES)
        // ************************************************************

        #region


        // SESSION OPTIONS
        #region Bloomberg Session Options

        protected BbergAPI.SessionOptions _sessionOptions = new BbergAPI.SessionOptions();
        public BbergAPI.SessionOptions sessionOptions
        {
            get
            {
                if (_sessionOptions == null)
                { this.initializeSessionOptions(); }

                return _sessionOptions;
            }


            protected set
            {
                this._sessionOptions = value;
            }
        }


        public void initializeSessionOptions()
        {
            this._sessionOptions.ServerHost = "localhost";
            this._sessionOptions.ServerPort = 8194;
        }

        #endregion


        // BLOOMBERG SESSION 
        #region Bloomberg Session

        protected BbergAPI.Session _session;
        public BbergAPI.Session session
        {
            get
            {
                if (_session == null)
                {
                    this.initializeSession();
                    this.initializeSessionOptions();
                }

                return this._session;
            }


            protected set
            {
                this._session = value;
            }
        }


        public void initializeSession()
        {
            this.session = new BbergAPI.Session(this.sessionOptions);
            if (!this.session.Start())
            {
                System.Console.WriteLine("Could not start session.");
                System.Environment.Exit(1);
            }
            if (!this.session.OpenService("//blp/refdata"))
            {
                System.Console.WriteLine("Could not open service " + "//blp/refdata");
                System.Environment.Exit(1);
            }
        }

        #endregion


        // CORRELATION ID
        #region Bloomberg Correlation ID

        protected BbergAPI.CorrelationID _correlationID = new BbergAPI.CorrelationID(1);
        public BbergAPI.CorrelationID correlationID
        {
            get
            {
                return _correlationID;
            }


            protected set
            {
                this._correlationID = value;
            }
        }



        #endregion


        // SERVICE
        #region Bloomberg Service

        protected BbergAPI.Service _service;
        public BbergAPI.Service service
        {
            get
            {
                if (this._service == null) { this.initializeService(); }
                return this._service;
            }


            protected set
            {
                this._service = value;
            }
        }

        public void initializeService()
        {

            this.service = this.session.GetService("//blp/refdata");

        }

        #endregion


        // REQUEST
        #region Bloomberg Request

        protected BbergAPI.Request _request;
        public BbergAPI.Request request
        {
            get
            {
                return this._request;
            }


            protected set
            {
                this._request = value;
            }
        }


        #endregion


        #endregion



        // ************************************************************
        // INSTANCE PROPERTIES -- IDENTIFICATIONS
        // ************************************************************

        #region

        // Ticker (as string, Bloomberg proprietary field)
        #region Ticker

        protected string _bloombergTicker;
        public string bloombergTicker
        {
            get
            {
                return this._bloombergTicker;
            }


            protected set
            {
                this._bloombergTicker = value;
            }
        }


        #endregion


        // DBID (as DBID, local unique identification field)
        #region DBID

        protected DBID _dbid;
        public int dbid
        {
            get
            {
                return this._dbid.ToInt();
            }

        }


        #endregion


        // BLOOMBERG FIELDS                               
        #region Bloomberg Fields to be requested

        protected Dictionary<string, string> _fieldNames = new Dictionary<string, string>() { };
        public Dictionary<string, string> fieldNames
        {
            get
            {
                return this._fieldNames;
            }


            protected set
            {
                this._fieldNames = value;
            }
        }


        #endregion


        // OVERRIDE FIELDS              --------------------------->>>> ca sert à quoi ?? 
        #region Bloomberg Override Fields ???????????????

        protected List<string> _overrideFields = new List<string>();
        public List<string> overrideFields
        {
            get
            {
                return this._overrideFields;
            }


            protected set
            {
                this._overrideFields = value;
            }
        }


        #endregion


        // OVERRIDE VALUES              --------------------------->>>> ca sert à quoi ?? 
        #region Bloomberg Override Values ???????????????

        protected List<string> _overrideValues = new List<string>();
        public List<string> overrideValues
        {
            get
            {
                return this._overrideValues;
            }


            protected set
            {
                this._overrideValues = value;
            }
        }


        #endregion


        #endregion



        // ************************************************************
        //  OPTIMIZATION FIELDS
        // ************************************************************


        // BLOOMBERG FIELDS                               
        #region Bloomberg Fields to be requested

        protected Dictionary<string, int> _skip;
        public Dictionary<string, int> skip
        {
            get
            {
                if (this._skip == null) { this._initializeSkipFields(); }// with underscore (return nothing) 
                return this._skip;
            }


            protected set
            {
                this._skip = value;
            }
        }


      

        private void _initializeSkipFields()
        {

            _skip = new Dictionary<string, int>();

            foreach (string localKey in this.fieldNames.Keys)
            {
                _skip[localKey] = 0;
            }


        }


        private Dictionary<string, int> initializeSkipFields()
        {

            Dictionary<string, int> ans = new Dictionary<string, int>();

            foreach (string localKey in this.fieldNames.Keys)
            {
                ans[localKey] = 0;
            }

            return ans;
        }


        #endregion


        // SCALING FACTORS                             
        #region Bloomberg scaling factors

        protected Dictionary<String, Double> _scaling;
        public Dictionary<String, Double> scaling
        {
            get
            {
                if (this._scaling == null) { this._initializeScalingFactors(); }
                return this._scaling;
            }


            protected set
            {
                this._scaling = value;
            }
        }




        private void _initializeScalingFactors()
        {

            _scaling = new Dictionary<String, Double>();

            foreach (string localKey in this.fieldNames.Keys)
            {
                _scaling[localKey] = 1.0;
            }


        }


      

        #endregion




        // ************************************************************
        // METHODS -- DATES & DATES SETTING
        // ************************************************************

        #region


        // FROM DATE
        #region Bloomberg Request Start Date

        private DateTime _fromDate = new DateTime(2013, 12, 31);
        public DateTime fromDate
        {
            get
            {
                return this._fromDate;
            }

            set
            {
                this._fromDate = value;
            }


        }


        public string fromDateString
        {
            get { return this._fromDate.ToString(BLOOMBERG_DATE_FORMAT); }
        }



        #endregion


        // TO DATE
        #region Bloomberg Request End Date

        protected DateTime _toDate = DateTime.Today.AddDays(-1);
        public DateTime toDate
        {
            get
            {
                return this._toDate;
            }

            set
            {
                this._toDate = value;
            }


        }


        public string toDateString
        {
            get { return this._toDate.ToString(BLOOMBERG_DATE_FORMAT); }
        }

        #endregion



        public void SetFromDate()
        {
            // this._fromDate = argDate;

            string dateString;
            dateString = this.fromDateString;
            this.request.Set("startDate", dateString);
        }


        public void SetToDate()
        {
            // this._toDate = argDate;

            string dateString;
            dateString = this.toDateString;
            this.request.Set("endDate", dateString);
        }


        protected void SetRequestDates()
        {

            // Set start date
            this.request.Set(START_DATE, fromDateString);

            // Set end date
            this.request.Set(END_DATE, toDateString);

        }



        #endregion




        // ************************************************************
        // METHODS -- FIELDS & TICKERS SETTING
        // ************************************************************

        #region

        public void appendTickers()
        {

            if (this.bloombergTicker == "") { throw new System.ArgumentException("BloombergTickerException", "Ticker must be defined."); }
            this.request.Append(SECURITIES, @bloombergTicker);

        }

        public void appendFields()
        {
            foreach (string key in this.fieldNames.Keys)
            {
                if (key == "") { throw new System.ArgumentException("BloombergFieldException", "Fields must be defined."); }
                this.request.GetElement(FIELDS).AppendValue(fieldNames[key]);
            }
        }

        #endregion



        // ************************************************************
        // METHODS -- REQUEST RELATED
        // ************************************************************

        #region

        private void OpenConnection()
        {
            // Start session with local options
            //      BbergAPI.SessionOptions sessionOptions = new BbergAPI.SessionOptions();
            //      session = new BbergAPI.Session(sessionOptions);
            this.session = new BbergAPI.Session(this.sessionOptions);

            // Error catching : Unable to start session
            if (!this.session.Start()) { throw new Exception(SESSION_EXCEPTION); }

            // Error catching : Session unavailable
            if (!this.session.OpenService(REFERENCE_DATA_SERVICE)) throw new Exception(SERVICE_EXCEPTION);

            // Start service
            this.service = this.session.GetService(REFERENCE_DATA_SERVICE);

            // Create request
            this.request = this.service.CreateRequest(requestType);

        }

        private void InitializeRequest()
        {

            this.appendTickers();
            this.appendFields();
            this.SetRequestDates();
            this.InitializeRequestOptions();

            // conditionally, append overrides into request object
            if (overrideFields.Count > 0)
            {
                BbergAPI.Element requestOverrides = request.GetElement(OVERRIDES);
                for (int i = 0; i < overrideFields.Count; i++)
                {
                    BbergAPI.Element requestOverride = requestOverrides.AppendElement();
                    requestOverride.SetElement(FIELD_ID, overrideFields[i]);
                    requestOverride.SetElement(VALUE, overrideValues[i]);
                }
            }


        }

        private void InitializeRequestOptions()
        {

            // Set Pricing Option (yield / price)
            request.Set(PRICING_OPTION, Option_PricingOption.ToString());

            // Set Periodicity adjustment
            request.Set(PERIODICITY_ADJUSTMENT, Option_PeriodicityAdjustement.ToString());

            // Set Periodicity
            request.Set(PERIODICITY_SELECTION, Option_PeriodicitySelection.ToString());

            // Set non trading day fill option
            request.Set(NON_TRADING_DAY_FILL_OPTION, Option_NonTradingDayFillOption.ToString());

            // Set non trading day fill method
            request.Set(NON_TRADING_DAY_FILL_METHOD, Option_NonTradingDayFillMethod.ToString());

            // Override Currency
            if (overrideCurrency != String.Empty) { request.Set(OVERRIDE_CURRENCY, overrideCurrency); }

        }
        
        private void SendRequest()
        {

            long ID = Guid.NewGuid().GetHashCode();

            this.session.SendRequest(request, new BbergAPI.CorrelationID(ID));

        }

        private void CloseConnection()
        {
            if (session != null) this.session.Stop();
        }


        #endregion


        // ************************************************************
        // METHODS -- PARSING DATA TO DATABASE_LINES
        // ************************************************************


        public List<T> Fetch()
        {

            this.OpenConnection();

            this.InitializeRequest();

            this.SendRequest();

            try 
            {
                return ParseResponse();
            }

            catch 
            { 
                this.CloseConnection();
                return new List<T>();
            }



        }

        private List<T> ParseResponse()
        {

            List<T> output = new List<T>();
            bool isProcessing = true;


            while (isProcessing)
            {

                BbergAPI.Event response = this.session.NextEvent();

                switch (response.Type)
                {
                    case BbergAPI.Event.EventType.PARTIAL_RESPONSE:
                        output = ParseEvent(response, output); // ----------->> THIS HAS BEEN MODIFIED, PREVIOUS VERSION : ParseEvent_OLD
                        break;

                    case BbergAPI.Event.EventType.RESPONSE:
                        output = ParseEvent(response, output); // ----------->> THIS HAS BEEN MODIFIED, PREVIOUS VERSION : ParseEvent_OLD
                        isProcessing = false;
                        break;

                    default:
                        break;

                }
            }

            return output; 

        }

        private List<T> ParseEvent_OLD(BbergAPI.Event response, List<T> outputContainer)
        {

            foreach (BbergAPI.Message message in response.GetMessages())
            {

                // Extract security
                BbergAPI.Element security = message.GetElement(SECURITY_DATA);
                string currentSec = (string)security.GetElementAsString(TICKER);

                // Extract fields
                BbergAPI.Element fields = security.GetElement(FIELD_DATA);

                int sequenceNumber = security.GetElementAsInt32(SEQUENCE_NUMBER);

                Dictionary<string, int> skipFields = this.initializeSkipFields();

                // Loop through all observation dates
                for (int i = 0; i < fields.NumValues; i++)
                {

                    // Determine type of <T> and create instance
                    var Ttype = typeof(T);
                    var thisLine = (T)Activator.CreateInstance(Ttype);

                    // extract all field data for a single observation date
                    BbergAPI.Element observationDateFields = fields.GetValueAsElement(i);

                    string currentStringDate = observationDateFields.GetElementAsString(DATE);
                    DateTime currentDate = DateTime.ParseExact(currentStringDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                    // Determine type of <T> and create instance
                    thisLine.SetDate(currentDate);
                    thisLine.SetDBID(dbid);

                    // Fill the line
                    skipFields = thisLine.SetFromBloomberg(ref observationDateFields, skipFields);

                    // Add to output container
                    outputContainer.Add(thisLine);

                }
            }

            // This kills the data inside the response object...
            this.CloseConnection();

            return outputContainer;

        }

        private List<T> ParseEvent(BbergAPI.Event response, List<T> outputContainer)
        {

            foreach (BbergAPI.Message message in response.GetMessages())
            {

                // Extract security
                BbergAPI.Element security = message.GetElement(SECURITY_DATA);
                string currentSec = (string)security.GetElementAsString(TICKER);

                // Extract fields
                BbergAPI.Element fields = security.GetElement(FIELD_DATA);

                int sequenceNumber = security.GetElementAsInt32(SEQUENCE_NUMBER);
                
                // Loop through all observation dates
                for (int i = 0; i < fields.NumValues; i++)
                {

                    // Determine type of <T> and create instance
                    var Ttype = typeof(T);
                    var thisLine = (T)Activator.CreateInstance(Ttype);

                    // extract all field data for a single observation date
                    BbergAPI.Element observationDateFields = fields.GetValueAsElement(i);

                    string currentStringDate = observationDateFields.GetElementAsString(DATE);
                    DateTime currentDate = DateTime.ParseExact(currentStringDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                    // Set the date and DBID to identify the line
                    thisLine.SetDate(currentDate);
                    thisLine.SetDBID(dbid);

                    // Fill the line with data
                    foreach(string localKey in this.fieldNames.Keys)
                    {
                        if (skip[localKey] < 10000)
                        {
                            try
                            {
                                // Warning : Bloomberg displays interest rates in percentage terms
                                thisLine[localKey] = scaling[localKey] * (double)observationDateFields.GetElementAsFloat64(fieldNames[localKey]);
                            }

                            catch
                            {
                                // Some log ?
                                skip[localKey] += 1;
                                // thisLine[localKey] = System.DBNull.Value;
                                thisLine[localKey] = null;
                                //thisLine[localKey] = 0.0;
                            }
                        }
                    }

                    // Add to output container
                    outputContainer.Add(thisLine);

                }
            }

            // This kills the data inside the response object...
            this.CloseConnection();

            return outputContainer;

        }


    }
}
