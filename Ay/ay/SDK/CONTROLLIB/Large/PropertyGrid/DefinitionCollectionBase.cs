using System;
using System.Collections.ObjectModel;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public abstract class DefinitionCollectionBase<T> : ObservableCollection<T> where T : DefinitionBase
	{
		internal DefinitionCollectionBase()
		{
		}

		protected override void InsertItem(int index, T item)
		{
			if (item == null)
			{
				throw new InvalidOperationException("Cannot insert null items in the collection.");
			}
			item.Lock();
			base.InsertItem(index, item);
		}

		protected override void SetItem(int index, T item)
		{
			if (item == null)
			{
				throw new InvalidOperationException("Cannot insert null items in the collection.");
			}
			item.Lock();
			base.SetItem(index, item);
		}
	}
}
