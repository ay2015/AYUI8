using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Globalization;

namespace ay.contentcore
{
    public class AyStrokeLabel : Label
    {
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty StretchSizeProperty;
        public static readonly DependencyProperty FillProperty;
        public static readonly DependencyProperty StrokeProperty;
        public static readonly DependencyProperty StrokeThicknessProperty;
        static AyStrokeLabel()
        {
            PropertyMetadata textMeta = new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                (d as AyStrokeLabel).InvalidateVisual();
            });
            PropertyMetadata strchMeta = new PropertyMetadata(0);


            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AyStrokeLabel), textMeta);
            StretchSizeProperty = DependencyProperty.Register("StretchSize", typeof(double), typeof(AyStrokeLabel));
            FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(AyStrokeLabel));
            StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(AyStrokeLabel));
            StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(AyStrokeLabel));
        }

        /// <summary>
        /// Create the outline geometry based on the formatted text.
        /// </summary>
        public void CreateText()
        {
            getformattedText(Text);
            //this.Content = Text;
        }

        PathGeometry pg = new PathGeometry();

        private void getformattedText(string str)
        {
            System.Windows.FontStyle fontStyle = FontStyles.Normal;
            FontWeight fontWeight = FontWeights.Normal;
            // Create the formatted text based on the properties set.
            FormattedText formattedText = new FormattedText(
                str,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface(
                    FontFamily,
                    FontStyle,
                    FontWeight,
                    FontStretch),
                FontSize,
                System.Windows.Media.Brushes.Black // This brush does not matter since we use the geometry of the text. 
                );

            this.Width = formattedText.Width;
            this.Height = formattedText.Height;
            // Build the geometry object that represents the text.
            //pg.AddGeometry(formattedText.BuildGeometry(new System.Windows.Point(5, 5)));
            TextGeometry = formattedText.BuildGeometry(new System.Windows.Point(0,0));
            // Build the geometry object that represents the text hightlight.
            if (Highlight == true)
            {
                TextHighLightGeometry = formattedText.BuildHighlightGeometry(new System.Windows.Point(0, 0));
            }
        }

        /// <summary>
        /// OnRender override draws the geometry of the text and optional highlight.
        /// </summary>
        /// <param name="drawingContext">Drawing context of the OutlineText control.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            CreateText();
            // Draw the outline based on the properties that are set.
            drawingContext.DrawGeometry(Fill, new System.Windows.Media.Pen(Stroke, StrokeThickness), TextGeometry);
            // Draw the text highlight based on the properties that are set.
            if (Highlight == true)
            {
                drawingContext.DrawGeometry(null, new System.Windows.Media.Pen(Stroke, StrokeThickness), TextHighLightGeometry);
            }
        }

        /// <summary>
        /// 字符串的格式化几何对象
        /// </summary>
        public Geometry TextGeometry { get;private set; }

        /// <summary>
        /// 是否高亮（暂时不支持）
        /// </summary>
        public bool Highlight { get; set; }

        /// <summary>
        /// 高亮几何对象
        /// </summary>
        public Geometry TextHighLightGeometry { get; private set; }

        /// <summary>
        /// 字符间距
        /// </summary>
        public double StretchSize
        {
            get
            {
                return (double)GetValue(StretchSizeProperty);
            }
            set
            {
                SetValue(StretchSizeProperty, value);
            }
        }

        /// <summary>
        /// 字符串格式化对象的填充花刷
        /// </summary>
        public Brush Fill { get { return GetValue(FillProperty) as Brush; } set { SetValue(FillProperty, value); } }

        /// <summary>
        /// 边缘画刷
        /// </summary>
        public Brush Stroke { get { return GetValue(StrokeProperty) as Brush; } set { SetValue(StrokeProperty, value); } }

        /// <summary>
        /// 边缘宽度
        /// </summary>
        public double StrokeThickness { get { return (double)GetValue(StrokeThicknessProperty); } set { SetValue(StrokeThicknessProperty, value); } }

        /// <summary>
        /// 显示的文字
        /// </summary>
        public string Text
        {
            get
            {
                return GetValue(TextProperty) as string;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}
