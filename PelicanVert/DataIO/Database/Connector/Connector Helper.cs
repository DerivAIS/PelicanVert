using QLyx.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Connector
{
    public class ConnectorHelper
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        // protected readonly myDB_Connector myDB_connector = new myDB_Connector();
        // Add other 'GenericConnector' implementations here




        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1: Generic
        public ConnectorHelper() { }

        #endregion




        // ************************************************************
        // METHODS -- RETREIVE DATA
        // ************************************************************

        #region Retreive / Get data


        public myFrame GetData(HistoricalDataRequest myRequest, string Database, string Table, Type ReturnContainerType)
        {

            switch (Database)
            {

                case "myDB":
                    return GetData_myDB(myRequest, Table, ReturnContainerType);

                default:
                    { throw new System.ArgumentException("CH_MappingException", "Connector Helper unable to map the database."); }

            }
        }


        private myFrame GetData_myDB(HistoricalDataRequest myRequest, string Table, Type ContainerType)
        {

            switch (Table)
            {

                case "InterestRate":
                    return GetData_myDB_InterestRate(myRequest, ContainerType);

                case "Equity":
                    return GetData_myDB_Equity(myRequest, ContainerType);

                case "EquityVolatility":
                    return GetData_myDB_EquityVolatility(myRequest, ContainerType);

                default:
                    { throw new System.ArgumentException("CH_MappingException", "Connector Helper unable to map the database."); }

            }

        }


        private myFrame GetData_myDB_InterestRate(HistoricalDataRequest myRequest, Type ContainerType)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<InterestRate_Line> myRes = myConnect.Select<InterestRate_Line>(myRequest.id.DBID, myRequest.fields, myRequest.startDate, myRequest.endDate);
            return new myFrame(myRes);

        }


        private myFrame GetData_myDB_Equity(HistoricalDataRequest myRequest, Type ContainerType)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<Equity_Line> myRes = myConnect.Select<Equity_Line>(myRequest.id.DBID, myRequest.fields, myRequest.startDate, myRequest.endDate);
            return new myFrame(myRes);
        }


        private myFrame GetData_myDB_EquityVolatility(HistoricalDataRequest myRequest, Type ContainerType)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<EquityVolatility_Line> myRes = myConnect.Select<EquityVolatility_Line>(myRequest.id.DBID, myRequest.fields, myRequest.startDate, myRequest.endDate);
            return new myFrame(myRes);
        }


        #endregion



        // ************************************************************
        // METHODS -- INSERT DATA
        // ************************************************************

        #region Insert data

        
        public void SetData(IDtoken idTok, string Database, string Table, myFrame Data)
        {

            switch (Database)
            {

                case "myDB":
                    SetData_myDB(idTok, Table, Data);
                    break;

                default:
                    { throw new System.ArgumentException("CH_MappingException", "Connector Helper unable to map the database."); }

            }
        }



        private void SetData_myDB(IDtoken idTok, string Table, myFrame Data)
        {

            switch (Table)
            {

                case "InterestRate":
                    SetData_myDB_InterestRate(idTok, Data);
                    break;

                case "Equity":
                    SetData_myDB_Equity(idTok, Data);
                    break;

                case "EquityVolatility":
                    SetData_myDB_EquityVolatility(idTok, Data);
                    break;

                default:
                    { throw new System.ArgumentException("CH_MappingException", "Connector Helper unable to map the database."); }

            }

        }


        private void SetData_myDB_InterestRate(IDtoken idTok, myFrame data)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<InterestRate_Line> dataToInsert = data.ToList<InterestRate_Line>(idTok.DBID);

            myConnect.Insert<InterestRate_Line>(idTok, dataToInsert);
            

        }

        private void SetData_myDB_Equity(IDtoken idTok, myFrame data)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<Equity_Line> dataToInsert = data.ToList<Equity_Line>(idTok.DBID);

            myConnect.Insert<Equity_Line>(idTok, dataToInsert);


        }

        private void SetData_myDB_EquityVolatility(IDtoken idTok, myFrame data)
        {
            myDB_Connector myConnect = new myDB_Connector();
            List<EquityVolatility_Line> dataToInsert = data.ToList<EquityVolatility_Line>(idTok.DBID);

            myConnect.Insert<EquityVolatility_Line>(idTok, dataToInsert);


        }
    

        #endregion

    }
}
