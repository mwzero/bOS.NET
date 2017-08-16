using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace bOS.Commons
{
    public class SessionHelper
    {

        protected static readonly ILog logger = LogManager.GetLogger(typeof(SessionHelper));


        public static void SetTabSelected(HttpSessionState Session, int tab)
        {
            Session["TabSelected"] = tab;
        }

        public static int GetTabSelected(HttpSessionState Session)
        {
            if (Session["TabSelected"] == null)
                return -1;
            else
                return (int)Session["TabSelected"];
        }

        #region Messages
        public static String MESSAGE_INFO = "MessageInfo";
        public static String MESSAGE_WARNING = "MessageWarning";
        #endregion

        #region User, Role, ACL 
        public static String USER_ATTRIBUTE = "User";
        public static String ROLE_ATTRIBUTE = "Role";
        public static String ACL_ATTRIBUTE = "ACL";
        #endregion

        #region Crud
        public static String CRUD_ATTRIBUTE = "Crud";
        public static String CRUDTYPE_ATTRIBUTE = "CrudType";
        #endregion

        #region Project
        public static String REQUEST_ATTRIBUTE = "Request";
        public static String REQUEST_DOCS_ATTRIBUTE = "Docs";
        public static String REQUEST_COMMENTS_ATTRIBUTE = "Comments";

        public static String REQUEST_LIST_FILTER = "RequestFilter";

        #endregion

        #region
        public static String DASHBOARD_PROJECT_ATTRIBUTE = "InDashboard";
        #endregion


        public static void ManageInfoMessage(HttpSessionState Session,Panel panel)
        {
            String msg = (String)Session[MESSAGE_INFO]; 
            if ( !String.IsNullOrEmpty(msg) ) {
                Literal ltl = new Literal();
                ltl.Text = msg;
                panel.Controls.Add(ltl);
                Session.Remove(MESSAGE_INFO);
            }

            msg = (String)Session[MESSAGE_WARNING];
            if (!String.IsNullOrEmpty(msg))
            {
                Literal ltl = new Literal();
                ltl.Text = msg;
                panel.Controls.Add(ltl);
                Session.Remove(MESSAGE_WARNING);
            }
        }

        public static void AddInfoMessage(HttpSessionState Session,String message)
        {
            Session[MESSAGE_INFO] = message;
        }

        public static void AddWarningMessage(HttpSessionState Session, String message)
        {
            Session[MESSAGE_WARNING] = message;
        }

        public static void RemoveDashboardAttribute(HttpSessionState Session)
        {
            Session.Remove(SessionHelper.DASHBOARD_PROJECT_ATTRIBUTE);
        }

        public static void RemoveRequestAttribute(HttpSessionState Session)
        {
            Session.Remove(SessionHelper.REQUEST_ATTRIBUTE);
        }

        public static void RemoveUserAttribute(HttpSessionState Session)
        {
            Session.Remove(SessionHelper.USER_ATTRIBUTE);
        }

        public static void RemoveRoleAttribute(HttpSessionState Session)
        {
            Session.Remove(SessionHelper.ROLE_ATTRIBUTE);
        }



        
    }
}