using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ay.Controls
{
    public class GridService : DependencyObject
    {
        public static string GetRowColumn(DependencyObject obj)
        {
            return (string)obj.GetValue(RowColumnProperty);
        }

        public static void SetRowColumn(DependencyObject obj, string value)
        {
            obj.SetValue(RowColumnProperty, value);
        }

        // Using a DependencyProperty as the backing store for RowColumn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowColumnProperty =
            DependencyProperty.RegisterAttached("RowColumn", typeof(string), typeof(GridService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnRowColumnChanged));

        private static void OnRowColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement box = d as UIElement;
            if (box!=null)
            {
                var _1 = (string)e.NewValue;
                if (_1!=null)
                {
                    string _2 = _1.Trim().Replace("，", " ").Replace(",", " ").Replace("#", " ");
                    var _2s = _2.Split(' ');
                    if (_2s[0] != "?")
                    {
                        Grid.SetRow(box, _2s[0].ToInt());
                    }
                    if (_2s[1] != "?")
                    {
                        Grid.SetColumn(box, _2s[1].ToInt());
                    }
                }
            }
        }

        public GridService()
        {

        }

        public static string GetColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        /// <summary>
        /// 字符串规则: 1 2 3 4 5 8* a ?
        /// a代表自动auto  ?代表不填写
        /// 支持空格或者#或者逗号，效果是一样的，主要方便断数字 比如 n2v1*x3 1* 1* 1* 2*#300 32
        /// n是min的结束词，表示最小宽度,v是value的缩略词，代表width，x代表max最大宽度
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnColumnsChanged));



        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid box = d as Grid;
            if (box!=null)
            {
                var _1 = (string)e.NewValue;
                if (_1!=null)
                {


                    string _2 = _1.Trim().Replace("，", " ").Replace(",", " ").Replace("#", " ");
                    var _2s = _2.Split(' ');
                    box.ColumnDefinitions.Clear();
                    foreach (var item in _2s)
                    {

                        var _11 = item.IndexOf("min");
                        var _12 = item.IndexOf("v");
                        var _13 = item.IndexOf("max");
                        if (_11 == -1 && _12 == -1 && _13 == -1)
                        {
                            //断定普通，单纯设置
                            ColumnDefinition cd = new ColumnDefinition();
                            if (item != "?")
                            {
                                cd.Width = item.ToGridLength();
                            }
                            box.ColumnDefinitions.Add(cd);
                            continue;
                        }
                        else
                        {
                            //启用高级模式
                            if (_11 > -1 && _12 > -1 && _13 > -1)
                            {

                                int[] arry = new int[] { _11, _12, _13 };
                                Array.Sort<int>(arry);
                                //排位
                                //var _nv = new string[3];
                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 3; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinWidth = groups["n"].Value.ToDouble();
                                    }
                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxWidth = groups["x"].Value.ToDouble();
                                    }
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Width = groups["v"].Value.ToGridLength();
                                    }

                                    box.ColumnDefinitions.Add(cd);

                                }

                            }
                            else if (_11 > -1 && _12 > -1 && _13 < 0)
                            {

                                int[] arry = new int[] { _11, _12 };
                                Array.Sort<int>(arry);
                                //排位
                                //var _nv = new string[3];
                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;

                                    ColumnDefinition cd = new ColumnDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinWidth = groups["n"].Value.ToDouble();
                                    }

                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Width = groups["v"].Value.ToGridLength();
                                    }
                                    box.ColumnDefinitions.Add(cd);
                                }
                            }
                            else if (_11 > -1 && _12 < 0 && _13 > -1)
                            {

                                int[] arry = new int[] { _11, _13 };
                                Array.Sort<int>(arry);
                                //排位

                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinWidth = groups["n"].Value.ToDouble();
                                    }
                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxWidth = groups["x"].Value.ToDouble();
                                    }


                                    box.ColumnDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 > -1 && _13 > -1)
                            {

                                int[] arry = new int[] { _12, _13 };
                                Array.Sort<int>(arry);
                                //排位

                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();

                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxWidth = groups["x"].Value.ToDouble();
                                    }
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Width = groups["v"].Value.ToGridLength();
                                    }

                                    box.ColumnDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 < 0 && _13 > -1)
                            {
                                MatchCollection m2 = Regex.Matches(item, "max(?<x>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();

                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxWidth = groups["x"].Value.ToDouble();
                                    }


                                    box.ColumnDefinitions.Add(cd);
                                }
                            }
                            else if (_11 > -1 && _12 < 0 && _13 < 0)
                            {
                                MatchCollection m2 = Regex.Matches(item, "min(?<n>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinWidth = groups["n"].Value.ToDouble();
                                    }


                                    box.ColumnDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 > -1 && _13 < 0)
                            {
                                MatchCollection m2 = Regex.Matches(item, "v(?<v>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    ColumnDefinition cd = new ColumnDefinition();
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Width = groups["v"].Value.ToGridLength();
                                    }
                                    box.ColumnDefinitions.Add(cd);
                                }
                            }


                        }
                    }
                }
            }
        }





        public static string GetRows(DependencyObject obj)
        {
            return (string)obj.GetValue(RowsProperty);
        }

        public static void SetRows(DependencyObject obj, string value)
        {
            obj.SetValue(RowsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(GridService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnRowsChanged));

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Grid box = d as Grid;
            if (box!=null)
            {
                var _1 = (string)e.NewValue;
                if (_1!=null)
                {
                    string _2 = _1.Trim().Replace("，", " ").Replace(",", " ").Replace("#", " ");
                    var _2s = _2.Split(' ');
                    box.RowDefinitions.Clear();
                    foreach (var item in _2s)
                    {

                        var _11 = item.IndexOf("min");
                        var _12 = item.IndexOf("v");
                        var _13 = item.IndexOf("max");
                        if (_11 == -1 && _12 == -1 && _13 == -1)
                        {
                            //断定普通，单纯设置
                            RowDefinition cd = new RowDefinition();
                            if (item != "?")
                            {
                                cd.Height = item.ToGridLength();
                            }
                            box.RowDefinitions.Add(cd);
                            continue;
                        }
                        else
                        {
                            //启用高级模式
                            if (_11 > -1 && _12 > -1 && _13 > -1)
                            {

                                int[] arry = new int[] { _11, _12, _13 };
                                Array.Sort<int>(arry);
                                //排位
                                //var _nv = new string[3];
                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 3; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinHeight = groups["n"].Value.ToDouble();
                                    }
                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxHeight = groups["x"].Value.ToDouble();
                                    }
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Height = groups["v"].Value.ToGridLength();
                                    }

                                    box.RowDefinitions.Add(cd);

                                }

                            }
                            else if (_11 > -1 && _12 > -1 && _13 < 0)
                            {

                                int[] arry = new int[] { _11, _12 };
                                Array.Sort<int>(arry);
                                //排位
                                //var _nv = new string[3];
                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;

                                    RowDefinition cd = new RowDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinHeight = groups["n"].Value.ToDouble();
                                    }

                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Height = groups["v"].Value.ToGridLength();
                                    }
                                    box.RowDefinitions.Add(cd);
                                }
                            }
                            else if (_11 > -1 && _12 < 0 && _13 > -1)
                            {

                                int[] arry = new int[] { _11, _13 };
                                Array.Sort<int>(arry);
                                //排位

                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _11)
                                    {
                                        _sb.Append("min(?<n>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinHeight = groups["n"].Value.ToDouble();
                                    }
                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxHeight = groups["x"].Value.ToDouble();
                                    }


                                    box.RowDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 > -1 && _13 > -1)
                            {

                                int[] arry = new int[] { _12, _13 };
                                Array.Sort<int>(arry);
                                //排位

                                StringBuilder _sb = new StringBuilder();
                                for (int i = 0; i < 2; i++)
                                {
                                    if (arry[i] == _12)
                                    {
                                        _sb.Append("v(?<v>.+)");
                                        continue;
                                    }
                                    else if (arry[i] == _13)
                                    {
                                        _sb.Append("max(?<x>.+)");
                                        continue;
                                    }
                                }

                                MatchCollection m2 = Regex.Matches(item, _sb.ToString());
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();

                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxHeight = groups["x"].Value.ToDouble();
                                    }
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Height = groups["v"].Value.ToGridLength();
                                    }

                                    box.RowDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 < 0 && _13 > -1)
                            {
                                MatchCollection m2 = Regex.Matches(item, "max(?<x>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();

                                    if (groups["x"].Value != "?")
                                    {
                                        cd.MaxHeight = groups["x"].Value.ToDouble();
                                    }


                                    box.RowDefinitions.Add(cd);
                                }
                            }
                            else if (_11 > -1 && _12 < 0 && _13 < 0)
                            {
                                MatchCollection m2 = Regex.Matches(item, "min(?<n>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();
                                    if (groups["n"].Value != "?")
                                    {
                                        cd.MinHeight = groups["n"].Value.ToDouble();
                                    }


                                    box.RowDefinitions.Add(cd);
                                }
                            }
                            else if (_11 < 0 && _12 > -1 && _13 < 0)
                            {
                                MatchCollection m2 = Regex.Matches(item, "v(?<v>.+)");
                                foreach (Match match in m2)
                                {
                                    GroupCollection groups = match.Groups;
                                    RowDefinition cd = new RowDefinition();
                                    if (groups["v"].Value != "?")
                                    {
                                        cd.Height = groups["v"].Value.ToGridLength();
                                    }
                                    box.RowDefinitions.Add(cd);
                                }
                            }
                        }

                    }
                }
            }
        }
    }


}
