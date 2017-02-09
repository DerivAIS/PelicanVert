using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // GAMMA R.V.
    // ************************************************************
    class Gamma : RandomVariable
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
            protected set
            {
                if (value <= 0.0) { throw new System.ArgumentException("GammaShape", "Invalid shape parameter for gamma distribution."); }
                _shape = value;
            }
        }


        // SCALE
        protected double _scale;
        public double scale
        {
            get { return _scale; }
            protected set
            {
                if (value <= 0.0) { throw new System.ArgumentException("GammaScale", "Invalid scale parameter for gamma distribution."); }
                _scale = value;
            }
        }

        #endregion



        // ************************************************************
        // SUPPORTING RANDOM VARIABLES
        // ************************************************************
        #region

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


        // NORMAL RANDOM VARIABLE
        protected Normal _normalRV;
        public Normal normalRV
        {
            get
            {
                if (this._normalRV == null) { this._normalRV = new Normal(0.0, 1.0); }
                return _normalRV;
            }

            protected set
            {
                _normalRV = value;
            }
        }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Gamma() : this(2.0, 2.0) { }

        public Gamma(double shape, double scale)
        {
            this.shape = shape;
            this.scale = scale;
        }

        #endregion



        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        protected double GetGamma(double shape, double scale)
        {

            // Implementation based on "A Simple Method for Generating Gamma Variables"
            // by George Marsaglia and Wai Wan Tsang.  ACM Transactions on Mathematical Software
            // Vol 26, No 3, September 2000, pages 363-372.

            double d, c, x, xsquared, v, u;

            if (shape >= 1.0)
            {
                d = shape - 1.0 / 3.0;
                c = 1.0 / Math.Sqrt(9.0 * d);
                for (; ; )
                {
                    do
                    {
                        x = this.normalRV.GetNormal();
                        v = 1.0 + c * x;
                    }
                    while (v <= 0.0);
                    v = v * v * v;
                    u = this.uniformRV.GetUniform();
                    xsquared = x * x;
                    if (u < 1.0 - .0331 * xsquared * xsquared || Math.Log(u) < 0.5 * xsquared + d * (1.0 - v + Math.Log(v)))
                        return scale * d * v;
                }
            }

            else
            {
                double g = this.GetGamma(shape + 1.0, 1.0);
                double w = this.uniformRV.GetUniform();
                return scale * g * Math.Pow(w, 1.0 / shape);
            }
        }

        public double next()
        {
            return this.GetGamma(this.shape, this.scale);
        }

        #endregion


    }
}
