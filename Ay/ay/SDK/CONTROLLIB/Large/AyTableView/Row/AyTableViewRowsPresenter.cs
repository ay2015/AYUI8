using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Collections.Specialized;

namespace ay.Controls
{

    public class AyTableViewRowsPresenter : ItemsControl
    {
        public AyTableViewRowsPresenter()
        {
            Loaded += AyTableViewRowsPresenter_Loaded;
        }

        private void AyTableViewRowsPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AyTableViewRowsPresenter_Loaded;
            if (ParentTableView.IsNotNull())
            {
                if (ParentTableView.PercentWidthSupport)
                {
                    ParentTableView.SizeChanged += ParentTableView_SizeChanged;
                }

            }
        }

        private void ParentTableView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ParentTableView.SizeChangedInvoke();


        }

        public AyTableViewRowsPanel RowsPanel { get; set; }

        private AyTableView _parentTableView;
        public AyTableView ParentTableView
        {
            get
            {
                if (_parentTableView == null)
                    _parentTableView = AyTableViewUtils.FindParent<AyTableView>(this);
                return _parentTableView;
            }
        }

        internal object GetItemAtLocation(Point loc)
        {
            var uie = InputHitTest(loc) as FrameworkElement;

            if (uie != null)
            {
                var rowPresenter = AyTableViewUtils.GetAncestorByType<AyTableViewCellsPresenter>(uie);

                if (rowPresenter != null)
                    return rowPresenter.Item;
            }

            return null;
        }

        internal int GetCellIndexAtLocation(Point loc)
        {
            var uie = InputHitTest(loc) as FrameworkElement;
            if (uie != null)
            {
                var rowPresenter = AyTableViewUtils.GetAncestorByType<AyTableViewCellsPresenter>(uie);

                if (rowPresenter != null)
                {
                    var cell = AyTableViewUtils.FindParent<AyTableViewCell>(uie);
                    if (cell != null)
                        return rowPresenter.ItemContainerGenerator.IndexFromContainer(cell);
                }
            }
            return -1;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ItemsSource = ParentTableView.ItemsSource;
            ParentTableView.ContentScollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
        }


        protected override bool HandlesScrolling
        {
            get { return true; }
        }

        public void ColumnsChanged()
        {
            if (RowsPanel != null)
                RowsPanel.ColumnsChanged();
        }

        public void RowsInvalidateArrange()
        {
            if (RowsPanel != null)
                RowsPanel.RowsInvalidateArrange();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (!WpfTreeHelper.IsInDesignMode)
                BringIndexIntoView(ParentTableView.FocusedRowIndex);
        }

        // Container generator
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            (element as AyTableViewCellsPresenter).Clear();
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            (element as AyTableViewCellsPresenter).PrepareRow(ParentTableView, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var container = new AyTableViewCellsPresenter();
            return container;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is AyTableViewCellsPresenter);
        }

        private int _focusedRowIndex;
        private void OnStatusChanged(object sender, EventArgs e)
        {
            if (this.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                this.ItemContainerGenerator.StatusChanged -= OnStatusChanged;

                var el2 = this.ItemContainerGenerator.ContainerFromIndex(_focusedRowIndex) as FrameworkElement;
                if (el2 != null)
                {
                    el2.Focus();
                    el2.BringIntoView();
                }
            }
        }

        public void BringIndexIntoView(int idx)
        {
            if (ParentTableView.SelectionMode != AyTableViewSelectionMode.NoSelect)
            {
                if (RowsPanel != null && idx >= 0 && idx < this.Items.Count)
                {
                    _focusedRowIndex = idx;
                    var el = this.ItemContainerGenerator.ContainerFromIndex(_focusedRowIndex) as FrameworkElement;
                    if (el == null)
                    {
                        this.ItemContainerGenerator.StatusChanged += OnStatusChanged;
                        RowsPanel.BringRowIntoView(_focusedRowIndex);
                    }
                    else
                      if (el != null)
                    {
                        el.Focus();
                        el.BringIntoView();
                    }
                }
            }
        }

        protected void OnPageUpOrDownKeyDown(KeyEventArgs e)
        {
            bool moveForward = e.Key == Key.Next;

            int idx = this.ItemContainerGenerator.IndexFromContainer(e.OriginalSource as FrameworkElement);

            int viewheight = (RowsPanel != null) ? (int)RowsPanel.ViewportHeight : 0;
            idx = moveForward ? idx + viewheight : idx - viewheight; // calculate the new index for the focus

            if (idx < 0 || idx >= this.Items.Count) // adjust to the bounds of the collection
                idx = moveForward ? Items.Count - 1 : 0;

            BringIndexIntoView(idx);
            e.Handled = true;
        }

        protected void OnHomeOrEndKeyDown(KeyEventArgs e)
        {
            int idx = (e.Key == Key.Home) ? 0 : this.Items.Count - 1;
            BringIndexIntoView(idx);
            e.Handled = true;
        }

        protected void OnLeftOrRightKeyDown(KeyEventArgs e)
        {
            if (ParentTableView.CellNavigation == false)
                e.Handled = true;
            else
            {
                if (e.Key == Key.Left && ParentTableView.FocusedColumnIndex <= 0)
                    e.Handled = true;
                if (e.Key == Key.Right && ParentTableView.FocusedColumnIndex >= ParentTableView.Columns.Count - 1)
                    e.Handled = true;
            }
        }

        protected void OnUpOrDownKeyDown(KeyEventArgs e)
        {
            int idx = -1;
            if (ParentTableView.CellNavigation)
            {
                var _22 = e.OriginalSource as AyTableViewCell;
                if (_22.IsNotNull())
                {
                    if (_22.ParentCellsPresenter.IsNotNull())
                    {
                        idx = this.ItemContainerGenerator.IndexFromContainer(_22.ParentCellsPresenter);
                    }
                }
            }
            else
            {
                var _22 = e.OriginalSource as AyTableViewCellsPresenter;
                if (_22.IsNotNull())
                {
                    idx = this.ItemContainerGenerator.IndexFromContainer(_22);
                }
            }
            if (e.Key == Key.Up && idx <= 0)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Down && idx >= this.Items.Count - 1)
                e.Handled = true;

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            Key key = e.Key;
            if (ParentTableView.CantNavigation())
            {
                if (key == Key.Tab) e.Handled = true;
                return;
            }

            switch (key)
            {
                case Key.Prior:
                case Key.Next:
                    this.OnPageUpOrDownKeyDown(e);
                    break;

                case Key.End:
                case Key.Home:
                    OnHomeOrEndKeyDown(e);
                    break;

                case Key.Left:
                case Key.Right:
                    OnLeftOrRightKeyDown(e);
                    break;

                case Key.Up:
                case Key.Down:
                    OnUpOrDownKeyDown(e);
                    break;
                case Key.Escape:
                    OnEscKeyDown(e);
                    break;
            }

            if (!e.Handled)
                base.OnKeyDown(e);
        }

        private void OnEscKeyDown(KeyEventArgs e)
        {
            if (ParentTableView.RowClickMode == AyTableView.RowClickModes.RowEdit)
            {
                if (ParentTableView.SelectedCellsPresenter.IsNotNull())
                {
                    ParentTableView.SelectedCellsPresenter.IsEdit = false;
                }
            }
            //else if (ParentTableView.RowClickMode == AyTableView.RowClickModes.CellEdit)
            //{
            //    if (ParentTableView.SelectedCell.IsNotNull())
            //    {
            //        ParentTableView.SelectedCell.CloseEditCell(true);
            //    }

            //}
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            // if we have clicked on the control but not on a row.
            if (GetItemAtLocation(e.GetPosition(this)) == null)
                this.Focus();

            base.OnMouseDown(e);
        }

        internal void NotifyDoubleClickRow(object data)
        {
            if (ParentTableView != null)
                ParentTableView.NotifyDoubleClickRow(data);
        }


        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Single || ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle)
            {
                var _1 = e.OriginalSource as FrameworkElement;
                if (_1.IsNotNull())
                {
                    var _2 = _1.GetVisualAncestor<AyTableViewCellsPresenter>();
                    if (_2.IsNotNull())
                        NotifyDoubleClickRow(_2.DataContext);
                }

            }
            else
            {
                e.Handled = true;
            }

        }

    }
}
