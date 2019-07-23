using System;
public interface AyTimeInterface
{
    /// <summary>
    /// 开始
    /// </summary>
    void Start();
    /// <summary>
    /// 结束
    /// </summary>
    void End();
    /// <summary>
    /// 释放
    /// </summary>
    void Dispose();
}
public static class AyTimeExtension
{
    public static void StartTimer(this System.Timers.Timer timer)
    {
        timer.Enabled = true;
        timer.Start();
    }
    public static void StartTimer(this System.Windows.Threading.DispatcherTimer timer)
    {
        timer.IsEnabled = true;
        timer.Start();
    }
    public static void EndTimer(this System.Timers.Timer timer)
    {
        timer.Enabled = false;
        timer.Stop();
    }
    public static void EndTimer(this System.Windows.Threading.DispatcherTimer timer)
    {
        timer.IsEnabled = true;
        timer.Stop();
    }
    public static void DisposeTimer(this System.Timers.Timer timer)
    {
        timer.Enabled = false;
        timer.Stop();
        timer.Dispose();
    }
    public static void DisposeTimer(this System.Windows.Threading.DispatcherTimer timer)
    {
        timer.IsEnabled = true;
        timer.Stop();
        timer = null;
    }
}
public class AyBackgroundTime : IDisposable, AyTimeInterface
{
    /// <summary>
    /// 秒
    /// </summary>
    public int MillSecond { get; set; }
    /// <summary>
    /// 任务，因为需要手动停止，所以需要传入timer的对象，让用户控制
    /// </summary>
    public Action<System.Timers.Timer> TaskAction { get; set; }
    /// <summary>
    /// 只执行一次或重复执行
    /// </summary>
    public bool AutoReset { get; set; }

    public AyBackgroundTime(Action<System.Timers.Timer> action)
    {
        this.TaskAction = action;
        this.MillSecond = 10;
        dTimer.Elapsed += DTimer_Elapsed;
        dTimer.Interval = MillSecond;
    }
    public AyBackgroundTime(int millsecond, Action<System.Timers.Timer> action)
    {
        this.TaskAction = action;
        this.MillSecond = millsecond;
        dTimer.Elapsed += DTimer_Elapsed;
        dTimer.Interval = MillSecond;
    }
    private void DTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        //因为很频繁执行，所以不判断null了
        TaskAction(dTimer);
    }


    public System.Timers.Timer dTimer = new System.Timers.Timer();


    public void Start()
    {
        if (dTimer != null)
        {
            dTimer.StartTimer();
        }
    }
    public void End()
    {
        if (dTimer != null)
        {
            dTimer.EndTimer();
        }
    }
    public void Dispose()
    {
        if (dTimer != null)
        {
            dTimer.DisposeTimer();
        }
    }
}
/// <summary>
/// 延迟自动解锁，设置IsLocked=true,启动锁，然后N毫秒后自动解锁
/// </summary>
public class AyTimeDelayAutoUnLock : IDisposable, AyTimeInterface
{
    /// <summary>
    /// 延迟锁
    /// </summary>
    private bool _IsLocked;

    public bool IsLocked
    {
        get
        {
            return _IsLocked;
        }
        set
        {
            if (_IsLocked == value) { return; }
            _IsLocked = value;
            Start();
        }
    }

    /// <summary>
    /// 秒
    /// </summary>
    public int MillSecond { get; set; }

    public AyTimeDelayAutoUnLock()
    {
        this.MillSecond = 100;
        dTimer.Tick += DTimer_Tick;
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, MillSecond);
    }
    public AyTimeDelayAutoUnLock(int millsecond)
    {
        this.MillSecond = millsecond;
        dTimer.Tick += DTimer_Tick;
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, MillSecond);
    }
    public System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();

    private void DTimer_Tick(object sender, EventArgs e)
    {
        IsLocked = false;
        if (sender is System.Windows.Threading.DispatcherTimer ds)
        {
            End();
        }
    }
    public void Start()
    {
        if (dTimer != null)
        {
            dTimer.StartTimer();
        }
    }
    public void End()
    {
        if (dTimer != null)
        {
            dTimer.EndTimer();
        }
    }
    public void Dispose()
    {
        if (dTimer != null)
        {
            dTimer.DisposeTimer();
        }
    }
}

