using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using PixelLab.Common;
using System.ComponentModel;
using System.Linq;

namespace ay.Controls
{
    public class AyTableViewCell : ContentControl
    {

        ///// <summary>
        ///// 单元格状态，如果编辑就会切换模板
        ///// </summary>
        //public bool IsEdit
        //{
        //    get { return (bool)GetValue(IsEditProperty); }
        //    set { SetValue(IsEditProperty, value); }
        //}
        //public static readonly DependencyProperty IsEditProperty =
        //    DependencyProperty.Register("IsEdit", typeof(bool), typeof(AyTableViewCell), new PropertyMetadata(false));



        public DataTemplate CellEditTemplate
        {
            get { return (DataTemplate)GetValue(CellEditTemplateProperty); }
            set { SetValue(CellEditTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellEditTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellEditTemplateProperty =
            DependencyProperty.Register("CellEditTemplate", typeof(DataTemplate), typeof(AyTableViewCell), new PropertyMetadata(null, OnCellEditTemplateChanged));

        private static void OnCellEditTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _1 = (d as AyTableViewCell);
            if (e.NewValue != null)
            {
                _1.IsEditCellNull = false;
            }
            else
            {
                _1.IsEditCellNull = true;
            }
        }

        public bool IsEditCellNull
        {
            get { return (bool)GetValue(IsEditCellNullProperty); }
            set { SetValue(IsEditCellNullProperty, value); }
        }
        public static readonly DependencyProperty IsEditCellNullProperty =
            DependencyProperty.Register("IsEditCellNull", typeof(bool), typeof(AyTableViewCell), new PropertyMetadata(true));




        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(AyTableViewCell), new PropertyMetadata(false, OnIsSelectedChanged));



        private static void OnIsSelectedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                (source as AyTableViewCell).UpdateSelection();
        }

        private void UpdateSelection()
        {
            if (ParentTableView.SelectedCell != null)
                ParentTableView.SelectedCell.IsSelected = false;
            ParentTableView.SelectedCell = this;
        }

        //public bool IsSelected
        //{
        //    get { return (bool)GetValue(IsSelectedProperty); }
        //    private set { SetValue(IsSelectedPropertyKey, value); }
        //}

        public AyTableViewCellsPresenter ParentCellsPresenter = null;
        public AyTableViewColumn _column = null;

