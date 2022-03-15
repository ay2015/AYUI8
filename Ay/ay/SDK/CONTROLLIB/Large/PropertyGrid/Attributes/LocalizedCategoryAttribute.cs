using System;
using System.ComponentModel;

namespace Xceed.Wpf.Toolkit.Core.Attributes
{
	public class LocalizedCategoryAttribute : CategoryAttribute
	{
		private LocalizationHelper _localizationHelper;

		private string _descriptionResourceKey;

		public string CategoryValue
		{
			get
			{
				return _descriptionResourceKey;
			}
		}

		public virtual string LocalizedCategory
		{
			get
			{
				return _localizationHelper.GetString(_descriptionResourceKey);
			}
		}

		public LocalizedCategoryAttribute(string descriptionResourceKey, Type resourceClassType)
			: base(descriptionResourceKey)
		{
			_localizationHelper = new LocalizationHelper(resourceClassType);
			_descriptionResourceKey = descriptionResourceKey;
		}

		protected override string GetLocalizedString(string value)
		{
			return LocalizedCategory ?? base.GetLocalizedString(value);
		}
	}
}
