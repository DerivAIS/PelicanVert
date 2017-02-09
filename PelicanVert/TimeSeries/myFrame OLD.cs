using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;

namespace QLyx.Containers
{
    public class myFrame_old
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        /*
        public static DatabaseLine

        // Extract fields from any DB line
                List<string> fieldsList = rawResults.internalData.FirstOrDefault().Value.GetDataFields();

                // Init
                Dictionary<string, mySeries> seriesDict = new Dictionary<string, mySeries>();

                // Initialize the dict
                foreach (string field in fieldsList)
                {
                    seriesDict[field] = new mySeries();
                }


                // Fill the dict
                foreach (GenericDatabaseLine line in rawResults)
                {
                    foreach (string field in fieldsList) 
                    {
                        seriesDict[field].pushback(line.Date, (double)line[field]);
                    }
                }


        */


        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        #region

        // DATA
        protected SortedDictionary<string, mySeries> _data;
        public SortedDictionary<string, mySeries> data
        {
            get
            {
                if (_data == null) { _data = new SortedDictionary<string, mySeries>(); }
                return _data;
            }

            protected set { _data = value; }
        }


        // DATETIME OF FIRST DATA POINT
        protected DateTime _startDate;
        public DateTime startDate
        {
            get { return _startDate; }
            protected set { _startDate = value; }
        }


        // DATETIME OF LAST DATA POINT
        protected DateTime _endDate;
        public DateTime endDate
        {
            get { return _endDate; }
            protected set { _endDate = value; }
        }


        // NUMBER OF TIME mySeries (number of elements)
        protected int _nbmySeries = 0;
        public int nbmySeries
        {
            get { return _nbmySeries; }
            protected set { _nbmySeries = value; }
        }

        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myFrame_old() { }

            
        // Constructor 2 : Interest Rate Line
        public myFrame_old(List<InterestRate_Line> databaseLine)
        {

            // 1. Perform checks on number of series @TODO
            if (databaseLine.Count() != 0)
            {

            // 2. Extract fields from any DB line
            List<string> fieldsList = databaseLine.FirstOrDefault().GetDataFields();

            // 3. Initialize the dict
            foreach (string field in fieldsList)
            {
                data[field] = new mySeries();
            }


            // 4. Fill the dict
            foreach (InterestRate_Line line in databaseLine)
            {
                foreach (string field in fieldsList)
                {
                    data[field].pushback(line.Date, (double)line[field]);
                }
            }

            }
        }


        // Constructor 3 : Dict of Series
        public myFrame_old(List<Equity_Line> databaseLine)
        {

            // 1. Perform checks on number of series @TODO


            // 2. Extract fields from any DB line
            List<string> fieldsList = databaseLine.FirstOrDefault().GetDataFields();

            // 3. Initialize the dict
            foreach (string field in fieldsList)
            {
                data[field] = new mySeries();
            }


            // 4. Fill the dict
            foreach (Equity_Line line in databaseLine)
            {
                foreach (string field in fieldsList)
                {
                    data[field].pushback(line.Date, (double)line[field]);
                }
            }
        }


        // Constructor 3 : Dict of Series
        public myFrame_old(List<EquityVolatility_Line> databaseLine)
        {

            // 1. Perform checks on number of series @TODO


            // 2. Extract fields from any DB line
            List<string> fieldsList = databaseLine.FirstOrDefault().GetDataFields();

            // 3. Initialize the dict
            foreach (string field in fieldsList)
            {
                data[field] = new mySeries();
            }


            // 4. Fill the dict
            foreach (EquityVolatility_Line line in databaseLine)
            {
                foreach (string field in fieldsList)
                {
                    data[field].pushback(line.Date, (double)line[field]);
                }
            }
        }



        #endregion





        // ************************************************************
        // COMMON METHODS 
        // ************************************************************

        #region


        public bool IsEmpty()
        {
            if (!(nbmySeries == 0)) { return false; }
            else { return true; }
        }

        // ADD FRAME TO FRAME
        public void add(string argmySeriesName, mySeries argmySeries)
        {

            // Perform sanity checks
            // @TODO : to be implemented

            // Add to frame
            this.data[argmySeriesName] = argmySeries;

        }


        // REMOVE FRAME FROM FRAME
        public void remove(string argmySeriesName)
        {
            if (this.data.ContainsKey(argmySeriesName))
            {
                this.data.Remove(argmySeriesName);
            }

        }


