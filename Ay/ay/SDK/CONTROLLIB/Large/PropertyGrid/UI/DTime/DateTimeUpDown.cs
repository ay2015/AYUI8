using System;
using System.Globalization;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>
	///   <para>Represents a control that allows a user to increment or decrement a DateTime using button spinners, up/down keys, or the mouse wheel.</para>
	/// </summary>
	public class DateTimeUpDown : DateTimeUpDownBase<DateTime?>
	{
		private DateTime? _lastValidDate;

		private bool _setKindInternal;

		/// <summary>Identifies the AutoClipTimeParts dependency property.</summary>
		public static readonly DependencyProperty AutoClipTimePartsProperty;

		/// <summary>Identifies the Format dependency property.</summary>
		public static readonly DependencyProperty FormatProperty;

		/// <summary>Identifies the FormatString dependency property.</summary>
		public static readonly DependencyProperty FormatStringProperty;

		public static readonly DependencyProperty KindProperty;

		/// <summary>Gets or sets if the hours, minutes and seconds time parts are clipped automatically.</summary>
		public bool AutoClipTimeParts
		{
			get
			{
				return (bool)GetValue(AutoClipTimePartsProperty);
			}
			set
			{
				SetValue(AutoClipTimePartsProperty, value);
			}
		}

		/// <summary>Gets or sets the date-time format used by DateTimeUpDown.</summary>
		public DateTimeFormat Format
		{
			get
			{
				return (DateTimeFormat)GetValue(FormatProperty);
			}
			set
			{
				SetValue(FormatProperty, value);
			}
		}

		/// <summary>Gets or sets the date-time format to be used by the control when Format is set to
		/// Custom.</summary>
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

		public DateTimeKind Kind
		{
			get
			{
				return (DateTimeKind)GetValue(KindProperty);
			}
			set
			{
				SetValue(KindProperty, value);
			}
		}

		internal DateTime? TempValue
		{
			get;
			set;
		}

		internal DateTime ContextNow
		{
			get
			{
				return DateTimeUtilities.GetContextNow(Kind);
			}
		}

		private static void OnFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateTimeUpDown dateTimeUpDown = o as DateTimeUpDown;
			if (dateTimeUpDown != null)
			{
				dateTimeUpDown.OnFormatChanged((DateTimeFormat)e.OldValue, (DateTimeFormat)e.NewValue);
			}
		}

		/// <summary>Called when Format changes.</summary>
		/// <param name="oldValue">The old DateTimeFormat.</param>
		/// <param name="newValue">The new DateTimeFormat.</param>
		protected virtual void OnFormatChanged(DateTimeFormat oldValue, DateTimeFormat newValue)
		{
			FormatUpdated();
		}

		internal static bool IsFormatStringValid(object value)
		{
			try
			{
				CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MinSupportedDateTime.ToString((string)value, CultureInfo.CurrentCulture);
			}
			catch
			{
				return false;
			}
			return true;
		}

		private static void OnFormatStringChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateTimeUpDown dateTimeUpDown = o as DateTimeUpDown;
			if (dateTimeUpDown != null)
			{
				dateTimeUpDown.OnFormatStringChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when FormatString changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnFormatStringChanged(string oldValue, string newValue)
		{
			FormatUpdated();
		}

		private static void OnKindChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateTimeUpDown dateTimeUpDown = o as DateTimeUpDown;
			if (dateTimeUpDown != null)
			{
				dateTimeUpDown.OnKindChanged((DateTimeKind)e.OldValue, (DateTimeKind)e.NewValue);
			}
		}

		protected virtual void OnKindChanged(DateTimeKind oldValue, DateTimeKind newValue)
		{
			if (!_setKindInternal && base.Value.HasValue && base.IsInitialized)
			{
				base.Value = ConvertToKind(base.Value.Value, newValue);
			}
		}

		private void SetKindInternal(DateTimeKind kind)
		{
			_setKindInternal = true;
			try
			{
				SetCurrentValue(KindProperty, kind);
			}
			finally
			{
				_setKindInternal = false;
			}
		}

		static DateTimeUpDown()
		{
			AutoClipTimePartsProperty = DependencyProperty.Register("AutoClipTimeParts", typeof(bool), typeof(DateTimeUpDown), new UIPropertyMetadata(false));
			FormatProperty = DependencyProperty.Register("Format", typeof(DateTimeFormat), typeof(DateTimeUpDown), new UIPropertyMetadata(DateTimeFormat.FullDateTime, OnFormatChanged));
			FormatStringProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(DateTimeUpDown), new UIPropertyMetadata(null, OnFormatStringChanged), IsFormatStringValid);
			KindProperty = DependencyProperty.Register("Kind", typeof(DateTimeKind), typeof(DateTimeUpDown), new FrameworkPropertyMetadata(DateTimeKind.Unspecified, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnKindChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeUpDown), new FrameworkPropertyMetadata(typeof(DateTimeUpDown)));
			UpDownBase<DateTime?>.MaximumProperty.OverrideMetadata(typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MaxSupportedDateTime));
			UpDownBase<DateTime?>.MinimumProperty.OverrideMetadata(typeof(DateTimeUpDown), new FrameworkPropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.Calendar.MinSupportedDateTime));
			UpDownBase<DateTime?>.UpdateValueOnEnterKeyProperty.OverrideMetadata(typeof(DateTimeUpDown), new FrameworkPropertyMetadata(true));
		}

		/// <summary>Initializes a new instance of the DateTimeUpDown class.</summary>
		public DateTimeUpDown()
		{
			base.Loaded += DateTimeUpDown_Loaded;
		}

		public override bool CommitInput()
		{
			bool result = SyncTextAndValueProperties(true, base.Text);
			_lastValidDate = base.Value;
			return result;
		}

		/// <summary>Called when the culture info changes.</summary>
		/// <param name="oldValue">The old culture info.</param>
		/// <param name="newValue">The new culture info.</param>
		protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
		{
			FormatUpdated();
		}

		/// <summary>Called by OnSpin when the spin direction is SpinDirection.Increase.</summary>
		protected override void OnIncrement()
		{
			if (IsCurrentValueValid())
			{
				Increment(base.Step);
			}
		}

		/// <summary>Called by OnSpin when the spin direction is SpinDirection.Decrease.</summary>
		protected override void OnDecrement()
		{
			if (IsCurrentValueValid())
			{
				Increment(-base.Step);
			}
		}

		/// <summary>Called when Text changes.</summary>
		/// <param name="previousValue">The previous value.</param>
		/// <param name="currentValue">The current value.</param>
		protected override void OnTextChanged(string previousValue, string currentValue)
		{
			if (_processTextChanged)
			{
				base.OnTextChanged(previousValue, currentValue);
			}
		}

		/// <summary>Converts text to a DateTime value.</summary>
		/// <returns>A converted DateTime.</returns>
		/// <param name="text">The text to convert.</param>
		protected override DateTime? ConvertTextToValue(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			DateTime result;
			TryParseDateTime(text, out result);
			if (Kind != 0)
			{
				result = ConvertToKind(result, Kind);
			}
			if (base.ClipValueToMinMax)
			{
				return GetClippedMinMaxValue(result);
			}
			ValidateDefaultMinMax(result);
			return result;
		}

		/// <summary>Converts the DateTimeUpDown's value to text.</summary>
		/// <returns>A string representing the converted value.</returns>
		protected override string ConvertValueToText()
		{
			if (!base.Value.HasValue)
			{
				return string.Empty;
			}
			return base.Value.Value.ToString(GetFormatString(Format), base.CultureInfo);
		}

		/// <summary>Sets the valid spin directions.</summary>
		protected override void SetValidSpinDirection()
		{
			ValidSpinDirections validSpinDirections = ValidSpinDirections.None;
			if (!base.IsReadOnly)
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

		protected override object OnCoerceValue(object newValue)
		{
			DateTime? dateTime = (DateTime?)base.OnCoerceValue(newValue);
			if (dateTime.HasValue && base.IsInitialized)
			{
				SetKindInternal(dateTime.Value.Kind);
			}
			return dateTime;
		}

		/// <summary>Called when the DateTimeUpDown's value changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected override void OnValueChanged(DateTime? oldValue, DateTime? newValue)
		{
			DateTimeInfo dateTimeInfo = _selectedDateTimeInfo;
			if (dateTimeInfo == null)
			{
				dateTimeInfo = ((base.CurrentDateTimePart != DateTimePart.Other) ? GetDateTimeInfo(base.CurrentDateTimePart) : _dateTimeInfoList[0]);
			}
			if (dateTimeInfo == null)
			{
				dateTimeInfo = _dateTimeInfoList[0];
			}
			if (newValue.HasValue)
			{
				ParseValueIntoDateTimeInfo(base.Value);
			}
			base.OnValueChanged(oldValue, newValue);
			if (!_isTextChangedFromUI)
			{
				_lastValidDate = newValue;
			}
			if (base.TextBox != null)
			{
				_fireSelectionChangedEvent = false;
				base.TextBox.Select(dateTimeInfo.StartPosition, dateTimeInfo.Length);
				_fireSelectionChangedEvent = true;
			}
		}

		protected override bool IsCurrentValueValid()
		{
			if (string.IsNullOrEmpty(base.TextBox.Text))
			{
				return true;
			}
			DateTime result;
			return TryParseDateTime(base.TextBox.Text, out result);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (base.Value.HasValue)
			{
				DateTimeKind kind = base.Value.Value.Kind;
				if (kind != Kind)
				{
					if (Kind == DateTimeKind.Unspecified)
					{
						SetKindInternal(kind);
					}
					else
					{
						base.Value = ConvertToKind(base.Value.Value, Kind);
					}
				}
			}
		}

		protected override void PerformMouseSelection()
		{
			if (base.UpdateValueOnEnterKey)
			{
				ParseValueIntoDateTimeInfo(ConvertTextToValue(base.TextBox.Text));
			}
			base.PerformMouseSelection();
		}

		protected internal override void PerformKeyboardSelection(int nextSelectionStart)
		{
			if (base.UpdateValueOnEnterKey)
			{
				ParseValueIntoDateTimeInfo(ConvertTextToValue(base.TextBox.Text));
			}
			base.PerformKeyboardSelection(nextSelectionStart);
		}

		protected override void InitializeDateTimeInfoList(DateTime? value)
		{
			_dateTimeInfoList.Clear();
			_selectedDateTimeInfo = null;
			string text = GetFormatString(Format);
			if (!string.IsNullOrEmpty(text))
			{
				while (text.Length > 0)
				{
					int num = GetElementLengthByFormat(text);
					DateTimeInfo item = null;
					switch (text[0])
					{
					case '"':
					case '\'':
					{
						int num2 = text.IndexOf(text[0], 1);
						DateTimeInfo dateTimeInfo16 = new DateTimeInfo();
						dateTimeInfo16.IsReadOnly = true;
						dateTimeInfo16.Type = DateTimePart.Other;
						dateTimeInfo16.Length = 1;
						dateTimeInfo16.Content = text.Substring(1, Math.Max(1, num2 - 1));
						item = dateTimeInfo16;
						num = Math.Max(1, num2 + 1);
						break;
					}
					case 'D':
					case 'd':
					{
						string text10 = text.Substring(0, num);
						if (num == 1)
						{
							text10 = "%" + text10;
						}
						if (num > 2)
						{
							DateTimeInfo dateTimeInfo13 = new DateTimeInfo();
							dateTimeInfo13.IsReadOnly = true;
							dateTimeInfo13.Type = DateTimePart.DayName;
							dateTimeInfo13.Length = num;
							dateTimeInfo13.Format = text10;
							item = dateTimeInfo13;
						}
						else
						{
							DateTimeInfo dateTimeInfo14 = new DateTimeInfo();
							dateTimeInfo14.IsReadOnly = false;
							dateTimeInfo14.Type = DateTimePart.Day;
							dateTimeInfo14.Length = num;
							dateTimeInfo14.Format = text10;
							item = dateTimeInfo14;
						}
						break;
					}
					case 'F':
					case 'f':
					{
						string text6 = text.Substring(0, num);
						if (num == 1)
						{
							text6 = "%" + text6;
						}
						DateTimeInfo dateTimeInfo7 = new DateTimeInfo();
						dateTimeInfo7.IsReadOnly = false;
						dateTimeInfo7.Type = DateTimePart.Millisecond;
						dateTimeInfo7.Length = num;
						dateTimeInfo7.Format = text6;
						item = dateTimeInfo7;
						break;
					}
					case 'h':
					{
						string text7 = text.Substring(0, num);
						if (num == 1)
						{
							text7 = "%" + text7;
						}
						DateTimeInfo dateTimeInfo8 = new DateTimeInfo();
						dateTimeInfo8.IsReadOnly = false;
						dateTimeInfo8.Type = DateTimePart.Hour12;
						dateTimeInfo8.Length = num;
						dateTimeInfo8.Format = text7;
						item = dateTimeInfo8;
						break;
					}
					case 'H':
					{
						string text8 = text.Substring(0, num);
						if (num == 1)
						{
							text8 = "%" + text8;
						}
						DateTimeInfo dateTimeInfo10 = new DateTimeInfo();
						dateTimeInfo10.IsReadOnly = false;
						dateTimeInfo10.Type = DateTimePart.Hour24;
						dateTimeInfo10.Length = num;
						dateTimeInfo10.Format = text8;
						item = dateTimeInfo10;
						break;
					}
					case 'M':
					{
						string text4 = text.Substring(0, num);
						if (num == 1)
						{
							text4 = "%" + text4;
						}
						if (num >= 3)
						{
							DateTimeInfo dateTimeInfo4 = new DateTimeInfo();
							dateTimeInfo4.IsReadOnly = false;
							dateTimeInfo4.Type = DateTimePart.MonthName;
							dateTimeInfo4.Length = num;
							dateTimeInfo4.Format = text4;
							item = dateTimeInfo4;
						}
						else
						{
							DateTimeInfo dateTimeInfo5 = new DateTimeInfo();
							dateTimeInfo5.IsReadOnly = false;
							dateTimeInfo5.Type = DateTimePart.Month;
							dateTimeInfo5.Length = num;
							dateTimeInfo5.Format = text4;
							item = dateTimeInfo5;
						}
						break;
					}
					case 'S':
					case 's':
					{
						string text9 = text.Substring(0, num);
						if (num == 1)
						{
							text9 = "%" + text9;
						}
						DateTimeInfo dateTimeInfo11 = new DateTimeInfo();
						dateTimeInfo11.IsReadOnly = false;
						dateTimeInfo11.Type = DateTimePart.Second;
						dateTimeInfo11.Length = num;
						dateTimeInfo11.Format = text9;
						item = dateTimeInfo11;
						break;
					}
					case 'T':
					case 't':
					{
						string text3 = text.Substring(0, num);
						if (num == 1)
						{
							text3 = "%" + text3;
						}
						DateTimeInfo dateTimeInfo3 = new DateTimeInfo();
						dateTimeInfo3.IsReadOnly = false;
						dateTimeInfo3.Type = DateTimePart.AmPmDesignator;
						dateTimeInfo3.Length = num;
						dateTimeInfo3.Format = text3;
						item = dateTimeInfo3;
						break;
					}
					case 'Y':
					case 'y':
					{
						string text11 = text.Substring(0, num);
						if (num == 1)
						{
							text11 = "%" + text11;
						}
						DateTimeInfo dateTimeInfo15 = new DateTimeInfo();
						dateTimeInfo15.IsReadOnly = false;
						dateTimeInfo15.Type = DateTimePart.Year;
						dateTimeInfo15.Length = num;
						dateTimeInfo15.Format = text11;
						item = dateTimeInfo15;
						break;
					}
					case '\\':
						if (text.Length >= 2)
						{
							DateTimeInfo dateTimeInfo12 = new DateTimeInfo();
							dateTimeInfo12.IsReadOnly = true;
							dateTimeInfo12.Content = text.Substring(1, 1);
							dateTimeInfo12.Length = 1;
							dateTimeInfo12.Type = DateTimePart.Other;
							item = dateTimeInfo12;
							num = 2;
						}
						break;
					case 'g':
					{
						string str = text.Substring(0, num);
						if (num == 1)
						{
							str = "%" + str;
						}
						DateTimeInfo dateTimeInfo9 = new DateTimeInfo();
						dateTimeInfo9.IsReadOnly = true;
						dateTimeInfo9.Type = DateTimePart.Period;
						dateTimeInfo9.Length = num;
						dateTimeInfo9.Format = text.Substring(0, num);
						item = dateTimeInfo9;
						break;
					}
					case 'm':
					{
						string text5 = text.Substring(0, num);
						if (num == 1)
						{
							text5 = "%" + text5;
						}
						DateTimeInfo dateTimeInfo6 = new DateTimeInfo();
						dateTimeInfo6.IsReadOnly = false;
						dateTimeInfo6.Type = DateTimePart.Minute;
						dateTimeInfo6.Length = num;
						dateTimeInfo6.Format = text5;
						item = dateTimeInfo6;
						break;
					}
					case 'z':
					{
						string text2 = text.Substring(0, num);
						if (num == 1)
						{
							text2 = "%" + text2;
						}
						DateTimeInfo dateTimeInfo2 = new DateTimeInfo();
						dateTimeInfo2.IsReadOnly = true;
						dateTimeInfo2.Type = DateTimePart.TimeZone;
						dateTimeInfo2.Length = num;
						dateTimeInfo2.Format = text2;
						item = dateTimeInfo2;
						break;
					}
					default:
					{
						num = 1;
						DateTimeInfo dateTimeInfo = new DateTimeInfo();
						dateTimeInfo.IsReadOnly = true;
						dateTimeInfo.Length = 1;
						dateTimeInfo.Content = text[0].ToString();
						dateTimeInfo.Type = DateTimePart.Other;
						item = dateTimeInfo;
						break;
					}
					}
					_dateTimeInfoList.Add(item);
					text = text.Substring(num);
				}
			}
		}

		protected override bool IsLowerThan(DateTime? value1, DateTime? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return value1.Value < value2.Value;
		}

		protected override bool IsGreaterThan(DateTime? value1, DateTime? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return value1.Value > value2.Value;
		}

		protected override void OnUpdateValueOnEnterKeyChanged(bool oldValue, bool newValue)
		{
			throw new NotSupportedException("DateTimeUpDown controls do not support modifying UpdateValueOnEnterKey property.");
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				SyncTextAndValueProperties(false, null);
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		/// <summary>Selects all the of the content of the control's textbox.</summary>
		public void SelectAll()
		{
			_fireSelectionChangedEvent = false;
			base.TextBox.SelectAll();
			_fireSelectionChangedEvent = true;
		}

		private void FormatUpdated()
		{
			InitializeDateTimeInfoList(base.Value);
			if (base.Value.HasValue)
			{
				ParseValueIntoDateTimeInfo(base.Value);
			}
			_processTextChanged = false;
			SyncTextAndValueProperties(false, null);
			_processTextChanged = true;
		}

		private static int GetElementLengthByFormat(string format)
		{
			for (int i = 1; i < format.Length; i++)
			{
				if (string.Compare(format[i].ToString(), format[0].ToString(), false) != 0)
				{
					return i;
				}
			}
			return format.Length;
		}

		private void Increment(int step)
		{
			_fireSelectionChangedEvent = false;
			DateTime? currentDateTime = ConvertTextToValue(base.TextBox.Text);
			if (currentDateTime.HasValue)
			{
				DateTime? dateTime = UpdateDateTime(currentDateTime, step);
				if (!dateTime.HasValue)
				{
					return;
				}
				base.TextBox.Text = dateTime.Value.ToString(GetFormatString(Format), base.CultureInfo);
			}
			else
			{
				base.TextBox.Text = (base.DefaultValue.HasValue ? base.DefaultValue.Value.ToString(GetFormatString(Format), base.CultureInfo) : ContextNow.ToString(GetFormatString(Format), base.CultureInfo));
			}
			if (base.TextBox != null)
			{
				DateTimeInfo dateTimeInfo = _selectedDateTimeInfo;
				if (dateTimeInfo == null)
				{
					dateTimeInfo = ((base.CurrentDateTimePart != DateTimePart.Other) ? GetDateTimeInfo(base.CurrentDateTimePart) : _dateTimeInfoList[0]);
				}
				if (dateTimeInfo == null)
				{
					dateTimeInfo = _dateTimeInfoList[0];
				}
				ParseValueIntoDateTimeInfo(ConvertTextToValue(base.TextBox.Text));
				base.TextBox.Select(dateTimeInfo.StartPosition, dateTimeInfo.Length);
			}
			_fireSelectionChangedEvent = true;
			SyncTextAndValueProperties(true, base.Text);
		}

		private void ParseValueIntoDateTimeInfo(DateTime? newDate)
		{
			string text = string.Empty;
			_dateTimeInfoList.ForEach(delegate(DateTimeInfo info)
			{
				if (info.Format == null)
				{
					info.StartPosition = text.Length;
					info.Length = info.Content.Length;
					text += info.Content;
				}
				else if (newDate.HasValue)
				{
					DateTime value = newDate.Value;
					info.StartPosition = text.Length;
					info.Content = value.ToString(info.Format, base.CultureInfo.DateTimeFormat);
					info.Length = info.Content.Length;
					text += info.Content;
				}
			});
		}

		internal string GetFormatString(DateTimeFormat dateTimeFormat)
		{
			switch (dateTimeFormat)
			{
			case DateTimeFormat.ShortDate:
				return base.CultureInfo.DateTimeFormat.ShortDatePattern;
			case DateTimeFormat.LongDate:
				return base.CultureInfo.DateTimeFormat.LongDatePattern;
			case DateTimeFormat.ShortTime:
				return base.CultureInfo.DateTimeFormat.ShortTimePattern;
			case DateTimeFormat.LongTime:
				return base.CultureInfo.DateTimeFormat.LongTimePattern;
			case DateTimeFormat.FullDateTime:
				return base.CultureInfo.DateTimeFormat.FullDateTimePattern;
			case DateTimeFormat.MonthDay:
				return base.CultureInfo.DateTimeFormat.MonthDayPattern;
			case DateTimeFormat.RFC1123:
				return base.CultureInfo.DateTimeFormat.RFC1123Pattern;
			case DateTimeFormat.SortableDateTime:
				return base.CultureInfo.DateTimeFormat.SortableDateTimePattern;
			case DateTimeFormat.UniversalSortableDateTime:
				return base.CultureInfo.DateTimeFormat.UniversalSortableDateTimePattern;
			case DateTimeFormat.YearMonth:
				return base.CultureInfo.DateTimeFormat.YearMonthPattern;
			case DateTimeFormat.Custom:
				switch (FormatString)
				{
				case "d":
					return base.CultureInfo.DateTimeFormat.ShortDatePattern;
				case "t":
					return base.CultureInfo.DateTimeFormat.ShortTimePattern;
				case "T":
					return base.CultureInfo.DateTimeFormat.LongTimePattern;
				case "D":
					return base.CultureInfo.DateTimeFormat.LongDatePattern;
				case "f":
					return base.CultureInfo.DateTimeFormat.LongDatePattern + " " + base.CultureInfo.DateTimeFormat.ShortTimePattern;
				case "F":
					return base.CultureInfo.DateTimeFormat.FullDateTimePattern;
				case "g":
					return base.CultureInfo.DateTimeFormat.ShortDatePattern + " " + base.CultureInfo.DateTimeFormat.ShortTimePattern;
				case "G":
					return base.CultureInfo.DateTimeFormat.ShortDatePattern + " " + base.CultureInfo.DateTimeFormat.LongTimePattern;
				case "m":
					return base.CultureInfo.DateTimeFormat.MonthDayPattern;
				case "y":
					return base.CultureInfo.DateTimeFormat.YearMonthPattern;
				case "r":
					return base.CultureInfo.DateTimeFormat.RFC1123Pattern;
				case "s":
					return base.CultureInfo.DateTimeFormat.SortableDateTimePattern;
				case "u":
					return base.CultureInfo.DateTimeFormat.UniversalSortableDateTimePattern;
				default:
					return FormatString;
				}
			default:
				throw new ArgumentException("Not a supported format");
			}
		}

		private DateTime? UpdateDateTime(DateTime? currentDateTime, int value)
		{
			DateTimeInfo dateTimeInfo = _selectedDateTimeInfo;
			if (dateTimeInfo == null)
			{
				dateTimeInfo = ((base.CurrentDateTimePart != DateTimePart.Other) ? GetDateTimeInfo(base.CurrentDateTimePart) : _dateTimeInfoList[0]);
			}
			if (dateTimeInfo == null)
			{
				dateTimeInfo = _dateTimeInfoList[0];
			}
			DateTime? value2 = null;
			try
			{
				switch (dateTimeInfo.Type)
				{
				case DateTimePart.Other:
				case DateTimePart.Period:
				case DateTimePart.TimeZone:
					break;
				case DateTimePart.Year:
					value2 = currentDateTime.Value.AddYears(value);
					break;
				case DateTimePart.Month:
				case DateTimePart.MonthName:
					value2 = currentDateTime.Value.AddMonths(value);
					break;
				case DateTimePart.Day:
				case DateTimePart.DayName:
					value2 = currentDateTime.Value.AddDays((double)value);
					break;
				case DateTimePart.Hour12:
				case DateTimePart.Hour24:
					value2 = currentDateTime.Value.AddHours((double)value);
					break;
				case DateTimePart.Minute:
					value2 = currentDateTime.Value.AddMinutes((double)value);
					break;
				case DateTimePart.Second:
					value2 = currentDateTime.Value.AddSeconds((double)value);
					break;
				case DateTimePart.Millisecond:
					value2 = currentDateTime.Value.AddMilliseconds((double)value);
					break;
				case DateTimePart.AmPmDesignator:
					value2 = currentDateTime.Value.AddHours((double)(value * 12));
					break;
				}
			}
			catch
			{
			}
			return CoerceValueMinMax(value2);
		}

		private bool TryParseDateTime(string text, out DateTime result)
		{
			bool flag = false;
			result = ContextNow;
			DateTime dateTime = ContextNow;
			try
			{
				dateTime = (TempValue.HasValue ? TempValue.Value : (base.Value.HasValue ? base.Value.Value : DateTime.Parse(ContextNow.ToString(), base.CultureInfo.DateTimeFormat)));
				flag = DateTimeParser.TryParse(text, GetFormatString(Format), dateTime, base.CultureInfo, AutoClipTimeParts, out result);
			}
			catch (FormatException)
			{
				flag = false;
			}
			if (!flag)
			{
				flag = DateTime.TryParseExact(text, GetFormatString(Format), base.CultureInfo, DateTimeStyles.None, out result);
			}
			if (!flag)
			{
				result = (_lastValidDate.HasValue ? _lastValidDate.Value : dateTime);
			}
			return flag;
		}

		private DateTime ConvertToKind(DateTime dateTime, DateTimeKind kind)
		{
			if (kind == dateTime.Kind)
			{
				return dateTime;
			}
			if (dateTime.Kind != 0)
			{
				switch (kind)
				{
				case DateTimeKind.Unspecified:
					break;
				default:
					return dateTime.ToUniversalTime();
				case DateTimeKind.Local:
					return dateTime.ToLocalTime();
				}
			}
			return DateTime.SpecifyKind(dateTime, kind);
		}

		private void DateTimeUpDown_Loaded(object sender, RoutedEventArgs e)
		{
			if (Format == DateTimeFormat.Custom && string.IsNullOrEmpty(FormatString))
			{
				throw new InvalidOperationException("A FormatString is necessary when Format is set to Custom.");
			}
		}
	}
}
