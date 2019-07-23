using System;
using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{

    public class AyWrapPanelFill : Panel
    {


        public static int GetItemIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(ItemIndexProperty);
        }

        public static void SetItemIndex(DependencyObject obj, int value)
        {
            obj.SetValue(ItemIndexProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.RegisterAttached("ItemIndex", typeof(int), typeof(AyWrapPanelFill), new PropertyMetadata(0));


        // Using a DependencyProperty as the backing store for MinItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinItemWidthProperty =
            DependencyProperty.Register("MinItemWidth", typeof(double), typeof(AyWrapPanelFill), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        // Using a DependencyProperty as the backing store for MaxItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxItemWidthProperty =
            DependencyProperty.Register("MaxItemWidth", typeof(double), typeof(AyWrapPanelFill), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        // Using a DependencyProperty as the backing store for ItemMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register("ItemMargin", typeof(double), typeof(AyWrapPanelFill), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        // Using a DependencyProperty as the backing store for RowMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowMarginProperty =
            DependencyProperty.Register("RowMargin", typeof(double), typeof(AyWrapPanelFill), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
   
        //单独介绍一下FloorItemWidth的作用，这个属性用来设置是否让每个Item的宽度一定是个整数。如果宽度不是整数，TextBlock、Image等内容可能会变得很模糊。
        // Using a DependencyProperty as the backing store for FloorItemWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloorItemWidthProperty =
            DependencyProperty.Register("FloorItemWidth", typeof(bool), typeof(AyWrapPanelFill), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));
        ///单独介绍一下FloorItemWidth的作用，这个属性用来设置是否让每个Item的宽度一定是个整数。如果宽度不是整数，TextBlock、Image等内容可能会变得很模糊。
        public bool FloorItemWidth
        {
            get { return (bool)GetValue(FloorItemWidthProperty); }
            set { SetValue(FloorItemWidthProperty, value); }
        }

        public double MinItemWidth
        {
            get { return (double)GetValue(MinItemWidthProperty); }
            set { SetValue(MinItemWidthProperty, value); }
        }

        public double MaxItemWidth
        {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }

        public double ItemMargin
        {
            get { return (double)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public double RowMargin
        {
            get { return (double)GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            double height = CalculateHeight(0.0);
            double maxHeight = 0;
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
                double h = CalculateHeight(child.DesiredSize.Height);
                height += h;

                if (maxHeight < h)
                { maxHeight = h; }
            }

            if (this.RenderSize.Width == 0 || this.RenderSize.Height == 0)
            {
                return new Size(0, 0);
            }

            int itemCountInRow = CalculateItemsCountInOneRow(new Size(this.RenderSize.Width, RenderSize.Height));
            if (Children.Count % itemCountInRow != 0)
            {
                height += (itemCountInRow - Children.Count % itemCountInRow) * maxHeight;
            }
            if (itemCountInRow == 0)
            {
                return new Size(0, 0);
            }
            return new Size(0, height / itemCountInRow);
        }

        private double CalculateHeight(double height)
        {
            return height + RowMargin;
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            double yOffset = 0.0;
            double xOffset = 0.0;
            int itemCountInRow = CalculateItemsCountInOneRow(finalSize);
            double itemWidth = CalculateItemWidth(finalSize.Width, itemCountInRow);
            int j = 0;
            for (int i = 0; i < Children.Count;)
            {
       
                double rowHeight = 0;
                for (int column = 0; column < itemCountInRow && i + column < Children.Count; column++)
                {
                    UIElement child = Children[i + column];
                    child.Arrange(new Rect(xOffset, yOffset, itemWidth, child.DesiredSize.Height));
                    if (child.DesiredSize.Height > rowHeight)
                    {
                        rowHeight = child.DesiredSize.Height;
                    }

                    xOffset += itemWidth + ItemMargin;
                    AyWrapPanelFill.SetItemIndex(child,j);
                    j++;
                }

                yOffset += rowHeight + RowMargin;
                xOffset = 0.0;
                i += itemCountInRow;
                j = i;
            }

            return base.ArrangeOverride(finalSize);
        }

        private int CalculateItemsCountInOneRow(Size finalSize)
        {
            // Calling Math.Floor is necessory or not?
            return (int)Math.Floor(((finalSize.Width + ItemMargin) / (MinItemWidth + ItemMargin)));
        }

        private double CalculateItemWidth(double totalWidth, int itemCountInRow)
        {
            if (itemCountInRow > Children.Count)
            {
                itemCountInRow = Children.Count;
            }

            double itemWidth = (totalWidth - (itemCountInRow - 1) * ItemMargin) / itemCountInRow;

            if (itemWidth > MaxItemWidth)
            {
                itemWidth = MaxItemWidth;
            }

            return FloorItemWidth ? Math.Floor(itemWidth) : itemWidth;
        }
    }
}
