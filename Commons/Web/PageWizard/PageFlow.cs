using System;
using System.Collections.Generic;
using System.Web.UI;

namespace bOS.Commons.Web.PageWizard
{
    public class PageFlow
    {
        List<PageControl> controls = new List<PageControl>();
        String flowName = String.Empty;

        public PageFlow(String flowName)
        {
            this.flowName = flowName;
        }

        public String GetName() { return this.flowName; }

        public void AddControls(String[] ids)
        {
            foreach (var id in ids)
            {
                AddControl(id);
            }
        }
        public PageControl AddControl(String id)
        {
            PageControl control = new PageControl(id, true);
            controls.Add(control);

            return control;
        }

        public void AddControl(String id, Boolean notInWizard)
        {
            PageControl control = new PageControl(id, true);
            control.NotInWizard = notInWizard;

            controls.Add(control);
        }

        public void OnOff(Control rootControl, bool bOnOff)
        {
            foreach (PageControl control in controls)
            {
                Control controlToReturn = WebHelper.FindControlRecursive(rootControl, control.ControlId);
                if (controlToReturn != null)
                {
                    controlToReturn.Visible = bOnOff;
                }
            }
        }

        public void Register(Control rootControl)
        {
            foreach (PageControl control in controls)
            {
                if (control.NotInWizard)
                {

                }
                else
                {
                    Control controlToReturn = WebHelper.FindControlRecursive(rootControl, control.ControlId);
                    if (controlToReturn != null)
                    {
                        control.ControlVisible = controlToReturn.Visible;
                    }
                }
            }
        }

        public bool AlmosteOneControlEnabled(Control rootControl)
        {
            foreach (PageControl control in controls)
            {
                Control controlToReturn = WebHelper.FindControlRecursive(rootControl, control.ControlId);
                if ((controlToReturn != null) && (controlToReturn.Visible)) return true;
            }

            return false;
        }

        public bool AlmosteOneControlEnabled()
        {
            foreach (PageControl control in controls)
            {
                if (control.ControlVisible) return true;
            }

            return false;
        }

        public void Refresh(Control rootControl)
        {
            foreach (PageControl control in controls)
            {
                Control controlToReturn = WebHelper.FindControlRecursive(rootControl, control.ControlId);
                if (controlToReturn != null)
                {
                    controlToReturn.Visible = control.ControlVisible;
                }
            }
        }


    }
}