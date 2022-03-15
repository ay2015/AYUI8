using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Xceed.Wpf.Toolkit.Primitives
{
	public abstract class DateTimeUpDownBase<T> : UpDownBase<T>
	{
		internal List<DateTimeInfo> _dateTimeInfoList = new List<DateTimeInfo>();

		internal DateTimeInfo _selectedDateTimeInfo;

		internal bool _fireSelectionChangedEvent = true;

		internal bool _processTextChanged = true;

		public static readonly DependencyProperty CurrentDateTimePartProperty = DependencyProperty.Register("CurrentDateTimePart", typeof(DateTimePart), typeof(DateTimeUpDownBase<T>), new UIPropertyMetadata(DateTimePart.Other, OnCurrentDateTimePartChanged));

		public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(DateTimeUpDownBase<T>), new UIPropertyMetadata(1, OnStepChanged));

		/// <summary>
		///   <font size="2">Gets/Sets the current Date/Time part that will be changed when using the Up/Down buttons (or Up/Down keys). This value must match a valid
		/// DateTime part on the DateTimeUpDownBase. This property will be modified when a new DateTimePart is selected via the mouse or keyboard. Default is
		/// DateTimePart.Other.</font>
		/// </summary>
		public DateTimePart CurrentDateTimePart
		{
			get
			{
				return (DateTimePart)GetValue(CurrentDateTimePartProperty);
			}
			set
			{
				SetValue(CurrentDateTimePartProperty, value);
			}
		}

		/// <summary>
		///   <font size="2">Gets/sets the step to use when incrementing/decrementing a DateTimeUpDown or TimeSpanUpDown with Up/Down buttons or Up/Down keys. Default is
		/// 1.</font>
		/// </summary>
		public int Step
		{
			get
			{
				return (int)GetValue(StepProperty);
			}
			set
			{
				SetValue(StepProperty, value);
			}
		}

		private static void OnCurrentDateTimePartChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateTimeUpDownBase<T> dateTimeUpDownBase = o as DateTimeUpDownBase<T>;
			if (dateTimeUpDownBase != null)
			{
				dateTimeUpDownBase.OnCurrentDateTimePartChanged((DateTimePart)e.OldValue, (DateTimePart)e.NewValue);
			}
		}

		protected virtual void OnCurrentDateTimePartChanged(DateTimePart oldValue, DateTimePart newValue)
		{
			Select(GetDateTimeInfo(newValue));
		}

		private static void OnStepChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateTimeUpDownBase<T> dateTimeUpDownBase = o as DateTimeUpDownBase<T>;
			if (dateTimeUpDownBase != null)
			{
				dateTimeUpDownBase.OnStepChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		protected virtual void OnStepChanged(int oldValue, int newValue)
		{
		}

		internal DateTimeUpDownBase()
		{
			InitializeDateTimeInfoList(base.Value);
			base.Loaded += DateTimeUpDownBase_Loaded;
		}

		public override void OnApplyTemplate()
		{
			if (base.TextBox != null)
			{
				base.TextBox.SelectionChanged -= TextBox_SelectionChanged;
			}
			base.OnApplyTemplate();
			if (base.TextBox != null)
			{
				base.TextBox.SelectionChanged += TextBox_SelectionChanged;
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			int num = (_selectedDateTimeInfo != null) ? _selectedDateTimeInfo.StartPosition : 0;
			int num2 = (_selectedDateTimeInfo != null) ? _selectedDateTimeInfo.Length : 0;
			switch (e.Key)
			{
			case Key.Return:
				if (!base.IsReadOnly)
				{
					_fireSelectionChangedEvent = false;
					BindingExpression bindingExpression = BindingOperations.GetBindingExpression(base.TextBox, TextBox.TextProperty);
					bindingExpression.UpdateSource();
					_fireSelectionChangedEvent = true;
				}
				return;
			case Key.Add:
				if (base.AllowSpin && !base.IsReadOnly)
				{
					DoIncrement();
					e.Handled = true;
				}
				_fireSelectionChangedEvent = false;
				break;
			case Key.Subtract:
				if (base.AllowSpin && !base.IsReadOnly)
				{
					DoDecrement();
					e.Handled = true;
				}
				_fireSelectionChangedEvent = false;
				break;
			case Key.Right:
				if (IsCurrentValueValid())
				{
					PerformKeyboardSelection(num + num2);
					e.Handled = true;
				}
				_fireSelectionChangedEvent = false;
				break;
			case Key.Left:
				if (IsCurrentValueValid())
				{
					PerformKeyboardSelection((num > 0) ? (num - 1) : 0);
					e.Handled = true;
				}
				_fireSelectionChangedEvent = false;
				break;
			default:
				_fireSelectionChangedEvent = false;
				break;
			}
			base.OnPreviewKeyDown(e);
		}

		private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			if (_fireSelectionChangedEvent)
			{
				PerformMouseSelection();
			}
			else
			{
				_fireSelectionChangedEvent = true;
			}
		}

		private void DateTimeUpDownBase_Loaded(object sender, RoutedEventArgs e)
		{
			InitSelection();
		}

		protected virtual void InitializeDateTimeInfoList(T value)
		{
		}

		protected virtual bool IsCurrentValueValid()
		{
			return true;
		}

		protected virtual void PerformMouseSelection()
		{
			DateTimeInfo dateTimeInfo = GetDateTimeInfo(base.TextBox.SelectionStart);
			if (dateTimeInfo != null && dateTimeInfo.Type == DateTimePart.Other)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)delegate
				{
					Select(GetDateTimeInfo(dateTimeInfo.StartPosition + dateTimeInfo.Length));
				});
			}
			else
			{
				Select(dateTimeInfo);
			}
		}

		protected virtual bool IsLowerThan(T value1, T value2)
		{
			return false;
		}

		protected virtual bool IsGreaterThan(T value1, T value2)
		{
			return false;
		}

		internal DateTimeInfo GetDateTimeInfo(int selectionStart)
		{
			return _dateTimeInfoList.FirstOrDefault(delegate(DateTimeInfo info)
			{
				if (info.StartPosition <= selectionStart)
				{
					return selectionStart < info.StartPosition + info.Length;
				}
				return false;
			});
		}

		internal DateTimeInfo GetDateTimeInfo(DateTimePart part)
		{
			return _dateTimeInfoList.FirstOrDefault((DateTimeInfo info) => info.Type == part);
		}

		internal virtual void Select(DateTimeInfo info)
		{
			if (info != null && !info.Equals(_selectedDateTimeInfo) && base.TextBox != null && !string.IsNullOrEmpty(base.TextBox.Text))
			{
				_fireSelectionChangedEvent = false;
				base.TextBox.Select(info.StartPosition, info.Length);
				_fireSelectionChangedEvent = true;
				_selectedDateTimeInfo = info;
				SetCurrentValue(CurrentDateTimePartProperty, info.Type);
			}
		}

		internal T CoerceValueMinMax(T value)
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

		internal void ValidateDefaultMinMax(T value)
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

		internal T GetClippedMinMaxValue(T value)
		{
			if (IsGreaterThan(value, base.Maximum))
			{
				return base.Maximum;
			}
			if (IsLowerThan(value, base.Minimum))
			{
				return base.Minimum;
			}
			return value;
		}

		protected internal virtual void PerformKeyboardSelection(int nextSelectionStart)
		{
			base.TextBox.Focus();
			if (!base.UpdateValueOnEnterKey)
			{
				CommitInput();
			}
			int num = (_selectedDateTimeInfo != null) ? _selectedDateTimeInfo.StartPosition : 0;
			int num2 = nextSelectionStart - num;
			if (num2 > 0)
			{
				Select(GetNextDateTimeInfo(nextSelectionStart));
			}
			else
			{
				Select(GetPreviousDateTimeInfo(nextSelectionStart - 1));
			}
		}

		private DateTimeInfo GetNextDateTimeInfo(int nextSelectionStart)
		{
			DateTimeInfo dateTimeInfo = GetDateTimeInfo(nextSelectionStart);
			if (dateTimeInfo == null)
			{
				dateTimeInfo = _dateTimeInfoList.First();
			}
			DateTimeInfo objB = dateTimeInfo;
			while (dateTimeInfo.Type == DateTimePart.Other)
			{
				dateTimeInfo = GetDateTimeInfo(dateTimeInfo.StartPosition + dateTimeInfo.Length);
				if (dateTimeInfo == null)
				{
					dateTimeInfo = _dateTimeInfoList.First();
				}
				if (object.Equals(dateTimeInfo, objB))
				{
					throw new InvalidOperationException("Couldn't find a valid DateTimeInfo.");
				}
			}
			return dateTimeInfo;
		}

		private DateTimeInfo GetPreviousDateTimeInfo(int previousSelectionStart)
		{
			DateTimeInfo dateTimeInfo = GetDateTimeInfo(previousSelectionStart);
			if (dateTimeInfo == null && _dateTimeInfoList.Count > 0)
			{
				dateTimeInfo = _dateTimeInfoList.Last();
			}
			DateTimeInfo objB = dateTimeInfo;
			while (dateTimeInfo != null && dateTimeInfo.Type == DateTimePart.Other)
			{
				dateTimeInfo = GetDateTimeInfo(dateTimeInfo.StartPosition - 1);
				if (dateTimeInfo == null)
				{
					dateTimeInfo = _dateTimeInfoList.Last();
				}
				if (object.Equals(dateTimeInfo, objB))
				{
					throw new InvalidOperationException("Couldn't find a valid DateTimeInfo.");
				}
			}
			return dateTimeInfo;
		}

		private void InitSelection()
		{
			if (_selectedDateTimeInfo == null)
			{
				Select((CurrentDateTimePart != DateTimePart.Other) ? GetDateTimeInfo(CurrentDateTimePart) : GetDateTimeInfo(0));
			}
		}
	}
}
