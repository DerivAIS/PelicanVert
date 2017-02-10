using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace Pascal.Instruments.Derivatives
{
    public class AutocallExercise : EarlyExercise
    {

        // ************************************************************
        // PROPERTIES
        // ************************************************************

        // ENUMERATOR PROPERTIES 
        private int Position = -1;



        // ************************************************************
        // CONSTRUCTORS
        // ************************************************************

        public AutocallExercise(List<Date> dates) : this(dates, false) { }

        public AutocallExercise(List<Date> dates, bool payoffAtExpiry)
            : base(Type.Bermudan, payoffAtExpiry)
        {

            if (dates.Count == 0)
                throw new ApplicationException("no exercise date given");

            dates_ = dates;
            dates_.Sort();
        }




        // ************************************************************
        // ENUMERATOR-RELATED METHODS
        // ************************************************************

        #region Enumerators


        public IEnumerator GetEnumerator()
        {
            //return (IEnumerator)this;
            return dates_.GetEnumerator();
        }
        
        public bool MoveNext()
        {

            if (Position < dates_.Count - 1)
            {
                ++Position;
                return true;
            }
            return false;
        }
        
        public void Reset()
        {
            Position = -1;
        }
        
        public object Current
        {
            get
            {
                DateTime key = dates_.ElementAt(Position);
                return key;
            }
        }



        #endregion



    }
}
