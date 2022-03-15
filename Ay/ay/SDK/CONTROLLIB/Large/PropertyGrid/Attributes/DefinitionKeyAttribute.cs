using System;

namespace Xceed.Wpf.Toolkit.PropertyGrid.Attributes
{
	/// <summary>The DefinitionKeyAttribute can be assigned to properties of your selected object to define which EditorDefinition to use for the decorated property. As an
	/// alternative to the Editor attribute, this allows you separate the UI-specific code from your business model code. It can also be used to specify a specific
	/// default editor when a property type does not resolve to a valid editor (e.g., Object).</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class DefinitionKeyAttribute : Attribute
	{
		public object Key
		{
			get;
			set;
		}

		public DefinitionKeyAttribute()
		{
			
		}

		public DefinitionKeyAttribute(object key)
		{
			Key = key;
		}
	}
}
