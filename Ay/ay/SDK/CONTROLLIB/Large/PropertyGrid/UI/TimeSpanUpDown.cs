using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit
{
	/// <summary>
	///   <para>The TimeSpanUpDown class represents a control that lets you increment or decrement a time value over 24 hours. The Format is Days.Hours:Minutes:Seconds.</para>
	///   <para></para>
	///   <para>This class derives from UpDownBase, which is an abstract class that is now the base class for DateTimeUpDown and TimeSpanUpDown. It contains their common
	/// methods and properties.</para>
	/// </summary>
	public class TimeSpanUpDown : DateTimeUpDownBase<TimeSpan?>
	{
		private static int HoursInDay;

		private static int MinutesInDay;

		private static int MinutesInHour;

		private static int SecondsInDay;

		private static int SecondsInHour;

		private static int SecondsInMinute;

		private static int MilliSecondsInDay;

		private static int MilliSecondsInHour;

		private static int MilliSecondsInMinute;

		private static int MilliSecondsInSecond;

		/// <summary>Identifies the FractionalSecondsDigitsCount dependency
		/// property.</summary>
		public static readonly DependencyProperty FractionalSecondsDigitsCountProperty;

		/// <summary>
		///   <div>
		///     Identifies the ShowDays dependency property.
		/// </div>
		/// </summary>
		public static readonly DependencyProperty ShowDaysProperty;

		/// <summary>
		///   <div>
		///     Identifies the ShowSeconds dependency property.
		/// </div>
		/// </summary>
		public static readonly DependencyProperty ShowSecondsProperty;

		/// <summary>Gets or sets the number of digits to use to represent the fractions of seconds in the TimeSpan.</summary>
		public int FractionalSecondsDigitsCount
		{
			get
			{
				return (int)GetValue(FractionalSecondsDigitsCountProperty);
			}
			set
			{
				SetValue(FractionalSecondsDigitsCountProperty, value);
			}
		}

		/// <summary>Gets or sets if the days will be displayed.</summary>
		public bool ShowDays
		{
			get
			{
				return (bool)GetValue(ShowDaysProperty);
			}
			set
			{
				SetValue(ShowDaysProperty, value);
			}
		}

		/// <summary>Gets or Sets if the seconds will be displayed in the TimeSpanUpDown.</summary>
		public bool ShowSeconds
		{
			get
			{
				return (bool)GetValue(ShowSecondsProperty);
			}
			set
			{
				SetValue(ShowSecondsProperty, value);
			}
		}

		static TimeSpanUpDown()
		{
			HoursInDay = 24;
			MinutesInDay = 1440;
			MinutesInHour = 60;
			SecondsInDay = 86400;
			SecondsInHour = 3600;
			SecondsInMinute = 60;
			MilliSecondsInDay = SecondsInDay * 1000;
			MilliSecondsInHour = SecondsInHour * 1000;
			MilliSecondsInMinute = SecondsInMinute * 1000;
			MilliSecondsInSecond = 1000;
			FractionalSecondsDigitsCountProperty = DependencyProperty.Register("FractionalSecondsDigitsCount", typeof(int), typeof(TimeSpanUpDown), new UIPropertyMetadata(0, OnFractionalSecondsDigitsCountChanged, OnCoerceFractionalSecondsDigitsCount));
			ShowDaysProperty = DependencyProperty.Register("ShowDays", typeof(bool), typeof(TimeSpanUpDown), new UIPropertyMetadata(true, OnShowDaysChanged));
			ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(TimeSpanUpDown), new UIPropertyMetadata(true, OnShowSecondsChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(typeof(TimeSpanUpDown)));
			UpDownBase<TimeSpan?>.MaximumProperty.OverrideMetadata(typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.MaxValue));
			UpDownBase<TimeSpan?>.MinimumProperty.OverrideMetadata(typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.MinValue));
			UpDownBase<TimeSpan?>.DefaultValueProperty.OverrideMetadata(typeof(TimeSpanUpDown), new FrameworkPropertyMetadata(TimeSpan.Zero));
		}

		/// <summary>Initializes a new instance of the TimeSpanUpDown class.</summary>
		public TimeSpanUpDown()
		{
			DataObject.AddPastingHandler(this, OnPasting);
		}

		private static object OnCoerceFractionalSecondsDigitsCount(DependencyObject o, object value)
		{
			TimeSpanUpDown timeSpanUpDown = o as TimeSpanUpDown;
			if (timeSpanUpDown != null)
			{
				int num = (int)value;
				if (num < 0 || num > 3)
				{
					throw new ArgumentException("Fractional seconds digits count must be between 0 and 3.");
				}
			}
			return value;
		}

		private static void OnFractionalSecondsDigitsCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanUpDown timeSpanUpDown = o as TimeSpanUpDown;
			if (timeSpanUpDown != null)
			{
				timeSpanUpDown.OnFractionalSecondsDigitsCountChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		protected virtual void OnFractionalSecondsDigitsCountChanged(int oldValue, int newValue)
		{
			UpdateValue();
		}

		private static void OnShowDaysChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanUpDown timeSpanUpDown = o as TimeSpanUpDown;
			if (timeSpanUpDown != null)
			{
				timeSpanUpDown.OnShowDaysChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnShowDaysChanged(bool oldValue, bool newValue)
		{
			UpdateValue();
		}

		private static void OnShowSecondsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			TimeSpanUpDown timeSpanUpDown = o as TimeSpanUpDown;
			if (timeSpanUpDown != null)
			{
				timeSpanUpDown.OnShowSecondsChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnShowSecondsChanged(bool oldValue, bool newValue)
		{
			UpdateValue();
		}

		public override bool CommitInput()
		{
			bool result = SyncTextAndValueProperties(true, base.Text);
			if (base.UpdateValueOnEnterKey && _selectedDateTimeInfo != null && _dateTimeInfoList != null)
			{
				DateTimeInfo dateTimeInfo = _dateTimeInfoList.FirstOrDefault(delegate(DateTimeInfo x)
				{
					if (x.Type == _selectedDateTimeInfo.Type)
					{
						return x.Type != DateTimePart.Other;
					}
					return false;
				});
				_selectedDateTimeInfo = ((dateTimeInfo != null) ? dateTimeInfo : _dateTimeInfoList.FirstOrDefault((DateTimeInfo x) => x.Type != DateTimePart.Other));
				if (_selectedDateTimeInfo != null)
				{
					_fireSelectionChangedEvent = false;
					base.TextBox.Select(_selectedDateTimeInfo.StartPosition, _selectedDateTimeInfo.Length);
					_fireSelectionChangedEvent = true;
				}
			}
			return result;
		}

		protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
		{
			TimeSpan? value = (!base.UpdateValueOnEnterKey) ? base.Value : ((base.TextBox != null) ? ConvertTextToValue(base.TextBox.Text) : null);
			InitializeDateTimeInfoList(value);
		}

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

		protected override void OnIncrement()
		{
			Increment(base.Step);
		}

		protected override void OnDecrement()
		{
			Increment(-base.Step);
		}

		protected override string ConvertValueToText()
		{
			if (!base.Value.HasValue)
			{
				return string.Empty;
			}
			return ParseValueIntoTimeSpanInfo(base.Value, true);
		}

		protected override TimeSpan? ConvertTextToValue(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			TimeSpan value = TimeSpan.MinValue;
			List<char> source = text.Where(delegate(char x)
			{
				if (x != ':')
				{
					return x == '.';
				}
				return true;
			}).ToList();
			string[] array = text.Split(':', '.');
			if (array.Count() <= 1 || array.Any((string x) => string.IsNullOrEmpty(x)))
			{
				return ResetToLastValidValue();
			}
			int[] array2 = new int[array.Count()];
			for (int i = 0; i < array.Count(); i++)
			{
				if (!int.TryParse(array[i].Replace("-", ""), out array2[i]))
				{
					return ResetToLastValidValue();
				}
			}
			if (array2.Count() >= 2)
			{
				bool flag = source.Count() > 1 && source.Last() == '.';
				bool flag2 = source.Count() > 1 && source.First() == '.' && array2.Count() >= 3;
				if (ShowDays)
				{
					int num = flag2 ? array2[0] : (array2[0] / 24);
					if (num > TimeSpan.MaxValue.Days)
					{
						return ResetToLastValidValue();
					}
					int num2 = flag2 ? array2[1] : (array2[0] % 24);
					if ((double)(num * HoursInDay + num2) > TimeSpan.MaxValue.TotalHours)
					{
						return ResetToLastValidValue();
					}
					int num3 = flag2 ? array2[2] : array2[1];
					if ((double)(num * MinutesInDay + num2 * MinutesInHour + num3) > TimeSpan.MaxValue.TotalMinutes)
					{
						return ResetToLastValidValue();
					}
					int num4 = ShowSeconds ? ((flag2 && array2.Count() >= 4) ? array2[3] : ((array2.Count() >= 3) ? array2[2] : 0)) : 0;
					if ((double)(num * SecondsInDay + num2 * SecondsInHour + num3 * SecondsInMinute + num4) > TimeSpan.MaxValue.TotalSeconds)
					{
						return ResetToLastValidValue();
					}
					int num5 = flag ? array2.Last() : 0;
					if ((double)(num * MilliSecondsInDay + num2 * MilliSecondsInHour + num3 * MilliSecondsInMinute + num4 * MilliSecondsInSecond + num5) > TimeSpan.MaxValue.TotalMilliseconds)
					{
						return ResetToLastValidValue();
					}
					value = new TimeSpan(num, num2, num3, num4, num5);
				}
				else
				{
					int num6 = array2[0];
					if ((double)num6 > TimeSpan.MaxValue.TotalHours)
					{
						return ResetToLastValidValue();
					}
					int num7 = array2[1];
					if ((double)(num6 * MinutesInHour + num7) > TimeSpan.MaxValue.TotalMinutes)
					{
						return ResetToLastValidValue();
					}
					int num8 = (ShowSeconds && array2.Count() >= 3) ? array2[2] : 0;
					if ((double)(num6 * SecondsInHour + num7 * SecondsInMinute + num8) > TimeSpan.MaxValue.TotalSeconds)
					{
						return ResetToLastValidValue();
					}
					int num9 = flag ? array2.Last() : 0;
					if ((double)(num6 * MilliSecondsInHour + num7 * MilliSecondsInMinute + num8 * MilliSecondsInSecond + num9) > TimeSpan.MaxValue.TotalMilliseconds)
					{
						return ResetToLastValidValue();
					}
					value = new TimeSpan(0, num6, num7, num8, num9);
				}
				if (text.StartsWith("-"))
				{
					value = value.Negate();
				}
			}
			if (base.ClipValueToMinMax)
			{
				return GetClippedMinMaxValue(value);
			}
			ValidateDefaultMinMax(value);
			return value;
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			e.Handled = !IsNumber(e.Text);
			base.OnPreviewTextInput(e);
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				e.Handled = true;
			}
			base.OnPreviewKeyDown(e);
		}

		protected override void OnTextChanged(string previousValue, string currentValue)
		{
			if (_processTextChanged)
			{
				if (string.IsNullOrEmpty(currentValue))
				{
					if (!base.UpdateValueOnEnterKey)
					{
						base.Value = null;
					}
				}
				else
				{
					if (base.Value.HasValue)
					{
						TimeSpan value = base.Value.Value;
					}
					List<char> source = currentValue.Where(delegate(char x)
					{
						if (x != ':')
						{
							return x == '.';
						}
						return true;
					}).ToList();
					string[] array = currentValue.Split(':', '.');
					if (array.Count() >= 2 && !array.Any((string x) => string.IsNullOrEmpty(x)))
					{
						bool flag = source.First() == '.' && array.Count() >= 3;
						bool flag2 = source.Count() > 1 && source.Last() == '.';
						int[] array2 = new int[array.Count()];
						for (int i = 0; i < array.Count(); i++)
						{
							if (!int.TryParse(array[i], out array2[i]))
							{
								return;
							}
						}
						int num = flag ? Math.Abs(array2[0]) : 0;
						if (num <= TimeSpan.MaxValue.Days)
						{
							int num2 = flag ? Math.Abs(array2[1]) : Math.Abs(array2[0]);
							if (!((double)(num * HoursInDay + num2) > TimeSpan.MaxValue.TotalHours))
							{
								int num3 = flag ? Math.Abs(array2[2]) : Math.Abs(array2[1]);
								if (!((double)(num * MinutesInDay + num2 * MinutesInHour + num3) > TimeSpan.MaxValue.TotalMinutes))
								{
									int num4 = (flag && ShowSeconds && array2.Count() >= 4) ? Math.Abs(array2[3]) : ((ShowSeconds && array2.Count() >= 3) ? Math.Abs(array2[2]) : 0);
									if (!((double)(num * SecondsInDay + num2 * SecondsInHour + num3 * SecondsInMinute + num4) > TimeSpan.MaxValue.TotalSeconds))
									{
										int num5 = flag2 ? Math.Abs(array2.Last()) : 0;
										if (!((double)(num * MilliSecondsInDay + num2 * MilliSecondsInHour + num3 * MilliSecondsInMinute + num4 * MilliSecondsInSecond + num5) > TimeSpan.MaxValue.TotalMilliseconds))
										{
											TimeSpan timeSpan = new TimeSpan(num, num2, num3, num4, num5);
											if (array2[0] < 0)
											{
												timeSpan = timeSpan.Negate();
											}
											currentValue = timeSpan.ToString();
											string[] array3 = (previousValue != null) ? previousValue.Split(':', '.') : null;
											string[] array4 = (currentValue != null) ? currentValue.Split(':', '.') : null;
											bool flag3 = array3 != null && array4 != null && array3.Length == array4.Length && currentValue.Length == previousValue.Length;
											if ((_isTextChangedFromUI && !base.UpdateValueOnEnterKey && flag3) || !_isTextChangedFromUI)
											{
												SyncTextAndValueProperties(true, currentValue);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		protected override void OnValueChanged(TimeSpan? oldValue, TimeSpan? newValue)
		{
			if (newValue.HasValue)
			{
				TimeSpan? value = (!base.UpdateValueOnEnterKey) ? base.Value : ((base.TextBox != null) ? ConvertTextToValue(base.TextBox.Text) : null);
				InitializeDateTimeInfoList(value);
			}
			base.OnValueChanged(oldValue, newValue);
		}

		protected override void PerformMouseSelection()
		{
			if (!base.UpdateValueOnEnterKey)
			{
				CommitInput();
				InitializeDateTimeInfoList(base.Value);
			}
			base.PerformMouseSelection();
		}

		protected override void InitializeDateTimeInfoList(TimeSpan? value)
		{
			DateTimeInfo dateTimeInfo = _dateTimeInfoList.FirstOrDefault((DateTimeInfo x) => x.Type == DateTimePart.Day);
			bool flag = dateTimeInfo != null;
			DateTimeInfo dateTimeInfo2 = _dateTimeInfoList.FirstOrDefault((DateTimeInfo x) => x.Type == DateTimePart.Other);
			bool flag2 = dateTimeInfo2 != null && dateTimeInfo2.Content == "-";
			_dateTimeInfoList.Clear();
			if (value.HasValue && value.Value.TotalMilliseconds < 0.0)
			{
				_dateTimeInfoList.Add(new DateTimeInfo
				{
					Type = DateTimePart.Other,
					Length = 1,
					Content = "-",
					IsReadOnly = true
				});
				if (!flag2 && base.TextBox != null)
				{
					_fireSelectionChangedEvent = false;
					base.TextBox.SelectionStart++;
					_fireSelectionChangedEvent = true;
				}
			}
			if (ShowDays)
			{
				if (value.HasValue && value.Value.Days != 0)
				{
					int length = Math.Abs(value.Value.Days).ToString().Length;
					_dateTimeInfoList.Add(new DateTimeInfo
					{
						Type = DateTimePart.Day,
						Length = length,
						Format = "dd"
					});
					_dateTimeInfoList.Add(new DateTimeInfo
					{
						Type = DateTimePart.Other,
						Length = 1,
						Content = ".",
						IsReadOnly = true
					});
					if (base.TextBox != null)
					{
						if (flag && length != dateTimeInfo.Length && _selectedDateTimeInfo.Type != 0)
						{
							_fireSelectionChangedEvent = false;
							base.TextBox.SelectionStart = Math.Max(0, base.TextBox.SelectionStart + (length - dateTimeInfo.Length));
							_fireSelectionChangedEvent = true;
						}
						else if (!flag)
						{
							_fireSelectionChangedEvent = false;
							base.TextBox.SelectionStart += length + 1;
							_fireSelectionChangedEvent = true;
						}
					}
				}
				else if (flag)
				{
					_fireSelectionChangedEvent = false;
					base.TextBox.SelectionStart = Math.Max(flag2 ? 1 : 0, base.TextBox.SelectionStart - (dateTimeInfo.Length + 1));
					_fireSelectionChangedEvent = true;
				}
			}
			_dateTimeInfoList.Add(new DateTimeInfo
			{
				Type = DateTimePart.Hour24,
				Length = 2,
				Format = "hh"
			});
			_dateTimeInfoList.Add(new DateTimeInfo
			{
				Type = DateTimePart.Other,
				Length = 1,
				Content = ":",
				IsReadOnly = true
			});
			_dateTimeInfoList.Add(new DateTimeInfo
			{
				Type = DateTimePart.Minute,
				Length = 2,
				Format = "mm"
			});
			if (ShowSeconds)
			{
				_dateTimeInfoList.Add(new DateTimeInfo
				{
					Type = DateTimePart.Other,
					Length = 1,
					Content = ":",
					IsReadOnly = true
				});
				_dateTimeInfoList.Add(new DateTimeInfo
				{
					Type = DateTimePart.Second,
					Length = 2,
					Format = "ss"
				});
			}
			if (FractionalSecondsDigitsCount > 0)
			{
				_dateTimeInfoList.Add(new DateTimeInfo
				{
					Type = DateTimePart.Other,
					Length = 1,
					Content = ".",
					IsReadOnly = true
				});
				string text = new string('f', FractionalSecondsDigitsCount);
				if (text.Length == 1)
				{
					text = "%" + text;
				}
				_dateTimeInfoList.Add(new DateTimeInfo
				{
					Type = DateTimePart.Millisecond,
					Length = FractionalSecondsDigitsCount,
					Format = text
				});
			}
			if (value.HasValue)
			{
				ParseValueIntoTimeSpanInfo(value, true);
			}
		}

		protected override bool IsLowerThan(TimeSpan? value1, TimeSpan? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return value1.Value < value2.Value;
		}

		protected override bool IsGreaterThan(TimeSpan? value1, TimeSpan? value2)
		{
			if (!value1.HasValue || !value2.HasValue)
			{
				return false;
			}
			return value1.Value > value2.Value;
		}

		internal override void Select(DateTimeInfo info)
		{
			if (base.UpdateValueOnEnterKey)
			{
				if (info != null && !info.Equals(_selectedDateTimeInfo) && base.TextBox != null && !string.IsNullOrEmpty(base.TextBox.Text))
				{
					int num = _dateTimeInfoList.IndexOf(info) / 2;
					if (num < 0)
					{
						base.Select(info);
					}
					else
					{
						string[] array = base.Text.Split(':', '.');
						int num2 = array.Take(num).Sum((string x) => x.Length) + num;
						int num3 = array[num].Length;
						if (num == 0 && array.First().StartsWith("-"))
						{
							num2++;
							num3--;
						}
						_fireSelectionChangedEvent = false;
						base.TextBox.Select(num2, num3);
						_fireSelectionChangedEvent = true;
						_selectedDateTimeInfo = info;
						SetCurrentValue(DateTimeUpDownBase<TimeSpan?>.CurrentDateTimePartProperty, info.Type);
					}
				}
			}
			else
			{
				base.Select(info);
			}
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.GenericAutomationPeer(this);
		}

		private string ParseValueIntoTimeSpanInfo(TimeSpan? value, bool modifyInfo)
		{
			string text = string.Empty;
			_dateTimeInfoList.ForEach(delegate(DateTimeInfo info)
			{
				if (info.Format == null)
				{
					if (modifyInfo)
					{
						info.StartPosition = text.Length;
						info.Length = info.Content.Length;
					}
					text += info.Content;
				}
				else
				{
					TimeSpan timeSpan = TimeSpan.Parse(value.ToString());
					if (modifyInfo)
					{
						info.StartPosition = text.Length;
					}
					string text2 = "";
					text2 = ((!ShowDays && timeSpan.Days != 0 && info.Format == "hh") ? Math.Truncate(Math.Abs(timeSpan.TotalHours)).ToString() : timeSpan.ToString(info.Format, base.CultureInfo.DateTimeFormat));
					if (modifyInfo)
					{
						if (info.Format == "dd")
						{
							text2 = Convert.ToInt32(text2).ToString();
						}
						info.Content = text2;
						info.Length = info.Content.Length;
					}
					text += text2;
				}
			});
			return text;
		}

		private TimeSpan? UpdateTimeSpan(TimeSpan? currentValue, int value)
		{
			DateTimeInfo dateTimeInfo = _selectedDateTimeInfo;
			if (dateTimeInfo == null)
			{
				dateTimeInfo = ((base.CurrentDateTimePart != DateTimePart.Other) ? GetDateTimeInfo(base.CurrentDateTimePart) : ((_dateTimeInfoList[0].Content != "-") ? _dateTimeInfoList[0] : _dateTimeInfoList[1]));
				if (dateTimeInfo == null)
				{
					dateTimeInfo = _dateTimeInfoList[0];
				}
			}
			TimeSpan? timeSpan = null;
			try
			{
				switch (dateTimeInfo.Type)
				{
				case DateTimePart.Day:
					timeSpan = currentValue.Value.Add(new TimeSpan(value, 0, 0, 0, 0));
					break;
				case DateTimePart.Hour24:
					timeSpan = currentValue.Value.Add(new TimeSpan(0, value, 0, 0, 0));
					break;
				case DateTimePart.Minute:
					timeSpan = currentValue.Value.Add(new TimeSpan(0, 0, value, 0, 0));
					break;
				case DateTimePart.Second:
					timeSpan = currentValue.Value.Add(new TimeSpan(0, 0, 0, value, 0));
					break;
				case DateTimePart.Millisecond:
					switch (FractionalSecondsDigitsCount)
					{
					case 1:
						value *= 100;
						break;
					case 2:
						value *= 10;
						break;
					default:
						value = value;
						break;
					}
					timeSpan = currentValue.Value.Add(new TimeSpan(0, 0, 0, 0, value));
					break;
				}
			}
			catch
			{
			}
			timeSpan = ((timeSpan.HasValue && timeSpan.HasValue) ? new TimeSpan?(timeSpan.Value) : timeSpan);
			return CoerceValueMinMax(timeSpan);
		}

		private void Increment(int step)
		{
			if (base.UpdateValueOnEnterKey)
			{
				string text = string.Empty;
				TimeSpan? currentValue = ConvertTextToValue(base.TextBox.Text);
				TimeSpan? timeSpan = currentValue.HasValue ? UpdateTimeSpan(currentValue, step) : new TimeSpan?(base.DefaultValue ?? TimeSpan.Zero);
				if (timeSpan.HasValue && _dateTimeInfoList != null)
				{
					int num = 0;
					int length = 0;
					if (timeSpan.Value.TotalMilliseconds < 0.0 && _dateTimeInfoList[0].Content != "-")
					{
						text = "-";
					}
					for (int i = 0; i < _dateTimeInfoList.Count; i++)
					{
						DateTimeInfo dateTimeInfo = _dateTimeInfoList[i];
						if (_selectedDateTimeInfo != null && dateTimeInfo.Type == _selectedDateTimeInfo.Type)
						{
							num = text.Length;
						}
						switch (dateTimeInfo.Type)
						{
						case DateTimePart.Day:
						{
							string text6 = Math.Abs(timeSpan.Value.Days).ToString(new string('0', dateTimeInfo.Content.Length));
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text6.Length;
							text += text6;
							break;
						}
						case DateTimePart.Hour24:
						{
							string text3 = (i <= 1) ? Math.Truncate(Math.Abs(timeSpan.Value.TotalHours)).ToString(new string('0', dateTimeInfo.Content.Length)) : Math.Abs(timeSpan.Value.Hours).ToString(new string('0', dateTimeInfo.Content.Length));
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text3.Length;
							text += text3;
							break;
						}
						case DateTimePart.Minute:
						{
							string text5 = (i <= 1) ? Math.Truncate(Math.Abs(timeSpan.Value.TotalMinutes)).ToString(new string('0', dateTimeInfo.Content.Length)) : Math.Abs(timeSpan.Value.Minutes).ToString(new string('0', dateTimeInfo.Content.Length));
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text5.Length;
							text += text5;
							break;
						}
						case DateTimePart.Second:
						{
							string text4 = (i <= 1) ? Math.Truncate(Math.Abs(timeSpan.Value.TotalSeconds)).ToString(new string('0', dateTimeInfo.Content.Length)) : Math.Abs(timeSpan.Value.Seconds).ToString(new string('0', dateTimeInfo.Content.Length));
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text4.Length;
							text += text4;
							break;
						}
						case DateTimePart.Millisecond:
						{
							string text7 = (i <= 1) ? Math.Truncate(Math.Abs(timeSpan.Value.TotalMilliseconds)).ToString(new string('0', dateTimeInfo.Content.Length)) : Math.Abs(timeSpan.Value.Milliseconds).ToString(new string('0', dateTimeInfo.Content.Length));
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text7.Length;
							text += text7;
							break;
						}
						case DateTimePart.Other:
						{
							string text2 = (i == 0 && timeSpan.Value.TotalMilliseconds >= 0.0) ? "" : dateTimeInfo.Content;
							dateTimeInfo.StartPosition = text.Length;
							dateTimeInfo.Length = text2.Length;
							text += text2;
							break;
						}
						}
						if (_selectedDateTimeInfo != null && dateTimeInfo.Type == _selectedDateTimeInfo.Type)
						{
							length = text.Length - num;
						}
					}
					base.TextBox.Text = text;
					base.TextBox.Select(num, length);
				}
			}
			else if (base.Value.HasValue)
			{
				TimeSpan? value = UpdateTimeSpan(base.Value, step);
				if (value.HasValue)
				{
					InitializeDateTimeInfoList(value);
					int selectionStart = base.TextBox.SelectionStart;
					int selectionLength = base.TextBox.SelectionLength;
					base.Value = value;
					base.TextBox.Select(selectionStart, selectionLength);
				}
			}
			else
			{
				base.Value = (base.DefaultValue ?? TimeSpan.Zero);
			}
		}

		private bool IsNumber(string str)
		{
			foreach (char c in str)
			{
				if (!char.IsNumber(c))
				{
					return false;
				}
			}
			return true;
		}

		private void UpdateValue()
		{
			TimeSpan? value = (!base.UpdateValueOnEnterKey) ? base.Value : ((base.TextBox != null) ? ConvertTextToValue(base.TextBox.Text) : null);
			InitializeDateTimeInfoList(value);
			SyncTextAndValueProperties(false, base.Text);
		}

		private TimeSpan? ResetToLastValidValue()
		{
			InitializeDateTimeInfoList(base.Value);
			return base.Value;
		}

		private void OnPasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string s = e.DataObject.GetData(typeof(string)) as string;
				TimeSpan result;
				if (!TimeSpan.TryParse(s, out result))
				{
					e.CancelCommand();
				}
			}
		}
	}
}
