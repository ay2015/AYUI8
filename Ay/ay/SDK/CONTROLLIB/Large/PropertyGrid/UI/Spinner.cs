using System;
using System.Windows;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Base class for controls that represents controls that can spin.</summary>
	public abstract class Spinner : Control
	{
		/// <summary>Identifies the ValidSpinDirection dependency property.</summary>
		public static readonly DependencyProperty ValidSpinDirectionProperty = DependencyProperty.Register("ValidSpinDirection", typeof(ValidSpinDirections), typeof(Spinner), new PropertyMetadata(ValidSpinDirections.Increase | ValidSpinDirections.Decrease, OnValidSpinDirectionPropertyChanged));

		public static readonly RoutedEvent SpinnerSpinEvent = EventManager.RegisterRoutedEvent("SpinnerSpin", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Spinner));

		/// <summary>Gets or sets the valid spin directions.</summary>
		public ValidSpinDirections ValidSpinDirection
		{
			get
			{
				return (ValidSpinDirections)GetValue(ValidSpinDirectionProperty);
			}
			set
			{
				SetValue(ValidSpinDirectionProperty, value);
			}
		}

		/// <summary>Raised when spinning is initiated by the end-user.</summary>
		public event EventHandler<SpinEventArgs> Spin;

		/// <summary>Used as the routed event in SpinEventArgs.</summary>
		public event RoutedEventHandler SpinnerSpin
		{
			add
			{
				AddHandler(SpinnerSpinEvent, value);
			}
			remove
			{
				RemoveHandler(SpinnerSpinEvent, value);
			}
		}

		private static void OnValidSpinDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Spinner spinner = (Spinner)d;
			ValidSpinDirections oldValue = (ValidSpinDirections)e.OldValue;
			ValidSpinDirections newValue = (ValidSpinDirections)e.NewValue;
			spinner.OnValidSpinDirectionChanged(oldValue, newValue);
		}

		/// <summary>Called when the Spin event is raised.</summary>
		/// <param name="e">A SpinEventArgs containing event information.</param>
		protected virtual void OnSpin(SpinEventArgs e)
		{
			ValidSpinDirections validSpinDirections = (e.Direction == SpinDirection.Increase) ? ValidSpinDirections.Increase : ValidSpinDirections.Decrease;
			if ((ValidSpinDirection & validSpinDirections) == validSpinDirections)
			{
				EventHandler<SpinEventArgs> spin = this.Spin;
				if (spin != null)
				{
					spin(this, e);
				}
			}
		}

		/// <summary>Called when ValidSpinDirection changes.</summary>
		/// <param name="oldValue">The old ValidSpinDirections value.</param>
		/// <param name="newValue">The new ValidSpinDirections value.</param>
		protected virtual void OnValidSpinDirectionChanged(ValidSpinDirections oldValue, ValidSpinDirections newValue)
		{
		}
	}
}
