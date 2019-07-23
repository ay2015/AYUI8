
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ay.contentcore
{
    /// <summary>
    /// AY通过配置文件方式显示path,只支持path_开头的
    /// </summary>
    [Ambient]
    [UsableDuringInitialization(true)]
    [TemplatePart(Name = "PART_Path", Type = typeof(Path))]
    public class AyPath : Control, IIconSupport
    {
        static AyPath()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyPath), new FrameworkPropertyMetadata(typeof(AyPath)));
        }

        /// <summary>
        /// 拉伸方式
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }


        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AyPath), new PropertyMetadata(Stretch.Uniform));



        /// <summary>
        /// Path的边框颜色
        /// </summary>
        public Brush PathStrokeBrush
        {
            get { return (Brush)GetValue(PathStrokeBrushProperty); }
            set { SetValue(PathStrokeBrushProperty, value); }
        }

        public static readonly DependencyProperty PathStrokeBrushProperty =
            DependencyProperty.Register("PathStrokeBrush", typeof(Brush), typeof(AyPath), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        /// <summary>
        /// stroke的粗细，double类型
        /// </summary>
        public double PathStrokeThickness
        {
            get { return (double)GetValue(PathStrokeThicknessProperty); }
            set { SetValue(PathStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty PathStrokeThicknessProperty =
            DependencyProperty.Register("PathStrokeThickness", typeof(double), typeof(AyPath), new PropertyMetadata(0.00));



        internal string PathData
        {
            get { return (string)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }
        internal static readonly DependencyProperty PathDataProperty =
            DependencyProperty.Register("PathData", typeof(string), typeof(AyPath), new PropertyMetadata(string.Empty));


        public override void OnApplyTemplate()
        {
            UpdatePathData();
            base.OnApplyTemplate();

        }

        private void UpdatePathData()
        {
            var key = PathIcon.GetIcon(this);
            if (string.IsNullOrEmpty(key))
            {
                key = "path_ay";
            }
            PathData = PathIcon.Instance.GetIconFromXml(key);
        }

        public void LoadIcon()
        {
            UpdatePathData();
        }
    }



}
