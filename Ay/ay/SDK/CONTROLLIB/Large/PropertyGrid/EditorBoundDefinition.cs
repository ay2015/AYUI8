using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class for various editor definitions of the PropertyGrid.</summary>
	public abstract class EditorBoundDefinition : EditorDefinitionBase
	{
		public static readonly DependencyProperty EditingElementStyleProperty = DependencyProperty.Register("EditingElementStyle", typeof(Style), typeof(EditorBoundDefinition), new UIPropertyMetadata(null));

		/// <summary>Gets or sets the Binding that will be used to privide the value for the editor.</summary>
		public BindingBase Binding
		{
			get;
			set;
		}

		public Style EditingElementStyle
		{
			get
			{
				return (Style)GetValue(EditingElementStyleProperty);
			}
			set
			{
				SetValue(EditingElementStyleProperty, value);
			}
		}

		public EditorBoundDefinition()
		{
			
		}

		internal void UpdateBinding(FrameworkElement element, DependencyProperty valueDP, PropertyItemBase propertyItem)
		{
			BindingBase bindingBase = Binding ?? PropertyGridUtilities.GetDefaultBinding(propertyItem);
			if (bindingBase == null)
			{
				BindingOperations.ClearBinding(element, valueDP);
			}
			else
			{
				BindingOperations.SetBinding(element, valueDP, bindingBase);
			}
		}

		internal virtual void UpdateStyle(FrameworkElement element)
		{
			if (EditingElementStyle != null)
			{
				element.Style = EditingElementStyle;
			}
		}
	}
}
