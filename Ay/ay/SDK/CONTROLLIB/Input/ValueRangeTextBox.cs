
using ay.Controls.Args;
using ay.Controls.Info;
using ay.Controls.Util;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace ay.Controls
{

	public class ValueRangeTextBox : AyTextBoxBase
	{
        public new string ControlID { get { return ControlGUID.ValueRangeTextBox; } }
        [Flags]
		private enum ValueRangeTextBoxFlags
		{
			IsFinalizingInitialization = 0x1,
			IsForcingText = 0x2,
			IsForcingValue = 0x4,
			IsInValueChanged = 0x8,
			IsNumericValueDataType = 0x10
		}

		public static readonly DependencyProperty BeepOnErrorProperty;

		public static readonly DependencyProperty FormatProviderProperty;

		public static readonly DependencyProperty MinValueProperty;

		public static readonly DependencyProperty MaxValueProperty;

		public static readonly DependencyProperty NullValueProperty;

		public static readonly DependencyProperty ValueProperty;

		public static readonly DependencyProperty ValueDataTypeProperty;

		private static readonly DependencyPropertyKey HasValidationErrorPropertyKey;

		public static readonly DependencyProperty HasValidationErrorProperty;

		private static readonly DependencyPropertyKey HasParsingErrorPropertyKey;

		public static readonly DependencyProperty HasParsingErrorProperty;

		private static readonly DependencyPropertyKey IsValueOutOfRangePropertyKey;

		public static readonly DependencyProperty IsValueOutOfRangeProperty;

		private BitVector32 m_flags;

		private CachedTextInfo m_imePreCompositionCachedTextInfo;

		public bool BeepOnError
		{
			get
			{
				return (bool)GetValue(BeepOnErrorProperty);
			}
			set
			{
				SetValue(BeepOnErrorProperty, value);
			}
		}

		public IFormatProvider FormatProvider
		{
			get
			{
				return (IFormatProvider)GetValue(FormatProviderProperty);
			}
			set
			{
				SetValue(FormatProviderProperty, value);
			}
		}

		public object MinValue
		{
			get
			{
				return GetValue(MinValueProperty);
			}
			set
			{
				SetValue(MinValueProperty, value);
			}
		}
		public object MaxValue
		{
			get
			{
				return GetValue(MaxValueProperty);
			}
			set
			{
				SetValue(MaxValueProperty, value);
			}
		}

		public object NullValue
		{
			get
			{
				return GetValue(NullValueProperty);
			}
			set
			{
				SetValue(NullValueProperty, value);
			}
		}

		public object Value
		{
			get
			{
				return GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public Type ValueDataType
		{
			get
			{
				return (Type)GetValue(ValueDataTypeProperty);
			}
			set
			{
				SetValue(ValueDataTypeProperty, value);
			}
		}

		public bool HasValidationError
		{
			get
			{
				return (bool)GetValue(HasValidationErrorProperty);
			}
		}

		public bool HasParsingError
		{
			get
			{
				return (bool)GetValue(HasParsingErrorProperty);
			}
		}

		public bool IsValueOutOfRange
		{
			get
			{
				return (bool)GetValue(IsValueOutOfRangeProperty);
			}
		}

		internal bool IsInValueChanged
		{
			get
			{
				return m_flags[8];
			}
			private set
			{
				m_flags[8] = value;
			}
		}

		internal bool IsForcingValue
		{
			get
			{
				return m_flags[4];
			}
			private set
			{
				m_flags[4] = value;
			}
		}

		internal bool IsForcingText
		{
			get
			{
				return m_flags[2];
			}
			private set
			{
				m_flags[2] = value;
			}
		}

		internal bool IsNumericValueDataType
		{
			get
			{
				return m_flags[16];
			}
			private set
			{
				m_flags[16] = value;
			}
		}

		internal virtual bool IsTextReadyToBeParsed
		{
			get
			{
				return true;
			}
		}

		internal bool IsInIMEComposition
		{
			get
			{
				return m_imePreCompositionCachedTextInfo != null;
			}
		}

		private bool IsFinalizingInitialization
		{
			get
			{
				return m_flags[1];
			}
			set
			{
				m_flags[1] = value;
			}
		}

		public event EventHandler<QueryTextFromValueEventArgs> QueryTextFromValue;

		public event EventHandler<QueryValueFromTextEventArgs> QueryValueFromText;

		static ValueRangeTextBox()
		{
			BeepOnErrorProperty = DependencyProperty.Register("BeepOnError", typeof(bool), typeof(ValueRangeTextBox), new UIPropertyMetadata(false));
			FormatProviderProperty = DependencyProperty.Register("FormatProvider", typeof(IFormatProvider), typeof(ValueRangeTextBox), new UIPropertyMetadata(null, FormatProviderPropertyChangedCallback));
			MinValueProperty = DependencyProperty.Register("MinValue", typeof(object), typeof(ValueRangeTextBox), new UIPropertyMetadata(null, null, MinValueCoerceValueCallback));
			MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(object), typeof(ValueRangeTextBox), new UIPropertyMetadata(null, null, MaxValueCoerceValueCallback));
			NullValueProperty = DependencyProperty.Register("NullValue", typeof(object), typeof(ValueRangeTextBox), new UIPropertyMetadata(null, NullValuePropertyChangedCallback, NullValueCoerceValueCallback));
			ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(ValueRangeTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValuePropertyChangedCallback, ValueCoerceValueCallback));
			ValueDataTypeProperty = DependencyProperty.Register("ValueDataType", typeof(Type), typeof(ValueRangeTextBox), new UIPropertyMetadata(null, ValueDataTypePropertyChangedCallback, ValueDataTypeCoerceValueCallback));
			HasValidationErrorPropertyKey = DependencyProperty.RegisterReadOnly("HasValidationError", typeof(bool), typeof(ValueRangeTextBox), new UIPropertyMetadata(false));
			HasValidationErrorProperty = HasValidationErrorPropertyKey.DependencyProperty;
			HasParsingErrorPropertyKey = DependencyProperty.RegisterReadOnly("HasParsingError", typeof(bool), typeof(ValueRangeTextBox), new UIPropertyMetadata(false));
			HasParsingErrorProperty = HasParsingErrorPropertyKey.DependencyProperty;
			IsValueOutOfRangePropertyKey = DependencyProperty.RegisterReadOnly("IsValueOutOfRange", typeof(bool), typeof(ValueRangeTextBox), new UIPropertyMetadata(false));
			IsValueOutOfRangeProperty = IsValueOutOfRangePropertyKey.DependencyProperty;
			TextBox.TextProperty.OverrideMetadata(typeof(ValueRangeTextBox), new FrameworkPropertyMetadata(null, TextCoerceValueCallback));
			TextBoxBase.AcceptsReturnProperty.OverrideMetadata(typeof(ValueRangeTextBox), new FrameworkPropertyMetadata(false, null, AcceptsReturnCoerceValueCallback));
			TextBoxBase.AcceptsTabProperty.OverrideMetadata(typeof(ValueRangeTextBox), new FrameworkPropertyMetadata(false, null, AcceptsTabCoerceValueCallback));
		}

		private static object AcceptsReturnCoerceValueCallback(DependencyObject sender, object value)
		{
			if ((bool)value)
			{
				throw new NotSupportedException("The ValueRangeTextBox does not support the AcceptsReturn property.");
			}
			return false;
		}

		private static object AcceptsTabCoerceValueCallback(DependencyObject sender, object value)
		{
			if ((bool)value)
			{
				throw new NotSupportedException("The ValueRangeTextBox does not support the AcceptsTab property.");
			}
			return false;
		}

		private static void FormatProviderPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ValueRangeTextBox valueRangeTextBox = (ValueRangeTextBox)sender;
			if (valueRangeTextBox.IsInitialized)
			{
				valueRangeTextBox.OnFormatProviderChanged();
			}
		}

		internal virtual void OnFormatProviderChanged()
		{
			RefreshConversionHelpers();
			RefreshCurrentText(false);
			RefreshValue();
		}

		private static object MinValueCoerceValueCallback(DependencyObject sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value == null)
			{
				return value;
			}
			Type valueDataType = valueRangeTextBox.ValueDataType;
			if (valueDataType == null)
			{
				throw new InvalidOperationException("An attempt was made to set a minimum value when the ValueDataType property is null.");
			}
			if (valueRangeTextBox.IsFinalizingInitialization)
			{
				value = ConvertValueToDataType(value, valueRangeTextBox.ValueDataType);
			}
			if (value.GetType() != valueDataType)
			{
				throw new ArgumentException("The value is not of type " + valueDataType.Name + ".", "MinValue");
			}
			IComparable comparable = value as IComparable;
			if (comparable == null)
			{
				throw new InvalidOperationException("MinValue does not implement the IComparable interface.");
			}
			object maxValue = valueRangeTextBox.MaxValue;
			valueRangeTextBox.ValidateValueInRange(value, maxValue, valueRangeTextBox.Value);
			return value;
		}

		private static object MaxValueCoerceValueCallback(DependencyObject sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value == null)
			{
				return value;
			}
			Type valueDataType = valueRangeTextBox.ValueDataType;
			if (valueDataType == null)
			{
				throw new InvalidOperationException("An attempt was made to set a maximum value when the ValueDataType property is null.");
			}
			if (valueRangeTextBox.IsFinalizingInitialization)
			{
				value = ConvertValueToDataType(value, valueRangeTextBox.ValueDataType);
			}
			if (value.GetType() != valueDataType)
			{
				throw new ArgumentException("The value is not of type " + valueDataType.Name + ".", "MinValue");
			}
			IComparable comparable = value as IComparable;
			if (comparable == null)
			{
				throw new InvalidOperationException("MaxValue does not implement the IComparable interface.");
			}
			object minValue = valueRangeTextBox.MinValue;
			valueRangeTextBox.ValidateValueInRange(minValue, value, valueRangeTextBox.Value);
			return value;
		}

		private static object NullValueCoerceValueCallback(DependencyObject sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value == null || value == DBNull.Value)
			{
				return value;
			}
			Type valueDataType = valueRangeTextBox.ValueDataType;
			if (valueDataType == null)
			{
				throw new InvalidOperationException("An attempt was made to set a null value when the ValueDataType property is null.");
			}
			if (valueRangeTextBox.IsFinalizingInitialization)
			{
				value = ConvertValueToDataType(value, valueRangeTextBox.ValueDataType);
			}
			if (value.GetType() != valueDataType)
			{
				throw new ArgumentException("The value is not of type " + valueDataType.Name + ".", "NullValue");
			}
			return value;
		}

		private static void NullValuePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (e.OldValue == null)
			{
				if (valueRangeTextBox.Value == null)
				{
					valueRangeTextBox.RefreshValue();
				}
			}
			else if (e.OldValue.Equals(valueRangeTextBox.Value))
			{
				valueRangeTextBox.RefreshValue();
			}
		}

		private static object ValueCoerceValueCallback(object sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (valueRangeTextBox.IsFinalizingInitialization)
			{
				value = ConvertValueToDataType(value, valueRangeTextBox.ValueDataType);
			}
			if (!valueRangeTextBox.IsForcingValue)
			{
				valueRangeTextBox.ValidateValue(value);
			}
			return value;
		}

		private static void ValuePropertyChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsForcingValue && !object.Equals(e.NewValue, e.OldValue))
			{
				valueRangeTextBox.IsInValueChanged = true;
				try
				{
					valueRangeTextBox.Text = valueRangeTextBox.GetTextFromValue(e.NewValue);
				}
				finally
				{
					valueRangeTextBox.IsInValueChanged = false;
				}
			}
		}

		private static object ValueDataTypeCoerceValueCallback(DependencyObject sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (valueRangeTextBox.IsInitialized)
			{
				Type type = value as Type;
				try
				{
					valueRangeTextBox.ValidateDataType(type);
					return value;
				}
				catch (Exception innerException)
				{
					throw new ArgumentException("An error occured while trying to change the ValueDataType.", innerException);
				}
			}
			return DependencyProperty.UnsetValue;
		}

		private static void ValueDataTypePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			Type type = e.NewValue as Type;
			valueRangeTextBox.IsNumericValueDataType = IsNumericType(type);
			valueRangeTextBox.RefreshConversionHelpers();
			valueRangeTextBox.ConvertValuesToDataType(type);
		}

		internal virtual void ValidateDataType(Type type)
		{
			if (!(type == null))
			{
				object obj = MinValue;
				if (obj != null && obj.GetType() != type)
				{
					obj = Convert.ChangeType(obj, type, CultureInfo.InvariantCulture);
				}
				object obj2 = MaxValue;
				if (obj2 != null && obj2.GetType() != type)
				{
					obj2 = Convert.ChangeType(obj2, type, CultureInfo.InvariantCulture);
				}
				object obj3 = NullValue;
				if (obj3 != null && obj3 != DBNull.Value && obj3.GetType() != type)
				{
					obj3 = Convert.ChangeType(obj3, type, CultureInfo.InvariantCulture);
				}
				object value = Value;
				if (value != null && value != DBNull.Value && value.GetType() != type)
				{
					value = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
				}
				if (obj != null || obj2 != null || (obj3 != null && obj3 != DBNull.Value))
				{
					Type @interface = type.GetInterface("IComparable");
					if (@interface == null)
					{
						throw new InvalidOperationException("MinValue, MaxValue, and NullValue must implement the IComparable interface.");
					}
				}
			}
		}

		private void ConvertValuesToDataType(Type type)
		{
			if (type == null)
			{
				MinValue = null;
				MaxValue = null;
				NullValue = null;
				Value = null;
			}
			else
			{
				object minValue = MinValue;
				if (minValue != null && minValue.GetType() != type)
				{
					MinValue = ConvertValueToDataType(minValue, type);
				}
				object maxValue = MaxValue;
				if (maxValue != null && maxValue.GetType() != type)
				{
					MaxValue = ConvertValueToDataType(maxValue, type);
				}
				object nullValue = NullValue;
				if (nullValue != null && nullValue != DBNull.Value && nullValue.GetType() != type)
				{
					NullValue = ConvertValueToDataType(nullValue, type);
				}
				object value = Value;
				if (value != null && value != DBNull.Value && value.GetType() != type)
				{
					Value = ConvertValueToDataType(value, type);
				}
			}
		}

		private static object TextCoerceValueCallback(object sender, object value)
		{
			ValueRangeTextBox valueRangeTextBox = sender as ValueRangeTextBox;
			if (!valueRangeTextBox.IsInitialized)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value == null)
			{
				return string.Empty;
			}
			return value;
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			RefreshValue();
			base.OnTextChanged(e);
		}

		private void SetHasValidationError(bool value)
		{
			SetValue(HasValidationErrorPropertyKey, value);
		}

		internal void SetHasParsingError(bool value)
		{
			SetValue(HasParsingErrorPropertyKey, value);
		}

		private void SetIsValueOutOfRange(bool value)
		{
			SetValue(IsValueOutOfRangePropertyKey, value);
		}

		internal string GetTextFromValue(object value)
		{
			string text = QueryTextFromValueCore(value);
			QueryTextFromValueEventArgs queryTextFromValueEventArgs = new QueryTextFromValueEventArgs(value, text);
			OnQueryTextFromValue(queryTextFromValueEventArgs);
			return queryTextFromValueEventArgs.Text;
		}

		protected virtual string QueryTextFromValueCore(object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return string.Empty;
			}
			IFormatProvider activeFormatProvider = GetActiveFormatProvider();
			CultureInfo cultureInfo = activeFormatProvider as CultureInfo;
			if (cultureInfo != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
				if (converter.CanConvertTo(typeof(string)))
				{
					return (string)converter.ConvertTo(null, cultureInfo, value, typeof(string));
				}
			}
			try
			{
				return Convert.ToString(value, activeFormatProvider);
			}
			catch
			{
			}
			return value.ToString();
		}

		private void OnQueryTextFromValue(QueryTextFromValueEventArgs e)
		{
			if (this.QueryTextFromValue != null)
			{
				this.QueryTextFromValue(this, e);
			}
		}

		internal object GetValueFromText(string text, out bool hasParsingError)
		{
			object value = null;
			bool flag = QueryValueFromTextCore(text, out value);
			QueryValueFromTextEventArgs queryValueFromTextEventArgs = new QueryValueFromTextEventArgs(text, value);
			queryValueFromTextEventArgs.HasParsingError = !flag;
			OnQueryValueFromText(queryValueFromTextEventArgs);
			hasParsingError = queryValueFromTextEventArgs.HasParsingError;
			return queryValueFromTextEventArgs.Value;
		}

		protected virtual bool QueryValueFromTextCore(string text, out object value)
		{
			value = null;
			Type valueDataType = ValueDataType;
			text = text.Trim();
			if (valueDataType == null)
			{
				return true;
			}
			if (!valueDataType.IsValueType && valueDataType != typeof(string))
			{
				return false;
			}
			try
			{
				value = ChangeTypeHelper.ChangeType(text, valueDataType, GetActiveFormatProvider());
			}
			catch
			{
				if (BeepOnError)
				{
					SystemSounds.Beep.Play();
				}
				return false;
			}
			return true;
		}

		private void OnQueryValueFromText(QueryValueFromTextEventArgs e)
		{
			if (this.QueryValueFromText != null)
			{
				this.QueryValueFromText(this, e);
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.ImeProcessedKey != 0 && !IsInIMEComposition)
			{
				StartIMEComposition();
			}
			base.OnPreviewKeyDown(e);
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			RefreshCurrentText(true);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			RefreshCurrentText(true);
		}

		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			if (IsInIMEComposition)
			{
				EndIMEComposition();
			}
			base.OnTextInput(e);
		}

		protected virtual void ValidateValue(object value)
		{
			if (value != null)
			{
				Type type = ValueDataType;
				if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					NullableConverter nullableConverter = new NullableConverter(type);
					type = nullableConverter.UnderlyingType;
				}
				if (type == null)
				{
					throw new InvalidOperationException("An attempt was made to set a value when the ValueDataType property is null.");
				}
				if (value != DBNull.Value && value.GetType() != type)
				{
					throw new ArgumentException("The value is not of type " + type.Name + ".", "Value");
				}
				ValidateValueInRange(MinValue, MaxValue, value);
			}
		}

		internal static bool IsNumericType(Type type)
		{
			if (type == null)
			{
				return false;
			}
			if (type.IsValueType && (type == typeof(int) || type == typeof(double) || type == typeof(decimal) || type == typeof(float) || type == typeof(short) || type == typeof(long) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong) || type == typeof(byte)))
			{
				return true;
			}
			return false;
		}

		internal void StartIMEComposition()
		{
			m_imePreCompositionCachedTextInfo = new CachedTextInfo(this);
		}

		internal void EndIMEComposition()
		{
			CachedTextInfo cachedTextInfo = m_imePreCompositionCachedTextInfo.Clone() as CachedTextInfo;
			m_imePreCompositionCachedTextInfo = null;
			OnIMECompositionEnded(cachedTextInfo);
		}

		internal virtual void OnIMECompositionEnded(CachedTextInfo cachedTextInfo)
		{
		}

		internal virtual void RefreshConversionHelpers()
		{
		}

		internal IFormatProvider GetActiveFormatProvider()
		{
			IFormatProvider formatProvider = FormatProvider;
			if (formatProvider != null)
			{
				return formatProvider;
			}
			return CultureInfo.CurrentCulture;
		}

		internal CultureInfo GetCultureInfo()
		{
			CultureInfo cultureInfo = GetActiveFormatProvider() as CultureInfo;
			if (cultureInfo != null)
			{
				return cultureInfo;
			}
			return CultureInfo.CurrentCulture;
		}

		internal virtual string GetCurrentText()
		{
			return base.Text;
		}

		internal virtual string GetParsableText()
		{
			return base.Text;
		}

		internal void ForceText(string text, bool preserveCaret)
		{
			IsForcingText = true;
			try
			{
				int caretIndex = base.CaretIndex;
				base.Text = text;
				if (preserveCaret && base.IsLoaded)
				{
					try
					{
						base.SelectionStart = caretIndex;
					}
					catch (NullReferenceException)
					{
					}
				}
			}
			finally
			{
				IsForcingText = false;
			}
		}

		internal bool IsValueNull(object value)
		{
			if (value == null || value == DBNull.Value)
			{
				return true;
			}
			Type type = ValueDataType;
			if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				NullableConverter nullableConverter = new NullableConverter(type);
				type = nullableConverter.UnderlyingType;
			}
			if (value.GetType() != type)
			{
				value = Convert.ChangeType(value, type);
			}
			object obj = NullValue;
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != type)
			{
				obj = Convert.ChangeType(obj, type);
			}
			return obj.Equals(value);
		}

		internal void ForceValue(object value)
		{
			IsForcingValue = true;
			try
			{
				Value = value;
			}
			finally
			{
				IsForcingValue = false;
			}
		}

		internal void RefreshCurrentText(bool preserveCurrentCaretPosition)
		{
			string currentText = GetCurrentText();
			if (!string.Equals(currentText, base.Text))
			{
				ForceText(currentText, preserveCurrentCaretPosition);
			}
		}

		internal void RefreshValue()
		{
			if (!IsForcingValue && !(ValueDataType == null) && !IsInIMEComposition)
			{
				object obj;
				bool hasParsingError;
				if (IsTextReadyToBeParsed)
				{
					string parsableText = GetParsableText();
					obj = GetValueFromText(parsableText, out hasParsingError);
					if (IsValueNull(obj))
					{
						obj = NullValue;
					}
				}
				else
				{
					hasParsingError = !GetIsEditTextEmpty();
					obj = NullValue;
				}
				SetHasParsingError(hasParsingError);
				bool hasValidationError = hasParsingError;
				try
				{
					ValidateValue(obj);
					SetIsValueOutOfRange(false);
				}
				catch (Exception ex)
				{
					hasValidationError = true;
					if (BeepOnError)
					{
						SystemSounds.Beep.Play();
					}
					if (ex is ArgumentOutOfRangeException)
					{
						SetIsValueOutOfRange(true);
					}
					obj = NullValue;
				}
				if (!object.Equals(obj, Value))
				{
					ForceValue(obj);
				}
				SetHasValidationError(hasValidationError);
			}
		}

		internal virtual bool GetIsEditTextEmpty()
		{
			return base.Text == string.Empty;
		}

		private static object ConvertValueToDataType(object value, Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (value != null && value != DBNull.Value && value.GetType() != type)
			{
				return ChangeTypeHelper.ChangeType(value, type, CultureInfo.InvariantCulture);
			}
			return value;
		}

		private void CanEnterLineBreak(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
			e.Handled = true;
		}

		private void CanEnterParagraphBreak(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
			e.Handled = true;
		}

		private void ValidateValueInRange(object minValue, object maxValue, object value)
		{
			if (!IsValueNull(value))
			{
				Type type = ValueDataType;
				if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					NullableConverter nullableConverter = new NullableConverter(type);
					type = nullableConverter.UnderlyingType;
				}
				if (value.GetType() != type)
				{
					value = Convert.ChangeType(value, type);
				}
				if (minValue != null)
				{
					IComparable comparable = (IComparable)minValue;
					if (maxValue != null && comparable.CompareTo(maxValue) > 0)
					{
						throw new ArgumentOutOfRangeException("minValue", "MaxValue must be greater than MinValue.");
					}
					if (comparable.CompareTo(value) > 0)
					{
						throw new ArgumentOutOfRangeException("minValue", "Value must be greater than MinValue.");
					}
				}
				if (maxValue != null)
				{
					IComparable comparable2 = (IComparable)maxValue;
					if (comparable2.CompareTo(value) < 0)
					{
						throw new ArgumentOutOfRangeException("maxValue", "Value must be less than MaxValue.");
					}
				}
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			IsFinalizingInitialization = true;
			try
			{
				CoerceValue(ValueDataTypeProperty);
				IsNumericValueDataType = IsNumericType(ValueDataType);
				RefreshConversionHelpers();
				CoerceValue(MinValueProperty);
				CoerceValue(MaxValueProperty);
				CoerceValue(ValueProperty);
				CoerceValue(NullValueProperty);
				CoerceValue(TextBox.TextProperty);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Initialization of the ValueRangeTextBox failed.", innerException);
			}
			finally
			{
				IsFinalizingInitialization = false;
			}
			base.OnInitialized(e);
		}
	}
}
