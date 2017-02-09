using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // STUDENT R.V.
    // ************************************************************
    class Student : RandomVariable
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
            protected set
            {
                if (value <= 0.0) { throw new System.ArgumentException("DegreesFreedom", "Invalid degrees of freedom for Student distribution."); }
                _degreesFreedom = value;
            }
        }


        #endregion



        // ************************************************************
        // SUPPORTING RANDOM VARIABLES
        // ************************************************************
        #region

        // CHI SQUARE RANDOM VARIABLE
        protected ChiSquare _chiSquareRV;
        public ChiSquare chiSquareRV
        {
            get
            {
                if (this._chiSquareRV == null) { this._chiSquareRV = new ChiSquare(1.0); }
                return _chiSquareRV;
            }

            protected set
            {
                _chiSquareRV = value;
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

        public Student() : this(2.0) { }

        public Student(double degreesFreedom)
        {
            this.degreesFreedom = degreesFreedom;
        }

        #endregion



        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public double next()
        {
            // See Seminumerical Algorithms by Knuth
            double y1 = this.normalRV.GetNormal();
            double y2 = this.chiSquareRV.GetChiSquare(this.degreesFreedom);
            return y1 / Math.Sqrt(y2 / this.degreesFreedom);
        }


        #endregion


    }
}
