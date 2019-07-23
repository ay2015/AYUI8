using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Ay.MvcFramework
{
    internal class WindowShowHelp
    {
        [DllImport("user32.dll")]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        internal static void ActiveProcess(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            SwitchToThisWindow(handle, true);    // 激活，显示在最前
        }
    }
    //public class ClientApplicationInfo
    //{
    //    public static string ClientAssemblyName = null;
    //}
    /// <summary>
    /// 生日 2016-10-21 04:22:04
    /// 模板模式+代理模式
    /// AY 制作
    /// </summary>
    public class AYUIApplication<T> : Application where T : Window
    {
        #region 全局系统设置 2016-10-21 03:36:37
        private AYUIGlobal global;

        public AYUIApplication()
        {
            //ClientApplicationInfo.ClientAssemblyName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            //AyExtension.ApplicationGlobalPackUriTemplate = string.Format(@"pack://application:,,,/{0};component/", assemblyname);
        }

        /// <summary>
        /// 是否是单例WPF程序
        /// AY：2017-8-16 15:23:08
        /// </summary>
        public bool IsSingleApplication { get; set; } = false;


        public AYUIApplication(AYUIGlobal _global)
        {
            //ClientApplicationInfo.ClientAssemblyName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            //AyExtension.ApplicationGlobalPackUriTemplate = string.Format(@"pack://application:,,,/{0};component/", assemblyname);
            this.global = _global;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_global">全局设置</param>
        /// <param name="_IsSingleApplication">是否是单例WPF程序</param>
        public AYUIApplication(AYUIGlobal _global, bool _IsSingleApplication)
        {
            IsSingleApplication = _IsSingleApplication;
            //ClientApplicationInfo.ClientAssemblyName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            //AyExtension.ApplicationGlobalPackUriTemplate = string.Format(@"pack://application:,,,/{0};component/", assemblyname);
            this.global = _global;
        }

        #endregion
        protected override void OnStartup(StartupEventArgs e)
        {
            if (global!=null)
            {
                global.Application_Start(e, this);
                global.RegisterFonts(GlobalCollection.Fonts);
                global.RegisterResourceDictionary(GlobalCollection.ResourceDictionaryCollection);
                global.RegisterLanuages(GlobalCollection.Lanuages);
                //global.RegisterGlobalFilters(GlobalCollection.Filters);
            }
            if (IsSingleApplication)
            {
                Process currentProcess = Process.GetCurrentProcess();
                Process[] Processes = Process.GetProcessesByName(currentProcess.ProcessName);
                Process nowProcess = null;
                foreach (Process process in Processes)
                {
                    if (process.Id != currentProcess.Id)
                    {
                        nowProcess = process;
                    }
                }

                if (nowProcess == null)
                {
                    global.Application_Run(this);
                    var model = System.Activator.CreateInstance<T>();
                    model.Show();
                }
                else
                {
                    WindowShowHelp.ActiveProcess(nowProcess);
                    this.Shutdown();
                }

            }
            else
            {
                global.Application_Run(this);
                var model = System.Activator.CreateInstance<T>();
                model.Show();
            }
            base.OnStartup(e);

        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            if (global!=null) global.Application_SessionEnding(e);
            base.OnSessionEnding(e);
        }
        protected override void OnActivated(EventArgs e)
        {
            if (global!=null) global.Application_Activated(e);
            base.OnActivated(e);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            if (global!=null) global.Application_Deactivated(e);
            base.OnDeactivated(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (global!=null) global.Application_End(e);
            base.OnExit(e);
        }

    }
    public static class ApplicationExt
    {
        /// <summary>
        /// xaml样式添加
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static Application AddResourceDictionary(this Application app, string path)
        {
            try
            {
                Ay.MvcFramework.GlobalCollection.ResourceDictionaryCollection.Add(new ResourceDictionary() { Source = new Uri(path, UriKind.RelativeOrAbsolute) });
                return app;
            }
            catch
            {
                return app;
            }
        }
    }
}
