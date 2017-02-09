using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{
    // ************************************************************
    // WEIBULL R.V.
    // ************************************************************
    class Weibull : Uniform
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // SHAPE
        protected double _shape;
        public double shape
        {
            get { return _shape; }
            protected set { _shape = value; }
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

        public Weibull() : this(1.0, 2.0) { }

        public Weibull(double shape, double scale)
            : base()
        {
            this.shape = shape;
            this.scale = scale;
        }

        #endregion



        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {

            if (this.shape <= 0.0 || this.scale <= 0.0)
            { throw new System.ArgumentException("Weibull_parameters", "Inconsistent shape and scale parameters in Beta distribution."); }

            return this.scale * Math.Pow(-Math.Log(base.GetUniform()), 1.0 / this.shape);

        }

        #endregion


    }
}
