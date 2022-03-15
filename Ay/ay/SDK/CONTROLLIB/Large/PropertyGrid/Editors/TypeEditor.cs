using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>
	///   <para>Represents a generic type definition of the base editor class of the built-in PropertyGrid editors.</para>
	/// </summary>
	/// <typeparam name="T">The type of element the editor is capable of editing..</typeparam>
	public abstract class TypeEditor<T> : ITypeEditor where T : FrameworkElement, new()
	{
		/// <summary>Gets or sets the editor of the TypeEditor.</summary>
		protected T Editor
		{
			get;
			set;
		}

		/// <summary>Gets or sets the value property of the TypeEditor.</summary>
		protected DependencyProperty ValueProperty
		{
			get;
			set;
		}

		/// <summary>Resolves the editor of the passed PropertyItem.</summary>
		/// <returns>
		///   <para>A FrameworkElement representing the returned editor.</para>
		/// </returns>
		/// <param name="propertyItem">The PropertyItem whose editor will be resolved.</param>
		public virtual FrameworkElement ResolveEditor(PropertyItem propertyItem)
		{
			Editor = CreateEditor();
			SetValueDependencyProperty();
			SetControlProperties(propertyItem);
			ResolveValueBinding(propertyItem);
			return Editor;
		}

		protected virtual T CreateEditor()
		{
			return new T();
		}

		/// <summary>Creates a value converter for the TypeEditor.</summary>
		protected virtual IValueConverter CreateValueConverter()
		{
			return null;
		}

		/// <summary>Resolves the binding of the value property of the passed PropertyItem.</summary>
		/// <param name="propertyItem">The PropertyItem whose value property binding will be resolved.</param>
		protected virtual void ResolveValueBinding(PropertyItem propertyItem)
		{
			Binding binding = new Binding("Value");
			binding.Source = propertyItem;
			binding.UpdateSourceTrigger = ((Editor is InputBase) ? UpdateSourceTrigger.PropertyChanged : UpdateSourceTrigger.Default);
			binding.Mode = (propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay);
			binding.Converter = CreateValueConverter();
			BindingOperations.SetBinding(Editor, ValueProperty, binding);
		}

		/// <summary>Sets the properties of the control.</summary>
		protected virtual void SetControlProperties(PropertyItem propertyItem)
		{
		}

		/// <summary>Sets the value dependency property.</summary>
		protected abstract void SetValueDependencyProperty();
	}
}
