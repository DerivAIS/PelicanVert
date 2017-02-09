using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.DataIO.Markit
{
    public enum MarkitEquityUnderlying
    {

        // EuroStoxx50 Excess Return
        Eurostoxx = 0,

        // EuroStoxx50 Total Return
        Eurostoxx_TR = 1,

        // S&P500 Excess Return
        SP_500 = 2,

        // S&P500 Total Return = SPXT INDEX
        SP_500_TR = 3,

        // CAC40 = CAC INDEX
        CAC40 = 4,

        // S&P500 Total Return = SPXT INDEX
        EPRA = 5,

        // FTSE 100 = UKX INDEX
        FTSE_100 = 6,

        // SWISS SMI = SMI INDEX
        SMI = 7,

        // TOPIX = TPX INDEX
        TOPIX = 8,

        // TOPIX = TPX INDEX
        NIKKEI_225 = 9

    }
}
