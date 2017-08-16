using bOS.Commons.IO;
using bOS.Commons.Scheduler;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Commons.CDN.Toolbox
{
    public partial class Console : System.Web.UI.Page
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(Console));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SchedulerHelper.IsStarted())
            {
                btnScheduler.Text = "Sospendi";
            }
            else
            {
                btnScheduler.Text = "Avvia";
            }
        }

        protected void btnScheduler_Click(object sender, EventArgs e)
        {
            if (SchedulerHelper.IsStarted())
            {
                logger.Info("Scheduler: Suspending");
                SchedulerHelper.StopScheduler();
                btnScheduler.Text = "Avvia";
            }
            else
            {
                logger.Info("Scheduler: Starting");
                SchedulerHelper.StartScheduler();
                btnScheduler.Text = "Sospendi";
            }
        }

        protected void btnClearCache_Click(object sender, EventArgs e)
        {
            logger.Info("Cache: Clear all objects");
            MemoryCache.Default.Dispose();

            String cacheFolder = bOS.Commons.Configuration.ConfigurationHelper.GetCacheFolderPath();
            logger.Info(String.Format ("Cache: Delete cache folders {0}", cacheFolder));
            FilesHelper.DeleteFiles (cacheFolder, true);
            
        }
    }
}