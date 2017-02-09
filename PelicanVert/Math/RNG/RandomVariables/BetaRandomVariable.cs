using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{

    // ************************************************************
    // BETA R.V.        
    // ************************************************************
    class Beta : Gamma
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // ALPHA
        protected double _alpha;
        public double alpha
        {
            get { return _alpha; }
            protected set { _alpha = value; }
        }


        // BETA
        protected double _beta;
        public double beta
        {
            get { return _beta; }
            protected set
            {
                _beta = value;
            }
        }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Beta() : this(2.0, 5.0) { }

        public Beta(double alpha, double beta)
            : base(alpha, beta)
        { }

        #endregion


        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {

            if (this.alpha <= 0.0 || this.beta <= 0.0)
            { throw new System.ArgumentException("Beta_parameters", "Inconsistent alpha and beta parameters in Beta distribution."); }

            double u = GetGamma(this.alpha, 1.0);
            double v = GetGamma(this.beta, 1.0);
            return u / (u + v);

        }

        #endregion


    }
}
