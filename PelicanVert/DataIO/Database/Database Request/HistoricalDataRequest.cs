using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QLNet;

namespace QLyx.DataIO
{
    public class HistoricalDataRequest : GenericDataRequest
    {


        // ************************************************************
        // CONSTRUCTOR
        // ************************************************************


        public HistoricalDataRequest(IDtoken Id_, List<string> Fields_, DateTime StartDate_, DateTime EndDate_, 
            TimeUnit Periodicity_, string Source_)

            : base(Id_: Id_, Fields_: Fields_, StartDate_: StartDate_,
                       EndDate_: EndDate_, Periodicity_: Periodicity_, Source_: Source_) { }


        public HistoricalDataRequest(IDtoken Id_, List<string> Fields_, DateTime StartDate_, DateTime EndDate_)

            : this(Id_: Id_, Fields_: Fields_, StartDate_: StartDate_,
                       EndDate_: EndDate_, Periodicity_: TimeUnit.Days, Source_: "Bloomberg") { }


        public HistoricalDataRequest(IDtoken Id_, DateTime StartDate_, DateTime EndDate_)

            : this(Id_: Id_, Fields_: new List<string>(), StartDate_: StartDate_,
                       EndDate_: EndDate_, Periodicity_: TimeUnit.Days, Source_: "Bloomberg") { }


        public HistoricalDataRequest(IDtoken Id_, DateTime StartDate_)

            : this(Id_: Id_, Fields_: new List<string>(), StartDate_: StartDate_,
                       EndDate_: DateTime.Today, Periodicity_: TimeUnit.Days, Source_: "Bloomberg") { }


        public HistoricalDataRequest(IDtoken Id_)

            : this(Id_: Id_, Fields_: new List<string>(), StartDate_: new DateTime(2013,12,31),
                       EndDate_: DateTime.Today, Periodicity_: TimeUnit.Days, Source_: "Bloomberg") { }


    }
}
