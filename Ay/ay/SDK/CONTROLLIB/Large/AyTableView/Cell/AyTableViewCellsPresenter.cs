using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace ay.Controls
{
    public class AyTableViewCellsPresenter : ItemsControl
    {


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(AyTableViewCellsPresenter), new PropertyMetadata(false, OnIsSelectedChanged));


        //public static readonly DependencyPropertyKey IsSelectedPropertyKey =
        //      DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(AyTableViewCellsPresenter), new PropertyMetadata(false, OnIsSelectedChanged));

        //public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        public int isMouseLeftDown = 1;
        private static void OnIsSelectedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var _vs = (source as AyTableViewCellsPresenter);
            if (_vs.isMouseLeftDown == 1)
            {
                _vs.OnCheckedFocusLe();
            }
            else if (_vs.isMouseLeftDown == 2)
            {
                if ((bool)e.NewValue)
                    _vs.UpdateSelection();
            }

        }
        //bool isChu = false;
        private void UpdateSelection()
        {
            if (ParentTableView.IsNotNull())
            {
                if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Single || ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle)
                {
                    if (ParentTableView.SelectedCellsPresenter != null)
                        ParentTableView.SelectedCellsPresenter.IsSelected = false;
                    ParentTableView.SelectedCellsPresenter = this;
                }
                else if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    ParentTableView.SelectedCellsPresenter = this;
                    //if (ParentTableView.SelectedCellsPresenter == this)
                    //{
                    //    ParentTableView.SelectedCellsPresenter.IsSelected = false;
                    //}
                    //else
                    //{
                    //    ParentTableView.SelectedCellsPresenter.IsSelected = true;
                    //}

                }

            }

        }

        //public bool IsSelected
        //{
        //    get { return (bool)GetValue(IsSelectedProperty); }
        //    private set { SetValue(IsSelectedPropertyKey, value); }
        //}




        /// <summary>
        /// 行状态
        /// </summary>
        public AyTableViewStatuss AyTableViewRowStatus
        {
            get { return (AyTableViewStatuss)GetValue(AyTableViewRowStatusProperty); }
            set { SetValue(AyTableViewRowStatusProperty, value); }
        }

        public static readonly DependencyProperty AyTableViewRowStatusProperty =
            DependencyProperty.Register("AyTableViewRowStatus", typeof(AyTableViewStatuss), typeof(AyTableViewCellsPresenter), new PropertyMetadata(AyTableViewStatuss.Normal));



        public AyTableView ParentTableView { get; set; }
        public AyTableViewCellsPanel CellsPanel { get; set; }

        protected override bool HandlesScrolling
        {
            get { return true; }
        }

        public object Item
        {
            get { return ItemsSource == null ? null : (ItemsSource as AyTableViewCellCollection).CopyObject; }
            private set
            {
                if (ItemsSource == null)
                    ItemsSource = new AyTableViewCellCollection(value, ParentTableView.Columns.Count);
                else
                    (ItemsSource as AyTableViewCellCollection).CopyObject = value;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ParentTableView.ShowHorizontalGridLines)
            {
                var pen = new Pen(ParentTableView.GridLinesBrush, 1.0);
                drawingContext.DrawLine(pen, new Point(0, base.RenderSize.Height - 0.5), new Point(base.RenderSize.Width, base.RenderSize.Height - 0.5));
                //Rect rectangle = new Rect(new Size(base.RenderSize.Width, 1));
                //rectangle.Y = base.RenderSize.Height - 1;
                //drawingContext.DrawRectangle(ParentTableView.GridLinesBrush, null, rectangle);
            }
        }

        public void ColumnsChanged()
        {
            var item = Item;
            ItemsSource = null;
            Item = item;

            CellsInvalidateArrange();
        }

        public void UpdateColumns()
        {
            (ItemsSource as AyTableViewCellCollection).Count = ParentTableView.Columns.Count;
        }

        public void CellsInvalidateArrange()
        {
            UpdateColumns();

            if (CellsPanel != null)
                CellsPanel.InvalidateArrange();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (ParentTableView.ShowHorizontalGridLines)
            {
                var tmp = new Size(arrangeBounds.Width, arrangeBounds.Height - 1);
                var size = base.ArrangeOverride(tmp);
                size.Height += 1;
                return size;
            }

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (ParentTableView.ShowHorizontalGridLines)
            {
                Size tmp = constraint;

                if (tmp.Height > 0)
                    tmp = new Size(constraint.Width, constraint.Height - 1);

                var size = base.MeasureOverride(tmp);
                size.Height += 1;
                return size;
            }
            return base.MeasureOverride(constraint);
        }

        public void PrepareRow(AyTableView parent, object dataItem)
        {
            ParentTableView = parent;
            Focusable = ParentTableView.CellNavigation == false;
            Item = dataItem;
            var scp = ParentTableView.SelectedCellsPresenter;
            var _cRow = ParentTableView.IndexOfRow(this);
            IsAlterRowLine = (_cRow + 1) % 2 == 0;
            if (scp != null)
            {
                if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Single)
                {
                    isMouseLeftDown = 2;
                    IsSelected = ParentTableView.IndexOfRow(scp) == _cRow;
                    isMouseLeftDown = 1;
                }
            }
        }

        /// <summary>
        /// 是否是偶数行
        /// </summary>
        public bool IsAlterRowLine
        {
            get { return (bool)GetValue(IsAlterRowLineProperty); }
            set { SetValue(IsAlterRowLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAlterRowLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAlterRowLineProperty =
            DependencyProperty.Register("IsAlterRowLine", typeof(bool), typeof(AyTableViewCellsPresenter), new FrameworkPropertyMetadata(false));

        ContentPresenter ccDetail = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ccDetail = GetTemplateChild("detailContent") as ContentPresenter;
        }

        public void Clear()
        {
            Item = null;
        }

        /// <summary>
        /// 是否编辑 2017-12-15 17:22:09
        /// 虚拟化问题
        /// </summary>
        public bool IsEdit
        {
            get { return (bool)GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }

        public static readonly DependencyProperty IsEditProperty =
            DependencyProperty.Register("IsEdit", typeof(bool), typeof(AyTableViewCellsPresenter), new FrameworkPropertyMetadata(false, OnIsEditChanged));



        private static void OnIsEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyTableViewCellsPresenter).NotifyIsEditChanged((bool)e.NewValue);
        }

        private void NotifyIsEditChanged(bool isopen)
        {
            //if (isopen)
            //{
            //    OpenEditRow(true);
            //}
            //else
            //{
            //    CloseEditRow(true);
            //}
        }


        public bool HasRowDetail
        {
            get { return (bool)GetValue(HasRowDetailProperty); }
            set { SetValue(HasRowDetailProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasRowDetail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasRowDetailProperty =
            DependencyProperty.Register("HasRowDetail", typeof(bool), typeof(AyTableViewCellsPresenter), new PropertyMetadata(false));


        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            (element as AyTableViewCell).PrepareCell(this, ItemContainerGenerator.IndexFromContainer(element));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new AyTableViewCell();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is AyTableViewCell);
        }

        //protected override void OnGotFocus(RoutedEventArgs e)
        //{
        //    base.OnGotFocus(e);
        //    OnFocusLe();
        //}
        private void OnCheckedFocusLe()
        {
            if (ParentTableView == null) return;
            if (ParentTableView.RowClickMode == AyTableView.RowClickModes.RowEdit)
            {
                if (ParentTableView.SelectedCellsPresenter != null)
                {
                    ParentTableView.SelectedCellsPresenter.IsEdit = false;
                }
                IsEdit = true;
            }
            else if (ParentTableView.RowClickMode == AyTableView.RowClickModes.RowDetail && ccDetail.IsNotNull())
            {
                if (ParentTableView.SelectedCellsPresenter != null)
                {
                    if (ParentTableView.SelectedCellsPresenter == this)
                    {
                        HasRowDetail = !HasRowDetail;
                        //ParentTableView.SelectedCellsPresenter.ccDetail.Visibility = ParentTableView.SelectedCellsPresenter.ccDetail.Visibility==Visibility.Collapsed? Visibility.Visible: Visibility.Collapsed;
                    }
                    else
                    {
                        ParentTableView.SelectedCellsPresenter.HasRowDetail = false;
                        //ParentTableView.SelectedCellsPresenter.ccDetail.Visibility = Visibility.Collapsed;
                        HasRowDetail = true;
                        //ccDetail.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    HasRowDetail = true;
                }
            }
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Single || ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle)
            {
                ParentTableView.FocusedRowChanged(this);
                if (ParentTableView.SelectedCellsPresenter != null)
                {
                    isMouseLeftDown = 3;
                    ParentTableView.SelectedCellsPresenter.IsSelected = false;
                    isMouseLeftDown = 1;
                }

                ParentTableView.SelectedCellsPresenter = this;

                if (ParentTableView.SelectedItem != Item)
                {
                    ParentTableView.SelectedItem = Item;
                    ParentTableView.RaiseSelectionChanged(Item);
                }
                else
                {
                    ParentTableView.SelectedItem = Item;
                }

            }
            else if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Multiple)
            {
                ParentTableView.FocusedRowChanged(this);
                ParentTableView.SelectedCellsPresenter = this;
                if (IsSelected)
                {
                    ParentTableView.SelectedItems.Add(Item);
                    ParentTableView.RaiseOnMultipleSelectionAdd(Item);
                }
                else
                {
                    ParentTableView.SelectedItems.Remove(Item);
                    ParentTableView.RaiseOnMultipleSelectionRemove(Item);
                }
            }


        }
        private void OnFocusLe()
        {
            isMouseLeftDown = 2;
            if (ParentTableView.RowClickMode == AyTableView.RowClickModes.RowEdit)
            {
                if (ParentTableView.SelectedCellsPresenter != null)
                {
                    ParentTableView.SelectedCellsPresenter.IsEdit = false;
                }
                IsEdit = true;
            }
            else if (ParentTableView.RowClickMode == AyTableView.RowClickModes.RowDetail && ccDetail.IsNotNull())
            {
                if (ParentTableView.SelectedCellsPresenter != null)
                {
                    if (ParentTableView.SelectedCellsPresenter == this)
                    {
                        HasRowDetail = !HasRowDetail;
                        //ParentTableView.SelectedCellsPresenter.ccDetail.Visibility = ParentTableView.SelectedCellsPresenter.ccDetail.Visibility==Visibility.Collapsed? Visibility.Visible: Visibility.Collapsed;
                    }
                    else
                    {
                        ParentTableView.SelectedCellsPresenter.HasRowDetail = false;
                        //ParentTableView.SelectedCellsPresenter.ccDetail.Visibility = Visibility.Collapsed;
                        HasRowDetail = true;
                        //ccDetail.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    HasRowDetail = true;
                }
            }
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Single || ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle)
            {
                ParentTableView.FocusedRowChanged(this);
                IsSelected = true;

                if (ParentTableView.SelectedItem != Item)
                {
                    ParentTableView.SelectedItem = Item;
                    ParentTableView.RaiseSelectionChanged(Item);
                }
                else
                {
                    ParentTableView.SelectedItem = Item;
                }
            }
            else if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Multiple)
            {
                ParentTableView.FocusedRowChanged(this);
                IsSelected = !IsSelected;
                if (IsSelected)
                {
                    ParentTableView.SelectedItems.Add(Item);
                    ParentTableView.RaiseOnMultipleSelectionAdd(Item);

                }
                else
                {
                    ParentTableView.SelectedItems.Remove(Item);
                    ParentTableView.RaiseOnMultipleSelectionRemove(Item);
                }
            }
            isMouseLeftDown = 1;

        }
        /// <summary>
        /// 开始编辑说
        /// </summary>
        public void OpenEditRow(bool trigger = false)
        {

            //if (CellsPanel.IsNull())
            //{
            //    return;
            //}
            //var _1 = CellsPanel.Children;
            //if (_1.IsNotNull())
            //{
            //    foreach (var item in _1)
            //    {
            //        var _2 = item as AyTableViewCell;
            //        if (_2.IsNotNull())
            //        {
            //            _2.IsEdit = true;
            //        }
            //    }
            //    if (trigger)
            //    {
            //        return;
            //    }
            //    ParentTableView.RaiseRowEditBegin(Item);
            //}
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseEditRow(bool isCancel = false)
        {
            //var _1 = CellsPanel.Children;
            //if (_1.IsNotNull())
            //{
            //    foreach (var item in _1)
            //    {
            //        var _2 = item as AyTableViewCell;
            //        if (_2.IsNotNull())
            //        {
            //            _2.IsEdit = false;
            //        }
            //    }
            //    if (isCancel) return;
            //    ParentTableView.RaiseRowEditEnd(Item);
            //}
        }


        protected override void OnTouchDown(TouchEventArgs e)
        {
            ParentTableView.FocusedRowChanged(this);
            base.OnTouchDown(e);
            this.Focus();
            OnFocusLe();

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            ParentTableView.FocusedRowChanged(this);
            base.OnMouseLeftButtonDown(e);
            this.Focus();
            OnFocusLe();
        }
        //protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        //{
            //ParentTableView.FocusedRowChanged(this);
            //base.OnMouseRightButtonDown(e);
            //this.Focus();
            //OnFocusLe();
        //}
    }
}
