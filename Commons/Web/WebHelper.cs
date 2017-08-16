using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace bOS.Commons.Web 
{
    public class WebHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(WebHelper));

        public static Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;

            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        public static Control FindControlRecursive(ControlCollection controls, string controlID)
        {
            foreach (Control controlToSearch in controls)
            {
                Control controlToReturn =
                    FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null) return controlToReturn;
            }
            return null;
        }

        public static void AssignSelectItemByText(HtmlSelect control, String text)
        {
            var i = 0;
            foreach (ListItem item in control.Items)
            {
                if (item.Text == text)
                {
                    control.SelectedIndex = i;
                    return;
                }
                i++;
            }
        }
       
        public static CustomValidator AddCustomValidator(Page Page, Exception err)
        {
            return AddCustomValidator(Page, "summary", err);
        }

        public static CustomValidator AddCustomValidator(Page Page, String validationGroup, Exception err)
        {
            logger.Error(err.StackTrace);

            if ((err.InnerException != null) && (!String.IsNullOrEmpty(err.InnerException.Message)))
            {
                AddCustomValidator(Page, validationGroup, err.InnerException.Message);
                return AddCustomValidator(Page, validationGroup, err.InnerException);
            }
            else
                return AddCustomValidator(Page, validationGroup, err.Message);
        }

        public static CustomValidator AddCustomValidator(Page page, String validationGroup, String message)
        {
            logger.Error(message);
            
            CustomValidator custVal = new CustomValidator();
            custVal.IsValid = false;
            custVal.ErrorMessage = message;
            custVal.EnableClientScript = false;
            custVal.Display = ValidatorDisplay.None;
            custVal.ValidationGroup = validationGroup;
            page.Validators.Add(custVal);
            
            return custVal;

        }
        
        public static CustomValidator AddCustomValidator(Page page, String message)
        {
            return AddCustomValidator(page, "summary", message);
        }

        public static DateTime? ManageDate(Page page, String date, String errorMessage)
        {
            try
            {
                return DateTime.ParseExact(date, "dd/MM/yyyy", null);
            }
            catch (Exception err)
            {
                String message = errorMessage;
                WebHelper.AddCustomValidator(page, message);
                logger.Error(message, err);

                return null;
            }
        }

        public static DateTime? ManageDateTime(Page page, String date, String errorMessage)
        {
            try
            {
                return DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", null);
            }
            catch (Exception err)
            {
                String message = errorMessage;
                WebHelper.AddCustomValidator(page, message);
                logger.Error(message, err);

                return null;
            }
        }
    }
}