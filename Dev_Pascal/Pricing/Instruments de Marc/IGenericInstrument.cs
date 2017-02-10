using System.Collections.Generic;
using System.Linq;
using System;
//using System.Linq.Expressions;

namespace QLNet
{

    public class GenericInstrument : Instrument
    {
        #region Attributs
        protected Dictionary<string, List<double>> indexDico_;
        protected Dictionary<string, List<Date>> datesDico_;

        protected Date maturity_;
        protected Func<Dictionary<string, List<double>>, Dictionary<string, List<double>>, Handle<YieldTermStructure>, Path, double> scriptDico_;

        protected Dictionary<string, double> inspout_;
        protected double samples_;

        public Arguments arguments;
        public Results results;
        public MCGenericEngineBase engine;

        #endregion

        # region Interface

        public static Dictionary<string, Object> BuildDico(string varName, Object varValues)
        {
            Dictionary<string, Object> dico = new Dictionary<string, Object>();
            dico.Add(varName, varValues);
            return dico;
        }

        public override bool isExpired()
        {
            Date today = Settings.evaluationDate();
            if (today >= maturity_)
                return true;
            else
                return false;
        }


        public virtual double ScriptDico(Dictionary<string, List<double>> timeDico,
                             Dictionary<string, List<double>> indexDico,
                             Handle<YieldTermStructure> discountTS,
                             Path path)
        {
            return 0.0;
        }



        public override void setupArguments(IPricingEngineArguments args)
        {
            GenericInstrument.Arguments arguments = args as GenericInstrument.Arguments;
            Utils.QL_REQUIRE(arguments != null, () => "wrong argument type");

            arguments.scriptDico = scriptDico_;
            arguments.datesDico = datesDico_;
            arguments.indexDico = indexDico_;
            arguments.maturity = maturity_;
            // arguments.inspout = inspout_;
        }


        public double samples()
        {
            return samples_;
        }


        public override void fetchResults(IPricingEngineResults r)
        {
            base.fetchResults(r);

            Results results = r as Results;
            if (results == null)
                throw new Exception("no results from pricing engine");
            samples_ = results.samples;

        }

        #endregion

        #region TEST


        protected void INSPOUT(string varName, double varValue)
        {
            List<double> tempList = new List<double>();

            if (inspout_.ContainsKey(varName))
            {
                //tempList = (List<double>)inspout_[varName];
                //tempList.Add(varValue);
                inspout_[varName] = (double)inspout_[varName] + varValue;
            }
            else
            {
                inspout_.Add(varName, varValue);

            }
            //inspout_ = additionalResults_;*/
        }



        public double inspout(string varName)
        {
            if (!inspout_.ContainsKey(varName)) throw new Exception("this value is not defined : " + varName);
            return inspout_[varName] / samples_;
        }


        #endregion

        #region Constructor

        public GenericInstrument(params IDictionary<string, object>[] arg)
        {
            List<string> keyList = new List<string>();
            Dictionary<string, object> Dico_ = new Dictionary<string, object>();
            Dictionary<string, List<double>> indexDico = new Dictionary<string, List<double>>();
            Dictionary<string, List<Date>> datesDico = new Dictionary<string, List<Date>>();


            foreach (Dictionary<string, object> Dico in arg)
            {
                Dico_ = Dico;
                keyList = new List<string>(Dico_.Keys);
                if (Dico_[keyList[0]] is List<double>)
                {
                    foreach (string Key in keyList)
                        indexDico.Add(Key, (List<double>)Dico_[Key]);
                }
                else if (Dico_[keyList[0]] is double)
                {
                    foreach (string Key in keyList)
                        indexDico.Add(Key, new InitializedList<double>() { (double)Dico_[Key] });
                }
                else if (Dico_[keyList[0]] is List<Date>)
                {
                    foreach (string Key in keyList)
                        datesDico.Add(Key, (List<Date>)Dico_[Key]);
                }
                else if (Dico_[keyList[0]] is Date)
                {
                    foreach (string Key in keyList)
                        datesDico.Add(Key, new InitializedList<Date>() { (Date)Dico_[Key] });
                }
                else
                    throw new Exception("this type is not supported : " + Dico.GetType());
            }


            /// feed Attributs
            datesDico_ = datesDico;
            indexDico_ = indexDico;
            scriptDico_ = (Func<Dictionary<string, List<double>>, Dictionary<string, List<double>>, Handle<YieldTermStructure>, Path, double>)ScriptDico;
            inspout_ = new Dictionary<string, double>();

            /// get maturity date

            maturity_ = Settings.evaluationDate();

            keyList = new List<string>(datesDico_.Keys);
            foreach (string Key in keyList)
            {
                foreach (Date date in datesDico_[Key])
                {
                    if (date > maturity_)
                        maturity_ = date;
                }
            }
        }



        #endregion

        #region Arguments,results, engine

        public class Arguments : IPricingEngineArguments
        {

            public Dictionary<string, List<Date>> datesDico;
            public Dictionary<string, List<double>> indexDico;
            public Func<Dictionary<string, List<double>>, Dictionary<string, List<double>>, Handle<YieldTermStructure>, Path, double> scriptDico;
            //public Dictionary<string, double> inspout;
            public Date maturity;
            public virtual void validate()
            {

            }
        }


        new public class Results : Instrument.Results
        {
            public double samples;
            // public Dictionary<string, double> inspout = new Dictionary<string, double>();

            public override void reset()
            {
                base.reset();
                samples = 0.0;
            }

        }




        public class MCGenericEngineBase : GenericEngine<GenericInstrument.Arguments, GenericInstrument.Results> { }

        #endregion
    }
}