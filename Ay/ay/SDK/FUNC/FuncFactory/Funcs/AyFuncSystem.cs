using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;

namespace ay.FuncFactory
{
    public partial class AyFuncSystem : AyFuncBase
    {
        private static AyFuncSystem _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncSystem CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncSystem();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncSystem Instance
        {
            get
            {
                return CreateInstance();
            }
        }


        public string GetSpecialFolder(System.Environment.SpecialFolder folder)
        {
            return System.Environment.GetFolderPath(folder);
        }

        //设置桌面壁纸
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern int SystemParametersInfo(
         int uAction,
         int uParam,
         string lpvParam,
         int fuWinIni
         );
    

        public virtual void SetDesktopBackgroundPicture(string imageFileAbsolutePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imageFileAbsolutePath);
            string path = System.IO.Path.Combine(GetSpecialFolder(Environment.SpecialFolder.LocalApplicationData), AyFuncConfig.TableSoftwareName, "DesktopPic");
            path= AyFuncIO.Instance.GetDirectory(path);
            var bmpPath = path + AyFuncConfig.TableSoftwareName + ".bmp";
            image.Save(bmpPath, System.Drawing.Imaging.ImageFormat.Bmp);
            SystemParametersInfo(20, 0, bmpPath, 0x2);
        }

