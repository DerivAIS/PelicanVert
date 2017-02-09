using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace QLyx.DataIO
{

    // DATABASE IDENTIFICATION
    public class DBID
    {

        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors

        private System.Int32 _DBID = 0;

        /// <summary>
        /// Constructor must include the underlying unique integer.
        /// </summary>
        /// <param name="argDBID"></param>
        public DBID(int argDBID)
        {
            _DBID = argDBID;
        }

        #endregion



        // ************************************************************
        // METHODS
        // ************************************************************

        #region Methods

        /// <summary>
        /// Return the int corresponding to the DBID.
        /// </summary>
        /// <returns></returns>
        public System.Int32 ToInt()
        {
            return _DBID;
        }


        #endregion




        // ************************************************************
        // CASTS
        // ************************************************************

        #region Casts

        /// <summary>
        /// Provides explicit cast of a DBID into an integer.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static implicit operator System.Int32(DBID x)
        {
            return x._DBID;
        }


        /// <summary>
        /// Provides explicit cast of a DBID into an integer.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static implicit operator DBID(System.Int32 x)
        {
            return new DBID(x);
        }


        #endregion

    }
}