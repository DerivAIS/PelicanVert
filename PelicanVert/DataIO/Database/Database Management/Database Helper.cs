using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO.Connector;
using QLyx.DataIO.Fetcher;
using QLyx.Containers;



namespace QLyx.DataIO
{

    public class DatabaseHelper 
    {


        // ************************************************************
        // DATABASE PROPERTIES
        // ************************************************************


        #region Instance properties


        // Reference table manager
        protected ReferenceManager refManager = ReferenceManager.Factory;

        // Database Connector Helper
        protected ConnectorHelper myConnectorHelper = new ConnectorHelper();

        // Fetcher Helper
        protected FetcherHelper myFetcherHelper = new FetcherHelper();


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************


        #region Constructors

        // Constructor 1 : Generic
        public DatabaseHelper() { }

        #endregion



        // ************************************************************
        // METHODS -- GET DATA
        // ************************************************************

        public myFrame GetEODPrices(HistoricalDataRequest myRequest)
        {

            // Check local database 
            myFrame localData = Get_FromLocalDatabase(myRequest);


            // CASE A : NO DATA PRESENT IN LOCAL DB
            if (localData.IsEmpty() == true) 
            {
                // 1. Fetch the data from external sources
                localData = Get_FromExternalSource(myRequest);

                // 2. Perform some checks 
                // @TODO : checks needed
                
                // 3. Map the local DB
                string database = MapDatabase(myRequest.id);
                string table = MapTable(myRequest.id);

                // 4. Assign to local DB
                myConnectorHelper.SetData(myRequest.id, database, table, localData);
                
                // 5. Return the data
                return localData;
            }



            // CASE B : DATA FOUND
            else
            {
                // Check data for completeness (start date included in local data)
                bool startDatePresent = (myRequest.startDate >= localData.startDate);

                // Check data for completeness (end date included in local data)
                bool endDatePresent = (myRequest.endDate <= localData.endDate);

                // Which leads to 4 possible cases...

                // CASE B-1 : Time period covered by local data
                if (startDatePresent && endDatePresent) { return localData; }

                // CASE B-2 : Start Date Covered but not end date
                else if (startDatePresent && !endDatePresent) 
                {
                    myFrame endData = UpdateData(myRequest, localData.endDate.AddDays(1), myRequest.endDate);

                    // Merge with existing data
                    localData.Union(endData);

                    // Return 
                    return localData;

                }

                // CASE B-3 : Start Date not covered but end date present
                else if (!startDatePresent && endDatePresent) // ----------------> Verified
                {
                    // Update local database
                    myFrame startData = UpdateData(myRequest, myRequest.startDate, localData.startDate.AddDays(-1)); 

                    // Merge with existing data
                    localData.Union(startData);

                    // Return 
                    return localData;

                }

                // CASE B-4 : Both ends not covered
                else 
                {
                    //myFrame startData = UpdateData(myRequest, localData.startDate, myRequest.startDate.AddDays(-1));
                    myFrame startData = UpdateData(myRequest, myRequest.startDate, localData.startDate.AddDays(-1));
                    localData.Union(startData);

                    myFrame endData = UpdateData(myRequest, myRequest.endDate.AddDays(1), localData.endDate);
                    localData.Union(endData);

                    return localData;
                }


            }
            
        }


        private myFrame UpdateData(HistoricalDataRequest oldRequest, DateTime newStartDate, DateTime newEndDate)
        {

            // Formulate request
            HistoricalDataRequest newRequest = new HistoricalDataRequest(oldRequest.id, oldRequest.fields, newStartDate, 
                                                                            newEndDate, oldRequest.periodicity, oldRequest.source);
            
            // Fetch data
            myFrame startData = Get_FromExternalSource(newRequest);
     
            // Map the local DB
            string database = MapDatabase(newRequest.id);
            string table = MapTable(newRequest.id);

            // Assign to local DB
            myConnectorHelper.SetData(newRequest.id, database, table, startData);

            //Return data
            return startData;

        }



    
        public myFrame Get_FromLocalDatabase(HistoricalDataRequest myRequest)
        {

            // 1. Identify request type
            string requestType = MapRequestType(myRequest);

            // 2. Map database
            string database = MapDatabase(myRequest.id);

            // 3. Map table
            string table = MapTable(myRequest.id);

            // 4. Get the correct container type
            Type ContainerType = MapContainer(database, table);

            // 5. Send request to connector helper
            return myConnectorHelper.GetData(myRequest, database, table, ContainerType);
   
        }
        
        public myFrame Get_FromExternalSource(HistoricalDataRequest myRequest)
        {

            // 1. Identify request type
            string requestType = MapRequestType(myRequest);

            // 2. Map database
            string database = MapDatabase(myRequest.id);

            // 3. Map table
            string table = MapTable(myRequest.id);

            // 4. Get the correct container type
            Type ContainerType = MapContainer(database, table);

            // 5. Send request to connector helper
            return myFetcherHelper.FetchData(myRequest, ContainerType);

        }




        // ************************************************************
        // METHODS -- MAPPING OF REQUEST
        // ************************************************************


        private string MapRequestType(GenericDataRequest requestType)
        {
            switch (requestType.id.AssetClass)
            {

                case "InterestRate":
                    return "InterestRate";

                case "Equity":
                    return "Equity";

                case "EquityVolatility":
                    return "EquityVolatility";

                case "Bond":
                    return "Bond";

                default:
                    { throw new System.ArgumentException("DBH_MappingException", "Database Helper unable to map schema table from the asset class."); }

            }

        }

        private string MapTable(IDtoken id)
        {
            return id.HistoryTable;
        }
        
        private string MapDatabase(IDtoken identification)
        {
            return "myDB";
        }

        private Type MapContainer(string database, string table)
        {

            switch (database)
            {

                case "myDB":
                    return MapContainerFrom_myDB(table);

                default:
                    { throw new System.ArgumentException("DBH_MappingException", "Database Helper unable to map the database."); }

            }

        }
        
        private Type MapContainerFrom_myDB(string table)
        {
            switch (table)
            {
                case "InterestRate":
                    return typeof(InterestRate_Line);

                case "Equity":
                    return typeof(Equity_Line);

                case "Bond":
                    return typeof(Bond_Line);

                case "EquityVolatility":
                    return typeof(EquityVolatility_Line);

                default:
                    { throw new System.ArgumentException("DBH_MappingException", "Database Helper unable to map the database."); }

            }
        }
    }
}
