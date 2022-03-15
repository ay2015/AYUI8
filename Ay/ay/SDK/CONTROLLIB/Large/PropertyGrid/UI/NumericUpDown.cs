using ay.Controls.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>Base class of the CommonNumericUpDown&lt;T&gt;.</summary>
	/// <typeparam name="T">The generic type of the class.</typeparam>
	public abstract class NumericUpDown<T> : UpDownBase<T>
	{
		/// <summary>Identifies the <strong>AutoMoveFocus</strong> dependency property.</summary>
		public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register("AutoMoveFocus", typeof(bool), typeof(NumericUpDown<T>), new UIPropertyMetadata(false));

        /// <summary>Identifies the <strong>AutoSelectBehavior</strong> dependency property.</summary>
        public static readonly DependencyProperty AutoSelectBehaviorProperty = DependencyProperty.Register("AutoSelectBehavior", typeof(AutoSelectBehavior), typeof(NumericUpDown<T>), new UIPropertyMetadata(AutoSelectBehavior.OnFocus));

		/// <summary>Identifies the FormatString dependency property.</summary>
		public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(NumericUpDown<T>), new UIPropertyMetadata(string.Empty, OnFormatStringChanged, OnCoerceFormatString));

		/// <summary>Identifies the Increment dependency property.</summary>
		public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(T), typeof(NumericUpDown<T>), new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));

		/// <summary>Identifies the MaxLength dependency property.</summary>
		public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(NumericUpDown<T>), new UIPropertyMetadata(0));

		public bool AutoMoveFocus
		{
			get
			{
				return (bool)GetValue(AutoMoveFocusProperty);
			}
			set
			{
				SetValue(AutoMoveFocusProperty, value);
			}
		}

		public AutoSelectBehavior AutoSelectBehavior
		{
			get
			{
				return (AutoSelectBehavior)GetValue(AutoSelectBehaviorProperty);
			}
			set
			{
				SetValue(AutoSelectBehaviorProperty, value);
			}
		}

		/// <summary>Gets or sets the dispaly format of the Value.</summary>
		public string FormatString
		{
			get
			{
				return (string)GetValue(FormatStringProperty);
			}
			set
			{
				SetValue(FormatStringProperty, value);
			}
		}

		/// <summary>Gets or sets the amount in which to increment the value.</summary>
		public T Increment
		{
			get
			{
				return (T)GetValue(IncrementProperty);
			}
			set
			{
				SetValue(IncrementProperty, value);
			}
		}

		/// <summary>
		///   <span id="BugEvents">Gets or sets the maximum number of characters that can be manually entered into the NumericUpDown.</span>
		/// </summary>
		public int MaxLength
		{
			get
			{
				return (int)GetValue(MaxLengthProperty);
			}
			set
			{
				SetValue(MaxLengthProperty, value);
			}
		}

		private static object OnCoerceFormatString(DependencyObject o, object baseValue)
		{
			NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
			if (numericUpDown != null)
			{
				return numericUpDown.OnCoerceFormatString((string)baseValue);
			}
			return baseValue;
		}

		protected virtual string OnCoerceFormatString(string baseValue)
		{
			return baseValue ?? string.Empty;
		}

		private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
			if (numericUpDown != null)
			{
				numericUpDown.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when FormatString changes.</summary>
		/// <param name="oldValue">The old string value.</param>
		/// <param name="newValue">The new string value.</param>
		protected virtual void OnFormatStringChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized)
			{
				SyncTextAndValueProperties(false, null);
			}
		}

		private static void OnIncrementChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown<T> numericUpDown = o as NumericUpDown<T>;
			if (numericUpDown != null)
			{
				numericUpDown.OnIncrementChanged((T)e.OldValue, (T)e.NewValue);
			}
		}

		protected virtual void OnIncrementChanged(T oldValue, T newValue)
		{
			if (base.IsInitialized)
			{
				SetValidSpinDirection();
			}
		}

		private static object OnCoerceIncrement(DependencyObject d, object baseValue)
		{
			NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
			if (numericUpDown != null)
			{
				return numericUpDown.OnCoerceIncrement((T)baseValue);
			}
			return baseValue;
		}

		protected virtual T OnCoerceIncrement(T baseValue)
		{
			return baseValue;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		/// <summary>Converts the string representation of a number to its equivalent as a decimal value using the specified culture-specific format information.</summary>
		/// <returns>The equivalent of <em>text</em> as a decimal value.</returns>
		/// <param name="text">The string representation of the number to convert.</param>
		/// <param name="cultureInfo">An IFormatProvider that supplies culture-specific parsing information about <em>text</em>.</param>
		protected static decimal ParsePercent(string text, IFormatProvider cultureInfo)
		{
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(cultureInfo);
			text = text.Replace(instance.PercentSymbol, null);
			decimal d = decimal.Parse(text, NumberStyles.Any, instance);
			return d / 100m;
		}
	}
}
