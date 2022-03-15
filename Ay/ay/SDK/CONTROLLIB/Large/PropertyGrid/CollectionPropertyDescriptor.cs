using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	internal class CollectionPropertyDescriptor : PropertyDescriptor
	{
		private object _item;

		private string _name;

		public object Item
		{
			get
			{
				return _item;
			}
		}

		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection((Attribute[])null);
			}
		}

		public override Type ComponentType
		{
			get
			{
				return Item.GetType();
			}
		}

		public override string DisplayName
		{
			get
			{
				return _name;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public override string Name
		{
			get
			{
				return _name;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return Item.GetType();
			}
		}

		public CollectionPropertyDescriptor(IEnumerable collection, object item)
			: base((!string.IsNullOrEmpty(item.ToString())) ? item.ToString() : item.GetType().Name, null)
		{
			_item = item;
			int num = 0;
			foreach (object item2 in collection)
			{
				num++;
				if (item2.Equals(Item))
				{
					Type type = Item.GetType();
					MethodInfo method = type.GetMethod("ToString");
					if (method != null && type.IsSubclassOf(method.DeclaringType))
					{
						_name = Item.ToString() + " " + num;
					}
					else
					{
						_name = Item.ToString();
					}
					break;
				}
			}
		}

		public override bool CanResetValue(object component)
		{
			return true;
		}

		public override object GetValue(object component)
		{
			return Item;
		}

		public override void ResetValue(object component)
		{
		}

		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		public override void SetValue(object component, object value)
		{
			_item = value;
		}
	}
}
