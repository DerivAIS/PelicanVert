

///////  Marc RAYGOT - 2017   ///////


using System.Collections.Generic;
using System.Linq;
using System;

namespace QLNet
{
    /// INSTRUMENT ///
    public class ScriptGenericAutocall : GenericScriptInstrument
    {


        public ScriptGenericAutocall(List<Date> fixings,
                               double coupon,
                               double barrierlvl,
                               double strike)
            : base(BuildDico("fixing", fixings),
                   BuildDico("coupons", BuildCouponList(coupon, fixings.Count)),
                   BuildDico("barrier", barrierlvl),
                   BuildDico("strike", strike))
        {

            setupScript(new AutocallScript().script);
        }


        /// Build Coupon List ///
        private static List<double> BuildCouponList(double couponValue, int couponNumber)
        {
            var couponList = new List<double>();
            for (var i = 0; i < couponNumber; i++)
            {
                couponList.Add(couponValue * (1 + i));
            }
            return couponList;
        }


        ///Script  ///
        
        public class AutocallScript : Script
        {
            public AutocallScript() { }

            public double script(ScriptFunctions sf, ScriptData sd)
            {
                double fixingValue = 0.0;
                double payoff = 0.0;
                double yield = 0.0;
                bool iscalled = false;
                double discount = 1.0;
                int i = 0;
                
                double strike = sd.INDEX("strike")[0];

                // Go through all dates //
                for (int t = 0; t < sf.PATHNB(); t++)
                {
                    double k = sf.PATHVALUE(t);
                    if (sf.PATHTIME(t) == sd.TIME("fixing")[i] && (iscalled == false))
                    {
                        fixingValue = sf.PATHVALUE(t);
                        yield = fixingValue / strike;
                        if (yield > 1.0)
                        {
                            iscalled = true;
                            discount = sf.DISCOUNT(sf.PATHTIME(t));
                                
                            payoff = (1 + sd.INDEX("coupons")[i]) * 100 * discount;
                            sf.INSPOUT("ProbaCall " + i, 1.0);
                            return payoff;
                        }
                        i++;
                    }
                }

                // if no previous payoff compute last redemption //
                fixingValue = sf.PATHVALUE();
                yield = fixingValue / strike;
                discount = sf.DISCOUNT();

                if ((iscalled == false) && (yield < sd.INDEX("barrier")[0]))
                {
                    payoff = yield * 100 * discount;
                    sf.INSPOUT("ProbaDown", 1.0);
                    sf.INSPOUT("AvgDown", yield);
                }
                else
                {
                    payoff = 100.0 * discount;
                    sf.INSPOUT("ProbaMid", 1.0);
                }

                // return payoff  //
                return payoff;
            }
        }
    }
}

