namespace System.Windows.Interactivity
{
	public sealed class BehaviorCollection : AttachableCollection<Behavior>
	{
		internal BehaviorCollection()
		{
		}

		protected override void OnAttached()
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Behavior current = enumerator.Current;
					current.Attach(base.AssociatedObject);
				}
			}
		}

		protected override void OnDetaching()
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Behavior current = enumerator.Current;
					current.Detach();
				}
			}
		}

		internal override void ItemAdded(Behavior item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		internal override void ItemRemoved(Behavior item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new BehaviorCollection();
		}
	}
}
