using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Data;

namespace ay.Controls
{
    public class AyTableViewHeaderPresenter : ItemsControl
    {
        public Panel HeaderItemsPanel { get; set; }

        private AyTableView _parentTableView;
        public AyTableView ParentTableView
        {
            get
            {
                if (_parentTableView == null)
                {
                    _parentTableView = AyTableViewUtils.FindParent<AyTableView>(this);
                    if (_parentTableView == null)
                    {
                        var _1 = AyTableViewUtils.FindParent<AyTableViewColumnHeader>(this);
                        if (_1.IsNotNull())
                        {
                            _parentTableView = AyTableViewUtils.FindParent<AyTableView>(_1);
                            //_1.FontFamily = _parentTableView.HeaderFontFamily;
                            //_1.FontWeight = _parentTableView.HeadFontWeight;
                            //_1.Foreground = _parentTableView.HeaderStaticForeground;
                            //_1.FontSize = _parentTableView.HeaderFontSize;
                        }
                    }
                }

                return _parentTableView;
            }
        }

        //internal AyTableViewColumnHeader GetColumnHeaderAtLocation(Point loc)
        //{
        //    var uie = InputHitTest(loc) as FrameworkElement;
        //    if (uie != null)
        //    {
        //        return AyTableViewUtils.FindParent<AyTableViewColumnHeader>(uie);
        //    }
        //    return null;
        //}

        internal void HeaderInvalidateArrange()
        {
            if (HeaderItemsPanel != null)
                HeaderItemsPanel.InvalidateArrange();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var _col = item as AyTableViewColumn;
            var _head = (element as AyTableViewColumnHeader);
            //剩余
            ParentTableView.SYWIDTH = ParentTableView.ActualWidth - ParentTableView.SYWIDTH33;
            if (_col.IsGroup)
            {
                _head.ColumnHeight = _col.Columns[0].ParentTableView.HeaderHeight / _col.Columns[0].ParentTableView.HeadRowCount;
                _head.IsGroup = _col.IsGroup;
                _head.Width = _col.Width;
                _col.ResizeColumn = false;
                _head.FromIndex = _col.FromIndex;
                _head.ToIndex = _col.ToIndex;
                _head.Columns = _col.Columns;
                _head.GroupName = _col.GroupName;
                //寻找 
                var pv = _col.Columns[0].ParentTableView;
                //List<double> ju = new List<double>();

                if (pv.PercentWidthSupport)
                {
                    double w1 = 0;
                    for (int i = _col.FromIndex; i <= _col.ToIndex; i++)
                    {
                        var _123 = pv.Columns[i];
                        if (_123.IsNotNull())
                        {
                            if (pv.Columns[i].PercentWidth > 1)
                            {
                                w1 += pv.Columns[i].PercentWidth;
                            }
                            else
                            {
                                w1 += (pv.Columns[i].PercentWidth * pv.SYWIDTH);
                            }
                            _123.ColumnWidthChanged += (send, eargs) =>
                            {
                                double w = 0;
                                for (int j = _col.FromIndex; j <= _col.ToIndex; j++)
                                {
                                    w += pv.Columns[j].Width;
                                }
                                _head.Width = w;
                                //double w = 0;

                                //for (int j = _col.FromIndex; j <= _col.ToIndex; j++)
                                //{
                                //    if (pv.Columns[j].PercentWidth > 1)
                                //    {
                                //        w += pv.Columns[j].PercentWidth;
                                //    }
                                //    else
                                //    {
                                //        w += (pv.Columns[j].PercentWidth*pv.SYWIDTH);
                                //    }
                                //}

                                //_head.Width = w;
                            };
                        }
                    }
                    _head.Width = w1;
                }
                else
                {
                    for (int i = _col.FromIndex; i <= _col.ToIndex; i++)
                    {
                        var _123 = pv.Columns[i];
                        if (_123.IsNotNull())
                        {
                            _123.ColumnWidthChanged += (send, eargs) =>
                            {
                                double w = 0;
                                for (int j = _col.FromIndex; j <= _col.ToIndex; j++)
                                {
                                    w += pv.Columns[j].Width;
                                }
                                _head.Width = w;
                            };
                        }
                    }
                }
            }
            else
            {
                _head.ColumnHeight = _col.ParentTableView.HeaderHeight / _col.ParentTableView.HeadRowCount;
                if (_col.ParentTableView.PercentWidthSupport)
                {
                    double w = 0;
                    if (_col.PercentWidth > 1)
                    {
                        w += _col.PercentWidth;
                    }
                    else
                    {
                        w += (_col.PercentWidth * _col.ParentTableView.SYWIDTH);
                    }
                    _head.Width = w;
                }
                else
                {
                    _head.Width = _col.Width;
                }
            }
        }
        //public AyTableViewColumnHeader GetItem(AyTableView pv, int index)
        //{
        //    return pv.HeaderRowPresenter.ItemContainerGenerator.ContainerFromItem(pv.HeaderRowPresenter.Items[index]) as AyTableViewColumnHeader;
        //}
        protected override DependencyObject GetContainerForItemOverride()
        {
            var _1 = new AyTableViewColumnHeader();

            Binding n1 = new Binding { Path = new PropertyPath("HeaderFontFamily"), Mode = BindingMode.TwoWay, Source = ParentTableView };
            BindingOperations.SetBinding(_1, AyTableViewColumnHeader.FontFamilyProperty, n1);

            Binding n2 = new Binding { Path = new PropertyPath("HeadFontWeight"), Mode = BindingMode.TwoWay, Source = ParentTableView };
            BindingOperations.SetBinding(_1, AyTableViewColumnHeader.FontWeightProperty, n2);

            Binding n3 = new Binding { Path = new PropertyPath("HeaderStaticForeground"), Mode = BindingMode.TwoWay, Source = ParentTableView };
            BindingOperations.SetBinding(_1, AyTableViewColumnHeader.ForegroundProperty, n3);

            Binding n4= new Binding { Path = new PropertyPath("HeaderFontSize"), Mode = BindingMode.TwoWay, Source = ParentTableView };
            BindingOperations.SetBinding(_1, AyTableViewColumnHeader.FontSizeProperty, n4);

            Binding n5 = new Binding { Path = new PropertyPath("HeaderResizeBorderBrush"), Mode = BindingMode.TwoWay, Source = ParentTableView };
            BindingOperations.SetBinding(_1, AyTableViewColumnHeader.BorderBrushProperty, n5);
            //_1.FontFamily = ParentTableView.HeaderFontFamily;
            //_1.FontWeight = ParentTableView.HeadFontWeight;
            //_1.Foreground = ParentTableView.HeaderStaticForeground;
            //_1.FontSize = ParentTableView.HeaderFontSize;

            return _1;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is AyTableViewColumnHeader);
        }
    }
}
