

///////  Marc RAYGOT - 2017   ///////


using System.Collections.Generic;
using System.Linq;
using System;

namespace QLNet
{
    /// INSTRUMENT ///
    public class GenericScriptRepack : GenericScriptInstrument
    {


        public GenericScriptRepack(List<Date> fixings,
                               double couponBond,
                               double couponBinary,
                               double barrierDown,

                               double strike,
                               double recovery)
            : base(BuildDico("fixing", fixings),
                   BuildDico("couponBond", couponBond),
                   BuildDico("couponBinary", couponBinary),
                   BuildDico("barrierDown", barrierDown),

                   BuildDico("strike", strike),
                   BuildDico("recovery", recovery))
        {
            setupScript(new RepackScript().script);
        }


        /// Build Coupon List ///


        ///Script  ///

        public class RepackScript : Script
        {
            public RepackScript() { }

            public double script(ScriptFunctions sf, ScriptData sd)
            {
                double bond = 0.0;
                double put = 0.0;
                double binaryStrip = 0.0;
                double swap = 0.0;

                int i = 0;
                double time = 0.0;

                double strike = sd.INDEX("strike")[0];
                double couponBond = sd.INDEX("couponBond")[0];
                double couponBinary = sd.INDEX("couponBinary")[0];
                double barrierDown = sd.INDEX("barrierDown")[0];

                double recovery = sd.INDEX("recovery")[0];
                double maturity = sd.ENDTIME("fixing");

                double div =  0.0;
                double rf = 0.0;
                double survpb = 1.0;
                double survpb_m1 = 1.0;
                double vol = 0.0;
                double s = 0.0;

                // Go through all dates //
                for (int t = 0; t < sf.PATHNB(); t++)
                {
                    s = sf.PATHVALUE(t);
                    time = sf.PATHTIME(t);

                    if ((time == sd.TIME("fixing")[i] )&& (time!= maturity))
                    {
                        bond = 0.0;
                        put = 0.0;
                        binaryStrip = 0.0;
                        
                        //bond
                        for (int j = i;j < sd.TIME("fixing").Count -1;j++)
                            bond = couponBond*sf.DISCOUNT(sd.TIME("fixing")[j]);
                        //bond += sf.DISCOUNT(maturity);
                        bond = bond / sf.DISCOUNT(time);
                        sf.INSPOUT("Bond" + i, bond);
                        
                        //pdi
                        vol = sf.VOLATILITY(maturity- time, s);
                        rf = sf.RISKFREERATE(maturity, time);
                        div = sf.DIVIDENDRATE(maturity, time);

                        put = sf.OPTIONPRICE(maturity, time, barrierDown, s, div, rf, vol, Option.Type.Put);
                        put = put / barrierDown;
                        sf.INSPOUT("PDI" + i, put);

                        // binary strip
                        for (int j = i; j < sd.TIME("fixing").Count - 1; j++)
                        {                
                            vol = sf.VOLATILITY(sd.TIME("fixing")[j] - time, s);
                            rf = sf.RISKFREERATE(maturity, time);
                            div = sf.DIVIDENDRATE(maturity, time);

                            binaryStrip += couponBinary / sf.DISCOUNT(time) * sf.BINARYPRICE(maturity, time, barrierDown, s, div, rf, vol, Option.Type.Call);
                        }
                        sf.INSPOUT("Binary" + i, binaryStrip);

                        // Swap
                        survpb = sf.SURVIVALPB(time);
                        swap += (survpb_m1- survpb) * Math.Max(0.0, recovery - bond + binaryStrip - put) * sf.DISCOUNT(time);
                        sf.INSPOUT("CondProba" + i, (survpb_m1 - survpb) );
                        survpb_m1 = survpb;

                        i++;
                    }
                }

                sf.INSPOUT("yield ", s);
                // return payoff  //
                return swap;
            }
        }
    }
}

