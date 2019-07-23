using System;

namespace ay.Animate
{
    /// <summary>
    /// 动画组，方便管理,延迟
    /// </summary>
    public class AyAnimateDelayTreeItem : AyAnimateTreeBase, IAyAnimateLifecycle
    {
        public AyAnimateDelayTreeItem()
        {
            DelayAction = () =>
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Begin();
                }
            };
        }
        public AyAnimateDelayTreeItem(int DelayMillseconds)
        {
            this.DelayMillseconds = DelayMillseconds;
            DelayAction = () =>
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Begin();
                }
            };
        }
        /// <summary>
        /// 延迟执行,null表示非延迟动画
        /// </summary>
        public int DelayMillseconds { get; set; }

        public Action DelayAction { get; set; }

        private AyTimeSetTimeout _Timer;
        /// <summary>
        /// 无注释
        /// </summary>
        public AyTimeSetTimeout Timer
        {
            get
            {
                if (_Timer == null)
                {
                    _Timer = new AyTimeSetTimeout(DelayMillseconds, DelayAction);
                }
                return _Timer;
            }
        }

        public void Begin()
        {
            Timer.Start();
        }

        public void Destroy()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Stop();
                    item.Destroy();
                }
            }
            Timer.End();
            Timer.Dispose();

        }

        public void Initialize()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.ReInitialize();
                }
            }
        }

        public void Pauze()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Pauze();
                }
            }

        }

        public void Resume()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Resume();
                }
            }
        }

        public void SkipToFill()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.SkipToFill();
                }
            }
        }

        public void Stop()
        {
            if (Timer.HasExecuted)
            {
                foreach (var item in AyAnimateBases)
                {
                    item.Stop();
                }
            }
        }
    }
}
