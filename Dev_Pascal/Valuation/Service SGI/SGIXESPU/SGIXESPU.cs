using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

using QLyx.DataIO;
using QLyx.DataIO.Markit;


namespace Pascal.Valuation
{

    public class SGIXESPU : EquityIndexStrangleStrategy
    {

        // ************************************************************
        // PROPERTIES 
        // ************************************************************

        // Identification
        private static DBID _callStrikeID() { return new DBID(100); } // SGPXUSD1
        private static DBID _putStrikeID() { return 101; } // SGPXUSD2  
        private static DBID _underlyingID() { return 112; } // SPX

        private static Dictionary<int, DBID> _counterparty_strangleID()
        {
            return new Dictionary<int, DBID>() {
                {0, new DBID(102)}, {1, new DBID(103)}, {2, new DBID(104)}, {3, new DBID(105)}, {4, new DBID(106)},
                {5, new DBID(107)}, {6, new DBID(108)}, {7, new DBID(109)}, {8, new DBID(110)}, {9, new DBID(111)}
            };
        }


        // ************************************************************
        // CONSTRUCTOR 
        // ************************************************************

        // No date
        public SGIXESPU()
            : base(new DateTime(), MarkitEquityUnderlying.SP_500, numberStrangles: 10, spacing: new Period(1, TimeUnit.Days),
                  calendar: new TARGET(), callStrikeDBID: _callStrikeID(), putStrikeDBID: _putStrikeID(), underlyingDBID: _underlyingID(),
                  strangleMtM_DBID: _counterparty_strangleID())
        { }

        // With date
        public SGIXESPU(DateTime valuationDate)
           : base(valuationDate, MarkitEquityUnderlying.SP_500, numberStrangles: 10, spacing: new Period(1, TimeUnit.Days),
                 calendar: new TARGET(), callStrikeDBID: _callStrikeID(), putStrikeDBID: _putStrikeID(), underlyingDBID: _underlyingID(),
                 strangleMtM_DBID: _counterparty_strangleID())
        { }



        // ************************************************************
        // OVERRIDDEN METHODS 
        // ************************************************************

        // tbd



    }
}
