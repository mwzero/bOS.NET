using bOS.Commons.Configuration;
using bOS.Services.CDN.Client;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace bOS.Commons
{
    public class TemplateHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(TemplateHelper));

        private static volatile TemplateHelper instance;
        private static object syncRoot = new Object();

        //
        public static String TEMPLATE_BUCKET = "Templates";
        public static String SURVEY_TEMPLATE = "Survey";
        public static String REQUEST_CHANGES_TEMPLATE = "RequestChanges";
        public static String LINK_SURVEY_VAR = "LinkSurvey";

        private TemplateHelper()
        {
            String templateFolder = ConfigurationHelper.GetPath(TEMPLATE_BUCKET);
            CdnHelper.Instance.Initialize(new CdnFileHelper(templateFolder));
        }

        public static TemplateHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TemplateHelper();
                    }
                }

                return instance;
            }
        }

        public static String Process(String processBody, Object obj, List<KeyValuePair<string, string>> myKeyValuePair, ResourceManager resourceManager)
        {
            List<KeyValuePair<string, string>> vars = GetVariables(obj, resourceManager);

            if (myKeyValuePair != null)
                vars.AddRange(myKeyValuePair);

            foreach (KeyValuePair<string, string> var in vars)
            {
                logger.Debug(String.Format("Template Process Variable on exam {0}={1}", var.Key, var.Value));
                processBody = processBody.Replace("{{" + var.Key + "}}", var.Value);
            }
            return processBody;
        }

        public static String Process(String templateName, String category, Object obj, List<KeyValuePair<string, string>> myKeyValuePair, ResourceManager resourceManager)
        {
            //find Template
            String content2Search = String.Format("{0}_{1}.html", templateName, "HelpDesk");

            try
            {
                String content = CdnHelper.Instance.GetContent(category, content2Search, null);
                return Process(content, obj, myKeyValuePair, resourceManager);
            }
            catch
            {
                return String.Empty;

            }
            
        }

        private static List<KeyValuePair<string, string>> GetVariables(Object obj2, ResourceManager resourceManager)
        {
            List<KeyValuePair<string, string>> vars = new List<KeyValuePair<string,string>>();

            if (obj2 == null)
                return vars;

            Type type = obj2.GetType();
            List<PropertyInfo> managedProperties = PropertyHelper.GetProperty2Manage(type, BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in managedProperties)
            {
                String propertyName = resourceManager.GetString(property.Name);
                if (String.IsNullOrEmpty(propertyName))
                    propertyName = property.Name;

                //retrieve value
                String propertyValue = PropertyHelper.GetValue(obj2, property);
                if (property.PropertyType == typeof(System.Boolean))
                {
                    propertyValue = String.IsNullOrEmpty(propertyValue) ? "no" : "si";
                }

                vars.Add(new KeyValuePair<string, string>(propertyName, propertyValue));
            }

            return vars;
        }


    }
}