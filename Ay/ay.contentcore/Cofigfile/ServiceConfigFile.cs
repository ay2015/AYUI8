using System.Xml;

namespace ay
{
    public sealed class ServiceConfigFile : IAyConfigManager
    {
        public ServiceConfigFile()
        {

        }
        public ServiceConfigFile(string filepath)
        {
            this.XmlFilePath = filepath;
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
                    doc.Load(XmlFilePath);
                }
                return doc;
            }
        }
        public string XmlFilePath { get; set; }

        public void Reload()
        {
            DOC.Load(XmlFilePath);
        }

        public string GetValue(string key)
        {
            XmlElement element = (XmlElement)DOC.SelectSingleNode("Application/" + key);
            if (element != null)
            {
                return element.Attributes["value"].Value;
            }
            return string.Empty;
        }

        public void SetValue(string key, string value)
        {
            XmlElement element = (XmlElement)DOC.SelectSingleNode("Application/" + key);
            if (element == null) return;
            element.SetAttribute("value", value);
            DOC.Save(XmlFilePath);
        }

        public void RemoveValue(string key)
        {
         
        }
    }

}