        // REPLACE mySeries WITHIN EXISTING FRAME
        public void replace(string argmySeriesName, mySeries argmySeries)
        {

            if (this.data.ContainsKey(argmySeriesName))
            {

                this.remove(argmySeriesName);
                this.add(argmySeriesName, argmySeries);
            }

        }


        // POP FRAME FROM FRAME
        public mySeries pop(string argmySeriesName)
        {
            mySeries ans = new mySeries();

            if (this.data.ContainsKey(argmySeriesName))
            {
                ans = this.data[argmySeriesName];
                this.remove(argmySeriesName);
            }

            return ans;

        }


        // SWAP FRAME WITHIN FRAME
        public mySeries swap(string argmySeriesName, mySeries argmySeries)
        {
            mySeries ans = new mySeries();

            if (this.data.ContainsKey(argmySeriesName))
            {
                ans = this.pop(argmySeriesName);
                this.add(argmySeriesName, argmySeries);
            }

            return ans;

        }


        // COUNT
        public int count()
        {
            return this.data.Count();
        }

        #endregion



        // ************************************************************
        // CSV-RELATED METHODS 
        // ************************************************************

        #region


        // LOAD CSV 
        public void loadCSV(string argFilename)
        {

            // Construct file name with path
            string filePath = @"C:\Users\plemieux080513\CSV\";
            string fileName = argFilename + ".csv";
            StreamReader sr = new StreamReader(filePath + fileName);


            // Declaring temporary variable(s)
            double _temp;
            SortedDictionary<String, SortedDictionary<DateTime, double>> myData = new SortedDictionary<String, SortedDictionary<DateTime, double>>();
            string[] Line;


            // Parsing headers
            string[] Headers = sr.ReadLine().Split(',');
            //sr.ReadLine();
            SortedDictionary<int, string> myHeaders = this.parseCSVheaders(Headers);
            int nbHeaders = myHeaders.Count();


            // Initialize data container
            for (int i = 1; i <= nbHeaders; i++)
            { myData.Add(Headers[i], new SortedDictionary<DateTime, double>()); }


            // Process data from stream
            while (!sr.EndOfStream)
            {
                // Get a line from CSV stream
                Line = sr.ReadLine().Split(',');

                // Parse the date into a datetime 
                DateTime current_date = Convert.ToDateTime(Line[0]);

                // parse the data
                for (int j = 1; j <= nbHeaders; j++)
                {
                    _temp = Convert.ToDouble(Line[j]);
                    myData[Headers[j]].Add(current_date, _temp);
                }

            }

            // Close file
            sr.Close();

            // Load into current object
            this.fromDictOfDict(myData);

        }


        // PARSE THE HEADER OF THE CSV FILE
        public SortedDictionary<int, string> parseCSVheaders(string[] argHeaders)
        {
            int nbHeaders = argHeaders.GetLength(0);
            SortedDictionary<int, string> headersDict = new SortedDictionary<int, string>();
            for (int i = 1; i < nbHeaders; i++)
            {
                headersDict[i] = argHeaders[i];
            }

            return headersDict;

        }


        #endregion



        // ************************************************************
        // LOAD-RELATED METHODS 
        // ************************************************************

        #region

        // LOAD THE OBJECT FROM A DICT OF DICT
        public void fromDictOfDict(SortedDictionary<string, SortedDictionary<DateTime, double>> argDictOfDict)
        {

            foreach (string mySeriesName in argDictOfDict.Keys)
            {
                mySeries mymySeries = new mySeries(argDictOfDict[mySeriesName]);
                this.add(mySeriesName, mymySeries);
            }


        }

        #endregion



        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public mySeries this[string i]
        {
            get { return this.data[i]; }
            protected set { this.data[i] = value; }
        }

        #endregion




        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************







        // ************************************************************
        // CASTS
        // ************************************************************


        // Interest Rate
        public static explicit operator myFrame_old(InterestRate_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults.ToList());
            }
        }
        public static explicit operator myFrame_old(List<InterestRate_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults);
            }
        }


        // Equity
        public static explicit operator myFrame_old(Equity_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults.ToList());
            }
        }
        public static explicit operator myFrame_old(List<Equity_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults);
            }
        }


        // Equity Volatility
        public static explicit operator myFrame_old(EquityVolatility_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults.ToList());
            }
        }
        public static explicit operator myFrame_old(List<EquityVolatility_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame_old
                return new myFrame_old();
            }


            else
            {
                // Create myFrame_old
                return new myFrame_old(rawResults);
            }
        }






    }
}



