using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace Pascal.Pricing.Instruments
{

    public class Kernel_FundedPDI : GenericInstrument
    {

        public Kernel_FundedPDI(List<Date> observationDates, double strikeMoneyness, double barrierMoneyness, double strikeLevel, 
            double leverageDown, double FixDiv_points = 0.0, double CashYield = 0.0)

                : base(BuildDico("Observation_Dates", observationDates),
                       BuildDico("Strike_Moneyness", strikeMoneyness),
                       BuildDico("Barrier_Moneyness", barrierMoneyness),
                       BuildDico("Strike_Level", strikeLevel),
                       BuildDico("Leverage_Down", leverageDown),
                       BuildDico("Fixed_Dividend", FixDiv_points),
                       BuildDico("Cash_yield", CashYield))
        { }


        public override double ScriptDico(Dictionary<string, List<double>> timeDico,
                                     Dictionary<string, List<double>> indexDico,
                                     Handle<YieldTermStructure> discountTS,
                                     Path path)
        {

            #region old variables declarations
            // double fixingValue = 0.0;
            // double payoff = 0.0;
            // bool isCliquet = false;
            // double cliquetMoney = indexDico["Cliquet_Moneyness"][0];
            // double couponRate = indexDico["Coupons"][0];
            #endregion

            
            // Strike definition
            double strike = indexDico["Strike_Level"][0];
            double strikeMoneyness = indexDico["Strike_Moneyness"][0];

            // Option Caracteristics
            double PDI_barrier = indexDico["Barrier_Moneyness"][0];
            double leverage = indexDico["Leverage_Down"][0];

            // Index Path Caracteristics
            double DivFix = indexDico["Fixed_Dividend"][0];
            double cashYield = indexDico["Cash_yield"][0];

            // Temp Variables Declaration
            double discount = 1.0;
            double yield = 0.0;

            // Current Spot Level
            double IL = path.value(0);
            double previous_IL = path.value(0);
            double previous_UIL = path.value(0);
            double previous_Time = 0.0;
            double dT = 0.0;
            int i = 0;

            // Loop through all dates
            for (int t = 1; t < path.length(); t++)
            {

                // Compute the path based for fixed div index
                dT = path.time(t) - previous_Time;
                IL = Math.Max((path.value(t) / previous_UIL) * previous_IL * (1 + dT * cashYield) - DivFix * dT, 0.0);

                // Update
                previous_Time = path.time(t);
                previous_UIL = path.value(t);
                previous_IL = IL;

                // Display on Observation Dates
                #region 

                if (Math.Round(path.time(t), 4) == Math.Round(timeDico["Observation_Dates"][i], 4))
                {
                    yield = IL / (strike * strikeMoneyness);
                    INSPOUT("Yield_" + i.ToString(), yield);
                    i++;
                }
               
                #endregion
            }

            // Yield computation
            yield = IL / (strike * strikeMoneyness);

            // Display
            INSPOUT("Yield_final", yield);
            INSPOUT("Yield_UIL", path.value(path.length()-1) / path.value(0));

            // Applicable discount factor
            discount = discountTS.link.discount(path.time(path.length() - 1), true);

            // Payoff computation
            double indicateur = 0.0;
            
            if (yield < PDI_barrier)
            {
                indicateur = 1.0;
            }
            
            // Return 
            return (leverage * yield - 1.0) * discount * indicateur;

        }
    }
}
