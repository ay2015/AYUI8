namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class CategoryDefinitionCollection : DefinitionCollectionBase<CategoryDefinition>
	{
		public CategoryDefinition this[object categoryId]
		{
			get
			{
				foreach (CategoryDefinition item in base.Items)
				{
					if (object.Equals(item.Name, categoryId))
					{
						return item;
					}
				}
				return null;
			}
		}
	}
}
