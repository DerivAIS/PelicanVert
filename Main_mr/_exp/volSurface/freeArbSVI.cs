using System.Collections.Generic;
using System.Linq;
using System;

//using System.Linq.Expressions;

namespace QLNet
{

    public class freeArbSVI 
    {
        /* 
        Based on the work of Matthias R. Fengler
        https://core.ac.uk/download/pdf/6978470.pdf
        */

        protected List<double> strikes_;
        protected List<double> times_; 
        protected double spot_;  
        protected Handle<YieldTermStructure> riskFreeTS_;
        protected Handle<YieldTermStructure> dividendTS_;
        protected Matrix crudeVolSurface_;

        protected List<CubicInterpolation> g_i_;
        protected List<Vector> gamma_;
        protected List<Vector> g_;
        protected double lambda_;
        protected Matrix A_;
        protected Matrix B_;
        protected List<List<double>> strikesSpline_;

        protected Matrix fwMoneynessGrid_;
        protected Matrix totalVariance_;
        protected Matrix X_;

        protected CumulativeNormalDistribution cumulNormdiv_;


        public freeArbSVI(List<double> strikes,
                          List<double> times, 
                          double spot,  
                          Handle<YieldTermStructure> riskFreeTS, 
                          Handle<YieldTermStructure> dividendTS,
                          Matrix crudeVolSurface,
                          double lambda) 
        {
            // sanity check to add : check date + strikes vs vol surface
            
            strikes_ = strikes;
            times_ = times;
            spot_ = spot;
            riskFreeTS_ = riskFreeTS;
            dividendTS_ = dividendTS;

            fwMoneynessGrid_ = new Matrix(times_.Count,strikes_.Count,  0.0);
            totalVariance_ = new Matrix(times_.Count, strikes_.Count, 0.0);
            X_ = new Matrix(times_.Count, 2*strikes_.Count+2, 0.0);
            crudeVolSurface_ = new Matrix(crudeVolSurface);
            cumulNormdiv_ = new CumulativeNormalDistribution();

            lambda_ = lambda;
            g_i_ = new List<CubicInterpolation>();
            A_ = new Matrix(2 * strikes_.Count+2, strikes_.Count, 0.0); ;
            B_ = new Matrix(2 * strikes_.Count + 2, 2 * strikes_.Count + 1, 0.0); ;
            strikesSpline_ = new InitializedList<List<double>>();


        }



       /// Build matrices ///

        /// Forward moneyness ///
        public void matricesBuildingForwardMoneyness()
        {
            double discount_ = 0.0;

            for (int j = 0; j < fwMoneynessGrid_.columns(); j++)
            {
                for (int i = 0; i < fwMoneynessGrid_.rows(); i++)
                {
                    discount_ =  dividendTS_.link.discount(times_[i], true) / riskFreeTS_.link.discount(times_[i], true);
                    fwMoneynessGrid_[i, j] = strikes_[j] / (spot_ * discount_);
                }
            }
        }

        /// Total variance ///
        public void matricesBuildingTotalVariance()
        {
            for (int j = 0; j < fwMoneynessGrid_.columns(); j++)
            {
                for (int i = 0; i < fwMoneynessGrid_.rows(); i++)
                {
                    totalVariance_[i, j] = times_[i] * crudeVolSurface_[i, j] * crudeVolSurface_[i, j];
                }
            }
        }

        /// Total BS prices ///
        public void matricesBuildingBSPrices()
        {

            for (int j = 0; j < fwMoneynessGrid_.columns(); j++)
            {
                for (int i = 0; i < fwMoneynessGrid_.rows(); i++)
                {
                    X_[i, j] = blackCallPrice(crudeVolSurface_[i, j],
                                                     times_[i],  
                                                     strikes_[j],
                                                     riskFreeTS_.link.discount(times_[i], true),
                                                     dividendTS_.link.discount(times_[i], true));
                }
            }
        }



