using System.Globalization;
using System.Windows.Media.Animation;

namespace System.Windows.Interactivity
{
	public abstract class Behavior : Animatable, IAttachedObject
	{
		private Type associatedType;

		private DependencyObject associatedObject;

		protected Type AssociatedType
		{
			get
			{
				ReadPreamble();
				return associatedType;
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

		DependencyObject IAttachedObject.AssociatedObject
		{
			get
			{
				return AssociatedObject;
			}
		}

		internal event EventHandler AssociatedObjectChanged;

		internal Behavior(Type associatedType)
		{
			this.associatedType = associatedType;
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

		private void OnAssociatedObjectChanged()
		{
			if (this.AssociatedObjectChanged != null)
			{
				this.AssociatedObjectChanged(this, new EventArgs());
			}
		}

		public void Attach(DependencyObject dependencyObject)
		{
			if (dependencyObject != AssociatedObject)
			{
				if (AssociatedObject != null)
				{
					throw new InvalidOperationException("CannotHostBehaviorMultipleTimesExceptionMessage");
				}
				if (dependencyObject != null && !AssociatedType.IsAssignableFrom(dependencyObject.GetType()))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "TypeConstraintViolatedExceptionMessage", new object[3]
					{
						GetType().Name,
						dependencyObject.GetType().Name,
						AssociatedType.Name
					}));
				}
				WritePreamble();
				associatedObject = dependencyObject;
				WritePostscript();
				OnAssociatedObjectChanged();
				OnAttached();
			}
		}

		public void Detach()
		{
			OnDetaching();
			WritePreamble();
			associatedObject = null;
			WritePostscript();
			OnAssociatedObjectChanged();
		}
	}
	public abstract class Behavior<T> : Behavior where T : DependencyObject
	{
		protected new T AssociatedObject
		{
			get
			{
				return (T)base.AssociatedObject;
			}
		}

		protected Behavior()
			: base(typeof(T))
		{
		}
	}
}
