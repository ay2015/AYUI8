namespace System.Windows.Interactivity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class TypeConstraintAttribute : Attribute
	{
		public Type Constraint
		{
			get;
			private set;
		}

		public TypeConstraintAttribute(Type constraint)
		{
			Constraint = constraint;
		}
	}
}
