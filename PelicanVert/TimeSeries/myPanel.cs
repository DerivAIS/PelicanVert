using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Containers
{
    public class myPanel
    {

        // ************************************************************
        // CLASS PROPERTIES 
        // ************************************************************

        #region

        // DATA
        protected Dictionary<string, myFrame> _data;
        public Dictionary<string, myFrame> data
        {
            get { return _data; }
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


        // NUMBER OF myFrameS
        protected int _nbmyFrames;
        public int nbmyFrames
        {
            get { return _nbmyFrames; }
            protected set { _nbmyFrames = value; }
        }



        #endregion


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        // Constructor 1 : Generic
        public myPanel() { }



        // ************************************************************
        // COMMON METHODS 
        // ************************************************************


        // ADD myFrame TO PANEL
        public void add(string argmyFrameName, myFrame argmyFrame)
        {

            // Perform sanity checks
            // @TODO : to be implemented
            if (this.data.ContainsKey(argmyFrameName)) { throw new System.ArgumentException("myFrame already exists in Panel.", "ExistingmyFrame"); }

            // Add to panel
            this.data[argmyFrameName] = argmyFrame;


        }


        // REMOVE myFrame FROM PANEL
        public void remove(string argmyFrameName)
        {
            if (this.data.ContainsKey(argmyFrameName))
            {
                this.data.Remove(argmyFrameName);
            }

        }


        // REPLACE myFrame WITHIN EXISTING PANEL
        public void replace(string argmyFrameName, myFrame argmyFrame)
        {

            if (this.data.ContainsKey(argmyFrameName))
            {

                this.remove(argmyFrameName);
                this.add(argmyFrameName, argmyFrame);
            }

        }


        // POP myFrame FROM PANEL
        public myFrame pop(string argmyFrameName)
        {
            myFrame ans = new myFrame();

            if (this.data.ContainsKey(argmyFrameName))
            {
                ans = this.data[argmyFrameName];
                this.remove(argmyFrameName);
            }

            return ans;

        }


        // SWAP myFrame WITHIN PANEL
        public myFrame swap(string argmyFrameName, myFrame argmyFrame)
        {
            myFrame ans = new myFrame();

            if (this.data.ContainsKey(argmyFrameName))
            {
                ans = this.pop(argmyFrameName);
                this.add(argmyFrameName, argmyFrame);
            }

            return ans;

        }


        // COUNT
        public int count()
        {
            return this.data.Count();
        }


        // ************************************************************
        // INDEXER METHODS 
        // ************************************************************




    }
}



