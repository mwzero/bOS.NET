using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bOS.Commons.Configuration;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Text;

namespace bOS.Services.CDN.Client
{
    public class CdnFileHelper : CdnPersistenceHelper
    {
        String baseUrl;

        public CdnFileHelper(String url)
            : base()
        {
            baseUrl = url;
        }

        public override String GetContent(String path, String contentName, NameValueCollection nvc)
        {
            String file = String.Empty;

            if (String.IsNullOrEmpty(path))
            {
                file = String.Format("{0}/{1}", baseUrl, contentName);
                
            }
            else
            {
                file = String.Format("{0}/{1}/{2}", baseUrl, path, contentName);
             
            }

            if (!File.Exists(file))
            {
                throw new Exception("content not found");
            }

            string text = string.Empty;
            using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            return text;
        }

        public override void SaveContent(String path, String contentName, String content, NameValueCollection nvc)
        {
            String file2Search = String.Format("{0}/{1}/{2}", baseUrl, path, contentName);
            File.WriteAllText(file2Search, content,System.Text.Encoding.UTF8);
        }

        public override void SaveFileContent(String path, String contentName, String fileContent, NameValueCollection nvc)
        {
            throw new NotImplementedException();
        }
    }
}