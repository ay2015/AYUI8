using System.Linq;
using System.Xml.Linq;

namespace ay.db
{
    public class AppConfig
    {
        private static AppConfig _Singleton = null;
        private static object _Lock = new object();
        static AppConfig CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AppConfig();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AppConfig Instance
        {
            get
            {
                return CreateInstance();
            }
        }
        private string _MySqlConnectionString;
        public string MySqlConnectionString
        {
            get
            {
                if (_MySqlConnectionString == null)
                {
                    _MySqlConnectionString = GetValue(nameof(MySqlConnectionString));
                }
                return _MySqlConnectionString;
            }
        }

        public XDocument xmlDoc = null;
        /// <returns></returns>  
        public string GetValue(string xmlElement, string xmlAttribute = "value", bool reloadXml = false)
        {
            if (xmlDoc == null || reloadXml)
            {
                xmlDoc = XDocument.Load(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location), "section.db.config"));
            }
            var results = from c in xmlDoc.Descendants(xmlElement)
                          select c;
            string s = "";
            foreach (var result in results)
            {
                s = result.Attribute(xmlAttribute).Value.ToString();
                break;
            }
            return s;
        }
    }
}
