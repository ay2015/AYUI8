using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ay.Framework.DataCreaters
{
    public class AyPhone
    {
        private static string[] telStarts = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');
        static Random ran = new Random();
        /// <summary>
        /// 随机生成电话号码
        /// </summary>
        /// <returns></returns>
        public static string PhoneNumber()
        {
            int n = ran.Next(10, 1000);
            int index = ran.Next(0, telStarts.Length - 1);
            string first = telStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }
        //Clipboard.SetDataObject(tt);
        //Clipboard.SetDataObject(tt);
    }


    
}
