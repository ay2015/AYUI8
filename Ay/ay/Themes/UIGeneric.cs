using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace ay.Utils
{
    public class UIGeneric
    {

        //[DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps", SetLastError = true)]
        //public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        private static double? _DayWidth;
        /// <summary>
        /// 日期宽度
        /// </summary>
        public static double? DayWidth
        {
            get
            {
                if (_DayWidth == null)
                {
                    var _1= Application.Current.Resources["DayWidth"];
                    if (_1 != null)
                    {
                        _DayWidth = _1.ToDouble();
                    }
                    else
                    {
                        _DayWidth = 40;
                    }
                }
                return _DayWidth;
            }
        }


    }
}
