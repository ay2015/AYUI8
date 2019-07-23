
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ay.Controls
{
    public enum DragMouseButton
    {
        L,
        R
    }
    public class DragInCanvasBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            switch (DragButton)
            {
                case DragMouseButton.L:
                    this.AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseRightButtonDown;
                    this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
                    this.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseRightButtonUp;
                    break;
       
                case DragMouseButton.R:
                    this.AssociatedObject.MouseRightButtonDown += AssociatedObject_MouseRightButtonDown;
                    this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
                    this.AssociatedObject.MouseRightButtonUp += AssociatedObject_MouseRightButtonUp;
                    break;
                default:
                    break;
            }
        }


        private DragMouseButton dragButton = DragMouseButton.R;

        public DragMouseButton DragButton
        {
            get { return dragButton; }
            set { dragButton = value; }
        }

        private Canvas canvas;
        private bool isDragging = false;

        private Point mouseOffset;


        void AssociatedObject_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                this.AssociatedObject.ReleaseMouseCapture();
                isDragging = false;
            }

        }
        double tempHeight;
        double tempWidth;

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging && DragMouseButton.R == DragButton && e.RightButton == MouseButtonState.Pressed)
            {
                ExecuteMode(e);
                return;
            }

            if (isDragging && DragMouseButton.L == DragButton && e.LeftButton == MouseButtonState.Pressed)
            {
                ExecuteMode(e);
                return;
            }
        }

        private void ExecuteMode(System.Windows.Input.MouseEventArgs e)
        {
            double maxMoveHeightMarginBottom = canvas.ActualHeight - tempHeight;
            double maxMoveWidthMarginLeft = canvas.ActualWidth - tempWidth;
            Point point = e.GetPosition(canvas);
            double endSetY = point.Y - mouseOffset.Y;
            double endSetX = point.X - mouseOffset.X;

            if (endSetY < 0) { endSetY = 0; }
            if (endSetY > maxMoveHeightMarginBottom) { endSetY = maxMoveHeightMarginBottom; }
            if (endSetX > maxMoveWidthMarginLeft) { endSetX = maxMoveWidthMarginLeft; }
            if (endSetX < 0) { endSetX = 0; }
            this.AssociatedObject.SetValue(Canvas.TopProperty, endSetY);
            this.AssociatedObject.SetValue(Canvas.LeftProperty, endSetX);
        }

        void AssociatedObject_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //获得canvas
            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
                //canvas.SizeChanged += canvas_SizeChanged;
            }
            isDragging = true;
            mouseOffset = e.GetPosition(this.AssociatedObject);
            tempHeight = (double)this.AssociatedObject.GetValue(FrameworkElement.HeightProperty);
            tempWidth = (double)this.AssociatedObject.GetValue(FrameworkElement.WidthProperty);
            this.AssociatedObject.CaptureMouse();
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseRightButtonDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseRightButtonUp;

        }
    }
}
