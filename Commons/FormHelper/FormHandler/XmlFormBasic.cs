using bOS.Commons.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace bOS.Commons.FormHelper.FormHandler
{
    public class XmlFormBasic : XmlForm
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(XmlFormBasic));

        public XmlFormBasic() : base()
        {
        }

        public override void InitializeControlsAfterLoad()
        {
            ;
        }

        public override void Render(
            Mode mode, 
            ControlPosition position, 
            int deltax, 
            int deltay, 
            ControlCollection controls, 
            List<DataTable> ds)
        {
            formulaHelper = new FormulaHelper(ds);
            XmlNodeList formControls = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in formControls)
            {


                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        WebControl control = null;
                        string controlloTipo = node.Attributes["ControlloTipo"].InnerText;
                        if (controlloTipo == "10")
                        {
                            Boolean bNotAvoidWidth = true;

                            control = addTabPanel(position, node, ds);

                            bNotAvoidWidth = false;

                            if (control == null) return;


                            control.Style.Add("position", position.GetString());

                            control.Style.Add("left", (int.Parse(node.Attributes["x"].InnerText) + deltax) + "px");
                            control.Style.Add("top", (int.Parse(node.Attributes["y"].InnerText) + deltay) + "px");

                            control.Style.Add("padding", "0 0 0 0");
                            control.Style.Add("margin", "0 0 0 0");

                            if ((bNotAvoidWidth) && (node.Attributes["w"] != null))
                            {
                                control.Style.Add("width", node.Attributes["w"].InnerText + "px");

                            }


                        }
                        else
                        {
                            control = addWebControl(position, controls, node, deltax, deltay, ds == null || ds.Count == 0 || ds[0].Rows.Count == 0 ? null : ds[0].Rows[0], String.Empty);
                        }


                        if (control != null)
                        {
                            if (mode == Mode.READ)
                                control.Enabled = false;

                            controls.Add(control);
                        }

                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(node.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + node.Name);
                        Console.WriteLine(">");
                        break;
                }
            }
        }

        private WebControl addWebControl(ControlPosition position, ControlCollection controls, XmlNode node, int deltaX, int deltaY, DataRow row, String fatherNode)
        {
            WebControl control = null;

            string controlloTipo = node.Attributes["ControlloTipo"].InnerText;

            Boolean bNotAvoidWidth = true;

            if (controlloTipo == "2") //TextBox3
            {
                control = addTextBoxControl(node, row);
                if ((node.Attributes["Multiline"] != null) && (node.Attributes["Multiline"].InnerText == "True"))
                {
                    if (node.Attributes["h"] != null)
                    {
                        control.Style.Add("height", node.Attributes["h"].InnerText + "px");
                    }
                }
                else
                    control.Style.Add("height", "20px");

            }
            else if (controlloTipo == "0") //Label20
            {
                control = addLabelControl(node);
                control.Style.Add("height", "20px");
            }
            else if (controlloTipo == "8") //ComboBox28
            {
                control = addComboBoxControl(node, row);
                control.Style.Add("height", "20px");
            }
            else if (controlloTipo == "3") //DataTextBox30
            {
                control = addDateBoxControl(node, row, "dd/MM/yyyy");
                control.Style.Add("height", "20px");
            }
            else if (controlloTipo == "6") //Rectangle54
            {
                control = addRectangleControl(node);
                if (node.Attributes["h"] != null)
                {
                    control.Style.Add("height", node.Attributes["h"].InnerText + "px");
                }

                control.Style.Add("z-index", "-1");
            }
            else if (controlloTipo == "4") //NumberTextBox37
            {
                control = addTextBoxControl(node, row);
                control.Style.Add("height", "20px");
            }
            else if (controlloTipo == "9") //RadioBox95
            {
                control = addOptionControl(node, row);
            }
            else if (controlloTipo == "5") //Checkbox
            {
                control = addCheckBoxControl(node, row);
            }


            if (control == null) return null;

            if (node.Attributes["nomeCampo"] == null)
                control.ID = Name + "_" + Guid.NewGuid().ToString();
            else
            {
                int i = 0;
                Boolean bContinue = true;
                do
                {
                    String controlName = String.Empty;

                    if (String.IsNullOrEmpty(fatherNode))
                        controlName = String.Format("{0}_{1}_{2}", Name, node.Name, i); ////node.Attributes["nomeCampo"].InnerText 
                    else
                        controlName = String.Format("{0}_{1}_{2}_{3}", Name, fatherNode, node.Name, i); ////node.Attributes["nomeCampo"].InnerText 

                    Control ctrl = WebHelper.FindControlRecursive(controls, controlName);
                    if (ctrl == null)
                    {
                        control.ID = controlName;
                        control.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
                        bContinue = false;
                    }
                    else i++;
                } while (bContinue);

                //control.ID = idForm + "_" + Guid.NewGuid().ToString();
                //control.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            }

            if (node.Attributes["TabIndex"] != null)
                control.TabIndex = short.Parse(node.Attributes["TabIndex"].InnerText);

            if (node.Attributes["Required"] != null)
            {
                control.Attributes.Add("required", "required");
            }

            if (node.Attributes["Font"] != null)
                control.Font.Name = node.Attributes["Font"].InnerText;

            if (node.Attributes["FontSize"] != null)
                control.Font.Size = new FontUnit(double.Parse(node.Attributes["FontSize"].InnerText));

            if (node.Attributes["FontStyle"] != null)
            {
                int iFontStyle = int.Parse(node.Attributes["FontStyle"].InnerText);
                switch (iFontStyle)
                {
                    case 1:
                        control.Font.Bold = true;
                        break;
                }
            }

            if ((node.Attributes["FontColorR"] != null) && (node.Attributes["FontColorG"] != null) && (node.Attributes["FontColorB"] != null))
            {
                control.ForeColor = System.Drawing.Color.FromArgb(int.Parse(node.Attributes["FontColorR"].InnerText),
                                                    int.Parse(node.Attributes["FontColorG"].InnerText),
                                                    int.Parse(node.Attributes["FontColorB"].InnerText));
            }

            /*
            if ((node.Attributes["BackColorR"] != null) && (node.Attributes["BackColorG"] != null) && (node.Attributes["BackColorB"] != null))
            {
                control.BackColor = System.Drawing.Color.FromArgb(int.Parse(node.Attributes["BackColorR"].InnerText),
                                                    int.Parse(node.Attributes["BackColorG"].InnerText),
                                                    int.Parse(node.Attributes["BackColorB"].InnerText));
            }
            */

            /*
            x="195" y="98"
            w="605" h="35"
            */
            control.Style.Add("position", position.GetString());

            control.Style.Add("left", (int.Parse(node.Attributes["x"].InnerText) + deltaX) + "px");
            control.Style.Add("top", (int.Parse(node.Attributes["y"].InnerText) + deltaY) + "px");

            control.Style.Add("padding", "0 0 0 0");
            control.Style.Add("margin", "0 0 0 0");

            if ((bNotAvoidWidth) && (node.Attributes["w"] != null))
            {
                control.Style.Add("width", node.Attributes["w"].InnerText + "px");

            }

            return control;


        }

        private WebControl addTabPanel(ControlPosition position, XmlNode node, List<DataTable> ds)
        {
            Panel htmlDivTabbable = new Panel();
            htmlDivTabbable.Attributes.Add("class", "tabbable");
            htmlDivTabbable.Style.Add("position", "relative");

            HtmlGenericControl htmlUl = new HtmlGenericControl("ul");
            htmlUl.Attributes.Add("class", "nav nav-tabs");

            Boolean bFirst = true;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                string pageText = childNode.Attributes["PageText"].InnerText.Replace(' ', '_');

                HtmlGenericControl htmlLi = new HtmlGenericControl("li");
                if (bFirst)
                {
                    htmlLi.Attributes.Add("class", "active");
                    bFirst = false;
                }
                HtmlGenericControl htmlA = new HtmlGenericControl("a");
                htmlA.Attributes.Add("href", "#" + pageText);
                htmlA.Attributes.Add("data-toggle", "tab");
                htmlA.InnerText = pageText;

                htmlLi.Controls.Add(htmlA);

                htmlUl.Controls.Add(htmlLi);

            }
            htmlDivTabbable.Controls.Add(htmlUl);

            Panel htmlContent = new Panel();
            htmlContent.Attributes.Add("class", "tab-content");

            bFirst = true;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                string pageText = childNode.Attributes["PageText"].InnerText.Replace(' ', '_');
                string nomeTabella = childNode.Attributes["NomeTabellaDefault"].Value;
                DataRow row = null;
                if (String.IsNullOrEmpty(nomeTabella))
                {
                    row = ds.Count == 0 || ds[0].Rows.Count == 0 ? null : ds[0].Rows[0];
                }
                else
                {
                    String nomefisicoTabella = "aux_ref_" + nomeTabella.ToLower();
                    foreach (DataTable dt in ds)
                    {
                        if (dt.TableName == nomefisicoTabella)
                            row = dt.Rows.Count == 0 ? null : dt.Rows[0];
                    }
                }
                Panel htmlContentPanel = new Panel();
                htmlContentPanel.Attributes.Add("id", pageText);
                if (bFirst)
                {
                    htmlContentPanel.Attributes.Add("class", "tab-pane active");
                    bFirst = false;
                }
                else
                    htmlContentPanel.Attributes.Add("class", "tab-pane");

                foreach (XmlNode nodeElement in childNode.ChildNodes)
                {
                    switch (nodeElement.NodeType)
                    {
                        case XmlNodeType.Element:

                            Console.Write("<" + nodeElement.Name);
                            Console.WriteLine(">");

                            addWebControl(position, htmlContentPanel.Controls, nodeElement, 0, 30, row, childNode.Name);

                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(nodeElement.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + nodeElement.Name);
                            Console.WriteLine(">");
                            break;
                    }
                }


                htmlContent.Controls.Add(htmlContentPanel);
            }

            htmlDivTabbable.Controls.Add(htmlContent);

            return htmlDivTabbable;
        }

        private WebControl addRectangleControl(XmlNode node)
        {
            Panel pnl = new Panel();
            if (node.Attributes["borderWidth"] != null)
                pnl.BorderWidth = new Unit(int.Parse(node.Attributes["borderWidth"].InnerText));
            else
                pnl.BorderWidth = new Unit(1);

            if ((node.Attributes["BorderColorR"] != null) && (node.Attributes["BorderColorG"] != null) && (node.Attributes["BorderColorB"] != null))
            {
                pnl.BorderColor = System.Drawing.Color.FromArgb(int.Parse(node.Attributes["BorderColorR"].InnerText),
                                                    int.Parse(node.Attributes["BorderColorG"].InnerText),
                                                    int.Parse(node.Attributes["BorderColorB"].InnerText));
            }

            return pnl;
        }

        private WebControl addOptionControl(XmlNode node, DataRow row)
        {
            RadioButton chk = new RadioButton();
            String fieldName = node.Attributes["nomeCampo"].Value;
            chk.GroupName = fieldName;

            String valoreDefault = node.Attributes["ValoreDefault"].Value;
            chk.Attributes.Add("value", valoreDefault);

            if (row != null)
            {
                chk.Checked = row[fieldName].ToString() == valoreDefault ? true : false;
            }

            return chk;

        }

        private WebControl addCheckBoxControl(XmlNode node, DataRow row)
        {
            CheckBox chk = new CheckBox();

            if (node.Attributes["ValoreDefault"] != null)
            {
                chk.Checked = node.Attributes["ValoreDefault"].InnerText == "False" ? false : true;
            }

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                String fieldValue = row[fieldName] == null ? "F" : row[fieldName].ToString();
                chk.Checked = fieldValue == "F" || fieldValue == "0" || fieldValue == "N" ? false : true;
            }

            return chk;
        }

        private WebControl addComboBoxControl(XmlNode node, DataRow row)
        {
            DropDownList ddl = new DropDownList();

            String[] voci = node.Attributes["voci"].InnerText.Split(new char[] { '|' });

            ddl.Items.Add(String.Empty);
            foreach (String voce in voci)
            {
                ddl.Items.Add(new ListItem(voce));
            }

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                if (String.IsNullOrEmpty(row[fieldName].ToString()))
                    ddl.SelectedValue = String.Empty;
                else
                    ddl.SelectedValue = row[fieldName].ToString();
            }
            else
                ddl.SelectedValue = String.Empty;

            return ddl;
        }

        private WebControl addTextBoxControl(XmlNode node, DataRow row)
        {
            TextBox txtBox = new TextBox();

            if (node.Attributes["ValoreDefault"] != null)
            {
                string defaultValue = node.Attributes["ValoreDefault"].InnerText;
                txtBox.Text = formulaHelper.ResolveFormula(defaultValue);

            }


            if (node.Attributes["Multiline"] != null)
            {
                bool multiLine = node.Attributes["Multiline"].InnerText == "True" ? true : false;
                if (multiLine)
                {
                    txtBox.TextMode = TextBoxMode.MultiLine;
                }
            }

            if (node.Attributes["MaxLen"] != null)
                txtBox.MaxLength = int.Parse(node.Attributes["MaxLen"].InnerText);

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                txtBox.Text = row[fieldName].ToString();
            }


            return txtBox;
        }

        private WebControl addDateBoxControl(XmlNode node, DataRow row, String format)
        {
            TextBox txtBox = new TextBox();

            if (node.Attributes["ValoreDefault"] != null)
            {
                string defaultValue = node.Attributes["ValoreDefault"].InnerText;
                txtBox.Text = formulaHelper.ResolveFormula(defaultValue);

            }

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                if (String.IsNullOrEmpty(format))
                    txtBox.Text = row[fieldName].ToString();
                else
                {
                    DateTime? dtValue = (row[fieldName] as DateTime?);
                    if (dtValue.HasValue)
                        txtBox.Text = dtValue.Value.ToString(format);
                }
            }


            return txtBox;
        }

        private WebControl addLabelControl(XmlNode node)
        {
            Label txtBox = new Label();
            txtBox.Text = node.Attributes["ValoreDefault"].InnerText;
            return txtBox;
        }
        
    }
}