using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruscottoBNL.Utils
{
    public class ErrorModule : IHttpModule
    {

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(OnError);
        }

        void OnError(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpException ex = app.Server.GetLastError() as HttpException;
            if (app.User.IsInRole( "Amministratore") )
            {
                app.Response.Clear();
                app.Response.TrySkipIisCustomErrors = true;
                app.Response.Write(
                string.Format("<h1>This error is only visible" +
                " to '{0}' members.</h1>", "Amministratore"));
                app.Response.Write(ex.GetHtmlErrorMessage());
                app.Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}