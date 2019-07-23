using System.Reflection;
using System.Windows.Input;

namespace System.Windows.Interactivity
{
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		private string commandName;

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);

		public string CommandName
		{
			get
			{
				ReadPreamble();
				return commandName;
			}
			set
			{
				if (CommandName != value)
				{
					WritePreamble();
					commandName = value;
					WritePostscript();
				}
			}
		}

		public ICommand Command
		{
			get
			{
				return (ICommand)GetValue(CommandProperty);
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}

		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				ICommand command = ResolveCommand();
				if (command != null && command.CanExecute(CommandParameter))
				{
					command.Execute(CommandParameter);
				}
			}
		}

		private ICommand ResolveCommand()
		{
			ICommand result = null;
			if (Command != null)
			{
				result = Command;
			}
			else if (base.AssociatedObject != null)
			{
				Type type = base.AssociatedObject.GetType();
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
					{
						result = (ICommand)propertyInfo.GetValue(base.AssociatedObject, null);
					}
				}
			}
			return result;
		}
	}
}
