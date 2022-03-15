using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class PropertiesCollectionView : ListCollectionView, IList, ICollection, IEnumerable
	{
		private readonly string InvalidOperationMessage;

		private IList SourceList
		{
			get
			{
				return (IList)SourceCollection;
			}
		}

		private bool IsReadOnly
		{
			get
			{
				return InvalidOperationMessage != null;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return IsReadOnly;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return SourceList[index];
			}
			set
			{
				ThrowIfReadOnly();
				SourceList[index] = value;
			}
		}

		int ICollection.Count
		{
			get
			{
				return SourceList.Count;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return SourceList.IsSynchronized;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return SourceList.SyncRoot;
			}
		}

		public PropertiesCollectionView()
			: base(new ObservableCollection<object>())
		{
			InvalidOperationMessage = null;
		}

		public PropertiesCollectionView(IList sourceList, string invalidOperationMessage)
			: base(sourceList)
		{
			InvalidOperationMessage = ((invalidOperationMessage != null) ? invalidOperationMessage : string.Empty);
		}

		private void ThrowIfReadOnly()
		{
			if (IsReadOnly)
			{
				throw new InvalidOperationException(InvalidOperationMessage);
			}
		}

		int IList.Add(object value)
		{
			ThrowIfReadOnly();
			return SourceList.Add(value);
		}

		void IList.Clear()
		{
			ThrowIfReadOnly();
			SourceList.Clear();
		}

		bool IList.Contains(object value)
		{
			return SourceList.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return SourceList.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			ThrowIfReadOnly();
			SourceList.Insert(index, value);
		}

		void IList.Remove(object value)
		{
			ThrowIfReadOnly();
			SourceList.Remove(value);
		}

		void IList.RemoveAt(int index)
		{
			ThrowIfReadOnly();
			SourceList.RemoveAt(index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			SourceList.CopyTo(array, index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return SourceList.GetEnumerator();
		}
	}
}
