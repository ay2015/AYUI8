namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public class CategoryDefinition : DefinitionBase
	{
		private string _name;

		private int? _displayOrder = 2147483647;

		private bool _isBrowsable = true;

		private bool _isExpanded = true;

		/// <summary>Gets or sets the name of the category.</summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				ThrowIfLocked(() => Name);
				_name = value;
			}
		}

		/// <summary>Gets or sets a value representing the display order of the category.</summary>
		public int? DisplayOrder
		{
			get
			{
				return _displayOrder;
			}
			set
			{
				ThrowIfLocked(() => DisplayOrder);
				_displayOrder = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the category and its properties will be shown in the property grid. (NOTE: This is a PLUS feature only).</summary>
		public bool IsBrowsable
		{
			get
			{
				return _isBrowsable;
			}
			set
			{
				ThrowIfLocked(() => IsBrowsable);
				_isBrowsable = value;
			}
		}

		/// <summary>Gets or sets if a category is expanded when loading the PropertyGrid with this category.</summary>
		public bool IsExpanded
		{
			get
			{
				return _isExpanded;
			}
			set
			{
				ThrowIfLocked(() => IsExpanded);
				_isExpanded = value;
			}
		}
	}
}
