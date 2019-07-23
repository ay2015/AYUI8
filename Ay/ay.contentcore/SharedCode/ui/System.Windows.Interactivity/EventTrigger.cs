namespace System.Windows.Interactivity
{
	public class EventTrigger : EventTriggerBase<object>
	{
		public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventTrigger), new FrameworkPropertyMetadata("Loaded", OnEventNameChanged));

		public string EventName
		{
			get
			{
				return (string)GetValue(EventNameProperty);
			}
			set
			{
				SetValue(EventNameProperty, value);
			}
		}

		public EventTrigger()
		{
		}

		public EventTrigger(string eventName)
		{
			EventName = eventName;
		}

		protected override string GetEventName()
		{
			return EventName;
		}

		private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
		}
	}
}
