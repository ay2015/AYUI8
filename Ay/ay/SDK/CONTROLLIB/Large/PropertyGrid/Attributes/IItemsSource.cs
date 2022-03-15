namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>
	///   <para>Provides an interface that is implemented by classes when a scenario calls for use of a collection of values represented by a ComboBox for a given
	/// property.</para>
	/// </summary>
	public interface IItemsSource
	{
		ItemCollection GetValues();
	}
}
