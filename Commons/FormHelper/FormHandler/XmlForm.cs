using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace bOS.Commons.FormHelper.FormHandler
{
    public abstract class XmlForm
    {
        static readonly ILog logger = LogManager.GetLogger(typeof(XmlForm));

        protected const string FORM_ROOT_TAG = "referto";
        protected const string FORM_NAME_TAG = "Nome";
        protected const string FORM_TABLENAME_TAG = "NomeTabella";
        protected const String FORM_REPORTS_TAG = "stampeRPT";

        protected XmlDocument doc = new XmlDocument();
        protected FormulaHelper formulaHelper;

        //properties
        public String Name { get { return GetAttribute(FORM_NAME_TAG).Trim().ToLower(); } }
        public String TableName { get { return GetAttribute(FORM_TABLENAME_TAG).Trim().ToLower(); } }
        public String[] Reports
        {
            get
            {
                String stampeRPT = GetAttribute(FORM_REPORTS_TAG);
                if (String.IsNullOrEmpty(stampeRPT))
                    return null;

                return stampeRPT.Split('\\');
            }
        }

        public void Load(String fileName)
        {
            logger.Debug(String.Format("Loading fileName [{0}]", fileName));
            try
            {
                doc.Load(fileName);
            }
            catch (Exception err)
            {
                logger.Error(String.Format("Impossible to load [{0}]. Is not a valid xml", fileName), err);
                throw new ViewEngineException("File is not a valid xml");
            }

            //checl if a valid form
            if (FORM_ROOT_TAG.Equals(doc.DocumentElement.Name.ToLower()))
            {

            }
            else
            {
                logger.Error(String.Format("file [{0}] is not a valid xml form template", fileName));
                throw new ViewEngineException("File is not a valid xml form template");
            }

            logger.Info(String.Format("File [{0}] loaded correctly!", fileName));

            this.InitializeControlsAfterLoad();

        }

        public String GetAttribute(string name)
        {
            if (doc.DocumentElement.Attributes[name] != null)
                return doc.DocumentElement.Attributes[name].Value;

            return null;
        }

        public XmlNodeList GetControls()
        {
            return doc.DocumentElement.ChildNodes;
        }


        public abstract void InitializeControlsAfterLoad();

        public abstract void Render(
            Mode mode, 
            ControlPosition position, 
            int deltax, 
            int deltay, 
            ControlCollection controls, 
            List<DataTable> ds);
        
    }
}