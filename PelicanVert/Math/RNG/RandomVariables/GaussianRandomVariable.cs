using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{

    // ************************************************************
    // NORMAL R.V.
    // ************************************************************
    class Normal : RandomVariable
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


        // STANDARD DEVIATION
        protected double _standardDeviation;
        public double standardDeviation
        {
            get { return _standardDeviation; }
            protected set
            {
                if (value <= 0.0) { throw new System.ArgumentException("Normal_RV", "Invalid standard deviation."); }
                _standardDeviation = value;
            }
        }


        // UNIFORM RANDOM VARIABLE
        protected Uniform _uniformRV;
        public Uniform uniformRV
        {
            get
            {
                if (this._uniformRV == null) { this._uniformRV = new Uniform(0.0, 1.0); }
                return _uniformRV;
            }

            protected set
            {
                _uniformRV = value;
            }
        }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Normal() : this(0.0, 1.0) { }

        public Normal(double mean, double standardDeviation)
            : base()
        {
            this.mean = mean;
            this.standardDeviation = standardDeviation;
        }


        #endregion


        // ************************************************************
        // NORMAL RANDOM NUMBER GENERATORS
        // ************************************************************

        #region


        // NORMAL RV 
        public double GetNormal()
        {
            // Box-Muller algorithm
            double u1 = this.uniformRV.GetUniform();
            double u2 = this.uniformRV.GetUniform();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = 2.0 * Math.PI * u2;
            return this.mean + this.standardDeviation * (r * Math.Sin(theta));
        }


        #endregion


        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public double next()
        {
            return this.GetNormal();
        }

        #endregion


    }

}
