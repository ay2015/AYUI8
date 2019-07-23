using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


namespace ay.Controls
{
    public sealed class ItemsControlSelector
    {
        private readonly ItemsControl itemsControl;
        private Rect previousArea;

        /// <summary>
        /// Initializes a new instance of the ItemsControlSelector class.
        /// </summary>
        /// <param name="itemsControl">
        /// The control that contains the items to select.
        /// </param>
        /// <exception cref="ArgumentNullException">itemsControl is null.</exception>
        public ItemsControlSelector(ItemsControl itemsControl)
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
                FrameworkElement item = this.itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (item != null)
                {
                    // Get the bounds in the parent's co-ordinates.
                    Point topLeft = item.TranslatePoint(new Point(0, 0), this.itemsControl);
                    Rect itemBounds = new Rect(topLeft.X, topLeft.Y, item.ActualWidth, item.ActualHeight);

                    // Only change the selection if it intersects with the area
                    // (or intersected i.e. we changed the value last time).
                    if (itemBounds.IntersectsWith(area))
                    {
                        Selector.SetIsSelected(item, true);
                    }
                    else if (itemBounds.IntersectsWith(this.previousArea))
                    {
                        // We previously changed the selection to true but it no
                        // longer intersects with the area so clear the selection.
                        Selector.SetIsSelected(item, false);
                    }
                }
            }
            this.previousArea = area;
        }
    }
}
