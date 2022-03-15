using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class TokenTextBoxCoreItemsControl : ItemsControl
	{
		public static readonly DependencyProperty HighlightedItemProperty = DependencyProperty.Register("HighlightedItem", typeof(object), typeof(TokenTextBoxCoreItemsControl), new UIPropertyMetadata(null));

		public object HighlightedItem
		{
			get
			{
				return GetValue(HighlightedItemProperty);
			}
			set
			{
				SetValue(HighlightedItemProperty, value);
			}
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TokenTextBoxItem();
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			UIElement uIElement = element as UIElement;
			if (uIElement != null)
			{
				TokenTextBoxItem TokenTextBoxItem = element as TokenTextBoxItem;
				if (TokenTextBoxItem != null)
				{
					TokenTextBoxItem.SetIsHighlighted(item == HighlightedItem);
				}
			}
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			UIElement uIElement = element as UIElement;
			if (uIElement != null)
			{
				TokenTextBoxItem TokenTextBoxItem = element as TokenTextBoxItem;
				if (TokenTextBoxItem != null)
				{
					TokenTextBoxItem.SetIsHighlighted(false);
				}
			}
		}
	}
}
