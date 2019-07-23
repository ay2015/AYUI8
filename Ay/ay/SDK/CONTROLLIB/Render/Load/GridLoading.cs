using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class GridLoading : UserControl
	{
		static GridLoading()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridLoading), new FrameworkPropertyMetadata(typeof(GridLoading)));
		}
	}
}
