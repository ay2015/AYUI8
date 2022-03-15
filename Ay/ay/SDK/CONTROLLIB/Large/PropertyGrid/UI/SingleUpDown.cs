using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Represents a textbox with button spinners that allow incrementing and decrementing float values by using the spinner buttons, keyboard up/down arrows, or mouse
	/// wheel.</summary>
	public class SingleUpDown : CommonNumericUpDown<float>
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

		static SingleUpDown()
		{
			AllowInputSpecialValuesProperty = DependencyProperty.Register("AllowInputSpecialValues", typeof(AllowedSpecialValues), typeof(SingleUpDown), new UIPropertyMetadata(AllowedSpecialValues.None));
			CommonNumericUpDown<float>.UpdateMetadata(typeof(SingleUpDown), 1f, float.NegativeInfinity, float.PositiveInfinity);
		}

		/// <summary>Initializes a new instance of the SingleUpDown class.</summary>
		public SingleUpDown()
			: base((FromText)float.TryParse, (FromDecimal)decimal.ToSingle, (Func<float, float, bool>)((float v1, float v2) => v1 < v2), (Func<float, float, bool>)((float v1, float v2) => v1 > v2))
		{
		}

		protected override float? OnCoerceIncrement(float? baseValue)
		{
			if (baseValue.HasValue && float.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Increment.");
			}
			return base.OnCoerceIncrement(baseValue);
		}

		protected override float? OnCoerceMaximum(float? baseValue)
		{
			if (baseValue.HasValue && float.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Maximum.");
			}
			return base.OnCoerceMaximum(baseValue);
		}

		protected override float? OnCoerceMinimum(float? baseValue)
		{
			if (baseValue.HasValue && float.IsNaN(baseValue.Value))
			{
				throw new ArgumentException("NaN is invalid for Minimum.");
			}
			return base.OnCoerceMinimum(baseValue);
		}

		protected override float IncrementValue(float value, float increment)
		{
			return value + increment;
		}

		protected override float DecrementValue(float value, float increment)
		{
			return value - increment;
		}

		protected override void SetValidSpinDirection()
		{
			if (base.Value.HasValue && float.IsInfinity(base.Value.Value) && base.Spinner != null)
			{
				base.Spinner.ValidSpinDirection = ValidSpinDirections.None;
			}
			else
			{
				base.SetValidSpinDirection();
			}
		}

		protected override float? ConvertTextToValue(string text)
		{
			float? result = base.ConvertTextToValue(text);
			if (result.HasValue)
			{
				if (float.IsNaN(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.NaN);
				}
				else if (float.IsPositiveInfinity(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.PositiveInfinity);
				}
				else if (float.IsNegativeInfinity(result.Value))
				{
					TestInputSpecialValue(AllowInputSpecialValues, AllowedSpecialValues.NegativeInfinity);
				}
			}
			return result;
		}
	}
}
