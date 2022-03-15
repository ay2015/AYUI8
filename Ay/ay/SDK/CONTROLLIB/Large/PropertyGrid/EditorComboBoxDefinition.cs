using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows use of a combobox editor in the PropertyGrid.</summary>
	public class EditorComboBoxDefinition : EditorDefinitionBase
	{
		public static readonly DependencyProperty DisplayMemberPathProperty = ItemsControl.DisplayMemberPathProperty.AddOwner(typeof(EditorComboBoxDefinition));

		public static readonly DependencyProperty SelectedValuePathProperty = Selector.SelectedValuePathProperty.AddOwner(typeof(EditorComboBoxDefinition));

		public static readonly DependencyProperty EditingElementStyleProperty = EditorBoundDefinition.EditingElementStyleProperty.AddOwner(typeof(EditorComboBoxDefinition));

		public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(EditorComboBoxDefinition));

		public string DisplayMemberPath
		{
			get
			{
				return (string)GetValue(DisplayMemberPathProperty);
			}
			set
			{
				SetValue(DisplayMemberPathProperty, value);
			}
		}

		public string SelectedValuePath
		{
			get
			{
				return (string)GetValue(SelectedValuePathProperty);
			}
			set
			{
				SetValue(SelectedValuePathProperty, value);
			}
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

		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public BindingBase SelectedItemBinding
		{
			get;
			set;
		}

		public BindingBase SelectedValueBinding
		{
			get;
			set;
		}

		public BindingBase TextBinding
		{
			get;
			set;
		}

		public EditorComboBoxDefinition()
		{
			
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			PropertyGridEditorComboBox propertyGridEditorComboBox = new PropertyGridEditorComboBox();
			UpdateProperty(propertyGridEditorComboBox, ItemsControl.DisplayMemberPathProperty, DisplayMemberPathProperty);
			UpdateProperty(propertyGridEditorComboBox, Selector.SelectedValuePathProperty, SelectedValuePathProperty);
			UpdateProperty(propertyGridEditorComboBox, ItemsControl.ItemsSourceProperty, ItemsSourceProperty);
			UpdateStyle(propertyGridEditorComboBox);
			if (SelectedItemBinding == null && SelectedValueBinding == null)
			{
				if (SelectedValuePath == null)
				{
					UpdateBinding(propertyGridEditorComboBox, Selector.SelectedItemProperty, PropertyGridUtilities.GetDefaultBinding(propertyItem));
				}
				else
				{
					UpdateBinding(propertyGridEditorComboBox, Selector.SelectedValueProperty, PropertyGridUtilities.GetDefaultBinding(propertyItem));
				}
			}
			else
			{
				UpdateBinding(propertyGridEditorComboBox, Selector.SelectedItemProperty, SelectedItemBinding);
				UpdateBinding(propertyGridEditorComboBox, Selector.SelectedValueProperty, SelectedValueBinding);
			}
			UpdateBinding(propertyGridEditorComboBox, ComboBox.TextProperty, TextBinding);
			return propertyGridEditorComboBox;
		}

		internal void UpdateStyle(FrameworkElement element)
		{
			if (EditingElementStyle != null)
			{
				element.Style = EditingElementStyle;
			}
		}

		private void UpdateBinding(FrameworkElement editor, DependencyProperty editorProperty, BindingBase binding)
		{
			if (binding == null)
			{
				BindingOperations.ClearBinding(editor, editorProperty);
			}
			else
			{
				BindingOperations.SetBinding(editor, editorProperty, binding);
			}
		}
	}
}
