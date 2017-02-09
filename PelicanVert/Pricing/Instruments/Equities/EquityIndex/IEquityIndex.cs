using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;
using QLNet;

namespace QLyx.Equities
{


    public interface IEquityIndex
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************


        Calendar calendar { get; }

        IDtoken DBID { get; }

        Dictionary<string, Equity> indexComposition { get; }

    }

}

