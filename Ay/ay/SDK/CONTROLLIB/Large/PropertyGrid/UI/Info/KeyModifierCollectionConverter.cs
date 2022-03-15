using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;

namespace Xceed.Wpf.Toolkit.Core.Input
{
	/// <summary>Converts between KeyModifier values and string representations.</summary>
	public sealed class KeyModifierCollectionConverter : TypeConverter
	{
		private static readonly TypeConverter _keyModifierConverter = TypeDescriptor.GetConverter(typeof(KeyModifier));

		/// <summary>Returns whether conversation can be done.</summary>
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type type)
		{
			return _keyModifierConverter.CanConvertFrom(typeDescriptorContext, type);
		}

		/// <summary>Returns whether conversion can be done.</summary>
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type type)
		{
			if (!(type == typeof(InstanceDescriptor)) && !(type == typeof(KeyModifierCollection)))
			{
				return type == typeof(string);
			}
			return true;
		}

		/// <summary>Converts a string delimited by '+', ' ', '|', or ',' to a KeyModifierCollection containing the <see cref="Xceed.Wpf.Toolkit~Xceed.Wpf.Toolkit.Core.Input.KeyModifier.html">KeyModifier</see> values corresponding to the substrings of the passed string.</summary>
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value)
		{
			KeyModifierCollection keyModifierCollection = new KeyModifierCollection();
			string text = value as string;
			if (value == null || (text != null && text.Trim() == string.Empty))
			{
				keyModifierCollection.Add(KeyModifier.None);
			}
			else
			{
				string[] array = text.Split(new char[4]
				{
					'+',
					' ',
					'|',
					','
				}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string value2 in array)
				{
					keyModifierCollection.Add((KeyModifier)_keyModifierConverter.ConvertFrom(typeDescriptorContext, cultureInfo, value2));
				}
				if (keyModifierCollection.Count == 0)
				{
					keyModifierCollection.Add(KeyModifier.None);
				}
			}
			return keyModifierCollection;
		}

		/// <summary>Converts between KeyModifier values and string representations.</summary>
		/// <returns>A "+" delimited string representation of the KeyModifiers.</returns>
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null || ((KeyModifierCollection)value).Count == 0)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					object result = null;
					try
					{
						result = ConstructInstanceDescriptor();
						return result;
					}
					catch (SecurityException)
					{
						return result;
					}
				}
				if (destinationType == typeof(string))
				{
					return _keyModifierConverter.ConvertTo(typeDescriptorContext, cultureInfo, KeyModifier.None, destinationType);
				}
			}
			if (destinationType == typeof(string))
			{
				string text = string.Empty;
				{
					foreach (KeyModifier item in (KeyModifierCollection)value)
					{
						if (text != string.Empty)
						{
							text += '+';
						}
						text += _keyModifierConverter.ConvertTo(typeDescriptorContext, cultureInfo, item, destinationType);
					}
					return text;
				}
			}
			return null;
		}

		private static object ConstructInstanceDescriptor()
		{
			ConstructorInfo constructor = typeof(KeyModifierCollection).GetConstructor(new Type[0]);
			return new InstanceDescriptor(constructor, new object[0]);
		}
	}
}
