

///////  Marc RAYGOT - 2017   ///////


using System;
using System.Collections.Generic;

namespace QLNet
{
    public class HazardRate : ITraits<DefaultProbabilityTermStructure>
    {
        const double maxValue = 2.0; // less than 15% survavility over a year... we don't deal that here !!
        const double avgHRate = 0.01;
        public Date initialDate(DefaultProbabilityTermStructure c) { return c.referenceDate(); }   // start of curve data
        public double initialValue(DefaultProbabilityTermStructure c) { return 1; }    // value at reference date
        public bool dummyInitialValue() { return false; }   // true if the initialValue is just a dummy value
        public double initialGuess() { return 1.0 / (1.0 + avgHRate * 0.25); }   // initial guess
        public double guess(DefaultProbabilityTermStructure c, Date d)
        { 
            return c.hazardRate(d,true); 
        }  // further guesses

        // possible constraints based on previous values
        public double minValueAfter(int s, List<double> l) {return Const.QL_EPSILON;}
        public double maxValueAfter(int i, List<double> data) { return maxValue; } 
        // update with new guess
        public void updateGuess(List<double> data, double hazardRate, int i) 
        {
            data[i] = hazardRate;
            if (i == 1)
                data[0] = hazardRate; 
        }
        public int maxIterations() { return 100; }   // upper bound for convergence loop

        public double hazardRate(Interpolation i, double t) { return i.value(t, true); }
        public double zeroYieldImpl(Interpolation i, double t) { return i.value(t, true); }

        public double discountImpl(Interpolation i, double t) { throw new NotSupportedException(); }
        public double forwardImpl(Interpolation i, double t) { throw new NotSupportedException(); }

        public double guess(int i, InterpolatedCurve c, bool validData, int f)
        {
            if (validData) // previous iteration value
                return c.data()[i];

            if (i == 1) // first pillar
                return avgHRate;

            return hazardRate(c.interpolation_, c.times()[i]);
        }

        public double minValueAfter(int i, InterpolatedCurve c, bool validData, int f)
        {
            if (validData)
            { 
                double min = 1.0;
                foreach ( double dbl in c.data())
                {
                    if (dbl < min) { min = dbl; }
                }
                return min / 2.0;
            }
            return Const.QL_EPSILON;
        }

        public double maxValueAfter(int i, InterpolatedCurve c, bool validData, int f)
        {
            if (validData)
            {
                double max = 1.0;
                foreach (double dbl in c.data())
                {
                    if (dbl > max) { max = dbl; }
                }
                return Math.Min(maxValue, max * 2.0);
            }
            return maxValue;
        }

    }

}
