using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
    public class StackPanelPadding : StackPanel
    {
        //        <Controls:StackPanelPadding Orientation = "Horizontal" CellPadding="15,0">
        //    <Label Content = "MyLabel" />
        //    < TextBlock Text="MyText"/>
        //</Controls:StackPanelPadding>
        public static DependencyProperty SpacingProperty = DependencyProperty.Register
    ("Spacing", typeof(Thickness), typeof(StackPanelPadding), new FrameworkPropertyMetadata(default(Thickness),
     FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSpacingChanged));
        public Thickness Spacing
        {
            get
            {
                return (Thickness)GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }
        private static void OnSpacingChanged(DependencyObject Object,
        DependencyPropertyChangedEventArgs e)
        {
            ((StackPanelPadding)Object).SetPadding();
        }

        public static DependencyProperty TrimFirstProperty = DependencyProperty.Register
    ("TrimFirst", typeof(bool), typeof(StackPanelPadding), new FrameworkPropertyMetadata(false,
    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTrimFirstChanged));
        public bool TrimFirst
        {
            get
            {
                return (bool)GetValue(TrimFirstProperty);
            }
            set
            {
                SetValue(TrimFirstProperty, value);
            }
        }
        private static void OnTrimFirstChanged(DependencyObject Object,
        DependencyPropertyChangedEventArgs e)
        {
            ((StackPanelPadding)Object).SetPadding();
        }

        public static DependencyProperty TrimLastProperty = DependencyProperty.Register
    ("TrimLast", typeof(bool), typeof(StackPanelPadding), new FrameworkPropertyMetadata
    (false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTrimLastChanged));
        public bool TrimLast
        {
            get
            {
                return (bool)GetValue(TrimLastProperty);
            }
            set
            {
                SetValue(TrimLastProperty, value);
            }
        }
        private static void OnTrimLastChanged(DependencyObject Object,
        DependencyPropertyChangedEventArgs e)
        {
            ((StackPanelPadding)Object).SetPadding();
        }

        private void SetPadding()
        {
            for (int i = 0, Count = this.Children.Count; i < Count; i++)
            {
                FrameworkElement Element = this.Children[i] as FrameworkElement;
                if ((i == 0 && TrimFirst) || (i == (Count - 1) && TrimLast))
                {
                    Element.Margin = new Thickness(0);
                    continue;
                }
                Element.Margin = this.Spacing;
            }
        }

        public StackPanelPadding()
        {
            this.LayoutUpdated += _LayoutUpdated;
        }

        private void _LayoutUpdated(object sender, System.EventArgs e)
        {
            this.SetPadding();
        }
    }
}
