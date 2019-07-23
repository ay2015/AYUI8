using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace System.Windows.Interactivity
{
	public abstract class AttachableCollection<T> : FreezableCollection<T>, IAttachedObject where T : DependencyObject, IAttachedObject
	{
		private Collection<T> snapshot;

		private DependencyObject associatedObject;

		protected DependencyObject AssociatedObject
		{
			get
			{
				ReadPreamble();
				return associatedObject;
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}

		internal AttachableCollection()
		{
			((INotifyCollectionChanged)this).CollectionChanged += OnCollectionChanged;
			snapshot = new Collection<T>();
		}

		protected abstract void OnAttached();

		protected abstract void OnDetaching();

		internal abstract void ItemAdded(T item);

		internal abstract void ItemRemoved(T item);

		[Conditional("DEBUG")]
		private void VerifySnapshotIntegrity()
		{
			if (base.Count == snapshot.Count)
			{
				int num = 0;
				while (true)
				{
					if (num >= base.Count)
					{
						return;
					}
					if (base[num] != snapshot[num])
					{
						break;
					}
					num++;
				}
			
			}
		}

		private void VerifyAdd(T item)
		{
			if (snapshot.Contains(item))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "DuplicateItemInCollectionExceptionMessage", new object[2]
				{
					typeof(T).Name,
					GetType().Name
				}));
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Add:
				foreach (T newItem in e.NewItems)
				{
					try
					{
						VerifyAdd(newItem);
						ItemAdded(newItem);
					}
					finally
					{
						snapshot.Insert(IndexOf(newItem), newItem);
					}
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (T oldItem in e.OldItems)
				{
					ItemRemoved(oldItem);
					snapshot.Remove(oldItem);
				}
				foreach (T newItem2 in e.NewItems)
				{
					try
					{
						VerifyAdd(newItem2);
						ItemAdded(newItem2);
					}
					finally
					{
						snapshot.Insert(IndexOf(newItem2), newItem2);
					}
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				foreach (T oldItem2 in e.OldItems)
				{
					ItemRemoved(oldItem2);
					snapshot.Remove(oldItem2);
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				foreach (T item in snapshot)
				{
					ItemRemoved(item);
				}
				snapshot = new Collection<T>();
				using (Enumerator enumerator2 = GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						T current2 = enumerator2.Current;
						VerifyAdd(current2);
						ItemAdded(current2);
					}
				}
				break;
			}
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException();
				}
				if (Interaction.ShouldRunInDesignMode || !(bool)GetValue(DesignerProperties.IsInDesignModeProperty))
				{
					WritePreamble();
					associatedObject = dependencyObject;
					WritePostscript();
				}
				OnAttached();
			}
		}

		public void Detach()
		{
			OnDetaching();
			WritePreamble();
			associatedObject = null;
			WritePostscript();
		}
	}
}
