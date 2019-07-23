using System.Xml.Linq;
using System.Linq;

namespace ay
{
    public class AyWindowShellConfigSetting
    {
        public static XDocument xmlDoc = null;
        public AyWindowShellConfigSetting() { }

        /// <summary>  
        /// 返回XMl文件指定元素的指定属性值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <param name="xmlpath">读取的xml文件名字</param>  
        /// <param name="reloadXml">是否重新加载XML</param>  
        /// <returns></returns>  
        public static string GetXmlValue(string xmlElement, string xmlAttribute = "value", bool reloadXml = false)
        {
            if (xmlDoc == null || reloadXml)
            {
                xmlDoc = XDocument.Load(AyGlobalConfig.AYUI_ConfigFileNamePath);
            }
            var results = from c in xmlDoc.Descendants(xmlElement)
                          select c;
            string s = "";
            foreach (var result in results)
            {
                s = result.Attribute(xmlAttribute).Value.ToString();
            }
            return s;
        }

        /// <summary>  
        /// 设置XMl文件指定元素的指定属性的值  
        /// </summary>  
        /// <param name="xmlElement">指定元素</param>  
        /// <param name="xmlAttribute">指定属性</param>  
        /// <param name="xmlValue">指定值</param>  
        public static void SetXmlValue(string xmlElement, string xmlAttribute, string xmlValue, string xmlRootName = "Application", bool reloadXml = false)
        {
            if (xmlDoc == null || reloadXml)
            {
                xmlDoc = XDocument.Load(AyGlobalConfig.AYUI_ConfigFileNamePath);
            }
            xmlDoc.Element(xmlRootName).Element(xmlElement).Attribute(xmlAttribute).SetValue(xmlValue);
            xmlDoc.Save(AyGlobalConfig.AYUI_ConfigFileNamePath);
        }
    }

}
