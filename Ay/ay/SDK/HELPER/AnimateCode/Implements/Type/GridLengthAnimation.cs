/*
    <local:GridLengthAnimation Storyboard.TargetProperty="(ColumnDefinition.Width)" Storyboard.TargetName="ColumnDefinition1" 
															   To="*" BeginTime="0:0:0"/>
 		<FrameworkElement x:Name="ProxyElement" DataContext="{Binding TemplateSettings, RelativeSource={RelativeSource Mode=TemplatedParent}}"
										  Width="0" Height="0" Visibility="Collapsed"
										  />
                                          <local:GridLengthAnimation Storyboard.TargetProperty="(ColumnDefinition.Width)" Storyboard.TargetName="ColumnDefinition1" 
																   To="{Binding DataContext.CompactPaneGridLength, FallbackValue=0, ElementName=ProxyElement}" BeginTime="0:0:0"/>
 
                         <Int32Animation Storyboard.TargetProperty="(Grid.Column)" Storyboard.TargetName="ContentRoot" To="1" BeginTime="0:0:0" />
											<Int32Animation Storyboard.TargetProperty="(Grid.ColumnSpan)" Storyboard.TargetName="ContentRoot" To="1" BeginTime="0:0:0" />
											<ObjectAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Visibility)" Storyboard.TargetName="PaneRoot">
												<DiscreteObjectKeyFrame KeyTime="0:0:0" Value="{x:Static Visibility.Visible}"/>
											</ObjectAnimationUsingKeyFrames>
                                GridLengthAnimation gla = new GridLengthAnimation();
                gla.From = new GridLength(bSingleImageMode 
                    ? 0 : 1, GridUnitType.Star);
                gla.To = new GridLength(bSingleImageMode 
                    ? 1 : 0, GridUnitType.Star);
                gla.Duration = new TimeSpan(0, 0, 2);
                mainGrid.ColumnDefinitions[indexCol].BeginAnimation(
                    ColumnDefinition.WidthProperty, gla);                                    
 */

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace ay.Animate
{
    public class GridLengthAnimation : AnimationTimeline
	{
		public override object GetCurrentValue(object defaultOriginValue,
			object defaultDestinationValue, AnimationClock animationClock)
		{
			var from = ((GridLength)GetValue(GridLengthAnimation.FromProperty));
			var to = ((GridLength)GetValue(GridLengthAnimation.ToProperty));
			if (from.GridUnitType != to.GridUnitType) //We can't animate different types, so just skip straight to it
				return to;
			double fromVal = from.Value;
			double toVal = to.Value;

			if (fromVal > toVal)
			{
				return new GridLength((1 - animationClock.CurrentProgress.Value) *
					(fromVal - toVal) + toVal, GridUnitType.Star);
			}
			else
			{
				return new GridLength(animationClock.CurrentProgress.Value *
					(toVal - fromVal) + fromVal, GridUnitType.Star);
			}
		}

		public override Type TargetPropertyType
		{
			get
			{
				return typeof(GridLength);
			}
		}

		protected override System.Windows.Freezable CreateInstanceCore()
		{
			return new GridLengthAnimation();
		}

		public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));
		public GridLength From
		{
			get
			{
				return (GridLength)GetValue(GridLengthAnimation.FromProperty);
			}
			set
			{
				SetValue(GridLengthAnimation.FromProperty, value);
			}
		}
		public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));
		public GridLength To
		{
			get
			{
				return (GridLength)GetValue(GridLengthAnimation.ToProperty);
			}
			set
			{
				SetValue(GridLengthAnimation.ToProperty, value);
			}
		}
	}
}
