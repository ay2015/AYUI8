namespace ay.Animate
{
    /// <summary>
    /// 动画组，方便管理，Add非延迟动画
    /// </summary>
    public class AyAnimateTreeItem : AyAnimateTreeBase, IAyAnimateLifecycle
    {

        public void Begin()
        {
            foreach (var item in AyAnimateBases)
            {
                item.Begin();
            }
        }

        public void Destroy()
        {
            foreach (var item in AyAnimateBases)
            {
                item.Stop();
                item.Destroy();
            }
        }

        public void Initialize()
        {
            foreach (var item in AyAnimateBases)
            {
                item.ReInitialize();
            }
        }

        public void Pauze()
        {
            foreach (var item in AyAnimateBases)
            {
                item.Pauze();
            }
        }

        public void Resume()
        {
            foreach (var item in AyAnimateBases)
            {
                item.Resume();
            }
        }

        public void SkipToFill()
        {
            foreach (var item in AyAnimateBases)
            {
                item.SkipToFill();
            }
        }

        public void Stop()
        {
            foreach (var item in AyAnimateBases)
            {
                item.Stop();
            }
        }
    }
}
