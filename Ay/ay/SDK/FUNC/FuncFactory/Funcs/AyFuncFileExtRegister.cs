using System.Text;
using Microsoft.Win32;
using System.IO;

namespace ay.FuncFactory
{
    /// <summary>
    /// 用于文件格式注册到系统，绑定指定的程序去打开
    /// </summary>
    public class AyFuncFileExtRegister:AyFuncBase
    {
        private static AyFuncFileExtRegister _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncFileExtRegister CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncFileExtRegister();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncFileExtRegister Instance
        {
            get
            {
                return CreateInstance();
            }
        }

        public void SetFileAssociation(string extension, string progID)
        {
            // Create extension subkey
            SetValue(Registry.ClassesRoot, extension, progID);

            // Create progid subkey
            string assemblyFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", @"\");
            StringBuilder sbShellEntry = new StringBuilder();
            sbShellEntry.AppendFormat("\"{0}\" \"%1\"", assemblyFullPath);
            SetValue(Registry.ClassesRoot, progID + @"\shell\open\command", sbShellEntry.ToString());
            StringBuilder sbDefaultIconEntry = new StringBuilder();
            sbDefaultIconEntry.AppendFormat("\"{0}\",0", assemblyFullPath);
            SetValue(Registry.ClassesRoot, progID + @"\DefaultIcon", sbDefaultIconEntry.ToString());

            // Create application subkey
            SetValue(Registry.ClassesRoot, @"Applications\" + Path.GetFileName(assemblyFullPath), "", "NoOpenWith");
        }

        private void SetValue(RegistryKey root, string subKey, object keyValue)
        {
            SetValue(root, subKey, keyValue, null);
        }

        private void SetValue(RegistryKey root, string subKey, object keyValue, string valueName)
        {
            bool hasSubKey = ((subKey != null) && (subKey.Length > 0));
            RegistryKey key = root;

            try
            {
                if (hasSubKey) key = root.CreateSubKey(subKey);
                key.SetValue(valueName, keyValue);
            }
            finally
            {
                if (hasSubKey && (key != null)) key.Close();
            }
        }
    }
}
