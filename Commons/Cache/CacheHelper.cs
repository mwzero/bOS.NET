using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Runtime.Caching;

namespace bOS.Commons.Cache
{
    public class CacheHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(CacheHelper));
        
        public static bool Contains (string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        public static void Add(String key, Object value)
        {
            MemoryCache.Default.Add(key, value, DateTimeOffset.MaxValue);
        }

        /*
         * CacheItemPolicy policy = new CacheItemPolicy
            {
                ChangeMonitors = {
		            new HostFileChangeMonitor(new List<String> {
			            fileCache
		            })
	            }
            };
            policy.RemovedCallback += delegate { 
                Console.WriteLine("Removed"); 
            };
         */
        public static void Add(String key, Object value, CacheItemPolicy policy)
        {
            MemoryCache.Default.Add(key, value, policy);
        }

        public static void Add(String key, Object value, DateTimeOffset dtOffset)
        {
            MemoryCache.Default.Add(key, value, dtOffset);
        }

        public static T Get<T>(String key)
        {
            return (T)MemoryCache.Default[key];
        }

        public static Object Get(String key)
        {
            return MemoryCache.Default[key];
        }

        public static void Remove(String key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}