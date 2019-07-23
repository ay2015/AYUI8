using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for Buttons in the Toolkit.</summary>
	public class ButtonAutomationPeer : GenericAutomationPeer
	{
		public ButtonAutomationPeer(ButtonBase owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}
	}
}
