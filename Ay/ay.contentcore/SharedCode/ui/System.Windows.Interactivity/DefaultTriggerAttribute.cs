using System.Collections;
using System.Globalization;

namespace System.Windows.Interactivity
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
	public sealed class DefaultTriggerAttribute : Attribute
	{
		private Type targetType;

		private Type triggerType;

		private object[] parameters;

		public Type TargetType
		{
			get
			{
				return targetType;
			}
		}

		public Type TriggerType
		{
			get
			{
				return triggerType;
			}
		}

		public IEnumerable Parameters
		{
			get
			{
				return parameters;
			}
		}

		public DefaultTriggerAttribute(Type targetType, Type triggerType, object parameter)
			: this(targetType, triggerType, new object[1]
			{
				parameter
			})
		{
		}

		public DefaultTriggerAttribute(Type targetType, Type triggerType, params object[] parameters)
		{
			if (!typeof(TriggerBase).IsAssignableFrom(triggerType))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "DefaultTriggerAttributeInvalidTriggerTypeSpecifiedExceptionMessage", new object[1]
				{
					triggerType.Name
				}));
			}
			this.targetType = targetType;
			this.triggerType = triggerType;
			this.parameters = parameters;
		}

		public TriggerBase Instantiate()
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(TriggerType, parameters);
			}
			catch
			{
			}
			return (TriggerBase)obj;
		}
	}
}
