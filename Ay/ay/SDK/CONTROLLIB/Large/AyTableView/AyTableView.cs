using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;
using System.Collections.Generic;
using System.Windows.Data;
using System.IO;
using System.Linq;
using System.Windows.Input;


public static class AyTableViewExtension
{
    public static T GetRouteArgs<T>(this object obj) where T : class
    {
        return (obj as object[])[2] as T;
    }
}

namespace ay.Controls
{
    [TemplatePart(Name = "PART_HeaderPresenter", Type = typeof(AyTableViewHeaderPresenter))]
    [TemplatePart(Name = "PART_HeaderPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_RowsPresenter", Type = typeof(AyTableViewRowsPresenter))]
    [TemplatePart(Name = "PART_RowsPanel", Type = typeof(Panel))]
    [ContentProperty("ColumnsHead")]
    [StyleTypedProperty(Property = "ScrollViewerStyle", StyleTargetType = typeof(ScrollViewer))]
    public class AyTableView : Control
    {


        static AyTableView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyTableView), new FrameworkPropertyMetadata(typeof(AyTableView)));
            {
                InputGestureCollection inputs = new InputGestureCollection();
                inputs.Add(new KeyGesture(Key.A, ModifierKeys.Control, "Ctrl+A"));
                _SelectedAllRows = new RoutedUICommand("全选(_M)", "SelectedAllRows", typeof(AyTableView), inputs);
            }
        }
        private static RoutedUICommand _SelectedAllRows;
        public static RoutedUICommand SelectedAllRows
        {
            get { return _SelectedAllRows; }
        }
        private static CommandBinding _SelectedAllRowsCommandBinding;
        public static CommandBinding SelectedAllRowsCommandBinding
        {
            get
            {
                if (_SelectedAllRowsCommandBinding == null)
                {
                    _SelectedAllRowsCommandBinding = new CommandBinding(AyTableView.SelectedAllRows, DoSelectedAllRows, SelectedAllRows_CanExecute);
                }
                return _SelectedAllRowsCommandBinding;
            }
        }
        private static AyTableView GetAyTableView(object e)
        {
            AyTableView _adb = null;

            if (e is AyTableView)
            {
                _adb = e as AyTableView;
            }
            else
            {
                _adb = WpfTreeHelper.FindParentControl<AyTableView>(e as DependencyObject);
            }

            return _adb;
        }
        private static void SelectedAllRows_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            AyTableView _adb = GetAyTableView(e.Source);
            if (_adb != null)
            {
                if (_adb.SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    e.CanExecute = true;
                }
                else
                {
                    e.CanExecute = false;
                }
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private static void DoSelectedAllRows(object sender, ExecutedRoutedEventArgs e)
        {
            AyTableView _adb = GetAyTableView(e.Source);
            if (_adb != null)
            {
                _adb.CheckAll();
            }
        }

        public AyTableView()
   : base()
        {
            ResetFixedClipRect();
            this.SetResourceReference(AyTableView.ScrollViewerStyleProperty, "ScrollViewerStyle");

            Columns = new TableViewColumnCollection();
            ColumnsHead = new TableViewColumnCollection();

            //ColSpans = new ColSpansCollection();

            //SnapsToDevicePixels = true;
            //UseLayoutRounding = true;
            //Columns.CollectionChanged += ColumnsChanged;
            //Loaded += AyTableView_Loaded1;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            CommandBindings.Add(AyTableView.SelectedAllRowsCommandBinding);
        }

        /// <summary>
        /// 是否启用Ctrl+V
        /// </summary>
        public bool IsEnabledPaste
        {
            get { return (bool)GetValue(IsEnabledPasteProperty); }
            set { SetValue(IsEnabledPasteProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledPasteProperty =
            DependencyProperty.Register("IsEnabledPaste", typeof(bool), typeof(AyTableView), new PropertyMetadata(false));


        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        internal void SizeChangedInvoke()
        {
            double sumNormalWidth = 0; //先算出 剩下的 需要百分比切割的宽度
            if (PercentWidthSupport)
            {
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        double _gWidth = 0;
                        CreatePercentWidthColumns(col, ref _gWidth);
                        sumNormalWidth += _gWidth;
                    }
                    else
                    {
                        if (col.PercentWidth > 1)
                        {
                            sumNormalWidth += col.PercentWidth;
                        }

                    }
                }
                SYWIDTH33 = sumNormalWidth;
                SYWIDTH = this.ActualWidth - SYWIDTH33;
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        double w = 0;
                        int maxFt = col.ToIndex - col.FromIndex + 1;
                        for (int j = 0; j < maxFt; j++)
                        {
                            //w += col.Columns[j].Width;
                            if (col.Columns[j].PercentWidth > 1)
                            {
                                var _3 = col.Columns[j].PercentWidth;
                                col.Columns[j].Width = _3;
                                w += _3;
                            }
                            else
                            {
                                var _4 = (col.Columns[j].PercentWidth * SYWIDTH);
                                col.Columns[j].Width = _4;
                                w += _4;
                            }
                        }
                        col.Width = w;

                    }
                    else
                    {
                        double w = 0;
                        if (col.PercentWidth > 1)
                        {
                            w += col.PercentWidth;
                        }
                        else
                        {
                            w += (col.PercentWidth * SYWIDTH);
                        }
                        col.Width = w;

                    }
                }


            }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(AyTableView), new PropertyMetadata(false));

        /// <summary>
        /// 是否拖拉过 列头thumb,拖动过，就是true，对百分比布局将不再生效
        /// </summary>
        public bool IsResizeColumnWidth { get; set; }
        /// <summary>
        /// 是否支持百分比宽度
        /// 2018-5-3 15:09:44
        /// AY
        /// </summary>
        public bool PercentWidthSupport
        {
            get { return (bool)GetValue(PercentWidthSupportProperty); }
            set { SetValue(PercentWidthSupportProperty, value); }
        }
        public static readonly DependencyProperty PercentWidthSupportProperty =
            DependencyProperty.Register("PercentWidthSupport", typeof(bool), typeof(AyTableView), new PropertyMetadata(false));


        public ContextMenu RowContextMenu
        {
            get { return (ContextMenu)GetValue(RowContextMenuProperty); }
            set { SetValue(RowContextMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowContextMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowContextMenuProperty =
            DependencyProperty.Register("RowContextMenu", typeof(ContextMenu), typeof(AyTableView), new PropertyMetadata(null));



        /// <summary>
        /// 尾行 2018-5-14 14:10:19 TODO
        /// </summary>
        //public bool EndRowSupport
        //{
        //    get { return (bool)GetValue(EndRowSupportProperty); }
        //    set { SetValue(EndRowSupportProperty, value); }
        //}
        //public static readonly DependencyProperty EndRowSupportProperty =
        //    DependencyProperty.Register("EndRowSupport", typeof(bool), typeof(AyTableView), new PropertyMetadata(false));




        /// <summary>
        /// 表状态，  normal和append状态
        /// </summary>
        public AyTableViewStatuss TableViewStatus
        {
            get { return (AyTableViewStatuss)GetValue(TableViewStatusProperty); }
            set { SetValue(TableViewStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TableViewStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TableViewStatusProperty =
            DependencyProperty.Register("TableViewStatus", typeof(AyTableViewStatuss), typeof(AyTableView), new FrameworkPropertyMetadata(AyTableViewStatuss.Normal));



        public object AddRowObject
        {
            get { return (object)GetValue(AddRowObjectProperty); }
            set { SetValue(AddRowObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AddRowObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AddRowObjectProperty =
            DependencyProperty.Register("AddRowObject", typeof(object), typeof(AyTableView), new PropertyMetadata(null, OnAddRowObjectChanged));

        private static void OnAddRowObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = d as AyTableView;
            if (_1.IsNotNull())
            {

                var newValue = e.NewValue;

                //bringInto最好了 AY 2017-12-14 18:10:47 设计
                //var _sv = _1.RowsPresenter.GetDesendentChild<ScrollViewer>();
                //if (_sv.IsNotNull()) _sv.ScrollToEnd();
                _1.RowsPresenter.RowsPanel.BringRowIntoView(_1.RowsPresenter.Items.Count - 1);
                var el1 = _1.RowsPresenter.RowsPanel.Children[_1.RowsPresenter.RowsPanel.Children.Count - 1] as AyTableViewCellsPresenter;
                if (el1 != null)
                {
                    el1.AyTableViewRowStatus = AyTableViewStatuss.Append;
                    el1.Focus();
                    _1.TableViewStatus = AyTableViewStatuss.Append;
                    AyTime.setTimeout(100, () =>
                    {
                        el1.IsEdit = true;
                    });
                    AyCommon.MemoryGC();
                }
                //_sv.ScrollChanged += (sen,ea) => {
                //    if (_sv.VerticalOffset == 0)
                //    {
                //        //var allCount = _1.RowsPresenter.Items.Count;
                //        var el1 = _1.RowsPresenter.RowsPanel.Children[_1.RowsPresenter.RowsPanel.Children.Count - 1];
                //        var el2 = el1 as AyTableViewCellsPresenter;
                //        if (el2 != null)
                //        {
                //            el2.Focus();
                //            el2.BringIntoView();
                //            el2.OpenEditRow();
                //        }
                //    }
                //};
            }
        }

        #region 附加属性
        #region IsBeginEditRowAction


        public static bool GetIsBeginEditRowAction(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBeginEditRowActionProperty);
        }

        public static void SetIsBeginEditRowAction(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBeginEditRowActionProperty, value);
        }

        public static readonly DependencyProperty IsBeginEditRowActionProperty =
            DependencyProperty.RegisterAttached("IsBeginEditRowAction", typeof(bool), typeof(AyTableView), new PropertyMetadata(false, OnIsBeginEditRowActionChanged));

        private static void OnIsBeginEditRowActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (d as Button);
            if (btn.IsNotNull())
            {
                btn.Click += (btnSender, btnArgs) =>
                {
                    var _1 = (btnSender as Button).GetVisualAncestor<AyTableViewCell>();
                    if (_1.IsNotNull())
                    {
                        if (_1.ParentCellsPresenter.IsNotNull())
                        {
                            _1.ParentCellsPresenter.IsEdit = true;
                        }
                    }
                };
            }
        }
        public static AyTableViewRowModel GetRowDataContext(object cellButton)
        {
            AyTableViewRowModel returnData = null;
            var _1 = (cellButton as Button).GetVisualAncestor<AyTableViewCell>();
            if (_1.IsNotNull())
            {
                if (_1.ParentCellsPresenter.IsNotNull())
                {
                    return _1.ParentCellsPresenter.DataContext as AyTableViewRowModel;
                }
            }
            return returnData;
        }


        #endregion
        #region IsEndEditRowAction


        public static bool GetIsEndEditRowAction(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEndEditRowActionProperty);
        }

        public static void SetIsEndEditRowAction(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEndEditRowActionProperty, value);
        }

        public static readonly DependencyProperty IsEndEditRowActionProperty =
            DependencyProperty.RegisterAttached("IsEndEditRowAction", typeof(bool), typeof(AyTableView), new PropertyMetadata(false, OnIsEndEditRowActionChanged));

        private static void OnIsEndEditRowActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (d as Button);
            if (btn.IsNotNull())
            {
                btn.Click += Btn_Click1;
                //btn.Unloaded += Btn_Unloaded1;
            }
        }
        private static void Btn_Click1(object sender, RoutedEventArgs e)
        {
            var _1 = (sender as Button).GetVisualAncestor<AyTableViewCell>();
            if (_1.IsNotNull() && _1.ParentCellsPresenter.IsNotNull())
            {
                if (_1.ParentCellsPresenter.AyTableViewRowStatus == AyTableViewStatuss.Append)
                {
                    _1.ParentCellsPresenter.ParentTableView.RaiseRowAppendCancelChanged(_1.ParentCellsPresenter.DataContext);
                    _1.ParentCellsPresenter.ParentTableView.TableViewStatus = AyTableViewStatuss.Normal;
                }
                else
                {
                    var _112312 = (_1.ParentCellsPresenter.DataContext as AyTableViewRowModel);
                    var _1231 = (AYUI.Session[_112312.ROWID] as RowOldNewValue).OldValue as AyTableViewRowModel;
                    if (_1231.IsNotNull())
                    {
                        _1231.RowStatus = AyTableViewStatuss.Normal;
                        AyTableViewService.CopyModel(_1.ParentCellsPresenter.DataContext, _1231);
                        _1.ParentCellsPresenter.IsEdit = false;
                        //_1.ParentCellsPresenter.DataContext = _1231;
                        if (AYUI.Session.ContainsKey(_1231.ROWID))
                        {
                            AYUI.Session.Remove(_1231.ROWID);
                        }

                    }


                    _1.ParentCellsPresenter.ParentTableView.RaiseRowEditEnd(_1231);


                }
            }
        }

        //private static void Btn_Unloaded1(object sender, RoutedEventArgs e)
        //{
        //    (sender as Button).Unloaded -= Btn_Unloaded1;
        //    (sender as Button).Click -= Btn_Click1;
        //}




        #endregion

        #region IsEditRow
        public static bool GetIsEditRowButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEditRowButtonProperty);
        }

        public static void SetIsEditRowButton(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEditRowButtonProperty, value);
        }

        public static readonly DependencyProperty IsEditRowButtonProperty =
            DependencyProperty.RegisterAttached("IsEditRowButton", typeof(bool), typeof(AyTableView), new PropertyMetadata(false, OnIsEditRowButtonChanged));

        private static void OnIsEditRowButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (d as Button);
            if (btn.IsNotNull())
            {

                btn.Loaded += Btn_Loaded;
                //btn.Unloaded += Btn_Unloaded;
            }
        }

        private static void Btn_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Button).Loaded -= Btn_Loaded;
            var _1 = (sender as Button).GetVisualAncestor<AyTableViewCell>();
            if (_1.IsNotNull())
            {
                (sender as Button).Click += Btn_Click;
                if (_1.ParentCellsPresenter.IsNotNull())
                {
                    var oldvalue = _1.ParentCellsPresenter.DataContext as AyTableViewRowModel;
                    if (!AYUI.Session.ContainsKey(oldvalue.ROWID))
                    {
                        if (oldvalue.IsNotNull())
                        {
                            RowOldNewValue btnThisSend = new RowOldNewValue();
                            btnThisSend.RowId = oldvalue.ROWID;
                            btnThisSend.OldValue = oldvalue.Clone();
                            btnThisSend.NewValue = oldvalue;
                            btnThisSend.RowStatus = _1.ParentCellsPresenter.AyTableViewRowStatus;

                            AYUI.Session[oldvalue.ROWID] = btnThisSend;
                        }
                    }


                    (sender as Button).IsVisibleChanged += (btnSender, ew) =>
                    {
                        if ((bool)ew.NewValue)
                        {

                            var _12 = (btnSender as Button).GetVisualAncestor<AyTableViewCell>();
                            if (_12.ParentCellsPresenter.IsNotNull())
                            {
                                var oldvalue3 = _12.ParentCellsPresenter.DataContext as AyTableViewRowModel;
                                if (!AYUI.Session.ContainsKey(oldvalue3.ROWID))
                                {
                                    if (oldvalue3.IsNotNull())
                                    {
                                        RowOldNewValue btnThisSend = new RowOldNewValue();
                                        btnThisSend.RowId = oldvalue3.ROWID;
                                        btnThisSend.OldValue = oldvalue3.Clone();
                                        btnThisSend.NewValue = oldvalue3;
                                        btnThisSend.RowStatus = _12.ParentCellsPresenter.AyTableViewRowStatus;

                                        AYUI.Session[oldvalue3.ROWID] = btnThisSend;

                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    };


                }
            }
        }


        private static void Btn_Click(object sender, RoutedEventArgs e)
        {

            var _rowModel = GetRowDataContext(sender);
            if (_rowModel.IsNotNull())
            {
                if (AYUI.Session.ContainsKey(_rowModel.ROWID))
                {
                    var _rownew = AYUI.Session[_rowModel.ROWID] as RowOldNewValue;
                    var _313 = (sender as Button).GetVisualAncestor<AyTableViewCell>();
                    var _cp = _313.ParentCellsPresenter.CellsPanel.Children;
                    if (_cp.IsNotNull())
                    {
                        bool hasWrong = false;
                        foreach (var item in _cp)
                        {
                            var _1112 = item as AyTableViewCell;
                            if (!_1112.IsEditCellNull)
                            {
                                var firstChild = WpfTreeHelper.FindFirstChild<FrameworkElement>(_1112._EditContent);
                                if (firstChild.IsNotNull())
                                {
                                    var _validate = firstChild as IAyValidate;
                                    if (_validate.IsNotNull())
                                    {
                                        bool b = _validate.Validate();
                                        if (!b)
                                        {
                                            hasWrong = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        var _hasTag = firstChild.Tag;
                                        if (_hasTag.IsNotNull())
                                        {
                                            var _validate1 = _hasTag as IAyValidate;
                                            if (_validate1.IsNotNull())
                                            {
                                                bool b = _validate1.Validate();
                                                if (!b)
                                                {
                                                    hasWrong = true;
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (!hasWrong)
                        {
                            _313.ParentCellsPresenter.IsEdit = false;

                            if (_313.ParentCellsPresenter.AyTableViewRowStatus == AyTableViewStatuss.Append)
                            {
                                _313.ParentCellsPresenter.AyTableViewRowStatus = AyTableViewStatuss.Normal;
                                _313.ParentCellsPresenter.ParentTableView.TableViewStatus = AyTableViewStatuss.Normal;
                            }
                            if (_rownew.ValidatePassed.IsNotNull()) _rownew.ValidatePassed();
                            AYUI.Session.Remove(_rownew.RowId);

                        }
                        else
                        {
                            if (_rownew.ValidateFail.IsNotNull()) _rownew.ValidateFail();
                        }
                    }
                }

            }
        }

        //private static void Btn_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    (sender as Button).Unloaded -= Btn_Unloaded;
        //    (sender as Button).Click -= Btn_Click;
        //}


        #endregion


        #endregion
        #region 拓展事件

        /// <summary>
        /// 行复制，Ctrl+C触发
        /// 2019-5-28 
        /// </summary>
        public event EventHandler<AyTableViewRowsEventArgs> OnCopyingRowClipboardContent;
        public void RaiseOnCopyingRowClipboardContent(List<object> datas)
        {
            if (OnCopyingRowClipboardContent.IsNotNull())
            {
                OnCopyingRowClipboardContent(this, new AyTableViewRowsEventArgs(datas));
            }
        }

        public event EventHandler<AyTableViewRowsEventArgs> OnPastingRowClipboardContent;
        public void RaiseOnPastingingRowClipboardContent()
        {
            if (OnPastingRowClipboardContent != null)
            {
                OnPastingRowClipboardContent(this, null);
            }
        }




        public event EventHandler<AyTableViewColumnEventArgs> ColumnWidthChanged;
        public event EventHandler<AyTableViewColumnEventArgs> SortingChanged;
        public void RaiseSortingChanged(AyTableViewColumn col)
        {
            if (SortingChanged.IsNotNull())
            {
                SortingChanged(this, new AyTableViewColumnEventArgs(col));
            }
        }


        /// <summary>
        /// 单选时候，选中改变
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> OnSelectionChanged;
        public void RaiseSelectionChanged(object obj)
        {
            if (OnSelectionChanged.IsNotNull())
            {
                OnSelectionChanged(this, new AyTableViewRowEventArgs(obj));
            }
        }
        /// <summary>
        /// 选中全部
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> OnCheckAll;

        public void RaiseOnCheckAll(object obj)
        {
            if (OnCheckAll.IsNotNull())
            {
                OnCheckAll(this, new AyTableViewRowEventArgs(ItemsSource));
            }
        }
        /// <summary>
        /// 取消选中全部
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> OnUnCheckAll;
        public void RaiseOnUnCheckAll(object obj)
        {
            if (OnUnCheckAll.IsNotNull())
            {
                OnUnCheckAll(this, new AyTableViewRowEventArgs(ItemsSource));
            }
        }
        /// <summary>
        /// 多选时，选择改变
        /// </summary>
        //public event EventHandler<AyTableViewRowEventArgs> OnSelectionsChanged;
        //public void RaiseSelectionsChanged(object obj)
        //{
        //    if (OnSelectionsChanged.IsNotNull())
        //    {
        //        OnSelectionsChanged(this, new AyTableViewRowEventArgs(obj));
        //    }
        //}
        public event EventHandler<AyTableViewRowEventArgs> OnMultipleSelectionAdd;
        public void RaiseOnMultipleSelectionAdd(object obj)
        {
            if (OnMultipleSelectionAdd.IsNotNull())
            {
                OnMultipleSelectionAdd(this, new AyTableViewRowEventArgs(obj));
            }
        }
        public event EventHandler<AyTableViewRowEventArgs> OnMultipleSelectionRemove;
        public void RaiseOnMultipleSelectionRemove(object obj)
        {
            if (OnMultipleSelectionRemove.IsNotNull())
            {
                OnMultipleSelectionRemove(this, new AyTableViewRowEventArgs(obj));
            }
        }
        public event EventHandler<AyTableViewRowEventArgs> RowAppendCancelChanged;
        public void RaiseRowAppendCancelChanged(object row)
        {
            if (RowAppendCancelChanged.IsNotNull())
            {
                RowAppendCancelChanged(this, new AyTableViewRowEventArgs(row));
            }
        }
        #region 行编辑 2017-12-12 13:49:05
        /// <summary>
        /// 行开始编辑时候
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> RowEditBegin;
        public void RaiseRowEditBegin(object data)
        {
            if (RowEditBegin.IsNotNull())
            {
                RowEditBegin(this, new AyTableViewRowEventArgs(data));
            }
        }

        /// <summary>
        /// 行单击模式
        /// </summary>
        public enum RowClickModes
        {
            /// <summary>
            /// 选择，默认值
            /// </summary>
            Select,
            /// <summary>
            /// 单击编辑 ,暂不支持
            /// </summary>
            RowEdit,
            /// <summary>
            /// 单元格编辑,暂不支持
            /// </summary>
            CellEdit,
            /// <summary>
            /// 手动行编辑
            /// </summary>
            MannalRowEdit,

            /// <summary>
            /// 行详情
            /// </summary>
            RowDetail,
        }

        /// <summary>
        /// 行单击模式
        /// </summary>
        public RowClickModes RowClickMode
        {
            get { return (RowClickModes)GetValue(RowClickModeProperty); }
            set { SetValue(RowClickModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowClickMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowClickModeProperty =
            DependencyProperty.Register("RowClickMode", typeof(RowClickModes), typeof(AyTableView), new PropertyMetadata(RowClickModes.Select));


        /// <summary>
        /// 行编辑完
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> RowEditEnd;
        public void RaiseRowEditEnd(object data)
        {
            if (RowEditEnd.IsNotNull())
            {
                RowEditEnd(this, new AyTableViewRowEventArgs(data));
            }
        }

        #endregion
        #region 单元格编辑 2017-12-12 18:32:47
        /// <summary>
        /// 行开始编辑时候
        /// </summary>
        public event EventHandler<AyTableViewCellEventArgs> CellEditBegin;
        public void RaiseCellEditBegin(object data, string field)
        {
            if (CellEditBegin.IsNotNull())
            {
                CellEditBegin(this, new AyTableViewCellEventArgs(data, field));
            }
        }

        /// <summary>
        /// 单元编辑完
        /// </summary>
        public event EventHandler<AyTableViewCellEventArgs> CellEditEnd;
        public void RaiseCellEditEnd(object data, string field)
        {
            if (CellEditEnd.IsNotNull())
            {
                CellEditEnd(this, new AyTableViewCellEventArgs(data, field));
            }
        }

        #endregion
        /// <summary>
        /// 行双击事件
        /// </summary>
        public event EventHandler<AyTableViewRowEventArgs> DoubleClickRow;
        #endregion
        #region 废物
        //private void AyTableView_Loaded1(object sender, RoutedEventArgs e)
        //{
        //    Loaded -= AyTableView_Loaded1;
        //    //AyTime.setTimeout(2000, () =>
        //    //{

        //    return;

        //    //var _1= Application.Current.Resources["columnHeaderStyle"] as Style; 
        //    //foreach (var item in ColSpans)
        //    //{
        //    //    var _t = HeaderRowPresenter.Items;

        //    //    AyTableViewColumn hg = new AyTableViewColumn();
        //    //    hg.GroupName = item.Title;
        //    //    hg.FromIndex = item.FromIndex;
        //    //    hg.ToIndex = item.ToIndex;
        //    //    hg.IsGroup = true;
        //    //    //hg.ColumnHeight = d;
        //    //    var sp = new StackPanel { Orientation = Orientation.Horizontal };

        //    //    var _34 = HeaderRowPresenter.ItemsSource.Cast<AyTableViewColumn>();
        //    //    int _count = _34.Count();
        //    //    int _35 = item.ToIndex - item.FromIndex + 1;
        //    //    int _36 = 0;
        //    //    double allwidth = 0;

        //    //    for (int i = item.FromIndex; i <= item.FromIndex; i++)
        //    //    {
        //    //        var _123 = HeaderRowPresenter.ItemContainerGenerator.ContainerFromItem(_t[item.FromIndex]) as AyTableViewColumnHeader;
        //    //        allwidth += _123.Width;
        //    //        //using (MemoryStream stream = new MemoryStream())
        //    //        //{
        //    //        //    XamlWriter.Save(_123, stream);
        //    //        //    stream.Seek(0, SeekOrigin.Begin);
        //    //        //    AyTableViewColumnHeader ach = (AyTableViewColumnHeader)XamlReader.Load(stream);
        //    //        //    ach.Style = _1;
        //    //        //    sp.Children.Add(ach);
        //    //        //}
        //    //        sp.Children.Add(_123);
        //    //        IList _list = HeaderRowPresenter.ItemsSource as IList;
        //    //        _list.RemoveAt(i);
        //    //        _36++;
        //    //        i--;
        //    //        if (_36 == _35)
        //    //        {
        //    //            break;
        //    //        }
        //    //    }

        //    //    hg.Items = sp;
        //    //    hg.Width = allwidth;
        //    //    Columns.Insert(item.FromIndex, hg);


        //    //}
        //    //}
        //    //});
        //}


        //private int _ColumnsCount = 0;

        //public int ColumnsCount
        //{
        //    get
        //    {
        //        if (_ColumnsCount == 0)
        //        {
        //            foreach (var col in Columns)
        //            {
        //                if (col.IsGroup && col.Columns.Count > 0)
        //                {
        //                    TotalColumns(col);
        //                }
        //                else
        //                {
        //                    _ColumnsCount++;
        //                }
        //            }
        //        }
        //        return _ColumnsCount;
        //    }
        //}
        //public void TotalColumns(AyTableViewColumn col1)
        //{
        //    foreach (var col in col1.Columns)
        //    {
        //        if (col.IsGroup && col.Columns.Count > 0)
        //        {
        //            TotalColumns(col);
        //        }
        //        else
        //        {
        //            _ColumnsCount++;
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 根据用户的列创建自己的最后一排列
        /// </summary>
        /// <param name="col1"></param>
        public void CreateColumns(AyTableViewColumn col1, ref double gWidth)
        {
            foreach (var col in col1.Columns)
            {
                gWidth += col.Width;
                if (col.Columns.Count > 0)
                {
                    double gwid = 0;
                    col.IsGroup = true;
                    CreateColumns(col, ref gwid);
                }
                else
                {
                    Columns.Add(col);
                }
            }
        }
        public void ResetSelectedItemsReset()
        {
            if (HasCheckBoxColumn && cbCheckAll != null)
            {
                if (SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    cbCheckAll.Visibility = Visibility.Visible;
                }
                else
                {
                    cbCheckAll.Visibility = Visibility.Collapsed;
                }
            }


            if (SelectedItem.IsNotNull())
            {
                var _1 = SelectedItem as AyUIEntity;
                _1.Selected = false;
            }
            if (SelectionMode == AyTableViewSelectionMode.Cell)
            {
                CellNavigation = true;
            }
            else
            {
                CellNavigation = false;
            }
            //SelectedCell = null;
            SelectedRowIndex = -1;
            FocusedRowIndex = -1;
            SelectedCellsPresenter = null;
            SelectedItem = null;
            if (SelectedItems.IsNotNull()) SelectedItems.Clear();
            DoubleSelectedItem = null;
        }
        public void ResetSelectedItems2()
        {
            if (HasCheckBoxColumn && cbCheckAll != null)
            {
                if (SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    cbCheckAll.Visibility = Visibility.Visible;
                }
                else
                {
                    cbCheckAll.Visibility = Visibility.Collapsed;
                }
            }


            if (SelectedItem.IsNotNull())
            {
                var _1 = SelectedItem as AyUIEntity;
                _1.Selected = false;
            }
            if (SelectedItems.IsNotNull())
            {
                var _1 = SelectedItems as List<object>;
                int _cint = _1.Count();
                for (int i = 0; i < _cint; i++)
                {
                    var _2 = _1[0] as AyUIEntity;
                    if (_2 != null)
                        _2.Selected = false;
                }
            }
            if (SelectionMode == AyTableViewSelectionMode.Cell)
            {
                CellNavigation = true;
            }
            else
            {
                CellNavigation = false;
            }
            //SelectedCell = null;
            SelectedRowIndex = -1;
            FocusedRowIndex = -1;
            SelectedCellsPresenter = null;
            SelectedItem = null;
            if (SelectedItems.IsNotNull()) SelectedItems.Clear();
            DoubleSelectedItem = null;
        }
        /// <summary>
        /// 重置选择
        /// </summary>
        public void ResetSelectedItems()
        {
            if (WpfTreeHelper.IsInDesignMode) return;
            if (HasCheckBoxColumn && cbCheckAll != null)
            {
                if (SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    cbCheckAll.Visibility = Visibility.Visible;
                }
                else
                {
                    cbCheckAll.Visibility = Visibility.Collapsed;
                }
            }


            if (SelectedItem.IsNotNull())
            {
                var _1 = SelectedItem as AyUIEntity;
                _1.Selected = false;
            }
            if (SelectedItems.IsNotNull())
            {
                var _1 = SelectedItems as List<object>;
                foreach (var item in _1)
                {
                    var _2 = item as AyUIEntity;
                    if (_2 != null)
                        _2.Selected = false;
                }
            }
            if (SelectionMode == AyTableViewSelectionMode.Cell)
            {
                CellNavigation = true;
            }
            else
            {
                CellNavigation = false;
            }
            //SelectedCell = null;
            SelectedRowIndex = -1;
            FocusedRowIndex = -1;
            SelectedCellsPresenter = null;
            SelectedItem = null;
            if (SelectedItems.IsNotNull()) SelectedItems.Clear();
            DoubleSelectedItem = null;
        }

        internal AyTableViewHeaderPresenter HeaderRowPresenter { get; set; }
        internal AyTableViewHeaderPresenter HeaderRowPresenter2 { get; set; }
        public AyTableViewRowsPresenter RowsPresenter { get; set; }
        internal AyTableViewCellsPresenter SelectedCellsPresenter { get; set; }
        internal AyTableViewCell SelectedCell { get; set; }

        private Rect _fixedClipRect = Rect.Empty;
        internal Rect FixedClipRect
        {
            get
            {
                if (_fixedClipRect == Rect.Empty)
                {
                    double width = 0.0;
                    if (Columns.Count >= FixedColumnCount)
                        for (int i = 0; i < FixedColumnCount; ++i)
                            width += Columns[i].Width;

                    _fixedClipRect = new Rect(HorizontalScrollOffset, 0, width, 0);
                }
                return _fixedClipRect;
            }
        }

        private Rect _fixedClipRect2 = Rect.Empty;
        internal Rect FixedClipRect2
        {
            get
            {
                if (_fixedClipRect2 == Rect.Empty)
                {
                    double width = 0.0;
                    if (Columns.Count >= FixedColumnCount)
                        for (int i = 0; i < FixedColumnCount; ++i)
                            width += Columns[i].Width;

                    _fixedClipRect2 = new Rect(HorizontalScrollOffset2, 0, width, 0);
                }
                return _fixedClipRect2;
            }
        }
        internal void ResetFixedClipRect2()
        {
            _fixedClipRect2 = Rect.Empty;
        }

        internal void ResetFixedClipRect()
        {
            _fixedClipRect = Rect.Empty;
        }

        //public double HeaderHeight { get { return HeaderRowPresenter == null ? 0 : HeaderRowPresenter.Height; } }

        public ItemCollection Items
        {
            get
            {
                if (this.RowsPresenter != null)
                    return this.RowsPresenter.Items;
                return null;
            }
        }
        /// <summary>
        /// 最后1次单击的排序列
        /// </summary>
        public AyTableViewColumnHeader ColumnLastClickOrderBy { get; set; }
        /// <summary>
        /// 排序条件   field asc, 或者 field desc
        /// </summary>
        public string OrderCondition
        {
            get { return (string)GetValue(OrderConditionProperty); }
            set { SetValue(OrderConditionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderCondition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderConditionProperty =
            DependencyProperty.Register("OrderCondition", typeof(string), typeof(AyTableView), new PropertyMetadata(null));




        public Style ScrollViewerStyle
        {
            get { return (Style)GetValue(ScrollViewerStyleProperty); }
            set { SetValue(ScrollViewerStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerStyleProperty =
            DependencyProperty.Register("ScrollViewerStyle", typeof(Style), typeof(AyTableView), new PropertyMetadata(null));



        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = IsEnabledPaste;

        }
        /// <summary>
        /// 粘贴执行，这里只捕获，自己从剪切板处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RaiseOnPastingingRowClipboardContent();
        }
        private void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectionMode == AyTableViewSelectionMode.RowTenSingle || SelectionMode == AyTableViewSelectionMode.Single)
            {
                e.CanExecute = SelectedItem != null;
                return;
            }
            else if (SelectionMode == AyTableViewSelectionMode.Multiple)
            {
                e.CanExecute = SelectedItems != null && SelectedItems.Count > 0;
                return;
            }
            e.CanExecute = false;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectionMode == AyTableViewSelectionMode.RowTenSingle || SelectionMode == AyTableViewSelectionMode.Single)
            {
                RaiseOnCopyingRowClipboardContent(new List<object> { SelectedItem });
            }
            else if (SelectionMode == AyTableViewSelectionMode.Multiple)
            {
                var _1 = new List<object>();
                foreach (var item in SelectedItems)
                {
                    _1.Add(item);
                }
                RaiseOnCopyingRowClipboardContent(_1);
            }
        }

        public void CreatePercentWidthColumns(AyTableViewColumn col1, ref double gWidth)
        {
            foreach (var col in col1.Columns)
            {
                if (col.PercentWidth > 1)
                {
                    gWidth += col.PercentWidth;
                }

                if (col.Columns.Count > 0)
                {
                    double gwid = 0;
                    CreatePercentWidthColumns(col, ref gwid);
                }
                else
                {

                }
            }
        }


        public ScrollViewer ContentScollViewer
        {
            get { return (ScrollViewer)GetValue(ContentScollViewerProperty); }
            set { SetValue(ContentScollViewerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentScollViewer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentScollViewerProperty =
            DependencyProperty.Register("ContentScollViewer", typeof(ScrollViewer), typeof(AyTableView), new PropertyMetadata(null));


        /// <summary>
        /// 根据用户的列创建自己的最后一排列
        /// </summary>
        /// <param name="col1"></param>
        public void CreateColumnsPercent(AyTableViewColumn col1, ref double gWidth)
        {
            foreach (var col in col1.Columns)
            {
                if (col.PercentWidth > 1)
                {
                    gWidth += col.PercentWidth;
                }
                gWidth += col.Width;
                if (col.Columns.Count > 0)
                {
                    double gwid = 0;
                    col.IsGroup = true;
                    CreateColumnsPercent(col, ref gwid);
                }
                else
                {
                    Columns.Add(col);
                }
            }
        }

        /// <summary>
        /// 剩余的宽度
        /// </summary>
        public double SYWIDTH { get; set; }
        /// <summary>
        /// 数字宽度占用的总宽度,由于未初始化完,整个aytableview是拿不到值
        /// </summary>
        public double SYWIDTH33 { get; set; }
        internal void RefreshColumn()
        {
            if (Columns == null)
            {
                return;
            }
            Columns.Clear();
            HeaderHeight = HeadRowCount * HeaderHeight;
            int headIndex = 0;
            int _from = 0;
            double sumNormalWidth = 0; //先算出 剩下的 需要百分比切割的宽度
            if (PercentWidthSupport)
            {
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        double _gWidth = 0;
                        CreatePercentWidthColumns(col, ref _gWidth);
                        sumNormalWidth += _gWidth;
                    }
                    else
                    {
                        if (col.PercentWidth > 1)
                        {
                            sumNormalWidth += col.PercentWidth;
                        }

                    }
                }
                SYWIDTH33 = sumNormalWidth;

                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        col.IsGroup = true;
                        col.FromIndex = _from;
                        col.ToIndex = col.FromIndex + col.Columns.Count - 1;
                        _from = col.ToIndex;
                        _from++;
                        double _gWidth = 0;
                        CreateColumns(col, ref _gWidth);
                        col.Width = _gWidth;

                        headIndex++;

                    }
                    else
                    {
                        col.ColumnsHeadIndex = headIndex;
                        headIndex++;
                        _from++;
                        Columns.Add(col);
                    }
                }
            }
            else
            {
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        col.IsGroup = true;
                        col.FromIndex = _from;
                        col.ToIndex = col.FromIndex + col.Columns.Count - 1;
                        _from = col.ToIndex;
                        _from++;
                        double _gWidth = 0;
                        CreateColumns(col, ref _gWidth);
                        col.Width = _gWidth;
                        headIndex++;

                    }
                    else
                    {
                        col.ColumnsHeadIndex = headIndex;
                        headIndex++;
                        _from++;
                        Columns.Add(col);
                    }
                }

            }

        }
        public void RefreshColumnInfo()
        {
            if (Columns == null)
            {
                return;
            }
            Columns.Clear();
            int headIndex = 0;
            int _from = 0;
            double sumNormalWidth = 0; //先算出 剩下的 需要百分比切割的宽度
            if (PercentWidthSupport)
            {
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        double _gWidth = 0;
                        CreatePercentWidthColumns(col, ref _gWidth);
                        sumNormalWidth += _gWidth;
                    }
                    else
                    {
                        if (col.PercentWidth > 1)
                        {
                            sumNormalWidth += col.PercentWidth;
                        }

                    }
                }
                SYWIDTH33 = sumNormalWidth;

                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        col.IsGroup = true;
                        col.FromIndex = _from;
                        col.ToIndex = col.FromIndex + col.Columns.Count - 1;
                        _from = col.ToIndex;
                        _from++;
                        double _gWidth = 0;
                        CreateColumns(col, ref _gWidth);
                        col.Width = _gWidth;

                        headIndex++;

                    }
                    else
                    {
                        col.ColumnsHeadIndex = headIndex;
                        headIndex++;
                        _from++;
                        Columns.Add(col);
                    }
                }
            }
            else
            {
                foreach (var col in ColumnsHead)
                {
                    if (col.Columns.Count > 0)
                    {
                        col.IsGroup = true;
                        col.FromIndex = _from;
                        col.ToIndex = col.FromIndex + col.Columns.Count - 1;
                        _from = col.ToIndex;
                        _from++;
                        double _gWidth = 0;
                        CreateColumns(col, ref _gWidth);
                        col.Width = _gWidth;
                        headIndex++;

                    }
                    else
                    {
                        col.ColumnsHeadIndex = headIndex;
                        headIndex++;
                        _from++;
                        Columns.Add(col);
                    }
                }

            }

        }
        public DataTemplate RowDetailTemplate
        {
            get { return (DataTemplate)GetValue(RowDetailTemplateProperty); }
            set { SetValue(RowDetailTemplateProperty, value); }
        }

        public static readonly DependencyProperty RowDetailTemplateProperty =
            DependencyProperty.Register("RowDetailTemplate", typeof(DataTemplate), typeof(AyTableView), new PropertyMetadata(null));


        CheckBox cbCheckAll = null;
        public bool CantNavigation()
        {
            return AyTableViewSelectionMode.NoSelect == SelectionMode || AyTableViewSelectionMode.Multiple == SelectionMode || AyTableViewSelectionMode.RowTenNoSelect == SelectionMode;
        }
        bool isfirstAddColumn = true;
        bool isfirstAddCheckColumn = true;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HeaderRowPresenter = GetTemplateChild("PART_HeaderPresenter") as AyTableViewHeaderPresenter;

            HeaderRowPresenter2 = GetTemplateChild("PART_HeaderPresenter2") as AyTableViewHeaderPresenter;

            RowsPresenter = GetTemplateChild("PART_RowsPresenter") as AyTableViewRowsPresenter;
            int ed = 0;
            if (HasIndexColumn && isfirstAddColumn)
            {
                double bw = 0;
                if (IndexColumnWidth.HasValue)
                {
                    bw = IndexColumnWidth.Value;
                }
                else
                {
                    if (this.ItemsCount > 1000)
                    {
                        bw = 38;
                    }
                    else if (this.ItemsCount > 10000)
                    {
                        bw = 48;
                    }
                    else if (this.ItemsCount > 100000)
                    {
                        bw = 58;
                    }
                    else if (this.ItemsCount > 1000000)
                    {
                        bw = 68;
                    }
                    else if (this.ItemsCount > 10000000)
                    {
                        bw = 78;
                    }
                    else if (this.ItemsCount > 100000000)
                    {
                        bw = 88;
                    }
                }

                var _vc = new AyTableViewColumn() { Title = "", MinResizeColumnWidth = 20, Width = bw, Field = "AYID" };
                _columnsHead.Insert(ed, _vc);
                if (PercentWidthSupport)
                {
                    _vc.PercentWidth = bw;
                }
                isfirstAddColumn = false;
                ed++;
            }
            if (HasCheckBoxColumn && isfirstAddCheckColumn)
            {
                cbCheckAll = new CheckBox();
                cbCheckAll.Padding = new Thickness(0);
                cbCheckAll.Checked += CbCheckAll_Checked;
                cbCheckAll.Unchecked += CbCheckAll_Unchecked;
                cbCheckAll.SetResourceReference(CheckBox.StyleProperty, "AyTableViewCheckBox");
                if (SelectionMode == AyTableViewSelectionMode.Multiple)
                {
                    cbCheckAll.Visibility = Visibility.Visible;
                }
                else
                {
                    cbCheckAll.Visibility = Visibility.Collapsed;
                }
                DataTemplate dt = new DataTemplate();
                FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox));
                Binding bd = new Binding("IsSelected");
                bd.Mode = BindingMode.TwoWay;
                bd.RelativeSource = new RelativeSource { Mode = RelativeSourceMode.FindAncestor, AncestorType = typeof(AyTableViewCellsPresenter) };
                factory.SetBinding(CheckBox.IsCheckedProperty, bd);

                factory.SetValue(CheckBox.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                factory.SetValue(CheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);
                factory.SetValue(CheckBox.PaddingProperty, new Thickness(0));
                factory.SetResourceReference(CheckBox.StyleProperty, "AyTableViewCheckBox");
                dt.VisualTree = factory;
                //dt.Seal();
                //=============
                //2022年3月18日15:23:07修改 原来的会导致程序报一些警告
                var _vc = new AyTableViewColumn()
                {
                    Title = cbCheckAll,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Width = 40,
                    ResizeColumn = false,
                    CellTemplate = dt,
                    Field = "",
                    Tag1="AYCHECK"
                };
                //修改
                _columnsHead.Insert(ed, _vc);

                if (PercentWidthSupport)
                {
                    _vc.PercentWidth = 40;
                }
                isfirstAddCheckColumn = false;
            }
            SelectedItems = new List<object>();

            RefreshColumn();

        }



        public void UnCheckAll()
        {
            SelectedItems.Clear();
            foreach (var item in ItemsSource)
            {
                var _2 = item as AyUIEntity;
                if (_2.IsNotNull())
                {
                    _2.Selected = false;

                }
            }
        }
        private void CbCheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            UnCheckAll();
            RaiseOnUnCheckAll(ItemsSource);
        }

        private void CbCheckAll_Checked(object sender, RoutedEventArgs e)
        {
            CheckAll();
        }

        public void CheckAll()
        {
            SelectedItems.Clear();
            foreach (var item in ItemsSource)
            {
                var _2 = item as AyUIEntity;
                if (_2.IsNotNull())
                {
                    _2.Selected = true;

                    SelectedItems.Add(item);
                }
            }

            RaiseOnCheckAll(ItemsSource);
        }

        internal void NotifyColumnWidthChanged(AyTableViewColumn column)
        {
            if (ColumnWidthChanged != null)
                ColumnWidthChanged(this, new AyTableViewColumnEventArgs(column));
        }

        //internal void NotifySortingChanged(AyTableViewColumn column)
        //{
        //    if (SortingChanged != null)
        //        SortingChanged(this, new AyTableViewColumnEventArgs(column));
        //}
        internal void NotifyDoubleClickRow(object data)
        {
            if (DoubleClickRow != null)
            {
                DoubleSelectedItem = data;
                DoubleClickRow(this, new AyTableViewRowEventArgs(data));

            }

        }
        public void SetParentTableView(AyTableViewColumn col1)
        {
            foreach (var col in col1.Columns)
            {
                col.ParentTableView = this;
                if (col.IsGroup && col.Columns.Count > 0)
                {
                    SetParentTableView(col);
                }
            }
        }

        private void ColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            WhenColumnsChanged();
        }

        public void WhenColumnsChanged()
        {
            if (isUpdateColumns)
            {
                foreach (var col in Columns)
                {
                    col.ParentTableView = this;
                    if (col.IsGroup && col.Columns.Count > 0)
                    {
                        SetParentTableView(col);
                    }
                }
                if (HeaderRowPresenter != null)
                {
                    ResetFixedClipRect();
                    HeaderRowPresenter.HeaderInvalidateArrange();
                    if (HeaderRowPresenter2 != null)
                        HeaderRowPresenter2.HeaderInvalidateArrange();
                }

                if (RowsPresenter != null)
                    RowsPresenter.ColumnsChanged();
            }
        }


        public int IndexOfRow(AyTableViewCellsPresenter cp)
        {
            if (RowsPresenter != null)
                return RowsPresenter.ItemContainerGenerator.IndexFromContainer(cp);
            return -1;
        }
        //public AyTableViewCellsPresenter RowOfDataItem(object cp)
        //{
        //    if (RowsPresenter != null)
        //        return RowsPresenter.ItemContainerGenerator.ContainerFromItem(cp) as AyTableViewCellsPresenter;
        //    return null;
        //}

        internal void FocusedRowChanged(AyTableViewCellsPresenter cp)
        {
            if (cp == null) return;
            FocusedRowIndex = IndexOfRow(cp);
            SelectedRowIndex = FocusedRowIndex;
        }

        internal void FocusedColumnChanged(AyTableViewColumn col)
        {
            FocusedColumnIndex = Columns.IndexOf(col);
            SelectedColumnIndex = FocusedColumnIndex;
        }
        Brush traBrush = new SolidColorBrush(Colors.Transparent);

        internal void LeaveColumnChanged(AyTableViewColumn col)
        {
            if (this.SelectionMode == AyTableViewSelectionMode.RowTenSingle || this.SelectionMode == AyTableViewSelectionMode.RowTenNoSelect)
            {
                col.ColumnFocusBrush = traBrush;
            }
        }

        internal void EnterColumnChanged(AyTableViewColumn col)
        {
            if (this.SelectionMode == AyTableViewSelectionMode.RowTenSingle || this.SelectionMode == AyTableViewSelectionMode.RowTenNoSelect)
            {
                col.ColumnFocusBrush = RowTenHoverBrush;
            }
        }



        //public AyTableViewColumnHeader GetColumnHeaderAtLocation(Point loc)
        //{
        //    if (HeaderRowPresenter != null)
        //        return HeaderRowPresenter.GetColumnHeaderAtLocation(loc);
        //    return null;
        //}

        //public AyTableViewColumn GetColumnAtLocation(Point loc)
        //{
        //    var ch = GetColumnHeaderAtLocation(loc);
        //    if (ch != null)
        //        return Columns[ch.ColumnIndex];

        //    return null;
        //}

        //public object GetItemAtLocation(Point loc)
        //{
        //    loc.Y -= HeaderRowPresenter.RenderSize.Height;
        //    if (RowsPresenter != null)
        //        return RowsPresenter.GetItemAtLocation(loc);
        //    return null;
        //}

        //public int GetCellIndexAtLocation(Point loc)
        //{
        //    loc.Y -= HeaderRowPresenter.RenderSize.Height;
        //    if (RowsPresenter != null)
        //        return RowsPresenter.GetCellIndexAtLocation(loc);
        //    return -1;
        //}





        //--------------
        //public static readonly DependencyProperty ColumnsProperty =
        //    DependencyProperty.Register("xColumns", typeof(ObservableCollection<TableViewColumn>), typeof(TableView), new PropertyMetadata(null, new PropertyChangedCallback(TableView.OnColumnsPropertyChanged)));

        //private static void OnColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //  var tv = d as TableView;
        //  if (e.OldValue != null)
        //    ((ObservableCollection<TableViewColumn>)e.OldValue).CollectionChanged -= tv.ColumnsChanged;
        //  if( e.NewValue != null)
        //    ((ObservableCollection<TableViewColumn>)e.NewValue).CollectionChanged += tv.ColumnsChanged;
        //}
        //private ColSpansCollection _ColSpans;

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public ColSpansCollection ColSpans
        //{
        //    get { return _ColSpans; }
        //    set
        //    {
        //        _ColSpans = value;
        //    }
        //}

        private TableViewColumnCollection _columns;
        public bool isUpdateColumns = true;
        public TableViewColumnCollection Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != null)
                    _columns.CollectionChanged -= ColumnsChanged;
                _columns = value;
                if (_columns != null)
                    _columns.CollectionChanged += ColumnsChanged;

            }
        }
        public void ColumnsAddRange(List<AyTableViewColumn> columns)
        {
            if (columns == null) return;
            isUpdateColumns = false;
            _columns.CollectionChanged -= ColumnsChanged;
            foreach (var item in columns)
            {
                item.ParentTableView = this;
                if (item.IsGroup && item.Columns.Count > 0)
                {
                    SetParentTableView(item);
                }
                Columns.Add(item);
            }
            _columns.CollectionChanged += ColumnsChanged;
            isUpdateColumns = true;
            WhenColumnsChanged();
        }


        private TableViewColumnCollection _columnsHead;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableViewColumnCollection ColumnsHead
        {
            get { return _columnsHead; }
            set
            {
                _columnsHead = value;
            }
        }


        private void WhenSetRectangle_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= WhenSetRectangle_Loaded;
            //if (HasRectangleSelection.HasValue && HasRectangleSelection.Value)
            //{
            AyTableViewSelector.SetEnabled(this, HasRectangleSelection.Value);
            //}


        }


        #region 依赖属性 普通
        /// <summary>
        /// 双击时候用于绑定的选中对象
        /// AY
        /// 2017-12-4 16:59:25
        /// </summary>
        public object DoubleSelectedItem
        {
            get { return (object)GetValue(DoubleSelectedItemProperty); }
            set { SetValue(DoubleSelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DoubleSelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleSelectedItemProperty =
            DependencyProperty.Register("DoubleSelectedItem", typeof(object), typeof(AyTableView), new PropertyMetadata(null));

        private void Notify2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HeaderRowPresenter2 != null)
            {
                ResetFixedClipRect2();
                HeaderRowPresenter2.HeaderInvalidateArrange();
            }
        }

        private void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HeaderRowPresenter != null)
            {
                ResetFixedClipRect();
                HeaderRowPresenter.HeaderInvalidateArrange();
            }

            if (RowsPresenter != null)
                RowsPresenter.RowsInvalidateArrange();
        }


        /// <summary>
        /// 是否支持各行换色
        /// </summary>
        public bool AlterLineBrushSupport
        {
            get { return (bool)GetValue(AlterLineBrushSupportProperty); }
            set { SetValue(AlterLineBrushSupportProperty, value); }
        }

        public static readonly DependencyProperty AlterLineBrushSupportProperty =
            DependencyProperty.Register("AlterLineBrushSupport", typeof(bool), typeof(AyTableView), new FrameworkPropertyMetadata(true));

        /// <summary>
        /// 当前选中的对象
        /// </summary>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(AyTableView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// 当前选中的多个
        /// </summary>
        [Bindable(true), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(AyTableView), new FrameworkPropertyMetadata(null));


        public Brush SelectedRowForeground
        {
            get { return (Brush)GetValue(SelectedRowForegroundProperty); }
            set { SetValue(SelectedRowForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedRowForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedRowForegroundProperty =
            DependencyProperty.Register("SelectedRowForeground", typeof(Brush), typeof(AyTableView), new PropertyMetadata(new SolidColorBrush(Colors.White)));


        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register("HeaderHeight", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(32.00));

        //------------------------
        public static readonly DependencyProperty CellNavigationProperty =
            DependencyProperty.Register("CellNavigation", typeof(bool), typeof(AyTableView), new PropertyMetadata(true));

        public bool CellNavigation
        {
            get { return (bool)GetValue(CellNavigationProperty); }
            set { SetValue(CellNavigationProperty, value); }
        }

        //------------------------
        public static readonly DependencyProperty ShowVerticalGridLinesProperty =
            DependencyProperty.Register("ShowVerticalGridLines", typeof(bool), typeof(AyTableView), new PropertyMetadata(true));

        public bool ShowVerticalGridLines
        {
            get { return (bool)GetValue(ShowVerticalGridLinesProperty); }
            set { SetValue(ShowVerticalGridLinesProperty, value); }
        }
        //------------------------
        public static readonly DependencyProperty ShowHorizontalGridLinesProperty =
            DependencyProperty.Register("ShowHorizontalGridLines", typeof(bool), typeof(AyTableView), new PropertyMetadata(false));

        public bool ShowHorizontalGridLines
        {
            get { return (bool)GetValue(ShowHorizontalGridLinesProperty); }
            set { SetValue(ShowHorizontalGridLinesProperty, value); }
        }

        //---------------
        public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AyTableView), new FrameworkPropertyMetadata(null, OnItemsSourcePropertyChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tv = d as AyTableView;
            if (tv != null)
            {
                var _1 = e.NewValue as IEnumerable;
                if (_1 == null) return;
                if (tv.RowsPresenter != null)
                {
                    tv.RowsPresenter.ItemsSource = _1;
                }
                bool hasValue = false;
                foreach (var item in _1)
                {
                    hasValue = true;
                    break;
                }
                if (hasValue && tv.HasIndexColumn)
                {
                    int iew = 1;

                    foreach (var item in _1)
                    {
                        var _2 = item as AyPropertyChanged;
                        if (_2.IsNotNull()) _2.AYID = (iew++).ToString();
                    }

                    iew = 1;
                    var _3 = e.NewValue as INotifyCollectionChanged;
                    if (_3.IsNotNull())
                    {
                        _3.CollectionChanged += (sen, ee) =>
                        {
                            if (tv.HasIndexColumn)
                            {

                                foreach (var item in _1)
                                {
                                    var _2 = item as AyPropertyChanged;
                                    if (_2.IsNotNull()) _2.AYID = (iew++).ToString();
                                }
                                iew = 1;
                            }
                            if (ee.Action == NotifyCollectionChangedAction.Remove)
                            {
                                tv.ResetSelectedItems2();
                                return;
                            }
                            if (ee.Action == NotifyCollectionChangedAction.Reset)
                            {
                                tv.ResetSelectedItemsReset();
                                return;
                            }


                        };
                    }
                }



                tv.ResetSelectedItems();
            }
        }


        //--------------
        public static readonly DependencyProperty SelectedRowIndexProperty =
            DependencyProperty.Register("SelectedRowIndex", typeof(object), typeof(AyTableView), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedRowIndex
        {
            get { return (int)GetValue(SelectedRowIndexProperty); }
            set { SetValue(SelectedRowIndexProperty, value); }
        }

        //--------------
        public static readonly DependencyProperty SelectedColumnIndexProperty =
            DependencyProperty.Register("SelectedColumnIndex", typeof(int), typeof(AyTableView), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedColumnIndex
        {
            get { return (int)GetValue(SelectedColumnIndexProperty); }
            set { SetValue(SelectedColumnIndexProperty, value); }
        }

        //--------------
        public static readonly DependencyProperty FocusedRowIndexProperty =
            DependencyProperty.Register("FocusedRowIndex", typeof(object), typeof(AyTableView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int FocusedRowIndex
        {
            get { return (int)GetValue(FocusedRowIndexProperty); }
            set { SetValue(FocusedRowIndexProperty, value); }
        }

        public static readonly DependencyProperty FocusedColumnIndexProperty =
            DependencyProperty.Register("FocusedColumnIndex", typeof(int), typeof(AyTableView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int FocusedColumnIndex
        {
            get { return (int)GetValue(FocusedColumnIndexProperty); }
            set { SetValue(FocusedColumnIndexProperty, value); }
        }

        public static readonly DependencyProperty FixedColumnCountProperty =
            DependencyProperty.Register("FixedColumnCount", typeof(int), typeof(AyTableView), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(AyTableView.OnFixedColumnsCountPropertyChanged)));

        private static void OnFixedColumnsCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).NotifyPropertyChanged(d, e);
        }

        public int FixedColumnCount
        {
            get { return (int)GetValue(FixedColumnCountProperty); }
            set { SetValue(FixedColumnCountProperty, value); }
        }



        public bool IsFreshColumns
        {
            get { return (bool)GetValue(IsFreshColumnsProperty); }
            set { SetValue(IsFreshColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFreshColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFreshColumnsProperty =
            DependencyProperty.Register("IsFreshColumns", typeof(bool), typeof(AyTableView), new PropertyMetadata(false, OnIsFreshColumnsChanged));

        private static void OnIsFreshColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyTableView).NotifyIsFreshColumns();
        }

        private void NotifyIsFreshColumns()
        {
            RefreshColumn();
        }



        /// <summary>
        /// 列头行数，默认1
        /// </summary>
        public int HeadRowCount
        {
            get { return (int)GetValue(HeadRowCountProperty); }
            set { SetValue(HeadRowCountProperty, value); }
        }

        public static readonly DependencyProperty HeadRowCountProperty =
            DependencyProperty.Register("HeadRowCount", typeof(int), typeof(AyTableView), new PropertyMetadata(1));

        //--------------
        public static readonly DependencyProperty HorizontalScrollOffsetProperty =
          DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(AyTableView.OnHorizontalOffsetPropertyChanged)));

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).NotifyPropertyChanged(d, e);
        }

        public double HorizontalScrollOffset
        {
            get { return (double)GetValue(HorizontalScrollOffsetProperty); }
            set { SetValue(HorizontalScrollOffsetProperty, value); }
        }

        //----------------------------
        public static readonly DependencyProperty CellsPanelStyleProperty =
          DependencyProperty.Register("CellsPanelStyle", typeof(Style), typeof(AyTableView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(AyTableView.OnCellsPanelStyleChanged)));

        public double HorizontalScrollOffset2
        {
            get { return (double)GetValue(HorizontalScrollOffset2Property); }
            set { SetValue(HorizontalScrollOffset2Property, value); }
        }
        //--------------
        public static readonly DependencyProperty HorizontalScrollOffset2Property =
          DependencyProperty.Register("HorizontalScrollOffset2", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(AyTableView.OnHorizontalOffset2PropertyChanged)));
        private static void OnHorizontalOffset2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).Notify2PropertyChanged(d, e);
        }


        private static void OnCellsPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).NotifyPropertyChanged(d, e);
        }

        public Style CellsPanelStyle
        {
            get { return (Style)GetValue(CellsPanelStyleProperty); }
            set { SetValue(CellsPanelStyleProperty, value); }
        }

        //----------------------------
        public static readonly DependencyProperty RowsPanelStyleProperty =
          DependencyProperty.Register("RowsPanelStyle", typeof(Style), typeof(AyTableView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(AyTableView.OnRowsPanelStyleChanged)));

        private static void OnRowsPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).NotifyPropertyChanged(d, e);
        }

        public Style RowsPanelStyle
        {
            get { return (Style)GetValue(RowsPanelStyleProperty); }
            set { SetValue(RowsPanelStyleProperty, value); }
        }

        //----------------------------
        public static readonly DependencyProperty HeaderPanelStyleProperty =
          DependencyProperty.Register("HeaderPanelStyle", typeof(Style), typeof(AyTableView), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(AyTableView.OnHeaderPanelStyleChanged)));

        private static void OnHeaderPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AyTableView)d).NotifyPropertyChanged(d, e);
        }

        public Style HeaderPanelStyle
        {
            get { return (Style)GetValue(HeaderPanelStyleProperty); }
            set { SetValue(HeaderPanelStyleProperty, value); }
        }

        //----------------------------
        public static readonly DependencyProperty GridLinesBrushProperty =
          DependencyProperty.Register("GridLinesBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(Brushes.Gray));

        public Brush GridLinesBrush
        {
            get { return (Brush)GetValue(GridLinesBrushProperty); }
            set { SetValue(GridLinesBrushProperty, value); }
        }



        public bool? HasRectangleSelection
        {
            get { return (bool?)GetValue(HasRectangleSelectionProperty); }
            set { SetValue(HasRectangleSelectionProperty, value); }
        }

        public static readonly DependencyProperty HasRectangleSelectionProperty =
            DependencyProperty.Register("HasRectangleSelection", typeof(bool?), typeof(AyTableView), new PropertyMetadata(null, OnHasRectangleSelectionChanged));

        private static void OnHasRectangleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (d as AyTableView);
            _1.NotifyHasRectangleSelection((bool?)e.NewValue);
        }

        private void NotifyHasRectangleSelection(bool? d)
        {
            if (d.HasValue)
            {
                if (!d.Value)
                {
                    AyTableViewSelector.SetEnabled(this, false);
                }
                else
                {

                    this.Loaded += WhenSetRectangle_Loaded;
                }
            }
            else
            {
                AyTableViewSelector.SetEnabled(this, false);
            }
        }
        public int ItemsCount
        {
            get { return (int)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(int), typeof(AyTableView), new FrameworkPropertyMetadata(0));
        /// <summary>
        /// 行高
        /// </summary>
        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(32.00));

        /// <summary>
        /// 最小行高
        /// </summary>
        public double MinRowHeight
        {
            get { return (double)GetValue(MinRowHeightProperty); }
            set { SetValue(MinRowHeightProperty, value); }
        }

        public static readonly DependencyProperty MinRowHeightProperty =
            DependencyProperty.Register("MinRowHeight", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(28.00));

        /// <summary>
        /// 最大行高
        /// </summary>
        public double MaxRowHeight
        {
            get { return (double)GetValue(MaxRowHeightProperty); }
            set { SetValue(MaxRowHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxRowHeightProperty =
            DependencyProperty.Register("MaxRowHeight", typeof(double), typeof(AyTableView), new FrameworkPropertyMetadata(1000.00));


        /// <summary>
        /// 隔行换色 2017-12-1 21:35:45
        /// </summary>
        public Brush AlterLineBrush
        {
            get { return (Brush)GetValue(AlterLineBrushProperty); }
            set { SetValue(AlterLineBrushProperty, value); }
        }

        public static readonly DependencyProperty AlterLineBrushProperty =
            DependencyProperty.Register("AlterLineBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#F5F5F5")));



        /// <summary>
        /// 是否有索引列
        /// ay
        /// 2017-12-2 09:30:31
        /// </summary>
        public bool HasIndexColumn
        {
            get { return (bool)GetValue(HasIndexColumnProperty); }
            set { SetValue(HasIndexColumnProperty, value); }
        }

        public static readonly DependencyProperty HasIndexColumnProperty =
            DependencyProperty.Register("HasIndexColumn", typeof(bool), typeof(AyTableView), new FrameworkPropertyMetadata(false));



        public bool HasCheckBoxColumn
        {
            get { return (bool)GetValue(HasCheckBoxColumnProperty); }
            set { SetValue(HasCheckBoxColumnProperty, value); }
        }

        public static readonly DependencyProperty HasCheckBoxColumnProperty =
            DependencyProperty.Register("HasCheckBoxColumn", typeof(bool), typeof(AyTableView), new FrameworkPropertyMetadata(false));



        /// <summary>
        /// 行选中颜色
        /// </summary>
        public Brush RowSelectedBrush
        {
            get { return (Brush)GetValue(RowSelectedBrushProperty); }
            set { SetValue(RowSelectedBrushProperty, value); }
        }

        public static readonly DependencyProperty RowSelectedBrushProperty =
            DependencyProperty.Register("RowSelectedBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#00C1DE")));


        public Brush RowHoverBrush
        {
            get { return (Brush)GetValue(RowHoverBrushProperty); }
            set { SetValue(RowHoverBrushProperty, value); }
        }

        public static readonly DependencyProperty RowHoverBrushProperty =
            DependencyProperty.Register("RowHoverBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#EEEEEE")));



        public Brush RowTenHoverBrush
        {
            get { return (Brush)GetValue(RowTenHoverBrushProperty); }
            set { SetValue(RowTenHoverBrushProperty, value); }
        }

        public static readonly DependencyProperty RowTenHoverBrushProperty =
            DependencyProperty.Register("RowTenHoverBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#EEEEEE")));



        /// <summary>
        /// 头部文字默认颜色
        /// </summary>
        public Brush HeaderStaticForeground
        {
            get { return (Brush)GetValue(HeaderStaticForegroundProperty); }
            set { SetValue(HeaderStaticForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderStaticForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderStaticForegroundProperty =
            DependencyProperty.Register("HeaderStaticForeground", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#888888")));



        public FontFamily HeaderFontFamily
        {
            get { return (FontFamily)GetValue(HeaderFontFamilyProperty); }
            set { SetValue(HeaderFontFamilyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderFontFamily.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderFontFamilyProperty =
            DependencyProperty.Register("HeaderFontFamily", typeof(FontFamily), typeof(AyTableView), new PropertyMetadata(new FontFamily("微软雅黑")));



        public Brush HeaderBorderBrush
        {
            get { return (Brush)GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register("HeaderBorderBrush", typeof(Brush), typeof(AyTableView), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        public Thickness HeaderBorderThickness
        {
            get { return (Thickness)GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderBorderThicknessProperty =
            DependencyProperty.Register("HeaderBorderThickness", typeof(Thickness), typeof(AyTableView), new PropertyMetadata(new Thickness(0)));




        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register("HeaderFontSize", typeof(double), typeof(AyTableView), new PropertyMetadata(12.00));


        /// <summary>
        /// 头部移上去背景色
        /// </summary>
        public Brush HeaderHoverBackground
        {
            get { return (Brush)GetValue(HeaderHoverBackgroundProperty); }
            set { SetValue(HeaderHoverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderHoverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHoverBackgroundProperty =
            DependencyProperty.Register("HeaderHoverBackground", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#EFF0F4")));


        /// <summary>
        /// 都不排序 按钮激活 颜色
        /// </summary>
        public Brush HeaderSortActive
        {
            get { return (Brush)GetValue(HeaderSortActiveProperty); }
            set { SetValue(HeaderSortActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderSortActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderSortActiveProperty =
            DependencyProperty.Register("HeaderSortActive", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#00C1DE")));




        ///// <summary>
        ///// 2017-12-12 10:50:08
        ///// 底行数据模板
        ///// </summary>
        //public IEnumerable FooterRows
        //{
        //    get { return (IEnumerable)GetValue(FooterRowsProperty); }
        //    set { SetValue(FooterRowsProperty, value); }
        //}

        //public static readonly DependencyProperty FooterRowsProperty =
        //    DependencyProperty.Register("FooterRows", typeof(IEnumerable), typeof(AyTableView), new FrameworkPropertyMetadata(null));




        public Brush HeaderResizeBorderBrush
        {
            get { return (Brush)GetValue(HeaderResizeBorderBrushProperty); }
            set { SetValue(HeaderResizeBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty HeaderResizeBorderBrushProperty =
            DependencyProperty.Register("HeaderResizeBorderBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(null));



        /// <summary>
        /// 是否支持排序，控制是否显示
        /// </summary>
        public bool OrderBySupport
        {
            get { return (bool)GetValue(OrderBySupportProperty); }
            set { SetValue(OrderBySupportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrderBySupport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderBySupportProperty =
            DependencyProperty.Register("OrderBySupport", typeof(bool), typeof(AyTableView), new FrameworkPropertyMetadata(false));

        public object EmptyDataResultTemplate
        {
            get { return (object)GetValue(EmptyDataResultTemplateProperty); }
            set { SetValue(EmptyDataResultTemplateProperty, value); }
        }
        public static readonly DependencyProperty EmptyDataResultTemplateProperty =
            DependencyProperty.Register("EmptyDataResultTemplate", typeof(object), typeof(AyTableView), new FrameworkPropertyMetadata(null));



        public object BusyingTemplate
        {
            get { return (object)GetValue(BusyingTemplateProperty); }
            set { SetValue(BusyingTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BusyingTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BusyingTemplateProperty =
            DependencyProperty.Register("BusyingTemplate", typeof(object), typeof(AyTableView), new FrameworkPropertyMetadata(null));



        public Brush HeadBrush
        {
            get { return (Brush)GetValue(HeadBrushProperty); }
            set { SetValue(HeadBrushProperty, value); }
        }

        public static readonly DependencyProperty HeadBrushProperty =
            DependencyProperty.Register("HeadBrush", typeof(Brush), typeof(AyTableView), new FrameworkPropertyMetadata(SolidColorBrushConverter.From16JinZhi("#F5F6FA")));



        public FontWeight HeadFontWeight
        {
            get { return (FontWeight)GetValue(HeadFontWeightProperty); }
            set { SetValue(HeadFontWeightProperty, value); }
        }

        public static readonly DependencyProperty HeadFontWeightProperty =
            DependencyProperty.Register("HeadFontWeight", typeof(FontWeight), typeof(AyTableView), new FrameworkPropertyMetadata(FontWeights.Normal));



        /// <summary>
        /// 列头可见性
        /// </summary>
        public Visibility HeadVisibility
        {
            get { return (Visibility)GetValue(HeadVisibilityProperty); }
            set { SetValue(HeadVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeadVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeadVisibilityProperty =
            DependencyProperty.Register("HeadVisibility", typeof(Visibility), typeof(AyTableView), new FrameworkPropertyMetadata(Visibility.Visible));


        /// <summary>
        /// 索引列宽度
        /// </summary>
        public double? IndexColumnWidth
        {
            get { return (double?)GetValue(IndexColumnWidthProperty); }
            set { SetValue(IndexColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IndexColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexColumnWidthProperty =
            DependencyProperty.Register("IndexColumnWidth", typeof(double?), typeof(AyTableView), new PropertyMetadata(48.00));


        public AyTableViewSelectionMode SelectionMode
        {
            get { return (AyTableViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(AyTableViewSelectionMode), typeof(AyTableView), new FrameworkPropertyMetadata(AyTableViewSelectionMode.Single, OnSelectionModeChanged));

        private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyTableView).NotifySelectionModeChanged();
        }

        private void NotifySelectionModeChanged()
        {
            ResetSelectedItems();
        }
        #endregion


    }

}
