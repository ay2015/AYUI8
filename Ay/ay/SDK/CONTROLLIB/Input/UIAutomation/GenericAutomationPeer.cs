using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;

namespace ay.UIAutomation
{
	public class GenericAutomationPeer : FrameworkElementAutomationPeer
	{
		public GenericAutomationPeer(FrameworkElement owner)
			: base(owner)
		{
		}

		protected override string GetClassNameCore()
		{
			string classNameCore = base.GetClassNameCore();
			if (!string.IsNullOrEmpty(classNameCore))
			{
				return classNameCore;
			}
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
			string name = AutomationProperties.GetName(base.Owner);
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			return GetClassNameCore();
		}
	}
}
