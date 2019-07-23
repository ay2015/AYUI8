using ay.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ay.Framework.WPF.Controls.Transitions
{
    public enum AyTransitionDirection
    {
        up,
        down,
        left,
        right
    }

    public class ScanMianSingleTransition : Transition
    {
        static ScanMianSingleTransition()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(ScanMianSingleTransition), new FrameworkPropertyMetadata(true));
        }


        public AyTransitionDirection Direction
        {
            get { return (AyTransitionDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(AyTransitionDirection), typeof(ScanMianSingleTransition), new PropertyMetadata(AyTransitionDirection.down));

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(ScanMianSingleTransition), new UIPropertyMetadata(Duration.Automatic));


        protected internal override void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            LinearGradientBrush gb = new LinearGradientBrush();

            GradientStop gs_gs1 = new GradientStop(SolidColorBrushConverter.ToColor("#FF000000"), 0);
            GradientStop gs_gs2 = new GradientStop(SolidColorBrushConverter.ToColor("#00000000"), 0);
            gb.GradientStops.Add(gs_gs1);
            gb.GradientStops.Add(gs_gs2);

            Point StartPoint = new Point(0, 0);
            Point EndPoint = new Point(0, 1);
            switch (Direction)
            {
                case AyTransitionDirection.down:
                    StartPoint = new Point(0, 0);
                    EndPoint = new Point(0, 1);
                    break;
                case AyTransitionDirection.up:
                    StartPoint = new Point(0, 1);
                    EndPoint = new Point(0, 0);
                    break;
                case AyTransitionDirection.left:
                    StartPoint = new Point(0, 0);
                    EndPoint = new Point(1, 0);
                    break;
                case AyTransitionDirection.right:
                    StartPoint = new Point(1, 0);
                    EndPoint = new Point(0, 0);
                    break;
            }
            gb.StartPoint = StartPoint;
            gb.EndPoint = EndPoint;
            oldContent.OpacityMask = gb;

            DoubleAnimation da1 = new DoubleAnimation();
            da1.From = 0;
            da1.To = 1;
            da1.Duration = Duration;
            gs_gs1.BeginAnimation(GradientStop.OffsetProperty, da1);
            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = 1;
            da2.Duration = Duration;
            da2.BeginTime = TimeSpan.FromMilliseconds(50);
            gs_gs2.BeginAnimation(GradientStop.OffsetProperty, da2);
        }

    }
}