        /// A and B ///
        public void matricesBuildingA()
        {
            int k = 0;

            for (int j = 0; j < A_.columns(); j++)
            {
                for (int i = 0; i < A_.rows(); i++)
                {
                    if  (i >= strikes_.Count + 1)
                    {
                        if (i == j + strikes_.Count + 1)
                        {
                            k = i - strikesSpline_[0].Count+1;
                            A_[i, j] = - (1.0 / 3) * (strikesSpline_[0][k + 2] - strikesSpline_[0][k]);
                        }
                        if ((i == j + strikes_.Count + 1+1) || (i + 1 == j + strikes_.Count + 1))
                        {
                            k = i - strikesSpline_[0].Count+1;
                            A_[i, j] = - (1.0 / 6) * (strikesSpline_[0][k + 1] - strikesSpline_[0][k]);
                        }
                    }
                }
            }
        }

        public void matricesBuildingB()
        {
            int k = 0;

            for (int j = 0; j < B_.columns(); j++)
            {
                for (int i = 0; i < B_.rows(); i++)
                {
                    if ((i == j) && (i < strikes_.Count + 2))
                        B_[i,i] = 1.0;
                    if ((j >= strikes_.Count - 1) && (i >= strikes_.Count + 2))
                    {
                        if (i == j)
                        {
                            k = i - strikesSpline_[0].Count;
                            B_[i, i] = lambda_ * (1.0 / 3) * (strikesSpline_[0][k + 2] - strikesSpline_[0][k]);
                        }
                        if ((i == j + 1) || (i + 1 == j))
                        {
                            k = i - strikesSpline_[0].Count;
                            B_[i, j] = lambda_ * (1.0 / 6) * (strikesSpline_[0][k + 1] - strikesSpline_[0][k]);
                        }
                    }
                }
            }
        }
 
        /// Spline  ///
        public void splincalculation()
        {
            
            double rightConditionValue = 0.0;
            double leftConditionValue = 0.0;
            int size = strikes_.Count+2;

            List<List<double>> bspriceSpline = new InitializedList<List<double>>();


            gamma_ = new InitializedList<Vector>();
            g_ = new InitializedList<Vector>();

            for (int i = 0; i < fwMoneynessGrid_.rows(); i++)
            {
                leftConditionValue = spot_ * dividendTS_.link.discount(times_[i], true) / riskFreeTS_.link.discount(times_[i], true);
                bspriceSpline.Add (new InitializedList<double>());
                strikesSpline_.Add(new InitializedList<double>());

                bspriceSpline[i].Add(leftConditionValue);
                strikesSpline_[i].Add(0.0);
                for (int j = 0; j < fwMoneynessGrid_.columns(); j++)
                {
                    bspriceSpline[i].Add(X_[i, j]);
                    strikesSpline_[i].Add(strikes_[j]);
                }

                rightConditionValue = 0.0;
                bspriceSpline[i].Add(rightConditionValue);
                strikesSpline_[i].Add(spot_ * 10000);
                g_i_.Add(new CubicInterpolation(strikesSpline_[i], 
                                                size,
                                                bspriceSpline[i],
                                                CubicInterpolation.DerivativeApprox.Spline,
                                                true,
                                                CubicInterpolation.BoundaryCondition.SecondDerivative,
                                                leftConditionValue,
                                                CubicInterpolation.BoundaryCondition.SecondDerivative,
                                                rightConditionValue));

            List<double> tempList = new List<double>();
            tempList = g_i_[i].bCoefficients();
            tempList[0] = 0.0;
            tempList[tempList.Count-1] = 0.0;
            gamma_.Add(new Vector(tempList)); 
            g_.Add (new Vector(strikes_));
            }
        }



        /// Other functions
        public double blackCallPrice(double vol, double time, double strike, double rf, double div)
        {
            double totalvolsqr = vol * Math.Sqrt(time);
            double divVal = -Math.Log(div) / time;
            double rfVal = -Math.Log(rf) / time;

            double d1 = (1 / totalvolsqr) * (Math.Log(spot_ / strike) + (0.5 * vol * vol + rfVal - divVal) * time);
            double d2 = d1 - totalvolsqr;

            return spot_ * div * cumulNormdiv_.value(d1) - strike * rf * cumulNormdiv_.value(d2);
        }



    }
}

