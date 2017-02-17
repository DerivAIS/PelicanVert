using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace Pascal.Pricing.Instruments
{

    public class ScriptedBinaire : GenericInstrument
    {
        
        public ScriptedBinaire(List<Date> observationDates,
            double coupon,
            double barrierMoneyness,
            double cliquetMoneyness,
            // double PDI_Moneyness,
            double strikeLevel,
            double FixDiv_points = 0.0,
            double CashYield = 0.0)

                : base(BuildDico("Observation_Dates", observationDates),
                       BuildDico("Coupons", coupon),
                       BuildDico("Barrier_Moneyness", barrierMoneyness),
                       BuildDico("Cliquet_Moneyness", cliquetMoneyness),
                       //BuildDico("PDI_Moneyness", PDI_Moneyness),
                       BuildDico("Fixed_Dividend", FixDiv_points),
                       BuildDico("Cash_yield", CashYield),
                       BuildDico("Strike_Level", strikeLevel))
            { }
        
            ///Script  ///
            public override double ScriptDico(Dictionary<string, List<double>> timeDico,
                                         Dictionary<string, List<double>> indexDico,
                                         Handle<YieldTermStructure> discountTS,
                                         Path path)
            {

            double fixingValue = 0.0;
            double payoff = 0.0;
            double yield = 0.0;

            bool isCliquet = false;

            double cliquetMoney = indexDico["Cliquet_Moneyness"][0];
            double barrierMoney = indexDico["Barrier_Moneyness"][0];
            double couponRate = indexDico["Coupons"][0];
            double DivFix = indexDico["Fixed_Dividend"][0];
            double cashYield = indexDico["Cash_yield"][0];

            double discount = 1.0;

            int i = 0;

            // Current spot level
            double IL = path.value(0);
            double previous_IL = path.value(0);
            double previous_UIL = path.value(0);
            double previous_Time = 0.0;
            double dT = 0.0;

            double pathValue = 0.0;
            double strike = indexDico["Strike_Level"][0];

            // Go through all dates //
            for (int t = 1; t < path.length(); t++)
            {

                // Compute the path based for fixed div index
                dT = path.time(t) - previous_Time;
                IL = ( path.value(t) / previous_UIL ) * previous_IL * (1 + dT * cashYield) - DivFix * dT;
                previous_Time = path.time(t);
                
                //
                if (path.time(t) == timeDico["Observation_Dates"][i]){

                    pathValue = path.value(t);
                    fixingValue = IL;
                    yield = fixingValue / strike;
                    discount = discountTS.link.discount(path.time(i), true);

                    if (isCliquet) { payoff += couponRate * discount; }

                    else if (yield >= cliquetMoney)
                    {
                        isCliquet = true;
                        payoff += couponRate * discount;
                        INSPOUT("Cliquet", 1.0);
                    }

                    else if (yield >= barrierMoney)
                    {
                        payoff += couponRate * discount;
                    }

                    i++;
                }
            }

            fixingValue = path.value(path.length()-1);
            yield = fixingValue / strike;
       
            INSPOUT("AvgYield", yield);
            return payoff;

            // if no previous payoff compute last redemption //
            /*
            fixingValue = path.value(path.length() - 1);
            yield = fixingValue / strike;

            discount = discountTS.link.discount(path.time(path.length() - 1), true);
            double PDI_barrier = indexDico["PDI_Moneyness"][0];

            if (yield < PDI_barrier){
                    payoff += yield * discount;
                    INSPOUT("ProbaDown", 1.0);
                    INSPOUT("AvgDown", yield);
                }

            else{
                    payoff += discount;
                    INSPOUT("ProbaMid", 1.0);
                    INSPOUT("AvgMid", yield);
            }

            // return payoff 
            return payoff;
            */
        }
    }
}
