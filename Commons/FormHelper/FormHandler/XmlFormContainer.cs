using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace bOS.Commons.FormHelper.FormHandler
{
    public class XmlFormContainer
    {
        public List<XmlFormElement> items = new List<XmlFormElement>();
        public List<XmlFormElement> Items
        {
            get { return this.items; }
        }

        XmlNode node;
        public XmlNode Node
        {
            get { return this.node; }
        }

        public XmlFormContainer(XmlNode node)
        {
            this.node = node;
        }

        public void AddItem ( XmlNode node )
        {
            /*
            //check if a label is relative to a textbox
            if ( node.Attributes["ControlloTipo"].InnerText == "0" )
            {
                String[] span = node.Attributes["Span"].InnerText.Split(';');
                if ( span.Count() > 1 )
                {
                    foreach (XmlFormElement element in items)
                    {

                    }
                }
                else
                    items.Add(new XmlFormElement(node));
            }
            else*/
                items.Add( new XmlFormElement(node));
        }

        public void ReorderItems()
        {

        }
    }
}