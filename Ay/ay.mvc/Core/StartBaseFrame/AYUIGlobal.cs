using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Ay.MvcFramework
{
    /// <summary>
    /// 生日 2016-10-21 03:13:55
    /// 作者：AY
    /// </summary>
    public class AYUIGlobal
    {
        /// <summary>
        /// 注册拦截器
        /// </summary>
        /// <param name="filters"></param>
        //public virtual void RegisterGlobalFilters(GlobalFilterCollection filters) { }

        ///// <summary>
        ///// 注册路由
        ///// </summary>
        ///// <param name="routes"></param>
        //public virtual void RegisterRoutes(RouteCollection routes) { }

        /// <summary>
        /// 注册资源字典
        /// </summary>
        /// <param name="routes"></param>
        public virtual void RegisterResourceDictionary(ClientResourceDictionaryCollection resources) { }


        public virtual void Application_Run(Application current) { }
        /// <summary>
        /// 注册客户端字体文件，方便系统使用
        /// </summary>
        /// <param name="routes"></param>
        public virtual void RegisterFonts(ClientFontsCollection languages) { }


        /// <summary>
        /// 注册程序语言,中文文件名必须 zh-cn.xaml
        /// </summary>
        /// <param name="fonts"></param>
        public virtual void RegisterLanuages(ClientLanuagesCollection fonts) { }

        /// <summary>
        /// 应用程序启动时候触发
        /// </summary>
        public virtual void Application_Start(StartupEventArgs e,Application appliation) { }


        /// <summary>
        /// 应用程序结束时候触发
        /// </summary>
        public virtual void Application_End(ExitEventArgs e) { }

        /// <summary>
        /// 当用户注销或关闭系统时发生
        /// </summary>
        public virtual void Application_SessionEnding(SessionEndingCancelEventArgs e) { }


        /// <summary>
        /// 应用程序退出时候
        /// </summary>
        public virtual void Application_Activated(EventArgs e) { }


        /// <summary>
        /// 应用程序退出时候
        /// </summary>
        public virtual void Application_Deactivated(EventArgs e) { }

        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Activated 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.EventArgs。
        //protected virtual void OnActivated(EventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Deactivated 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.EventArgs。
        //protected virtual void OnDeactivated(EventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Exit 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.ExitEventArgs。
        //protected virtual void OnExit(ExitEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.FragmentNavigation 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.FragmentNavigationEventArgs。
        //protected virtual void OnFragmentNavigation(FragmentNavigationEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.LoadCompleted 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigationEventArgs。
        //protected virtual void OnLoadCompleted(NavigationEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Navigated 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigationEventArgs。
        //protected virtual void OnNavigated(NavigationEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Navigating 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigatingCancelEventArgs。
        //protected virtual void OnNavigating(NavigatingCancelEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.NavigationFailed 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigationFailedEventArgs。
        //protected virtual void OnNavigationFailed(NavigationFailedEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.NavigationProgress 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigationProgressEventArgs。
        //protected virtual void OnNavigationProgress(NavigationProgressEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.NavigationStopped 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.Navigation.NavigationEventArgs。
        //protected virtual void OnNavigationStopped(NavigationEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.SessionEnding 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.SessionEndingCancelEventArgs。
        //protected virtual void OnSessionEnding(SessionEndingCancelEventArgs e);
        ////
        //// 摘要:
        ////     引发 System.Windows.Application.Startup 事件。
        ////
        //// 参数:
        ////   e:
        ////     一个包含事件数据的 System.Windows.StartupEventArgs。
        //protected virtual void OnStartup(StartupEventArgs e);
    }
}
