using System.Collections.Generic;
using System.Linq;
using System;

namespace QLNet
{
    /// INSTRUMENT ///
    public class GenericAutocall : GenericInstrument
    {


        public GenericAutocall(List<Date> fixings,
                               double coupon,
                               double barrierlvl,
                               double strike)
            : base(BuildDico("fixing", fixings),
                   BuildDico("coupons", BuildCouponList(coupon, fixings.Count)),
                   BuildDico("barrier", barrierlvl),
                   BuildDico("strike", strike))
        { }



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
        
        public override double ScriptDico(Dictionary<string, List<double>> timeDico,
                                     Dictionary<string, List<double>> indexDico,
                                     Handle<YieldTermStructure> discountTS,
                                     Path path)
        {

            double fixingValue = 0.0;
            double payoff = 0.0;
            double yield = 0.0;
            bool iscalled = false;
            double discount = 1.0;
            int i = 0;

            double strike = indexDico["strike"][0];

            // Go through all dates //
            for (int t = 0; t < path.length(); t++)
            {
                if (Math.Round( path.time(t),9) == Math.Round( timeDico["fixing"][i],9) && (iscalled == false))
                {
                    fixingValue = path.value(t);
                    yield = fixingValue / strike;
                    INSPOUT("yield " + i, yield);
                    if (yield > 1.0)
                    {
                        iscalled = true;
                        discount = discountTS.link.discount(path.time(i), true);
                        payoff = (1 + indexDico["coupons"][i]) * 100 * discount;
                        INSPOUT("ProbaCall " + i, 1.0);
                        //return payoff;
                    }
                    i++;
                }
            }

            // if no previous payoff compute last redemption //
            fixingValue = path.value(path.length() - 1);
            yield = fixingValue / strike;
            discount = discountTS.link.discount(path.time(path.length() - 1), true);

            if ((iscalled == false) && (yield < indexDico["barrier"][0]))
            {
                payoff = yield * 100 * discount;
                INSPOUT("ProbaDown", 1.0);
                INSPOUT("AvgDown", yield);
            }
            else if (iscalled == false)
            {
                payoff = 100.0 * discount;
                INSPOUT("ProbaMid", 1.0);
                INSPOUT("AvgUp", yield);
            }

            // return payoff  //
            return payoff;

        }

        
        /*
        public override double ScriptDico(Dictionary<string, List<double>> timeDico,
                              Dictionary<string, List<double>> indexDico,
                              Handle<YieldTermStructure> discountTS,
                              Path path)
        {

            double fixingValue = 0.0;
            double payoff = 0.0;
            double yield = 0.0;
            bool iscalled = false;
            double discount = 1.0;
            int i = 0;

            double strike = indexDico["strike"][0];

            fixingValue = path.value(path.length() - 1);
            yield = fixingValue / strike;
            discount = discountTS.link.discount(path.time(path.length() - 1), true);

            // return payoff  //
            return yield * discount;
        }
        */

    }

}

