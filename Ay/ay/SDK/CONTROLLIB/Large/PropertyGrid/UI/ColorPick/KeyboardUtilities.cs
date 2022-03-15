using System.Windows.Input;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal class KeyboardUtilities
	{
		internal static bool IsKeyModifyingPopupState(KeyEventArgs e)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt || (e.SystemKey != Key.Down && e.SystemKey != Key.Up))
			{
				return e.Key == Key.F4;
			}
			return true;
		}
	}
}
