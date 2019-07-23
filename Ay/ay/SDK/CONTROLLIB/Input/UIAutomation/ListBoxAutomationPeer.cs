using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for ListBoxes in the Toolkit.</summary>
	public class ListBoxAutomationPeer : GenericAutomationPeer
	{
		public ListBoxAutomationPeer(ItemsControl owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}
	}
}
