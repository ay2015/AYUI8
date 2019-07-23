using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ay.FuncFactory
{
    public class AyFuncHttp:AyFuncBase
    {
        private static AyFuncHttp _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncHttp CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncHttp();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncHttp Instance
        {
            get
            {
                return CreateInstance();
            }
        }
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        public bool CanConnectInternet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                return true;
            }
            else
            {
                //未联网
                return false;
            }

            //System.Net.NetworkInformation.Ping ping;
            //System.Net.NetworkInformation.PingReply res;
            //ping = new System.Net.NetworkInformation.Ping();
            //try
            //{
            //    res = ping.Send("www.baidu.com");
            //    if (res.Status != System.Net.NetworkInformation.IPStatus.Success)
            //        return false;
            //    else
            //        return true;
            //}
            //catch 
            //{
            //    return false;
            //}
        }

        public static string UrlEncode(string url)
        {
            return System.Web.HttpUtility.UrlEncode(url, System.Text.Encoding.Unicode); //编码
        }
        public static string UrlDecode(string url)
        {
            return System.Web.HttpUtility.UrlDecode(url, System.Text.Encoding.Unicode);  //解码
        }


        /// <summary>
        /// 执行HTTP GET请求。
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildPostData(parameters);
                }
                else
                {
                    url = url + "?" + BuildPostData(parameters);
                }
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;
            req.UserAgent = "Test";
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    rsp = null;
                }
            }

            if (rsp != null)
            {
                if (rsp.CharacterSet != null)
                {
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 每次读取不大于256个字符，并写入字符串
                char[] buffer = new char[256];
                int readBytes = 0;
                while ((readBytes = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    result.Append(buffer, 0, readBytes);
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    result = new StringBuilder();
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// 将对象封装成parameters
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private IDictionary<string, string> BuildIDicPara<T>(T model, string filter = "")
        {
            string[] filters = filter.Split(',');
            IDictionary<string, string> postData = new Dictionary<string, string>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(model, null) != null && !filters.Contains(prop.Name))
                {

                    postData.Add(prop.Name, prop.GetValue(model, null).ToString());
                }
            }
            return postData;
        }
        /// <summary>
        /// 将对象封装成parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private  IDictionary<string, string> BuildIDicParaPartial<T>(T model, string condition = "")
        {
            string[] conditions = condition.Split(',');
            IDictionary<string, string> postData = new Dictionary<string, string>();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(model, null) != null && conditions.Contains(prop.Name))
                {

                    postData.Add(prop.Name, prop.GetValue(model, null).ToString());
                }
            }
            return postData;
        }

        private  string BuildPostData<T>(T model, string filter = "")
        {
            return BuildPostData(BuildIDicPara<T>(model, filter));
        }
        private  string BuildPostDataPartial<T>(T model, string save = "")
        {
            return BuildPostData(BuildIDicParaPartial<T>(model, save));
        }
        public  string PostData(string url, string postData)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }
        /// <summary>
        /// 开始post请求
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string PostData<T>(string url, T model, string filter = "")
        {
            string postData = BuildPostData<T>(model, filter);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }
        /// <summary>
        /// 开始post请求
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string PostDataPartial<T>(string url, T model, string condition = "")
        {
            string postData = BuildPostDataPartial<T>(model, condition);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// 作者：AY
        /// 时间：2016-6-19 22:12:31
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典。</param>
        /// <returns>URL编码后的请求数据。</returns>
        private string BuildPostData(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(Uri.EscapeDataString(value));
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

    }
}
