using System;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Controls;

namespace ay.MARKUP.ResponsiveSupport
{
    [ContentProperty("VisualStates")]
    public class AyVisualStateGroup : DependencyObject
    {
        DependencyObject owner;
        public DependencyObject Owner
        {
            get { return owner; }
            set
            {
                owner = value;
            }
        }

        #region VisualState
        private AyVisualStateCollection _VisualStates = null;
        public AyVisualStateCollection VisualStates
        {
            get
            {

                VerifyAccess();

                if (_VisualStates == null)
                {
                    _VisualStates = new AyVisualStateCollection();
                }
                return _VisualStates;
            }
        }

        //private static readonly DependencyPropertyKey VisualStatesPropertyKey = DependencyProperty.RegisterReadOnly("VisualStates",
        //    typeof(AyVisualStateCollection), typeof(AyVisualStateGroup), new FrameworkPropertyMetadata(new AyVisualStateCollection()));
        //public static readonly DependencyProperty VisualStatesProperty = AyVisualStateGroup.VisualStatesPropertyKey.DependencyProperty;

        //public AyVisualStateCollection VisualStates
        //{
        //    get
        //    {
        //        VerifyAccess();
        //        var _1=(AyVisualStateCollection)base.GetValue(AyVisualStateGroup.VisualStatesProperty);
        //        return _1;
        //    }
        //}

        #endregion


        /// <summary>
        /// 组名
        /// </summary>
        public AyOrientation Orientation
        {
            get { return (AyOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(AyOrientation), typeof(AyVisualStateGroup), new FrameworkPropertyMetadata(AyOrientation.Both));


    }



    public enum AyOrientation
    {
        //
        // 摘要:
        //     控件或布局应是水平方向的。
        Horizontal = 0,
        //
        // 摘要:
        //     控件或布局应是垂直方向的。
        Vertical = 1,
        //
        // 摘要:
        //     控件或布局应是水平，垂直方向的。
        Both = 2
    }
}
