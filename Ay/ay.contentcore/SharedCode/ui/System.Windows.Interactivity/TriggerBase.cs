using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	[ContentProperty("Actions")]
	public abstract class TriggerBase : Animatable, IAttachedObject
	{
		private DependencyObject associatedObject;

		private Type associatedObjectTypeConstraint;

		private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly("Actions", typeof(TriggerActionCollection), typeof(TriggerBase), new FrameworkPropertyMetadata());

		public static readonly DependencyProperty ActionsProperty = ActionsPropertyKey.DependencyProperty;

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

		public TriggerActionCollection Actions
		{
			get
			{
				return (TriggerActionCollection)GetValue(ActionsProperty);
			}
		}

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}

		public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

		internal TriggerBase(Type associatedObjectTypeConstraint)
		{
			this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
			TriggerActionCollection value = new TriggerActionCollection();
			SetValue(ActionsPropertyKey, value);
		}

		protected void InvokeActions(object parameter)
		{
			if (this.PreviewInvoke != null)
			{
				PreviewInvokeEventArgs previewInvokeEventArgs = new PreviewInvokeEventArgs();
				this.PreviewInvoke(this, previewInvokeEventArgs);
				if (previewInvokeEventArgs.Cancelling)
				{
					return;
				}
			}
			foreach (TriggerAction action in Actions)
			{
				action.CallInvoke(parameter);
			}
		}

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
					throw new InvalidOperationException("CannotHostTriggerMultipleTimesExceptionMessage");
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
				Actions.Attach(dependencyObject);
				OnAttached();
			}
		}

		public void Detach()
		{
			OnDetaching();
			WritePreamble();
			associatedObject = null;
			WritePostscript();
			Actions.Detach();
		}
	}
	public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject
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

		protected TriggerBase()
			: base(typeof(T))
		{
		}
	}
}
