using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using log4net;

namespace bOS.Commons.IO
{
    public class FileHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(FileHelper));

        public String FileName { get; set; }

        public FileHelper(String path, String prefix, String extension)
        {
            FileName = String.Format("{0}/{1}_{2}.{3}", path, prefix, Guid.NewGuid().ToString(), extension);
        }

        public FileHelper(String prefix, String extension)
        {
            FileName = String.Format("{0}_{1}.{2}", prefix, Guid.NewGuid().ToString(), extension);
        }

        public FileHelper(String fileName, Boolean bPath)
        {
            if (bPath)
            {
                FileName = String.Format("{0}_{1}", Guid.NewGuid().ToString(), System.IO.Path.GetFileName(fileName));
            }
            else
            {
                FileName = String.Format("{0}_{1}", Guid.NewGuid().ToString(), fileName);
            }
        }


        public FileHelper(String path, String fileName, Boolean bPath)
        {
            if (bPath)
            {
                FileName = String.Format("{0}/{1}_{2}", path, Guid.NewGuid().ToString(), System.IO.Path.GetFileName(fileName));
            }
            else
            {
                if (fileName.ToUpper().StartsWith("HTTP://"))
                {
                    FileName = String.Format("{0}/{1}", path, Guid.NewGuid().ToString() + System.IO.Path.GetExtension(fileName).ToLower());
                    //SharePointHelper.instance().DownloadFile(fileName, FileName);
                }
                else
                {

                    FileName = String.Format("{0}/{1}_{2}", path, Guid.NewGuid().ToString(), FilesHelper.GetNameFile(fileName));
                }
            }
        }

        
    }

    public class FilesHelper
    {

        protected static readonly ILog logger = LogManager.GetLogger(typeof(FilesHelper));

        public static FileHelper GetTempFile(String path, String prefix, String extension)
        {
            return new FileHelper(path, prefix, extension);
        }

        public static FileHelper GetTempFile(String prefix, String extension) 
        {
            return new FileHelper(prefix, extension);
        }

        public static FileHelper GetTempFile(String path, String fileName, bool bPath)
        {
            return new FileHelper(path, fileName, bPath);
        }

        public static FileHelper GetTempFile(String fileName, bool bPath)
        {
            return new FileHelper(fileName, bPath);
        }

        public static Boolean CheckExtension(String fileName, String[] extensions)
        {
            String fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
            
            return extensions.Count(s=>s.Equals(fileExtension)) > 0;

        }

        public static String GetNameFile(String fileName)
        {
            return System.IO.Path.GetFileName(fileName);
        }

        public static byte[] GetFileAsByte(string file)
        {
            FileStream fs1 = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new System.IO.BinaryReader(fs1);
            byte[] data2 = br.ReadBytes((int)fs1.Length);
            br.Close();
            fs1.Close();

            return data2;
        }

        public static long GetFileLength(string file)
        {

            

            FileStream fs1 = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new System.IO.BinaryReader(fs1);
            return fs1.Length;
            
        }

        public static DateTime? GetLastWriteTime(string file)
        {

            
            return File.GetLastWriteTime(file);

        }



        public static Boolean GetExistsFile(string file)
        {
            return System.IO.File.Exists(file);            
        }

        public static int GetNumFile(string path)
        {
            
            return System.IO.Directory.GetFiles(path).Length;
        }

        public static int DeleteFile(string path)
        {
            foreach (string file in System.IO.Directory.GetFiles(path))
            {                
                    System.IO.File.Delete(file);                
            }            
            return System.IO.Directory.GetFiles(path).Length;
        }

        public static int DeleteFiles(string path, Boolean recursive)
        {
            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                System.IO.File.Delete(file);
            }

            if (recursive)
            {
                foreach (string d in Directory.GetDirectories(path))
                {
                    DeleteFiles(d, true);
                }
            }
            
            return System.IO.Directory.GetFiles(path).Length;
        }

        public static void ReleaseFile(String fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception err)
                {
                    logger.Error (String.Format ("Impossible to delete file [{0}]", fileName),err);
                }
            }
        }
    }
}