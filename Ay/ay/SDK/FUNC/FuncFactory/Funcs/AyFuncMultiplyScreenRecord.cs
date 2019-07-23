using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ay.FuncFactory
{
    [Serializable]
    public class AyFuncMultiplyScreenRecord : AyFuncBase
    {
        private static AyFuncMultiplyScreenRecord _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncMultiplyScreenRecord CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncMultiplyScreenRecord();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncMultiplyScreenRecord Instance
        {
            get
            {
                return CreateInstance();
            }
        }

        public string data = AyFuncConfig.ScreenRecordConfigFileNamePath;

        #region 保存与读取屏幕配置信息
        /// <summary>
        /// 注册--保存屏幕配置信息
        /// </summary>
        public void SaveSystemWindowInScreenStatus(Dictionary<string, SystemWindowInScreenStatus> systemWindowInScreenStatus)
        {
            using (FileStream fs = new FileStream(data, FileMode.Create))
            {
                //二进制格式化
                BinaryFormatter bf = new BinaryFormatter();
                //序列化
                bf.Serialize(fs, systemWindowInScreenStatus);
            }
        }
        /// <summary>
        /// 读取屏幕配置信息,string是显示器名字
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, SystemWindowInScreenStatus> LoadSystemWindowInScreenStatus()
        {
            Dictionary<string, SystemWindowInScreenStatus> ss = null;

            if (File.Exists(data))
            {
                using (FileStream fs = new FileStream(data, FileMode.Open))
                {
                    //二进制格式化
                    BinaryFormatter bf = new BinaryFormatter();
                    //反序列化
                    ss = (Dictionary<string, SystemWindowInScreenStatus>)bf.Deserialize(fs);
                }
            }
            return ss;
        }
        #endregion
    }


}
