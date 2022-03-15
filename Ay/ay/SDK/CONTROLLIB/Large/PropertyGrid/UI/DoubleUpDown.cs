using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing double values by using the spinner buttons, keyboard up/down arrows, or
	/// mouse wheel.</summary>
	public class DoubleUpDown : CommonNumericUpDown<double>
	{
		/// <summary>Identifies the AllowInputSpecialValues dependency property.</summary>
		public static readonly DependencyProperty AllowInputSpecialValuesProperty;

		/// <summary>Gets or sets a value representing the special values the user is allowed to input, such as "Infinity", "-Infinity" and "NaN" values.</summary>
		public AllowedSpecialValues AllowInputSpecialValues
		{
			get
			{
				return (AllowedSpecialValues)GetValue(AllowInputSpecialValuesProperty);
			}
			set
			{
				SetValue(AllowInputSpecialValuesProperty, value);
			}
		}

		static DoubleUpDown()
		{
			AllowInputSpecialValuesProperty = DependencyProperty.Register("AllowInputSpecialValues", typeof(AllowedSpecialValues), typeof(DoubleUpDown), new UIPropertyMetadata(AllowedSpecialValues.None));
			CommonNumericUpDown<double>.UpdateMetadata(typeof(DoubleUpDown), 1.0, double.NegativeInfinity, double.PositiveInfinity);
		}

		/// <summary>Initializes a new instance of the DoubleUpDown class.</summary>
		public DoubleUpDown()
			: base((FromText)double.TryParse, (FromDecimal)decimal.ToDouble, (Func<double, double, bool>)((double v1, double v2) => v1 < v2), (Func<double, double, bool>)((double v1, double v2) => v1 > v2))
		{
		}

		protected override double? OnCoerceIncrement(double? baseValue)
		{
			if (baseValue.HasValue && double.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Increment.");
			}
			return base.OnCoerceIncrement(baseValue);
		}

		protected override double? OnCoerceMaximum(double? baseValue)
		{
			if (baseValue.HasValue && double.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Maximum.");
			}
			return base.OnCoerceMaximum(baseValue);
		}

		protected override double? OnCoerceMinimum(double? baseValue)
		{
			if (baseValue.HasValue && double.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Minimum.");
			}
			return base.OnCoerceMinimum(baseValue);
		}

		protected override double IncrementValue(double value, double increment)
		{
			return value + increment;
		}

		protected override double DecrementValue(double value, double increment)
		{
			return value - increment;
		}

		/// <summary>Sets the valid spin directions.</summary>
		protected override void SetValidSpinDirection()
		{
			if (base.Value.HasValue && double.IsInfinity(base.Value.Value) && base.Spinner != null)
			{
				base.Spinner.ValidSpinDirection = ValidSpinDirections.None;
			}
			else
			{
				base.SetValidSpinDirection();
			}
		}

		/// <summary>Converts the formatted text to a value.</summary>
		/// <returns>A converted decimal value.</returns>
		/// <param name="text">The string to convert.</param>
		protected override double? ConvertTextToValue(string text)
		{
			double? result = base.ConvertTextToValue(text);
			if (result.HasValue)
			{
				if (double.IsNaN(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.NaN);
				}
				else if (double.IsPositiveInfinity(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.PositiveInfinity);
				}
				else if (double.IsNegativeInfinity(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.NegativeInfinity);
				}
			}
			return result;
		}
	}
}
