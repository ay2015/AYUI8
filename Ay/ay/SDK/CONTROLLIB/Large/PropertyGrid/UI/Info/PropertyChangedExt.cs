using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Xceed.Wpf.Toolkit.Core.Utilities
{
	internal static class PropertyChangedExt
	{
		public static void Notify<TMember>(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, Expression<Func<TMember>> expression)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("The expression must target a property or field.", "expression");
			}
			string propertyName = GetPropertyName(memberExpression, sender.GetType());
			NotifyCore(sender, handler, propertyName);
		}

		public static void Notify(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, string propertyName)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			NotifyCore(sender, handler, propertyName);
		}

		private static void NotifyCore(INotifyPropertyChanged sender, PropertyChangedEventHandler handler, string propertyName)
		{
			if (handler != null)
			{
				handler(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		internal static bool PropertyChanged(string propertyName, PropertyChangedEventArgs e, bool targetPropertyOnly)
		{
			string propertyName2 = e.PropertyName;
			if (propertyName2 == propertyName)
			{
				return true;
			}
			if (!targetPropertyOnly)
			{
				return string.IsNullOrEmpty(propertyName2);
			}
			return false;
		}

		internal static bool PropertyChanged<TOwner, TMember>(Expression<Func<TMember>> expression, PropertyChangedEventArgs e, bool targetPropertyOnly)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("The expression must target a property or field.", "expression");
			}
			return PropertyChanged(memberExpression, typeof(TOwner), e, targetPropertyOnly);
		}

		internal static bool PropertyChanged<TOwner, TMember>(Expression<Func<TOwner, TMember>> expression, PropertyChangedEventArgs e, bool targetPropertyOnly)
		{
			MemberExpression memberExpression = expression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("The expression must target a property or field.", "expression");
			}
			return PropertyChanged(memberExpression, typeof(TOwner), e, targetPropertyOnly);
		}

		private static bool PropertyChanged(MemberExpression expression, Type ownerType, PropertyChangedEventArgs e, bool targetPropertyOnly)
		{
			string propertyName = GetPropertyName(expression, ownerType);
			return PropertyChanged(propertyName, e, targetPropertyOnly);
		}

		private static string GetPropertyName(MemberExpression expression, Type ownerType)
		{
			Type type = expression.Expression.Type;
			if (!type.IsAssignableFrom(ownerType))
			{
				throw new ArgumentException("The expression must target a property or field on the appropriate owner.", "expression");
			}
			return ReflectionHelper.GetPropertyOrFieldName(expression);
		}
	}
}
