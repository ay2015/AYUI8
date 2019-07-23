using System.Windows;
using System.Windows.Automation.Peers;

namespace ay.UIAutomation
{
    public class WindowAutomationPeer : GenericAutomationPeer
    {
        public WindowAutomationPeer(Window owner)
            : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Window;
        }
    }
}
