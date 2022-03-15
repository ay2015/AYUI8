using AyLangManage.SDK;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LangToDevFileConsole
{
  
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static CreateLangStrongFile fileConvert = new CreateLangStrongFile();
        static void Main(string[] args)
        {
            //System.Xml.XmlNode
            Console.Title = "AY国际化程序集支持控制台-20190625";
            IntPtr intptr = FindWindow("ConsoleWindowClass", "AY国际化程序集支持控制台-20190621");
            if (intptr != IntPtr.Zero)
            {
                ShowWindow(intptr, 0);//隐藏这个窗口
            }
            var _d=Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string templateDirectory = System.IO.Path.GetDirectoryName(_d);
            string up1Directory = System.IO.Path.GetDirectoryName(templateDirectory);
            string absolutePath = System.IO.Path.Combine(up1Directory, "Content/Lang");

            fileConvert.ReadLangFiles(absolutePath);
            //获得上级目录
            var _rdname = System.IO.Path.Combine(_d, "Langs.xaml");
            var _csharpname = System.IO.Path.Combine(_d, "Langs.cs");
            fileConvert.ConvertAllResource(_rdname, _csharpname,true);
        }
    }
}
