using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ay.Framework.WPF.Controls
{
    public abstract class LinearGradientBrushAnimationBase : AnimationTimeline
    {
        protected LinearGradientBrushAnimationBase()
        {
        }

        public new LinearGradientBrushAnimationBase Clone()
        {
            return (LinearGradientBrushAnimationBase)base.Clone();
        }

        public sealed override object GetCurrentValue(object defaultOriginValue,
                      object defaultDestinationValue, AnimationClock animationClock)
        {
            if (defaultOriginValue == null)
            {
                throw new ArgumentNullException("defaultOriginValue");
            }
            if (defaultDestinationValue == null)
            {
                throw new ArgumentNullException("defaultDestinationValue");
            }
            return this.GetCurrentValue((LinearGradientBrush)defaultOriginValue,
                  (LinearGradientBrush)defaultDestinationValue, animationClock);
        }

        public LinearGradientBrush GetCurrentValue(LinearGradientBrush
               defaultOriginValue, LinearGradientBrush defaultDestinationValue,
               AnimationClock animationClock)
        {
            base.ReadPreamble();
            if (animationClock == null)
            {
                throw new ArgumentNullException("animationClock");
            }
            if (animationClock.CurrentState == ClockState.Stopped)
            {
                return defaultDestinationValue;
            }
            return this.GetCurrentValueCore(defaultOriginValue,
                   defaultDestinationValue, animationClock);
        }

        protected abstract LinearGradientBrush GetCurrentValueCore(LinearGradientBrush
                  defaultOriginValue, LinearGradientBrush defaultDestinationValue,
                  AnimationClock animationClock);

        // Properties
        public override Type TargetPropertyType
        {
            get { return typeof(LinearGradientBrush); }
        }
    }

    public class LinearGradientBrushAnimation : LinearGradientBrushAnimationBase
    {
        public static readonly DependencyProperty FromProperty;
        public static readonly DependencyProperty ToProperty;

        // Methods
        static LinearGradientBrushAnimation()
        {
            Type propertyType = typeof(LinearGradientBrush);
            Type ownerType = typeof(LinearGradientBrushAnimation);
            PropertyChangedCallback propertyChangedCallback =
              new PropertyChangedCallback(
              LinearGradientBrushAnimation.AnimationFunction_Changed);
            ValidateValueCallback validateValueCallback =
              new ValidateValueCallback(LinearGradientBrushAnimation.ValidateValues);
            FromProperty = DependencyProperty.Register("From", propertyType,
                           ownerType, new PropertyMetadata(null, propertyChangedCallback),
                           validateValueCallback);
            ToProperty = DependencyProperty.Register("To", propertyType,
                         ownerType, new PropertyMetadata(null, propertyChangedCallback),
                         validateValueCallback);
        }

        private static bool ValidateValues(object value)
        {
            return true;
        }

        private static void AnimationFunction_Changed(DependencyObject d,
                            DependencyPropertyChangedEventArgs e)
        {
            LinearGradientBrushAnimation animation = (LinearGradientBrushAnimation)d;
            //animation.PropertyChanged(e.Property);
        }

        public LinearGradientBrushAnimation()
        {
        }

        public LinearGradientBrushAnimation(LinearGradientBrush fromValue,
               LinearGradientBrush toValue, Duration duration)
            : this()
        {
            this.From = fromValue;
            this.To = toValue;
            base.Duration = duration;
        }

        protected override LinearGradientBrush GetCurrentValueCore(LinearGradientBrush
                  defaultOriginValue, LinearGradientBrush defaultDestinationValue,
                  AnimationClock animationClock)
        {
            // check for length of from and to
            if (From.GradientStops.Count != To.GradientStops.Count)
                return From;

            if (animationClock.CurrentProgress == null)
                return From;

            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = From.StartPoint + ((To.StartPoint - From.StartPoint) *
                              (double)animationClock.CurrentProgress);
            brush.EndPoint = From.EndPoint + ((To.EndPoint - From.EndPoint) *
                            (double)animationClock.CurrentProgress);

            // calc gradientstops
            for (int cnt = 0; cnt < From.GradientStops.Count; cnt++)
            {
                GradientStop stop1 = From.GradientStops[cnt];
                GradientStop stop2 = To.GradientStops[cnt];

                // calc color
                Color color1 = stop1.Color;
                Color color2 = stop2.Color;
                Color newColor = Color.Subtract(color2, color1);
                newColor = Color.Multiply(newColor,
                                (float)animationClock.CurrentProgress);
                newColor = Color.Add(newColor, color1);

                // calc offset
                double offset1 = (double)stop1.Offset;
                double offset2 = (double)stop2.Offset;
                double offset = offset1 + ((offset2 - offset1) *
                               (double)animationClock.CurrentProgress);

                brush.GradientStops.Add(new GradientStop(newColor, offset));
            }
            return brush;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new LinearGradientBrushAnimation();
        }

        // Properties
        public LinearGradientBrush From
        {
            get { return (LinearGradientBrush)base.GetValue(FromProperty); }
            set { base.SetValue(FromProperty, value); }
        }

        public LinearGradientBrush To
        {
            get { return (LinearGradientBrush)base.GetValue(ToProperty); }
            set { base.SetValue(ToProperty, value); }
        }
    }




    //ay test
 //   <Label Content = "Label 1" Margin="8" 
 // HorizontalContentAlignment="Center" 
 // VerticalContentAlignment="Center" 
 // Height="121" Width="131" 
 // Background="{StaticResource ButtonNormalBackground1}" 
 // Foreground="White">
 //   <Label.Triggers>
 //       <EventTrigger RoutedEvent = "Button.MouseEnter" >
 //           < BeginStoryboard >
 //               < Storyboard >
 //                   < res:LinearGradiantBrushAnimation
 //                       Storyboard.TargetProperty="Background"
 //                      Duration= "00:00:0.3"
 
 //                       AutoReverse= "False"
 
 //                       From= "{StaticResource ButtonNormalBackground1}"
 
 //                       To= "{StaticResource ButtonNormalBackgroundHover1}" />
 
 //                </ Storyboard >
 
 //            </ BeginStoryboard >
 
 //        </ EventTrigger >
 
 //        < EventTrigger RoutedEvent= "Button.MouseLeave" >
 
 //            < BeginStoryboard >
 
 //                < Storyboard >
 
 //                    < res:LinearGradiantBrushAnimation
 //                       Storyboard.TargetProperty= "Background"
 
 //                       Duration= "00:00:0.3"
 
 //                       AutoReverse= "False"
 
 //                       From= "{StaticResource ButtonNormalBackgroundHover1}"
 
 //                       To= "{StaticResource ButtonNormalBackground1}" />
 
 //                </ Storyboard >
 
 //            </ BeginStoryboard >
 
 //        </ EventTrigger >
 
 //    </ Label.Triggers >
 //</ Label >
}
