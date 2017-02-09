using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Utilities
{
    public static class QLNet_Date_extensionMethods
    {

        /// <summary>  
        ///  Returns a (new) corresponding DateTime from Date.
        /// </summary> 
        public static DateTime ToDateTime(this QLNet.Date myDate)
        {
            return new DateTime(myDate.year(), myDate.month(), myDate.Day);
        }


        /// <summary>  
        ///  Returns a (new) corresponding Date from DateTime.
        /// </summary> 
        public static QLNet.Date ToDate(this DateTime myDateTime)
        {
            return new QLNet.Date(myDateTime);
        }

    }
}
