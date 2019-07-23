using System;
using System.Collections.Generic;
using System.Windows;

namespace ay.Wpf.Theme.Element
{
	public class ElementStylesBase : ResourceDictionary
	{
		internal List<Type> MergedDatas
		{
			get;
			set;
		}
		public ElementStylesBase()
		{
			MergedDatas = new List<Type>();
		}

		public ElementStylesBase(List<Type> mergedDatas)
		{
			MergedDatas = mergedDatas;
		}

		public virtual void Initialize()
		{
		}

		internal void EnsureResource(object key, object value)
		{
			if (Contains(key))
			{
				Remove(key);
			}
			Add(key, value);
		}
	}
}
