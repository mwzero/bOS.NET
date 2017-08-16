using bOS.Commons;
using bOS.Commons.Configuration;
using bOS.Commons.Mail;

using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace bOS.Services.CDN.Jobs
{
    public class CopyDocuments2MyDocument : IJob
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(CopyDocuments2MyDocument)); 

        public void Execute(IJobExecutionContext context)
        {
            logger.Debug("Copy files to MyDocument");
        }
    }
}