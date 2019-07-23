using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	/// <summary>The AutomationPeer class used for ProgressBars in the Toolkit.</summary>
	public class ProgressBarAutomationPeer : GenericAutomationPeer
	{
		public ProgressBarAutomationPeer(ProgressBar owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ProgressBar;
		}
	}
}