        public AyTableView ParentTableView = null;
        public int ColumnIndex { get { return ParentTableView.Columns.IndexOf(_column); } }
        public object Item { get { return ParentCellsPresenter.Item; } }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var tmp = new Size(Math.Max(0.0, arrangeBounds.Width - 1), arrangeBounds.Height);
            Size sz = base.ArrangeOverride(tmp);
            sz.Width += 1;
            return sz;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var tmp = new Size(Math.Max(0.0, constraint.Width - 1), constraint.Height);
            Size sz = base.MeasureOverride(tmp);
            sz.Width += 1;
            return sz;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (ParentTableView.ShowVerticalGridLines)
            {
                double verticalGridLineThickness = 1;
                Rect rectangle = new Rect(new Size(verticalGridLineThickness, base.RenderSize.Height));
                rectangle.X = base.RenderSize.Width - verticalGridLineThickness;
                drawingContext.DrawRectangle(ParentTableView.GridLinesBrush, null, rectangle);
            }
        }
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(AyTableViewCell), new FrameworkPropertyMetadata(new Thickness(0.00)));


        /// <summary>
        /// 单元格内容换行方式
        /// 2017-11-29 22:05:43
        /// AY
        /// </summary>
        public TextWrapping CellTextWrapping
        {
            get { return (TextWrapping)GetValue(CellTextWrappingProperty); }
            set { SetValue(CellTextWrappingProperty, value); }
        }

        public static readonly DependencyProperty CellTextWrappingProperty =
            DependencyProperty.Register("CellTextWrapping", typeof(TextWrapping), typeof(AyTableViewCell), new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnCellTextWrappingChanged));

        private static void OnCellTextWrappingChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as AyTableViewCell).NotifyCellTextWrappingChanged((TextWrapping)e.NewValue);
        }
        private void NotifyCellTextWrappingChanged(TextWrapping newvalue)
        {
            if (tb.IsNotNull())
            {
                tb.TextWrapping = newvalue;
            }
        }

        public TextTrimming CellCharacterEllipsis
        {
            get { return (TextTrimming)GetValue(CellCharacterEllipsisProperty); }
            set { SetValue(CellCharacterEllipsisProperty, value); }
        }

        public static readonly DependencyProperty CellCharacterEllipsisProperty =
            DependencyProperty.Register("CellCharacterEllipsis", typeof(TextTrimming), typeof(AyTableViewCell), new FrameworkPropertyMetadata(TextTrimming.None, OnCellCharacterEllipsisChanged));
        private static void OnCellCharacterEllipsisChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            (source as AyTableViewCell).NotifyCellCharacterEllipsisChanged((TextTrimming)e.NewValue);
        }
        private void NotifyCellCharacterEllipsisChanged(TextTrimming newvalue)
        {
            if (tb.IsNotNull())
            {
                tb.TextTrimming = newvalue;
            }
        }






        public void PrepareCell(AyTableViewCellsPresenter parent, int idx)
        {
            ParentCellsPresenter = parent;
            ParentTableView = parent.ParentTableView;

            var column = ParentTableView.Columns[idx];

            //IsSelected = ParentCellsPresenter.IsSelected() && (ParentTableView.FocusedColumnIndex == column.ColumnIndex);

            if (_column != column)
            {
                _column = column;
                this.Width = column.Width;
                BindingOperations.ClearBinding(this, WidthProperty);
                BindingOperations.SetBinding(this, WidthProperty, column.WidthBinding);
                Focusable = ParentTableView.CellNavigation;
                BindingOperations.ClearBinding(this, ColumnFocusBrushProperty);
                BindingOperations.SetBinding(this, ColumnFocusBrushProperty, new Binding { Source = _column, Mode = BindingMode.TwoWay, Path = new PropertyPath("ColumnFocusBrush") });
            }
            column.GenerateCellContent(this);
        }


        public Brush ColumnFocusBrush
        {
            get { return (Brush)GetValue(ColumnFocusBrushProperty); }
            set { SetValue(ColumnFocusBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnFocusBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnFocusBrushProperty =
            DependencyProperty.Register("ColumnFocusBrush", typeof(Brush), typeof(AyTableViewCell), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public ContentPresenter _EditContent = null;
        TextBlock tb = null;
        Border bd = null;
        DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(TextBlock.ActualWidthProperty, typeof(TextBlock));
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            bd = GetTemplateChild("SelectTen") as Border;


            ContentControl cp = GetTemplateChild("contentPresenter") as ContentControl;
            _EditContent = GetTemplateChild("contentPresenter1") as ContentPresenter;
            if (cp == null) return;
            if (cp.ContentTemplate.IsNull())
            {
                if (_column.IsNotNull() && _column.Formatter.IsNotNull())
                {
                    var _b = BindingOperations.GetBinding(cp, ContentPresenter.ContentProperty);
                    Binding newbind = new Binding { Converter = _column.Formatter, Path = _b.Path, Mode = _b.Mode, RelativeSource = _b.RelativeSource };
                    BindingOperations.ClearBinding(cp, ContentPresenter.ContentProperty);
                    BindingOperations.SetBinding(cp, ContentPresenter.ContentProperty, newbind);
                }
                cp.ApplyTemplate();
                //TextBlock tb = new TextBlock();
                //var _1b = BindingOperations.GetBinding(cp, ContentPresenter.ContentProperty);
                //Binding newbind1 = new Binding { Path = _1b.Path, Mode = _1b.Mode, RelativeSource = _1b.RelativeSource };
                //BindingOperations.ClearBinding(cp, ContentPresenter.ContentProperty);

                //BindingOperations.SetBinding(tb, TextBlock.TextProperty, newbind1);
                var _cpa = WpfTreeHelper.FindChild<ContentPresenter>(cp);
                if (_cpa == null) return;
                _cpa.ApplyTemplate();
                tb = WpfTreeHelper.FindChild<TextBlock>(_cpa);
                Binding newbindh = new Binding { Path = new PropertyPath("HorizontalContentAlignment"), Mode = BindingMode.TwoWay, Source = this };
                BindingOperations.SetBinding(_cpa, ContentPresenter.HorizontalAlignmentProperty, newbindh);

                Binding newbindv = new Binding { Path = new PropertyPath("VerticalContentAlignment"), Mode = BindingMode.TwoWay, Source = this };
                BindingOperations.SetBinding(_cpa, ContentPresenter.VerticalAlignmentProperty, newbindv);
                Binding newbindv2 = new Binding { Path = new PropertyPath("VerticalContentAlignment"), Mode = BindingMode.TwoWay, Source = this };
                BindingOperations.SetBinding(cp, ContentControl.VerticalContentAlignmentProperty, newbindv2);
           
                if (tb.IsNotNull())
                {
                    tb.VerticalAlignment = this.VerticalContentAlignment;
                    tb.TextWrapping = CellTextWrapping;

                    if (CellCharacterEllipsis == TextTrimming.CharacterEllipsis)
                    {
                        ToolTip tt = new System.Windows.Controls.ToolTip();
                        TextBlock ttb = new TextBlock();
                        ttb.SetBinding(TextBlock.TextProperty, new Binding { Source = cp.Content });
                        tt.Content = ttb;
                        tb.ToolTip = tt;
                        if (dpd != null)
                        {
                            dpd.AddValueChanged(tb, ActualWidthPropertyChangedHandler);
                        }
                        if (tb.ActualWidth < tb.DesiredSize.Width)
                            tt.Visibility = Visibility.Visible;
                        else
                            tt.Visibility = Visibility.Collapsed;
                    }
                    tb.TextTrimming = CellCharacterEllipsis;
                }
            }



        }

        private void ActualWidthPropertyChangedHandler(object sender, EventArgs e)
        {
            var _1 = sender as FrameworkElement;
            tb.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));
            if (_1.ActualWidth < _1.DesiredSize.Width)
                ((ToolTip)_1.ToolTip).Visibility = Visibility.Visible;
            else
                ((ToolTip)_1.ToolTip).Visibility = Visibility.Collapsed;
        }



        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (ParentTableView.CellNavigation)
            {
                _column.FocusColumn();
            }
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.Cell)
            {
                IsSelected = true;
            }

            //ParentTableView.FocusedItemChanged(ParentCellsPresenter);
        }
        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
            ActionDown();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            ActionDown();
        }

        private void ActionDown()
        {
            _column.FocusColumn();
            if (ParentTableView.RowClickMode == AyTableView.RowClickModes.CellEdit)
            {
                if (ParentTableView.SelectedCell != null)
                {
                    //ParentTableView.SelectedCell.CloseEditCell();
                }
                //OpenEditCell();
            }
            Focus();

        }

        //public void OpenEditCell()
        //{
        //    if (_column.Field == "AYID" || _column.Field == "AYCHECK")
        //    {
        //        return;
        //    }
        //    UpdateSelection();
        //    this.IsEdit = true;
        //    if (_column.CellEditTemplate.IsNotNull())
        //    {
        //        if (_column.Field.IsEmptyAndNull())
        //        {
        //            ParentTableView.RaiseCellEditBegin(Item, _column.FieldRemark);
        //        }
        //        else
        //        {
        //            ParentTableView.RaiseCellEditBegin(Item, _column.Field);
        //        }
        //    }
         

        //}

        //public void CloseEditCell(bool isCancel = false)
        //{

        //    this.IsEdit = false;
        //    if (isCancel) return;
        //    if (_column.CellEditTemplate.IsNotNull())
        //    {
        //        if (_column.Field.IsEmptyAndNull())
        //        {
        //            ParentTableView.RaiseCellEditEnd(Item, _column.FieldRemark);
        //        }
        //        else
        //        {
        //            ParentTableView.RaiseCellEditEnd(Item, _column.Field);
        //        }
        //    }
        //}

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ActionEnter();
        }
        protected override void OnTouchEnter(TouchEventArgs e)
        {
            base.OnTouchEnter(e);
            ActionEnter();
        }

        protected override void OnTouchLeave(TouchEventArgs e)
        {
            base.OnTouchEnter(e);
            ActionLeave();
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            ActionLeave();
        }
        private void ActionEnter()
        {
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle || AyTableViewSelectionMode.RowTenNoSelect == ParentTableView.SelectionMode)
            {
                if (ParentTableView != null)
                    ParentTableView.EnterColumnChanged(_column);
                Focus();
            }
        }
        private void ActionLeave()
        {
            if (ParentTableView.SelectionMode == AyTableViewSelectionMode.RowTenSingle
                || AyTableViewSelectionMode.RowTenNoSelect == ParentTableView.SelectionMode
                )
            {
                _column.LostFocusColumn();
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            _column.FocusColumn();
            Focus();
        }

    }
}
//public class TrimmedTextBlockVisibilityConverter : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//    {
//        if (value == null) return Visibility.Collapsed;

