using ay.Controls.Info;
using System;
using System.Collections;
using System.Globalization;

namespace ay.AyExpression
{

    public class AyDatePickerHelper
    {
        public static IFormatProvider culture = new CultureInfo("zh-CN", true);
        /// <summary>
        /// 获得月份的天数，或者获得每月的最后一天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int NumOfDays(int year, int month)
        {
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                return 31;
            }
            else if (month == 2)
            {
                return AyDatePickerHelper.LeepYear(year);
            }
            else
            {
                return 30;
            }
        }

        public static int GetDayOfWeek(int year, int month, int date)
        {
            int week;
            if (month == 1)
            {
                month = 13;
                year = year - 1;
            }
            if (month == 2)
            {
                month = 14;
                year = year - 1;
            }
            week = (year / 100) / 4 - 2 * (year / 100) + (year % 100) + (year % 100) / 4 + (13 * (month + 1)) / 5 + date - 1;
            while (week >= 7)
            {
                week = week - 7;
            }
            while (week < 0)
            {
                week = week + 7;
            }
            return week;
        }
        public static int LeepYear(int year)
        {
            int y, x;
            y = year / 10;
            if (year % 10 == 0 && y % 10 == 0)
                x = year % 400;
            else
                x = year % 4;
            if (x == 0)
                return 29;
            else
                return 28;
        }


