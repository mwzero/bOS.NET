using bOS.Commons.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace bOS.Services.CDN.Controllers
{
    public class ArchivexFileHelper
    {
        private static String DOC_FOLDER = "Documenti";
        private static String IMG_FOLDER = "Immagini";
        private static String FILM_FOLDER = "Films";
        private static String OTHER_FOLDER = "Altro";

        private static String GetPath(long paziente, int visita, String folderType, String nomeFile)
        {
            String path = ConfigurationHelper.GetPath("ArchiveXFolder");

            long div = (paziente / 5000);
            div = div * 5000;

            String pathPaziente = String.Format("{0}/{1}-{2}/{3}", path, div, div + 4999, paziente);
            if (!Directory.Exists(pathPaziente))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathPaziente);
            }
            String pathVisita = String.Format("{0}/{1}", pathPaziente, visita);
            if (!Directory.Exists(pathVisita))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathVisita);
            }

            String pathFolderType = String.Format("{0}/{1}", pathVisita, folderType);
            if (!Directory.Exists(pathFolderType))
            {
                DirectoryInfo di = Directory.CreateDirectory(pathFolderType);
            }

            return String.Format("{0}/{1}", pathFolderType, nomeFile);

        }

        public static String GetDocumentPath(long paziente, int visita, String nomeFile)
        {
            return ArchivexFileHelper.GetPath(paziente, visita, DOC_FOLDER, nomeFile);
        }

        public static String GetImagePath(long paziente, int visita, String nomeFile)
        {
            return ArchivexFileHelper.GetPath(paziente, visita, IMG_FOLDER, nomeFile);
        }

        public static String GetFilmPath(long paziente, int visita, String nomeFile)
        {
            return ArchivexFileHelper.GetPath(paziente, visita, FILM_FOLDER, nomeFile);
        }

        public static string GetOtherPath(long paziente, int visita, String nomeFile)
        {
            return ArchivexFileHelper.GetPath(paziente, visita, OTHER_FOLDER, nomeFile);
        }
    }
}