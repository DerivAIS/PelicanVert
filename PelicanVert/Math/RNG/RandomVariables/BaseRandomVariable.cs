using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.RandomVariables
{


    // ************************************************************
    // GENERIC R.V. CLASS
    // ************************************************************
    abstract class RandomVariable
    {

        // ************************************************************
        // STATIC PROPERTIES
        // ************************************************************
        #region

        protected uint m_w;

        protected uint m_z;

        #endregion



        // ************************************************************
        // SEED METHODS
        // ************************************************************

        #region

        public void SetSeed(uint u, uint v)
        {
            if (u != 0) this.m_w = u;
            if (v != 0) this.m_z = v;
        }


        public void SetSeed(uint u)
        {
            this.m_w = u;
        }


        public void SetSeedFromSystemTime()
        {
            System.DateTime dt = System.DateTime.Now;
            long x = dt.ToFileTime();
            this.SetSeed((uint)(x >> 16), (uint)(x % 4294967296));
        }

        #endregion



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************
        #region

        public RandomVariable()
        {
            this.m_w = 521288629;
            this.m_z = 362436069;
        }

        #endregion



    }
}
