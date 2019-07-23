using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ay.mvc.CommonConvert
{
    /// <summary>
    /// 放置的非WPF类型的常用转换，一般理解POCO转换
    /// </summary>
    public static class AyCommonConvert
    {
        public static T FromType<T, TK>(TK text)
        {
            try
            {
                return (T)Convert.ChangeType(text, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 1和"true"返回   true，否则返回false
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static bool ToBoolByByte(this object obj)
        {
            try
            {
                string s = obj.ToObjectString().ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }
        #region <<时间扩展>>

        /// <summary>
        /// 例如2012-03-22 12:22:24 可以转换成20120322122224
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDate2IntString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 获得本周，上周，下周
        /// 2013年11月6日15:38:54 杨洋写
        /// </summary>
        /// <param name="dts">时间</param>
        /// <param name="day">差天，-7就是上周，7就是下周</param>
        /// <returns></returns>
        public static DateTime[] GetWeekDate(this DateTime dts, int day = 0)
        {
            DateTime dt = dts.AddDays(day);
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一
            DateTime endWeek = startWeek.AddDays(6);  //本周周日
            return new DateTime[] { startWeek, endWeek };
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetBeginTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetEndTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }
        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? GetBeginTime(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return dateTime;
            }
            return new Nullable<DateTime>(dateTime.Value.GetBeginTime());
        }
        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? GetEndTime(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return dateTime;
            }
            return new Nullable<DateTime>(dateTime.Value.GetEndTime());
        }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static string GetYYYYMMddDateTime(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// yyyy{0}MM{0}dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="splitchar">分隔符/或者-</param>
        /// <returns></returns>
        [Pure]
        public static string GetYYYYMMddDateTime(this DateTime obj, string splitchar)
        {
            return obj.ToString(string.Format("yyyy{0}MM{0}dd HH:mm:ss", splitchar));
        }

        #endregion

        /// <summary>
        /// string.Format(source, args);
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string StringFormat(this string source, params object[] args)
        {
            Contract.Requires(source != null);
            Contract.Requires(args != null);
            return string.Format(source, args);
        }
        #region <<字符串长度验证扩展>>
        /// <summary>
        /// 判断此字符串是否超过指定长度  length >= s.Length
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static bool ValidateLength(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            return length >= s.Length;
        }
        #endregion

        /// <summary>
        ///  string转数组[1,2]转 '1','2'
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        [Pure]
        public static string ToArrayString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            else
            {
                return "'" + text.Replace(",", "','") + "'";
            }
        }

        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static Byte[] GetByte(this object obj)
        {
            if (!string.IsNullOrEmpty(obj.ToObjectString()))
            {
                return (Byte[])obj;
            }
            else
                return null;
        }

        /// <summary>
        /// string 的Uri转 Uri类型
        /// </summary>
        /// <param name="Uri"></param>
        /// <returns></returns>
        public static Uri ToUri(this string Uri)
        {
            return new Uri(Uri, UriKind.RelativeOrAbsolute);
        }


        /// <summary>
        /// guid按照英文逗号分隔成List<Guid>
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Pure]
        public static List<Guid> ToGuidList(this string ids)
        {
            if (ids.IsNull()) return null;

            return ids.Split(',').Select(x =>
            {
                Guid gid = Guid.Empty;
                Guid.TryParse(x, out gid);
                if (gid != Guid.Empty)
                {
                    return gid;
                }
                return Guid.Empty;
            }

            ).ToList();
        }
        /// <summary>
        /// 默认是 逗号分割
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Pure]
        public static List<string> ToStringList(this string ids)
        {
            if (ids.IsNull()) return null;

            return ids.Split(',').ToList();
        }
        [Pure]
        public static List<string> ToStringList(this string ids, char splitchar)
        {
            if (ids.IsNull()) return null;

            return ids.Split(splitchar).ToList();
        }
        [Pure]
        public static string ToObjectString(this object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }
        [Pure]
        public static string ToGuidStringNoSplit(this Guid obj, string addBeforeChar = "", bool isUpperCase = false)
        {
            if (isUpperCase)
            {
                return addBeforeChar + obj.ToObjectString().ToUpper().Replace("-", "");
            }
            else
            {
                return addBeforeChar + obj.ToObjectString().ToLower().Replace("-", "");
            }

        }


        /// <summary>
        /// 切勿转换用于double的字符串 转int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static int ToInt(this object obj)
        {
            int ad;
            if (int.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 处理，例如03的字符串，即返回3，如果"2"的字符串，那么返回2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Pure]
        public static int ToInt2(this object obj)
        {
            int ad;
            string ass = obj.ToObjectString();
            if (ass[0] == '0')
            {
                if (int.TryParse(ass[1].ToObjectString(), out ad))
                {
                    return ad;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (int.TryParse(obj.ToObjectString(), out ad))
                {
                    return ad;
                }
                else
                {
                    return 0;
                }
            }


        }
        [Pure]
        public static Guid ToGuid(this object obj)
        {
            Guid gid = Guid.Empty;
            Guid.TryParse(ToObjectString(obj), out gid);
            if (gid != Guid.Empty)
            {
                return gid;
            }
            return Guid.Empty;
        }
        [Pure]
        public static long ToLong(this object obj)
        {
            long ad;
            if (long.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1L;
            }
        }
        [Pure]
        public static decimal ToDecimal(this object obj)
        {
            decimal ad;
            if (decimal.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1M;
            }

        }
        [Pure]
        public static double ToDouble(this object obj)
        {
            double ad;
            if (double.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return 0.0;
            }


        }
        [Pure]
        public static float ToFloat(this object obj)
        {
            float ad;
            if (float.TryParse(obj.ToObjectString(), out ad))
            {
                return ad;
            }
            else
            {
                return -1;
            }
        }
        [Pure]
        public static DateTime ToDateTime(this object obj)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return dt;
                return DateTime.Now;
            }
            catch
            { return DateTime.Now; }
        }

        [Pure]
        public static byte ToByteByBool(this object obj)
        {
            string text = ToObjectString(obj).Trim();
            if (text == string.Empty)
                return 0;
            else
            {
                try
                {
                    return (byte)(text.ToLower() == "true" ? 1 : 0);
                }
                catch
                {
                    return 0;
                }
            }
        }


        [Pure]
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        [Pure]
        public static bool IsEmptyAndNull(this string obj)
        {
            return string.IsNullOrEmpty(obj.ToObjectString());
        }
        [Pure]
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }
        [Pure]
        public static bool IsNullAndTrimAndEmpty(this object obj)
        {
            if (obj.IsNull()) return true;
            return string.IsNullOrEmpty(obj.ToObjectString().Trim());
        }
        [Pure]
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return str == null || str.Trim().Length == 0;
        }

        [Pure]
        public static bool IsNotNullAndMinusOne(this string obj)
        {
            return obj != null && obj != "-1";
        }
        [Pure]
        public static bool IsNull(this Guid obj)
        {
            return obj == Guid.Empty;
        }
        [Pure]
        public static bool IsNotNull(this Guid obj)
        {
            return obj != Guid.Empty;
        }
        [Pure]
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}






