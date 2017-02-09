using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO.Markit
{
    public class BaseIndexOption
    {


        // ************************************************************
        // CLASS PROPERTIES - TYPE 
        // ************************************************************

        // OPTION TYPE
        #region Option Type

        protected Option.Type _optionType;
        public Option.Type optionType
        {
            get { return _optionType; }
            protected set
            {
                this._optionType = value;
            }
        }

        #endregion


        // ************************************************************
        // CLASS PROPERTIES - DATES 
        // ************************************************************

        // STRIKE DATE
        #region Strike Date

        protected DateTime _strikeDate = DateTime.MinValue;
        public DateTime strikeDate
        {
            get { return _strikeDate; }
            protected set
            {
                this._strikeDate = value;
            }
        }

        #endregion

        // EXPIRY DATE
        #region Expiry Date

        protected DateTime _expiryDate = DateTime.MinValue;
        public DateTime expiryDate
        {
            get { return _expiryDate; }
            protected set
            {
                this._expiryDate = value;
            }
        }

        #endregion

        // PRICING DATE
        #region Pricing Date

        protected DateTime _pricingDate = DateTime.MinValue;
        public DateTime pricingDate
        {
            get { return _pricingDate; }
            protected set
            {
                this._pricingDate = value;
            }
        }

        #endregion

        // FORWARD START DATE
        #region Forward Start Date

        protected DateTime _forwardStartDate = DateTime.MinValue;
        public DateTime forwardStartDate
        {
            get { return _forwardStartDate; }
            protected set
            {
                this._forwardStartDate = value;
            }
        }

        #endregion

        // INFORMATION DATE
        // Corresponds to the filtration, ie data stored in the object
        #region Information Date
            
        public DateTime informationDate
        {
            get
            {
                return this.impliedVolatilitySurface.informationDate; 
            }
        }

        #endregion

  

        // ************************************************************
        // CLASS PROPERTIES - STRIKE DATA 
        // ************************************************************

        // STRIKE LEVEL
        #region Strike Level (in underlying terms)

        protected double _strikeLevel;
        public double strikeLevel
        {
            get { return _strikeLevel; }
            protected set
            {
                this._strikeLevel = value;
            }
        }

        #endregion
        
        
        // STRIKE MONEYNESS
        #region  Strike Moneyness
            
        public double strikeMoneyness
        {
            get
            {
                return this.strikeLevel / this.spot;
            }

           
        }

        #endregion


        // UNDERLYING STRIKE LEVEL
        #region Underlying Level at Strike Date

        protected double _underlyingAtStrike;
        public double underlyingAtStrike
        {
            get { return _underlyingAtStrike; }
            protected set
            {
                this._underlyingAtStrike = value;
            }
        }

        #endregion

        
        
        // ************************************************************
        // CLASS PROPERTIES - SPOT / FORWARD
        // ************************************************************

        // SPOT
        #region Spot

       public Double spot
        {
            get
            {
                return this.impliedVolatilitySurface.impliedSpot;
            }
                    
        }

        #endregion

        // FORWARD
        #region Forward

        public Double forward
        {
            get
            {
                return this.impliedVolatilitySurface.forward[this.expiryDate]; 
            }

        }

        #endregion



        // ************************************************************
        // CLASS PROPERTIES - DIV / REPO
        // ************************************************************

        // DIVIDEND YIELD
        #region Dividend Yield

        public Double dividendYield
        {
            get
            {
                return impliedVolatilitySurface.dividend[expiryDate]; 
            }

        }

        #endregion

        
        // REPO
        #region Repo Rate

        public Double repoRate
        {
            get
            {
                Console.WriteLine("Warning : Repo not implemented. Add repo component to the dividend rate.");
                return 0.0;
            }
            
        }

        #endregion



        // ************************************************************
        // CLASS PROPERTIES - RATE / DF
        // ************************************************************


        // DISCOUNT FACTOR
        #region Discount Factor

        public Double discountFactor
        {
            get
            {
                return this.impliedVolatilitySurface.DF[this.expiryDate]; 
            }

        }

        #endregion


        // RATE
        #region Rate

        public Double zeroRate
        {
            get
            {
                return -365.25 * Math.Log(discountFactor) / (expiryDate.AddDays(settlementDelay) - pricingDate).TotalDays;
                
            }
        }

        #endregion


        // ************************************************************
        // CLASS PROPERTIES - VOLATILITY
        // ************************************************************

        // IMPLIED VOLATILITY AT STRIKE & MATURITY (SINGLE POINT)
        #region Implied Volatility
    
        public Double impliedVolatility
        {
            get { return this.impliedVolatilitySurface[this.expiryDate][this.strikeMoneyness]; }
            
        }

        #endregion


        // IMPLIED VOLATILITY OBJECT (ENTIRE SURFACE)
        #region Implied Volatility Surface

        protected MarkitSurface _impliedVolatilitySurface;
        public MarkitSurface impliedVolatilitySurface
        {
            get { return _impliedVolatilitySurface; }
            protected set
            {
                _impliedVolatilitySurface = value;
            }
        }

        #endregion



        // ************************************************************
        // CLASS PROPERTIES - CALENDAR / SETTLEMENT
        // ************************************************************

        // SETTLEMENT DELAY
        #region Settlement delay

        protected Double _settlementDelay = 0.0;
        public Double settlementDelay
        {
            get { return _settlementDelay; }
            protected set
            {
                _settlementDelay = value;
            }
        }

        #endregion

        

        // ************************************************************
        // CONSTRUCTORS 
        // ************************************************************

        #region

        // Constructor 1 : Generic
        // public BaseIndexOption() { }


        // Constructor 2 : From given date
        // spublic BaseIndexOption(DateTime pricingDate)  { }

            
        // Constructor 3 : From a Markit volatility surface
        public BaseIndexOption(DateTime pricingDate, DateTime strikeDate, DateTime forwardStartDate, 
            DateTime expiryDate, Double strikeLevel, MarkitSurface surfaceObject)
        {

            // Dates
            this.pricingDate = pricingDate;
            this.strikeDate = strikeDate;
            this.expiryDate = expiryDate;
            this.forwardStartDate = forwardStartDate;

            // Strikes
            this.strikeLevel = strikeLevel;

            // Volatility
            this.impliedVolatilitySurface = surfaceObject;

        }


        #endregion




    }
}

