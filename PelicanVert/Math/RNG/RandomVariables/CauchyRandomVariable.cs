using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // CAUCHY R.V.
    // ************************************************************

    class Cauchy : Uniform
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // MEDIAN
        protected double _median;
        public double median
        {
            get { return _median; }
            protected set { _median = value; }
        }


        // SCALE
        protected double _scale;
        public double scale
        {
            get { return _scale; }
            protected set { _scale = value; }
        }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Cauchy() : this(1.0, 2.0) { }

        public Cauchy(double median, double scale)
            : base()
        {
            this.median = median;
            this.scale = scale;
        }

        #endregion



        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            if (this.scale <= 0)
            { throw new System.ArgumentException("Cauchy", "Inconsistent scale parameter in Cauchy distribution."); }


            // Apply inverse of the Cauchy distribution function to a uniform
            return this.median + this.scale * Math.Tan(Math.PI * (base.GetUniform() - 0.5));
        }

        #endregion


    }
}
