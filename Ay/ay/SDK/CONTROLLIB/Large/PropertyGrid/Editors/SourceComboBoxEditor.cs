using System.Collections;
using System.ComponentModel;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents the built-in SourceComboBox editor.</summary>
	public class SourceComboBoxEditor : ComboBoxEditor
	{
		private ICollection _collection;

		private TypeConverter _typeConverter;

		public SourceComboBoxEditor(ICollection collection, TypeConverter typeConverter)
		{
			_collection = collection;
			_typeConverter = typeConverter;
		}

		protected override IEnumerable CreateItemsSource(PropertyItem propertyItem)
		{
			return _collection;
		}

		protected override IValueConverter CreateValueConverter()
		{
			if (_typeConverter != null && _typeConverter is StringConverter)
			{
				return new SourceComboBoxEditorConverter(_typeConverter);
			}
			return null;
		}
	}
}