//        FrameworkElement textBlock = (FrameworkElement)value;

//        textBlock.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));
//        FrameworkElement.ActualWidthProperty.OverrideMetadata(
//          typeof(FrameworkElement),
//          new FrameworkPropertyMetadata(new PropertyChangedCallback(ActualWidthChanged)));

//        if (((FrameworkElement)value).ActualWidth < ((FrameworkElement)value).DesiredSize.Width)
//            return Visibility.Visible;
//        else
//            return Visibility.Collapsed;
//    }

//    private void ActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        var _1= (FrameworkElement)d;
//        if (_1.ActualWidth < _1.DesiredSize.Width)
//            _1.Visibility= Visibility.Visible;
//        else
//            _1.Visibility = Visibility.Collapsed;
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}
//public class AyTextBlock
//{
//    public static bool GetCharacterEllipsis(DependencyObject obj)
//    {
//        return (bool)obj.GetValue(CharacterEllipsisProperty);
//    }

//    public static void SetCharacterEllipsis(DependencyObject obj, bool value)
//    {
//        obj.SetValue(CharacterEllipsisProperty, value);
//    }

//    // Using a DependencyProperty as the backing store for CharacterEllipsis.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty CharacterEllipsisProperty =
//        DependencyProperty.RegisterAttached("CharacterEllipsis", typeof(bool), typeof(AyTextBlock),
//        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, OnCharacterEllipsisChanged));

