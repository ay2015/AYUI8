using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	public class SliderAutomationPeer : GenericAutomationPeer
	{
		public SliderAutomationPeer(Slider owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Slider;
		}
	}
}
