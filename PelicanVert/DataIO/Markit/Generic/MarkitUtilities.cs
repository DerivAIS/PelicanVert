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
        Eurostoxx = 61,

        // EuroStoxx50 Total Return
        Eurostoxx_TR = 62,

        // S&P500 Excess Return
        SP_500 = 112,

        // S&P500 Total Return = SPXT INDEX
        SP_500_TR = 134,

        // CAC40 = CAC INDEX
        CAC40 = 135,

        // EPRA NAREIT = EPRA INDEX
        EPRA = 140,

        // FTSE 100 = UKX INDEX
        FTSE_100 = 136,

        // SWISS SMI = SMI INDEX
        SMI = 137,

        // TOPIX = TPX INDEX
        TOPIX = 138,

        // TOPIX = TPX INDEX
        NIKKEI_225 = 139

    }
}
