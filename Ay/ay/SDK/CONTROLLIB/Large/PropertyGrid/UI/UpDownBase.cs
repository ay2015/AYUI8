using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit.Core.Input;

namespace Xceed.Wpf.Toolkit.Primitives
{
	/// <summary>Base class of controls providing an up-down spinner.</summary>
	/// <typeparam name="T">The type for this class.</typeparam>
	[TemplatePart(Name = "PART_Spinner", Type = typeof(Spinner))]
	[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
	public abstract class UpDownBase<T> : InputBase, IValidateInput
	{
		internal const string PART_TextBox = "PART_TextBox";

		internal const string PART_Spinner = "PART_Spinner";

		internal bool _isTextChangedFromUI;

		private bool _isSyncingTextAndValueProperties;

		private bool _internalValueSet;

		/// <summary>Identifies the AllowSpin dependency property.</summary>
		public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register("AllowSpin", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));

		public static readonly DependencyProperty ButtonSpinnerLocationProperty = DependencyProperty.Register("ButtonSpinnerLocation", typeof(Location), typeof(UpDownBase<T>), new UIPropertyMetadata(Location.Right));

		public static readonly DependencyProperty ClipValueToMinMaxProperty = DependencyProperty.Register("ClipValueToMinMax", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(false));

		public static readonly DependencyProperty DisplayDefaultValueOnEmptyTextProperty = DependencyProperty.Register("DisplayDefaultValueOnEmptyText", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(false, OnDisplayDefaultValueOnEmptyTextChanged));

		/// <summary>Identifies the DefaultValue dependency property.</summary>
		public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnDefaultValueChanged));

		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));

		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(T), typeof(UpDownBase<T>), new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));

		/// <summary>Identifies the MouseWheelActiveTrigger dependency property.</summary>
		public static readonly DependencyProperty MouseWheelActiveTriggerProperty = DependencyProperty.Register("MouseWheelActiveTrigger", typeof(MouseWheelActiveTrigger), typeof(UpDownBase<T>), new UIPropertyMetadata(MouseWheelActiveTrigger.FocusedMouseOver));

		[Obsolete("Use MouseWheelActiveTrigger property instead")]
		public static readonly DependencyProperty MouseWheelActiveOnFocusProperty = DependencyProperty.Register("MouseWheelActiveOnFocus", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true, OnMouseWheelActiveOnFocusChanged));

		/// <summary>Identifies the ShowButtonSpinner dependency property.</summary>
		public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register("ShowButtonSpinner", typeof(bool), typeof(UpDownBase<T>), new UIPropertyMetadata(true));

		public static readonly DependencyProperty UpdateValueOnEnterKeyProperty = DependencyProperty.Register("UpdateValueOnEnterKey", typeof(bool), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(false, OnUpdateValueOnEnterKeyChanged));

		/// <summary>Identifies the Value dependency property.</summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(T), typeof(UpDownBase<T>), new FrameworkPropertyMetadata(default(T), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

		/// <summary>Identifies the ValueChanged routed event.</summary>
		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(UpDownBase<T>));

		/// <summary>Gets or sets the Spinner of this control.</summary>
		protected Spinner Spinner
		{
			get;
			private set;
		}

		/// <summary>Gets or sets the control's TextBox.</summary>
		protected TextBox TextBox
		{
			get;
			private set;
		}

		/// <summary>Gets or sets a value indicating whether increment/decrement operations via the keyboard, button spinners, or mouse wheel can be performed.</summary>
		public bool AllowSpin
		{
			get
			{
				return (bool)GetValue(AllowSpinProperty);
			}
			set
			{
				SetValue(AllowSpinProperty, value);
			}
		}

		/// <summary>
		///   <span style="WHITE-SPACE: normal; WORD-SPACING: 0px; TEXT-TRANSFORM: none; FLOAT: none; COLOR: rgb(0,0,0); FONT: 14px Verdana, Arial, Helvetica, sans-serif; DISPLAY: inline !important; LETTER-SPACING: normal; BACKGROUND-COLOR: rgb(251,251,251); TEXT-INDENT: 0px; -webkit-text-stroke-width: 0px">
		/// Gets/Sets the location of the Up/Down buttons (either on the left or on the right) of numericUpDown controls.</span>
		/// </summary>
		public Location ButtonSpinnerLocation
		{
			get
			{
				return (Location)GetValue(ButtonSpinnerLocationProperty);
			}
			set
			{
				SetValue(ButtonSpinnerLocationProperty, value);
			}
		}

		public bool ClipValueToMinMax
		{
			get
			{
				return (bool)GetValue(ClipValueToMinMaxProperty);
			}
			set
			{
				SetValue(ClipValueToMinMaxProperty, value);
			}
		}

		public bool DisplayDefaultValueOnEmptyText
		{
			get
			{
				return (bool)GetValue(DisplayDefaultValueOnEmptyTextProperty);
			}
			set
			{
				SetValue(DisplayDefaultValueOnEmptyTextProperty, value);
			}
		}

		/// <summary>Gets or sets the default value of the control.</summary>
		public T DefaultValue
		{
			get
			{
				return (T)GetValue(DefaultValueProperty);
			}
			set
			{
				SetValue(DefaultValueProperty, value);
			}
		}

		public T Maximum
		{
			get
			{
				return (T)GetValue(MaximumProperty);
			}
			set
			{
				SetValue(MaximumProperty, value);
			}
		}

		public T Minimum
		{
			get
			{
				return (T)GetValue(MinimumProperty);
			}
			set
			{
				SetValue(MinimumProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets a value indicating when the mouse wheel event should affect the value.</para>
		/// </summary>
		public MouseWheelActiveTrigger MouseWheelActiveTrigger
		{
			get
			{
				return (MouseWheelActiveTrigger)GetValue(MouseWheelActiveTriggerProperty);
			}
			set
			{
				SetValue(MouseWheelActiveTriggerProperty, value);
			}
		}

		[Obsolete("Use MouseWheelActiveTrigger property instead")]
		public bool MouseWheelActiveOnFocus
		{
			get
			{
				return (bool)GetValue(MouseWheelActiveOnFocusProperty);
			}
			set
			{
				SetValue(MouseWheelActiveOnFocusProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the button spinners are visible.</summary>
		public bool ShowButtonSpinner
		{
			get
			{
				return (bool)GetValue(ShowButtonSpinnerProperty);
			}
			set
			{
				SetValue(ShowButtonSpinnerProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the synchronization between "Value" and "Text" should be done only on the Enter key press (and lost focus).</summary>
		public bool UpdateValueOnEnterKey
		{
			get
			{
				return (bool)GetValue(UpdateValueOnEnterKeyProperty);
			}
			set
			{
				SetValue(UpdateValueOnEnterKeyProperty, value);
			}
		}

		/// <summary>Gets or sets the value of the control.</summary>
		public T Value
		{
			get
			{
				return (T)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		/// <summary>Raised when there is a validation error.</summary>
		public event InputValidationErrorEventHandler InputValidationError;

		/// <summary>
		///   <font size="2">Raised when an Increment/Decrement action is initiated (Mouse click on ButtonSpinners, keyboard Up/Down keys pressed, mouse-wheel
		/// activated).</font>
		/// </summary>
		public event EventHandler<SpinEventArgs> Spinned;

		/// <summary>Invoked when Value changes.</summary>
		public event RoutedPropertyChangedEventHandler<object> ValueChanged
		{
			add
			{
				AddHandler(ValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(ValueChangedEvent, value);
			}
		}

		private static void OnDisplayDefaultValueOnEmptyTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
		{
			((UpDownBase<T>)source).OnDisplayDefaultValueOnEmptyTextChanged((bool)args.OldValue, (bool)args.NewValue);
		}

		private void OnDisplayDefaultValueOnEmptyTextChanged(bool oldValue, bool newValue)
		{
			if (base.IsInitialized && string.IsNullOrEmpty(base.Text))
			{
				SyncTextAndValueProperties(false, base.Text);
			}
		}

		private static void OnDefaultValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
		{
			((UpDownBase<T>)source).OnDefaultValueChanged((T)args.OldValue, (T)args.NewValue);
		}

		private void OnDefaultValueChanged(T oldValue, T newValue)
		{
			if (base.IsInitialized && string.IsNullOrEmpty(base.Text))
			{
				SyncTextAndValueProperties(true, base.Text);
			}
		}

		private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UpDownBase<T> upDownBase = o as UpDownBase<T>;
			if (upDownBase != null)
			{
				upDownBase.OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
			}
		}

		protected virtual void OnMaximumChanged(T oldValue, T newValue)
		{
			if (base.IsInitialized)
			{
				SetValidSpinDirection();
			}
		}

		private static object OnCoerceMaximum(DependencyObject d, object baseValue)
		{
			UpDownBase<T> upDownBase = d as UpDownBase<T>;
			if (upDownBase != null)
			{
				return upDownBase.OnCoerceMaximum((T)baseValue);
			}
			return baseValue;
		}

		protected virtual T OnCoerceMaximum(T baseValue)
		{
			return baseValue;
		}

		private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UpDownBase<T> upDownBase = o as UpDownBase<T>;
			if (upDownBase != null)
			{
				upDownBase.OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
			}
		}

		protected virtual void OnMinimumChanged(T oldValue, T newValue)
		{
			if (base.IsInitialized)
			{
				SetValidSpinDirection();
			}
		}

		private static object OnCoerceMinimum(DependencyObject d, object baseValue)
		{
			UpDownBase<T> upDownBase = d as UpDownBase<T>;
			if (upDownBase != null)
			{
				return upDownBase.OnCoerceMinimum((T)baseValue);
			}
			return baseValue;
		}

		protected virtual T OnCoerceMinimum(T baseValue)
		{
			return baseValue;
		}

		private static void OnMouseWheelActiveOnFocusChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UpDownBase<T> upDownBase = o as UpDownBase<T>;
			if (upDownBase != null)
			{
				upDownBase.MouseWheelActiveTrigger = (((bool)e.NewValue) ? MouseWheelActiveTrigger.FocusedMouseOver : MouseWheelActiveTrigger.MouseOver);
			}
		}

		private static void OnUpdateValueOnEnterKeyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UpDownBase<T> upDownBase = o as UpDownBase<T>;
			if (upDownBase != null)
			{
				upDownBase.OnUpdateValueOnEnterKeyChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnUpdateValueOnEnterKeyChanged(bool oldValue, bool newValue)
		{
		}

		private void SetValueInternal(T value)
		{
			_internalValueSet = true;
			try
			{
				Value = value;
			}
			finally
			{
				_internalValueSet = false;
			}
		}

		private static object OnCoerceValue(DependencyObject o, object basevalue)
		{
			return ((UpDownBase<T>)o).OnCoerceValue(basevalue);
		}

		/// <summary>Called when the value is coerced.</summary>
		protected virtual object OnCoerceValue(object newValue)
		{
			return newValue;
		}

		private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UpDownBase<T> upDownBase = o as UpDownBase<T>;
			if (upDownBase != null)
			{
				upDownBase.OnValueChanged((T)e.OldValue, (T)e.NewValue);
			}
		}

		/// <summary>Called when Value changes.</summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void OnValueChanged(T oldValue, T newValue)
		{
			if (!_internalValueSet && base.IsInitialized)
			{
				SyncTextAndValueProperties(false, null, true);
			}
			SetValidSpinDirection();
			RaiseValueChangedEvent(oldValue, newValue);
		}

		internal UpDownBase()
		{
			
			AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, new RoutedEventHandler(HandleClickOutsideOfControlWithMouseCapture), true);
			base.IsKeyboardFocusWithinChanged += UpDownBase_IsKeyboardFocusWithinChanged;
		}

		/// <summary>Sets the focus to TextBox.</summary>
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (TextBox != null)
			{
				TextBox.Focus();
			}
			base.OnAccessKey(e);
		}

		/// <summary>Builds the visual tree for the element.</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (TextBox != null)
			{
				TextBox.TextChanged -= TextBox_TextChanged;
				TextBox.RemoveHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(TextBox_PreviewMouseDown));
			}
			TextBox = (GetTemplateChild("PART_TextBox") as TextBox);
			if (TextBox != null)
			{
				TextBox.Text = base.Text;
				TextBox.TextChanged += TextBox_TextChanged;
				TextBox.AddHandler(Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(TextBox_PreviewMouseDown), true);
			}
			if (Spinner != null)
			{
				Spinner.Spin -= OnSpinnerSpin;
			}
			Spinner = (GetTemplateChild("PART_Spinner") as Spinner);
			if (Spinner != null)
			{
				Spinner.Spin += OnSpinnerSpin;
			}
			SetValidSpinDirection();
		}

		/// <summary>Called when the KeyDown event is raised.</summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key == Key.Return)
			{
				bool flag = CommitInput();
				e.Handled = !flag;
			}
		}

		/// <summary>Called when when Text changes.</summary>
		/// <param name="oldValue">The old string value of Text.</param>
		/// <param name="newValue">The new string value of Text.</param>
		protected override void OnTextChanged(string oldValue, string newValue)
		{
			if (base.IsInitialized)
			{
				if (UpdateValueOnEnterKey)
				{
					if (!_isTextChangedFromUI)
					{
						SyncTextAndValueProperties(true, base.Text);
					}
				}
				else
				{
					SyncTextAndValueProperties(true, base.Text);
				}
			}
		}

		/// <summary>Called when the CultureInfo property has changed.</summary>
		protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
		{
			if (base.IsInitialized)
			{
				SyncTextAndValueProperties(false, null);
			}
		}

		/// <summary>Called when the ReadOnly property has changed.</summary>
		protected override void OnReadOnlyChanged(bool oldValue, bool newValue)
		{
			SetValidSpinDirection();
		}

		private void TextBox_PreviewMouseDown(object sender, RoutedEventArgs e)
		{
			if (MouseWheelActiveTrigger == MouseWheelActiveTrigger.Focused && Mouse.Captured != Spinner)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate
				{
					Mouse.Capture(Spinner);
				});
			}
		}

		private void HandleClickOutsideOfControlWithMouseCapture(object sender, RoutedEventArgs e)
		{
			if (Mouse.Captured is Spinner)
			{
				Spinner.ReleaseMouseCapture();
			}
		}

		private void OnSpinnerSpin(object sender, SpinEventArgs e)
		{
			if (AllowSpin && !base.IsReadOnly)
			{
				MouseWheelActiveTrigger mouseWheelActiveTrigger = MouseWheelActiveTrigger;
				bool flag = !e.UsingMouseWheel;
				flag |= (mouseWheelActiveTrigger == MouseWheelActiveTrigger.MouseOver);
				flag |= (TextBox != null && TextBox.IsFocused && mouseWheelActiveTrigger == MouseWheelActiveTrigger.FocusedMouseOver);
				if (flag | (TextBox != null && TextBox.IsFocused && mouseWheelActiveTrigger == MouseWheelActiveTrigger.Focused && Mouse.Captured is Spinner))
				{
					e.Handled = true;
					OnSpin(e);
				}
			}
		}

		/// <summary>Called when Spinner.Spin is raised.</summary>
		/// <param name="e">Spin event information.</param>
		protected virtual void OnSpin(SpinEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			EventHandler<SpinEventArgs> spinned = this.Spinned;
			if (spinned != null)
			{
				spinned(this, e);
			}
			if (e.Direction == SpinDirection.Increase)
			{
				DoIncrement();
			}
			else
			{
				DoDecrement();
			}
		}

		protected virtual void RaiseValueChangedEvent(T oldValue, T newValue)
		{
			RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue);
			routedPropertyChangedEventArgs.RoutedEvent = ValueChangedEvent;
			RaiseEvent(routedPropertyChangedEventArgs);
		}

		/// <summary>Called when the Initialized event is raised.</summary>
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			bool flag = ReadLocalValue(ValueProperty) == DependencyProperty.UnsetValue && BindingOperations.GetBinding(this, ValueProperty) == null && object.Equals(Value, ValueProperty.DefaultMetadata.DefaultValue);
			SyncTextAndValueProperties(flag, base.Text, !flag);
		}

		internal void DoDecrement()
		{
			if (Spinner == null || (Spinner.ValidSpinDirection & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease)
			{
				OnDecrement();
			}
		}

		internal void DoIncrement()
		{
			if (Spinner == null || (Spinner.ValidSpinDirection & ValidSpinDirections.Increase) == ValidSpinDirections.Increase)
			{
				OnIncrement();
			}
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (base.IsKeyboardFocusWithin)
			{
				try
				{
					_isTextChangedFromUI = true;
					base.Text = ((TextBox)sender).Text;
				}
				finally
				{
					_isTextChangedFromUI = false;
				}
			}
		}

		private void UpDownBase_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				CommitInput();
			}
		}

		private void RaiseInputValidationError(Exception e)
		{
			if (this.InputValidationError != null)
			{
				InputValidationErrorEventArgs inputValidationErrorEventArgs = new InputValidationErrorEventArgs(e);
				this.InputValidationError(this, inputValidationErrorEventArgs);
				if (inputValidationErrorEventArgs.ThrowException)
				{
					throw inputValidationErrorEventArgs.Exception;
				}
			}
		}

		/// <summary>Commits input made in the control.</summary>
		/// <returns>
		///   <strong>true</strong> if the input is successfully committed; <strong>false</strong> otherwise.</returns>
		public virtual bool CommitInput()
		{
			return SyncTextAndValueProperties(true, base.Text);
		}

		/// <summary>Synchronizes the Text and Value properties.</summary>
		protected bool SyncTextAndValueProperties(bool updateValueFromText, string text)
		{
			return SyncTextAndValueProperties(updateValueFromText, text, false);
		}

		private bool SyncTextAndValueProperties(bool updateValueFromText, string text, bool forceTextUpdate)
		{
			if (!_isSyncingTextAndValueProperties)
			{
				_isSyncingTextAndValueProperties = true;
				bool flag = true;
				try
				{
					if (updateValueFromText)
					{
						if (!string.IsNullOrEmpty(text))
						{
							try
							{
								T val = ConvertTextToValue(text);
								if (!object.Equals(val, Value))
								{
									SetValueInternal(val);
								}
							}
							catch (Exception e)
							{
								flag = false;
								if (!_isTextChangedFromUI)
								{
									RaiseInputValidationError(e);
								}
							}
						}
						else
						{
							SetValueInternal(DefaultValue);
						}
					}
					if (!_isTextChangedFromUI)
					{
						if (forceTextUpdate || !string.IsNullOrEmpty(base.Text) || !object.Equals(Value, DefaultValue) || DisplayDefaultValueOnEmptyText)
						{
							string text2 = ConvertValueToText();
							if (!object.Equals(base.Text, text2))
							{
								base.Text = text2;
							}
						}
						if (TextBox != null)
						{
							TextBox.Text = base.Text;
						}
					}
					if (_isTextChangedFromUI && !flag)
					{
						if (Spinner == null)
						{
							return flag;
						}
						Spinner.ValidSpinDirection = ValidSpinDirections.None;
						return flag;
					}
					SetValidSpinDirection();
					return flag;
				}
				finally
				{
					_isSyncingTextAndValueProperties = false;
				}
			}
			return true;
		}

		/// <summary>Converts the formatted text to a value.</summary>
		/// <returns>The converted text.</returns>
		/// <param name="text">The formatted text.</param>
		protected abstract T ConvertTextToValue(string text);

		/// <summary>Converts the value to formatted text.</summary>
		/// <returns>The converted value.</returns>
		protected abstract string ConvertValueToText();

		/// <summary>Called by OnSpin when the spin direction is SpinDirection.Increase.</summary>
		protected abstract void OnIncrement();

		/// <summary>Called by OnSpin when the spin direction is SpinDirection.Decrease.</summary>
		protected abstract void OnDecrement();

		/// <summary>Sets the valid spin directions.</summary>
		protected abstract void SetValidSpinDirection();
	}
}
