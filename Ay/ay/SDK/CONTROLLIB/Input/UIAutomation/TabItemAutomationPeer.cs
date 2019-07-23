using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ay.UIAutomation
{
	public class TabItemAutomationPeer : GenericAutomationPeer
	{
		public TabItemAutomationPeer(TabItem owner)
			: base(owner)
		{
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TabItem;
		}

		protected override string GetNameCore()
		{
			string name = AutomationProperties.GetName(base.Owner);
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			object header = ((TabItem)base.Owner).Header;
			if (header == null)
			{
				return "";
			}
			return header.ToString();
		}
	}
}
