using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	[TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
	[TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
	[TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
	[TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
	public class CirclePointRingLoading : Control
	{
		private string StateActive = "Active";

		private string StateInActive = "InActive";

		private string StateLarge = "Large";

		private string StateSmall = "Small";

		public static readonly DependencyProperty BindableWidthProperty;

		public static readonly DependencyProperty IsActiveProperty;

		public static readonly DependencyProperty IsLargeProperty;

		public static readonly DependencyProperty MaxSideLengthProperty;

		public static readonly DependencyProperty EllipseDiameterProperty;

		public static readonly DependencyProperty EllipseOffsetProperty;

		public double BindableWidth
		{
			get
			{
				return (double)GetValue(BindableWidthProperty);
			}
			private set
			{
				SetValue(BindableWidthProperty, value);
			}
		}

		public bool IsActive
		{
			get
			{
				return (bool)GetValue(IsActiveProperty);
			}
			set
			{
				SetValue(IsActiveProperty, value);
			}
		}

		public bool IsLarge
		{
			get
			{
				return (bool)GetValue(IsLargeProperty);
			}
			set
			{
				SetValue(IsLargeProperty, value);
			}
		}

		public double MaxSideLength
		{
			get
			{
				return (double)GetValue(MaxSideLengthProperty);
			}
			set
			{
				SetValue(MaxSideLengthProperty, value);
			}
		}

		public double EllipseDiameter
		{
			get
			{
				return (double)GetValue(EllipseDiameterProperty);
			}
			set
			{
				SetValue(EllipseDiameterProperty, value);
			}
		}

		public Thickness EllipseOffset
		{
			get
			{
				return (Thickness)GetValue(EllipseOffsetProperty);
			}
			set
			{
				SetValue(EllipseOffsetProperty, value);
			}
		}

		private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			CirclePointRingLoading circlePointRingLoading = dependencyObject as CirclePointRingLoading;
			if (circlePointRingLoading != null)
			{
				circlePointRingLoading.SetEllipseDiameter((double)e.NewValue);
				circlePointRingLoading.SetEllipseOffset((double)e.NewValue);
				circlePointRingLoading.SetMaxSideLength((double)e.NewValue);
			}
		}

		private void SetEllipseDiameter(double width)
		{
			EllipseDiameter = width / 8.0;
		}

		private void SetEllipseOffset(double width)
		{
			EllipseOffset = new Thickness(0.0, width / 2.0, 0.0, 0.0);
		}

		private void SetMaxSideLength(double width)
		{
			MaxSideLength = ((width <= 20.0) ? 20.0 : width);
		}

		private static void IsActiveChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			CirclePointRingLoading circlePointRingLoading = sender as CirclePointRingLoading;
			if (circlePointRingLoading != null)
			{
				circlePointRingLoading.UpdateActiveState();
			}
		}

		private void UpdateActiveState()
		{
			if (IsActive)
			{
				VisualStateManager.GoToState(this, StateActive, true);
			}
			else
			{
				VisualStateManager.GoToState(this, StateInActive, true);
			}
		}

		private static void IsLargeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			CirclePointRingLoading circlePointRingLoading = sender as CirclePointRingLoading;
			if (circlePointRingLoading != null)
			{
				circlePointRingLoading.UpdateLargeState();
			}
		}

		private void UpdateLargeState()
		{
			if (IsLarge)
			{
				VisualStateManager.GoToState(this, StateLarge, true);
			}
			else
			{
				VisualStateManager.GoToState(this, StateSmall, true);
			}
		}

		public CirclePointRingLoading()
		{
			base.SizeChanged += delegate
			{
				BindableWidth = base.ActualWidth;
			};
		}

		static CirclePointRingLoading()
		{
			BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(0.0, BindableWidthCallback));
			IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(CirclePointRingLoading), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsActiveChanged));
			IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(CirclePointRingLoading), new PropertyMetadata(true, IsLargeChangedCallback));
			MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(0.0));
			EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(CirclePointRingLoading), new PropertyMetadata(0.0));
			EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(CirclePointRingLoading), new PropertyMetadata(default(Thickness)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CirclePointRingLoading), new FrameworkPropertyMetadata(typeof(CirclePointRingLoading)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(CirclePointRingLoading), new FrameworkPropertyMetadata(delegate(DependencyObject ringObject, DependencyPropertyChangedEventArgs e)
			{
				if (e.NewValue != e.OldValue)
				{
					CirclePointRingLoading circlePointRingLoading = (CirclePointRingLoading)ringObject;
					if ((Visibility)e.NewValue != 0)
					{
						circlePointRingLoading.SetCurrentValue(IsActiveProperty, false);
					}
					else
					{
						circlePointRingLoading.IsActive = true;
					}
				}
			}));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UpdateLargeState();
			UpdateActiveState();
		}
	}
}
