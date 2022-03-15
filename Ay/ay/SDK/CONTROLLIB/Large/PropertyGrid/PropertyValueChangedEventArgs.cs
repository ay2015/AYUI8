using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Provides information related to changes to property values.</summary>
	public class PropertyValueChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Gets or sets the new value.</summary>
		public object NewValue
		{
			get;
			set;
		}

		/// <summary>Gets or sets the old value.</summary>
		public object OldValue
		{
			get;
			set;
		}

		/// <summary>Initializes a new instance of the PropertyValueChangedEventArgs class, using the specified routed event, the source of the changed value, the old value, and
		/// the new value.</summary>
		/// <param name="routedEvent">The routed event.</param>
		/// <param name="source">The source of the changed value.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public PropertyValueChangedEventArgs(RoutedEvent routedEvent, object source, object oldValue, object newValue)
			: base(routedEvent, source)
		{
			NewValue = newValue;
			OldValue = oldValue;
		}
	}
}
