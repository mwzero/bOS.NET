using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class ConfigurationHelper
    {
        private static String bOS_SECTION = "bOS";

        //Links
        public static String GOOGLE_SURVEY = "GoogleSurvey";

        //Folders
        public static String EMAIL_SMS_NOTIFY = "EmailSMS";
        private static String CACHE_FOLDER = "Cache";
        private static String LUCENEINDEX_FOLDER = "Lucene";
        private static String TEMPLATES_FOLDER = "Templates";
        private static String METRICS_FOLDER = "Metrics";
        private static String FORMS_FOLDER = "Forms";

        static BasicConfiguration section = (BasicConfiguration)ConfigurationManager.GetSection(bOS_SECTION);

        public static Boolean GetFlagAsBoolean(String name)
        {
            FlagElement flagElement = section.FlagsItems[name];

            return Boolean.Parse(flagElement.Value);
        }

        public static String GetLink(String link)
        {
            LinkElement lnkElement = section.LinksItems[link];
            return lnkElement.Url;
        }



        public static LinkElement GetLinkElement(String link)
        {
            return section.LinksItems[link];
        }

        public static EmailElement GetEmailElement(String mail)
        {
            return section.EmailsItems[mail];
        }

        public static List<EmailElement> GetEmails(string type)
        {
            EmailsCollection links = section.EmailsItems;
            List<EmailElement> links2Return = new List<EmailElement>();

            for (var i = 0; i < links.Count; i++)
            {
                if ( String.IsNullOrEmpty (type) || (links[i].Type == type) )
                {
                    links2Return.Add(links[i]);
                }
            }
            return links2Return;
        }

        public static List<LinkElement> GetLinks(string type)
        {
            LinksCollection links = section.LinksItems;

            List<LinkElement> links2Return = new List<LinkElement>();

            for (var i = 0; i < links.Count; i++)
            {
                if (links[i].Type == type)
                {
                    links2Return.Add(links[i]);
                }
            }
            return links2Return;
        }

        #region
        public static String GetPath(String folder)
        {
            string path = HttpRuntime.AppDomainAppPath;

            return String.Format("{0}{1}", path, section.FolderItems[folder].Path);
        }

        public static String GetCacheFolderPath()
        {
            return GetPath(CACHE_FOLDER);
        }

        public static String GetLuceneIndexPath()
        {
            return GetPath(LUCENEINDEX_FOLDER);
        }

        public static String GetTemplateFolderPath()
        {
            return GetPath(TEMPLATES_FOLDER);
        }

        public static String MetricsFolder
        {
            get { return GetPath(METRICS_FOLDER); }
        }

        public static String FormsFolder
        {
            get { return GetPath(FORMS_FOLDER); }
        }
        #endregion
    }
}