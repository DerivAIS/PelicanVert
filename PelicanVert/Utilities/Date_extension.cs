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

        /// <summary>  
        ///  Returns a (new) list of corresponding Date from DateTime.
        /// </summary> 
        public static List<QLNet.Date> ToDateList(this List<DateTime> myDateTimeList)
        {
            List<QLNet.Date> res = new List<QLNet.Date>(); 
            foreach (DateTime dt in myDateTimeList) {
                res.Add(new QLNet.Date(dt));
            }
            return res;
        }

        /// <summary>  
        ///  Returns a (new) list of corresponding DateTime from Date.
        /// </summary> 
        public static List<DateTime> ToDateTimeList(this List<DateTime> myDateList)
        {
            List<DateTime> res = new List<DateTime>();
            foreach (QLNet.Date myDate in myDateList)
            {
                res.Add(new DateTime(myDate.year(), myDate.month(), myDate.Day));
            }
            return res;
        }

    }
}
