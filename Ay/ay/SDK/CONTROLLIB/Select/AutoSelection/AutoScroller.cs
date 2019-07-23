using ay.Controls.Args;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ay.Controls
{
    public sealed class AutoScroller
    {
        private readonly DispatcherTimer autoScroll = new DispatcherTimer();
        private readonly ItemsControl itemsControl;
        private readonly ScrollViewer scrollViewer;
        private readonly ScrollContentPresenter scrollContent;
        private bool isEnabled;
        private Point offset;
        private Point mouse;

        /// <summary>
        /// Initializes a new instance of the AutoScroller class.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl that is scrolled.</param>
        /// <exception cref="ArgumentNullException">itemsControl is null.</exception>
        public AutoScroller(ItemsControl itemsControl)
        {
            if (itemsControl == null)
            {
                throw new ArgumentNullException("itemsControl");
            }

            this.itemsControl = itemsControl;
            this.scrollViewer = WpfTreeHelper.FindChild<ScrollViewer>(itemsControl);
            this.scrollViewer.ScrollChanged += this.OnScrollChanged;
            this.scrollContent = WpfTreeHelper.FindChild<ScrollContentPresenter>(this.scrollViewer);

            this.autoScroll.Tick += delegate { this.PreformScroll(); };
            this.autoScroll.Interval = TimeSpan.FromMilliseconds(GetRepeatRate());
        }

        /// <summary>Occurs when the scroll offset has changed.</summary>
        public event EventHandler<OffsetChangedEventArgs> OffsetChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the auto-scroller is enabled
        /// or not.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                if (this.isEnabled != value)
                {
                    this.isEnabled = value;

                    // Reset the auto-scroller and offset.
                    this.autoScroll.IsEnabled = false;
                    this.offset = new Point();
                }
            }
        }

        /// <summary>
        /// Translates the specified point by the current scroll offset.
        /// </summary>
        /// <param name="point">The point to translate.</param>
        /// <returns>A new point offset by the current scroll amount.</returns>
        public Point TranslatePoint(Point point)
        {
            return new Point(point.X - this.offset.X, point.Y - this.offset.Y);
        }

        /// <summary>
        /// Removes all the event handlers registered on the control.
        /// </summary>
        public void UnRegister()
        {
            this.scrollViewer.ScrollChanged -= this.OnScrollChanged;
        }

        /// <summary>
        /// Updates the location of the mouse and automatically scrolls if required.
        /// </summary>
        /// <param name="mouse">
        /// The location of the mouse, relative to the ScrollViewer's content.
        /// </param>
        public void Update(Point mouse)
        {
            this.mouse = mouse;

            // If scrolling isn't enabled then see if it needs to be.
            if (!this.autoScroll.IsEnabled)
            {
                this.PreformScroll();
            }
        }

        // Returns the default repeat rate in milliseconds.
        private static int GetRepeatRate()
        {
            // The RepeatButton uses the SystemParameters.KeyboardSpeed as the
            // default value for the Interval property. KeyboardSpeed returns
            // a value between 0 (400ms) and 31 (33ms).
            const double Ratio = (400.0 - 33.0) / 31.0;
            return 400 - (int)(SystemParameters.KeyboardSpeed * Ratio);
        }

        private double CalculateOffset(int startIndex, int endIndex)
        {
            double sum = 0;
            for (int i = startIndex; i != endIndex; i++)
            {
                FrameworkElement container = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (container != null)
                {
                    // Height = Actual height + margin
                    sum += container.ActualHeight;
                    sum += container.Margin.Top + container.Margin.Bottom;
                }
            }
            return sum;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Do we need to update the offset?
            if (this.IsEnabled)
            {
                double horizontal = e.HorizontalChange;
                double vertical = e.VerticalChange;

                // VerticalOffset means two seperate things based on the CanContentScroll
                // property. If this property is true then the offset is the number of
                // items to scroll; false then it's in Device Independant Pixels (DIPs).
                if (this.scrollViewer.CanContentScroll)
                {
                    // We need to either increase the offset or decrease it.
                    if (e.VerticalChange < 0)
                    {
                        int start = (int)e.VerticalOffset;
                        int end = (int)(e.VerticalOffset - e.VerticalChange);
                        vertical = -this.CalculateOffset(start, end);
                    }
                    else
                    {
                        int start = (int)(e.VerticalOffset - e.VerticalChange);
                        int end = (int)e.VerticalOffset;
                        vertical = this.CalculateOffset(start, end);
                    }
                }

                this.offset.X += horizontal;
                this.offset.Y += vertical;

                var callback = this.OffsetChanged;
                if (callback != null)
                {
                    callback(this, new OffsetChangedEventArgs(horizontal, vertical));
                }
            }
        }

        private void PreformScroll()
        {
            bool scrolled = false;

            if (this.mouse.X > this.scrollContent.ActualWidth)
            {
                this.scrollViewer.LineRight();
                scrolled = true;
            }
            else if (this.mouse.X < 0)
            {
                this.scrollViewer.LineLeft();
                scrolled = true;
            }

            if (this.mouse.Y > this.scrollContent.ActualHeight)
            {
                this.scrollViewer.LineDown();
                scrolled = true;
            }
            else if (this.mouse.Y < 0)
            {
                this.scrollViewer.LineUp();
                scrolled = true;
            }

            // It's important to disable scrolling if we're inside the bounds of
            // the control so that when the user does leave the bounds we can
            // re-enable scrolling and it will have the correct initial delay.
            this.autoScroll.IsEnabled = scrolled;
        }
    }
}
