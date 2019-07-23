using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ay
{
    /// <summary>
    /// 模拟键盘
    /// 对应WIN10、WIN8下的平板系统对应的系统模拟键盘
    /// </summary>
    public class AnalogKeyBox
    {
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegisterWindowMessage(string lpString);

        /// <summary>
        /// 显示屏幕键盘
        /// </summary>
        /// <returns></returns>
        public static int ShowInputPanel()
        {
            try
            {
                dynamic file = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
                if (!System.IO.File.Exists(file))
                    return -1;
                Process.Start(file);
                //return SetUnDock(); //不知SetUnDock()是什么，所以直接注释返回1
                return 1;
            }
            catch (Exception)
            {
                return 255;
            }
        }


        ///// <summary>
        /////  异步开启模拟键盘
        ///// </summary>
        ///// <returns></returns>
        //public static async Task ShowInputPanelAsync()
        //{
        //    await Task.Run(() => ShowInputPanel());
        //}
        /// <summary>
        /// 隐藏屏幕键盘
        /// </summary>
        public static void HideInputPanel()
        {
            IntPtr TouchhWnd = new IntPtr(0);
            TouchhWnd = FindWindow("IPTip_Main_Window", null);
            if (TouchhWnd == IntPtr.Zero)
                return;
            PostMessage(TouchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
        }
    }

    public class SystemHelper
    {
    
        public static void ShowKeyBoard()
        {
            ShowKeyboard();
        }
        private static void ShowKeyboard()
        {
            var path64 = Path.Combine(Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "winsxs"), "amd64_microsoft-windows-osk_*")[0], "osk.exe");
            var path32 = @"C:\windows\system32\osk.exe";
            var path = (Environment.Is64BitOperatingSystem) ? path64 : path32;
            Process.Start(path);
        }

        

    }




}
