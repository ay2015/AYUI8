using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ay.Controls
{
    public class SelectionAdorner : Adorner
    {
        private string selectionBrush;

        public string SelectionBrush
        {
            get { return selectionBrush; }
            set { selectionBrush = value; }
        }

        private Rect selectionRect;

        /// <summary>
        /// Initializes a new instance of the SelectionAdorner class.
        /// </summary>
        /// <param name="parent">
        /// The UIElement which this instance will overlay.
        /// </param>
        /// <exception cref="ArgumentNullException">parent is null.</exception>
        public SelectionAdorner(UIElement parent, string _selectionBrush)
            : base(parent)
        {
            this.SelectionBrush = _selectionBrush;
            // Make sure the mouse doesn't see us.
            this.IsHitTestVisible = false;

            // We only draw a rectangle when we're enabled.
            this.IsEnabledChanged += delegate { this.InvalidateVisual(); };

        }

        /// <summary>Gets or sets the area of the selection rectangle.</summary>
        public Rect SelectionArea
        {
            get
            {
                return this.selectionRect;
            }
            set
            {
                this.selectionRect = value;
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Participates in rendering operations that are directed by the layout system.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.IsEnabled)
            {
                // Make the lines snap to pixels (add half the pen width [0.5])
                double[] x = { this.SelectionArea.Left + 0.5, this.SelectionArea.Right + 0.5 };
                double[] y = { this.SelectionArea.Top + 0.5, this.SelectionArea.Bottom + 0.5 };
                drawingContext.PushGuidelineSet(new GuidelineSet(x, y));

                Brush fill = SolidColorBrushConverter.From16JinZhi(SelectionBrush);
                fill.Opacity = 0.4;
                drawingContext.DrawRectangle(fill, new Pen(fill, 1.0), this.SelectionArea);
            }
        }
    }


}
