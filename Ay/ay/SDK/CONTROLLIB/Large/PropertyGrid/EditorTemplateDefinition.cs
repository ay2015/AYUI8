using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Allows an editor definition to be specified.</summary>
	public class EditorTemplateDefinition : EditorDefinitionBase
	{
		public static readonly DependencyProperty EditingTemplateProperty = DependencyProperty.Register("EditingTemplate", typeof(DataTemplate), typeof(EditorTemplateDefinition), new UIPropertyMetadata(null));

		public DataTemplate EditingTemplate
		{
			get
			{
				return (DataTemplate)GetValue(EditingTemplateProperty);
			}
			set
			{
				SetValue(EditingTemplateProperty, value);
			}
		}

		protected override FrameworkElement GenerateEditingElement(PropertyItemBase propertyItem)
		{
			if (EditingTemplate == null)
			{
				return null;
			}
			return EditingTemplate.LoadContent() as FrameworkElement;
		}
	}
}
