using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // CHI SQUARE R.V.
    // ************************************************************
    class ChiSquare : Gamma
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // DEGREES OF FREEDOM
        protected double _degreesFreedom;
        public double degreesFreedom
        {
            get { return _degreesFreedom; }
            protected set { _degreesFreedom = value; }
        }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public ChiSquare() : this(1.0) { }

        public ChiSquare(double degreesFreedom)
            : base()
        {
            this.degreesFreedom = degreesFreedom;
        }

        #endregion




        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            return this.GetChiSquare(this.degreesFreedom);
        }


        public double GetChiSquare(double degreesFreedom)
        {
            return base.GetGamma(0.5 * degreesFreedom, 2.0);
        }

        #endregion


    }
}
