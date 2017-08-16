using bOS.Services.CDN.Utils;
using Commons.CDN.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace bOS.Services.CDN
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(WebApiApplication));

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger.Info("Start Application on Date: " + DateTime.Now.ToString("yyyyMMddHHmmss"));
            logger.Info("*******************************************************************");
            logger.Info("*******************************************************************");
            logger.Info("*******************************************************************");
            AuditHelper.Instance.auditLogs.MaxItems = 100;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        void Application_End(object sender, EventArgs e)
        {
            logger.Info("Shutdown Application on Date: " + DateTime.Now.ToString("yyyyMMdd"));
        }


    }
}