        private static ChineseLunisolarCalendar china = new ChineseLunisolarCalendar();
        private static Hashtable gHoliday = new Hashtable();
        private static Hashtable nHoliday = new Hashtable();
        private static string[] JQ = { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        private static int[] JQData = { 0, 21208, 42467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };
        static AyDatePickerHelper()
        {
            //公历节日
            gHoliday.Add("0101", "元旦");
            gHoliday.Add("0214", "情人节");
            gHoliday.Add("0305", "雷锋日");
            gHoliday.Add("0308", "妇女节");
            gHoliday.Add("0312", "植树节");
            gHoliday.Add("0315", "消权日");
            gHoliday.Add("0401", "愚人节");
            gHoliday.Add("0501", "劳动节");
            gHoliday.Add("0504", "青年节");
            gHoliday.Add("0601", "儿童节");
            gHoliday.Add("0701", "建党节");
            gHoliday.Add("0801", "建军节");
            gHoliday.Add("0910", "教师节");
            gHoliday.Add("1001", "国庆节");
            gHoliday.Add("1224", "平安夜");
            gHoliday.Add("1225", "圣诞节");

            //农历节日
            nHoliday.Add("0101", "春节");
            nHoliday.Add("0115", "元宵节");
            nHoliday.Add("0505", "端午节");
            nHoliday.Add("0815", "中秋节");
            nHoliday.Add("0909", "重阳节");
            nHoliday.Add("1208", "腊八节");
        }

        /// <summary>
        /// ay 2015年11月19日17:31:25
        /// 增加
        /// </summary>
        /// <param name="Cyear"></param>
        /// <param name="Cmonth"></param>
        /// <param name="Cday"></param>
        /// <returns></returns>
        public static DayInfo GetNongLi(int Cyear, int Cmonth, int Cday)
        {
            DateTime dt = new DateTime(Cyear, Cmonth, Cday);
            DayInfo result = new DayInfo();
            result.Day = GetDay(dt);
            result.TitleType = 0;
           result.Title = result.Day;
            result.Month = GetMonth(dt);
            result.MonthAndDay = result.Month + result.Day;

            if (result.Day == "初一")
            {
                result.Title = result.Month;
                result.TitleType = 1;
            }
            //获得节气，节日，中国农历节日
            string strJQ = GetSolarTerm(dt);
            if (strJQ != "")
            {
                result.JieQi = strJQ;
                result.Title = result.JieQi;
                result.TitleType = 2;
            }

            string strHoliday = GetHoliday(dt);
            if (strHoliday != "")
            {
                result.GongHoliday = strHoliday;
                result.Title = result.GongHoliday;
                result.TitleType = 3;
            }
            string strChinaHoliday = GetChinaHoliday(dt);
            if (strChinaHoliday != "")
            {
                result.NongHoliday = strChinaHoliday;
                result.Title = result.NongHoliday;
                result.TitleType = 4;
            }

            int yearIndex = china.GetSexagenaryYear(dt);
            string yearTG = " 甲乙丙丁戊己庚辛壬癸";
            string yearDZ = " 子丑寅卯辰巳午未申酉戌亥";
            string yearSX = " 鼠牛虎兔龙蛇马羊猴鸡狗猪";
            int year = china.GetYear(dt);
            int yTG = china.GetCelestialStem(yearIndex);
            int yDZ = china.GetTerrestrialBranch(yearIndex);

            result.Year = year;
            result.TianGan = yearTG[yTG].ToString();
            result.DiGan = yearDZ[yDZ].ToString();
            result.ShengXiao = yearSX[yDZ].ToString();


            return result;
        }
        /// <summary>
        /// 获取农历
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaDate(DateTime dt)
        {
            if (dt > china.MaxSupportedDateTime || dt < china.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！", china.MinSupportedDateTime.ToString("yyyy-MM-dd"), china.MaxSupportedDateTime.ToString("yyyy-MM-dd")));
            }
            string str = string.Format("{0} {1}{2}", GetYear(dt), GetMonth(dt), GetDay(dt));
            string strJQ = GetSolarTerm(dt);
            if (strJQ != "")
            {
                str += " (" + strJQ + ")";
            }
            string strHoliday = GetHoliday(dt);
            if (strHoliday != "")
            {
                str += " " + strHoliday;
            }
            string strChinaHoliday = GetChinaHoliday(dt);
            if (strChinaHoliday != "")
            {
                str += " " + strChinaHoliday;
            }

            return str;
        }

        /// <summary>
        /// 获取农历年份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetYear(DateTime dt)
        {
            int yearIndex = china.GetSexagenaryYear(dt);
            string yearTG = " 甲乙丙丁戊己庚辛壬癸";
            string yearDZ = " 子丑寅卯辰巳午未申酉戌亥";
            string yearSX = " 鼠牛虎兔龙蛇马羊猴鸡狗猪";
            int year = china.GetYear(dt);
            int yTG = china.GetCelestialStem(yearIndex);
            int yDZ = china.GetTerrestrialBranch(yearIndex);

            string str = string.Format("[{1}]{2}{3}{0}", year, yearSX[yDZ], yearTG[yTG], yearDZ[yDZ]);
            return str;
        }

        /// <summary>
        /// 获取农历月份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMonth(DateTime dt)
        {
            int year = china.GetYear(dt);
            int iMonth = china.GetMonth(dt);
            int leapMonth = china.GetLeapMonth(year);
            bool isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
            {
                iMonth--;
            }

            string szText = "正二三四五六七八九十";
            string strMonth = isLeapMonth ? "闰" : "";
            if (iMonth <= 10)
            {
                strMonth += szText.Substring(iMonth - 1, 1);
            }
            else if (iMonth == 11)
            {
                strMonth += "十一";
            }
            else
            {
                strMonth += "腊";
            }
            return strMonth + "月";
        }

        /// <summary>
        /// 获取农历日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDay(DateTime dt)
        {
            int iDay = china.GetDayOfMonth(dt);
            string szText1 = "初十廿三";
            string szText2 = "一二三四五六七八九十";
            string strDay;
            if (iDay == 20)
            {
                strDay = "二十";
            }
            else if (iDay == 30)
            {
                strDay = "三十";
            }
            else
            {
                strDay = szText1.Substring((iDay - 1) / 10, 1);
                strDay = strDay + szText2.Substring((iDay - 1) % 10, 1);
            }
            return strDay;
        }

        /// <summary>
        /// 获取节气
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetSolarTerm(DateTime dt)
        {
            DateTime dtBase = new DateTime(1900, 1, 6, 2, 5, 0);
            DateTime dtNew;
            double num;
            int y;
            string strReturn = "";

            y = dt.Year;
            for (int i = 1; i <= 24; i++)
            {
                num = 525948.76 * (y - 1900) + JQData[i - 1];
                dtNew = dtBase.AddMinutes(num);
                if (dtNew.DayOfYear == dt.DayOfYear)
                {
                    strReturn = JQ[i - 1];
                }
            }

            return strReturn;
        }

        /// <summary>
        /// 获取公历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetHoliday(DateTime dt)
        {
            string strReturn = "";
            object g = gHoliday[dt.Month.ToString("00") + dt.Day.ToString("00")];
            if (g != null)
            {
                strReturn = g.ToString();
            }

            return strReturn;
        }

        /// <summary>
        /// 获取农历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaHoliday(DateTime dt)
        {
            string strReturn = "";
            int year = china.GetYear(dt);
            int iMonth = china.GetMonth(dt);
            int leapMonth = china.GetLeapMonth(year);
            int iDay = china.GetDayOfMonth(dt);
            if (china.GetDayOfYear(dt) == china.GetDaysInYear(year))
            {
                strReturn = "除夕";
            }
            else if (leapMonth != iMonth)
            {
                if (leapMonth != 0 && iMonth >= leapMonth)
                {
                    iMonth--;
                }
                object n = nHoliday[iMonth.ToString("00") + iDay.ToString("00")];
                if (n != null)
                {
                    if (strReturn == "")
                    {
                        strReturn = n.ToString();
                    }
                    else
                    {
                        strReturn += " " + n.ToString();
                    }
                }
            }

            return strReturn;
        }

    }
}
