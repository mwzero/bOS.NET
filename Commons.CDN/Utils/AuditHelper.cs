using Commons.CDN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Services.CDN.Utils
{
    public class AuditHelper
    {
        private static AuditHelper instance;
        public bOS.Commons.Collections.LimitedList<Audit> auditLogs;

        private AuditHelper() 
        { 
            auditLogs = new bOS.Commons.Collections.LimitedList<Audit>();
            auditLogs.MaxItems = 100;
        }

        public static AuditHelper Instance {
            get {
                if ( instance == null )
                    instance = new AuditHelper();

                return instance;
            }
        }
    }
}