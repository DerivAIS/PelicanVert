

///////  Marc RAYGOT - 2017   ///////



using System.Collections.Generic;
using System.Linq;
using System;

namespace QLNet
{

    public abstract class GenericScriptInstrument : Instrument
    {
        #region Attributs
        protected Dictionary<string, List<double>> indexDico_;
        protected Dictionary<string, List<Date>> datesDico_;

        protected Date maturity_;

        static public SortedDictionary<string, double> inspout_Static;
        protected double samples_;


        public Arguments arguments;
        public Results results;
        public MCGenericScriptEngineBase engine;



        protected Func<GenericScriptInstrument.Script.ScriptFunctions,GenericScriptInstrument.Script.ScriptData, double> scriptFunc_;
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


        protected void setupScript(Func<GenericScriptInstrument.Script.ScriptFunctions, GenericScriptInstrument.Script.ScriptData, double> scriptFunc)
        {
            scriptFunc_ = scriptFunc;
        }

        public override void setupArguments(IPricingEngineArguments args)
        {
            GenericScriptInstrument.Arguments arguments = args as GenericScriptInstrument.Arguments;
            Utils.QL_REQUIRE(arguments != null, () => "wrong argument type");


            arguments.scriptFunc = scriptFunc_;
            arguments.datesDico = datesDico_;
            arguments.indexDico = indexDico_;
            arguments.maturity = maturity_;
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



        public void inspout(int decimals = 10, bool percent = false)
        {

            
            List<string> keyList = new List<string>(inspout_Static.Keys);
            int width = 5;
            double val = 0.0;
            string format = "0.";

            for (int i = 0; i < decimals; i++)
                format = string.Concat(format, "0");

            if (percent)
                format = string.Concat(format, "%");

            foreach (string key in keyList)
            {
                width = Math.Max(width, key.Length);
            }

            foreach (string key in keyList)
            {
                Console.Write("{0,-" + width + "}", key);
                Console.Write(" = ");
                val = inspout_Static[key] / samples_;
                val.ToString(format);
                Console.WriteLine(val.ToString(format));
            }


        }

        public double inspout(string varName)
        {
            if (!inspout_Static.ContainsKey(varName)) 
                return 0.0;
            return inspout_Static[varName] / samples_;
        }


        #endregion

        #region Constructor

        public GenericScriptInstrument(params IDictionary<string, object>[] arg)
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
            inspout_Static = new SortedDictionary<string, double>();

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
            public Script script;
            public Func<GenericScriptInstrument.Script.ScriptFunctions, GenericScriptInstrument.Script.ScriptData, double> scriptFunc;

            public Date maturity;

            public virtual void validate()
            {

            }
        }


        new public class Results : Instrument.Results
        {
            public double samples;
            public Dictionary<string, double> inspout = new Dictionary<string, double>();

            public override void reset()
            {
                base.reset();
                samples = 0.0;
            }

        }



        public class MCGenericScriptEngineBase : GenericEngine<GenericScriptInstrument.Arguments, GenericScriptInstrument.Results> { }

        public class Script
        {

            protected Handle<YieldTermStructure> discountTS_;
            protected Handle<YieldTermStructure> dividendTS_;
            protected Handle<BlackVolTermStructure> volatilityTS_;
            protected Handle<DefaultProbabilityTermStructure> creditTS_;
            protected MultiPath multipath_;
            protected Func<ScriptFunctions, ScriptData,double> scriptFunc_;


            protected ScriptFunctions scriptfunctions_;
            protected ScriptData scriptdata_;

            public Script() { }

            public Script(Handle<YieldTermStructure> discountTS,
                          Handle<DefaultProbabilityTermStructure> creditTS,
                          Handle<YieldTermStructure> dividendTS,
                          Handle<BlackVolTermStructure> volatilityTS,
                          Dictionary<string, List<double>> timeDico,
                          Dictionary<string, List<double>> indexDico,
                          Func<ScriptFunctions, ScriptData,double> scriptFunc)
            {
                discountTS_ = discountTS;
                dividendTS_ = dividendTS;
                volatilityTS_ = volatilityTS;
                creditTS_ = creditTS;
                scriptdata_ = new ScriptData(timeDico, indexDico);
                scriptFunc_ = scriptFunc;
            }
             
            public void setScriptFunctions(IPath multipath)
            {
                string type = multipath.GetType().ToString();
                if (type == "QLNet.MultiPath")
                {
                    multipath_ = (MultiPath)multipath;
                }
                else if(type == "QLNet.Path")
                {
                    multipath_ = new MultiPath(new List<Path>() { (Path)multipath });
                }
                
               else
                {
                    Utils.QL_FAIL("Path or MultiPath needed");
                }

                scriptfunctions_ = new ScriptFunctions(discountTS_, dividendTS_, volatilityTS_, creditTS_, multipath_);

            }


            public double scriptPayoff()
            {
                return scriptFunc_(scriptfunctions_, scriptdata_);
            }


            public class ScriptData
            {
                protected Dictionary<string, List<double>> timeDico_;
                protected Dictionary<string, List<double>> indexDico_;
                protected List<string> keyList;

                public ScriptData(Dictionary<string, List<double>> timeDico,
                                  Dictionary<string, List<double>> indexDico)
                {
                    timeDico_ = timeDico;
                    keyList = new List<string>(timeDico_.Keys);
                    foreach(string key in keyList)
                        for (int i = 0; i < timeDico_[key].Count; i++)
                        timeDico_[key][i] = Math.Round(timeDico_[key][i], 8);

                    indexDico_ = indexDico;
                }

