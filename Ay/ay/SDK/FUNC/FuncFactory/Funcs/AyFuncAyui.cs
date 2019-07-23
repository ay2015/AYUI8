using System;
using System.Linq;
using ay.FuncFactory.Base;

namespace ay.FuncFactory
{
    public partial class AyFuncAyui : AyFuncBase
    {
        private static AyFuncAyui _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncAyui CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncAyui();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncAyui Instance
        {
            get
            {
                return CreateInstance();
            }
        }


        public AyVersion? CurrentVersion;
        public virtual AyVersion? GetAyuiVersion()
        {
            if (CurrentVersion.HasValue)
            {
                return CurrentVersion.Value;
            }
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var funcVersion = version.Split('.');
            CurrentVersion = new AyVersion(version[0].ToInt(), version[1].ToInt(), version[2].ToInt(), version[3].ToInt());
            return CurrentVersion;

        }
        public AyVersion ToAyVersion(string version)
        {
            var listOfStrings = (version.Split('.')).ToList();
            var listOfInts = listOfStrings.Select<string, int>(q => Convert.ToInt32(q)).ToList<int>();
            return new AyVersion(listOfInts[0], listOfInts[1], listOfInts[2], listOfInts[3]);
        }

        public static string GetFontAwesomeVersion()
        {
            return "4.7";
        }
    }
}
