using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{

    // ************************************************************
    // INVERSE GAMMA R.V.
    // ************************************************************
    class InverseGamma : Gamma
    {


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public InverseGamma() : this(2.0, 2.0) { }

        public InverseGamma(double shape, double scale)
            : base(shape, scale)
        { }

        #endregion




        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            return 1.0 / base.GetGamma(this.shape, 1.0 / this.scale);
        }

        #endregion


    }
}
