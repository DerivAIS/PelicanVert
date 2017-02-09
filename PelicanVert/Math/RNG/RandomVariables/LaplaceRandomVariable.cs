using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{
    // ************************************************************
    // LAPLACE R.V.
    // ************************************************************
    class Laplace : Uniform
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

        public Laplace() : this(0.0, 1.0) { }

        public Laplace(double mean, double scale)
            : base()
        {
            this.mean = mean;
            this.scale = scale;
        }

        #endregion




        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            double u = base.next();
            return (u < 0.5) ?
                this.mean + this.scale * Math.Log(2.0 * u) :
                this.mean - this.scale * Math.Log(2 * (1 - u));

        }

        #endregion








    }

}
