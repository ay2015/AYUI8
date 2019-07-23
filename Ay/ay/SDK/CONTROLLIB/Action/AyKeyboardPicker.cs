using ay.contentcore;
using ay.FuncFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using i = System.Windows.Interactivity;



namespace ay.Controls
{

    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    public class AyKeyboardPicker : TriggerAction<FrameworkElement>
    {
      
        public FrameworkElement Target
        {
            get { return (FrameworkElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(AyKeyboardPicker), new PropertyMetadata(null));


        protected override void Invoke(object parameter)
        {
            if (Target != null)
            {
                Target.Focus();
            }
            SystemHelper.ShowKeyBoard();
        }

     
    }


}


