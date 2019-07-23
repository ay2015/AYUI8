using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ay.Controls
{
    //-----------------------------------------------------------------------------------
    public class AyTableViewHeaderThumb : Thumb
    {
        public AyTableViewHeaderThumb()
          : base()
        {
            PreviewMouseLeftButtonDown += (s, e) => Mouse.Capture(this);
            PreviewMouseLeftButtonUp += (s, e) => Mouse.Capture(null);
            DragDelta += onDragDelta;
        }

        public void onDragDelta(object sender, DragDeltaEventArgs e)
        {
            var tvch = AyTableViewUtils.GetAncestorByType<AyTableViewColumnHeader>(this);

            if (tvch != null)
            {
                var width = tvch.Width + e.HorizontalChange;
                if (tvch.Column.MinResizeColumnWidth.HasValue && width< tvch.Column.MinResizeColumnWidth)
                {
                    width = tvch.Column.MinResizeColumnWidth.Value;
                }
                if (tvch.Column.MaxResizeColumnWidth.HasValue && width > tvch.Column.MaxResizeColumnWidth)
                {
                    width = tvch.Column.MaxResizeColumnWidth.Value;
                }
                tvch.Column.ParentTableView.IsResizeColumnWidth = true;
                tvch.AdjustWidth(width);
            }
        }
    }

}
