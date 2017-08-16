using bOS.Commons;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bOS.Services.CDN.Client
{
    public class CdnWebHelper : CdnPersistenceHelper
    {
        String baseUrl;

        public CdnWebHelper(String url) : base ()
        {
            baseUrl = url;
        }

        public override String GetContent(String path, String contentName, NameValueCollection nvc)
        {
            throw new NotImplementedException();
        }

        public override void SaveContent(String path, String contentName, String content, NameValueCollection nvc)
        {
            throw new NotImplementedException();
        }

        public override void SaveFileContent(String path, String fileName, String file, NameValueCollection nvc)
        {
            String url = String.Empty;
            if ( String.IsNullOrEmpty (path))
            {
                url = baseUrl;
            }
            else
            {
                url = string.Format("{0}/{1}", baseUrl, path);
            }
            HttpRequestHelper helper = new HttpRequestHelper(url, HttpRequestHelper.ActionTypeEnum.Post);

            String mimeType = MimeTypeHelper.GetMimeType( System.IO.Path.GetExtension(fileName));
            helper.HttpUploadFile(url, file, "file", mimeType, nvc);
        }
    }
}