                public List<double> INDEX (string value)
                {
                    return indexDico_[value];
                }

                public List<double> TIME(string value)
                {
                    return timeDico_[value];
                }

                public double ENDTIME(string value)
                {
                    return timeDico_[value][timeDico_[value].Count-1];
                }

            }


            public class ScriptFunctions
            {
                protected Handle<DefaultProbabilityTermStructure> creditTS_;
                protected Handle<YieldTermStructure> discountTS_;
                protected Handle<YieldTermStructure> dividendTS_;
                protected Handle<BlackVolTermStructure> volatilityTS_;
                protected MultiPath multipath_;
                

                public ScriptFunctions(Handle<YieldTermStructure> discountTS,
                                       Handle<YieldTermStructure> dividendTS,
                                       Handle<BlackVolTermStructure> volatilityTS,
                                       Handle<DefaultProbabilityTermStructure> creditTS, 
                                       MultiPath multipath)
                {
                    discountTS_ = discountTS;
                    dividendTS_ = dividendTS;
                    volatilityTS_ = volatilityTS;
                    creditTS_ = creditTS;
                    multipath_ = multipath;

                }

                public double DISCOUNT(double t = -1)
                {                   
                    if (t == -1)
                        return discountTS_.link.discount(multipath_[0].time(PATHNB()-1), true);

                    return discountTS_.link.discount(t, true); ;
                }


                public double DIVIDEND(double t = -1)
                {
                    if (t == -1)
                        return dividendTS_.link.discount(multipath_[0].time(PATHNB()-1), true);

                    return dividendTS_.link.discount(t, true); ;
                }


                public double PATHVALUE(int t = -1)
                {
                    if (t == -1)
                        return multipath_[0].value(PATHNB()-1);

                    return multipath_[0].value(t); ;
                }

                public double PATHTIME(int t = -1)
                {
                    if (t == -1)
                        return Math.Round(multipath_[0].time(PATHNB()-1),8);

                    return Math.Round(multipath_[0].time(t),8) ;
                }


                public int PATHNB()
                {
                   
                    return multipath_[0].length();
                }


                public void INSPOUT(string varName, double varValue)
                {
                    

                    if (inspout_Static.ContainsKey(varName))
                    {
                        inspout_Static[varName] = (double)inspout_Static[varName] + varValue;
                    }
                    else
                    {
                        inspout_Static.Add(varName, varValue);

                    }
                }

                public double SURVIVALPB(double t)
                {

                    return creditTS_.link.survivalProbability(t);
                }

                public double DIVIDENDRATE(double T, double t = 0.0)
                {
                    return -Math.Log(DIVIDEND(T) / DIVIDEND(t)) / (T - t);
                }

                public double RISKFREERATE(double T, double t = 0.0)
                {
                    return -Math.Log(DISCOUNT(T) / DISCOUNT(t)) / (T - t);
                }

                public double VOLATILITY(double t,double k)
                {
                    return volatilityTS_.link.blackVol(t,k,false);
                }

                public double OPTIONPRICE(double T, double t, double k, double s0, double d, double r, double sigma, Option.Type type )
                {

                    if (type == Option.Type.Call)
                    {
                        CumulativeNormalDistribution nic = new CumulativeNormalDistribution();
                        return + s0 * Math.Exp(-d*(T-t)) * nic.value(d1(T, t, k, s0, d, r, sigma)) 
                                - k * Math.Exp(-r*(T-t)) * nic.value(d2(T, t, k, s0, d, r, sigma));
                    }

                    else if (type == Option.Type.Put)
                    {
                        CumulativeNormalDistribution nic = new CumulativeNormalDistribution();
                        return - s0 * Math.Exp(-d * (T - t)) * nic.value( - d1(T, t, k, s0, d, r, sigma))
                                + k * Math.Exp(-r * (T - t)) * nic.value( - d2(T, t, k, s0, d, r, sigma));
                    }
                    else throw new NotImplementedException();

                }

                public double BINARYPRICE(double T, double t, double k, double s0, double d, double r, double sigma, Option.Type type)
                {

                    if (type == Option.Type.Call)
                    {
                        CumulativeNormalDistribution nic = new CumulativeNormalDistribution();
                        return Math.Exp(-r * (T - t)) * nic.value(d2(T, t, k, s0, d, r, sigma));
                    }

                    else if (type == Option.Type.Put)
                    {
                        CumulativeNormalDistribution nic = new CumulativeNormalDistribution();
                        return Math.Exp(-r * (T - t)) * nic.value(-d2(T, t, k, s0, d, r, sigma));
                    }
                    else throw new NotImplementedException();

                }



                private double d1(double T, double t, double k, double s0, double d, double r, double sigma)
                {
                    return 1 / (sigma * Math.Sqrt(T - t)) * (Math.Log(s0 / k) + (r - d + 0.5 * sigma * sigma) * (T - t));
                }

                private double d2(double T, double t, double k, double s0, double d, double r, double sigma)
                {
                    return d1( T,  t,  k,  s0,  d,  r,  sigma)- sigma * Math.Sqrt(T - t);
                }


            }


        }


        #endregion
    }



  
}