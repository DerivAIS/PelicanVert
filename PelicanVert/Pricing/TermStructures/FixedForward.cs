

///////  Marc RAYGOT - 2017   ///////


namespace QLNet
{
    //! Fixed value curve (designed for fixed dividend)
    public class FixedForward : YieldTermStructure
    {
        private double fixedDiv_;
        private double initialQuote_;


        // Constructor
        public FixedForward(Date referenceDate, double fixedDiv, double initialQuote, DayCounter dayCounter):
                base(referenceDate, new Calendar(), dayCounter)
        {
            fixedDiv_ = fixedDiv;
            initialQuote_ = initialQuote;
        }

        // TermStructure interface
        public override Date maxDate() { return Date.maxDate(); }

        protected override double discountImpl(double t)
        {
            return (1- fixedDiv_/ initialQuote_ * t);
        }

    }
}
