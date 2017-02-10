using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

using QLyx.Equities;



namespace Pascal.Valuation
{
    public class SGIXESPE : EquityIndexStrangleStrategy<Eurostoxx50>
    {

        // ************************************************************
        // PROPERTIES 
        // ************************************************************

        // tbd



        // ************************************************************
        // CONSTRUCTOR 
        // ************************************************************

        // Generic
        public SGIXESPE() : base() { }

        // Generic
        public SGIXESPE(int numberStrangles, Period spacing, Calendar calendar)
            : this(new DateTime(), numberStrangles, spacing, calendar)
        { }

        public SGIXESPE(DateTime valuationDate, int numberStrangles, Period spacing, Calendar calendar)
           : base(valuationDate, numberStrangles, spacing, calendar)
        {
            
        }




        // ************************************************************
        // OVERRIDDEN METHODS 
        // ************************************************************

        // tbd


    }



    }
