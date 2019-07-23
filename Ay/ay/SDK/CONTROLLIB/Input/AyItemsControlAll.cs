using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ay.Controls
{
    public class AyItemsControlAll : ItemsControl
    {
        public AyItemsControlAll()
        {

        }

        public bool IsHandScollBarSet
        {
            get { return (bool)GetValue(IsHandScollBarSetProperty); }
            set { SetValue(IsHandScollBarSetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsHandScollBarSet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHandScollBarSetProperty =
            DependencyProperty.Register("IsHandScollBarSet", typeof(bool), typeof(AyItemsControlAll), new PropertyMetadata(false));


        /// <summary>
        /// 容器面板类型
        /// </summary>
        public AyPanelAllPanelType? PanelType
        {
            get { return (AyPanelAllPanelType?)GetValue(PanelTypeProperty); }
            set { SetValue(PanelTypeProperty, value); }
        }

        public static readonly DependencyProperty PanelTypeProperty =
            DependencyProperty.Register("PanelType", typeof(AyPanelAllPanelType?), typeof(AyItemsControlAll), new PropertyMetadata(null, OnPanelTypeChanged));

        private static void OnPanelTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AyItemsControlAll).WhenPanelTypeChanged((AyPanelAllPanelType?)e.NewValue);
        }
        /// <summary>
        /// 作者：AY
        /// 生日:2016-12-19 14:08:12
        /// </summary>
        /// <param name="n"></param>
        public void WhenPanelTypeChanged(AyPanelAllPanelType? n)
        {
            if (n.HasValue)
            {
                var _1 = n.Value;
                switch (_1)
                {
                    case AyPanelAllPanelType.VirtualizingStackPanel_H:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
                            factory.SetValue(VirtualizingStackPanel.OrientationProperty, Orientation.Horizontal);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }

                        break;
                    case AyPanelAllPanelType.VirtualizingStackPanel_V:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
                            factory.SetValue(VirtualizingStackPanel.OrientationProperty, Orientation.Vertical);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.StackPanel_H:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
                            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.StackPanel_V:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
                            factory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.WrapPanel_H:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(WrapPanel));
                            factory.SetValue(WrapPanel.OrientationProperty, Orientation.Horizontal);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.WrapPanel_V:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(WrapPanel));
                            factory.SetValue(WrapPanel.OrientationProperty, Orientation.Vertical);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.Grid:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Grid));
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.UniformGrid:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(UniformGrid));
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.Canvas:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Canvas));
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    case AyPanelAllPanelType.DockPanel:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(DockPanel));
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                    default:
                        {
                            if (!IsHandScollBarSet)
                            {
                                ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
                            }
                            ItemsPanelTemplate tac = new ItemsPanelTemplate();
                            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
                            factory.SetValue(VirtualizingStackPanel.OrientationProperty, Orientation.Vertical);
                            tac.VisualTree = factory;
                            ItemsPanel = tac;
                        }
                        break;
                }


            }
        }




    }

}
