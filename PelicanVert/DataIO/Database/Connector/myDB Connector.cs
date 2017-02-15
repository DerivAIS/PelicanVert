using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data.SqlClient;

using System.Reflection;

using QLyx.Containers;

using QLyx.DataIO;
using System.Collections;
using QLyx.DataIO.Connector;

namespace QLyx.DataIO.Connector
{

    public class myDB_Connector : GenericDatabaseConnector
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        static string defaultDatabaseSchema = "myDB";
        static string defaultUsername = "guest";
        static string defaultPassword = "1234";
        static string defaultServerIP = "localhost";


        // CONNECTION STRING
        #region Connection String

        protected string _connectionString;
        public string connectionString
        {
            get
            {
                if (_connectionString == null) { _connectionString = buildconnectionString(); }
                return _connectionString;
            }

            protected set
            {
                _connectionString = value;
            }
        }


        #endregion


        string databaseSchema;
        string username;
        string password;
        string serverIP;

        public bool isInitialized;


        #endregion


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        // Constructor 1: Generic
        public myDB_Connector() : this(databaseSchema: defaultDatabaseSchema, username: defaultUsername, password: defaultPassword, serverIP: defaultServerIP) { }


        // Constructor 2: With schema name
        public myDB_Connector(string argDatabaseSchema)
            : this(databaseSchema: argDatabaseSchema, username: defaultUsername, password: defaultPassword, serverIP: defaultServerIP) { }


        // Constructor 3: With schema name and access rights
        public myDB_Connector(string argDatabaseSchema, string argUsername, string argPassword)
            : this(databaseSchema: defaultDatabaseSchema, username: argUsername, password: argPassword, serverIP: defaultServerIP) { }


        // Constructor 4: With schema name, access rights and server IP
        public myDB_Connector(string databaseSchema, string username, string password, string serverIP)
        {
            this.databaseSchema = databaseSchema;
            this.username = username;
            this.password = password;
            this.serverIP = serverIP;
            this.connectionString = this.buildconnectionString();

        }

        #endregion


        // ************************************************************
        // METHODS -- ACCESS RIGHT MANAGEMENT
        // ************************************************************

        #region Access rights

        protected string buildconnectionString()
        {
            // string connectionString = "SERVER=184.92.190.201;DATABASE=schema_sol;UID=hnguyen;PWD=hnguyen";
            string server = "SERVER=" + serverIP;
            string schema = "DATABASE=" + databaseSchema;
            string uid = "UID=" + username;
            string pwd = "PWD=" + password;

            return String.Join(";", new string[4] { server, schema, uid, pwd });
        }

        /*
        protected string buildGenericString(List<string> elements, string separator)
        {
            if (!elements.Contains("Date")) { elements.Add("Date"); }
            return String.Join(separator, elements.ToArray());
        }
        */

        #endregion


        // ************************************************************
        // METHODS -- STRING MANIPULATION
        // ************************************************************

        #region String related

        /// <summary>
        ///  Construct a string from the elements in the list and the separator. Bool atInFront if true adds @ in front of every element.  
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="separator"></param>
        /// <param name="atInFront"></param>
        /// <returns></returns>
        protected string joinString(List<string> elements, string separator, bool atInFront)
        {
            if (!atInFront)
            {
                return String.Join(separator, elements.ToArray());
            }

            else
            {
                List<string> newElements = new List<string>();
                foreach (string s in elements)
                {
                    newElements.Add("@" + s);
                }

                return String.Join(separator, newElements.ToArray());
            }
        }

        /// <summary>
        /// Construct a string from the elements in the list and the separator. Add the field "Date" if not already present. Bool atInFront if true adds @ in front of every element.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="separator"></param>
        /// <param name="atInFront"></param>
        /// <returns></returns>
        protected string joinStringWithDate(List<string> elements, string separator, bool atInFront)
        {

            if (!elements.Contains("Date")) { elements.Add("Date"); }

            return joinString(elements, separator, atInFront);

        }




        /// <summary>
        /// Returns an SQL readable string representation of the DateTime object.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected string buildDateString(DateTime dt)
        {
            return "'" + dt.Year + "-" + dt.Month + "-" + dt.Day + "'";
        }

        #endregion


        // ************************************************************
        // METHODS -- SELECT
        // ************************************************************

        #region SELECT


        /// <summary>
        /// Select an entire table and returns a list of the (T) line objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<T> SelectAll<T>(myDB table)
        {
            string SelectUndlDataEOD = "SELECT * from " + table.ToString();

            using (var connection = new MySqlConnection(connectionString))
            {
                var res = connection.Query<T>(SelectUndlDataEOD);
                return (List<T>)res;
            }


        }


