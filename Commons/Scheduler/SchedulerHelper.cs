using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz.Impl;
using Quartz;

namespace bOS.Commons.Scheduler
{
    public class SchedulerHelper
    {
        static IScheduler scheduler = null;

        public static void StartScheduler()
        {
            scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();
        }

        public static void StopScheduler()
        {
            scheduler.Shutdown();
        }

        public static Boolean IsStarted()
        {
            if (scheduler == null)
                return false;

            return scheduler.IsStarted;
        }
    }
}