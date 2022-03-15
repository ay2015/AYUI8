using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Xceed.Wpf.Toolkit.Primitives;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Represents a base class for up/down editors.</summary>
	public class UpDownEditor<TEditor, TType> : TypeEditor<TEditor> where TEditor : UpDownBase<TType>, new()
	{
		protected override void SetControlProperties(PropertyItem propertyItem)
		{
			TEditor editor = base.Editor;
			editor.TextAlignment = TextAlignment.Left;
		}

		protected override void SetValueDependencyProperty()
		{
			base.ValueProperty = UpDownBase<TType>.ValueProperty;
		}

		internal void SetMinMaxFromRangeAttribute(PropertyDescriptor propertyDescriptor, TypeConverter converter)
		{
			if (propertyDescriptor != null)
			{
				RangeAttribute attribute = PropertyGridUtilities.GetAttribute<RangeAttribute>(propertyDescriptor);
				if (attribute != null)
				{
					TEditor editor = base.Editor;
					editor.Maximum = (TType)converter.ConvertFrom(attribute.Maximum.ToString());
					TEditor editor2 = base.Editor;
					editor2.Minimum = (TType)converter.ConvertFrom(attribute.Minimum.ToString());
				}
			}
		}
	}
}
