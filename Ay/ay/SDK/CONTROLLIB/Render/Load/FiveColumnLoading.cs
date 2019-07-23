using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class FiveColumnLoading : UserControl
	{
		static FiveColumnLoading()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FiveColumnLoading), new FrameworkPropertyMetadata(typeof(FiveColumnLoading)));
		}
	}
}
