using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using log4net;

namespace bOS.Commons
{
    public class HttpRequestHelper
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(HttpRequestHelper));

        public enum ActionTypeEnum
        {
            Get,
            Post
        }

        private string m_url = string.Empty;
        private ActionTypeEnum m_type = ActionTypeEnum.Get;



        public HttpRequestHelper(string url, ActionTypeEnum type)
        {
            m_url = url;
            m_type = type;
        }

        public string Post(NameValueCollection values, String fileName)
        {
            StringBuilder parameters = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                EncodeAndAddItem(ref parameters, values.GetKey(i), values[i]);
            }
            if (String.IsNullOrEmpty(fileName))
                return PostData(m_url, parameters.ToString());
            else
            {
                PostData2File(m_url, parameters.ToString(), fileName);
            }
            return String.Empty;
        }

        private void PostData2File(string url, string postData, String file)
        {
            HttpWebRequest request = null;
            if (m_type == ActionTypeEnum.Post)
            {
                Uri uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                using (Stream writeStream = request.GetRequestStream())
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(postData);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                Uri uri = new Uri(url + "?" + postData);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
            }
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (Stream s = File.Create(file))
                    {
                        responseStream.CopyTo(s);
                    }
                }

            }
        }

        private string PostData(string url, string postData)
        {
            HttpWebRequest request = null;
            if (m_type == ActionTypeEnum.Post)
            {
                Uri uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                using (Stream writeStream = request.GetRequestStream())
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(postData);
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                Uri uri = new Uri(url + "?" + postData);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
            }
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                }
            }

            return result;
        }

        private void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(System.Web.HttpUtility.UrlEncode(dataItem));
        }

        public void UploadFile(string url, string filename)
        {
            WebClient wc = new WebClient();
            byte[] response = wc.UploadFile(url, filename);
        }

        public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, Path.GetFileName(file), contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            rs = null;

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                String result = new StreamReader(stream2).ReadToEnd();
                log.Debug("Result = " + result);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                wr = null;
            }
        }
    }
}
