using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.Primitives
{
	/// <summary>Base class of controls that receive input.</summary>
	public abstract class InputBase : Control
	{
		public static readonly DependencyProperty AllowTextInputProperty = DependencyProperty.Register("AllowTextInput", typeof(bool), typeof(InputBase), new UIPropertyMetadata(true, OnAllowTextInputChanged));

		/// <summary>Identifies the CultureInfo dependency property.</summary>
		public static readonly DependencyProperty CultureInfoProperty = DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(InputBase), new UIPropertyMetadata(CultureInfo.CurrentCulture, OnCultureInfoChanged));

		/// <summary>Identifies the IsReadOnly dependency property.</summary>
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(InputBase), new UIPropertyMetadata(false, OnReadOnlyChanged));

		public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(InputBase), new UIPropertyMetadata(true, OnIsUndoEnabledChanged));

		/// <summary>Identifies the Text dependency property.</summary>
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InputBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged, null, false, UpdateSourceTrigger.LostFocus));

		/// <summary>Identifies the TextAlignment dependency property.</summary>
		public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(InputBase), new UIPropertyMetadata(TextAlignment.Left));

		/// <summary>Identifies the Watermark dependency property.</summary>
		public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(InputBase), new UIPropertyMetadata(null));

		/// <summary>Identifies the WatermarkTemplate dependency property.</summary>
		public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(InputBase), new UIPropertyMetadata(null));

		/// <summary>
		///   <para>Determines if the editable part of the control can be edited. The editable part does not include buttons or spinners, it is typically the text part.</para>
		/// </summary>
		public bool AllowTextInput
		{
			get
			{
				return (bool)GetValue(AllowTextInputProperty);
			}
			set
			{
				SetValue(AllowTextInputProperty, value);
			}
		}

		/// <summary>Gets or sets the CultureInfo of the input control.</summary>
		public CultureInfo CultureInfo
		{
			get
			{
				return (CultureInfo)GetValue(CultureInfoProperty);
			}
			set
			{
				SetValue(CultureInfoProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the input control is read-only.</summary>
		public bool IsReadOnly
		{
			get
			{
				return (bool)GetValue(IsReadOnlyProperty);
			}
			set
			{
				SetValue(IsReadOnlyProperty, value);
			}
		}

		public bool IsUndoEnabled
		{
			get
			{
				return (bool)GetValue(IsUndoEnabledProperty);
			}
			set
			{
				SetValue(IsUndoEnabledProperty, value);
			}
		}

		/// <summary>
		///   <para>Gets or sets the formated string representation of the value of the input control.</para>
		/// </summary>
		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		/// <summary>Gets or sets the alignment of the text.</summary>
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)GetValue(TextAlignmentProperty);
			}
			set
			{
				SetValue(TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the watermark of the input control.</summary>
		public object Watermark
		{
			get
			{
				return GetValue(WatermarkProperty);
			}
			set
			{
				SetValue(WatermarkProperty, value);
			}
		}

		/// <summary>Gets or sets the DataTemplate of the watermark.</summary>
		public DataTemplate WatermarkTemplate
		{
			get
			{
				return (DataTemplate)GetValue(WatermarkTemplateProperty);
			}
			set
			{
				SetValue(WatermarkTemplateProperty, value);
			}
		}

		private static void OnAllowTextInputChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			InputBase inputBase = o as InputBase;
			if (inputBase != null)
			{
				inputBase.OnAllowTextInputChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnAllowTextInputChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnCultureInfoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			InputBase inputBase = o as InputBase;
			if (inputBase != null)
			{
				inputBase.OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue);
			}
		}

		/// <summary>Called when CultureInfo changes.</summary>
		/// <param name="oldValue">The old CultureInfo value.</param>
		/// <param name="newValue">The new CultureInfo value.</param>
		protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
		{
		}

		private static void OnReadOnlyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			InputBase inputBase = o as InputBase;
			if (inputBase != null)
			{
				inputBase.OnReadOnlyChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnReadOnlyChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnIsUndoEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			InputBase inputBase = o as InputBase;
			if (inputBase != null)
			{
				inputBase.OnIsUndoEnabledChanged((bool)e.OldValue, (bool)e.NewValue);
			}
		}

		protected virtual void OnIsUndoEnabledChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			InputBase inputBase = o as InputBase;
			if (inputBase != null)
			{
				inputBase.OnTextChanged((string)e.OldValue, (string)e.NewValue);
			}
		}

		/// <summary>Called when the text of the input control changes.</summary>
		/// <param name="oldValue">The old string value.</param>
		/// <param name="newValue">The new string value.</param>
		protected virtual void OnTextChanged(string oldValue, string newValue)
		{
		}
	}
}
