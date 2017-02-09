using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.Instruments.Derivatives
{
    public class MarkitForwardCurve : QLNet.InterpolatedForwardCurve<LogLinear>
    {
        
        public MarkitForwardCurve(List<Date> Dates, List<double> ForwardRates, DayCounter dayCounter) : base(Dates, ForwardRates, dayCounter) { }


        protected override double forwardImpl(double s)
        {
            Console.WriteLine("Calling forwardImpl() from MarkitForwardCurve.");
            return base.forwardImpl(s);
        }

        protected virtual double zeroYieldImpl(double t)
        {
            if (t == 0.0)
                return forwardImpl(0.0);
            // implement smarter integration if plan to use the following code
            double sum = 0.5 * forwardImpl(0.0);
            int N = 1000;
            double dt = t / N;
            for (double i = dt; i < t; i += dt)
                sum += forwardImpl(i);
            sum += 0.5 * forwardImpl(t);
            return (sum * dt / t);
        }


        protected override double discountImpl(double t)
        {
            if (t == 0.0)     // this acts as a safe guard in cases where
                return 1.0;   // zeroYieldImpl(0.0) would throw.

            double r = zeroYieldImpl(t);
            return Math.Exp(-r * t);
        }

    }
}
