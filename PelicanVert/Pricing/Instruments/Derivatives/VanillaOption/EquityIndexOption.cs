using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO.Markit
{
    public class EquityIndexOption : BaseIndexOption
    {


        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : From given date, no data
        //public EquityIndexOption(DateTime pricingDate) : base(pricingDate: pricingDate) { }

            
        // Constructor 2: For given date & Markit Volatility surface
        public EquityIndexOption(  DateTime pricingDate, DateTime strikeDate, DateTime forwardStartDate,
                                DateTime expiryDate, Double strikeLevel, MarkitSurface surfaceObject)

            : base( pricingDate: pricingDate,  strikeDate: strikeDate,  forwardStartDate: forwardStartDate,
                    expiryDate: expiryDate, strikeLevel: strikeLevel, surfaceObject: surfaceObject)
        { }


        #endregion


        // ************************************************************
        // PRICING 
        // ************************************************************


        #region

        // PRICING WITH BLACK-SCHOLES  MODEL (CONTINOUS FOR EQUITY INDICES)

        public Double EuropeanCall(DateTime pricingDate)
        {
            return EuropeanOption(pricingDate, 1);

        }

        public Double EuropeanPut(DateTime pricingDate)
        {
            return EuropeanOption(pricingDate, -1);

        }

        public Double EuropeanOption(DateTime pricingDate, int CallPut)
        {
            if (impliedVolatilitySurface.isNewFormat == false) { return EuropeanOption_Black76(pricingDate, CallPut); }

            else { return EuropeanOption_BlackScholes(pricingDate, CallPut); }

        }

        protected Double EuropeanOption_BlackScholes(DateTime pricingDate, int CallPut)
        {
            Double iv = impliedVolatility;
            Double ttm = (expiryDate - pricingDate).TotalDays / 365.25;
            Double sq_var = iv * Math.Sqrt(ttm);
            
            Double d1 = (Math.Log(spot / strikeLevel) + (zeroRate - (repoRate + dividendYield) + 0.5*iv*iv) * ttm) / sq_var;
            Double d2 = d1 - sq_var;

            return CallPut * spot * NormalCDF(CallPut * d1) - CallPut * strikeLevel * NormalCDF(CallPut * d2) * Math.Exp(-1 * zeroRate * ttm);

        }



        protected Double EuropeanOption_Black76(DateTime pricingDate, int CallPut)
        {
            Double iv = impliedVolatility;
            Double ttm = (expiryDate - pricingDate).TotalDays / 365.25;
            Double sq_var = iv * Math.Sqrt(ttm);

            Double d1 = (Math.Log(forward / strikeLevel) + ( 0.5 * iv * iv) * ttm) / sq_var;
            Double d2 = d1 - sq_var;

            return Math.Exp(-1 * zeroRate * ttm) *(CallPut * NormalCDF(CallPut * d1) * forward - CallPut * strikeLevel * NormalCDF(CallPut * d2) );

        }


        public Double NormalCDF(Double x)
        {

            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of x
            //int sign = 1;
            //if (x < 0)
            //    sign = -1;
            // x = fabs(x) / sqrt(2.0);

            int sign = Math.Sign(x);
            x = Math.Abs(x) / Math.Pow(2.0, 0.5); 

            // A & S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return 0.5 * (1.0 + sign * y);


        }


        #endregion

    }
}
