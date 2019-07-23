using ay.Controls.Args;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ay.Controls
{

    public class AyTableViewSelectionAdorner : Adorner
    {
        private Brush selectionBrush;

        public Brush SelectionBrush
        {
            get { return selectionBrush; }
            set { selectionBrush = value; }
        }

        private Rect selectionRect;

        /// <summary>
        /// Initializes a new instance of the AyTableViewSelectionAdorner class.
        /// </summary>
        /// <param name="parent">
        /// The UIElement which this instance will overlay.
        /// </param>
        /// <exception cref="ArgumentNullException">parent is null.</exception>
        public AyTableViewSelectionAdorner(UIElement parent, Brush _selectionBrush)
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

                //var _1 = SelectionBrush as SolidColorBrush;
                drawingContext.DrawRectangle(SelectionBrush, new Pen(SelectionBrush, 1.0), this.SelectionArea);
            }
        }
    }
    public sealed class AyTableViewItemsControlSelector
    {
        private readonly ItemsControl itemsControl;
        private Rect previousArea;

        /// <summary>
        /// Initializes a new instance of the AyTableViewItemsControlSelector class.
        /// </summary>
        /// <param name="itemsControl">
        /// The control that contains the items to select.
        /// </param>
        /// <exception cref="ArgumentNullException">itemsControl is null.</exception>
        public AyTableViewItemsControlSelector(ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }
            this.itemsControl = itemsControl;
        }

        /// <summary>
        /// Resets the cached information, allowing a new selection to begin.
        /// </summary>
        public void Reset()
        {
            this.previousArea = new Rect();
        }

        /// <summary>
        /// Scrolls the selection area by the specified amount.
        /// </summary>
        /// <param name="x">The horizontal scroll amount.</param>
        /// <param name="y">The vertical scroll amount.</param>
        public void Scroll(double x, double y)
        {
            this.previousArea.Offset(-x, -y);
        }

        /// <summary>
        /// Updates the controls selection based on the specified area.
        /// </summary>
        /// <param name="area">
        /// The selection area, relative to the control passed in the contructor.
        /// </param>
        public void UpdateSelection(Rect area)
        {
            // Check eack item to see if it intersects with the area.
            for (int i = 0; i < this.itemsControl.Items.Count; i++)
            {
                AyTableViewCellsPresenter item = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as AyTableViewCellsPresenter;
                if (item != null)
                {
                    // Get the bounds in the parent's co-ordinates.
                    Point topLeft = item.TranslatePoint(new Point(0, 0), this.itemsControl);
                    Rect itemBounds = new Rect(topLeft.X, topLeft.Y, item.ActualWidth, item.ActualHeight);

                    // Only change the selection if it intersects with the area
                    // (or intersected i.e. we changed the value last time).
                    if (itemBounds.IntersectsWith(area))
                    {
                        item.isMouseLeftDown = 2;
                        item.IsSelected = true;
                        item.isMouseLeftDown = 1;
                        if (!item.ParentTableView.SelectedItems.Contains(item.Item))
                        {
                            item.ParentTableView.SelectedItems.Add(item.Item);
                        }
                    }
                    else if (itemBounds.IntersectsWith(this.previousArea))
                    {
                        item.isMouseLeftDown = 2;
                        item.IsSelected = false;
                        item.isMouseLeftDown = 1;
                        if (item.ParentTableView.SelectedItems.Contains(item.Item))
                        {
                            item.ParentTableView.SelectedItems.Remove(item.Item);
                        }
                    }
                }
            }
            this.previousArea = area;
        }
    }

    public class AyTableViewSelector
    {
        /// <summary>Identifies the IsEnabled attached property.</summary>
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(AyTableViewSelector), new UIPropertyMetadata(false, IsEnabledChangedCallback));

        // This stores the ListBoxSelector for each ListBox so we can unregister it.
        private static readonly Dictionary<AyTableViewRowsPresenter, AyTableViewSelector> attachedControls = new Dictionary<AyTableViewRowsPresenter, AyTableViewSelector>();

        private readonly AyTableView twv;

        private readonly AyTableViewRowsPresenter listBox;
        private ScrollContentPresenter scrollContent;

        private AyTableViewSelectionAdorner selectionRect;
        private AutoScroller autoScroller;
        private AyTableViewItemsControlSelector selector;

        private bool mouseCaptured;
        private Point start;
        private Point end;


        public static Brush GetSelectionBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(SelectionBrushProperty);
        }

        public static void SetSelectionBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectionBrushProperty, value);
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.RegisterAttached("SelectionBrush", typeof(Brush), typeof(AyTableViewSelector), new PropertyMetadata(SolidColorBrushConverter.From16JinZhi("#AFFFE48D")));



        private AyTableViewSelector(AyTableViewRowsPresenter listBox, AyTableView tv)
        {
            this.listBox = listBox;
            this.twv = tv;
            if (this.listBox.IsLoaded)
            {
                this.Register();
            }
            else
            {
                // We need to wait for it to be loaded so we can find the
                // child controls.
                this.listBox.Loaded += this.OnListBoxLoaded;
            }
        }

        /// <summary>
        /// Gets the value of the IsEnabled attached property that indicates
        /// whether a selection rectangle can be used to select items or not.
        /// </summary>
        /// <param name="obj">Object on which to get the property.</param>
        /// <returns>
        /// true if items can be selected by a selection rectangle; otherwise, false.
        /// </returns>
        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }

        /// <summary>
        /// Sets the value of the IsEnabled attached property that indicates
        /// whether a selection rectangle can be used to select items or not.
        /// </summary>
        /// <param name="obj">Object on which to set the property.</param>
        /// <param name="value">Value to set.</param>
        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        private static void IsEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AyTableView atv = d as AyTableView;
            var listBox = atv.RowsPresenter;
            if (listBox != null)
            {
                if ((bool)e.NewValue)
                {
                    if (atv.SelectionMode == AyTableViewSelectionMode.Single)
                    {
                        atv.SelectionMode = AyTableViewSelectionMode.Multiple;
                    }

                    attachedControls.Add(listBox, new AyTableViewSelector(listBox, atv));
                }
                else
                {
                    AyTableViewSelector selector;
                    if (attachedControls.TryGetValue(listBox, out selector))
                    {
                        attachedControls.Remove(listBox);
                        selector.UnRegister();
                    }
                }
            }


        }

        private bool Register()
        {
            this.listBox.UpdateLayout();
            this.scrollContent = WpfTreeHelper.FindChild<ScrollContentPresenter>(this.listBox);
            if (this.scrollContent != null)
            {
                this.autoScroller = new AutoScroller(this.listBox);
                this.autoScroller.OffsetChanged += this.OnOffsetChanged;
                var d = GetSelectionBrush(this.listBox);
                this.selectionRect = new AyTableViewSelectionAdorner(this.scrollContent, d);
                this.scrollContent.AdornerLayer.Add(this.selectionRect);

                this.selector = new AyTableViewItemsControlSelector(this.listBox);
                this.listBox.PreviewMouseLeftButtonDown += this.OnPreviewMouseLeftButtonDown;
                this.listBox.MouseLeftButtonUp += this.OnMouseLeftButtonUp;
                this.listBox.MouseMove += this.OnMouseMove;
            }

            return (this.scrollContent != null);
        }

        private void UnRegister()
        {
            this.StopSelection();

            // Remove all the event handlers so this instance can be reclaimed by the GC.
            this.listBox.PreviewMouseLeftButtonDown -= this.OnPreviewMouseLeftButtonDown;
            this.listBox.MouseLeftButtonUp -= this.OnMouseLeftButtonUp;
            this.listBox.MouseMove -= this.OnMouseMove;
            if (autoScroller != null)
                this.autoScroller.UnRegister();
        }

        private void OnListBoxLoaded(object sender, EventArgs e)
        {
            if (this.Register())
            {
                this.listBox.Loaded -= this.OnListBoxLoaded;
            }
        }

        private void OnOffsetChanged(object sender, OffsetChangedEventArgs e)
        {
            this.selector.Scroll(e.HorizontalChange, e.VerticalChange);
            this.UpdateSelection();
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
    
                this.mouseCaptured = false;
                this.scrollContent.ReleaseMouseCapture();
                this.StopSelection();
                isDragSelect = false;
               firstpoint =null;
           
        }
        bool isDragSelect = false;
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isScrollerBar)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    var _1 = e.GetPosition(this.scrollContent);
                    if (!isDragSelect && firstpoint!=null)
                    {
                        if (Math.Abs(_1.X - firstpoint.Value.X) > 40 || Math.Abs(_1.Y - firstpoint.Value.Y) > 40) //AY 2017-12-6 16:37:14
                        {
                            isDragSelect = true;
                            if ((firstpoint.Value.X >= 0) && (firstpoint.Value.X < this.scrollContent.ActualWidth) &&
                                (firstpoint.Value.Y >= 0) && (firstpoint.Value.Y < this.scrollContent.ActualHeight))
                            {
                                this.mouseCaptured = this.scrollContent.CaptureMouse();

                                this.StartSelection(firstpoint.Value);
                            }
                        }
                    }
                    if (this.mouseCaptured)
                    {
                        this.end = _1;
                        this.autoScroller.Update(this.end);
                        this.UpdateSelection();
                    }
                }
            }
        }
        static bool HitTestScrollBar(object sender, MouseButtonEventArgs e)
        {
            HitTestResult hit = VisualTreeHelper.HitTest((Visual)sender, e.GetPosition((IInputElement)sender));
            return hit.VisualHit.GetVisualAncestor<System.Windows.Controls.Primitives.ScrollBar>() != null;
        }
        Point? firstpoint = null;
        bool isScrollerBar = false;
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (HitTestScrollBar(sender, e))
            {
                isScrollerBar = true;
                return;
            }
            else
            {
                isScrollerBar = false;
            }
            // Check that the mouse is inside the scroll content (could be on the
            // scroll bars for example).
            Point mouse = e.GetPosition(this.scrollContent);
            firstpoint = mouse;
            //if ((mouse.X >= 0) && (mouse.X < this.scrollContent.ActualWidth) &&
            //    (mouse.Y >= 0) && (mouse.Y < this.scrollContent.ActualHeight))
            //{
            //    this.mouseCaptured = this.TryCaptureMouse(e);
            //    if (this.mouseCaptured)
            //    {
            //        this.StartSelection(mouse);
            //    }
            //}
        }

        //private bool TryCaptureMouse(MouseButtonEventArgs e)
        //{
        //    Point position = e.GetPosition(this.scrollContent);

        //    // Check if there is anything under the mouse.
        //    UIElement element = this.scrollContent.InputHitTest(position) as UIElement;
        //    if (element != null)
        //    {

        //        //var _1 = WpfTreeHelper.FindParentControl<AyTableViewCellsPresenter>(element);
        //        // if (_1.IsNotNull())
        //        // {
        //        //     return _1.IsSelected;
        //        // }

        //        // Simulate a mouse click by sending it the MouseButtonDown
        //        // event based on the data we received.
        //        //var args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left, e.StylusDevice);
        //        //args.RoutedEvent = Mouse.MouseDownEvent;
        //        //args.Source = e.Source;
        //        //_1.RaiseEvent(args);

        //        // The ListBox will try to capture the mouse unless something
        //        // else captures it.
        //        //if (Mouse.Captured != this.listBox)
        //        //{
        //        //    return false; // Something else wanted the mouse, let it keep it.
        //        //}
        //        //}
        //    }

        //    // Either there's nothing under the mouse or the element doesn't want the mouse.
        //    return this.scrollContent.CaptureMouse();
        //}

        private void StopSelection()
        {
            // Hide the selection rectangle and stop the auto scrolling.
            if (this.selectionRect != null)
            {
                this.selectionRect.IsEnabled = false;

            }
            if (this.autoScroller != null)
            {
                this.autoScroller.IsEnabled = false;
            }
        }

        private void StartSelection(Point location)
        {
            // We've stolen the MouseLeftButtonDown event from the ListBox
            // so we need to manually give it focus.
            this.listBox.Focus();

            this.start = location;
            this.end = location;

            // Do we need to start a new selection?
            if (((Keyboard.Modifiers & ModifierKeys.Control) == 0) &&
                ((Keyboard.Modifiers & ModifierKeys.Shift) == 0))
            {
                // Neither the shift key or control key is pressed, so
                // clear the selection.
                twv.UnCheckAll();
            }

            this.selector.Reset();
            this.UpdateSelection();

            this.selectionRect.IsEnabled = true;
            this.autoScroller.IsEnabled = true;
        }

        private void UpdateSelection()
        {
            // Offset the start point based on the scroll offset.
            Point start = this.autoScroller.TranslatePoint(this.start);

            // Draw the selecion rectangle.
            // Rect can't have a negative width/height...
            double x = Math.Min(start.X, this.end.X);
            double y = Math.Min(start.Y, this.end.Y);
            double width = Math.Abs(this.end.X - start.X);
            double height = Math.Abs(this.end.Y - start.Y);
            Rect area = new Rect(x, y, width, height);
            this.selectionRect.SelectionArea = area;

            // Select the items.
            // Transform the points to be relative to the ListBox.
            Point topLeft = this.scrollContent.TranslatePoint(area.TopLeft, this.listBox);
            Point bottomRight = this.scrollContent.TranslatePoint(area.BottomRight, this.listBox);

            // And select the items.
            this.selector.UpdateSelection(new Rect(topLeft, bottomRight));
        }

    }
}