        /// <summary>
        /// Select an entire table and returns a list of the (T) line objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<T> SelectAll<T>(GenericDatabaseLine myLine)
        {
            string SelectUndlDataEOD = "SELECT * from " + myLine.GetTable();

            using (var connection = new MySqlConnection(connectionString))
            {
                var res = connection.Query<T>(SelectUndlDataEOD);
                return (List<T>)res;
            }

        }


        /// <summary>
        /// Produces a list of all distinct elements from a table for a given column (field).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public List<T> ListAllFromField<T>(myDB table, string field)
        {
            // Initialize
            List<T> resultsList = new List<T>();

            // Build command string
            string select_field = "select DISTINCT " + field;
            string select_table = "from " + table.ToString();
            string order_criteria = "order by " + field;
            string sqlCommand = String.Join(" ", new List<string>() { select_field, select_table, order_criteria });

            // Connect and retreive data
            using (var connection = new MySqlConnection(connectionString))
            {
                resultsList = (List<T>)connection.Query<T>(sqlCommand);

            }
            return resultsList;
        }


        /// <summary>
        /// Produces a list of all distinct tickers from a given table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<T> ListAllEquityTickers<T>()
        {
            return ListAllFromField<T>(myDB.Equity, "DBID");
        }


        /// <summary>
        /// Produces a list of all distinct underlying from a given table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<T> ListAllUnderlyings<T>()
        {
            return ListAllFromField<T>(myDB.EquityVolatility, "DBID");
        }


        /// <summary>
        /// Select data from a table in the "myDB" schema..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DBID"></param>
        /// <param name="fieldNames"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<T> Select<T>(DBID DBID, List<string> fieldNames, DateTime startDate, DateTime endDate) where T : IGenericDatabaseLine
        {

            // Determine type of <T> and create instance
            var type = typeof(T);
            var Tobj = (T)Activator.CreateInstance(type);

            // Identify table to be addressed
            string table = Tobj.GetTable();

            //Check dates
            DateTime myStartDate = (startDate == DateTime.MinValue) ? myStartDate = new DateTime(2013, 12, 31) : myStartDate = startDate;
            DateTime myEndDate = (startDate == DateTime.MinValue) ? myEndDate = DateTime.Today : myEndDate = endDate;

            // Check fields
            if (fieldNames.Count == 0) { fieldNames = new List<string>() { "*" }; }

            // Build string with columns to select
            string columnsForSQL = this.joinString(fieldNames, " ,", false);

            // Build string with start date
            string startDateForRequest = this.buildDateString(myStartDate);

            // Build string with end date
            string endDateForRequest = this.buildDateString(myEndDate);

            // Build mySQL Select string
            string mySQLSelect = "SELECT " + columnsForSQL + " from " + table + " where DBID = '" + (uint)DBID.ToInt() + "' and Date >= " + startDateForRequest + " and Date <= " + endDateForRequest + " ORDER BY Date;";



            // Connect and Request
            using (var connection = new MySqlConnection(connectionString))
            {
                var sqlResults = (List<T>)connection.Query<T>(mySQLSelect);
                return (List<T>)sqlResults;
            }

        }


        /// <summary>
        /// Retrieves the data for a given table, a set of tickers, a list of fields between two dates (inclusive). Return data is dict indexed on tickers (string).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <param name="inputTicker"></param>
        /// <param name="fieldNames"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Dictionary<DBID, List<T>> Select<T>(List<DBID> inputTicker, List<string> fieldNames, DateTime startDate, DateTime endDate) where T : IGenericDatabaseLine
        {

            Dictionary<DBID, List<T>> myResult = new Dictionary<DBID, List<T>>();

            foreach (DBID tick in inputTicker)
            {
                myResult[tick] = Select<T>(tick, fieldNames, startDate, endDate);
            }

            return myResult;

        }


        /// <summary>
        /// Retrieves the reference data for a given DBID (well-defined problem). 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <param name="inputTicker"></param>
        /// <param name="fieldNames"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IDtoken Reference(DBID DBID)
        {

            string table = myDB.Reference.ToString();

            // Build mySQL Select string
            string mySQLSelect = "SELECT * from " + table + " where DBID = " + DBID.ToInt();

            // Connect and Request
            using (var connection = new MySqlConnection(connectionString))
            {
                List<IDtoken> sqlResults = (List<IDtoken>)connection.Query<IDtoken>(mySQLSelect);
                return sqlResults.FirstOrDefault();
            }

        }


