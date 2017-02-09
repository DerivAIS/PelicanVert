using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace QLyx.DataIO
{


    // CACHE MANAGEMENT CLASS
    public sealed class CacheManager : MemoryCache
    {


        // ************************************************************
        // PRIVATE CONSTRUCTORS 
        // ************************************************************

        private CacheManager() : base("myCache") { }

        static readonly object padlock = new object();



        // ************************************************************
        // STATIC PROPERTIES 
        // ************************************************************

        public static CacheManager Instance { get { return Nested.instance; } }


        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly CacheManager instance = new CacheManager();

        }



        // ************************************************************
        // METHODS
        // ************************************************************

        public void AddItem(string key, object value)
        {
            base.Add(key, value, DateTime.MaxValue);
        }



        public object GetItem(string key)
        {
            return base.Get(key);
        }



        public object PopItem(string key, bool remove)
        // protected virtual object GetItem(string key, bool remove)
        {
            lock (padlock)
            {
                var res = base.Get(key);

                if (res != null)
                {
                    if (remove == true)
                        base.Remove(key);
                }
                else
                {
                    WriteToLog("CachingProvider-GetItem: Don't contains key: " + key);
                }

                return res;
            }
        }


        // ************************************************************
        // ERROR LOGS
        // ************************************************************

        #region Error Logs

        string LogPath = System.Environment.GetEnvironmentVariable("TEMP");

        private void DeleteLog()
        {
            System.IO.File.Delete(string.Format("{0}\\CachingProvider_Errors.txt", LogPath));
        }

        private void WriteToLog(string text)
        {
            using (System.IO.TextWriter tw = System.IO.File.AppendText(string.Format("{0}\\CachingProvider_Errors.txt", LogPath)))
            {
                tw.WriteLine(text);
                tw.Close();
            }
        }

        #endregion




    }


}
