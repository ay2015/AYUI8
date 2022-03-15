using System.Windows;
using System.Windows.Controls;

namespace ay.Controls
{
	public class TokenItem : ContentControl
	{
		private object _userItem;

		public static readonly DependencyProperty IsValidProperty;

		public bool IsValid
		{
			get
			{
				return (bool)GetValue(IsValidProperty);
			}
			set
			{
				SetValue(IsValidProperty, value);
			}
		}

		internal object UserItem
		{
			get
			{
				return _userItem;
			}
			set
			{
				_userItem = value;
			}
		}

		internal void SetIsValidInternal(bool value)
		{
			SetCurrentValue(IsValidProperty, value);
		}

		static TokenItem()
		{
			IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(TokenItem), new PropertyMetadata(true));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TokenItem), new FrameworkPropertyMetadata(typeof(TokenItem)));
		}

		public TokenItem()
		{
		
		}
	}
}
