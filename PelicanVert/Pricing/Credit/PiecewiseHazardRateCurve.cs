

///////  Marc RAYGOT - 2017   ///////
 


using System;
using System.Collections.Generic;
using System.Linq;

namespace QLNet
{
    /*
     JP Morgan. Credit Derivatives: A Primer. JP Morgan Credit Derivatives and Quantitative Research (January 2005)
     D. O\'kane and S. Turnbull. Valuation of Credit Default Swaps. Lehman Brothers Quantitative Credit Research (Apr. 2003)
     Standard CDS Examples. Supporting document for the Implementation of the ISDA CDS Standard Model (Oct. 2012)
    */

    public class PiecewiseHazardRateCurve : DefaultProbabilityTermStructure, InterpolatedCurve, Curve<DefaultProbabilityTermStructure>
    {
        # region new fields: Curve

        public double initialValue() { return _traits_.initialValue(this); }
        public Date initialDate() { return _traits_.initialDate(this); }
        public void registerWith(BootstrapHelper<DefaultProbabilityTermStructure> helper)
        {
            helper.registerWith(this.update);
        }
        public new bool moving_
        {
            get { return base.moving_; }
            set { base.moving_ = value; }
        }
        public void setTermStructure(BootstrapHelper<DefaultProbabilityTermStructure> helper)
        {
            helper.setTermStructure(this);
        }
        protected ITraits<DefaultProbabilityTermStructure> _traits_ = null;//todo define with the trait for yield curve
        public ITraits<DefaultProbabilityTermStructure> traits_
        {
            get
            {
                return _traits_;
            }
        }
        public double minValueAfter(int i, InterpolatedCurve c, bool validData, int first) { return traits_.minValueAfter(i, c, validData, first); }
        public double maxValueAfter(int i, InterpolatedCurve c, bool validData, int first) { return traits_.maxValueAfter(i, c, validData, first); }
        public double guess(int i, InterpolatedCurve c, bool validData, int first) { return traits_.guess(i, c, validData, first); }

        # endregion

        #region InterpolatedCurve
        public List<double> times_ { get; set; }
        public List<double> times() { calculate(); return times_; }

        public List<Date> dates_ { get; set; }
        public List<Date> dates() { calculate(); return dates_; }
        // here we do not refer to the base curve as in QL because our base curve is YieldTermStructure and not Traits::base_curve
        public Date maxDate_ { get; set; }
        public override Date maxDate()
        {
            calculate();
            if (maxDate_ != null)
                return maxDate_;

            return dates_.Last();
        }

        public List<double> data_ { get; set; }
        public List<double> data() { calculate(); return data_; }

        public Interpolation interpolation_ { get; set; }
        public IInterpolationFactory interpolator_ { get; set; }

        public Dictionary<Date, double> nodes()
        {
            calculate();
            Dictionary<Date, double> results = new Dictionary<Date, double>();
            dates_.ForEach((i, x) => results.Add(x, data_[i]));
            return results;
        }

        public void setupInterpolation()
        {
            interpolation_ = interpolator_.interpolate(times_, times_.Count, data_);
        }

        public object Clone()
        {
            InterpolatedCurve copy = this.MemberwiseClone() as InterpolatedCurve;
            copy.times_ = new List<double>(times_);
            copy.data_ = new List<double>(data_);
            copy.interpolator_ = interpolator_;
            copy.setupInterpolation();
            return copy;
        }
        #endregion

        #region BootstrapTraits

        public Date initialDate(DefaultProbabilityTermStructure c) { return traits_.initialDate(c); }
        public double initialValue(DefaultProbabilityTermStructure c) { return traits_.initialValue(c); }
        public bool dummyInitialValue() { return traits_.dummyInitialValue(); }
        public double initialGuess() { return traits_.initialGuess(); }
        public double guess(DefaultProbabilityTermStructure c, Date d) { return traits_.guess(c, d); }
        public double minValueAfter(int s, List<double> l) { return traits_.minValueAfter(s, l); }
        public double maxValueAfter(int s, List<double> l) { return traits_.maxValueAfter(s, l); }
        public void updateGuess(List<double> data, double discount, int i) { traits_.updateGuess(data, discount, i); }
        public int maxIterations() { return traits_.maxIterations(); }

        #region DefaultProbabilityTermStructure implementation
   
        public double hazardRateImpl(double t)
        {
            if (t <= this.times_.Last())
                return this.interpolation_.value(t, true);

            // flat hazard rate extrapolation
            return this.data_.Last();
        }

        protected override double survivalProbabilityImpl(double t)
        {
            if (t == 0.0)
                return 1.0;

            double integral;
            if (t <= this.times_.Last())
            {
                integral = this.interpolation_.primitive(t, true);
            }
            else
            {
                // flat hazard rate extrapolation
                integral = this.interpolation_.primitive(this.times_.Last(), true)
                         + this.data_.Last() * (t - this.times_.Last());
            }
            return Math.Exp(-integral);
        }

        protected override double defaultDensityImpl(double t)
        {
            return hazardRateImpl(t) * survivalProbabilityImpl(t);
        }

        #endregion
        //these are dummy methods (for the sake of ITraits and should not be called directly
        //public double hazardRate(Interpolation i, double t) { return i.value(t, true); }
        public double discountImpl(Interpolation i, double t) { throw new NotSupportedException(); }
        public double zeroYieldImpl(Interpolation i, double t) { throw new NotSupportedException(); }
        public double forwardImpl(Interpolation i, double t) { throw new NotSupportedException(); }
        #endregion

        #region Properties

