using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for ComboBoxes in the Toolkit.</summary>
	public class ComboBoxAutomationPeer : GenericAutomationPeer
	{
		public ComboBoxAutomationPeer(ItemsControl owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ComboBox;
		}
	}
}
