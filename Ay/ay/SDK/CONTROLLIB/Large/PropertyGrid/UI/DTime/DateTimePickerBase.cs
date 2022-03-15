using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.Primitives
{
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	public class DateTimePickerBase : DateTimeUpDown
	{
		private const string PART_Popup = "PART_Popup";

		private Popup _popup;

		private DateTime? _initialValue;

		/// <summary>Identifies the IsOpen dependency property.</summary>
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(DateTimePickerBase), new UIPropertyMetadata(false, OnIsOpenChanged));

		/// <summary>Identifies the ShowDropDownButton dependency property.</summary>
		public static readonly DependencyProperty ShowDropDownButtonProperty = DependencyProperty.Register("ShowDropDownButton", typeof(bool), typeof(DateTimePickerBase), new UIPropertyMetadata(true));

		/// <summary>Gets or sets a value indicating whether the <strong>DateTimePicker</strong> is open.</summary>
		public bool IsOpen
		{
			get
			{
				return (bool)GetValue(IsOpenProperty);
			}
			set
			{
				SetValue(IsOpenProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the drop-down button of the <strong>DateTimePicker</strong> should be displayed.</summary>
		public bool ShowDropDownButton
		{
			get
			{
				return (bool)GetValue(ShowDropDownButtonProperty);
			}
			set
			{
				SetValue(ShowDropDownButtonProperty, value);
			}
		}

		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DateTimePickerBase dateTimePickerBase = (DateTimePickerBase)d;
			if (dateTimePickerBase != null)
			{
				dateTimePickerBase.OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
		{
			if (newValue)
			{
				_initialValue = base.Value;
			}
		}

		public DateTimePickerBase()
		{
			AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(HandleKeyDown), true);
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_popup != null)
			{
				_popup.Opened -= Popup_Opened;
			}
			_popup = (GetTemplateChild("PART_Popup") as Popup);
			if (_popup != null)
			{
				_popup.Opened += Popup_Opened;
			}
		}

		protected virtual void HandleKeyDown(object sender, KeyEventArgs e)
		{
			if (!IsOpen)
			{
				if (KeyboardUtilities.IsKeyModifyingPopupState(e))
				{
					IsOpen = true;
					e.Handled = true;
				}
			}
			else if (KeyboardUtilities.IsKeyModifyingPopupState(e))
			{
				ClosePopup(true);
				e.Handled = true;
			}
			else if (e.Key == Key.Return)
			{
				ClosePopup(true);
				e.Handled = true;
			}
			else if (e.Key == Key.Escape)
			{
				if (!object.Equals(base.Value, _initialValue))
				{
					base.Value = _initialValue;
				}
				ClosePopup(true);
				e.Handled = true;
			}
		}

		private void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
		{
			ClosePopup(true);
		}

		protected virtual void Popup_Opened(object sender, EventArgs e)
		{
		}

		protected void ClosePopup(bool isFocusOnTextBox)
		{
			if (IsOpen)
			{
				IsOpen = false;
			}
			ReleaseMouseCapture();
			if (isFocusOnTextBox && base.TextBox != null)
			{
				base.TextBox.Focus();
			}
		}
	}
}
