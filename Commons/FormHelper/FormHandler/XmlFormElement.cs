using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace bOS.Commons.FormHelper.FormHandler
{
    public class XmlFormElement 
    {
        XmlNode node;
        public XmlNode Node
        {
            get { return this.node; }
        }
        public XmlFormElement(XmlNode node)
        {
            this.node = node;
        }
    }
}   