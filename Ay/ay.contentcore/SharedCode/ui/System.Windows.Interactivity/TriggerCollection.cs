namespace System.Windows.Interactivity
{
	public sealed class TriggerCollection : AttachableCollection<TriggerBase>
	{
		internal TriggerCollection()
		{
		}

		protected override void OnAttached()
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TriggerBase current = enumerator.Current;
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
					TriggerBase current = enumerator.Current;
					current.Detach();
				}
			}
		}

		internal override void ItemAdded(TriggerBase item)
		{
			if (base.AssociatedObject != null)
			{
				item.Attach(base.AssociatedObject);
			}
		}

		internal override void ItemRemoved(TriggerBase item)
		{
			if (((IAttachedObject)item).AssociatedObject != null)
			{
				item.Detach();
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new TriggerCollection();
		}
	}
}
