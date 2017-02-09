using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.Containers;

namespace QLyx.Equities
{
    public class DividendCurve : mySchedule
    {




        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        #region Constructors


        // Constructor 1 : Generic
        public DividendCurve() : base() { }


        // Constructor 2 : Specified by start and end dates
        public DividendCurve(DateTime startDate, DateTime endDate) : base(argStartDate: startDate, argEndDate: endDate) { }


        // Constructor 3 : Specified all cashflows
        public DividendCurve(Dictionary<DateTime, double> argCashflows) : base(argCashflows: argCashflows) { }



        #endregion







    }
}