        protected double _accuracy_;//= 1.0e-12;
        public double accuracy_
        {
            get { return _accuracy_; }
            set { _accuracy_ = value; }
        }

        protected List<CreditDefaultSwapHelper> _instruments_ = new List<CreditDefaultSwapHelper>();
        public List<BootstrapHelper<DefaultProbabilityTermStructure>> instruments_
        {
            get
            {
                //todo edem 
                List<BootstrapHelper<DefaultProbabilityTermStructure>> instruments = new List<BootstrapHelper<DefaultProbabilityTermStructure>>();
                _instruments_.ForEach((i, x) => instruments.Add(x));
                return instruments;
            }
        }

        protected IBootStrap<PiecewiseHazardRateCurve> bootstrapH_;

        #endregion

        #region Constructors

        // two constructors to forward down the ctor chain
        public PiecewiseHazardRateCurve(Date referenceDate, Calendar cal, DayCounter dc,
           List<Handle<Quote>> jumps = null, List<Date> jumpDates = null)
            : base(referenceDate, cal, dc, jumps, jumpDates)
        { }
        public PiecewiseHazardRateCurve(int settlementDays, Calendar cal, DayCounter dc,
           List<Handle<Quote>> jumps = null, List<Date> jumpDates = null)
            : base(settlementDays, cal, dc, jumps, jumpDates)
        { }
        public PiecewiseHazardRateCurve()
            : base()
        { }
    }

    public class PiecewiseHazardRateCurve<Traits, Interpolator, BootStrap> : PiecewiseHazardRateCurve
        where Traits : ITraits<DefaultProbabilityTermStructure>, new()
        where Interpolator : IInterpolationFactory, new()
        where BootStrap : IBootStrap<PiecewiseHazardRateCurve>, new()
    {

        
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments, DayCounter dayCounter)
            : this(referenceDate, instruments, dayCounter, new List<Handle<Quote>>(), new List<Date>(),
                     1.0e-12, new Interpolator(), new BootStrap()) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates)
            : this(referenceDate, instruments, dayCounter, jumps, jumpDates, 1.0e-12, new Interpolator(), new BootStrap()) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps,
                                   List<Date> jumpDates, double accuracy)
            : this(referenceDate, instruments, dayCounter, jumps, jumpDates, accuracy, new Interpolator(), new BootStrap()) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps,
                                   List<Date> jumpDates, double accuracy, Interpolator i)
            : this(referenceDate, instruments, dayCounter, jumps, jumpDates, accuracy, i, new BootStrap()) { }
       
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates,
                                   double accuracy, Interpolator i, BootStrap bootstrap)
            : base(referenceDate, new Calendar(), dayCounter, jumps, jumpDates)
        {

            _instruments_ = instruments;

            accuracy_ = accuracy;
            interpolator_ = i;
            bootstrapH_ = bootstrap;
            _traits_ = new Traits();

            bootstrapH_.setup(this);
        }

        public PiecewiseHazardRateCurve(int settlementDays, Calendar calendar, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates, double accuracy)
            : this(settlementDays, calendar, instruments, dayCounter, jumps, jumpDates, accuracy,
                     new Interpolator(), new BootStrap()) { }
        public PiecewiseHazardRateCurve(int settlementDays, Calendar calendar, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates, double accuracy,
                                   Interpolator i, BootStrap bootstrap)
            : base(settlementDays, calendar, dayCounter, jumps, jumpDates)
        {
            _instruments_ = instruments;
            accuracy_ = accuracy;
            interpolator_ = i;
            bootstrapH_ = bootstrap;
            _traits_ = new Traits();

            bootstrapH_.setup(this);
        }
        #endregion

        // observer interface
        public override void update()
        {
            base.update();
            // LazyObject::update();        // we do it in the TermStructure 
        }

        protected override void performCalculations()
        {
            // just delegate to the bootstrapper
            bootstrapH_.calculate();
        }
    }
    
    public class IterativeBootstrapForHazard : IterativeBootstrap<PiecewiseHazardRateCurve, DefaultProbabilityTermStructure>
    {
    }
    
    // Allows for optional 3rd generic parameter defaulted to IterativeBootstrap
    public class PiecewiseHazardRateCurve<Traits, Interpolator> : PiecewiseHazardRateCurve<Traits, Interpolator, IterativeBootstrapForHazard>
        where Traits : ITraits<DefaultProbabilityTermStructure>, new()
        where Interpolator : IInterpolationFactory, new()
    {

        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments, DayCounter dayCounter)
            : base(referenceDate, instruments, dayCounter) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates)
            : base(referenceDate, instruments, dayCounter, jumps, jumpDates) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates, double accuracy)
            : base(referenceDate, instruments, dayCounter, jumps, jumpDates, accuracy) { }
        public PiecewiseHazardRateCurve(Date referenceDate, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates, double accuracy, Interpolator i)
            : base(referenceDate, instruments, dayCounter, jumps, jumpDates, accuracy, i) { }

        public PiecewiseHazardRateCurve(int settlementDays, Calendar calendar, List<CreditDefaultSwapHelper> instruments,
                                   DayCounter dayCounter)
            : this(settlementDays, calendar, instruments, dayCounter, new List<Handle<Quote>>(), new List<Date>(), 1.0e-12) { }
        public PiecewiseHazardRateCurve(int settlementDays, Calendar calendar, List<CreditDefaultSwapHelper> instruments,
                                      DayCounter dayCounter, List<Handle<Quote>> jumps, List<Date> jumpDates, double accuracy)
            : base(settlementDays, calendar, instruments, dayCounter, jumps, jumpDates, accuracy) { }
    }


}
