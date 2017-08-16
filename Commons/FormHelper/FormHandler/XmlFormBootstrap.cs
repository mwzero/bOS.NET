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
    public class XmlFormBootstrap : XmlForm
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(XmlFormBootstrap));

        List<XmlFormContainer> xmlContainers;

        public XmlFormBootstrap() : base()
        {
            xmlContainers = null;
        }

        public override void InitializeControlsAfterLoad()
        {
            xmlContainers = RetrieveContainers();
            AssignItem2Container();
        }

        public void RenderItem(
            HtmlGenericControl divContainer
            ,Mode mode
            ,ControlCollection controls
            ,XmlFormElement xmlElement
            ,List<DataTable> ds)
        {
            HtmlGenericControl control = addWebControl(
                        mode,
                        controls, //to check if id already assigned
                        xmlElement,
                        ds == null || ds.Count == 0 || ds[0].Rows.Count == 0 ? null : ds[0].Rows[0], String.Empty);



            if (control != null)
            {
                divContainer.Controls.Add(control);
            }
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

            //Render
            foreach (XmlFormContainer container in xmlContainers )
            {
                int controlloTipo = int.Parse(container.Node.Attributes["ControlloTipo"].InnerText);

                HtmlGenericControl divContainer = new HtmlGenericControl("div");
                divContainer.Attributes["class"] = container.Node.Attributes["Span"].Value;

                if (controlloTipo == 10) //tab
                {
                    string pageText = container.Node.ChildNodes[0].Attributes["PageText"].InnerText.Replace(' ', '_');
                    
                    Boolean bMultiOccurrence = Boolean.Parse(container.Node.ChildNodes[0].Attributes["MultiOccurence"].InnerText);
                    if (bMultiOccurrence)
                    {
                        HtmlGenericControl chk = new HtmlGenericControl("button");
                        chk.Attributes.Add("class", "btn btn-primary");
                        chk.InnerHtml = "<i class=\"icon-plus-sign\">";
                    }

                    foreach (XmlNode nodeElement in container.Node.ChildNodes[0].ChildNodes)
                    {
                        switch (nodeElement.NodeType)
                        {
                            case XmlNodeType.Element:

                                Console.Write("<" + nodeElement.Name);
                                Console.WriteLine(">");

                                RenderItem(divContainer, mode, controls, new XmlFormElement(nodeElement), ds);

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

                    

                    controls.Add(divContainer);
                }
                else
                {
                    foreach (XmlFormElement xmlElement in container.Items)
                    {
                        RenderItem(divContainer, mode, controls, xmlElement, ds);
                    }

                    controls.Add(divContainer);
                }

            }
            
        }

        private void AssignItem2Container()
        {
            XmlNodeList formControls = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in formControls)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:

                        int controlloTipo = int.Parse(node.Attributes["ControlloTipo"].InnerText);
                        switch (controlloTipo)
                        {
                            case 10: //tab
                                break;

                            case 6: //Rectangle54
                                break;

                            case 0: //Label20
                                break;

                            case 2: //TextBox3
                            case 8: //ComboBox28
                            case 3: //DataTextBox30
                            case 4: //NumberTextBox37
                            case 9: //RadioBox95
                            case 5: //Checkbox

                                Boolean bAdded = Add2Container(node);
                                if (!bAdded)
                                {
                                    logger.Warn(String.Format("Node {0} not belongs to container", node.Name));
                                }

                                break;

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

        private bool Add2Container(XmlNode node)
        {
            int left = int.Parse(node.Attributes["x"].InnerText);
            int top = int.Parse(node.Attributes["y"].InnerText);

            foreach (XmlFormContainer container in xmlContainers)
            {
                //i prefer calculate here because in the future it's no make sense have "left" and "top" 
                //in a responsiveness skin
                int leftContainer = int.Parse(container.Node.Attributes["x"].InnerText);
                int topContainer = int.Parse(container.Node.Attributes["y"].InnerText);
                int widthContainer = int.Parse(container.Node.Attributes["w"].InnerText);
                int heightContainer = int.Parse(container.Node.Attributes["h"].InnerText);
                
                if  ( ( left >= leftContainer ) && ( top >= topContainer ) && 
                      ( left <= leftContainer + widthContainer) && (top <= topContainer + heightContainer) )
                {
                    container.AddItem(node);
                    return true;
                }
            }

            return false;
        }

        private List<XmlFormContainer> RetrieveContainers()
        {
            List<XmlFormContainer> xmlContainers = new List<XmlFormContainer>();

            //retrieving all containers
            XmlNodeList formControls = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in formControls)
            {
                Boolean bAddNode = false;
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:

                        int controlloTipo = int.Parse(node.Attributes["ControlloTipo"].InnerText);
                        switch (controlloTipo)
                        {
                            case 10: //tab
                                
                                break;

                            case 6: //Rectangle54
                                bAddNode = true;
                                break;
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

                if ( bAddNode )
                {
                    int i = 0;
                    int yToAdd = int.Parse(node.Attributes["y"].InnerText);
                    bool bNotAdded = true;
                    foreach (XmlFormContainer container in xmlContainers)
                    {
                        int y = int.Parse(container.Node.Attributes["y"].InnerText);
                        if ( yToAdd < y )
                        {
                            xmlContainers.Insert(i, new XmlFormContainer(node));
                            bNotAdded = false;
                            break;
                        }

                        i++;
                    }

                    if ( bNotAdded )
                        xmlContainers.Insert(i, new XmlFormContainer(node));
                }
                

            }

            return xmlContainers;
        }

        private HtmlGenericControl addWebControl(Mode mode, ControlCollection controls, XmlFormElement xmlElement, DataRow row, String fatherNode)
        {
            XmlNode node = xmlElement.Node;

            HtmlGenericControl control = null;

            int controlloTipo = int.Parse (node.Attributes["ControlloTipo"].InnerText);
            String controlID = String.Empty;

            if (node.Attributes["nomeCampo"] == null)
                controlID = Name + "_" + Guid.NewGuid().ToString();
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
                        controlID = controlName;
                        
                        bContinue = false;
                    }
                    else i++;
                } while (bContinue);

                //control.ID = idForm + "_" + Guid.NewGuid().ToString();
                //control.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            }

            switch (controlloTipo)
            {
                case 2: //TextBox3
                case 4: //NumberTextBox37
                    control = addTextBoxControl(controlID, node, row, mode);
                    break;

                case 0: //Label20
                    //control = addLabelControl(node);
                    break;

                case 8: //ComboBox28
                    control = addComboBoxControl(controlID, node, row, mode);
                    break;

                case 3: //DataTextBox30
                    //control = addDateBoxControl(node, row, "dd/MM/yyyy");
                    break;

                case 9: //RadioBox95
                    //control = addOptionControl(node, row);
                    break;

                case 5: //Checkbox
                    control = addCheckBoxControl(controlID, node, row, mode);
                    break;
            }

            if (control == null) return null;

            

            return control;


        }

        private HtmlGenericControl addComboBoxControl(string controlID, XmlNode node, DataRow row, Mode mode)
        {
            HtmlGenericControl divField = new HtmlGenericControl("div");
            divField.Attributes["class"] = node.Attributes["Span"].Value;

            HtmlGenericControl divLabel = new HtmlGenericControl("label");

            //invece di prendere il nomeCampo si potrebbe cercare la label associata
            //applicare la globalizzazione
            divLabel.InnerHtml = node.Attributes["nomeCampo"].Value;

            divField.Controls.Add(divLabel);
            DropDownList ddl = new DropDownList();

            String[] voci = node.Attributes["voci"].InnerText.Split(new char[] { '|' });
            ddl.Items.Add(String.Empty);
            foreach (String voce in voci)
            {
                ddl.Items.Add(new ListItem(voce));
            }

            if (node.Attributes["ValoreDefault"] != null)
            {
                string defaultValue = node.Attributes["ValoreDefault"].InnerText;
                ddl.SelectedValue = formulaHelper.ResolveFormula(defaultValue);

            }

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                ddl.SelectedValue = row[fieldName].ToString();
            }

            ddl.CssClass = node.Attributes["Span"].Value;

            if (mode == Mode.READ)
            {
                ddl.Enabled = false;
            }

            ddl.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            ddl.ID = controlID;
            if (node.Attributes["Required"] != null)
            {
                ddl.Attributes.Add("required", "required");
            }
            divField.Controls.Add(ddl);

            return divField;
        }

        private HtmlGenericControl addTextBoxControl(String controlID, XmlNode node, DataRow row, Mode mode)
        {
            HtmlGenericControl divField = new HtmlGenericControl("div");
            divField.Attributes["class"] = node.Attributes["Span"].Value;

            HtmlGenericControl divLabel = new HtmlGenericControl("label");

            //invece di prendere il nomeCampo si potrebbe cercare la label associata
            //applicare la globalizzazione
            divLabel.InnerHtml = node.Attributes["nomeCampo"].Value;

            divField.Controls.Add(divLabel);
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

            txtBox.CssClass = node.Attributes["Span"].Value;

            if (mode == Mode.READ)
            {
                txtBox.Attributes.Add("readonly", "");
            }

            txtBox.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            txtBox.ID = controlID;
            if (node.Attributes["Required"] != null)
            {
                txtBox.Attributes.Add("required", "required");
            }
            divField.Controls.Add(txtBox);

            return divField;

        }

        private HtmlGenericControl addCheckBoxControl(String controlID, XmlNode node, DataRow row, Mode mode)
        {
            HtmlGenericControl divField = new HtmlGenericControl("div");
            divField.Attributes["class"] = node.Attributes["Span"].Value;

            HtmlGenericControl divLabel = new HtmlGenericControl("label");
            divLabel.Attributes["class"] = "checkbox inline";
            divField.Controls.Add (divLabel);

            HtmlGenericControl chk = new HtmlGenericControl("input");
            chk.Attributes.Add("type", "checkbox");

            if ( (node.Attributes["ValoreDefault"] != null) && (node.Attributes["ValoreDefault"].InnerText == "True") )
            {
                
            }

            if (row != null)
            {
                String fieldName = node.Attributes["nomeCampo"].Value;
                String fieldValue = row[fieldName] == null ? "F" : row[fieldName].ToString();
                if ( (fieldValue != "F") && (fieldValue != "0") && (fieldValue != "N") )
                    chk.Attributes.Add("checked", "checked");
            }
            chk.InnerText = node.Attributes["nomeCampo"].Value;
            

            chk.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            chk.ID = controlID;
            if (node.Attributes["Required"] != null)
            {
                chk.Attributes.Add("required", "required");
            }
            if (mode == Mode.READ)
            {
                chk.Attributes.Add("nomeCampo", node.Attributes["nomeCampo"].InnerText);
            }

            divLabel.Controls.Add(chk);
            return divField;
        }

        /*
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
            txtBox.CssClass = node.Attributes["Span"].Value;
            return txtBox;
        }
        */
    }
}
