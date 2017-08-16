using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Commons.Web.PageWizard
{
    public class PageControl
    {
        private String _controlId;
        public String ControlId
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        private Boolean _controlVisible;
        public Boolean ControlVisible
        {
            get { return _controlVisible; }
            set { _controlVisible = value; }
        }

        private Boolean _notInWizard;
        public Boolean NotInWizard
        {
            get { return _notInWizard; }
            set { _notInWizard = value; }
        }

        public PageControl(String controlId, Boolean visible) 
        {
            ControlId = controlId;
            ControlVisible = visible;
        }
    }
}