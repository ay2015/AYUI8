using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for TabControls in the Toolkit.</summary>
	public class TabControlAutomationPeer : GenericAutomationPeer
	{
		public TabControlAutomationPeer(TabControl owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Tab;
		}
	}
}
