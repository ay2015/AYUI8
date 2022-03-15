using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>This Control is intended to be used in the template of the PropertyItemBase and PropertyGrid classes to contain the sub-children properties.</summary>
	public class PropertyItemsControl : ItemsControl
	{
		internal static readonly RoutedEvent PreparePropertyItemEvent = EventManager.RegisterRoutedEvent("PreparePropertyItem", RoutingStrategy.Bubble, typeof(PropertyItemEventHandler), typeof(PropertyItemsControl));

		internal static readonly RoutedEvent ClearPropertyItemEvent = EventManager.RegisterRoutedEvent("ClearPropertyItem", RoutingStrategy.Bubble, typeof(PropertyItemEventHandler), typeof(PropertyItemsControl));

		internal event PropertyItemEventHandler PreparePropertyItem
		{
			add
			{
				AddHandler(PreparePropertyItemEvent, value);
			}
			remove
			{
				RemoveHandler(PreparePropertyItemEvent, value);
			}
		}

		internal event PropertyItemEventHandler ClearPropertyItem
		{
			add
			{
				AddHandler(ClearPropertyItemEvent, value);
			}
			remove
			{
				RemoveHandler(ClearPropertyItemEvent, value);
			}
		}

		public PropertyItemsControl()
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, new Attribute[1]
			{
				new PropertyFilterAttribute(PropertyFilterOptions.All)
			});
			PropertyDescriptor propertyDescriptor = properties.Find("VirtualizingPanel.IsVirtualizingWhenGrouping", false);
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(this, true);
			}
			PropertyDescriptor propertyDescriptor2 = properties.Find("VirtualizingPanel.CacheLengthUnit", false);
			if (propertyDescriptor2 != null)
			{
				propertyDescriptor2.SetValue(this, Enum.ToObject(propertyDescriptor2.PropertyType, 1));
			}
		}

		private void RaisePreparePropertyItemEvent(PropertyItemBase propertyItem, object item)
		{
			RaiseEvent(new PropertyItemEventArgs(PreparePropertyItemEvent, this, propertyItem, item));
		}

		private void RaiseClearPropertyItemEvent(PropertyItemBase propertyItem, object item)
		{
			RaiseEvent(new PropertyItemEventArgs(ClearPropertyItemEvent, this, propertyItem, item));
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is PropertyItemBase;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListPropertyItem();
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			RaisePreparePropertyItemEvent((PropertyItemBase)element, item);
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			RaiseClearPropertyItemEvent((PropertyItemBase)element, item);
			base.ClearContainerForItemOverride(element, item);
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ay.UIAutomation.ItemsControlAutomationPeer(this);
		}
	}
}
