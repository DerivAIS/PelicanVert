using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLyx.DataIO;

namespace QLyx.Equities
{

    interface IEquity
    {


        // ************************************************************
        // PROPERTIES
        // ************************************************************

        string currencyName { get; }

        string exchange { get; }

        string calendar { get; }

        IDtoken DBID { get; }



    }



}

