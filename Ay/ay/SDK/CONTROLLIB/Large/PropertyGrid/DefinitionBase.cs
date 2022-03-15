using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace Xceed.Wpf.Toolkit.PropertyGrid
{
	public abstract class DefinitionBase : DependencyObject
	{
		private bool _isLocked;

		internal bool IsLocked
		{
			get
			{
				return _isLocked;
			}
		}

		internal void ThrowIfLocked<TMember>(Expression<Func<TMember>> propertyExpression)
		{
			if (DesignerProperties.GetIsInDesignMode(this) || !IsLocked)
			{
				return;
			}
			string propertyOrFieldName = ReflectionHelper.GetPropertyOrFieldName(propertyExpression);
			string message = string.Format("Cannot modify {0} once the definition has beed added to a collection.", propertyOrFieldName);
			throw new InvalidOperationException(message);
		}

		internal virtual void Lock()
		{
			if (!_isLocked)
			{
				_isLocked = true;
			}
		}
	}
}
