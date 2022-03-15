using System.IO;
using System.Xml;

namespace ay
{
    public sealed class ConfigFile : IAyConfigManager
    {
        public ConfigFile()
        {

        }
        string rootname = "sys";
        public ConfigFile(string filepath,string _rootname)
        {
            this.XmlFilePath = filepath;
            this.rootname = _rootname;
            if (!File.Exists(XmlFilePath))
            {
                //创建一个文件
                Create(rootname);
            }
        }

        private void Create(string rootname)
        {
            if (XmlFilePath == null) return;
            string p = Path.GetDirectoryName(XmlFilePath);
            if (!Directory.Exists(p))
            {
                DirectoryInfo d1 = new DirectoryInfo(p);
                d1.Create();
            }
   
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement(rootname);
            xmldoc.AppendChild(xmlelem);
            xmldoc.Save(XmlFilePath);
            doc = xmldoc;
        }

        public ConfigFile(string filepath)
        {
            this.XmlFilePath = filepath;
            if (!File.Exists(XmlFilePath))
            {
                //创建一个文件
                Create(rootname);
            }
        }
        public string this[string key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        private XmlDocument doc;
        internal XmlDocument DOC
        {
            get
            {
                if (doc == null)
                {
                    doc = new XmlDocument();

                    //doc.Load(XmlFilePath);
                }
                return doc;
            }
        }
        public string XmlFilePath { get; set; }

        public void Reload()
        {
            if (XmlFilePath == null) return;
            DOC.Load(XmlFilePath);
        }

        public string GetValue(string key)
        {
            XmlNode xnRoot = DOC.DocumentElement;
            if (xnRoot == null)
            {
                Reload();
                xnRoot = DOC.DocumentElement;
            }
            if (xnRoot == null)
            {
                return "";
            }
            XmlNodeList xe = xnRoot.ChildNodes;
            for (int j = 0; j < xe.Count; j++)
            {
                XmlElement _1 = xe[j] as XmlElement;
                if (_1 != null && _1.Attributes["key"].Value == key)
                {
                    return _1.Attributes["value"].Value;
                }
            }
            return string.Empty;
        }

        public void SetValue(string key, string value)
        {
            XmlNode xnRoot = DOC.DocumentElement;
            if (xnRoot == null)
            {
                Reload();
                xnRoot = DOC.DocumentElement;
            }
            if (xnRoot == null)
            {
                return ;
            }
            XmlNodeList xe = xnRoot.ChildNodes;

            bool hasFind = false;
            for (int j = 0; j < xe.Count; j++)
            {
                XmlElement _1 = xe[j] as XmlElement;
                if (_1 != null && _1.Attributes["key"].Value == key)
                {
                    _1.Attributes["value"].Value = value;
                    hasFind = true;
                    DOC.Save(XmlFilePath);
                    break;
                }
            }
            if (!hasFind)
            {
                XmlNode xmldocSelect = DOC.SelectSingleNode(rootname);
                XmlElement pe = DOC.CreateElement("add");
                pe.SetAttribute("key", key);
                pe.SetAttribute("value", value);
                xmldocSelect.AppendChild(pe);
                DOC.Save(XmlFilePath);
            }
        }

        public void RemoveValue(string key)
        {
            XmlNode xnRoot = DOC.DocumentElement;
            if (xnRoot == null)
            {
                Reload();
                xnRoot = DOC.DocumentElement;
            }
            if (xnRoot == null)
            {
                return;
            }
            XmlNodeList xe = xnRoot.ChildNodes;
            for (int j = 0; j < xe.Count; j++)
            {
                XmlElement _1 = xe[j] as XmlElement;
                if (_1 != null && _1.Attributes["key"].Value == key)
                {
                    XmlNode xmldocSelect = DOC.SelectSingleNode(rootname);
                    xmldocSelect.RemoveChild( _1);
                    DOC.Save(XmlFilePath);
                    break;
                }
            }
         
        }

    }

}
