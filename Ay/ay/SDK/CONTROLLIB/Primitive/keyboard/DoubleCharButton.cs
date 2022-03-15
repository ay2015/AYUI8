using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
    public class DoubleCharButton : Button
    {
        static DoubleCharButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleCharButton),
             new FrameworkPropertyMetadata(typeof(DoubleCharButton)));
        }


        public string Text1
        {
            get { return (string)GetValue(Text1Property); }
            set { SetValue(Text1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Text1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Text1Property =
            DependencyProperty.Register("Text1", typeof(string), typeof(DoubleCharButton), new PropertyMetadata(null));




        public string Text2
        {
            get { return (string)GetValue(Text2Property); }
            set { SetValue(Text2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Text2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Text2Property =
            DependencyProperty.Register("Text2", typeof(string), typeof(DoubleCharButton), new PropertyMetadata(null));


        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            set { SetValue(CurrentTextProperty, value); }
        }

        public static readonly DependencyProperty CurrentTextProperty =
            DependencyProperty.Register("CurrentText", typeof(string), typeof(DoubleCharButton), new PropertyMetadata(null));




        public bool? IsSwitched
        {
            get { return (bool?)GetValue(IsSwitchedProperty); }
            set { SetValue(IsSwitchedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSwitched.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSwitchedProperty =
            DependencyProperty.Register("IsSwitched", typeof(bool?), typeof(DoubleCharButton), new PropertyMetadata(null, new PropertyChangedCallback(OnIsSwitchedChanged)));

        private static void OnIsSwitchedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (d as DoubleCharButton);
            if (_1.IsNotNull())
            {
                if ((bool)e.NewValue)
                {
                    _1.CurrentText = _1.Text2;
                }
                else
                {
                    _1.CurrentText = _1.Text1;
                }
            }
        }
    }
}
