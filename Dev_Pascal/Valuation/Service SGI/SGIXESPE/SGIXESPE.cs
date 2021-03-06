﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

using QLyx.DataIO;
using QLyx.DataIO.Markit;


namespace Pascal.Valuation
{

    public class SGIXESPE : EquityIndexStrangleStrategy
    {

        // ************************************************************
        // PROPERTIES 
        // ************************************************************

        // Identification
        private static DBID _callStrikeID() { return new DBID(68); } // SGPXESD1
        private static DBID _putStrikeID() { return 69; } // SGPXESD2  
        private static DBID _underlyingID() { return 61; } // SX5E

        private static Dictionary<int, DBID> _counterparty_strangleID() { return new Dictionary<int, DBID>()
        {
            { 0, new DBID(70)}, {1, new DBID(71)}, {2, new DBID(72)}, {3, new DBID(73)}, {4, new DBID(74)},
            {5, new DBID(75)}, {6, new DBID(76)}, {7, new DBID(77)}, {8, new DBID(78)}, {9, new DBID(79)}};
        }


        // ************************************************************
        // CONSTRUCTOR 
        // ************************************************************

        // No date
        public SGIXESPE()
            : base(new DateTime(), MarkitEquityUnderlying.Eurostoxx, numberStrangles: 10, spacing: new Period(1, TimeUnit.Days), 
                  calendar: new TARGET(), callStrikeDBID: _callStrikeID(), putStrikeDBID: _putStrikeID(), underlyingDBID: _underlyingID(),
                  strangleMtM_DBID: _counterparty_strangleID())
        { }

        // With date
        public SGIXESPE(DateTime valuationDate)
           : base(valuationDate, MarkitEquityUnderlying.Eurostoxx, numberStrangles: 10, spacing: new Period(1, TimeUnit.Days), 
                 calendar: new TARGET(), callStrikeDBID: _callStrikeID(), putStrikeDBID: _putStrikeID(), underlyingDBID: _underlyingID(),
                 strangleMtM_DBID: _counterparty_strangleID())
        { }



        // ************************************************************
        // OVERRIDDEN METHODS 
        // ************************************************************

        // tbd

        

    }
 }
