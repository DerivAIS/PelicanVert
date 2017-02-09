using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // UNIFORM R.V.
    // ************************************************************
    class Uniform : RandomVariable
    {


        // ************************************************************
        // INSTANCE PROPERTIES
        // ************************************************************

        #region

        // LOWER LIMIT (EXCLUDED)
        protected double _lowerLimit;
        public double lowerLimit
        {
            get { return _lowerLimit; }
            protected set { _lowerLimit = value; }
        }


        // UPPER LIMIT (EXCLUDED)
        protected double _upperLimit;
        public double upperLimit
        {
            get { return _upperLimit; }
            protected set { _upperLimit = value; }
        }


        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region

        public Uniform() : this(0.0, 1.0) { }

        public Uniform(double lowerLimit, double upperLimit)
            : base()
        {
            this.lowerLimit = lowerLimit;
            this.upperLimit = upperLimit;
        }

        #endregion


        // ************************************************************
        // UNIFORM RANDOM NUMBER GENERATORS
        // ************************************************************

        #region

        public double GetUniform()
        {
            // 0 <= u < 2^32
            uint u = GetUint();
            // The magic number below is 1/(2^32 + 2).
            // The result is strictly between 0 and 1.
            return (u + 1.0) * 2.328306435454494e-10;
        }


        public double GetUniform(double lowerLimit, double upperLimit)
        {
            if (upperLimit <= lowerLimit) { throw new System.ArgumentException("GetUniform_limits", "Invalid upper and lower limits."); }
            return GetUniform() * (upperLimit - lowerLimit) + lowerLimit;
        }


        protected uint GetUint()
        {
            this.m_z = 36969 * (this.m_z & 65535) + (this.m_z >> 16);
            this.m_w = 18000 * (this.m_w & 65535) + (this.m_w >> 16);
            return (this.m_z << 16) + this.m_w;
        }


        #endregion


        // ************************************************************
        // METHOD
        // ************************************************************

        #region

        public double next()
        {
            return this.GetUniform(this.lowerLimit, this.upperLimit);
        }

        #endregion


    }
}