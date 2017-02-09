using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Markit
{
    public class MarkitForward : QLyx.Markit.Generic.MarkitTermStructure
    {


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        //public MarkitForward() { }


        // Constructor 2 : From given date
        public MarkitForward(DateTime pricingDate) : base(pricingDate: pricingDate) { }


        // Constructor 3 : For given date & content
        public MarkitForward(DateTime pricingDate, Dictionary<DateTime, Double> data) : base(pricingDate: pricingDate, data: data) { }

        #endregion




        
    }

}
