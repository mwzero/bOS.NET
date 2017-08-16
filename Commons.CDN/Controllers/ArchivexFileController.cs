using bOS.Commons;
using bOS.Services.CDN.Utils;
using Commons.CDN.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace bOS.Services.CDN.Controllers
{
    public class ArchivexFileController : ApiController
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(ArchivexFileController));

        public HttpResponseMessage GetFile(long paziente, int visita, int folderType, String fileName)
        {
            logger.Debug (String.Format ("Retrieving Content: Paziente {0} Visita {1} FolderType {2} fileName {3}",
                paziente,
                visita,
                folderType,
                fileName));

            String pathFile = String.Empty;

            switch (folderType)
            {
                case 0:
                    pathFile = ArchivexFileHelper.GetDocumentPath(paziente, visita, fileName);
                    break;
                case 1:
                    pathFile = ArchivexFileHelper.GetFilmPath(paziente, visita, fileName);
                    break;
                case 2:
                    pathFile = ArchivexFileHelper.GetImagePath(paziente, visita, fileName);
                    break;
                default:
                    pathFile = ArchivexFileHelper.GetOtherPath(paziente, visita, fileName);
                    break;
            }
            logger.Debug(String.Format("Path File {0}", pathFile));

            //byte[] data2 = FilesHelper.GetFileAsByte(pathFile);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(pathFile, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;

            AuditHelper.Instance.auditLogs.Add(
                new Audit(
                    String.Format("GET: Retrieving file {0}", fileName), 
                    pathFile));

            return response;
        }

        public async Task<HttpResponseMessage> PostFormData()
        {
            logger.Debug("Post content");
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                logger.Debug("Request contains multipart/form-data");
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            logger.Debug("Try save request stream directly on a file");
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                logger.Debug("Read the form data");
                await Request.Content.ReadAsMultipartAsync(provider);

                long paziente = 0;
                int visita = 0;
                int folderType = 0;
                String fileName;

                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key.CompareTo("paziente") == 0)
                        {
                            paziente = long.Parse(val);
                        }
                        else if (key.CompareTo("visita") == 0)
                        {
                            visita = int.Parse(val);
                        }
                        else if (key.CompareTo("folderType") == 0)
                        {
                            folderType = int.Parse(val);
                        }


                        logger.Debug(string.Format("{0}: {1}", key, val));
                    }
                }

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    fileName = file.Headers.ContentDisposition.FileName.Replace('\"', ' ');
                    logger.Debug("Server file path: " + file.LocalFileName);

                    String pathFile = String.Empty;

                    switch (folderType)
                    {
                        case 0:
                            pathFile = ArchivexFileHelper.GetDocumentPath(paziente, visita, fileName);
                            break;
                        case 1:
                            pathFile = ArchivexFileHelper.GetFilmPath(paziente, visita, fileName);
                            break;
                        case 2:
                            pathFile = ArchivexFileHelper.GetImagePath(paziente, visita, fileName);
                            break;
                        default:
                            pathFile = ArchivexFileHelper.GetOtherPath(paziente, visita, fileName);
                            break;
                    }

                    File.Move(file.LocalFileName, pathFile);

                    AuditHelper.Instance.auditLogs.Add(
                        new Audit(
                            String.Format("POST: file {0}", fileName),
                            pathFile));

                }

                
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                logger.Error(e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
