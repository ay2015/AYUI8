using Microsoft.Win32;

namespace ay.FuncFactory
{
    /// <summary>
    /// 默认的Get,Write,Delete,IsExist方法是对localMachine下的software下的你的 ayuiconfig.AYUI_RegisterTableSoftwareName的操作
    /// </summary>
    public class AyFuncRegisterTable : AyFuncBase
    {
        private static AyFuncRegisterTable _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncRegisterTable CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncRegisterTable();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncRegisterTable Instance
        {
            get
            {
                return CreateInstance();
            }
        }


        /// <summary>
        /// 读取指定名称的注册表的值 
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public object Get(string name)
        {
            string registData;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(AyFuncConfig.TableSoftwareName, true);
            registData = aimdir.GetValue(name).ToString();
            hkml.Close();
            return registData;
        }

        /// <summary>
        /// 向注册表中写数据 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="tovalue">值</param>
        public void Write(string name, object tovalue)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey software = hklm.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.CreateSubKey(AyFuncConfig.TableSoftwareName);
            aimdir.SetValue(name, tovalue);
        }
        /// <summary>
        /// 2016-6-19 01:30:12
        /// ay增加，用于删除注册表
        /// </summary>
        /// <param name="name"></param>
        public void Delete(string name)
        {
            using (RegistryKey delKey = Registry.LocalMachine.OpenSubKey("Software\\" + AyFuncConfig.TableSoftwareName, true))
            {
                delKey.DeleteValue(name);
            }
        }

        /// <summary>
        /// 判断指定注册表项是否存在 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExist(string name)
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE");
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中  
            foreach (string keyName in subkeyNames)
            //遍历整个数组  
            {
                if (keyName == name)
                //判断子项的名称  
                {
                    hkml.Close();
                    return true;
                }
            }
            hkml.Close();
            return false;
        }



        #region 自定义位置的注册表操作
        /// <summary>
        /// 读取指定名称的注册表的值
        /// </summary>
        public string Get(RegistryKey root, string subkey, string name)
        {
            string registData = "";
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            if (myKey != null)
            {
                registData = myKey.GetValue(name).ToString();
            }

            return registData;
        }

        /// <summary>
        /// 向注册表中写数据
        /// </summary>
        public void Write(RegistryKey root, string subkey, string name, string value)
        {
            RegistryKey aimdir = root.CreateSubKey(subkey);
            aimdir.SetValue(name, value);
        }

        /// <summary>
        /// 删除注册表中指定的注册表项
        /// </summary>
        /// <param name="name"></param>
        public void Delete(RegistryKey root, string subkey, string name)
        {
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            subkeyNames = myKey.GetSubKeyNames();
            foreach (string aimKey in subkeyNames)
            {
                if (aimKey == name)
                    myKey.DeleteSubKeyTree(name);
            }
        }

        /// <summary>
        /// 判断指定注册表项是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist(RegistryKey root, string subkey, string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey myKey = root.OpenSubKey(subkey, true);
            subkeyNames = myKey.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }

            return _exit;
        }

        public const string UninstallReg = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
        #endregion




    }
}
