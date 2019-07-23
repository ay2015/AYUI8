using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for Panels in the Toolkit.</summary>
	public class PanelAutomationPeer : GenericAutomationPeer
	{
		public PanelAutomationPeer(Panel owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}
	}
}
