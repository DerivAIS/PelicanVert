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
    public class myFrame : IEnumerable, IEnumerator
    {

       

        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************


        #region

        // DATA
        protected SortedDictionary<DateTime, myElement> _data;
        public SortedDictionary<DateTime, myElement> data
        {

            get
            {
                if (_data == null) { _data = new SortedDictionary<DateTime, myElement>(); }
                return _data;
            }

            protected set 
            { 
                _data = value; 
            }

        }


        // DATETIME OF FIRST DATA POINT
        public DateTime startDate
        {
            get { return data.Keys.FirstOrDefault(); }
        }


        // DATETIME OF LAST DATA POINT
        public DateTime endDate
        {
            get { return data.Keys.LastOrDefault(); }
        }


        // NUMBER OF TIME mySeries (number of elements)
        public int numberElements
        {
            get { return data.Count(); }
        }



        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        public myFrame() { }


        // Constructor 2 : Interest Rate Line
        public myFrame(List<InterestRate_Line> databaseLines)
        {

            // 1. Perform checks on number of series @TODO
            if (databaseLines.Count() != 0)
            {

                // 2. Extract fields from any DB line
                List<string> fieldsList = databaseLines.FirstOrDefault().GetDataFields();

                // 3. Fill this object
                foreach (InterestRate_Line line in databaseLines)
                {
                    foreach (string field in fieldsList)
                    {
                        data[line.Date] = new myElement(line.ToDict());
                    }
                }

            }
        }


        // Constructor 3 : Equity Line
        public myFrame(List<Equity_Line> databaseLines)
        {

            // 1. Perform checks on number of series @TODO
            if (databaseLines.Count() != 0)
            {

                // 2. Extract fields from any DB line
                List<string> fieldsList = databaseLines.FirstOrDefault().GetDataFields();

                // 3. Fill this object
                foreach (Equity_Line line in databaseLines)
                {
                    foreach (string field in fieldsList)
                    {
                        data[line.Date] = new myElement(line.ToDict());
                    }
                }

            }
        }


        // Constructor 4 : Equity Volatility
        public myFrame(List<EquityVolatility_Line> databaseLines)
        {

            // 1. Perform checks on number of series @TODO
            if (databaseLines.Count() != 0)
            {

                // 2. Extract fields from any DB line
                List<string> fieldsList = databaseLines.FirstOrDefault().GetDataFields();

                // 3. Fill this object
                foreach (EquityVolatility_Line line in databaseLines)
                {
                    foreach (string field in fieldsList)
                    {
                        data[line.Date] = new myElement(line.ToDict());
                    }
                }

            }
        }


        // Constructor 5 : Equity Volatility
        public myFrame(List<Bond_Line> databaseLines)
        {

            // 1. Perform checks on number of series @TODO
            if (databaseLines.Count() != 0)
            {

                // 2. Extract fields from any DB line
                List<string> fieldsList = databaseLines.FirstOrDefault().GetDataFields();

                // 3. Fill this object
                foreach (Bond_Line line in databaseLines)
                {
                    foreach (string field in fieldsList)
                    {
                        data[line.Date] = new myElement(line.ToDict());
                    }
                }

            }
        }


        #endregion





        // ************************************************************
        // COMMON METHODS 
        // ************************************************************



        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************

        #region

        public myElement this[DateTime i]
        {
            get { return data[i]; }
            protected set { data[i] = value; }
        }

        #endregion


        // ************************************************************
        // GENERIC METHODS 
        // ************************************************************

        public bool IsEmpty()
        { 
            return !(data.Count()>0);
        }


        // ************************************************************
        // OUTPUT METHODS 
        // ************************************************************


        /*
        public List<InterestRate_Line> ToList(DBID dbid) 
        {

            // Instanciate a new line
            InterestRate_Line myLine = new InterestRate_Line();

            // Identify table to be addressed
            List<string> fields = myLine.GetDataFields();

            // Output container
            List<InterestRate_Line> output = new List<InterestRate_Line>();

            // Loop around lines
            foreach(KeyValuePair<DateTime, myElement> item in data)
            {
                output.Add(new InterestRate_Line(item.Key, dbid, item.Value.data));
            }

            // Return
            return output;

        }
        */

        public List<T> ToList<T>(DBID dbid) where T : GenericDatabaseLine
        {

            // Determine type of <T> and create instance
            var type = typeof(T);
            T Tobj = (T)Activator.CreateInstance(type);

            // Identify table to be addressed
            List<string> fields = Tobj.GetDataFields();

            // Output container
            List<T> output = new List<T>();

            // Loop around lines
            foreach (KeyValuePair<DateTime, myElement> item in data)
            {
                T myLine = (T)Activator.CreateInstance(type, item.Key, dbid, item.Value.data);
                output.Add(myLine);
            }

            // Return
            return output;

        }



        // ************************************************************
        // CASTS
        // ************************************************************


        // Interest Rate
        public static explicit operator myFrame(InterestRate_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults.ToList());
            }
        }

        public static explicit operator myFrame(List<InterestRate_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults);
            }
        }


        // Equity
        public static explicit operator myFrame(Equity_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults.ToList());
            }
        }
        public static explicit operator myFrame(List<Equity_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults);
            }
        }


        // Equity Volatility
        public static explicit operator myFrame(EquityVolatility_Table rawResults)
        {

            if (rawResults.internalData.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults.ToList());
            }
        }
        public static explicit operator myFrame(List<EquityVolatility_Line> rawResults)
        {

            if (rawResults.Count() == 0)
            {
                // Create myFrame
                return new myFrame();
            }


            else
            {
                // Create myFrame
                return new myFrame(rawResults);
            }
        }



        // ************************************************************
        // UNION, MERGE, ADD, ETC.
        // ************************************************************


  


        public void Union(myFrame anotherFrame)
        {
            foreach (KeyValuePair<DateTime, myElement> kvp in anotherFrame.data)
            {
                if (!data.ContainsKey(kvp.Key)) { this.data.Add(kvp.Key, kvp.Value); }
            }

        }



        // ************************************************************
        // ENUMERATOR-RELATED METHODS
        // ************************************************************

        #region Enumerators


        // ENUMERATOR PROPERTY
        private int Position = -1;


        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }



        public bool MoveNext()
        {

            if (Position < data.Count - 1)
            {
                ++Position;
                return true;
            }
            return false;
        }



        public void Reset()
        {
            Position = -1;
        }



        public object Current
        {
            get
            {
                DateTime key = data.Keys.ElementAt(Position);
                return key;
            }
        }



        #endregion



    }
}



