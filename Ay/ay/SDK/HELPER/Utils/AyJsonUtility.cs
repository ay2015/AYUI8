using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace ay
{
    /// <summary>
    /// json类
    /// </summary>
    public static class AyJsonUtility
    {
        /// <summary>   
        /// 添加时间转换器   
        /// </summary>   
        /// <param name="serializer"></param>   
        private static void AddIsoDateTimeConverter(JsonSerializer serializer)
        {
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            //定义时间转化格式   
            idtc.DateTimeFormat = "yyyy年MM月dd日";  
            serializer.Converters.Add(idtc);
        }

        /// <summary>   
        /// Json转换配置   
        /// </summary>   
        /// <param name="serializer"></param>   
        private static void SerializerSetting(JsonSerializer serializer)
        {
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //serializer.NullValueHandling = NullValueHandling.Ignore;   
            //serializer.MissingMemberHandling = MissingMemberHandling.Ignore;   
            //serializer.DefaultValueHandling = DefaultValueHandling.Ignore;   
        }

        ///// <summary>   
        ///// 返回结果消息编码   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="sucess"></param>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="data"></param>   
        ///// <returns></returns>   
        //public static string ReturnMessage(bool sucess, int total, string message, string exMessage, string data)
        //{
        //    message = message.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");
        //    exMessage = exMessage.Replace("'", "").Replace("\"", "").Replace("<", "").Replace(">", "");

        //    return string.Format("{{success:{0},total:{1},data:{2},message:\"{3}\",exMessage:\"{4}\"}}",
        //        sucess.ToString().ToLower(), total, data, message, exMessage);
        //}

        ///// <summary>   
        ///// 返回失败信息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <returns></returns>   
        //public static string ReturnFailureMessage(string message, string exMessage)
        //{
        //    return ReturnMessage(false, 0, message, exMessage, "[]");
        //}

        ///// <summary>   
        ///// 返回失败信息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <returns></returns>   
        //public static string ReturnFailureMessageTouch(string message, string exMessage)
        //{
        //    return "{\"success\":\"false\",\"msg\":\"" + exMessage + "\"}";
        //}

        ///// <summary>   
        ///// 返回成功信息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="total"></param>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="objList"></param>   
        ///// <returns></returns>   
        //public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, List<T> objList)
        //{
        //    string data = ListToJson<T>(objList);
        //    return ReturnMessage(true, total, message, exMessage, data);
        //}

        //public static string ReturnSuccessMessageTouch<T>(T obj)
        //{
        //    string data = ObjectToJson<T>(obj);
        //    return data;
        //}

        ///// <summary>   
        ///// 返回成功信息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="total"></param>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="objList"></param>   
        ///// <returns></returns>   
        //public static string ReturnSuccessMessage(string message, string exMessage)
        //{
        //    return ReturnMessage(true, 0, message, exMessage, "[]");
        //}

        ///// <summary>   
        ///// 返回成功信息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="total"></param>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="objList"></param>   
        ///// <returns></returns>   
        //public static string ReturnSuccessMessageTouch(string message, string exMessage)
        //{
        //    return "{\"success\":\"true\",\"msg\":\"" + message + "\"}";
        //}



        ///// <summary>   
        ///// 返回成功信息   
        ///// </summary>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="data">JSON 对象</param>   
        ///// <returns></returns>   
        //public static string ReturnSuccessMessage(string message, string exMessage, string data)
        //{
        //    return ReturnMessage(true, 0, message, exMessage, "[" + data + "]");
        //}

        ///// <summary>   
        ///// 返回成功消息   
        ///// </summary>   
        ///// <typeparam name="T"></typeparam>   
        ///// <param name="total"></param>   
        ///// <param name="message"></param>   
        ///// <param name="exMessage"></param>   
        ///// <param name="obj"></param>   
        ///// <returns></returns>   
        //public static string ReturnSuccessMessage<T>(int total, string message, string exMessage, T obj)
        //{
        //    string data = ObjectToJson<T>(obj);
        //    return ReturnMessage(true, total, message, exMessage, data);
        //}

        /// <summary>   
        /// 把对象列表编码为Json数据   
        /// </summary>   
        /// <typeparam name="T"></typeparam>   
        /// <param name="objList"></param>   
        /// <returns></returns>   
        public static string ListToJson<T>(List<T> objList)
        {
            JsonSerializer serializer = new JsonSerializer();
            SerializerSetting(serializer);
            AddIsoDateTimeConverter(serializer);

            using (TextWriter sw = new StringWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, objList);
                return sw.ToString();
            }
        }

        /// <summary>   
        ///  把一个对象编码为Json数据   
        /// </summary>   
        /// <typeparam name="T"></typeparam>   
        /// <param name="obj"></param>   
        /// <returns></returns>   
        public static string ObjectToJson<T>(T obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            SerializerSetting(serializer);
            AddIsoDateTimeConverter(serializer);

            using (TextWriter sw = new StringWriter())
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }


        /// <summary>   
        /// 根据传入的Json数据，解码为对象(一个)   
        /// </summary>   
        /// <typeparam name="T"></typeparam>   
        /// <param name="data"></param>   
        /// <returns></returns>   
        public static T DecodeObject2<T>(string data)
        {
            data = "{" + data + "}";
            JsonSerializer serializer = new JsonSerializer();
            serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            AddIsoDateTimeConverter(serializer);
            StringReader sr = new StringReader(data);
            return (T)serializer.Deserialize(sr, typeof(T));
        }
        public static T DecodeObject<T>(string data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            AddIsoDateTimeConverter(serializer);
            StringReader sr = new StringReader(data);
            return (T)serializer.Deserialize(sr, typeof(T));


        }

        /// <summary>   
        /// 功能同DecodeObject   
        /// </summary>   
        /// <typeparam name="T"></typeparam>   
        /// <param name="data"></param>   
        /// <returns></returns>   
        public static List<T> DecodeObjectList<T>(string data)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
            AddIsoDateTimeConverter(serializer);
            StringReader sr = new StringReader(data);
            return (List<T>)serializer.Deserialize(sr, typeof(List<T>));
        }

        public static string EncodeAjaxResponseJson(string jsonString, string callback)
        {
            String responseString = "";
            //判断是否jsonp调用   
            if (!String.IsNullOrEmpty(callback))
            {
                //jsonp调用，需要封装回调函数，并返回   
                responseString = callback + "(" + jsonString + ")";
            }
            else
            {
                //普通ajax调用，直接返回Json数据   
                responseString = jsonString;
            }

            return responseString;
        }

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        //public static string ExtGridSortInfo(string property, string direction)
        //{
        //    return string.Format("[{{\"property\":\"{0}\",\"direction\":\"{1}\"}}]", property, direction);
        //}

    }
}
