using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using bOS.Commons;
using System.Web.UI.HtmlControls;
using bOS.Commons.Configuration;
using Commons.CDN.Utils;
using bOS.Services.CDN.Utils;

namespace bOS.Services.CDN
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack )
            {
            } 
            else
            {
                gridAuditLogs.DataSource = AuditHelper.Instance.auditLogs.GetList();
                gridAuditLogs.DataBind();
           
            }
        }

    }
}