using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ay.contentcore
{
	public class AdornerContainer : Adorner
	{
		private UIElement child;

		public UIElement Child
		{
			get
			{
				return child;
			}
			set
			{
				AddVisualChild(value);
				child = value;
			}
		}

		protected override int VisualChildrenCount
		{
			get
			{
				if (child != null)
				{
					return 1;
				}
				return 0;
			}
		}

		public AdornerContainer(UIElement adornedElement)
			: base(adornedElement)
		{
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (child != null)
			{
				child.Arrange(new Rect(finalSize));
			}
			return finalSize;
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index == 0 && child != null)
			{
				return child;
			}
			return base.GetVisualChild(index);
		}
	}
}