/// <summary>
/// 频繁延迟执行，但不频繁创建的计时器
/// 2019-6-13 10:31:53
/// </summary>
public class AyTimeSetTimeout : IDisposable, AyTimeInterface
{
    /// <summary>
    /// 秒
    /// </summary>
    public int MillSecond { get; set; }
    /// <summary>
    /// 任务
    /// </summary>
    public Action TaskAction { get; set; }
    /// <summary>
    /// 是否已经执行了
    /// </summary>
    public bool HasExecuted { get; set; } = false;
    public AyTimeSetTimeout(Action action)
    {
        this.TaskAction = action;
        this.MillSecond = 1;
        dTimer.Tick += DTimer_Tick;
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, MillSecond);
    }
    public AyTimeSetTimeout(int millsecond, Action action)
    {
        this.TaskAction = action;
        this.MillSecond = millsecond;
        dTimer.Tick += DTimer_Tick;
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, MillSecond);
    }
    public System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();

    private void DTimer_Tick(object sender, EventArgs e)
    {
        if (TaskAction != null)
        {
            TaskAction();
            HasExecuted = true;
        }
        if (sender is System.Windows.Threading.DispatcherTimer ds)
        {
            ds.IsEnabled = false;
            ds.Stop();

        }
    }
    public void Start()
    {
        if (dTimer != null)
        {
            dTimer.StartTimer();
        }
    }
    public void End()
    {
        if (dTimer != null)
        {
            dTimer.EndTimer();
        }
    }
    public void Dispose()
    {
        if (dTimer != null)
        {
            dTimer.DisposeTimer();
        }

    }

}
public class AyTime
{
    /// <summary>
    ///  2015年12月10日14:18:09 
    /// ay编写，用于方便定时任务，这里假如3000，那么第3秒执行
    /// </summary>
    /// <param name="millsecond"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static System.Windows.Threading.DispatcherTimer setInterval(int millsecond, Action action)
    {
        System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
        dTimer.Tick += (sender, e) => { action(); };
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, millsecond);
        //启动 DispatcherTimer对象dTime。
        dTimer.Start();
        return dTimer;
    }
    /// <summary>
    /// 不建议使用，建议使用AyTimeSetTimeout
    /// </summary>
    /// <param name="millsecond"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static System.Windows.Threading.DispatcherTimer setTimeout(int millsecond, Action action)
    {
        System.Windows.Threading.DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
        dTimer.Tick += (sender, e) =>
        {
            action();
            System.Windows.Threading.DispatcherTimer ds = sender as System.Windows.Threading.DispatcherTimer;
            if (ds != null)
            {
                ds.DisposeTimer();
            }
        };
        dTimer.Interval = new TimeSpan(0, 0, 0, 0, millsecond);
        dTimer.StartTimer();
        return dTimer;
    }



    /// <summary>
    /// 作用：用于创建指定执行次数的时间计时器，对AyTime的拓展，用于当前执行次数不能static，防止共享导致错误，特地创建一个AyTimeEx的类，用于每个重复执行任务，都有1个当前的执行次数
    /// 时间：2016-6-20 00:09:57
    /// 作者：AY
    /// </summary>
    public class AyTimeEx : AyTimeInterface
    {
        private int executeCnt = 1;
        private Action<System.Windows.Threading.DispatcherTimer> executeAction;
        private Action EndAction;
        private int millsecond;
        public System.Windows.Threading.DispatcherTimer currentDispatcherTimer;

        [ThreadStatic]
        int currentExecuteCnt = 0;

        public AyTimeEx(int _millsecond, Action<System.Windows.Threading.DispatcherTimer> _executeAction, int _executeCnt)
        {
            this.millsecond = _millsecond;
            this.executeCnt = _executeCnt;
            this.executeAction = _executeAction;
        }
        public AyTimeEx(int _millsecond, Action<System.Windows.Threading.DispatcherTimer> _executeAction, int _executeCnt, Action endAction)
        {
            this.millsecond = _millsecond;
            this.executeCnt = _executeCnt;
            this.executeAction = _executeAction;
            this.EndAction = endAction;
        }
        public void Start()
        {
            currentDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            currentDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, millsecond);
            //注：此处 Tick 为 dTimer 对象的事件（ 超过计时器间隔时发生）
            currentDispatcherTimer.Tick += (sender, e) =>
            {
                if (currentExecuteCnt >= executeCnt)
                {
                    System.Windows.Threading.DispatcherTimer ds = sender as System.Windows.Threading.DispatcherTimer;
                    if (ds != null)
                    {
                        ds.DisposeTimer();
                        currentExecuteCnt = 0;
                        if (EndAction != null) EndAction();
                    }
                }
                else
                {
                    executeAction(currentDispatcherTimer);
                    currentExecuteCnt++;
                }

            };
            currentDispatcherTimer.StartTimer();
        }

        public void Dispose()
        {
            if (currentDispatcherTimer != null)
            {
                currentDispatcherTimer.DisposeTimer();
            }
        }

        public void End()
        {
            if (currentDispatcherTimer != null)
            {
                currentDispatcherTimer.DisposeTimer();
                currentExecuteCnt = 0;
                if (EndAction != null) EndAction();
            }
        }
    }
}

