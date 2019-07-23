using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for ItemsControl in the Toolkit.</summary>
	public class ItemsControlAutomationPeer : GenericAutomationPeer
	{
		public ItemsControlAutomationPeer(FrameworkElement owner)
			: base(owner)
		{
		}

		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			GetChildPeers(base.Owner, list);
			return list;
		}

		private void GetChildPeers(UIElement element, List<AutomationPeer> list)
		{
			Rect rect = new Rect(0.0, 0.0, ((FrameworkElement)base.Owner).ActualWidth, ((FrameworkElement)base.Owner).ActualHeight);
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
				if (frameworkElement != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(frameworkElement);
					if (automationPeer != null && !(automationPeer is ScrollViewerAutomationPeer) && !(automationPeer is GroupItemAutomationPeer) && !automationPeer.IsOffscreen())
					{
						Rect rect2 = frameworkElement.TransformToAncestor(base.Owner).TransformBounds(new Rect(0.0, 0.0, frameworkElement.RenderSize.Width, frameworkElement.RenderSize.Height));
						if (rect.IntersectsWith(rect2))
						{
							list.Add(automationPeer);
						}
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
