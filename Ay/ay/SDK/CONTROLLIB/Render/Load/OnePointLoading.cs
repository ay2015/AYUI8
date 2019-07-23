using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class OnePointLoading : UserControl
	{
		static OnePointLoading()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OnePointLoading), new FrameworkPropertyMetadata(typeof(OnePointLoading)));
		}
	}
}
