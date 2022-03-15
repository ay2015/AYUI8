using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of PropertyDefinition.</summary>
	public abstract class PropertyDefinitionBase : DefinitionBase
	{
		private IList _targetProperties;

		private PropertyDefinitionCollection _propertyDefinitions;

		[TypeConverter(typeof(ListConverter))]
		public IList TargetProperties
		{
			get
			{
				return _targetProperties;
			}
			set
			{
				ThrowIfLocked(() => TargetProperties);
				_targetProperties = value;
			}
		}

		/// <summary>Gets or sets the Collection of PropertyDefinition to specify which sub-PropertyItems should be shown in the PropertyGrid.</summary>
		public PropertyDefinitionCollection PropertyDefinitions
		{
			get
			{
				return _propertyDefinitions;
			}
			set
			{
				ThrowIfLocked(() => PropertyDefinitions);
				_propertyDefinitions = value;
			}
		}

		internal PropertyDefinitionBase()
		{
			_targetProperties = new List<object>();
			PropertyDefinitions = new PropertyDefinitionCollection();
		}

		internal override void Lock()
		{
			if (!base.IsLocked)
			{
				base.Lock();
				List<object> list = new List<object>();
				if (_targetProperties != null)
				{
					foreach (object targetProperty in _targetProperties)
					{
						object obj = targetProperty;
						TargetPropertyType targetPropertyType = obj as TargetPropertyType;
						if (targetPropertyType != null)
						{
							obj = targetPropertyType.Type;
						}
						list.Add(obj);
					}
				}
				object targetProperties;
				if (!DesignerProperties.GetIsInDesignMode(this))
				{
					IList list2 = new ReadOnlyCollection<object>(list);
					targetProperties = list2;
				}
				else
				{
					targetProperties = new Collection<object>(list);
				}
				_targetProperties = (IList)targetProperties;
			}
		}
	}
}
