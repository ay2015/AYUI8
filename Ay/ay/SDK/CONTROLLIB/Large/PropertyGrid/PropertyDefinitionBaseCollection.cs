using System;
using System.Collections.Generic;
using System.Linq;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	/// <summary>Base class of the PropertyDefinitionCollection and <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.PropertyGrid.EditorDefinitionCollection.html">EditorDefinitionCollection</see> classes.</summary>
	public abstract class PropertyDefinitionBaseCollection<T> : DefinitionCollectionBase<T> where T : PropertyDefinitionBase
	{
		public virtual T this[object propertyId]
		{
			get
			{
				foreach (T item in base.Items)
				{
					T current = item;
					if (current.TargetProperties.Contains(propertyId))
					{
						return current;
					}
					List<string> list = current.TargetProperties.OfType<string>().ToList();
					if (list != null && list.Count > 0)
					{
						if (propertyId is string)
						{
							string text = (string)propertyId;
							foreach (string item2 in list)
							{
								if (item2.Contains("*"))
								{
									string value = item2.Replace("*", "");
									if (text.StartsWith(value) || text.EndsWith(value))
									{
										return current;
									}
								}
							}
						}
					}
					else
					{
						Type type = propertyId as Type;
						if (type != null)
						{
							foreach (Type targetProperty in current.TargetProperties)
							{
								if (targetProperty.IsAssignableFrom(type))
								{
									return current;
								}
							}
						}
					}
				}
				return null;
			}
		}

		internal T GetRecursiveBaseTypes(Type type)
		{
			T val = null;
			while (val == null && type != null)
			{
				val = this[type];
				type = type.BaseType;
			}
			return val;
		}
	}
}
