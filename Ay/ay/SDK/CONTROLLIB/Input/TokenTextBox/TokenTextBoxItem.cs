using System.Windows;
using System.Windows.Input;


namespace ay.Controls
{
	public class TokenTextBoxItem : SDK.CONTROLLIB.Primitive.SelectorItem
    {
		private static readonly DependencyPropertyKey IsHighlightedPropertyKey;

		public static readonly DependencyProperty IsHighlightedProperty;

		internal static readonly RoutedEvent RequestHighlightEvent;

		internal static readonly RoutedEvent RequestSelectionEvent;

		public bool IsHighlighted
		{
			get
			{
				return (bool)GetValue(IsHighlightedProperty);
			}
			protected set
			{
				SetValue(IsHighlightedPropertyKey, value);
			}
		}

		internal void SetIsHighlighted(bool isHighlighted)
		{
			IsHighlighted = isHighlighted;
		}

		static TokenTextBoxItem()
		{
			IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(TokenTextBoxItem), new PropertyMetadata(false));
			IsHighlightedProperty = IsHighlightedPropertyKey.DependencyProperty;
			RequestHighlightEvent = EventManager.RegisterRoutedEvent("RequestHighlight", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TokenTextBoxItem));
			RequestSelectionEvent = EventManager.RegisterRoutedEvent("RequestSelection", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TokenTextBoxItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TokenTextBoxItem), new FrameworkPropertyMetadata(typeof(TokenTextBoxItem)));
		}

		public TokenTextBoxItem()
		{
			base.MouseMove += TokenizedTextBoxItem_MouseMove;
			base.MouseLeftButtonUp += TokenizedTextBoxItem_MouseLeftButtonUp;
			base.MouseLeftButtonDown += TokenizedTextBoxItem_MouseLeftButtonDown;
		}

		private void TokenizedTextBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			TokenizedTextBoxItem_MouseMove(sender, e);
		}

		private void TokenizedTextBoxItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (!IsHighlighted)
				{
					RaiseEvent(new RoutedEventArgs(RequestHighlightEvent));
				}
				e.Handled = true;
			}
		}

		private void TokenizedTextBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (IsHighlighted)
			{
				RaiseEvent(new RoutedEventArgs(RequestSelectionEvent));
				e.Handled = true;
			}
		}
	}
}
