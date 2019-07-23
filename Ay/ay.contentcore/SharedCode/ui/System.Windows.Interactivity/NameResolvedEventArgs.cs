namespace System.Windows.Interactivity
{
	internal sealed class NameResolvedEventArgs : EventArgs
	{
		private object oldObject;

		private object newObject;

		public object OldObject
		{
			get
			{
				return oldObject;
			}
		}

		public object NewObject
		{
			get
			{
				return newObject;
			}
		}

		public NameResolvedEventArgs(object oldObject, object newObject)
		{
			this.oldObject = oldObject;
			this.newObject = newObject;
		}
	}
}
