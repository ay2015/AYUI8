using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Collections.Generic;

namespace ay.Controls
{

    public class AyTableViewColumn : ContentControl
    {
        //该列对应内容列的索引
        public int? ColumnsHeadIndex { get; set; }
        public AyTableView ParentTableView { get; set; }
        public int ColumnIndex { get { return (ParentTableView == null) ? -1 : ParentTableView.Columns.IndexOf(this); } }

        /// <summary>
        /// 百分比宽度 2018-5-3 11:58:28
        /// <=1>0 就是 百分比布局  >1 就是具体数字
        /// 作者 AY
        /// 功能进度 0%
        /// </summary>
        public double PercentWidth
        {
            get { return (double)GetValue(PercentWidthProperty); }
            set { SetValue(PercentWidthProperty, value); }
        }

        public static readonly DependencyProperty PercentWidthProperty =
            DependencyProperty.Register("PercentWidth", typeof(double), typeof(AyTableViewColumn), new PropertyMetadata(null));

        #region Dependency Properties


        public enum ColumnSortDirection { None, No, Up, Down }; //No就是不排序状态但是支持，up当前时升序，down降续,none就是不支持排序，不显示

        public static readonly DependencyProperty SortDirectionProperty =
          DependencyProperty.Register("SortDirection", typeof(ColumnSortDirection), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(ColumnSortDirection.None));

        public ColumnSortDirection SortDirection
        {
            get { return (ColumnSortDirection)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }



        #region 组头


        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(AyTableViewColumn), new PropertyMetadata(false));




