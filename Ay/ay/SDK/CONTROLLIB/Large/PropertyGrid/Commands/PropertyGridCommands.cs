using System.Windows.Input;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Commands
{
	/// <summary>Represents commands related to PropertyGrid.</summary>
	public class PropertyGridCommands
	{
		private static RoutedCommand _clearFilterCommand = new RoutedCommand();

		/// <summary>Gets the ClearFilter routed command.</summary>
		public static RoutedCommand ClearFilter
		{
			get
			{
				return _clearFilterCommand;
			}
		}
	}
}
