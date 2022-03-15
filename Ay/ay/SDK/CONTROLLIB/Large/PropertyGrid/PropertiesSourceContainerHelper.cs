using System;
using System.Collections;
using Xceed.Wpf.Toolkit.Core;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class PropertiesSourceContainerHelper : PropertiesContainerHelperBase
	{
		private static readonly string ReadOnlyCollectionExceptionMessage = "You cannot modify this collection directly when using PropertiesSource with the PropertyGrid. Have the content of PropertiesSource implement IList and INotifyCollectionChanged then modify the orignial source";

		public PropertiesSourceContainerHelper(IPropertyContainer propertyContainer, IEnumerable propertiesSource)
			: base(propertyContainer)
		{
			if (propertiesSource == null)
			{
				throw new ArgumentNullException("propertiesSource");
			}
			IList list = propertiesSource as IList;
			if (list == null)
			{
				list = new ArrayList();
				if (propertiesSource != null)
				{
					foreach (object item in propertiesSource)
					{
						list.Add(item);
					}
				}
			}
			list = new WeakCollectionChangedWrapper(list);
			base.CollectionView = new PropertiesCollectionView(list, ReadOnlyCollectionExceptionMessage);
		}

		public override void ClearHelper()
		{
			WeakCollectionChangedWrapper weakCollectionChangedWrapper = base.CollectionView.SourceCollection as WeakCollectionChangedWrapper;
			if (weakCollectionChangedWrapper != null)
			{
				weakCollectionChangedWrapper.ReleaseEvents();
			}
			base.ClearHelper();
		}
	}
}
