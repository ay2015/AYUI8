#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	[Obsolete("Use EditorTemplateDefinition instead of EditorDefinition.  (XAML Ex: <t:EditorTemplateDefinition TargetProperties=\"FirstName,LastName\" .../> OR <t:EditorTemplateDefinition TargetProperties=\"{x:Type l:MyType}\" .../> )")]
	public class EditorDefinition : EditorTemplateDefinition
	{
		private const string UsageEx = " (XAML Ex: <t:EditorTemplateDefinition TargetProperties=\"FirstName,LastName\" .../> OR <t:EditorTemplateDefinition TargetProperties=\"{x:Type l:MyType}\" .../> )";

		private PropertyDefinitionCollection _properties = new PropertyDefinitionCollection();

		public DataTemplate EditorTemplate
		{
			get;
			set;
		}

		public PropertyDefinitionCollection PropertiesDefinitions
		{
			get
			{
				return _properties;
			}
			set
			{
				_properties = value;
			}
		}

		public Type TargetType
		{
			get;
			set;
		}

		public EditorDefinition()
		{
			Trace.TraceWarning(string.Format("{0} is obsolete. Instead use {1}.", typeof(EditorDefinition), typeof(EditorTemplateDefinition)) + " (XAML Ex: <t:EditorTemplateDefinition TargetProperties=\"FirstName,LastName\" .../> OR <t:EditorTemplateDefinition TargetProperties=\"{x:Type l:MyType}\" .../> )");
		}

		internal override void Lock()
		{
			if (base.EditingTemplate != null)
			{
				throw new InvalidOperationException(string.Format("Use a EditorTemplateDefinition instead of EditorDefinition in order to use the '{0}' property.", "EditingTemplate"));
			}
			if (base.TargetProperties != null && base.TargetProperties.Count > 0)
			{
				throw new InvalidOperationException(string.Format("Use a EditorTemplateDefinition instead of EditorDefinition in order to use the '{0}' property.", "TargetProperties"));
			}
			List<object> list = new List<object>();
			if (PropertiesDefinitions != null)
			{
				foreach (PropertyDefinition propertiesDefinition in PropertiesDefinitions)
				{
					if (propertiesDefinition.TargetProperties != null)
					{
						list.AddRange(propertiesDefinition.TargetProperties.Cast<object>());
					}
				}
			}
			base.TargetProperties = list;
			base.EditingTemplate = EditorTemplate;
			base.Lock();
		}
	}
}
