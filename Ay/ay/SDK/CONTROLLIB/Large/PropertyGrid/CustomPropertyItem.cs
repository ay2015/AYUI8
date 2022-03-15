using System;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Used when properties are provided using a list source of items (e.g., Properties or PropertiesSource).</summary>
	public class CustomPropertyItem : PropertyItemBase
	{
		/// <summary>Identifies the Category dependency property.</summary>
		public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(CustomPropertyItem), new UIPropertyMetadata(null));

		private int _categoryOrder;

		private bool _isCategoryExpanded = true;

		/// <summary>Identifies the PropertyOrder dependency property.</summary>
		public static readonly DependencyProperty PropertyOrderProperty = DependencyProperty.Register("PropertyOrder", typeof(int), typeof(CustomPropertyItem), new UIPropertyMetadata(0));

		/// <summary>Identifies the Value dependency property.</summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(CustomPropertyItem), new UIPropertyMetadata(null, OnValueChanged, OnCoerceValueChanged));

		/// <summary>Gets or sets a value representing the name of the category.</summary>
		public string Category
		{
			get
			{
				return (string)GetValue(CategoryProperty);
			}
			set
			{
				SetValue(CategoryProperty, value);
			}
		}

		/// <summary>
		///   <span style="WHITE-SPACE: normal; WORD-SPACING: 0px; TEXT-TRANSFORM: none; FLOAT: none; COLOR: rgb(0,0,0); FONT: 13px &amp;quot;Segoe UI&amp;quot;, Verdana, Arial; DISPLAY: inline !important; LETTER-SPACING: normal; TEXT-INDENT: 0px; -webkit-text-stroke-width: 0px">
		/// Gets a value representing the order in which the category containing this property will appear in the PropertyGrid relative to the other categories when the <strong>PropertyGrid</strong>
		/// is set to <strong>Categorized</strong> mode.</span>
		/// </summary>
		public int CategoryOrder
		{
			get
			{
				return _categoryOrder;
			}
			set
			{
				if (_categoryOrder != value)
				{
					_categoryOrder = value;
					RaisePropertyChanged(() => CategoryOrder);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the category is expanded.</summary>
		public bool IsCategoryExpanded
		{
			get
			{
				return _isCategoryExpanded;
			}
			set
			{
				_isCategoryExpanded = value;
				RaisePropertyChanged("IsCategoryExpanded");
			}
		}

		/// <summary>
		///   <span style="WHITE-SPACE: normal; WORD-SPACING: 0px; TEXT-TRANSFORM: none; FLOAT: none; COLOR: rgb(0,0,0); FONT: 13px &amp;quot;Segoe UI&amp;quot;, Verdana, Arial; DISPLAY: inline !important; LETTER-SPACING: normal; TEXT-INDENT: 0px; -webkit-text-stroke-width: 0px">
		/// Gets or sets a value representing the order in which this property will appear in its Category section relative to the other properties in that category when
		/// the <strong>PropertyGrid</strong> is set to <strong>Categorized</strong> mode.</span>
		/// </summary>
		public int PropertyOrder
		{
			get
			{
				return (int)GetValue(PropertyOrderProperty);
			}
			set
			{
				SetValue(PropertyOrderProperty, value);
			}
		}

		/// <summary>Gets or sets the an object that represents the value of the property.</summary>
		public object Value
		{
			get
			{
				return GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public CustomPropertyItem()
		{
			if (base.ContainerHelper != null)
			{
				base.ContainerHelper.ClearHelper();
			}
			base.ContainerHelper = new PropertiesContainerHelper(this);
		}

		internal CustomPropertyItem(bool isPropertyGridCategorized, bool isSortedAlphabetically, bool IsExpandingNonPrimitiveTypes)
		{
			_isPropertyGridCategorized = isPropertyGridCategorized;
			_isSortedAlphabetically = isSortedAlphabetically;
			_isExpandingNonPrimitiveTypes = IsExpandingNonPrimitiveTypes;
		}

		private static object OnCoerceValueChanged(DependencyObject o, object baseValue)
		{
			CustomPropertyItem customPropertyItem = o as CustomPropertyItem;
			if (customPropertyItem != null)
			{
				return customPropertyItem.OnCoerceValueChanged(baseValue);
			}
			return baseValue;
		}

		protected virtual object OnCoerceValueChanged(object baseValue)
		{
			return baseValue;
		}

		private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			CustomPropertyItem customPropertyItem = o as CustomPropertyItem;
			if (customPropertyItem != null)
			{
				customPropertyItem.OnValueChanged(e.OldValue, e.NewValue);
			}
		}

		protected virtual void OnValueChanged(object oldValue, object newValue)
		{
			if (base.IsInitialized)
			{
				RaiseEvent(new PropertyValueChangedEventArgs(PropertyGrid.PropertyValueChangedEvent, this, oldValue, newValue));
			}
		}

		protected override Type GetPropertyItemType()
		{
			return Value.GetType();
		}

		protected override void OnEditorChanged(FrameworkElement oldValue, FrameworkElement newValue)
		{
			if (oldValue != null)
			{
				oldValue.DataContext = null;
			}
			if (newValue != null && newValue.DataContext == null)
			{
				newValue.DataContext = this;
			}
		}
	}
}
