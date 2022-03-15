using ay.Controls.Enums;
using System;
using System.Globalization;
using System.Windows;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of the numeric up-down definition classes.</summary>
	public abstract class EditorNumericUpDownDefinitionBase<TEditor, TType> : EditorUpDownDefinitionBase<TEditor, TType?> where TEditor : CommonNumericUpDown<TType>, new()where TType : struct, IFormattable, IComparable<TType>
	{
		/// <summary>Identifies the AutoSelectBehavior
		/// dependency property.</summary>
		public static readonly DependencyProperty AutoSelectBehaviorProperty = NumericUpDown<TType?>.AutoSelectBehaviorProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the ParsingNumberStyle
		/// dependency property.</summary>
		public static readonly DependencyProperty ParsingNumberStyleProperty = CommonNumericUpDown<TType>.ParsingNumberStyleProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the ClipValueToMinMax
		/// dependency property.</summary>
		public static readonly DependencyProperty ClipValueToMinMaxProperty = UpDownBase<TType?>.ClipValueToMinMaxProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the FormatString dependency
		/// property.</summary>
		public static readonly DependencyProperty FormatStringProperty = NumericUpDown<TType?>.FormatStringProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the Increment dependency property.</summary>
		public static readonly DependencyProperty IncrementProperty = NumericUpDown<TType?>.IncrementProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the Maximum dependency property.</summary>
		public static readonly DependencyProperty MaximumProperty = UpDownBase<TType?>.MaximumProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the Minimum dependency property.</summary>
		public static readonly DependencyProperty MinimumProperty = UpDownBase<TType?>.MinimumProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

		/// <summary>Identifies the UpdateValueOnEnterKey dependency
		/// property.</summary>
		public static readonly DependencyProperty UpdateValueOnEnterKeyProperty = UpDownBase<TType?>.UpdateValueOnEnterKeyProperty.AddOwner(typeof(EditorNumericUpDownDefinitionBase<TEditor, TType>));

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

		public TType? Increment
		{
			get
			{
				return (TType?)GetValue(IncrementProperty);
			}
			set
			{
				SetValue(IncrementProperty, value);
			}
		}

		public TType? Maximum
		{
			get
			{
				return (TType?)GetValue(MaximumProperty);
			}
			set
			{
				SetValue(MaximumProperty, value);
			}
		}

		public TType? Minimum
		{
			get
			{
				return (TType?)GetValue(MinimumProperty);
			}
			set
			{
				SetValue(MinimumProperty, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the synchronization between "Value" and "Text" will only be done on an Enter key press or a lostFocus.</summary>
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

		internal static void UpdateMetadata(Type type, TType? increment, TType? minValue, TType? maxValue)
		{
			IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
			MaximumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
			MinimumProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
		}

		internal override void InitializeUpDownEditor(UpDownBase<TType?> editor)
		{
			base.InitializeUpDownEditor(editor);
			CommonNumericUpDown<TType> element = (CommonNumericUpDown<TType>)editor;
			UpdateProperty(element, CommonNumericUpDown<TType>.ParsingNumberStyleProperty, ParsingNumberStyleProperty);
			UpdateProperty(element, UpDownBase<TType?>.ClipValueToMinMaxProperty, ClipValueToMinMaxProperty);
			UpdateProperty(element, NumericUpDown<TType?>.FormatStringProperty, FormatStringProperty);
			UpdateProperty(element, NumericUpDown<TType?>.IncrementProperty, IncrementProperty);
			UpdateProperty(element, UpDownBase<TType?>.MaximumProperty, MaximumProperty);
			UpdateProperty(element, UpDownBase<TType?>.MinimumProperty, MinimumProperty);
			UpdateProperty(element, NumericUpDown<TType?>.AutoSelectBehaviorProperty, AutoSelectBehaviorProperty);
			UpdateProperty(element, UpDownBase<TType?>.UpdateValueOnEnterKeyProperty, UpdateValueOnEnterKeyProperty);
		}
	}
}
