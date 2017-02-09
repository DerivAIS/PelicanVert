using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{

    // ************************************************************
    // EXPONENTIAL R.V.
    // ************************************************************
    class Exponential : Uniform
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // MEAN
        protected double _mean;
        public double mean
        {
            get { return _mean; }
            protected set { _mean = value; }
        }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Exponential() : this(1.0) { }

        public Exponential(double mean)
            : base()
        {
            this.mean = mean;
        }

        #endregion


        // ************************************************************
        // EXPONENTIAL RANDOM NUMBER GENERATORS
        // ************************************************************

        #region

        // EMPTY

        #endregion


        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            double mySample = base.next();
            return -Math.Log(mySample) * this.mean;

        }

        #endregion


    }
}
