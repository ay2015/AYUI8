using System.Windows;
using System.Windows.Controls;

namespace ay.SDK.CONTROLLIB.Primitive
{
	/// <summary>Represents an item in the Selector-based control.</summary>
	public class SelectorItem : ContentControl
	{
		/// <summary>Identifies the IsSelected dependency property.</summary>
		public static readonly DependencyProperty IsSelectedProperty;

		/// <summary>Identifies the Selected routed event.</summary>
		public static readonly RoutedEvent SelectedEvent;

		/// <summary>Identifies the Unselected routed event.</summary>
		public static readonly RoutedEvent UnselectedEvent;

		/// <summary>Gets or sets a value indicating whether the SelectorItem is selected.</summary>
		public bool? IsSelected
		{
			get
			{
				return (bool?)GetValue(IsSelectedProperty);
			}
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		internal Selector ParentSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
			}
		}

		static SelectorItem()
		{
			IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool?), typeof(SelectorItem), new UIPropertyMetadata(false, OnIsSelectedChanged));
			SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(SelectorItem));
			UnselectedEvent = Selector.UnSelectedEvent.AddOwner(typeof(SelectorItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectorItem), new FrameworkPropertyMetadata(typeof(SelectorItem)));
		}

		private static void OnIsSelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			SelectorItem selectorItem = o as SelectorItem;
			if (selectorItem != null)
			{
				selectorItem.OnIsSelectedChanged((bool?)e.OldValue, (bool?)e.NewValue);
			}
		}

		protected virtual void OnIsSelectedChanged(bool? oldValue, bool? newValue)
		{
			if (newValue.HasValue)
			{
				if (newValue.Value)
				{
					RaiseEvent(new RoutedEventArgs(Selector.SelectedEvent, this));
				}
				else
				{
					RaiseEvent(new RoutedEventArgs(Selector.UnSelectedEvent, this));
				}
			}
		}
	}
}
