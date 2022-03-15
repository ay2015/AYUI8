using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Base class of the numerical up-down controls.</summary>
	/// <typeparam name="T">The generic type of the class.</typeparam>
	public abstract class CommonNumericUpDown<T> : NumericUpDown<T?> where T : struct, IFormattable, IComparable<T>
	{
		protected delegate bool FromText(string s, NumberStyles style, IFormatProvider provider, out T result);

		protected delegate T FromDecimal(decimal d);

		private FromText _fromText;

		private FromDecimal _fromDecimal;

		private Func<T, T, bool> _fromLowerThan;

		private Func<T, T, bool> _fromGreaterThan;

		internal static readonly DependencyProperty IsInvalidProperty = DependencyProperty.Register("IsInvalid", typeof(bool), typeof(CommonNumericUpDown<T>), new UIPropertyMetadata(false));

		/// <summary>Identifies the ParsingNumberStyle dependency property.</summary>
		public static readonly DependencyProperty ParsingNumberStyleProperty = DependencyProperty.Register("ParsingNumberStyle", typeof(NumberStyles), typeof(CommonNumericUpDown<T>), new UIPropertyMetadata(NumberStyles.Any));

		internal bool IsInvalid
		{
			get
			{
				return (bool)GetValue(IsInvalidProperty);
			}
			private set
			{
				SetValue(IsInvalidProperty, value);
			}
		}

		/// <summary>Gets or sets a value representing the number style of the numeric up-down control.</summary>
		public NumberStyles ParsingNumberStyle
		{
			get
			{
				return (NumberStyles)GetValue(ParsingNumberStyleProperty);
			}
			set
			{
				SetValue(ParsingNumberStyleProperty, value);
			}
		}

		protected CommonNumericUpDown(FromText fromText, FromDecimal fromDecimal, Func<T, T, bool> fromLowerThan, Func<T, T, bool> fromGreaterThan)
		{
			if (fromText == null)
			{
				throw new ArgumentNullException("tryParseMethod");
			}
			if (fromDecimal == null)
			{
				throw new ArgumentNullException("fromDecimal");
			}
			if (fromLowerThan == null)
			{
				throw new ArgumentNullException("fromLowerThan");
			}
			if (fromGreaterThan == null)
			{
				throw new ArgumentNullException("fromGreaterThan");
			}
			_fromText = fromText;
			_fromDecimal = fromDecimal;
			_fromLowerThan = fromLowerThan;
			_fromGreaterThan = fromGreaterThan;
		}

		protected static void UpdateMetadata(Type type, T? increment, T? minValue, T? maxValue)
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
			UpdateMetadataCommon(type, increment, minValue, maxValue);
		}

		protected void TestInputSpecialValue(AllowedSpecialValues allowedValues, AllowedSpecialValues valueToCompare)
		{
			if ((allowedValues & valueToCompare) != valueToCompare)
			{
				switch (valueToCompare)
				{
				case AllowedSpecialValues.NaN | AllowedSpecialValues.PositiveInfinity:
					break;
				case AllowedSpecialValues.NaN:
					throw new InvalidDataException("Value to parse shouldn't be NaN.");
				case AllowedSpecialValues.PositiveInfinity:
					throw new InvalidDataException("Value to parse shouldn't be Positive Infinity.");
				case AllowedSpecialValues.NegativeInfinity:
					throw new InvalidDataException("Value to parse shouldn't be Negative Infinity.");
				}
			}
		}

		internal bool IsBetweenMinMax(T? value)
		{
			if (!IsLowerThan(value, base.Minimum))
			{
				return !IsGreaterThan(value, base.Maximum);
			}
			return false;
		}

		private static void UpdateMetadataCommon(Type type, T? increment, T? minValue, T? maxValue)
		{
			NumericUpDown<T?>.IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
			UpDownBase<T?>.MaximumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
			UpDownBase<T?>.MinimumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
		}

		private bool IsLowerThan(T? value1, T? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return _fromLowerThan(value1.Value, value2.Value);
		}

		private bool IsGreaterThan(T? value1, T? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return _fromGreaterThan(value1.Value, value2.Value);
		}

		private bool HandleNullSpin()
		{
			if (!base.Value.HasValue)
			{
				T value = base.DefaultValue.HasValue ? base.DefaultValue.Value : default(T);
				base.Value = CoerceValueMinMax(value);
				return true;
			}
			if (!base.Increment.HasValue)
			{
				return true;
			}
			return false;
		}

		private T? CoerceValueMinMax(T value)
		{
			if (IsLowerThan(value, base.Minimum))
			{
				return base.Minimum;
			}
			if (IsGreaterThan(value, base.Maximum))
			{
				return base.Maximum;
			}
			return value;
		}

		protected override void OnIncrement()
		{
			if (!HandleNullSpin())
			{
				if (base.UpdateValueOnEnterKey)
				{
					T value = IncrementValue(ConvertTextToValue(base.TextBox.Text).Value, base.Increment.Value);
					T? val = CoerceValueMinMax(value);
					base.TextBox.Text = val.Value.ToString(base.FormatString, base.CultureInfo);
				}
				else
				{
					T value2 = IncrementValue(base.Value.Value, base.Increment.Value);
					base.Value = CoerceValueMinMax(value2);
				}
			}
		}

		protected override void OnDecrement()
		{
			if (!HandleNullSpin())
			{
				if (base.UpdateValueOnEnterKey)
				{
					T value = DecrementValue(ConvertTextToValue(base.TextBox.Text).Value, base.Increment.Value);
					T? val = CoerceValueMinMax(value);
					base.TextBox.Text = val.Value.ToString(base.FormatString, base.CultureInfo);
				}
				else
				{
					T value2 = DecrementValue(base.Value.Value, base.Increment.Value);
					base.Value = CoerceValueMinMax(value2);
				}
			}
		}

		protected override void OnMinimumChanged(T? oldValue, T? newValue)
		{
			base.OnMinimumChanged(oldValue, newValue);
			if (base.Value.HasValue && base.ClipValueToMinMax)
			{
				base.Value = CoerceValueMinMax(base.Value.Value);
			}
		}

		protected override void OnMaximumChanged(T? oldValue, T? newValue)
		{
			base.OnMaximumChanged(oldValue, newValue);
			if (base.Value.HasValue && base.ClipValueToMinMax)
			{
				base.Value = CoerceValueMinMax(base.Value.Value);
			}
		}

		protected override T? ConvertTextToValue(string text)
		{
			T? result = null;
			if (string.IsNullOrEmpty(text))
			{
				return result;
			}
			string text2 = ConvertValueToText();
			if (object.Equals(text2, text))
			{
				IsInvalid = false;
				return base.Value;
			}
			result = ConvertTextToValueCore(text2, text);
			if (base.ClipValueToMinMax)
			{
				return GetClippedMinMaxValue(result);
			}
			ValidateDefaultMinMax(result);
			return result;
		}

		protected override string ConvertValueToText()
		{
			if (!base.Value.HasValue)
			{
				return string.Empty;
			}
			IsInvalid = false;
			if (base.FormatString.Contains("{0"))
			{
				return string.Format(base.CultureInfo, base.FormatString, new object[1]
				{
					base.Value.Value
				});
			}
			return base.Value.Value.ToString(base.FormatString, base.CultureInfo);
		}

		protected override void SetValidSpinDirection()
		{
			ValidSpinDirections validSpinDirections = ValidSpinDirections.None;
			if (base.Increment.HasValue && !base.IsReadOnly)
			{
				if (IsLowerThan(base.Value, base.Maximum) || !base.Value.HasValue || !base.Maximum.HasValue)
				{
					validSpinDirections |= ValidSpinDirections.Increase;
				}
				if (IsGreaterThan(base.Value, base.Minimum) || !base.Value.HasValue || !base.Minimum.HasValue)
				{
					validSpinDirections |= ValidSpinDirections.Decrease;
				}
			}
			if (base.Spinner != null)
			{
				base.Spinner.ValidSpinDirection = validSpinDirections;
			}
		}

		private bool IsPercent(string stringToTest)
		{
			int num = stringToTest.IndexOf("P");
			if (num >= 0)
			{
				bool flag = stringToTest.Substring(0, num).Contains("'") && stringToTest.Substring(num, base.FormatString.Length - num).Contains("'");
				return !flag;
			}
			return false;
		}

		private T? ConvertTextToValueCore(string currentValueText, string text)
		{
			T? result;
			if (IsPercent(base.FormatString))
			{
				result = _fromDecimal(NumericUpDown<T?>.ParsePercent(text, base.CultureInfo));
			}
			else
			{
				T result2 = default(T);
				if (!_fromText(text, ParsingNumberStyle, base.CultureInfo, out result2))
				{
					bool flag = true;
					T result3;
					if (!_fromText(currentValueText, ParsingNumberStyle, base.CultureInfo, out result3))
					{
						IEnumerable<char> enumerable = from c in currentValueText
						where !char.IsDigit(c)
						select c;
						if (enumerable.Count() > 0)
						{
							IEnumerable<char> enumerable2 = from c in text
							where !char.IsDigit(c)
							select c;
							if (enumerable.Except(enumerable2).ToList().Count == 0)
							{
								foreach (char item in enumerable2)
								{
									text = text.Replace(item.ToString(), string.Empty);
								}
								if (_fromText(text, ParsingNumberStyle, base.CultureInfo, out result2))
								{
									flag = false;
								}
							}
						}
					}
					if (flag)
					{
						IsInvalid = true;
						throw new InvalidDataException("Input string was not in a correct format.");
					}
				}
				result = result2;
			}
			return result;
		}

		private T? GetClippedMinMaxValue(T? result)
		{
			if (IsGreaterThan(result, base.Maximum))
			{
				return base.Maximum;
			}
			if (IsLowerThan(result, base.Minimum))
			{
				return base.Minimum;
			}
			return result;
		}

		private void ValidateDefaultMinMax(T? value)
		{
			if (!object.Equals(value, base.DefaultValue))
			{
				if (IsLowerThan(value, base.Minimum))
				{
					throw new ArgumentOutOfRangeException("Minimum", string.Format("Value must be greater than MinValue of {0}", base.Minimum));
				}
				if (IsGreaterThan(value, base.Maximum))
				{
					throw new ArgumentOutOfRangeException("Maximum", string.Format("Value must be less than MaxValue of {0}", base.Maximum));
				}
			}
		}

		protected abstract T IncrementValue(T value, T increment);

		protected abstract T DecrementValue(T value, T increment);
	}
}
