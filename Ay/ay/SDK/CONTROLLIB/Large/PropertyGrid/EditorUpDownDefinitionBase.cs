using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of the up-down editor definition classes.</summary>
	public class EditorUpDownDefinitionBase<TEditor, TType> : EditorBoundDefinition where TEditor : UpDownBase<TType>, new()
	{
		public static readonly DependencyProperty AllowSpinProperty = UpDownBase<TType>.AllowSpinProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty CultureInfoProperty = InputBase.CultureInfoProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty DefaultValueProperty = UpDownBase<TType>.DefaultValueProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty HorizontalContentAlignmentProperty = Control.HorizontalContentAlignmentProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>), new FrameworkPropertyMetadata(HorizontalAlignment.Left));

		public static readonly DependencyProperty MouseWheelActiveTriggerProperty = UpDownBase<TType>.MouseWheelActiveTriggerProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty ShowButtonSpinnerProperty = UpDownBase<TType>.ShowButtonSpinnerProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty TextAlignmentProperty = InputBase.TextAlignmentProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>), new UIPropertyMetadata(TextAlignment.Left));

		public static readonly DependencyProperty WatermarkProperty = InputBase.WatermarkProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

		public static readonly DependencyProperty WatermarkTemplateProperty = InputBase.WatermarkTemplateProperty.AddOwner(typeof(EditorUpDownDefinitionBase<TEditor, TType>));

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

		public TType DefaultValue
		{
			get
			{
				return (TType)GetValue(DefaultValueProperty);
			}
			set
			{
				SetValue(DefaultValueProperty, value);
			}
		}

		public HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty);
			}
			set
			{
				SetValue(HorizontalContentAlignmentProperty, value);
			}
		}

		public bool MouseWheelActiveTrigger
		{
			get
			{
				return (bool)GetValue(MouseWheelActiveTriggerProperty);
			}
			set
			{
				SetValue(MouseWheelActiveTriggerProperty, value);
			}
		}

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

		internal virtual void InitializeUpDownEditor(UpDownBase<TType> editor)
		{
			UpdateProperty(editor, InputBase.CultureInfoProperty, CultureInfoProperty);
			UpdateProperty(editor, InputBase.TextAlignmentProperty, TextAlignmentProperty);
			UpdateProperty(editor, Control.HorizontalContentAlignmentProperty, HorizontalContentAlignmentProperty);
			UpdateProperty(editor, InputBase.WatermarkProperty, WatermarkProperty);
			UpdateProperty(editor, InputBase.WatermarkTemplateProperty, WatermarkTemplateProperty);
			UpdateProperty(editor, UpDownBase<TType>.AllowSpinProperty, AllowSpinProperty);
			UpdateProperty(editor, UpDownBase<TType>.DefaultValueProperty, DefaultValueProperty);
			UpdateProperty(editor, UpDownBase<TType>.MouseWheelActiveTriggerProperty, MouseWheelActiveTriggerProperty);
			UpdateProperty(editor, UpDownBase<TType>.ShowButtonSpinnerProperty, ShowButtonSpinnerProperty);
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			TEditor val = CreateEditor();
			InitializeUpDownEditor(val);
			UpdateStyle(val);
			UpdateBinding(val, UpDownBase<TType>.ValueProperty, propertyItem);
			return val;
		}

		protected virtual TEditor CreateEditor()
		{
			return new TEditor();
		}
	}
}
