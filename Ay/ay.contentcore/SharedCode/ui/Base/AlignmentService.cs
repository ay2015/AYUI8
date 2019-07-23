using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ay.Controls
{

    /// <summary>
    /// 生日：2016-10-27 00:22:36
    /// 理想：简化xaml写法
    /// </summary>
    public class AlignmentService : DependencyObject
    {
        public AlignmentService()
        {

        }

        public static string GetAlignment(DependencyObject obj)
        {
            return (string)obj.GetValue(AlignmentProperty);
        }

        public static void SetAlignment(DependencyObject obj, string value)
        {
            obj.SetValue(AlignmentProperty, value);
        }

        // Using a DependencyProperty as the backing store for Alignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AlignmentProperty =
            DependencyProperty.RegisterAttached("Alignment", typeof(string), typeof(AlignmentService), new PropertyMetadata(null, OnAlignmentChanged));

        private static void OnAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _0 = d as ContentControl;
            if (_0 != null)
            {
                var _1 = (string)e.NewValue;
                if (!_1.IsNullAndTrimAndEmpty())
                {
                    string _2 = _1.Trim().Replace("，", " ").Replace(",", " ").Replace("#", " ");
                    var _2s = _2.Split(' ');
                    foreach (var item in _2s)
                    {
                        if (_2s.Length >= 1 && _2s[0] != "?")
                            _0.HorizontalAlignment = _2s[0].ToHorAlign();
                        if (_2s.Length >= 2 && _2s[1] != "?")
                            _0.VerticalAlignment = _2s[1].ToVerAlign();
                        if (_2s.Length >= 3 && _2s[2] != "?")
                            _0.HorizontalContentAlignment = _2s[2].ToHorAlign();
                        if (_2s.Length >= 4 && _2s[3] != "?")
                            _0.VerticalContentAlignment = _2s[3].ToVerAlign();
                    }
                }
                else
                {
                    _0.ClearValue(ContentControl.HorizontalAlignmentProperty);
                    _0.ClearValue(ContentControl.VerticalAlignmentProperty);
                    _0.ClearValue(ContentControl.HorizontalContentAlignmentProperty);
                    _0.ClearValue(ContentControl.VerticalContentAlignmentProperty);
                }
            }
            else
            {
                var _00 = d as FrameworkElement;
                if (_00 != null)
                {
                    var _1 = (string)e.NewValue;
                    if (!_1.IsNullAndTrimAndEmpty())
                    {
                        string _2 = _1.Trim().Replace("，", " ").Replace(",", " ").Replace("#", " ");
                        var _2s = _2.Split(' ');
                        foreach (var item in _2s)
                        {
                            if (_2s.Length >= 1 && _2s[0] != "?")
                                _00.HorizontalAlignment = _2s[0].ToHorAlign();
                            if (_2s.Length >= 2 && _2s[1] != "?")
                                _00.VerticalAlignment = _2s[1].ToVerAlign();
                        }
                    }
                }
                else
                {
                    _00.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
                    _00.ClearValue(FrameworkElement.VerticalAlignmentProperty);

                }
            }




        }
    }
}
