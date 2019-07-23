using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class RadnerLoading : UserControl
	{
		static RadnerLoading()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RadnerLoading), new FrameworkPropertyMetadata(typeof(RadnerLoading)));
		}
	}
}
