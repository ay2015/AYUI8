using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Interactivity
{
	public abstract class TargetedTriggerAction : TriggerAction
	{
		private Type targetTypeConstraint;

		private bool isTargetChangedRegistered;

		private NameResolver targetResolver;

		public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(OnTargetObjectChanged));

		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(TargetedTriggerAction), new FrameworkPropertyMetadata(OnTargetNameChanged));

		public object TargetObject
		{
			get
			{
				return GetValue(TargetObjectProperty);
			}
			set
			{
				SetValue(TargetObjectProperty, value);
			}
		}

		public string TargetName
		{
			get
			{
				return (string)GetValue(TargetNameProperty);
			}
			set
			{
				SetValue(TargetNameProperty, value);
			}
		}

		protected object Target
		{
			get
			{
				object obj = base.AssociatedObject;
				if (TargetObject != null)
				{
					obj = TargetObject;
				}
				else if (IsTargetNameSet)
				{
					obj = TargetResolver.Object;
				}
				if (obj != null && !TargetTypeConstraint.IsAssignableFrom(obj.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "RetargetedTypeConstraintViolatedExceptionMessage", GetType().Name, obj.GetType(), TargetTypeConstraint, "Target"));
				}
				return obj;
			}
		}

		protected sealed override Type AssociatedObjectTypeConstraint
		{
			get
			{
				AttributeCollection attributes = TypeDescriptor.GetAttributes(GetType());
				TypeConstraintAttribute typeConstraintAttribute = attributes[typeof(TypeConstraintAttribute)] as TypeConstraintAttribute;
				if (typeConstraintAttribute != null)
				{
					return typeConstraintAttribute.Constraint;
				}
				return typeof(DependencyObject);
			}
		}

		protected Type TargetTypeConstraint
		{
			get
			{
				ReadPreamble();
				return targetTypeConstraint;
			}
		}

		private bool IsTargetNameSet
		{
			get
			{
				if (string.IsNullOrEmpty(TargetName))
				{
					return ReadLocalValue(TargetNameProperty) != DependencyProperty.UnsetValue;
				}
				return true;
			}
		}

		private NameResolver TargetResolver
		{
			get
			{
				return targetResolver;
			}
		}

		private bool IsTargetChangedRegistered
		{
			get
			{
				return isTargetChangedRegistered;
			}
			set
			{
				isTargetChangedRegistered = value;
			}
		}

		internal TargetedTriggerAction(Type targetTypeConstraint)
			: base(typeof(DependencyObject))
		{
			this.targetTypeConstraint = targetTypeConstraint;
			targetResolver = new NameResolver();
			RegisterTargetChanged();
		}

		internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			RegisterTargetChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += OnBehaviorHostChanged;
			}
			TargetResolver.NameScopeReferenceElement = (associatedObject as FrameworkElement);
		}

		protected override void OnDetaching()
		{
			Behavior behavior = base.AssociatedObject as Behavior;
			base.OnDetaching();
			OnTargetChangedImpl(TargetResolver.Object, null);
			UnregisterTargetChanged();
			if (behavior != null)
			{
				behavior.AssociatedObjectChanged -= OnBehaviorHostChanged;
			}
			TargetResolver.NameScopeReferenceElement = null;
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			TargetResolver.NameScopeReferenceElement = (((IAttachedObject)sender).AssociatedObject as FrameworkElement);
		}

		private void RegisterTargetChanged()
		{
			if (!IsTargetChangedRegistered)
			{
				TargetResolver.ResolvedElementChanged += OnTargetChanged;
				IsTargetChangedRegistered = true;
			}
		}

		private void UnregisterTargetChanged()
		{
			if (IsTargetChangedRegistered)
			{
				TargetResolver.ResolvedElementChanged -= OnTargetChanged;
				IsTargetChangedRegistered = false;
			}
		}

		private static void OnTargetObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
			targetedTriggerAction.OnTargetChanged(obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
		}

		private static void OnTargetNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			TargetedTriggerAction targetedTriggerAction = (TargetedTriggerAction)obj;
			targetedTriggerAction.TargetResolver.Name = (string)args.NewValue;
		}

		private void OnTargetChanged(object sender, NameResolvedEventArgs e)
		{
			if (base.AssociatedObject != null)
			{
				OnTargetChangedImpl(e.OldObject, e.NewObject);
			}
		}
	}
	public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
	{
		protected new T Target
		{
			get
			{
				return (T)base.Target;
			}
		}

		protected TargetedTriggerAction()
			: base(typeof(T))
		{
		}

		internal sealed override void OnTargetChangedImpl(object oldTarget, object newTarget)
		{
			base.OnTargetChangedImpl(oldTarget, newTarget);
			OnTargetChanged(oldTarget as T, newTarget as T);
		}

		protected virtual void OnTargetChanged(T oldTarget, T newTarget)
		{
		}
	}
}
