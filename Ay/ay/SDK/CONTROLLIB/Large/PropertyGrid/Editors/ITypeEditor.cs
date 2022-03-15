using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Editors
{
	/// <summary>Provides an interface that is implemented by TypeEditor and custom editors classes/controls.</summary>
	public interface ITypeEditor
	{
		/// <summary>Resolves the editor of the passed PropertyItem.</summary>
		/// <returns>A FrameworkElement representing the returned editor.</returns>
		/// <param name="propertyItem">The PropertyItem whose editor will be resolved.</param>
		FrameworkElement ResolveEditor(PropertyItem propertyItem);
	}
}
