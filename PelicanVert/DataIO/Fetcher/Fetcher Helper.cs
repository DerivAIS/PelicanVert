using QLyx.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Fetcher
{
    public class FetcherHelper
    {


        // ************************************************************
        // STATIC PROPERTIES -- MAPPING AT CLASS LEVEL
        // ************************************************************

        #region STATIC -- Bloomberg Mapping

        protected static Dictionary<string, string> BloombergDict = new Dictionary<string, string>
                {
                    { "Bid", "PX_BID"  }, // Modifie de BID à PX_BID ... même chose pour l'ask
                    { "Ask", "PX_ASK"  },

                    { "Open", "PX_OPEN"  }, 
                    { "High", "PX_HIGH"  },
                    { "Low", "PX_LOW"  },
                    { "Close", "PX_LAST"  },
                    { "Volume", "VOLUME"  },
                    { "AdjustedClose", ""  },
                    { "Last", "PX_LAST"  }

                };


        static protected string Bloomberg(string arg)
        {
            
            return BloombergDict[arg];

        }


        static protected Dictionary<String, String> MapBloomberg(List<String> localFields)
        {

            Dictionary<String, String> output = new Dictionary<string, string>();
            
            foreach(String s in localFields)
            {
                //if (BloombergDict[s] != "") // ----------------------------------------------------------------  REMOVED THE IF
                //{
                    output[s] = BloombergDict[s];
                //}
            }

            return output;

        }


        #endregion



        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region Constructors

        public FetcherHelper() { }

        #endregion


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region


        protected string _externalID;
        public string externalID
        {
            get
            {
                return this._externalID;
            }


            protected set
            {
                this._externalID = value;
            }
        }

        
        protected DBID _dbid;
        public DBID dbid
        {
            get
            {
                return this._dbid;
            }
            protected set
            {
                this._dbid = value;
            }

        }


        protected Dictionary<string, string> _mappedFields = new Dictionary<string, string>();
        public Dictionary<string, string> mappedFields
        {
            get
            {
                return this._mappedFields;
            }

            protected set
            {
                this._mappedFields = value;
            }

        }


        protected string _source = "";
        public string source
        {
            get
            {
                return this._source;
            }


            protected set
            {
                this._source = value;
            }
        }


        #endregion





        // ************************************************************
        // METHODS -- FETCH (AND DELEGATES)
        // ************************************************************

        #region Fetch

        public myFrame FetchData(HistoricalDataRequest myRequest, Type ReturnContainerType)
        { 

            // 1. Map available sources
            Dictionary<string, string> sources = MapSources(myRequest.id);

            // 2. Map external id for each source
            Dictionary<string, string> extIds = MapExtIDs(myRequest.id);

            // 3. Return Bloomberg @TODO
            // this always return Bloomberg... need to change !! @TODO

            return FetcherRouter(Source: "Bloomberg", Request: myRequest, ExternalID: extIds, ContainerType: ReturnContainerType);
            

        }



        private myFrame FetcherRouter(string Source, HistoricalDataRequest Request, Dictionary<string, string> ExternalID, Type ContainerType)
        {

            switch (Source)
            {

                case "Bloomberg":
                    return FetchData_Bloomberg(Request, ExternalID, ContainerType);

                default:
                    { throw new System.ArgumentException("MappingException", "Fetcher Helper unable to map the external source."); }

            }

        }


        private myFrame FetchData_Bloomberg(HistoricalDataRequest myRequest, Dictionary<string, string> ExternalID, Type ContainerType)
        {

            // CASE A : INTEREST RATE
            if (ContainerType == typeof(InterestRate_Line))
            {

                var type = typeof(InterestRate_Line);
                var Tobj = (InterestRate_Line)Activator.CreateInstance(type);

                Dictionary<String, String> Mapping = MapExternalFieldsToLocal("Bloomberg", Tobj.GetDataFields());
                Dictionary<String, Double> scalingFactors = MapScalingFactors("Bloomberg", Tobj.GetDataFields(), ContainerType);

                List<InterestRate_Line> myRes = Fetch_Bloomberg<InterestRate_Line>(myRequest.id.DBID, myRequest.id.Bloomberg, Mapping, myRequest.startDate, myRequest.endDate, scalingFactors);
                return new myFrame(myRes);
            }

            
            // CASE B : EQUITY
            else if (ContainerType == typeof(Equity_Line))
            {
                var type = typeof(Equity_Line);
                var Tobj = (Equity_Line)Activator.CreateInstance(type);

                Dictionary<String, String> Mapping = MapExternalFieldsToLocal("Bloomberg", Tobj.GetDataFields());
                Dictionary<String, Double> scalingFactors = MapScalingFactors("Bloomberg", Mapping.Keys.ToList(), ContainerType);

                List<Equity_Line> myRes = Fetch_Bloomberg<Equity_Line>(myRequest.id.DBID, myRequest.id.Bloomberg, Mapping, myRequest.startDate, myRequest.endDate, scalingFactors);
                return new myFrame(myRes);
            }


            // CASE C : EQUITY VOLATILITY
            else if (ContainerType == typeof(EquityVolatility_Line))
            {
                var type = typeof(EquityVolatility_Line);
                var Tobj = (EquityVolatility_Line)Activator.CreateInstance(type);

                Dictionary<String, String> Mapping = MapExternalFieldsToLocal("Bloomberg", Tobj.GetDataFields());
                Dictionary<String, Double> scalingFactors = MapScalingFactors("Bloomberg", Tobj.GetDataFields(), ContainerType);

                List<EquityVolatility_Line> myRes = Fetch_Bloomberg<EquityVolatility_Line>(myRequest.id.DBID, myRequest.id.Bloomberg, Mapping, myRequest.startDate, myRequest.endDate, scalingFactors);
                return new myFrame(myRes);
            }
          

            // CASE D : DEFAULT (EXCEPTION THROWN)
            else 
            { 
                throw new System.ArgumentException("FetcherException", "Bloomberg Fetcher delegation failed, unable to identify the return type."); 
            }

        }


        protected List<T> Fetch_Bloomberg<T>(DBID dbid_, String externalID_, Dictionary<String, String> Fields, 
            DateTime startDate_, DateTime endDate_, Dictionary<String, Double> scaling_) where T : GenericDatabaseLine
        {


            Dictionary<string, double> Scaling = new Dictionary<string, double>() 
                {   
                    {"Open", 1.0}, 
                    {"High", 1.0},
                    {"Low", 1.0},
                    {"Close", 1.0}, 
                    {"Volume", 1.0},
                    {"Bid", 1.0},                 
                    {"Ask", 1.0}, 
                    {"AdjustedClose", 1.0 } };


            Dictionary<string, string> BloombergDict = new Dictionary<string, string>
                {
                    { "Bid", "PX_BID"  }, // Modifie de BID à PX_BID ... même chose pour l'ask
                    { "Ask", "PX_ASK"  },
                    { "Open", "PX_OPEN"  }, 
                    { "High", "PX_HIGH"  },
                    { "Low", "PX_LOW"  },
                    { "Close", "PX_LAST"  },
                    { "Volume", "VOLUME"  },
                    { "AdjustedClose", ""  } };



            // Fetch
            var myBbergFetcher = new BloombergFetcher<T>(dbid_, externalID_, Fields, startDate_, endDate_, scaling_);
            //var myBbergFetcher = new BloombergFetcher<T>(dbid_, externalID_, BloombergDict, startDate_, endDate_, Scaling);

            // Return
            return myBbergFetcher.Fetch();

        }




        private Dictionary<string, string> MapExtIDs(IDtoken Idtoken)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            string[] availableSources = Enum.GetNames(typeof(ExternalSources));

            foreach (string key in availableSources)
            {
                if (!(Idtoken[(string)key] == "")) { output[key] = (string)Idtoken[key]; }
            }

            return output;
        }


        private Dictionary<string, string> MapSources(IDtoken Idtoken)
        {
            List<string> fields = new List<string>() { "Source1", "Source2", "Source3", "Source4", "Source5"};

            Dictionary<string, string> output = new Dictionary<string, string>() { { "Source1", "" }, { "Source2", "" }, { "Source3", "" }, { "Source4", "" }, { "Source5", "" }};

            foreach (string key in fields)
            {
                if (!(Idtoken[(string)key] == "")) { output[key] = (string)Idtoken[key]; }
            }

            return output;
        }


   


        #endregion



        // ************************************************************
        // METHODS -- MAPPING OF FIELDS & SCALING FACTORS
        // ************************************************************

        #region Mapping methods


        protected Dictionary<string, string> MapExternalFieldsToLocal(string source, List<string> Fields)
        {
            switch (source)
            {

                case "Bloomberg":
                    return Map_BloombergFieldsToLocal(Fields);
                   
                default:
                    { throw new System.ArgumentException("FetcherHelperMappingException", "Fetcher mapper unable to map local fields to external fields."); }

            }
        }


        protected Dictionary<String, String> Map_BloombergFieldsToLocal(List<string> argFields)
        {
            // Sanity check
            if (argFields.Count() == 0) { throw new System.ArgumentException("FetcherHelperSourceException", "Fetcher Helper unable to map the source from reference table.");}

            // Map fields with static method
            Dictionary<String, String> mappedFields = new Dictionary<String, String>();
            mappedFields = FetcherHelper.MapBloomberg(argFields);

            // Return
            return mappedFields;

        }



        protected void MapSource(IDtoken CompleteID)
        {
            if (CompleteID.Source1 == null)
            {
                throw new System.ArgumentException("FetcherHelperSourceException", "Fetcher Helper unable to map the source from reference table.");
            }

            source = CompleteID.Source1;
        }



        private void MapLocalID(IDtoken CompleteID)
        {
            if (CompleteID.DBID == 0)
            {
                throw new System.ArgumentException("FetcherHelperSourceException", "Fetcher Helper: reference line does not contain DBID.");
            }

            this.dbid = new DBID(CompleteID.DBID);

        }


        private void MapExternalID(string source, IDtoken CompleteID)
        {
            if (CompleteID[source] == null)
            {
                throw new System.ArgumentException("FetcherHelperExtIDException", "Fetcher Helper: Unable to get the external ID from the reference line.");
            }

            this.externalID = (string)CompleteID[source];

        }




        protected Dictionary<String, Double> MapScalingFactors(string source, List<string> Fields, Type ContainerType)
        {
            switch (source)
            {

                case "Bloomberg":
                    return MapScalingFactors_Bloomberg(Fields, ContainerType);

                default:
                    { throw new System.ArgumentException("FetcherHelperMappingException", "Fetcher mapper unable to map scaling factors."); }

            }

        }


        protected Dictionary<String, Double> MapScalingFactors_Bloomberg(List<string> argFields, Type ContainerType)
        {

            // CASE A : INTEREST RATE
            if (ContainerType == typeof(InterestRate_Line))
            {
                    return FillScalingDict(0.01, argFields);
            }

            // CASE B : Equity
            if (ContainerType == typeof(Equity_Line))
            {
                return FillScalingDict(1.0, argFields);
            }

            // CASE C : Equity Volatility
            if (ContainerType == typeof(EquityVolatility_Line))
            {
                return FillScalingDict(1.0, argFields);
            }

            
            else { throw new System.ArgumentException("FetcherHelperMappingException", "Fetcher mapper unable to map scaling factors."); }

         }

        


        private Dictionary<string, double> FillScalingDict(double scalingFactor, List<string> argFields)
        {
            Dictionary<String, Double> output = new Dictionary<string, double>();

            foreach(string s in argFields)
            {
                output[s] = scalingFactor;
            }

            return output;
        }





        #endregion



    }
}
