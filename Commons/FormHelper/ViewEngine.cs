using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using log4net;
using bOS.Commons.Web;
using bOS.Commons.FormHelper.FormHandler;

namespace bOS.Commons.FormHelper
{
    public class ViewEngine
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(ViewEngine));

        FormulaHelper formulaHelper;
        XmlForm form;

        #region properties
        public String fileName = String.Empty;
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public String Name
        {
            get { return form.Name; }
        }

        public String[] Reports
        {
            get { return form.Reports; }
        }

        #endregion

        public ViewEngine(String file, XmlForm formHandler)
        {
            FileName = Path.GetFileNameWithoutExtension(file).ToLower();
            formHandler.Load(file);

            form = formHandler;
        }

        public String GetAttribute(String attribute)
        {
            return form.GetAttribute(attribute);
        }

        public void Render(Mode mode, ControlPosition position, int deltax, int deltay, ControlCollection controls, List<DataTable> ds)
        {
            form.Render(mode, position, deltax, deltay, controls, ds);
        }

        public String[] RetrieveAllTablesInUse(String prefix)
        {
            List<String> tables = new List<string>();
            tables.Add(prefix.ToLower() + form.TableName.ToLower());

            XmlNodeList controls = form.GetControls();

            foreach (XmlNode node in controls)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    string controlloTipo = node.Attributes["ControlloTipo"].InnerText;
                    if (controlloTipo == "10")
                    {
                        foreach (XmlNode childNode in node.ChildNodes)
                        {
                            string nomeTabella = childNode.Attributes["NomeTabellaDefault"].Value.ToLower();
                            if (!String.IsNullOrEmpty(nomeTabella))
                            {
                                if (!tables.Contains(nomeTabella))
                                    tables.Add(prefix + nomeTabella);
                            }
                        }
                    }
                }
            }

            return tables.ToArray();
        }

        public List<String> GetDeleteCommands(int idVisita, String[] tables)
        {
            List<String> commands = new List<string>();
            Boolean masterTable = true;

            foreach (String table in tables)
            {
                if (masterTable)
                {
                    commands.Add(String.Format("delete from {0} where id={1};", table, idVisita));
                    masterTable = false;
                }
                else
                    commands.Add(String.Format("delete from {0} where idPatient={1};", table, idVisita));
            }

            return commands;
        }

        public List<String> GetUpdateCommands(ControlCollection controls, int idVisita, List<DataTable> ds)
        {
            List<String> commands = new List<string>();

            String nomefisicoTabella = "aux_ref_" + form.TableName.ToLower();
            DataTable dt = ds[0]; //da rivedere

            foreach (Control ctrl in controls)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    TextBox txt = (TextBox)ctrl;
                    if (txt.Attributes["nomeCampo"] != null)
                    {
                        String nomeCampo = txt.Attributes["nomeCampo"];
                        String newCommand = String.Empty;


                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["column_name"].ToString().ToLower() != nomeCampo.ToLower())
                                continue;

                            String colType = row["data_type"].ToString();



                            if (colType == "int")
                            {
                                newCommand = String.Format("update {0} set {1}={2} where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);

                            }
                            else if (colType == "date")
                            {
                                newCommand = String.Format("update {0} set {1}=STR_TO_DATE('{2}', '%c/%e/%Y %H:%i') where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);

                            }
                            else
                            {
                                newCommand = String.Format("update {0} set {1}='{2}' where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);
                            }
                        }

                        commands.Add(newCommand);
                    }
                }
                else if (ctrl.GetType() == typeof(DropDownList))
                {
                    DropDownList txt = (DropDownList)ctrl;
                    if (txt.Attributes["nomeCampo"] != null)
                    {
                        String nomeCampo = txt.Attributes["nomeCampo"];
                        String newCommand = String.Empty;


                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["column_name"].ToString().ToLower() != nomeCampo.ToLower())
                                continue;

                            String colType = row["data_type"].ToString();



                            if (colType == "int")
                            {
                                newCommand = String.Format("update {0} set {1}={2} where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);

                            }
                            else if (colType == "date")
                            {
                                newCommand = String.Format("update {0} set {1}=STR_TO_DATE('{2}', '%c/%e/%Y %H:%i') where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);

                            }
                            else
                            {
                                newCommand = String.Format("update {0} set {1}='{2}' where IdPatient={3};", nomefisicoTabella, nomeCampo, txt.Text, idVisita);
                            }
                        }

                        commands.Add(newCommand);
                    }
                }

                List<String> commands2 = GetUpdateCommands(ctrl.Controls, idVisita, ds);
                foreach (String command in commands2)
                    commands.Add(command);
            }

            return commands;
        }
            

        public String GetCheckCreateTable()
        {
            String nomefisicoTabella = "aux_ref_" + form.TableName.ToLower();

            //String command = "SHOW COLUMNs FROM " + nomefisicoTabella + ";";
            String command = "select id FROM " + nomefisicoTabella + ";";
            return command;
        }

        public String GetCheckCreateTableSqlServer()
        {
            String nomefisicoTabella = "aux_ref_" + form.TableName.ToLower() + "";

            //String command = "SHOW COLUMNs FROM " + nomefisicoTabella + ";";
            String command = "select id FROM " + nomefisicoTabella + ";";
            return command;
        }
        
        public List<String> GetCreateCommands()
        {
            List<String> commands = new List<string>();
            String nomefisicoTabella = "aux_ref_" + form.TableName.ToLower();

            commands.Add("CREATE TABLE IF NOT EXISTS " + nomefisicoTabella + " (id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,idPatient int(11) NOT NULL DEFAULT '-1', nomeXML VARCHAR(50),IdCreatore VARCHAR(50),dataCreazione DATETIME,IdUserModifica VARCHAR(50),dataModifica DATETIME,NomeReferto VARCHAR(50),idReparto INTEGER UNSIGNED,ExpCSV TINYINT(1) not null default 0 , PRIMARY KEY(id));");

            XmlNodeList formControls = form.GetControls();
            foreach (XmlNode node in formControls)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        int controlloTipo = int.Parse(node.Attributes["ControlloTipo"].InnerText);
                        String campo = node.Attributes["nomeCampo"] != null ? node.Attributes["nomeCampo"].InnerText.Replace("/", "_") : String.Empty;

                        switch (controlloTipo)
                        {
                            case 10: //Tabs
                                break;

                            case 2: //TextBox3
                                int maxLen = Int32.Parse(node.Attributes["MaxLen"].InnerText);
                                if (maxLen < 255)
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " VARCHAR(" + maxLen + ") ; ");
                                else
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " TEXT ; ");

                                break;

                            case 0: //Label20
                            case 6: //Rectangle54
                                break;

                            case 8: //ComboBox28
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " VARCHAR(100) ; ");
                                break;

                            case 3: //DataTextBox30
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " DATE ; ");
                                break;

                            case 4: //NumberTextBox37
                                int decimalPart = Int32.Parse(node.Attributes["decimalPart"].InnerText);
                                int digitPart = Int32.Parse(node.Attributes["digitPart"].InnerText);

                                if (decimalPart == 0)
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " INT ; ");
                                else
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " FLOAT(" + digitPart + ", " + decimalPart + ") ; ");
                                break;

                            case 9: //RadioBox95
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + "  VARCHAR(50) ; ");
                                break;

                            case 5: //Checkbox
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD COLUMN " + campo + " CHAR(1); ");
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

            return commands;
        }

        public List<String> GetCreateCommandsForSQLServer()
        {
            List<String> commands = new List<string>();
            String nomefisicoTabella = "[dbo].[aux_ref_" + form.TableName.ToLower() + "]";

            commands.Add("CREATE TABLE " + nomefisicoTabella + " ("
            + "id int IDENTITY(1,1) NOT NULL"
            + ",idPatient int NOT NULL DEFAULT '-1'"
            + ",nomeXML nvarchar(50)"
            + ",IdCreatore nvarchar(50)"
            + ",dataCreazione DATETIME"
            + ",IdUserModifica nvarchar(50)"
            + ",dataModifica DATETIME"
            + ");");

            XmlNodeList formControls = form.GetControls();
            foreach (XmlNode node in formControls)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        int controlloTipo = int.Parse(node.Attributes["ControlloTipo"].InnerText);
                        String campo = node.Attributes["nomeCampo"] != null ? node.Attributes["nomeCampo"].InnerText.Replace("/", "_") : String.Empty;

                        switch (controlloTipo)
                        {
                            case 10: //Tabs
                                break;

                            case 2: //TextBox3
                                int maxLen = Int32.Parse(node.Attributes["MaxLen"].InnerText);
                                if (maxLen < 255)
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " nvarchar(" + maxLen + ") ; ");
                                else
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " ntext ; ");

                                break;

                            case 0: //Label20
                            case 6: //Rectangle54
                                break;

                            case 8: //ComboBox28
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " nvarchar(100) ; ");
                                break;

                            case 3: //DataTextBox30
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " DATE ; ");
                                break;

                            case 4: //NumberTextBox37
                                int decimalPart = Int32.Parse(node.Attributes["decimalPart"].InnerText);
                                int digitPart = Int32.Parse(node.Attributes["digitPart"].InnerText);

                                if (decimalPart == 0)
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " INT ; ");
                                else
                                    commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " FLOAT ; ");
                                break;

                            case 9: //RadioBox95
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + "  nvarchar(50) ; ");
                                break;

                            case 5: //Checkbox
                                commands.Add("ALTER TABLE " + nomefisicoTabella + "  ADD " + campo + " CHAR(1); ");
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

            return commands;
        }

        private Control FindControl(ControlCollection controls, string nomeCampo)
        {
            foreach (Control control in controls)
            {
                if (control is WebControl)
                {
                    WebControl rootControl = control as WebControl;

                    if ((rootControl.Attributes["nomeCampo"] != null) && (rootControl.Attributes["nomeCampo"] == nomeCampo))
                        return rootControl;
                }

                Control controlToReturn = FindControl(control.Controls, nomeCampo);
                if (controlToReturn != null) return controlToReturn;

            }
            return null;

        }

        public List<String> GetAddCommands(ControlCollection control, int idVisita, String fieldsName, String fieldsValue)
        {
            List<String> commands = new List<string>();

            String nomefisicoTabella = "aux_ref_" + form.TableName.ToLower();

            XmlNodeList formControls = form.GetControls();
            foreach (XmlNode node in formControls)
            {
                int controlloTipo = int.Parse(node.Attributes["ControlloTipo"].InnerText);

                if (node.Attributes["nomeCampo"] == null)
                    continue;

                string nomeCampo = node.Attributes["nomeCampo"].InnerText;

                if (String.IsNullOrEmpty(fieldsName))
                    fieldsName = nomeCampo;
                else
                    fieldsName = fieldsName + "," + nomeCampo;

                String fieldValue = String.Empty;

                switch (controlloTipo)
                {
                    case 2: //TextBox3
                        {
                            TextBox txt = FindControl(control, nomeCampo) as TextBox;
                            fieldValue = String.Format("'{0}'", txt == null ? String.Empty : txt.Text);
                            break;
                        }
                    case 8: //ComboBox28
                        {
                            DropDownList ddl = FindControl(control, nomeCampo) as DropDownList;
                            fieldValue = String.Format("'{0}'", ddl == null ? String.Empty : ddl.Text);

                            break;
                        }
                    case 3: //DataTextBox30
                        {
                            TextBox txt = FindControl(control, nomeCampo) as TextBox;

                            fieldValue = String.Format("STR_TO_DATE('{0}', '%c/%e/%Y %H:%i')", txt == null ? String.Empty : txt.Text);
                            break;

                        }
                    case 4: //NumberTextBox37
                        {
                            TextBox txt = FindControl(control, nomeCampo) as TextBox;
                            fieldValue = String.Format("{0}", txt == null ? String.Empty : txt.Text);
                            break;

                        }
                    case 9: //RadioBox95
                        {
                            break;
                        }
                    case 5: //Checkbox
                        {
                            CheckBox chk = FindControl(control, nomeCampo) as CheckBox;
                            fieldValue = String.Format("'{0}'", chk == null ? String.Empty : chk.Checked.ToString());
                            break;
                        }
                    default:
                        continue;
                }

                if (String.IsNullOrEmpty(fieldsValue))
                    fieldsValue = fieldValue;
                else
                    fieldsValue = fieldsValue + "," + fieldValue;

            }
            String insertCommand = String.Format("insert into {0} (IdPatient,{1} ) values ({2}, {3})", nomefisicoTabella, fieldsName, idVisita, fieldsValue);
            commands.Add(insertCommand);


            return commands;
        }

        public NameValueCollection GetFields(List<DataTable> ds)
        {
            NameValueCollection values = new NameValueCollection();

            DataRow row = ds[0].Rows.Count == 0 ? null : ds[0].Rows[0];

            formulaHelper = new FormulaHelper(ds);
            XmlNodeList formControls = form.GetControls();
            foreach (XmlNode node in formControls)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:

                        string controlloTipo = node.Attributes["ControlloTipo"].InnerText;
                        if (controlloTipo == "10")
                        {
                            foreach (XmlNode childNode in node.ChildNodes)
                            {
                                string pageText = childNode.Attributes["PageText"].InnerText.Replace(' ', '_');
                                string nomeTabella = childNode.Attributes["NomeTabellaDefault"].Value;
                                DataRow rowTab = null;
                                if (String.IsNullOrEmpty(nomeTabella))
                                {
                                    rowTab = ds.Count == 0 || ds[0].Rows.Count == 0 ? null : ds[0].Rows[0];
                                }
                                else
                                {
                                    String nomefisicoTabella = "aux_ref_" + nomeTabella.ToLower();
                                    foreach (DataTable dt in ds)
                                    {
                                        if (dt.TableName == nomefisicoTabella)
                                            rowTab = dt.Rows.Count == 0 ? null : dt.Rows[0];
                                    }
                                }

                                foreach (XmlNode nodeElement in childNode.ChildNodes)
                                {
                                    switch (nodeElement.NodeType)
                                    {
                                        case XmlNodeType.Element:

                                            if (nodeElement.Attributes["nomeCampo"] == null)
                                                continue;

                                            String fieldName = pageText + "." + nodeElement.Attributes["nomeCampo"].Value;
                                            if (rowTab == null)
                                                values.Add(fieldName, String.Empty);
                                            else
                                                values.Add(fieldName, rowTab[nodeElement.Attributes["nomeCampo"].Value].ToString());
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (node.Attributes["nomeCampo"] == null)
                                continue;

                            String fieldName = node.Attributes["nomeCampo"].Value;

                            if (row == null)
                                values.Add(fieldName, String.Empty);
                            else
                                values.Add(fieldName, row[fieldName].ToString());

                        }
                        break;
                }
            }

            return values;
        }



    }
}