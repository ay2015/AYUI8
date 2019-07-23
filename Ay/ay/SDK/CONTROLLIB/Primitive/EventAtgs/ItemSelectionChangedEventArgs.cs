using System.Windows;

namespace ay.SDK.CONTROLLIB.Primitive
{
    /// <summary>Represents the method that will handle the ItemSelectionChanged routed event.</summary>
    public delegate void ItemSelectionChangedEventHandler(object sender, ItemSelectionChangedEventArgs e);
    /// <summary>Provides information used in the ItemSelectionChanged routed
    /// event.</summary>
    public class ItemSelectionChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Gets a value indicating whether the item is selected.</summary>
		public bool IsSelected
		{
			get;
			private set;
		}

		/// <summary>Gets the item that has changed.</summary>
		public object Item
		{
			get;
			private set;
		}

		/// <summary>Initializes a new instance of the ItemSelectionChangedEventArgs class.</summary>
		/// <param name="routedEvent">
		///   <span>
		///     <span>The routed event identifier for this instance of the ItemSelectionChangedEventArgs class.</span>
		///   </span>
		/// </param>
		/// <param name="source">An alternate source that will be reported when the event is handled. This pre-populates the Source property.</param>
		public ItemSelectionChangedEventArgs(RoutedEvent routedEvent, object source, object item, bool isSelected)
			: base(routedEvent, source)
		{
			Item = item;
			IsSelected = isSelected;
		}
	}
}
