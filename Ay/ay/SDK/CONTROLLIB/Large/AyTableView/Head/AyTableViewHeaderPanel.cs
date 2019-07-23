using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Collections.Generic;

namespace ay.Controls
{
    public class AyTableViewHeaderPanel : Panel
    {
        private AyTableView _parentTableView;
        private AyTableView ParentTableView
        {
            get
            {
                _parentTableView = AyTableViewUtils.FindParent<AyTableView>(this);
                if (_parentTableView == null)
                {
                    var _1 = AyTableViewUtils.FindParent<AyTableViewColumnHeader>(this);
                    if (_1.IsNotNull())
                    {
                        _parentTableView = _1.Column.ParentTableView;
                        //_parentTableView = AyTableViewUtils.FindParent<AyTableView>(_1);
                        //if (_parentTableView == null)
                        //{
                        //        //_parentTableView = _11.Column.ParentTableView;

                        //}
                    }
                }


                return _parentTableView;
            }
        }

        protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {
            if (!WpfTreeHelper.IsInDesignMode)
            {
                base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
                if (ParentTableView.IsNotNull())
                {
                    this.Style = ParentTableView.HeaderPanelStyle;

                    ParentTableView.HeaderRowPresenter.HeaderItemsPanel = this;
                }
            }
           

        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var columns = ParentTableView.Columns;
            var children = base.Children;
            double leftX = 0;
            int fixedColumnCount = ParentTableView.FixedColumnCount;

            ParentTableView.ResetFixedClipRect();

            Rect fixedClip = ParentTableView.FixedClipRect;
            fixedClip.X = 0;
            fixedClip.Height = arrangeSize.Height;


            // Arrange the children into a line
            int idx = 0;
            Rect cellRect = new Rect(0, 0, 0, arrangeSize.Height);
            foreach (var child in children)
            {
                if (idx == fixedColumnCount)
                    leftX -= ParentTableView.HorizontalScrollOffset;

                cellRect.X = leftX;
                cellRect.Width = columns[idx].Width;
                leftX += cellRect.Width;

                (child as UIElement).Clip = null;
                if (idx >= fixedColumnCount)
                {
                    if (cellRect.Right < fixedClip.Right)
                        cellRect.X = -cellRect.Width;   // hide children that are to the left of the fixed columns
                    else
                    {
                        var overlap = fixedClip.Right - cellRect.X; // check for columns that overlap the fixed columns and clip them
                        if (overlap > 0)
                        {
                            var r = new Rect(overlap, cellRect.Y, cellRect.Width - overlap, cellRect.Height);
                            (child as UIElement).Clip = new RectangleGeometry(r);
                        }
                    }
                }
              (child as UIElement).Arrange(cellRect);

                ++idx;
            }
            return arrangeSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            var children = base.Children;
            foreach (var child in children)
            {
                var element = (child as UIElement);
                element.Measure(availableSize);
                size.Width += element.DesiredSize.Width;
                size.Height = Math.Max(size.Height, element.DesiredSize.Height);
            }

            return size;
        }
    }
}
