using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	[DefaultTrigger(typeof(UIElement), typeof(EventTrigger), "MouseLeftButtonDown")]
	[DefaultTrigger(typeof(ButtonBase), typeof(EventTrigger), "Click")]
	public abstract class TriggerAction : Animatable, IAttachedObject
	{
		private bool isHosted;

		private DependencyObject associatedObject;

		private Type associatedObjectTypeConstraint;

		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(TriggerAction), new FrameworkPropertyMetadata(true));

		public bool IsEnabled
		{
			get
			{
				return (bool)GetValue(IsEnabledProperty);
			}
			set
			{
				SetValue(IsEnabledProperty, value);
			}
		}

		protected DependencyObject AssociatedObject
		{
			get
			{
				ReadPreamble();
				return associatedObject;
			}
		}

		protected virtual Type AssociatedObjectTypeConstraint
		{
			get
			{
				ReadPreamble();
				return associatedObjectTypeConstraint;
			}
		}

		internal bool IsHosted
		{
			get
			{
				ReadPreamble();
				return isHosted;
			}
			set
			{
				WritePreamble();
				isHosted = value;
				WritePostscript();
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}

		internal TriggerAction(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
		}

		internal void CallInvoke(object parameter)
		{
			if (IsEnabled)
			{
				Invoke(parameter);
			}
		}

		protected abstract void Invoke(object parameter);

		protected virtual void OnAttached()
		{
		}

		protected virtual void OnDetaching()
		{
		}

		protected override Freezable CreateInstanceCore()
		{
			Type type = GetType();
			return (Freezable)Activator.CreateInstance(type);
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException("CannotHostTriggerActionMultipleTimesExceptionMessage");
				}
				if (dependencyObject != null && !AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "TypeConstraintViolatedExceptionMessage", new object[3]
					{
						GetType().Name,
						dependencyObject.GetType().Name,
						AssociatedObjectTypeConstraint.Name
					}));
				}
				WritePreamble();
				associatedObject = dependencyObject;
				WritePostscript();
				OnAttached();
			}
		}

		public void Detach()
		{
			OnDetaching();
			WritePreamble();
			associatedObject = null;
			WritePostscript();
		}
	}
	public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
	{
		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				return base.AssociatedObjectTypeConstraint;
			}
		}

		protected TriggerAction()
			: base(typeof(T))
		{
		}
	}
}