        /// <summary>
        /// 杀进程
        /// </summary>
        /// <param name="processName">进程名字</param>
        public virtual void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (Process thisproc in Process.GetProcessesByName(processName))
            {
                if (!thisproc.CloseMainWindow())
                {
                    thisproc.Kill();
                    GC.Collect();
                }
                Process[] prcs = Process.GetProcesses();
                foreach (Process p in prcs)
                {
                    if (p.ProcessName.Equals(processName))
                    {
                        p.Kill();
                    }
                }
            }
        }
        public static Process StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();
                Process myprocess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
                startInfo.WindowStyle= ProcessWindowStyle.Maximized;
                myprocess.StartInfo = startInfo;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                return myprocess;
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 创建一个url文件
        /// </summary>
        /// <param name="lnkName">链接的名字</param>
        /// <param name="uri">链接指定的地址</param>
        public virtual void CreateUrlShortCut(string lnkName, string uri)
        {
            string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹 
            string lName = DesktopPath + "\\" + lnkName + ".url";
            if (File.Exists(lName))
            {
                File.Delete(lName);
            }
            using (StreamWriter writer = new StreamWriter(lName))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + uri);
                writer.Flush();
            }
        }

        public virtual void CopyStream(Stream i, Stream o)
        {
            byte[] b = new byte[32768];
            while (true)
            {
                int r = i.Read(b, 0, b.Length);
                if (r <= 0)
                    return;
                o.Write(b, 0, r);
            }
        }

        /// <summary>
        /// C#获取已安装 .NET Framework 版本
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetDotNetVersions()
        {
            DirectoryInfo[] directories = new DirectoryInfo(Environment.SystemDirectory + @"\..\Microsoft.NET\Framework").GetDirectories("v?.?.*");
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (DirectoryInfo info2 in directories)
            {
                list.Add(info2.Name.Substring(1));
            }
            return (list.ToArray(typeof(string)) as string[]);
        }

        ///<summary>
        /// 获取硬盘卷标号
        ///</summary>
        ///<returns></returns>
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        ///<summary>
        /// 获取CPU序列号
        ///</summary>
        ///<returns></returns>
        private string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuCollection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
            }
            return strCpu;
        }

        public string GetMNum()
        {
            string strNum = GetCpu() + GetDiskVolumeSerialNumber();
            strNum = strNum.Substring(0, 16);    //截取前24位作为机器码
            return strNum;
        }

        #region 暂时不用
        /// <summary>
        /// 创建一个桌面软件快捷方式的lnk文件的链接
        /// </summary>
        /// <param name="lnkName">链接名称，不包含后缀</param>
        /// <param name="targetPath">程序地址</param>
        /// <param name="workingDirectory">程序所在文件夹</param>
        /// <param name="description">描述</param>
        /// <param name="IconLocation">图标地址</param>
        /// <param name="Arguments">附加信息</param>
        /// <param name="Hotkey">快捷键</param>
        //public void CreateSoftShortCut(string lnkName, string targetPath, string workingDirectory, string description,string savePath=null,
        //    string IconLocation = null, string Arguments = "", string Hotkey = null)
        //{

        //    if (savePath == null) {
        //        savePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹 
        //    }
        //    string lName = savePath + "\\" + lnkName + ".lnk";
        //    if (File.Exists(lName))
        //    {
        //        File.Delete(lName);
        //    }
        //    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
        //    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(lName);
        //    shortcut.TargetPath = targetPath;
        //    shortcut.Arguments = Arguments;// 参数 
        //    shortcut.Description = description;
        //    shortcut.WorkingDirectory = workingDirectory;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性 

        //    if (IconLocation == null)
        //    {
        //        shortcut.IconLocation = targetPath + @",0";//图标 
        //    }
        //    else
        //    {
        //        shortcut.IconLocation = IconLocation;
        //    }
        //    if (Hotkey != null)
        //    {
        //        shortcut.Hotkey = Hotkey;//热键 
        //    }
        //    shortcut.WindowStyle = 1;
        //    shortcut.Save();
        //}

        /// <summary>
        /// 设置开机自动运行
        /// </summary>
        /// <param name="mode">true代表 开机自动运行，false代表取消自动运行</param>
        //public void AutoStartWhenSystemRuning(bool mode, string keyName, string filePath, string installDirectory)
        //{

        //    Version currentVersion = Environment.OSVersion.Version;
        //    Version compareToVersion = new Version("6.2");
        //    if (currentVersion.CompareTo(compareToVersion) >= 0)
        //    {
        //        //Console.WriteLine("当前系统是WIN8及以上版本系统。");      
        //        //添加链接方式到启动项
        //        string qd_directory = String.Format(@"C:\Users\{0}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\", Environment.UserName);
        //        if (mode) //设置开机自启动  
        //        {
        //            //C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp
        //            this.CreateSoftShortCut(WizardInfo.LNK_NAME, filePath, installDirectory, WizardInfo.LNK_DESCRIPTION, qd_directory);
        //        }
        //        else
        //        {
        //            string lName = qd_directory + "\\" + WizardInfo.LNK_NAME + ".lnk";
        //            if (File.Exists(lName))
        //            {
        //                File.Delete(lName);
        //            }
        //        }

        //    }
        //    else
        //    {
        //        //Console.WriteLine("当前系统不是WIN8及以上版本系统。");
        //        if (mode) //设置开机自启动  
        //        {
        //            RegistryKey rk = Registry.LocalMachine;
        //            RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
        //            rk2.SetValue(keyName, filePath);
        //            rk2.Close();
        //            rk.Close();
        //        }
        //        else //取消开机自启动  
        //        {
        //            RegistryKey rk = Registry.LocalMachine;
        //            RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
        //            rk2.DeleteValue(keyName, false);
        //            rk2.Close();
        //            rk.Close();
        //        }
        //    }


        /// <summary>
        /// 创建一个uri的lnk文件的链接
        /// </summary>
        /// <param name="lnkName">链接名称，不包含后缀</param>
        /// <param name="workingDirectory">程序所在文件夹</param>
        /// <param name="description">描述</param>
        /// <param name="IconLocation">图标地址</param>
        /// <param name="Arguments">附加信息</param>
        /// <param name="Hotkey">快捷键</param>
        //    public void CreateUrlShortCut(string lnkName, string workingDirectory, string description,
        //       string IconLocation = null, string Arguments = "", string Hotkey = null)
        //{
        //    string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹 
        //    string lName = DesktopPath + "\\" + lnkName + ".lnk";
        //    if (File.Exists(lName))
        //    {
        //        File.Delete(lName);
        //    }
        //    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();
        //    IWshRuntimeLibrary.IWshShortcut shortcutWeb = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(lName);
        //    shortcutWeb.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
        //    shortcutWeb.Arguments = Arguments;// 参数 
        //    shortcutWeb.Description = description;
        //    shortcutWeb.WorkingDirectory = workingDirectory;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性 
        //    if (IconLocation == null)
        //    {
        //        shortcutWeb.IconLocation = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE, 0";//图标 
        //    }
        //    else
        //    {
        //        shortcutWeb.IconLocation = IconLocation;
        //    }

        //    if (Hotkey != null)
        //    {
        //        shortcutWeb.Hotkey = Hotkey;//热键 
        //    }

        //    shortcutWeb.WindowStyle = 1;
        //    shortcutWeb.Save();
        //}
        #endregion

    }
}
