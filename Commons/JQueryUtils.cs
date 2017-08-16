using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace bOS.Commons
{
    public class JQueryUtils
    {
        public static void RegisterTextBoxForDatePicker(Page page,
                           params TextBox[] textBoxes)
        {
            RegisterTextBoxForDatePicker(page, "dd/mm/yy", textBoxes);
        }

        public static void RegisterTextBoxForDatePicker(Page page,
                           params HtmlInputText[] textBoxes)
        {
            RegisterTextBoxForDatePicker(page, "dd/mm/yy", textBoxes);
        }

        public static void RegisterTextBoxForDatePicker(Page page,
               string format, params HtmlInputText[] textBoxes)
        {
            bool allTextBoxNull = true;
            foreach (HtmlInputText textBox in textBoxes)
            {
                if (textBox != null) allTextBoxNull = false;
            }
            if (allTextBoxNull) return;
            /*
            page.ClientScript.RegisterClientScriptInclude(page.GetType(), 
                 "jquery", "~/Scripts/jquery-1.8.2.js");
            page.ClientScript.RegisterClientScriptInclude(page.GetType(), 
                 "jquery.ui.all", "~/Scripts/jquery-ui.js");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), 
                 "datepickerCss", 
                 "<link  rel=\"stylesheet\" href=\"JQuery/themes/" + 
                 "flora/flora.datepicker.css\" />");
            */
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(function() {");
            foreach (HtmlInputText textBox in textBoxes)
            {
                if (textBox != null)
                {
                    sb.Append("$('#" + textBox.ClientID + "').datepicker({dateFormat: \"" + format + "\"});");
                    sb.Append("$('#" + textBox.ClientID + "').datepicker('option', $.datepicker.regional[ 'it' ] );");

                    //sb.Append("$('#" + textBox.ClientID + "').datepicker(\"show\");");
                }
            }

            sb.Append("});");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(),
                 "jQueryScript", sb.ToString(), true);
        }

        public static void RegisterTextBoxForDatePicker(Page page,
               string format, params TextBox[] textBoxes)
        {
            bool allTextBoxNull = true;
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox != null) allTextBoxNull = false;
            }
            if (allTextBoxNull) return;
            /*
            page.ClientScript.RegisterClientScriptInclude(page.GetType(), 
                 "jquery", "~/Scripts/jquery-1.8.2.js");
            page.ClientScript.RegisterClientScriptInclude(page.GetType(), 
                 "jquery.ui.all", "~/Scripts/jquery-ui.js");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), 
                 "datepickerCss", 
                 "<link  rel=\"stylesheet\" href=\"JQuery/themes/" + 
                 "flora/flora.datepicker.css\" />");
            */
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(function() {");
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox != null)
                {
                    sb.Append("$('#" + textBox.ClientID + "').datepicker({dateFormat: \"" + format + "\"});");
                    sb.Append("$('#" + textBox.ClientID + "').datepicker('option', $.datepicker.regional[ 'it' ] );");
                    
                    //sb.Append("$('#" + textBox.ClientID + "').datepicker(\"show\");");
                }
            }

            sb.Append("});");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(),
                 "jQueryScript", sb.ToString(), true);
        }

        public static void SelectTab(Page page,String tab)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(function() {");
            sb.Append(String.Format("$('#tabs').tabs('select', '{0}');", tab));
            sb.Append("});");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(),
                 "jQueryScriptTab", sb.ToString(), true);
        }
                    
    }
}