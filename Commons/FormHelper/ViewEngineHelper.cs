using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using log4net;

namespace bOS.Commons.FormHelper
{
    public class ViewEngineHelper
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(ViewEngineHelper));

        //Tutti i referti presenti sul disco
        public List<ViewEngine> referti = new List<ViewEngine>();

        #region Properties
        public List<ViewEngine> ViewEngines
        {
            get { return referti; }

        }
        #endregion

        private static ViewEngineHelper instance;

        private ViewEngineHelper() { }

        public static ViewEngineHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ViewEngineHelper();
                }
                return instance;
            }
        }

        

        public String GetRefertoName(String fileName)
        {
            String nomeReferto = Path.GetFileNameWithoutExtension(fileName).ToLower();

            foreach (ViewEngine view in referti)
            {
                if (view.FileName == nomeReferto)
                {
                    return view.Name;
                }
            }

            throw new ViewEngineException("Referto non trovato");
        }

        public ViewEngine GetViewEngine(String fileName)
        {
            String nomeReferto = Path.GetFileNameWithoutExtension(fileName).ToLower();
            logger.Debug(String.Format("Looking for viewengine file {0}", nomeReferto));
             
            foreach (ViewEngine view in referti)
            {
                if (view.FileName == nomeReferto)
                {
                    return view;
                }
            }
            return null;
        }

        public void LoadReferti(String path, String[] prefixes, bOS.Commons.FormHelper.FormHandler.FormHandlerType type)
        {
            string[] filePaths = Directory.GetFiles(path, "*.xml");

            foreach (var file in filePaths)
            {
                String fileName = Path.GetFileName(file).ToLower();

                Boolean manageFile = false;
                if ((prefixes == null) || (prefixes.Count() == 0))
                    manageFile = true;
                else
                {
                    foreach (String prefix in prefixes)
                    {
                        if (fileName.IndexOf(prefix.ToLower()) == 0)
                        {
                            manageFile = true;
                            break;
                        }
                    }
                }

                if (manageFile)
                {
                    logger.Info("Loading file[" + fileName + "]");

                    try
                    {
                        bOS.Commons.FormHelper.FormHandler.XmlForm form = null;
                        switch ( type )
                        {
                            case bOS.Commons.FormHelper.FormHandler.FormHandlerType.XmlFormBasic:
                                form = new bOS.Commons.FormHelper.FormHandler.XmlFormBasic();
                                break;
                            case FormHandler.FormHandlerType.XmlFormBootstrap:
                                form = new bOS.Commons.FormHelper.FormHandler.XmlFormBootstrap();
                                break;
                        } 
                        ViewEngine engine = new ViewEngine(file, form);
                        referti.Add( engine );
                    }
                    catch (Exception err)
                    {
                        logger.Warn(String.Format("File {0} is not e valid viewengine", file), err);
                        
                    }
                    
                }
            }
        }
    }
}