#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Represents a property definition for use in an EditorDefinition.</summary>
	public class PropertyDefinition : PropertyDefinitionBase
	{
		private string _name;

		private bool? _isBrowsable = true;

		private bool? _isExpandable = null;

		private string _displayName;

		private string _description;

		private string _category;

		private int? _displayOrder = null;

		[Obsolete("Use 'TargetProperties' instead of 'Name'")]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				Trace.TraceWarning("{0}: 'Name' property is obsolete. Instead use 'TargetProperties'. (XAML example: <t:PropertyDefinition TargetProperties=\"FirstName,LastName\" />)", typeof(PropertyDefinition));
				_name = value;
			}
		}

		public string Category
		{
			get
			{
				return _category;
			}
			set
			{
				ThrowIfLocked(() => Category);
				_category = value;
			}
		}

		public string DisplayName
		{
			get
			{
				return _displayName;
			}
			set
			{
				ThrowIfLocked(() => DisplayName);
				_displayName = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				ThrowIfLocked(() => Description);
				_description = value;
			}
		}

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

		public bool? IsBrowsable
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

		public bool? IsExpandable
		{
			get
			{
				return _isExpandable;
			}
			set
			{
				ThrowIfLocked(() => IsExpandable);
				_isExpandable = value;
			}
		}

		internal override void Lock()
		{
			if (_name != null && base.TargetProperties != null && base.TargetProperties.Count > 0)
			{
				throw new InvalidOperationException(string.Format("{0}: When using 'TargetProperties' property, do not use 'Name' property.", typeof(PropertyDefinition)));
			}
			if (_name != null)
			{
				base.TargetProperties = new List<object>
				{
					_name
				};
			}
			base.Lock();
		}
	}
}
