using Microsoft.VisualBasic.Devices;
using System;
using System.IO;
using System.Security.AccessControl;

namespace ay.FuncFactory
{
    public partial class AyFuncIO : AyFuncBase
    {
        private static AyFuncIO _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncIO CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncIO();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncIO Instance
        {
            get
            {
                return CreateInstance();
            }
        }


        Computer MyComputer = null;
        /// <summary>   
        /// 重命名文件夹内的所有子文件夹   
        /// AYUI4.2 2016-7-6 11:24:28
        /// 作者AY
        /// </summary>   
        /// <param name="directoryName">文件夹名称</param>   
        /// <param name="newDirectoryName">新子文件夹名称格式字符串</param>   
        public void RenameDirectories(string directoryNamePath, string newDirectoryName)
        {
            if (MyComputer.IsNull())
            {
                MyComputer = new Computer();
            }
            MyComputer.FileSystem.RenameDirectory(directoryNamePath, newDirectoryName);
        }
        public void OpenPlaceAndSelectFile(string filename)
        {
            System.Diagnostics.Process.Start("Explorer.exe", @"/select,"+ filename);
        }
       
        /// <summary>
        /// AYUI4.2 2016-7-6 11:24:28
        /// 作者A Y
        /// </summary>
        /// <param name="filenmamePath">源文件路径</param>
        /// <param name="newFileName">文件名，非绝对路径</param>
        public void RenameFile(string filenmamePath,string newFileName)
        {
            if (MyComputer.IsNull())
            {
                MyComputer = new Computer();
            }
            MyComputer.FileSystem.RenameFile(filenmamePath, newFileName);
        }

     
        /// <summary>
        /// 作用：获取文件夹，不存在，就创建文件夹，并返回文件夹路径
        /// 作者：杨洋 AY
        /// 添加时间：2016-6-19
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        public string GetDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 作用：文件夹授权
        /// 作者：杨洋AY
        /// 添加时间：2016-6-19
        /// </summary>
        /// <param name="DirectoryPath">需要被授权的文件夹，默认是Everyone</param>
        public virtual void AccessDirectoryRights(string DirectoryPath, string DirectoryAuth = "Everyone")
        {
            DirectoryInfo di = Directory.CreateDirectory(DirectoryPath);
            //赋予文件夹权限
            System.Security.AccessControl.DirectorySecurity dirSecurity = di.GetAccessControl();
            dirSecurity.AddAccessRule(new FileSystemAccessRule(DirectoryAuth, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            di.SetAccessControl(dirSecurity);
        }

        /// <summary>
        /// 作用：清空指定的文件夹，但不删除文件夹
        /// 作者：杨洋AY
        /// 添加时间：2016-6-19
        /// </summary>
        /// <param name="dir"></param>
        public virtual void EmptyFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        EmptyFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

        //注册表注册资料

        //string portName = rh.GetRegistryData(Registry.LocalMachine, "SOFTWARE\\TagReceiver\\Params\\SerialPort", "PortName");

        //写注册表：
        //RegistryHelper rh = new RegistryHelper();
        //rh.SetRegistryData(Registry.LocalMachine, "SOFTWARE\\TagReceiver\\Params\\SerialPort", "PortName", portName);


        #region 2016-6-30 10:20:12 新增
        /// <summary>
        /// C#按文件名排序（顺序）
        /// </summary>
        /// <param name="arrFi">待排序数组</param>
        public void SortAsFileNameShun(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return x.Name.CompareTo(y.Name); });
        }


        /// <summary>
        /// C#按文件名排序（倒序）
        /// </summary>
        /// <param name="arrFi">待排序数组</param>
        public void SortAsFileNameNi(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.Name.CompareTo(x.Name); });
        }

        /// <summary>
        /// C#按创建时间排序（顺序）
        /// </summary>
        /// <param name="arrFi">待排序数组</param>
        public void SortAsFileCreationTimeShun(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return x.CreationTime.CompareTo(y.CreationTime); });
        }

        /// <summary>
        /// C#按创建时间排序（倒序）
        /// </summary>
        /// <param name="arrFi">待排序数组</param>
        public void SortAsFileCreationTimeNi(ref FileInfo[] arrFi)
        {
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
        }

        /// <summary>
        /// C#按文件夹名称排序（顺序）
        /// </summary>
        /// <param name="dirs">待排序文件夹数组</param>
        public void SortAsFolderNameShun(ref DirectoryInfo[] dirs)
        {
            Array.Sort(dirs, delegate(DirectoryInfo x, DirectoryInfo y) { return x.Name.CompareTo(y.Name); });
        }


        //private void FolderSort()
        //{
        //    string filePath = "E:\\";
        //    DirectoryInfo di = new DirectoryInfo(filePath);

        //    DirectoryInfo[] arrDir = di.GetDirectories();
        //    SortAsFolderName(ref arrDir);

        //    for (int i = 0; i < arrDir.Length; i++)
        //        Response.Write(arrDir[i].Name + "：<br />");
        //}

        /// <summary>
        /// C#按文件夹名称排序（倒序）
        /// </summary>
        /// <param name="dirs">待排序文件夹数组</param>
        public void SortAsFolderNameNi(ref DirectoryInfo[] dirs)
        {
            Array.Sort(dirs, delegate(DirectoryInfo x, DirectoryInfo y) { return y.Name.CompareTo(x.Name); });
        }

        /// <summary>
        /// C#按文件夹夹创建时间排序（顺序）
        /// </summary>
        /// <param name="dirs">待排序文件夹数组</param>
        public void SortAsFolderCreationTimeShun(ref DirectoryInfo[] dirs)
        {
            Array.Sort(dirs, delegate(DirectoryInfo x, DirectoryInfo y) { return x.CreationTime.CompareTo(y.CreationTime); });
        }

        /// <summary>
        /// C#按文件夹创建时间排序（倒序）
        /// </summary>
        /// <param name="dirs">待排序文件夹数组</param>
        public void SortAsFolderCreationTimeNi(ref DirectoryInfo[] dirs)
        {
            Array.Sort(dirs, delegate(DirectoryInfo x, DirectoryInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });
        }
        #endregion

        #region 2016-8-4 11:24:29 新增，读取流信息
        public  string ReadAllAsString(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public  byte[] ReadAllAsBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        #endregion
    }

}
