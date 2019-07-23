
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Ay.Framework.WPF.Controls;

namespace ay.Controls
{
    internal class DragInGridBehavior : Behavior<UIElement>
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


        private DragMouseButton dragButton = DragMouseButton.L;

        internal DragMouseButton DragButton
        {
            get { return dragButton; }
            set { dragButton = value; }
        }

        private Border bd;
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
        Thickness ne = new Thickness();
        AyLayer dl = null;
        private void ExecuteMode(System.Windows.Input.MouseEventArgs e)
        {
            Point point = e.GetPosition(dl);
            double endSetY = point.Y - mouseOffset.Y;
            double endSetX = point.X - mouseOffset.X;
            if (endSetY < 0) endSetY = 0;
            if (endSetX < 0) endSetX = 0;

            if (endSetY > dl.ActualHeight-40)
            {
                endSetY = dl.ActualHeight - 40;
            }
            if (endSetX > dl.ActualWidth - 140)
            {
                endSetX = dl.ActualWidth - 140;
            }
            ne.Top = endSetY;
            ne.Left = endSetX;
            ne.Right = 0;
            ne.Bottom = 0;
    
            bd.SetValue(Border.MarginProperty, ne);
        }

        void AssociatedObject_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //获得canvas
            if (bd == null)
            {
                //bd = ((this.AssociatedObject as FrameworkElement).Parent as FrameworkElement).Parent as Border;
                dl = (this.AssociatedObject as FrameworkElement).GetLogicalAncestor<AyLayer>();
                if (dl == null)
                {
                    dl = (this.AssociatedObject as FrameworkElement).GetVisualAncestor<AyLayer>();
                }
          
                if (dl != null)
                {
                    bd = (dl.Content as Grid).FindChild("body", typeof(Border)) as Border;
                }
             (this.AssociatedObject as FrameworkElement).Cursor = Cursors.SizeAll;
                //canvas.SizeChanged += canvas_SizeChanged;
            }
            if (dl.DragTitleBarStart != null)
            {
                dl.DragTitleBarStart();
            }
            isDragging = true;
            mouseOffset = e.GetPosition(this.bd);
            tempHeight = (double)this.bd.GetValue(FrameworkElement.ActualHeightProperty);
            tempWidth = (double)this.bd.GetValue(FrameworkElement.ActualWidthProperty);
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
