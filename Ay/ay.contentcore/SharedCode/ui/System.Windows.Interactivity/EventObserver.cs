using System.Reflection;

namespace System.Windows.Interactivity
{
	public sealed class EventObserver : IDisposable
	{
		private EventInfo eventInfo;

		private object target;

		private Delegate handler;

		public EventObserver(EventInfo eventInfo, object target, Delegate handler)
		{
			if (eventInfo == null)
			{
				throw new ArgumentNullException("eventInfo");
			}
			if ((object)handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.eventInfo = eventInfo;
			this.target = target;
			this.handler = handler;
			this.eventInfo.AddEventHandler(this.target, handler);
		}

		public void Dispose()
		{
			eventInfo.RemoveEventHandler(target, handler);
		}
	}
}
