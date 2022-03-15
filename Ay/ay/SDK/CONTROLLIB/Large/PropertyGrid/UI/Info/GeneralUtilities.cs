using System.Windows;
using System.Windows.Data;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal sealed class GeneralUtilities : DependencyObject
	{
		internal static readonly DependencyProperty StubValueProperty = DependencyProperty.RegisterAttached("StubValue", typeof(object), typeof(GeneralUtilities), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		private GeneralUtilities()
		{
		}

		internal static object GetStubValue(DependencyObject obj)
		{
			return obj.GetValue(StubValueProperty);
		}

		internal static void SetStubValue(DependencyObject obj, object value)
		{
			obj.SetValue(StubValueProperty, value);
		}

		public static object GetPathValue(object sourceObject, string path)
		{
			GeneralUtilities generalUtilities = new GeneralUtilities();
			BindingOperations.SetBinding(generalUtilities, StubValueProperty, new Binding(path)
			{
				Source = sourceObject
			});
			object stubValue = GetStubValue(generalUtilities);
			BindingOperations.ClearBinding(generalUtilities, StubValueProperty);
			return stubValue;
		}

		public static object GetBindingValue(object sourceObject, Binding binding)
		{
			Binding binding2 = new Binding();
			binding2.BindsDirectlyToSource = binding.BindsDirectlyToSource;
			binding2.Converter = binding.Converter;
			binding2.ConverterCulture = binding.ConverterCulture;
			binding2.ConverterParameter = binding.ConverterParameter;
			binding2.FallbackValue = binding.FallbackValue;
			binding2.Mode = BindingMode.OneTime;
			binding2.Path = binding.Path;
			binding2.StringFormat = binding.StringFormat;
			binding2.TargetNullValue = binding.TargetNullValue;
			binding2.XPath = binding.XPath;
			Binding binding3 = binding2;
			binding3.Source = sourceObject;
			GeneralUtilities generalUtilities = new GeneralUtilities();
			BindingOperations.SetBinding(generalUtilities, StubValueProperty, binding3);
			object stubValue = GetStubValue(generalUtilities);
			BindingOperations.ClearBinding(generalUtilities, StubValueProperty);
			return stubValue;
		}

		internal static bool CanConvertValue(object value, object targetType)
		{
			if (value != null && !object.Equals(value.GetType(), targetType))
			{
				return !object.Equals(targetType, typeof(object));
			}
			return false;
		}
	}
}
