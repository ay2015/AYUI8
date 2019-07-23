using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	public class TextBoxAutomationPeer : System.Windows.Automation.Peers.TextBoxAutomationPeer
	{
		public TextBoxAutomationPeer(TextBox owner)
			: base(owner)
		{
		}

		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		protected override string GetAutomationIdCore()
		{
			string automationIdCore = base.GetAutomationIdCore();
			if (!string.IsNullOrEmpty(automationIdCore))
			{
				return automationIdCore;
			}
			return GetNameCore();
		}

		protected override string GetNameCore()
		{
			return ((TextBox)base.Owner).Text;
		}
	}
}
