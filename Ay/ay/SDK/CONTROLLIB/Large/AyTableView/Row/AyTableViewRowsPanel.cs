using System.Windows.Controls;
using System.Windows;

namespace ay.Controls
{
    public class AyTableViewRowsPanel : VirtualizingStackPanel
    {
        private AyTableView _parentTableView;
        private AyTableView ParentTableView
        {
            get
            {
                if (_parentTableView == null)
                    _parentTableView = AyTableViewUtils.FindParent<AyTableView>(this);
                return _parentTableView;
            }
        }

        private AyTableViewRowsPresenter _parentRowsPresenter;
        private AyTableViewRowsPresenter ParentRowsPresenter
        {
            get
            {
                if (_parentRowsPresenter == null)
                    _parentRowsPresenter = AyTableViewUtils.FindParent<AyTableViewRowsPresenter>(this);
                return _parentRowsPresenter;
            }
        }

        protected override void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset)
        {
            ParentTableView.HorizontalScrollOffset = newViewportOffset.X;
            ParentTableView.HorizontalScrollOffset2 = newViewportOffset.X;
        }

        protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {
            base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
            this.Style = ParentTableView.RowsPanelStyle;
            this.ParentRowsPresenter.RowsPanel = this;
        }

        public void BringRowIntoView(int idx)
        {
            if (idx >= 0 && idx < ParentRowsPresenter.Items.Count)
                this.BringIndexIntoView(idx);
        }

        internal void ColumnsChanged()
        {
            foreach (var child in Children)
                (child as AyTableViewCellsPresenter).ColumnsChanged();

        }

        internal void RowsInvalidateArrange()
        {
            foreach (var child in Children)
                (child as AyTableViewCellsPresenter).CellsInvalidateArrange();
        }
    }
}