        /// <summary>
        /// Retrieves the entire reference data for processing (not a well-defined problem). 
        /// <summary>
        public List<IDtoken> Reference()
        {

            string table = myDB.Reference.ToString();

            // Build mySQL Select string
            string mySQLSelect = "SELECT * from " + table;

            // Connect and Request
            using (var connection = new MySqlConnection(connectionString))
            {
                List<IDtoken> sqlResults = (List<IDtoken>)connection.Query<IDtoken>(mySQLSelect);
                return sqlResults;
            }

        }


        #endregion


        // ************************************************************
        // METHODS -- INSERT
        // ************************************************************

        #region

        /// <summary>
        /// Insert data into a table. Data is passed as a line. 
        /// The DB line type must (i) be a subclass of GenericDatabaseLine and (ii) match the given table columns.
        /// </summary>
        /// <param name="argLineToInsert"></param>
        public void Insert_OLD<T>(IDtoken id, List<T> argLineToInsert) where T : GenericDatabaseLine
        {

            if (argLineToInsert.Count() > 0)
            {
            // Identify table to be addressed
            string table = argLineToInsert.FirstOrDefault().GetTable();

            // Build SQL command string
            //DateTime dt = (argLineToInsert as T).GetDate();


            string insert_table = "INSERT into " + table;
            string insert_fields = "(" + joinString(argLineToInsert.FirstOrDefault().GetAllFields(), " ,", false) + ")";
            string insert_values = "values (" + joinStringWithDate(argLineToInsert.FirstOrDefault().GetAllFields(), " ,", true) + ")";
            string InsertUndlDataEOD = joinString(new List<string>() { insert_table, insert_fields, insert_values }, " ", false);

            // Execute SQL command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(InsertUndlDataEOD, argLineToInsert);
            }
            }

        }


        /// <summary>
        /// Insert data into a table. Data is passed as a line. 
        /// The DB line type must (i) be a subclass of GenericDatabaseLine and (ii) match the given table columns.
        /// </summary>
        /// <param name="argLineToInsert"></param>
        public void Insert<T>(IDtoken id, List<T> argLineToInsert, bool includeDate = true) where T : GenericDatabaseLine
        {

            if (argLineToInsert.Count() > 0)
            {
                // Identify table to be addressed
                string table = argLineToInsert.FirstOrDefault().GetTable();

                // Build SQL command string
                //DateTime dt = (argLineToInsert as T).GetDate();


                string insert_table = "INSERT into " + table;
                string insert_fields = "(" + joinString(argLineToInsert.FirstOrDefault().GetAllFields(), " ,", false) + ")";

                string insert_values;

                if(includeDate)
                {
                    insert_values = "values (" + joinStringWithDate(argLineToInsert.FirstOrDefault().GetAllFields(), " ,", true) + ")";
                }
                else
                {
                    insert_values = "values (" + joinString(argLineToInsert.FirstOrDefault().GetAllFields(), " ,", true) + ")";
                }

                string InsertUndlDataEOD = joinString(new List<string>() { insert_table, insert_fields, insert_values }, " ", false);

                // Execute SQL command
                using (var connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Execute(InsertUndlDataEOD, argLineToInsert);
                    }
                    catch {
                        // tbd
                    }
                }
            }

        }


        /// <summary>
        /// Insert data into a table. Data is passed as a list of DB lines. 
        /// The DB line type must (i) be a subclass of GenericDatabaseLine and (ii) match the given table columns.
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="argLineToInsert"></param>
        /*
        public void Insert<T>(IDtoken id, List<T> argLineToInsert) where T : GenericDatabaseLine
        {
            foreach (T singleLine in argLineToInsert)
            {
                Insert<T>(id, singleLine);
            }
        }


        */
        /*
        public void Insert<T>(IDtoken id, T argLineToInsert) where T : GenericDatabaseLine
        {
            // Identify table to be addressed
            string table = argLineToInsert.GetTable();

            // Build SQL command string
            //DateTime dt = (argLineToInsert as T).GetDate();


            string insert_table = "INSERT into " + table;
            string insert_fields = "(" + joinString(argLineToInsert.GetAllFields(), " ,", false) + ")";
            string insert_values = "values (" + joinStringWithDate(argLineToInsert.GetAllFields(), " ,", true) + ")";
            string InsertUndlDataEOD = joinString(new List<string>() { insert_table, insert_fields, insert_values }, " ", false);

            // Execute SQL command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(InsertUndlDataEOD, argLineToInsert);
            }

        }

        */

        #endregion


        // ************************************************************
        // METHODS -- UPDATE
        // ************************************************************

        #region

        /// <summary>
        /// Update data for a given set of table, ticker and date.
        /// Data is passed in the form of a DB line (subclass of GenericDatabaseLine).
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="argLineToUpdate"></param>
        /// <param name="updateFields"></param>
        public void Update(GenericDatabaseLine argLineToUpdate, List<string> updateFields)
        {

            // Identify table to be addressed
            string table = argLineToUpdate.GetTable();

            // Construct the command string
            string update_dataValues = buildUpdateStringData(table, argLineToUpdate.GetDataFields());
            string update_keys = buildUpdateStringKeys(argLineToUpdate.GetKeyFields());
            string updateString = joinString(new List<string>() { update_dataValues, update_keys }, " ", false);

            // Execute the command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(updateString, argLineToUpdate);
            }

        }


        /// <summary>
        /// Returns a string for an 'update' SQL command indicating what table and fields to update.
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="argFields"></param>
        /// <returns></returns>
        protected string buildUpdateStringData(string argTable, List<string> argFields)
        {
            string beginningD = "UPDATE " + argTable + " SET ";

            List<string> output = new List<string>();

            foreach (string key in argFields)
            {
                output.Add(key + " = @" + key);
            }

            string res = beginningD + joinString(output, ", ", false);
            return res;

        }


        /// <summary>
        /// Returns a string for an 'update' SQL command indicating what keys requirements (conditions) must be satisfied to update.
        /// </summary>
        /// <param name="argFields"></param>
        /// <returns></returns>
        protected string buildUpdateStringKeys(List<string> argFields)
        {
            string beginning = "WHERE ";

            List<string> output = new List<string>();

            foreach (string key in argFields)
            {
                output.Add(key + " = @" + key);
            }

            return beginning + joinString(output, " and ", false) + ";";

        }


        #endregion


        // ************************************************************
        // METHODS -- DELETE
        // ************************************************************

        #region Delete

        /// <summary>
        /// Delete an entire line from a table. The keys to identify the line are passed in a DB line (subclass of GenericDatabaseLine).
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="myLine"></param>
        public void EraseLine(GenericDatabaseLine myLine)
        {

            // Get string with table name
            string table = myLine.GetTable();

            //Check date
            DateTime myStartDate = (myLine.Date == DateTime.MinValue) ? myStartDate = new DateTime(2013, 12, 31) : myStartDate = myLine.Date;

            // Build string from start date
            string startDateForRequest = this.buildDateString(myStartDate);

            // Build mySQL Delete command string
            string mySQLDeleteCommand = joinString(new List<string>() { buildDeleteStringData(table), buildDeleteStringKeys(myLine.GetKeyFields()) }, " ", false);

            // Execute the command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(mySQLDeleteCommand, myLine);
            }
        }


        /// <summary>
        /// Delete an entire line from a table. The keys to identify the line are passed in a DB line (subclass of GenericDatabaseLine).
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="myLine"></param>
        public void EraseColumn(myDB argTable, string argField)
        {

            // Get string with table name
            string table = argTable.ToString();

            // Build mySQL Delete command string
            string mySQLDeleteCommand = "UPDATE " + table + " SET " + argField + " = null;";

            // Execute the command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(mySQLDeleteCommand);
            }
        }

        public void EraseTable(myDB argTable)
        {

            // Get string with table name
            string table = argTable.ToString();

            // Build mySQL Delete command string
            string mySQLDeleteCommand = "DELETE FROM " + table + ";";

            // Execute the command
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Execute(mySQLDeleteCommand);
            }
        }


        /// <summary>
        /// Returns a string for a 'delete' SQL command (implicitely for all fields/columns).
        /// </summary>
        /// <param name="argTable"></param>
        /// <param name="argFields"></param>
        /// <returns></returns>
        public string buildDeleteStringData(string argTable)
        {
            return "DELETE FROM " + argTable;

        }


        /// <summary>
        /// Returns a string for an 'delete' SQL command indicating what keys requirements (conditions) must be satisfied to update.
        /// </summary>
        /// <param name="argFields"></param>
        /// <returns></returns>
        public string buildDeleteStringKeys(List<string> argFields)
        {
            string beginning = "WHERE ";

            List<string> output = new List<string>();

            foreach (string key in argFields)
            {
                output.Add(key + " = @" + key);
            }

            string res = beginning + joinString(output, " and ", false) + ";";
            return res;

        }


        #endregion



    }
}
