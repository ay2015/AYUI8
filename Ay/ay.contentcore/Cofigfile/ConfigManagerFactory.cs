using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ay
{

    public class ConfigManagerFactory
    {
        private static string _AppDocumentFolderName;
        public static string AppDocumentFolderName
        {
            get
            {
                return _AppDocumentFolderName;
            }
            set
            {
                _AppDocumentFolderName = value;
                configfile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\"+ _AppDocumentFolderName + @"\Config\Config.xml";
                serviceconfig = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + _AppDocumentFolderName + @"\Config\ServiceConfig.xml";
                //ControlLoveInstance.ControlLoveConfig= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\"+ _AppDocumentFolderName + @"\Config\ControlLoveConfig.xml";
         
            }
        }
        public static string configfile = null;
        public static string serviceconfig = null;
        public static IAyConfigManager Create(ManagerFile managerFile)
        {
            switch (managerFile)
            {
                case ManagerFile.Config:

                    return new ConfigFile(configfile);
                case ManagerFile.ServiceConfig:
                    return new ServiceConfigFile(serviceconfig);
            }
            return null;
        }
    }

    public enum ManagerFile
    {
        Config,
        ServiceConfig
    }

}
