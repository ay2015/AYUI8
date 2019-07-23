using ay.Controls.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace ay.AyExpression
{
    /// <summary>
    /// AY为AyDateBoxCalendar,AyCalendar提供的日期表达式
    /// 生日：2017-2-24 11:48:50
    /// 语法：%y	当前年
    ///           %M 当前月
    ///           %d 当前日
    ///           %ld 本月最后一天
    ///           %H 当前时
    ///           %m 当前分
    ///           %s 当前秒
    ///           { } 运算表达式,如:{%d+1}:表示明天
    ///           关于#F{}用 这里不作处理，在控件中处理
    /// </summary>
    public class AyDateStrictExpression
    {
        const string teSepc = "c";
        internal static DateTime Convert(string expression)
        {
            var _afterReplaceText = Regex.Replace(expression, @"{(?<c>.*?)}", teSepc);
            MatchCollection matches = Regex.Matches(expression, @"{(?<c>.*?)}");
            List<string> strs = new List<string>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string _1 = groups["c"].Value;
                if (_1 != null)
                {
                    strs.Add(groups["c"].Value);
                }
            }
            var dtnow = DateTime.Now;
            int curSpecIndex = 0;
            DateTime dtReturn = new DateTime();
            var _p1 = _afterReplaceText.Split(' ');
            if (_p1.Length == 1)
            {
                //判断是年月还是时间
                var _text = _p1[0];
                if (_text.IndexOf(":") > -1)  //时间方式处理
                {
                    ReturnTimeDateTime(teSepc, strs, dtnow, ref curSpecIndex, ref dtReturn, _text);
                    dtReturn = new DateTime(dtnow.Year, dtnow.Month, dtnow.Day, dtReturn.Hour, dtReturn.Minute, dtReturn.Second);
                }
                else//年月日方式处理
                {
                    ReturnDateDateTime(teSepc, strs, dtnow, ref curSpecIndex, ref dtReturn, _text);
                }
            }
            else if (_p1.Length == 2)
            {
                //判断是年月 时间处理
                var _date = _p1[0];
                var _time = _p1[1];
                //必须按照年月日时分秒方式去处理
                ReturnDateDateTime(teSepc, strs, dtnow, ref curSpecIndex, ref dtReturn, _date);
                //处理时间 Time
                ReturnTimeDateTime(teSepc, strs, dtnow, ref curSpecIndex, ref dtReturn, _time);

            }
            else
            {
                MessageBox.Show("[AY日期限制表达式]语法错误,语句为：" + expression);
            }
            return dtReturn;
        }

        private static void ReturnTimeDateTime(string teSepc, List<string> strs, DateTime dtnow, ref int curSpecIndex, ref DateTime dtReturn, string _time)
        {
            var _p3 = _time.Split(':');
            int _p3Length = _p3.Length;
            for (int i = 0; i < _p3Length; i++)
            {
                var _p3_0 = _p3[i];
                //当是特殊标记的时候处理
                if (_p3_0 == teSepc)
                {
                    if (strs.Count > curSpecIndex)
                    {
                        string _guize = strs[curSpecIndex];
                        string _yanma = null;
                        char oper = '+';
                        int operIndex = 2;
                        oper = _guize[operIndex];
                        _yanma = _guize.Substring(0, 2);

                        int operNo = _guize.Substring(operIndex).ToInt();
                        //if (oper == '-')
                        //{
                        //    operNo = operNo * (-1);
                        //}
                        if (_yanma == "%H")
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtnow.Hour, 0, 0);
                            dtReturn = dtReturn.AddHours(operNo);
                        }
                        else if (_yanma == "%m")
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, dtnow.Minute, 0);
                            dtReturn = dtReturn.AddMinutes(operNo);
                        }
                        else if (_yanma == "%s")
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, dtReturn.Minute, dtnow.Second);
                            dtReturn = dtReturn.AddSeconds(operNo);
                        }

                        curSpecIndex++;

                    }
                }
                else if (_p3_0 == "%H")
                {
                    dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtnow.Hour, 0, 0);
                }
                else if (_p3_0 == "%m")
                {
                    dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, dtnow.Minute, 0);
                }
                else if (_p3_0 == "%s")
                {
                    dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, dtReturn.Minute, dtnow.Second);
                }
                else
                {
                    if (i == 0)
                    {
                        dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, _p3_0.ToInt(), 0, 0);
                    }
                    else if (i == 1)
                    {
                        dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, _p3_0.ToInt(), 0);
                    }
                    else if (i == 2)
                    {
                        dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtReturn.Day, dtReturn.Hour, dtReturn.Minute, _p3_0.ToInt());
                    }
                }

            }
        }

        private const string yuemo = "%ld";

        private static void ReturnDateDateTime(string teSepc, List<string> strs, DateTime dtnow, ref int curSpecIndex, ref DateTime dtReturn, string _text)
        {
            var _p2 = _text.Split('-');
            int _p2Length = _p2.Length;

            for (int i = 0; i < _p2Length; i++)
            {
                var _p2_0 = _p2[i];
                //当是特殊标记的时候处理
                if (_p2_0 == teSepc)
                {
                    if (strs.Count > curSpecIndex)
                    {
                        string _guize = strs[curSpecIndex];
                        string _yanma = null;
                        char oper = '+';
                        int operIndex = 2;
                        if (_guize.IndexOf(yuemo) > -1)
                        {
                            operIndex = 3;
                            oper = _guize[operIndex];

                            _yanma = _guize.Substring(0, 3);
                        }
                        else
                        {
                            oper = _guize[operIndex];
                            _yanma = _guize.Substring(0, 2);
                        }
                        int operNo = _guize.Substring(operIndex).ToInt();
                        //if (oper == '-')
                        //{
                        //    operNo = operNo * (-1);
                        //}
                        if (_yanma == "%y")
                        {
                            int y = dtnow.Year + operNo;
                            dtReturn = new DateTime(y, 1, 1);
                        }
                        else if (_yanma == "%M")
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtnow.Month, 1);
                            dtReturn = dtReturn.AddMonths(operNo);
                        }
                        else if (_yanma == yuemo)
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, AyDatePickerHelper.NumOfDays(dtReturn.Year, dtReturn.Month));
                        }
                        else if (_yanma == "%d")
                        {
                            try
                            {
                                dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtnow.Day);
                                dtReturn = dtReturn.AddDays(operNo);
                            }
                            catch
                            {
                                //有时候因为此时   2月没有30,31,4月没有31日一样，这里时间要额外处理
                                var _2 = AyDatePickerHelper.NumOfDays(dtReturn.Year, dtReturn.Month);
                                if (dtnow.Day > _2)
                                {
                                    int _21 = dtnow.Day - _2;
                                    operNo = operNo + _21;
                                    dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, _2);
                                    dtReturn = dtReturn.AddDays(operNo);
                                }

                            }

                        }

                        curSpecIndex++;

                    }
                }
                else if (_p2_0 == "%y")
                {
                    dtReturn = new DateTime(dtnow.Year, 1, 1);
                }
                else if (_p2_0 == "%M")
                {
                    dtReturn = new DateTime(dtReturn.Year, dtnow.Month, 1);
                }
                else if (_p2_0 == yuemo)
                {
                    dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, AyDatePickerHelper.NumOfDays(dtReturn.Year, dtReturn.Month));
                }
                else if (_p2_0 == "%d")
                {
                    try
                    {
                        dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, dtnow.Day);
                    }
                    catch
                    {
                        //有时候因为此时   2月没有30,31,4月没有31日一样，这里时间要额外处理
                        var _2 = AyDatePickerHelper.NumOfDays(dtReturn.Year, dtReturn.Month);
                        if (dtnow.Day > _2)
                        {
                            int _21 = dtnow.Day - _2;
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, _2);
                            dtReturn = dtReturn.AddDays(_21);
                        }

                    }
                }
                else
                {
                    if (i == 0)
                    {
                        dtReturn = new DateTime(_p2_0.ToInt(), 1, 1);
                    }
                    else if (i == 1)
                    {
                        dtReturn = new DateTime(dtReturn.Year, _p2_0.ToInt(), 1);
                    }
                    else if (i == 2)
                    {
                        try
                        {
                            dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, _p2_0.ToInt());
                        }
                        catch
                        {
                            //有时候因为此时   2月没有30,31,4月没有31日一样，这里时间要额外处理
                            int _12 = _p2_0.ToInt();
                            var _2 = AyDatePickerHelper.NumOfDays(dtReturn.Year, dtReturn.Month);
                            if (_12 > _2)
                            {
                                int _21 = _p2_0.ToInt() - _2;
                                dtReturn = new DateTime(dtReturn.Year, dtReturn.Month, _2);
                                dtReturn = dtReturn.AddDays(_21);
                            }

                        }

                    }

                }
            }
        }





        public static DateTime? ConvertD(string dt, string expression, string fmt)
        {
            DateTime? dtReturn = null;
            //读取文本框的值
            if (dt.IsNullAndTrimAndEmpty())
            {
                return dtReturn;
            }
            else
            {
                MatchCollection matches = Regex.Matches(expression, @"#D\(ay,{(?<c>.*?)}\)");
                List<string> strs = new List<string>();
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    string _1 = groups["c"].Value;
                    if (_1 != null)
                    {
                        strs.Add(groups["c"].Value);
                    }
                }
                if (strs.Count > 0)
                {
                    AyDCDateExpression _ewai = AyJsonUtility.DecodeObject2<AyDCDateExpression>(strs[0]);
                    try
                    {
                        string[] expectedFormats = { fmt };
                        dtReturn = DateTime.ParseExact(dt, expectedFormats, AyDatePickerHelper.culture, DateTimeStyles.None);
                    }
                    catch 
                    {
                        throw new Exception("AY日期表达式 D函数表达式发生问题");
                    }

                    //dtReturn = dt.ToDateTime();
                    //开始处理
                    if (_ewai.y != 0)
                        dtReturn = dtReturn.Value.AddYears(_ewai.y);
                    if (_ewai.M != 0)
                        dtReturn = dtReturn.Value.AddMonths(_ewai.M);
                    if (_ewai.d != 0)
                        dtReturn = dtReturn.Value.AddDays(_ewai.d);
                    if (_ewai.H != 0)
                        dtReturn = dtReturn.Value.AddHours(_ewai.H);
                    if (_ewai.m != 0)
                        dtReturn = dtReturn.Value.AddMinutes(_ewai.m);
                    if (_ewai.s != 0)
                        dtReturn = dtReturn.Value.AddSeconds(_ewai.s);
                    return dtReturn;
                }
                else
                {
                    //这里要换成 tryparseextra方式的时间，如果转换失败就当空白处理
                    dtReturn = dt.ToDateTime();

                    return dtReturn;
                }
            }
        }
        public static DateTime? ConvertDV(string expression)
        {
            DateTime? dtReturn = null;
            string str1 = null;
            string str2 = null;
            if (expression.IndexOf(",{") > 0)
            {
                MatchCollection matches = Regex.Matches(expression, @"#DV\((?<c>.*?),{(?<h>.*?)}");
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    string _1 = groups["c"].Value;
                    if (_1 != null)
                    {
                        str1 = groups["c"].Value;

                    }
                    string _2 = groups["h"].Value;
                    if (_2 != null)
                    {
                        str2 = groups["h"].Value;
                    }
                }
            }
            else
            {
                MatchCollection matches = Regex.Matches(expression, @"#DV\((?<c>.*?)\)");
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    string _1 = groups["c"].Value;
                    if (_1 != null)
                    {
                        str1 = groups["c"].Value;

                    }
                }
            }
            try
            {
                if (str1 == null)
                {
                    throw new Exception("AY日期表达式中DV片段有错误");
                }
                dtReturn = Convert(str1);

                if (str2 != null)
                {
                    AyDCDateExpression _ewai = AyJsonUtility.DecodeObject2<AyDCDateExpression>(str2);
                    //开始处理
                    if (_ewai.y != 0)
                        dtReturn = dtReturn.Value.AddYears(_ewai.y);
                    if (_ewai.M != 0)
                        dtReturn = dtReturn.Value.AddMonths(_ewai.M);
                    if (_ewai.d != 0)
                        dtReturn = dtReturn.Value.AddDays(_ewai.d);
                    if (_ewai.H != 0)
                        dtReturn = dtReturn.Value.AddHours(_ewai.H);
                    if (_ewai.m != 0)
                        dtReturn = dtReturn.Value.AddMinutes(_ewai.m);
                    if (_ewai.s != 0)
                        dtReturn = dtReturn.Value.AddSeconds(_ewai.s);
                }
            }
            catch
            {
                throw new Exception("AY日期表达式中DV片段中的时间部分不能转换成有效的DateTime");
            }



            return dtReturn;



        }
        /// <summary>
        /// AY
        /// 生日：2017-3-6 16:43:21
        /// 用来转换日期表达式 #D，#DV的部分
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="expression"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static List<DateTime?> ConvertDDVF(string dt, string expression, string fmt)
        {
            List<DateTime?> dtReturns = new List<DateTime?>();
            string _1 = expression.Substring(3);
            int _2 = _1.Length;
            string _3 = _1.Substring(0, _2 - 1);
            _3 = _3.Replace("||", "|");
            string[] _4 = _3.Split('|');
            foreach (var item in _4)
            {
                if (item.IndexOf("#DV(") == 0)
                {
                    var _ = ConvertDV(item);
                    if (_.IsNotNull())
                    {
                        dtReturns.Add(_);
                    }

                }
                else if (item.IndexOf("#D(") == 0)
                {
                    var _ = ConvertD(dt, item, fmt);
                    if (_.IsNotNull())
                    {
                        dtReturns.Add(_);
                    }
                }
            }
            return dtReturns;
        }







        #region 翻译特殊正则
        static List<string> zhanwei = new List<string> { "A1", "A2", "A3", "A4", "A5", "A6" };
        public static string ConvertDynamicAyDateExpression(string expression)
        {
            StringBuilder result = new StringBuilder();
            DateTime dtnow = DateTime.Now;

            int uIndex = 0;
            int axlength = expression.Length;
            List<string> extString = new List<string>();
            string resultString = null;
            for (int i = 0; i < axlength; i++)
            {
                if (expression[i] == '-' || expression[i] == '.' || expression[i] == ' ' || expression[i] == ':')
                {
                    result.Append(expression[i]);
                }
                else if (expression[i] == '%')
                {
                    string _d = expression.Substring(i, 2);
                    switch (_d)
                    {
                        case "%y":
                            result.Append(dtnow.Year.ToString().PadLeft(2, '0'));
                            i++;
                            break;
                        case "%M":
                            result.Append(dtnow.Month.ToString().PadLeft(2, '0'));
                            i++;
                            break;
                        case "%d":
                            result.Append(dtnow.Day.ToString().PadLeft(2, '0'));
                            i++;
                            break;
                        case "%H":
                            result.Append(dtnow.Hour.ToString().PadLeft(2, '0'));
                            i++;
                            break;
                        case "%m":
                            result.Append(dtnow.Minute.ToString().PadLeft(2, '0'));
                            i++;
                            break;
                        case "%s":
                            result.Append(dtnow.Second.ToString().PadLeft(2, '0'));
                            i++;

                            break;
                    }
                }
                else if (expression[i] == '{')
                {

                    MatchCollection matches = Regex.Matches(expression, @"{(?<c>.*?)}");
                    string xy = null;
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        xy = groups["c"].Value;
                        break;
                    }
                    if (!xy.IsNullAndTrimAndEmpty())
                    {
                        extString.Add(xy);
                        i = i + xy.Length + 1;
                        result.Append(zhanwei[uIndex]);
                        uIndex++;
                    }

                }
                else
                {
                    result.Append(expression[i]);
                }
            }
            resultString = result.ToString();
            //分析运算式
            if (extString.Count > 0)
            {
                int extStringIndex = 0;
                int? _yyyy = null;
                int? _MM = null;
                DateTime? dtReturn = null; //临时使用计算add的
                foreach (var item in extString)
                {
                    if (item.IndexOf("%y") == 0)
                    {
                        //如果处理年
                        int operNo = item.Substring(2).ToInt();
                        int _ = dtnow.Year + operNo;
                        if (_ > YearStrick.MAXYEAR)
                        {
                            _ = YearStrick.MAXYEAR;
                        }
                        else if (_ < YearStrick.MINYEAR)
                        {
                            _ = YearStrick.MINYEAR;
                        }
                        _yyyy = _;
                        resultString = resultString.Replace(zhanwei[extStringIndex], _yyyy.ToString());
                        extStringIndex++;
                    }
                    else if (item.IndexOf("%M") == 0)
                    {
                        //如果处理月
                        int operNo = item.Substring(2).ToInt();
                        int _ = dtnow.Month + operNo;
                        if (_ > 12)
                        {
                            int _yeI = 0;
                            string _ye = null;
                            if (_yyyy == null)
                            {
                                //从result读取前
                                _ye = resultString.Substring(0, 4);
                                _yeI = _ye.ToInt();
                            }
                            else
                            {
                                _ye = _yyyy.Value.ToString();
                                _yeI = _yyyy.Value;
                            }
                            _yeI++;
                            if (_yeI > YearStrick.MAXYEAR)
                            {
                                _yeI = YearStrick.MAXYEAR;
                            }
                            resultString = resultString.Replace(_ye, _yeI.ToString());
                            int month = _ - 12;
                            _MM = month;
                            resultString = resultString.Replace(zhanwei[extStringIndex], _MM.ToString());
                            extStringIndex++;
                        }
                        else if (_ < 1)
                        {
                            int _yeI = 0;
                            string _ye = null;
                            if (_yyyy == null)
                            {
                                //从result读取前
                                _ye = resultString.Substring(0, 4);
                                _yeI = _ye.ToInt();
                            }
                            else
                            {
                                _ye = _yyyy.Value.ToString();
                                _yeI = _yyyy.Value;
                            }
                            _yeI--;
                            if (_yeI < YearStrick.MINYEAR)
                            {
                                _yeI = YearStrick.MINYEAR;
                            }
                            resultString = resultString.Replace(_ye, _yeI.ToString());
                            int month = _ + 12;
                            _MM = month;
                            resultString = resultString.Replace(zhanwei[extStringIndex], _MM.ToString());
                            extStringIndex++;
                        }
                    }
                    else if (item.IndexOf("%d") == 0)
                    {
                        //获得年月
                        int _yeI = 0;
                        string _ye = null;
                        if (_yyyy == null)
                        {
                            //从result读取前
                            _ye = resultString.Substring(0, 4);
                            _yeI = _ye.ToInt();
                        }
                        else
                        {
                            _ye = _yyyy.Value.ToString();
                            _yeI = _yyyy.Value;
                        }
                        int _MeI = 0;
                        string _Me = null;
                        if (_MM == null)
                        {
                            //从result读取前
                            _Me = resultString.Substring(5, 2);
                            _MeI = _Me.ToInt();
                        }
                        else
                        {
                            _Me = _MM.Value.ToString();
                            _MeI = _MM.Value;
                        }
                        int operNo = item.Substring(2).ToInt();
                        try
                        {
                            dtReturn = new DateTime(_yeI, _MeI, dtnow.Day);
                            dtReturn = dtReturn.Value.AddDays(operNo);
                        }
                        catch
                        {
                            //有时候因为此时   2月没有30,31,4月没有31日一样，这里时间要额外处理
                            var _2 = AyDatePickerHelper.NumOfDays(_yeI, _MeI);
                            if (dtnow.Day > _2)
                            {
                                int _21 = dtnow.Day - _2;
                                operNo = operNo + _21;
                                dtReturn = new DateTime(_yeI, _MeI, _2);
                                dtReturn = dtReturn.Value.AddDays(operNo);
                            }

                        }
                        var _343 = dtReturn.Value.Day.ToString().PadLeft(2, '0');
                        resultString = resultString.Replace(zhanwei[extStringIndex], _343);
                        extStringIndex++;
                    }
                    else if (item.IndexOf("%H") == 0)
                    {
                        dtReturn = InitReturnDate(resultString, _yyyy, _MM, dtReturn);
                        dtReturn = dtReturn.Value.AddHours(dtnow.Hour);
                        //dtReturn = new DateTime(dtReturn.Value.Year, dtReturn.Value.Month, dtReturn.Value.Day, dtnow.Hour, 0, 0);
                        int operNo = item.Substring(2).ToInt();
                        dtReturn = dtReturn.Value.AddHours(operNo);

                        var _343 = dtReturn.Value.Hour.ToString().PadLeft(2, '0');
                        resultString = resultString.Replace(zhanwei[extStringIndex], _343);
                        extStringIndex++;
                    }
                    else if (item.IndexOf("%m") == 0)
                    {
                        dtReturn = InitReturnDate(resultString, _yyyy, _MM, dtReturn);
                        string _he = resultString.Substring(11, 2);
                        int _heI = _he.ToInt();
                        dtReturn = new DateTime(dtReturn.Value.Year, dtReturn.Value.Month, dtReturn.Value.Day, _heI, dtnow.Minute, 0);
                        int operNo = item.Substring(2).ToInt();
                        dtReturn = dtReturn.Value.AddMinutes(operNo);

                        var _343 = dtReturn.Value.Minute.ToString().PadLeft(2, '0');
                        resultString = resultString.Replace(zhanwei[extStringIndex], _343);
                        extStringIndex++;
                    }
                    else if (item.IndexOf("%s") == 0)
                    {
                        dtReturn = InitReturnDate(resultString, _yyyy, _MM, dtReturn);
                        string _he = resultString.Substring(11, 2);
                        int _heI = _he.ToInt();

                        string _me = resultString.Substring(14, 2); //17,2代表获得秒
                        int _meI = _me.ToInt();


                        dtReturn = new DateTime(dtReturn.Value.Year, dtReturn.Value.Month, dtReturn.Value.Day, _heI, _meI, dtnow.Second);
                        int operNo = item.Substring(2).ToInt();
                        dtReturn = dtReturn.Value.AddSeconds(operNo);
                        var _343 = dtReturn.Value.Second.ToString().PadLeft(2, '0');
                        resultString = resultString.Replace(zhanwei[extStringIndex], _343);
                        extStringIndex++;
                    }
                }
            }
            return resultString;

        }

        private static DateTime? InitReturnDate(string resultString, int? _yyyy, int? _MM, DateTime? dtReturn)
        {

            if (!dtReturn.HasValue)
            {
                try
                {
                    //获得年月
                    int _yeI = 0;
                    string _ye = null;
                    if (_yyyy == null)
                    {
                        //从result读取前
                        _ye = resultString.Substring(0, 4);
                        _yeI = _ye.ToInt();
                    }
                    else
                    {
                        _ye = _yyyy.Value.ToString();
                        _yeI = _yyyy.Value;
                    }
                    int _MeI = 0;
                    string _Me = null;
                    if (_MM == null)
                    {
                        //从result读取前
                        _Me = resultString.Substring(5, 2);
                        _MeI = _Me.ToInt();
                    }
                    else
                    {
                        _Me = _MM.Value.ToString();
                        _MeI = _MM.Value;
                    }
                    //从result读取前
                    //2017-03-09 15:09:32
                    string _de = resultString.Substring(8, 2);
                    int _deI = _de.ToInt();
                    dtReturn = new DateTime(_yeI, _MeI, _deI);
                }
                catch
                {
                    throw new Exception("AY日期表达式-DisabledDates语法有误,不支持年月日无法确定的写法,比如%y-..-%d {%H-1}:..:..");
                }
            }

            return dtReturn;
        }
        #endregion
    }
}
