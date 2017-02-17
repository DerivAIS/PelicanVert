using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO
{


    /// <summary>
    /// Returns an enum identifying a table physically present in the 'myDB' schema.
    /// </summary>
    public enum myDB
    {

        // Equities
        Equity,
        EquityVolatility,


        // Interest Rates
        InterestRate,
        Bond,


        // Reference
        Reference

    };




    /// <summary>
    /// Returns an enum identifying a type of data to be returned
    /// </summary>
    public enum RequestDataType
    {

        Historical,
        Fundamantal


    };



    /// <summary>
    /// Returns an enum identifying a table physically present in the 'myDB' schema.
    /// </summary>
    public enum ExternalSources
    {

        Bloomberg,
        Reuters,
        Sophis,
        MSD,
        MarketMap,
        Markit,
        FRED,            
        Yahoo,
        Google,
        ISIN

    };



}
