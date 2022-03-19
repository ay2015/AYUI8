using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ay.Controls
{
    public interface ITableViewColumnHeader
    {
        int ColumnIndex { get; }
        AyTableViewColumn Column { get; }
    }


    public class AyTableViewColumnHeader : Button, ITableViewColumnHeader
    {
        #region 组头


        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(AyTableViewColumnHeader), new FrameworkPropertyMetadata(false));


        public int FromIndex { get; set; }
        public int ToIndex { get; set; }

        internal string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        internal static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(AyTableViewColumnHeader), new FrameworkPropertyMetadata(""));





        //public object StackItems
        //{
        //    get { return (object)GetValue(StackItemsProperty); }
        //    set { SetValue(StackItemsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for StackItems.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty StackItemsProperty =
        //    DependencyProperty.Register("StackItems", typeof(object), typeof(AyTableViewColumnHeader), new FrameworkPropertyMetadata(null));




        public TableViewColumnCollection Columns
        {
            get { return (TableViewColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(TableViewColumnCollection), typeof(AyTableViewColumnHeader), new PropertyMetadata(new TableViewColumnCollection()));




        #endregion

        public int ColumnIndex { get { return Column.ColumnIndex; } }
        public AyTableViewColumn Column { get { return this.Content as AyTableViewColumn; } }

        /// <summary>
        /// 列高,单倍列高
        /// </summary>
        public double ColumnHeight
        {
            get { return (double)GetValue(ColumnHeightProperty); }
            set { SetValue(ColumnHeightProperty, value); }
        }

        public static readonly DependencyProperty ColumnHeightProperty =
            DependencyProperty.Register("ColumnHeight", typeof(double), typeof(AyTableViewColumnHeader), new PropertyMetadata(32.00));



        internal void AdjustWidth(double width)
        {
            if (width < 1)
                width = 1;

            Width = width;  // adjust the width of this control

            Column.AdjustWidth(width);  // adjust the width of the column

        }




        public string CellContentStringFormat
        {
            get { return (string)GetValue(CellContentStringFormatProperty); }
            set { SetValue(CellContentStringFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CellContentStringFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellContentStringFormatProperty =
            DependencyProperty.Register("CellContentStringFormat", typeof(string), typeof(AyTableViewColumnHeader), new PropertyMetadata(null));




        private AyTableViewColumn.ColumnSortDirection CurrentSort { get; set; }
        public override void OnApplyTemplate()
        {
            var col = this.Content as AyTableViewColumn;
            if (col != null)
            {
                this.ContentTemplate = col.TitleTemplate;
                this.ContentStringFormat = col.ContentStringFormat;
                this.HorizontalContentAlignment = col.HorizontalAlignment;
                this.CellContentStringFormat = col.CellContentStringFormat;//2018-2-22 17:18:12增加
                this.VerticalContentAlignment = col.VerticalAlignment;
                this.ContentMargin = col.HeaderMargin;
                this.ResizeColumn = col.ResizeColumn;
                if (col.ParentTableView.IsNotNull())
                {
                    if (col.ParentTableView.HasIndexColumn && col.Field.ToObjectString() == "AYID")
                    {
                        this.ColumnHeight = col.ParentTableView.HeaderHeight;
                    }
                    else if (col.ParentTableView.HasCheckBoxColumn && col.Tag1.ToObjectString() == "AYCHECK")
                    {
                        this.ColumnHeight = col.ParentTableView.HeaderHeight;
                    }
                    else
                    {
                        this.ColumnHeight = col.RowSpan * (col.ParentTableView.HeaderHeight / col.ParentTableView.HeadRowCount);
                    }
                    if (Column.ParentTableView.OrderBySupport)
                    {
                        CurrentSort = Column.SortDirection;
                        this.Click += AyTableViewColumnHeader_Click;
                        this.Unloaded += AyTableViewColumnHeader_Unloaded;
                    }
                }


            }


            base.OnApplyTemplate();
        }


        public bool ResizeColumn
        {
            get { return (bool)GetValue(ResizeColumnProperty); }
            set { SetValue(ResizeColumnProperty, value); }
        }
        public static readonly DependencyProperty ResizeColumnProperty =
            DependencyProperty.Register("ResizeColumn", typeof(bool), typeof(AyTableViewColumnHeader), new FrameworkPropertyMetadata(true));



        private void AyTableViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (Column.SortDirection == AyTableViewColumn.ColumnSortDirection.None)
            {
                return;
            }
            string _3 = null;
            if (CurrentSort == AyTableViewColumn.ColumnSortDirection.No)
            {
                CurrentSort = AyTableViewColumn.ColumnSortDirection.Up;
                _3 = " asc";
            }
            else if (CurrentSort == AyTableViewColumn.ColumnSortDirection.Up)
            {
                CurrentSort = AyTableViewColumn.ColumnSortDirection.Down;
                _3 = " desc";
            }
            else if (CurrentSort == AyTableViewColumn.ColumnSortDirection.Down)
            {
                CurrentSort = AyTableViewColumn.ColumnSortDirection.No;
                _3 = " no";
            }
            Column.SortDirection = CurrentSort;
            //触发排序+=  客户端排序  还是服务端排序，服务端需要提供action
            if (Column.ParentTableView.OrderBySupport)
            {
                if (!Column.ParentTableView.IsBusy)
                {
                    if (Column.ParentTableView.ColumnLastClickOrderBy != null)
                    {
                        if (Column.ParentTableView.ColumnLastClickOrderBy.Column != Column)
                        {
                            Column.ParentTableView.OrderCondition = Column.Field + _3;
                            Column.ParentTableView.ColumnLastClickOrderBy.CurrentSort = AyTableViewColumn.ColumnSortDirection.No;
                            Column.ParentTableView.ColumnLastClickOrderBy.Column.SortDirection = AyTableViewColumn.ColumnSortDirection.No;
                            Column.ParentTableView.ColumnLastClickOrderBy = this;
                            Column.ParentTableView.RaiseSortingChanged(Column);
                        }
                        else
                        {
                            Column.ParentTableView.OrderCondition = Column.Field + _3;
                            Column.ParentTableView.RaiseSortingChanged(Column);

                        }
                    }
                    else
                    {
                        Column.ParentTableView.OrderCondition = Column.Field + _3;
                        Column.ParentTableView.ColumnLastClickOrderBy = this;
                        Column.ParentTableView.RaiseSortingChanged(Column);

                    }
                }
            }

        }

        private void AyTableViewColumnHeader_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= AyTableViewColumnHeader_Unloaded;
            if (Column.IsNotNull())
            {
                if (Column.ParentTableView.OrderBySupport)
                {
                    this.Click -= AyTableViewColumnHeader_Click;
                }
            }

        }
    

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(AyTableViewColumnHeader), new PropertyMetadata(new Thickness(0.00)));




        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Focus();
            Column.FocusColumn();
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            Focus();
            Column.FocusColumn();
            base.OnMouseRightButtonDown(e);
        }
    }
}
