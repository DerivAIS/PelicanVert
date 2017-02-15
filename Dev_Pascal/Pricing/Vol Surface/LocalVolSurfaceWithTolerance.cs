
using System;

namespace QLNet
{

    public class LocalVolSurfaceTol : LocalVolSurface
    {
        Handle<BlackVolTermStructure> blackTS_;
        Handle<YieldTermStructure> riskFreeTS_, dividendTS_;
        Handle<Quote> underlying_;

        public LocalVolSurfaceTol(Handle<BlackVolTermStructure> blackTS, Handle<YieldTermStructure> riskFreeTS,
                               Handle<YieldTermStructure> dividendTS, Handle<Quote> underlying)
            : base(blackTS, riskFreeTS, dividendTS, underlying)
        {
            blackTS_ = blackTS;
            riskFreeTS_ = riskFreeTS;
            dividendTS_ = dividendTS;
            underlying_ = underlying;

            blackTS_.registerWith(update);
            riskFreeTS_.registerWith(update);
            dividendTS_.registerWith(update);
            underlying_.registerWith(update);
        }


        protected override double localVolImpl(double t, double underlyingLevel)
        {

            double forwardValue = underlying_.link.value() *
                (dividendTS_.link.discount(t, true) /
                 riskFreeTS_.link.discount(t, true));

            // strike derivatives
            double strike, y, dy, strikep, strikem;
            double w, wp, wm, dwdy, d2wdy2;
            strike = underlyingLevel;
            y = Math.Log(strike / forwardValue);
            dy = ((y != 0.0) ? y * 0.000001 : 0.000001);
            strikep = strike * Math.Exp(dy);
            strikem = strike / Math.Exp(dy);
            w = blackTS_.link.blackVariance(t, strike, true);
            wp = blackTS_.link.blackVariance(t, strikep, true);
            wm = blackTS_.link.blackVariance(t, strikem, true);
            dwdy = (wp - wm) / (2.0 * dy);
            d2wdy2 = (wp - 2.0 * w + wm) / (dy * dy);

            // time derivative
            double dt, wpt, wmt, dwdt;
            if (t == 0.0)
            {
                dt = 0.0001;
                wpt = blackTS_.link.blackVariance(t + dt, strike, true);

                if (!(wpt >= w))
                    throw new Exception("decreasing variance at strike " + strike
                          + " between time " + t + " and time " + (t + dt));
                dwdt = (wpt - w) / dt;
            }
            else
            {
                dt = Math.Min(0.0001, t / 2.0);
                wpt = blackTS_.link.blackVariance(t + dt, strike, true);
                wmt = blackTS_.link.blackVariance(t - dt, strike, true);
                if (!(wpt >= w))
                    wpt = w;
                /*
                    throw new Exception("decreasing variance at strike " + strike
                          + " between time " + t + " and time " + (t + dt));
                 */
                if (!(w >= wmt))
                    wmt = w;
                /*
                throw new Exception("decreasing variance at strike " + strike
                      + " between time " + (t - dt) + " and time " + t);
                */
                dwdt = (wpt - wmt) / (2.0 * dt);
            }

            if (dwdy == 0.0 && d2wdy2 == 0.0)
            { // avoid /w where w might be 0.0
                return Math.Sqrt(dwdt);
            }
            else
            {
                double den1 = 1.0 - y / w * dwdy;
                double den2 = 0.25 * (-0.25 - 1.0 / w + y * y / w / w) * dwdy * dwdy;
                double den3 = 0.5 * d2wdy2;
                double den = den1 + den2 + den3;
                double result = dwdt / den;
                if (!(result >= 0.0))
                    return 0.0;
                /*
                throw new Exception("negative local vol^2 at strike " + strike
                      + " and time " + t + "; the black vol surface is not smooth enough");
                */
                return Math.Sqrt(result);
                // return std::sqrt(dwdt / (1.0 - y/w*dwdy +
                //    0.25*(-0.25 - 1.0/w + y*y/w/w)*dwdy*dwdy + 0.5*d2wdy2));
            }
        }
    }
}
