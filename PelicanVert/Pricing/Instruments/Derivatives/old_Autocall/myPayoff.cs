using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.Instruments.Derivatives
{
    public class AutocallPayoff : Payoff
    {

        // ************************************************************
        // INSTANCE PROPERTIES 
        // ************************************************************

        public double spotAtStrikeDate() { return spotAtStrikeDate_; }
        protected double spotAtStrikeDate_;

        public double recallMoneyness() { return recallMoneyness_; }
        protected double recallMoneyness_;
        

        public double finalBarrierMoneyness() { return finalBarrierMoneyness_; }
        protected double finalBarrierMoneyness_;

        public double CouponRate() { return CouponRate_; }
        protected double CouponRate_;

        public double nominal() { return nominal_; }
        protected double nominal_;

        DayCounter dayCount_;
        Calendar calendar_;


        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        public AutocallPayoff(double nominal, double spotAtStrikeDate, double CouponRate, double recallMoneyness,
                               double finalBarrierMoneyness, DayCounter dayCount) : base()
        {

            // Assign levels
            spotAtStrikeDate_ = spotAtStrikeDate;
            recallMoneyness_ = recallMoneyness;
            finalBarrierMoneyness_ = finalBarrierMoneyness;
            CouponRate_ = CouponRate;

            //Assign nominal
            nominal_ = nominal;

            // Assign QLNet Calendar and Day counter objects
            dayCount_ = dayCount;
           
        }


        public AutocallPayoff(double nominal, double spotAtStrikeDate, double CouponRate, double recallMoneyness,
                       double finalBarrierMoneyness) : this(nominal, spotAtStrikeDate, CouponRate, recallMoneyness,
                                finalBarrierMoneyness, new Actual365Fixed())
        { }



        // ************************************************************
        // METHODS - VALUATION
        // ************************************************************

        /*
        public double value(Path thisPath, AutocallExercise thisExercise, GeneralizedBlackScholesProcess process)
        {

            int cas = 3;

            switch (cas)
            {
                case 1:
                    //Console.WriteLine("Pricing : Forward");
                    return value_forward(thisPath, thisExercise, process);
                    
                case 2:
                    //Console.WriteLine("Pricing : PDI");
                    return value_PDI(thisPath, thisExercise, process);

                case 3:
                    //Console.WriteLine("Pricing : Autocall");
                    return value_autocall(thisPath, thisExercise, process);
                    
            }

            return this.value_autocall(thisPath, thisExercise, process);
        }

        */


        /// <summary>  
        ///  Performs valuations of the autocall payoff for a given Monte Carlo trajectory.
        /// </summary>  
        public double value(Path thisPath, AutocallExercise thisExercise, GeneralizedBlackScholesProcess process)
        {

            int nbSteps = thisExercise.dates().Count();

            double pmt = 0.0;
            double yield = 0.0;

            for (int i = 0; i < nbSteps; i++)
            {
                Date d = Settings.evaluationDate();
                double t_k = dayCount_.yearFraction(d, thisExercise.dates()[i]);

                // Check for coupons payments and autocall events
                if (t_k >= 0.0)
                {
                    int k = thisPath.timeGrid().closestIndex(t_k);
                    yield = thisPath.value(k) / spotAtStrikeDate_;

                    if (yield >= recallMoneyness_)
                    {
                        double df = process.riskFreeRate().link.discount(t_k);
                        pmt += df * nominal_ * (1 + (1+i) * CouponRate_);
                        return pmt;
                    }
                }
            }


            // Product maturity reached without autocall event, checking only PDI
            double final_DF = process.riskFreeRate().link.discount(thisPath.timeGrid().Last());

            yield = thisPath.back() / spotAtStrikeDate_;

            // Case 1 : Above (or equal to) PDI barrier
            if (yield >= finalBarrierMoneyness_)
            {
                pmt += 1.0;
            }

            // Case 2 : Below PDI barrier
            else
            {
                pmt += yield;
            }

            return nominal_ * final_DF * pmt;
        }


        /*

      #region -- DO NOT USE -- other payoffs for testing purposes


      public double value_forward(Path thisPath, AutocallExercise thisExercise, GeneralizedBlackScholesProcess process) // Forward
      {
          double df = process.riskFreeRate().link.discount(thisPath.timeGrid().Last());

          return  nominal_ * thisPath.back() / spotAtStrikeDate_;

      }

      public double value_PDI(Path thisPath, AutocallExercise thisExercise, GeneralizedBlackScholesProcess process) // PDI fundée
      {

          double yield = 0.0;
          double pmt = 0.0;

          yield = thisPath.back() / spotAtStrikeDate_;

          // Case 1 : Above (or equal to) PDI barrier
          if (yield >= finalBarrierMoneyness_)
          {
              pmt += 1.0;
          }

          // Case 1 : Below PDI barrier
          else
          {
              pmt += yield;
          }

          return nominal_ * pmt;

      }

      #endregion

      */

    }
}
