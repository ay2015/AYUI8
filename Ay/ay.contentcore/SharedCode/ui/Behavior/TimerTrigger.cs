using System;
using System.Windows;
using System.Windows.Threading;

namespace ay.contentcore
{
    public sealed class TimerTrigger : System.Windows.Interactivity.EventTrigger
    {
        public static readonly DependencyProperty MillisecondsPerTickProperty = DependencyProperty.Register("MillisecondsPerTick", typeof(double), typeof(TimerTrigger), new FrameworkPropertyMetadata(1000.0));

        public static readonly DependencyProperty TotalTicksProperty = DependencyProperty.Register("TotalTicks", typeof(int), typeof(TimerTrigger), new FrameworkPropertyMetadata(-1));

        private DispatcherTimer timer;

        private EventArgs eventArgs;

        private int tickCount;

        public double MillisecondsPerTick
        {
            get
            {
                return (double)GetValue(MillisecondsPerTickProperty);
            }
            set
            {
                SetValue(MillisecondsPerTickProperty, value);
            }
        }

        public int TotalTicks
        {
            get
            {
                return (int)GetValue(TotalTicksProperty);
            }
            set
            {
                SetValue(TotalTicksProperty, value);
            }
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            StopTimer();
            this.eventArgs = eventArgs;
            tickCount = 0;
            StartTimer();
        }

        protected override void OnDetaching()
        {
            StopTimer();
            base.OnDetaching();
        }

        internal void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(MillisecondsPerTick);
            timer.Tick += OnTimerTick;
            timer.Start();
        }

        internal void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (TotalTicks > 0 && ++tickCount >= TotalTicks)
            {
                StopTimer();
            }
            InvokeActions(eventArgs);
        }
    }
}
