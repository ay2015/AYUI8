using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.contentcore
{
    public partial class AyFuncDisk 
    {
        private static AyFuncDisk _Singleton = null;
        private static object _Lock = new object();
        internal static AyFuncDisk CreateInstance()
        {
            if (_Singleton == null) //双if +lock
            {
                lock (_Lock)
                {
                    if (_Singleton == null)
                    {
                        _Singleton = new AyFuncDisk();
                    }
                }
            }
            return _Singleton;
        }
        /// <summary>
        /// 对外操作实例
        /// </summary>
        public static AyFuncDisk Instance
        {
            get
            {
                return CreateInstance();
            }
        }



        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public virtual double GetHardDiskFreeSpace(string str_HardDiskName)
        {
            double freeSpace = new double();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / 1073741824.00;// 1024 * 1024 * 1024;
                }
            }
            return Math.Round(freeSpace, 2);
        }

        public virtual double GetHardDiskFreeSpace1(string str_HardDiskName)
        {
            double freeSpace = new double();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace;// 1024 * 1024 * 1024;
                }
            }
            return freeSpace;
        }
        public virtual double GetHardDiskSpace1(string str_HardDiskName)
        {
            double totalSize = new double();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize;
                }
            }
            return totalSize;
        }


        ///  <summary> 
        /// 获取指定驱动器的空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 （大写）</param> 
        ///  <returns> </returns> 
        public virtual double GetHardDiskSpace(string str_HardDiskName)
        {
            double totalSize = new double();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize / 1073741824.00;
                }
            }
            return Math.Round(totalSize, 2);
        }

        #region 设置数据长度值
        private const double B = 1d;
        private const double K = 1024 * 1d;
        private const double M = 1024 * 1024 * 1d;
        private const double G = 1024 * 1024 * 1024 * 1d;
        private const String templete = "{0:N2}";
        private const String templete2 = "{0}";
        #endregion

        string RegexExpression = @"^\+?[1-9][0-9]*$";

        /// <summary>
        /// 根据值的大小输出格式化后的计算机表示值
        /// </summary>
        /// <param name="size">文件或者文件夹尺寸，例如1991kb的文件夹，那么传递1991的参数</param>
        /// <returns></returns>
        public String GetFileOrDirectoryFormatedSize(double size)
        {
            double result = 0;
            String format = templete;
            string ho = null;
            if (size > G)
            {
                result = size / G;
                format += "G";
                ho = "G";
            }
            else if (size > M)
            {
                result = size / M;
                format += "M";
                ho = "M";
            }
            else if (size > K)
            {
                result = size / K;
                format += "K";
                ho = "K";
            }
            else
            {
                result = size / B;
                format += "B";
                ho = "B";
            }


            if (System.Text.RegularExpressions.Regex.IsMatch(result.ToString(), RegexExpression, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                var _1 = String.Format("{0}" + ho, result);
                if (_1 == "1024G") return "1T";
                if (_1 == "1024M") return "1G";
                if (_1 == "1024K") return "1M";
                return _1;
            }
            else
            {
                return String.Format(format, result);
            }
        }
    }
}
