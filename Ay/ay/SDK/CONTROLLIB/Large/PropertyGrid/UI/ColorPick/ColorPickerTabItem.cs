using System.Windows.Controls;
using System.Windows.Input;

namespace Xceed.Wpf.Toolkit
{
	public class ColorPickerTabItem : TabItem
	{
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (e.Source == this || !base.IsSelected)
			{
				e.Handled = true;
			}
			else
			{
				base.OnMouseLeftButtonDown(e);
			}
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (e.Source == this || !base.IsSelected)
			{
				base.OnMouseLeftButtonDown(e);
			}
			base.OnMouseLeftButtonUp(e);
		}
	}
}