        internal int FromIndex
        {
            get { return (int)GetValue(FromIndexProperty); }
            set { SetValue(FromIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromIndex.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty FromIndexProperty =
            DependencyProperty.Register("FromIndex", typeof(int), typeof(AyTableViewColumn), new PropertyMetadata(0));



        internal int ToIndex
        {
            get { return (int)GetValue(ToIndexProperty); }
            set { SetValue(ToIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToIndex.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty ToIndexProperty =
            DependencyProperty.Register("ToIndex", typeof(int), typeof(AyTableViewColumn), new PropertyMetadata(0));




        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(AyTableViewColumn), new PropertyMetadata(""));


        internal double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnHeight.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty ColumnHeightProperty =
            DependencyProperty.Register("ColumnHeight", typeof(double), typeof(AyTableViewColumn), new PropertyMetadata(32.00));


        private TableViewColumnCollection _columns;

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

        private void ColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            WhenSubColumnsChanged();

        }

        private void WhenSubColumnsChanged()
        {

            if (ParentTableView.HeaderRowPresenter != null)
            {
                ParentTableView.ResetFixedClipRect();
                ParentTableView.ResetFixedClipRect2();
                ParentTableView.HeaderRowPresenter.HeaderInvalidateArrange();
                ParentTableView.HeaderRowPresenter2.HeaderInvalidateArrange();
            }

            if (ParentTableView.RowsPresenter != null)
                ParentTableView.RowsPresenter.ColumnsChanged();

        }




        #endregion


        #region 临时设计 排序

        /// <summary>
        /// 位置列，用于组
        /// </summary>
        public int ColSpan
        {
            get { return (int)GetValue(ColSpanProperty); }
            set { SetValue(ColSpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColSpanProperty =
            DependencyProperty.Register("ColSpan", typeof(int), typeof(AyTableViewColumn), new PropertyMetadata(null));


        /// <summary>
        /// 位置行
        /// </summary>
        public int RowSpan
        {
            get { return (int)GetValue(RowSpanProperty); }
            set { SetValue(RowSpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.Register("RowSpan", typeof(int), typeof(AyTableViewColumn), new PropertyMetadata(1));



        #endregion

        /// <summary>
        /// ay 2017-11-29 19:01:27 格式化cell
        /// </summary>
        //StickyNoteControl
        public string CellContentStringFormat
        {
            get { return (string)GetValue(CellContentStringFormatProperty); }
            set { SetValue(CellContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty CellContentStringFormatProperty =
            DependencyProperty.Register("CellContentStringFormat", typeof(string), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(null));


        /// <summary>
        /// 可以调整列宽
        /// 2017-12-1 13:09:01
        /// </summary>
        public bool ResizeColumn
        {
            get { return (bool)GetValue(ResizeColumnProperty); }
            set { SetValue(ResizeColumnProperty, value); }
        }

        public static readonly DependencyProperty ResizeColumnProperty =
            DependencyProperty.Register("ResizeColumn", typeof(bool), typeof(AyTableView), new FrameworkPropertyMetadata(true));



        /// <summary>
        /// 列单元格的内容margin
        ///2017-11-29 20:35:29
        /// </summary>
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(new Thickness(0.00)));



        /// <summary>
        /// cell单元格的内容margin
        /// 2017-11-29 20:32:03
        /// </summary>
        public Thickness CellMargin
        {
            get { return (Thickness)GetValue(CellMarginProperty); }
            set { SetValue(CellMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellMarginProperty =
            DependencyProperty.Register("CellMargin", typeof(Thickness), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(new Thickness(0)));





        /// <summary>
        /// 聚焦时候颜色
        /// </summary>
        internal Brush ColumnFocusBrush
        {
            get { return (Brush)GetValue(ColumnFocusBrushProperty); }
            set { SetValue(ColumnFocusBrushProperty, value); }
        }

        internal static readonly DependencyProperty ColumnFocusBrushProperty =
            DependencyProperty.Register("ColumnFocusBrush", typeof(Brush), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));



        //private static void OnSortDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((AyTableViewColumn)d).NotifySortingChanged();
        //}

        public static readonly DependencyProperty SortOrderProperty =
          DependencyProperty.Register("SortOrder", typeof(int), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(0));

        public int SortOrder
        {
            get { return (int)GetValue(SortOrderProperty); }
            set { SetValue(SortOrderProperty, value); }
        }

        public static readonly DependencyProperty CellTemplateProperty =
          DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(null));

        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        public static readonly DependencyProperty CellEditTemplateProperty =
  DependencyProperty.Register("CellEditTemplate", typeof(DataTemplate), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(null));



        //public enum CellEditType
        //{
        //    textbox, numberbox, integer,boolean,date

        //}

        /// <summary>
        /// 编辑模板
        /// </summary>
        public DataTemplate CellEditTemplate
        {
            get { return (DataTemplate)GetValue(CellEditTemplateProperty); }
            set { SetValue(CellEditTemplateProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty =
          DependencyProperty.Register("Title", typeof(object), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(null));



        public object Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleTemplateProperty =
          DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(null));

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }

        /// <summary>
        /// 单元格内容换行方式
        /// </summary>
        public TextWrapping CellTextWrapping
        {
            get { return (TextWrapping)GetValue(CellTextWrappingProperty); }
            set { SetValue(CellTextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellTextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellTextWrappingProperty =
            DependencyProperty.Register("CellTextWrapping", typeof(TextWrapping), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(TextWrapping.NoWrap));



        public TextTrimming CellCharacterEllipsis
        {
            get { return (TextTrimming)GetValue(CellCharacterEllipsisProperty); }
            set { SetValue(CellCharacterEllipsisProperty, value); }
        }

        public static readonly DependencyProperty CellCharacterEllipsisProperty =
            DependencyProperty.Register("CellCharacterEllipsis", typeof(TextTrimming), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(TextTrimming.None));



        public static readonly DependencyProperty FieldProperty =
          DependencyProperty.Register("Field", typeof(string), typeof(AyTableViewColumn), new FrameworkPropertyMetadata(""));

        public string Field
        {
            get { return (string)GetValue(FieldProperty); }
            set { SetValue(FieldProperty, value); }
        }

        //2022年3月18日15:21:57 增加控制内置功能列的属性值，方便其他的类获取处理
        internal string Tag1 { get; set; }
        internal string Tag2 { get; set; }

        #endregion

        public Binding WidthBinding { get; private set; }
        public Binding ContextBinding { get; set; }

        //internal void NotifySortingChanged()
        //{
        //    if (ParentTableView != null)
        //        ParentTableView.NotifySortingChanged(this);
        //}

        internal void AdjustWidth(double width)
        {
            if (width < 0)
                width = 0;

            Width = width;  // adjust the width of this control

            if (ParentTableView != null)
                ParentTableView.NotifyColumnWidthChanged(this); // let the table view know that this has changed
            if (ColumnWidthChanged.IsNotNull())
            {
                ColumnWidthChanged(this, null);
            }
            else
            {
                if (ResizeColumn && ParentTableView != null)
                {
                    if (ColumnsHeadIndex.HasValue)
                    {
                        ParentTableView.ColumnsHead[ColumnsHeadIndex.Value].Width = width;
                    }
                }
            }

        }

        public event EventHandler<EventArgs> ColumnWidthChanged;


        public IValueConverter Formatter
        {
            get { return (IValueConverter)GetValue(FormatterProperty); }
            set { SetValue(FormatterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Formatter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatterProperty =
            DependencyProperty.Register("Formatter", typeof(IValueConverter), typeof(AyTableViewColumn), new PropertyMetadata(null));


        public double? MinResizeColumnWidth
        {
            get { return (double?)GetValue(MinResizeColumnWidthProperty); }
            set { SetValue(MinResizeColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinResizeColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinResizeColumnWidthProperty =
            DependencyProperty.Register("MinResizeColumnWidth", typeof(double?), typeof(AyTableViewColumn), new PropertyMetadata(10.00));



        public double? MaxResizeColumnWidth
        {
            get { return (double?)GetValue(MaxResizeColumnWidthProperty); }
            set { SetValue(MaxResizeColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxResizeColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxResizeColumnWidthProperty =
            DependencyProperty.Register("MaxResizeColumnWidth", typeof(double?), typeof(AyTableViewColumn), new PropertyMetadata(null));




        public void GenerateCellContent(AyTableViewCell cell)
        {
            cell.ContentStringFormat = this.ContentStringFormat;
            //if (CellEditTemplate.IsNotNull())
            //{
            //    //cell.ContentTemplateSelector = new AyTableViewCellTemplateSelector
            //    //{
            //    //    EditDataTemplate = this.CellEditTemplate,
            //    //    NormalDataTemplate = this.CellTemplate
            //    //};
            //    cell.ContentTemplate = CellTemplate;
            //}
            //else
            //{
            cell.CellEditTemplate = this.CellEditTemplate;
            cell.ContentTemplate = CellTemplate;
            //}

            cell.HorizontalContentAlignment = HorizontalContentAlignment;
            cell.VerticalContentAlignment = VerticalContentAlignment;
            cell.ContentTemplateSelector = ContentTemplateSelector;
            cell.ContentMargin = this.CellMargin;
            cell.CellTextWrapping = this.CellTextWrapping;
            cell.CellCharacterEllipsis = this.CellCharacterEllipsis;

            if (ContextBinding == null)
                ContextBinding = new Binding(Field);
            if (ContextBinding != null)
                BindingOperations.SetBinding(cell, DataContextProperty, ContextBinding);
        }


        public string FieldRemark
        {
            get { return (string)GetValue(FieldRemarkProperty); }
            set { SetValue(FieldRemarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldRemark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldRemarkProperty =
            DependencyProperty.Register("FieldRemark", typeof(string), typeof(AyTableViewColumn), new PropertyMetadata(null));


        public void FocusColumn()
        {
            if (ParentTableView != null)
                ParentTableView.FocusedColumnChanged(this);
        }

        public AyTableViewColumn()
          : base()
        {

            Width = 100;
            //HorizontalContentAlignment = HorizontalAlignment.Stretch;
            _columns = new TableViewColumnCollection();
            WidthBinding = new Binding("Width");
            WidthBinding.Mode = BindingMode.OneWay;
            WidthBinding.Source = this;

            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        internal void LostFocusColumn()
        {
            if (ParentTableView != null)
                ParentTableView.LeaveColumnChanged(this);
        }
    }
}
