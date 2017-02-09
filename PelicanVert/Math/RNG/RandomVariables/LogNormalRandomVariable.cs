using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{
    // ************************************************************
    // LOG-NORMAL R.V.
    // ************************************************************
    class LogNormal : Normal
    {


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public LogNormal() : this(0.0, 1.0) { }

        public LogNormal(double mean, double standardDeviation)
            : base(mean, standardDeviation) { }

        #endregion



        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public new double next()
        {
            double mySample = base.next();
            return Math.Exp(mySample);

        }

        #endregion


    }
}