//    private static void OnCharacterEllipsisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is TextBlock && (bool)e.NewValue)
//        {
//            TextBlock tb = (TextBlock)d;
//            tb.TextTrimming = TextTrimming.CharacterEllipsis;
//        }
//        else if (d is ContentPresenter && (bool)e.NewValue)
//        {
//            ContentPresenter cpp = (ContentPresenter)d;
//            cpp.ApplyTemplate();
//            TextBlock _1 = WpfTreeHelper.FindChild<TextBlock>(cpp);
//            if (_1.IsNotNull())
//                _1.TextTrimming = TextTrimming.CharacterEllipsis;
//        }
//    }

//    public static TextWrapping GetTextWrapping(DependencyObject obj)
//    {
//        return (TextWrapping)obj.GetValue(TextWrappingProperty);
//    }

//    public static void SetTextWrapping(DependencyObject obj, TextWrapping value)
//    {
//        obj.SetValue(CharacterEllipsisProperty, value);
//    }

//    // Using a DependencyProperty as the backing store for CharacterEllipsis.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty TextWrappingProperty =
//        DependencyProperty.RegisterAttached("TextWrapping", typeof(TextWrapping), typeof(AyTextBlock),
//        new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.Inherits, OnTextWrappingChanged));

//    private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is TextBlock)
//        {
//            TextBlock tb = (TextBlock)d;
//            tb.TextWrapping = (TextWrapping)e.NewValue;
//        }
//        else if (d is ContentPresenter)
//        {
//            ContentPresenter cpp = (ContentPresenter)d;
//            cpp.ApplyTemplate();
//            TextBlock _1 = WpfTreeHelper.FindChild<TextBlock>(cpp);
//            if (_1.IsNotNull())
//                _1.TextWrapping = (TextWrapping)e.NewValue;
//        }
//    }

//}

//public class AyTableViewCellTemplateSelector : DataTemplateSelector
//{
//    private DataTemplate _NormalDataTemplate;

//    public DataTemplate NormalDataTemplate
//    {
//        get { return _NormalDataTemplate; }
//        set { _NormalDataTemplate = value; }
//    }
//    private DataTemplate _EditDataTemplate;

//    public DataTemplate EditDataTemplate
//    {
//        get { return _EditDataTemplate; }
//        set { _EditDataTemplate = value; }
//    }


//    public override DataTemplate SelectTemplate(object item, DependencyObject container)
//    {
//        if (item != null && item is AyTableViewCell)
//        {
//            AyTableViewCell ac = item as AyTableViewCell;

//            if (ac.IsEdit)
//            {
//                return EditDataTemplate;
//            }
//            else
//            {
//                return NormalDataTemplate;
//            }

//        }
//        return null;
//    }
//}

