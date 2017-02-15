using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.Containers;
using QLyx.DataIO.Connector;

using QLNet;

namespace QLyx.DataIO
{
    public class DataReader
    {
        
        // ************************************************************
        // INSTANCE PROPERTIES - CUSTOM OBJECTS
        // ************************************************************


        // CACHE
        #region Cache object

        // to implement 

        #endregion


        // DATABASE MANAGER
        #region Database Accessor 

        protected DatabaseHelper _myDBhelper = new DatabaseHelper();

        #endregion




        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************


        #region Constructors

        // Constructor 1 : Generic
        public DataReader() { }


        #endregion




        // ************************************************************
        // METHODS -- TO GET EOD PRICES
        // ************************************************************

        public myFrame EndOfDay(IDtoken IdToken, List<string> LocalFields, DateTime StartDate, DateTime EndDate,  TimeUnit Periodicity, String Source)
        {

            // 1. Formulate Request
            HistoricalDataRequest myRequest = new HistoricalDataRequest(IdToken, LocalFields, StartDate, EndDate, Periodicity, Source);

            // 2. Send request to Database Helper
            myFrame localData = _myDBhelper.GetEODPrices(myRequest);

            // 3. Return
            return localData;
            

        }

        public myFrame EndOfDay(IDtoken IdToken, List<string> LocalFields, DateTime StartDate, DateTime EndDate, TimeUnit Periodicity)
        {
            return EndOfDay(IdToken, LocalFields, StartDate, EndDate, Periodicity,  "");
        }

        public myFrame EndOfDay(IDtoken IdToken, List<string> LocalFields, DateTime StartDate, DateTime EndDate)
        {
            return EndOfDay(IdToken, LocalFields, StartDate, EndDate, TimeUnit.Days, "");
        }

        public myFrame EndOfDay(IDtoken IdToken, List<string> LocalFields, DateTime StartDate)
        {
            return EndOfDay(IdToken, LocalFields, StartDate, DateTime.Today, TimeUnit.Days, "");
        }

        public myFrame EndOfDay(IDtoken IdToken, DateTime StartDate, DateTime EndDate)
        {
            return EndOfDay(IdToken, new List<string>(), StartDate, EndDate, TimeUnit.Days, "");
        }

        public myFrame EndOfDay(IDtoken IdToken, DateTime ObsDate)
        {
            return EndOfDay(IdToken, new List<string>(), ObsDate, ObsDate, TimeUnit.Days, "");
        }


        public myFrame EndOfDay(IDtoken IdToken, List<string> LocalFields)
        {
            return EndOfDay(IdToken, LocalFields, new DateTime(2013,12,31), DateTime.Today, TimeUnit.Days, "");
        }

        public myFrame EndOfDay(IDtoken IdToken)
        {
            return EndOfDay(IdToken, new List<string>(), new DateTime(2013, 12, 31), DateTime.Today, TimeUnit.Days, "");
        }





    }
}
