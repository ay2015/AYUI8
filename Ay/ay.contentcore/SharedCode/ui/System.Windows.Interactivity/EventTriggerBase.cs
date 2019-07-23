using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Interactivity
{
	public abstract class EventTriggerBase : TriggerBase
	{
		private Type sourceTypeConstraint;

		private bool isSourceChangedRegistered;

		private NameResolver sourceNameResolver;

		private MethodInfo eventHandlerMethodInfo;

		public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register("SourceObject", typeof(object), typeof(EventTriggerBase), new PropertyMetadata(OnSourceObjectChanged));

		public static readonly DependencyProperty SourceNameProperty = DependencyProperty.Register("SourceName", typeof(string), typeof(EventTriggerBase), new PropertyMetadata(OnSourceNameChanged));

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

		protected Type SourceTypeConstraint
		{
			get
			{
				return sourceTypeConstraint;
			}
		}

		public object SourceObject
		{
			get
			{
				return GetValue(SourceObjectProperty);
			}
			set
			{
				SetValue(SourceObjectProperty, value);
			}
		}

		public string SourceName
		{
			get
			{
				return (string)GetValue(SourceNameProperty);
			}
			set
			{
				SetValue(SourceNameProperty, value);
			}
		}

		public object Source
		{
			get
			{
				object obj = base.AssociatedObject;
				if (SourceObject != null)
				{
					obj = SourceObject;
				}
				else if (IsSourceNameSet)
				{
					obj = SourceNameResolver.Object;
					if (obj != null && !SourceTypeConstraint.IsAssignableFrom(obj.GetType()))
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "RetargetedTypeConstraintViolatedExceptionMessage", GetType().Name, obj.GetType(), SourceTypeConstraint, "Source"));
					}
				}
				return obj;
			}
		}

		private NameResolver SourceNameResolver
		{
			get
			{
				return sourceNameResolver;
			}
		}

		private bool IsSourceChangedRegistered
		{
			get
			{
				return isSourceChangedRegistered;
			}
			set
			{
				isSourceChangedRegistered = value;
			}
		}

		private bool IsSourceNameSet
		{
			get
			{
				if (string.IsNullOrEmpty(SourceName))
				{
					return ReadLocalValue(SourceNameProperty) != DependencyProperty.UnsetValue;
				}
				return true;
			}
		}

		private bool IsLoadedRegistered
		{
			get;
			set;
		}

		internal EventTriggerBase(Type sourceTypeConstraint)
			: base(typeof(DependencyObject))
		{
			this.sourceTypeConstraint = sourceTypeConstraint;
			sourceNameResolver = new NameResolver();
			RegisterSourceChanged();
		}

		protected abstract string GetEventName();

		protected virtual void OnEvent(EventArgs eventArgs)
		{
			InvokeActions(eventArgs);
		}

		private void OnSourceChanged(object oldSource, object newSource)
		{
			if (base.AssociatedObject != null)
			{
				OnSourceChangedImpl(oldSource, newSource);
			}
		}

		internal virtual void OnSourceChangedImpl(object oldSource, object newSource)
		{
			if (!string.IsNullOrEmpty(GetEventName()) && string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) != 0)
			{
				if (oldSource != null && SourceTypeConstraint.IsAssignableFrom(oldSource.GetType()))
				{
					UnregisterEvent(oldSource, GetEventName());
				}
				if (newSource != null && SourceTypeConstraint.IsAssignableFrom(newSource.GetType()))
				{
					RegisterEvent(newSource, GetEventName());
				}
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			DependencyObject associatedObject = base.AssociatedObject;
			Behavior behavior = associatedObject as Behavior;
			FrameworkElement frameworkElement = associatedObject as FrameworkElement;
			RegisterSourceChanged();
			if (behavior != null)
			{
				associatedObject = ((IAttachedObject)behavior).AssociatedObject;
				behavior.AssociatedObjectChanged += OnBehaviorHostChanged;
			}
			else if (SourceObject != null || frameworkElement == null)
			{
				try
				{
					OnSourceChanged(null, Source);
				}
				catch (InvalidOperationException)
				{
				}
			}
			else
			{
				SourceNameResolver.NameScopeReferenceElement = frameworkElement;
			}
			if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && frameworkElement != null && !Interaction.IsElementLoaded(frameworkElement))
			{
				RegisterLoaded(frameworkElement);
			}
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Behavior behavior = base.AssociatedObject as Behavior;
			FrameworkElement frameworkElement = base.AssociatedObject as FrameworkElement;
			try
			{
				OnSourceChanged(Source, null);
			}
			catch (InvalidOperationException)
			{
			}
			UnregisterSourceChanged();
			if (behavior != null)
			{
				behavior.AssociatedObjectChanged -= OnBehaviorHostChanged;
			}
			SourceNameResolver.NameScopeReferenceElement = null;
			if (string.Compare(GetEventName(), "Loaded", StringComparison.Ordinal) == 0 && frameworkElement != null)
			{
				UnregisterLoaded(frameworkElement);
			}
		}

		private void OnBehaviorHostChanged(object sender, EventArgs e)
		{
			SourceNameResolver.NameScopeReferenceElement = (((IAttachedObject)sender).AssociatedObject as FrameworkElement);
		}

		private static void OnSourceObjectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
			object @object = eventTriggerBase.SourceNameResolver.Object;
			if (args.NewValue == null)
			{
				eventTriggerBase.OnSourceChanged(args.OldValue, @object);
			}
			else
			{
				if (args.OldValue == null && @object != null)
				{
					eventTriggerBase.UnregisterEvent(@object, eventTriggerBase.GetEventName());
				}
				eventTriggerBase.OnSourceChanged(args.OldValue, args.NewValue);
			}
		}

		private static void OnSourceNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EventTriggerBase eventTriggerBase = (EventTriggerBase)obj;
			eventTriggerBase.SourceNameResolver.Name = (string)args.NewValue;
		}

		private void RegisterSourceChanged()
		{
			if (!IsSourceChangedRegistered)
			{
				SourceNameResolver.ResolvedElementChanged += OnSourceNameResolverElementChanged;
				IsSourceChangedRegistered = true;
			}
		}

		private void UnregisterSourceChanged()
		{
			if (IsSourceChangedRegistered)
			{
				SourceNameResolver.ResolvedElementChanged -= OnSourceNameResolverElementChanged;
				IsSourceChangedRegistered = false;
			}
		}

		private void OnSourceNameResolverElementChanged(object sender, NameResolvedEventArgs e)
		{
			if (SourceObject == null)
			{
				OnSourceChanged(e.OldObject, e.NewObject);
			}
		}

		private void RegisterLoaded(FrameworkElement associatedElement)
		{
			if (!IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded += OnEventImpl;
				IsLoadedRegistered = true;
			}
		}

		private void UnregisterLoaded(FrameworkElement associatedElement)
		{
			if (IsLoadedRegistered && associatedElement != null)
			{
				associatedElement.Loaded -= OnEventImpl;
				IsLoadedRegistered = false;
			}
		}

		private void RegisterEvent(object obj, string eventName)
		{
			Type type = obj.GetType();
			EventInfo @event = type.GetEvent(eventName);
			if (@event == null)
			{
				if (SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "EventTriggerCannotFindEventNameExceptionMessage", new object[2]
					{
						eventName,
						obj.GetType().Name
					}));
				}
			}
			else if (!IsValidEvent(@event))
			{
				if (SourceObject != null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "EventTriggerBaseInvalidEventExceptionMessage", new object[2]
					{
						eventName,
						obj.GetType().Name
					}));
				}
			}
			else
			{
				eventHandlerMethodInfo = typeof(EventTriggerBase).GetMethod("OnEventImpl", BindingFlags.Instance | BindingFlags.NonPublic);
				@event.AddEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
			}
		}

		private static bool IsValidEvent(EventInfo eventInfo)
		{
			Type eventHandlerType = eventInfo.EventHandlerType;
			if (typeof(Delegate).IsAssignableFrom(eventInfo.EventHandlerType))
			{
				MethodInfo method = eventHandlerType.GetMethod("Invoke");
				ParameterInfo[] parameters = method.GetParameters();
				if (parameters.Length == 2 && typeof(object).IsAssignableFrom(parameters[0].ParameterType))
				{
					return typeof(EventArgs).IsAssignableFrom(parameters[1].ParameterType);
				}
				return false;
			}
			return false;
		}

		private void UnregisterEvent(object obj, string eventName)
		{
			if (string.Compare(eventName, "Loaded", StringComparison.Ordinal) == 0)
			{
				FrameworkElement frameworkElement = obj as FrameworkElement;
				if (frameworkElement != null)
				{
					UnregisterLoaded(frameworkElement);
				}
			}
			else
			{
				UnregisterEventImpl(obj, eventName);
			}
		}

		private void UnregisterEventImpl(object obj, string eventName)
		{
			Type type = obj.GetType();
			if (!(eventHandlerMethodInfo == null))
			{
				EventInfo @event = type.GetEvent(eventName);
				@event.RemoveEventHandler(obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
				eventHandlerMethodInfo = null;
			}
		}

		private void OnEventImpl(object sender, EventArgs eventArgs)
		{
			OnEvent(eventArgs);
		}

		internal void OnEventNameChanged(string oldEventName, string newEventName)
		{
			if (base.AssociatedObject != null)
			{
				FrameworkElement frameworkElement = Source as FrameworkElement;
				if (frameworkElement != null && string.Compare(oldEventName, "Loaded", StringComparison.Ordinal) == 0)
				{
					UnregisterLoaded(frameworkElement);
				}
				else if (!string.IsNullOrEmpty(oldEventName))
				{
					UnregisterEvent(Source, oldEventName);
				}
				if (frameworkElement != null && string.Compare(newEventName, "Loaded", StringComparison.Ordinal) == 0)
				{
					RegisterLoaded(frameworkElement);
				}
				else if (!string.IsNullOrEmpty(newEventName))
				{
					RegisterEvent(Source, newEventName);
				}
			}
		}
	}
	public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
	{
		public new T Source
		{
			get
			{
				return (T)base.Source;
			}
		}

		protected EventTriggerBase()
			: base(typeof(T))
		{
		}

		internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
		{
			base.OnSourceChangedImpl(oldSource, newSource);
			OnSourceChanged(oldSource as T, newSource as T);
		}

		protected virtual void OnSourceChanged(T oldSource, T newSource)
		{
		}
	}
}
