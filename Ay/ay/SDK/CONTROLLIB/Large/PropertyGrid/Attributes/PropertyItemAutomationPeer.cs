using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for PropertyItems in the Toolkit.</summary>
	public class PropertyItemAutomationPeer : GenericAutomationPeer
	{
		public PropertyItemAutomationPeer(PropertyItemBase owner)
			: base(owner)
		{
		}

		protected override string GetNameCore()
		{
			string name = AutomationProperties.GetName(base.Owner);
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			return ((PropertyItemBase)base.Owner).DisplayName;
		}

		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			GetChildPeers(base.Owner, list);
			return list;
		}

		private void GetChildPeers(UIElement element, List<AutomationPeer> list)
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
				if (frameworkElement != null && (!(frameworkElement is Expander) || ((Expander)frameworkElement).IsExpanded))
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(frameworkElement);
					if (automationPeer != null)
					{
						list.Add(automationPeer);
					}
					else
					{
						GetChildPeers(frameworkElement, list);
					}
				}
			}
		}
	}
}
