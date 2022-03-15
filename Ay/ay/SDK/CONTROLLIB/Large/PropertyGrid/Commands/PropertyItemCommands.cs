using System.Windows.Input;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Commands
{
	/// <summary>Represents commands related to PropertyItem.</summary>
	public static class PropertyItemCommands
	{
		private static RoutedCommand _resetValueCommand = new RoutedCommand();

		/// <summary>Gets the ResetValue routed command.</summary>
		public static RoutedCommand ResetValue
		{
			get
			{
				return _resetValueCommand;
			}
		}
	}
}
