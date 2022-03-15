namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class PropertiesContainerHelper : PropertiesContainerHelperBase
	{
		public PropertiesContainerHelper(IPropertyContainer propertyContainer)
			: base(propertyContainer)
		{
			base.CollectionView = new PropertiesCollectionView();
		}

		public override void OnEndInit()
		{
			base.OnEndInit();
			base.CollectionView.Refresh();
		}
	}
}
