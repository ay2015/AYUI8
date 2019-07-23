using System;

namespace ay.Animate
{
    /// <summary>
    /// AY动画树管理
    /// 设计：每组动画的播放应该有单独的管理，它只管理调度和看进度
    /// </summary>
    public class AyAnimateTreePad : IAyAnimateTreePad
    {
        /// <summary>
        /// 动画树，每个的分支
        /// </summary>
        public AyAnimateTreeBase RootAnimateTree { get; set; }
        /// <summary>
        /// 当前正在链接的树分支
        /// </summary>
        internal AyAnimateTreeBase CurrentLinkAnimateTreeItem { get; set; }
        /// <summary>
        /// 当前正在执行的动画分支
        /// </summary>
        public AyAnimateTreeBase CurrentAyAnimate;

        /// <summary>
        /// 总时长=动画时长+延迟时长
        /// 同时播放的时长，按照最长的那个动画计算
        /// </summary>
        public double AllPlayTime { get; set; }
        /// <summary>
        /// 添加一个动画
        /// </summary>
        /// <param name="ayAnimate">一个动画</param>
        /// <returns></returns>
        public IAyAnimateTreePad Add(AyAnimateBase ayAnimate)
        {
            if (RootAnimateTree == null)
            {
                RootAnimateTree = new AyAnimateTreeItem { AyAnimateBases = new AyAnimateBase[] { ayAnimate }, LastAyAnimateBase = ayAnimate, IsFirst = true, IsEnd = true, Time = ayAnimate.AnimateSpeed };
                AllPlayTime += RootAnimateTree.Time;
            }
            else
            {
                var _AnimateTree = new AyAnimateTreeItem { AyAnimateBases = new AyAnimateBase[] { ayAnimate }, LastAyAnimateBase = ayAnimate, Time = ayAnimate.AnimateSpeed };
                AllPlayTime += _AnimateTree.Time;
                if (RootAnimateTree.NextAnimateTreeItem == null)
                {
                    RootAnimateTree.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = RootAnimateTree;
                    RootAnimateTree.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
                else
                {
                    CurrentLinkAnimateTreeItem.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = CurrentLinkAnimateTreeItem;
                    CurrentLinkAnimateTreeItem.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();

                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }

            }
            return this;
        }
        /// <summary>
        /// 延迟多少毫秒后执行 一个动画
        /// </summary>
        /// <param name="millSeconds">毫秒</param>
        /// <param name="ayAnimate">Ay动画</param>
        /// <returns></returns>
        public IAyAnimateTreePad DelayAdd(int millSeconds, AyAnimateBase ayAnimate)
        {
            if (RootAnimateTree == null)
            {
                RootAnimateTree = new AyAnimateDelayTreeItem { DelayMillseconds = millSeconds, AyAnimateBases = new AyAnimateBase[] { ayAnimate }, LastAyAnimateBase = ayAnimate, IsFirst = true, IsEnd = true, Time = ayAnimate.AnimateSpeed };
                AllPlayTime += RootAnimateTree.Time;
            }
            else
            {
                var _AnimateTree = new AyAnimateDelayTreeItem { DelayMillseconds = millSeconds, AyAnimateBases = new AyAnimateBase[] { ayAnimate }, LastAyAnimateBase = ayAnimate, Time = ayAnimate.AnimateSpeed };
                AllPlayTime += _AnimateTree.Time;
                AllPlayTime += _AnimateTree.DelayMillseconds;
                if (RootAnimateTree.NextAnimateTreeItem == null)
                {
                    RootAnimateTree.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = RootAnimateTree;
                    RootAnimateTree.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
                else
                {
                    CurrentLinkAnimateTreeItem.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = CurrentLinkAnimateTreeItem;
                    CurrentLinkAnimateTreeItem.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
            }
            return this;
        }

        /// <summary>
        /// 添加一组同时执行的Ay动画
        /// </summary>
        /// <param name="ayAnimate">Ay动画</param>
        /// <returns></returns>
        public IAyAnimateTreePad AddSameBegin(params AyAnimateBase[] ayAnimate)
        {
            ///寻找时长最长的动画
            double time1 = 0;
            AyAnimateBase maxTimeAyAnimate = null;
            foreach (var item in ayAnimate)
            {
                if (item.AnimateSpeed > time1)
                {
                    maxTimeAyAnimate = item;
                    time1 = item.AnimateSpeed;
                }
            }

            if (RootAnimateTree == null)
            {
                RootAnimateTree = new AyAnimateTreeItem { AyAnimateBases = ayAnimate, LastAyAnimateBase = maxTimeAyAnimate, IsFirst = true, IsEnd = true, Time = time1 };
                AllPlayTime += RootAnimateTree.Time;
            }
            else
            {
                var _AnimateTree = new AyAnimateTreeItem { AyAnimateBases = ayAnimate, LastAyAnimateBase = maxTimeAyAnimate, Time = time1 };
                AllPlayTime += _AnimateTree.Time;
                if (RootAnimateTree.NextAnimateTreeItem == null)
                {
                    RootAnimateTree.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = RootAnimateTree;
                    RootAnimateTree.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
                else
                {
                    CurrentLinkAnimateTreeItem.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = CurrentLinkAnimateTreeItem;
                    CurrentLinkAnimateTreeItem.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }

            }
            return this;

        }

        /// <summary>
        /// 延迟多少毫秒后执行一组同时执行的Ay动画
        /// </summary>
        /// <param name="millSeconds">毫秒</param>
        /// <param name="ayAnimate">一组Ay动画</param>
        /// <returns></returns>
        public IAyAnimateTreePad DelayAddSameBegin(int millSeconds, params AyAnimateBase[] ayAnimate)
        {
            ///寻找时长最长的动画
            double time1 = 0;
            AyAnimateBase maxTimeAyAnimate = null;
            foreach (var item in ayAnimate)
            {
                if (item.AnimateSpeed > time1)
                {
                    maxTimeAyAnimate = item;
                    time1 = item.AnimateSpeed;
                }
            }
            if (RootAnimateTree == null)
            {
                RootAnimateTree = new AyAnimateDelayTreeItem { DelayMillseconds = millSeconds, AyAnimateBases = ayAnimate, LastAyAnimateBase = maxTimeAyAnimate, IsFirst = true, IsEnd = true, Time = time1 };
                AllPlayTime += RootAnimateTree.Time;
            }
            else
            {
                var _AnimateTree = new AyAnimateDelayTreeItem { DelayMillseconds = millSeconds, AyAnimateBases = ayAnimate, LastAyAnimateBase = maxTimeAyAnimate, Time = time1 };
                AllPlayTime += _AnimateTree.Time;
                AllPlayTime += _AnimateTree.DelayMillseconds;
                if (RootAnimateTree.NextAnimateTreeItem == null)
                {
                    RootAnimateTree.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = RootAnimateTree;
                    RootAnimateTree.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
                else
                {
                    CurrentLinkAnimateTreeItem.NextAnimateTreeItem = _AnimateTree;
                    _AnimateTree.LastAnimateTreeItem = CurrentLinkAnimateTreeItem;
                    CurrentLinkAnimateTreeItem.LastAyAnimateBase.NextAyAnimateTreeItem = () =>
                    {
                        _AnimateTree.Begin();
                        CurrentAyAnimate = _AnimateTree;
                    };
                    CurrentLinkAnimateTreeItem = _AnimateTree;
                }
            }
            return this;
        }


        /// <summary>
        /// 销毁所有动画
        /// </summary>
        public void Destroy()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Destroy();
                }
            }
            if (RootAnimateTree is IAyAnimateLifecycle animate1)
            {
                animate1.Destroy();
            }
            DestroyAll(RootAnimateTree);
        }
        /// <summary>
        /// 初始化所有动画
        /// </summary>
        public void Initialize()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Initialize();
                }
            }
            if (RootAnimateTree is IAyAnimateLifecycle animate1)
            {
                animate1.Initialize();
            }
            InitializeAll(RootAnimateTree);
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        public void Begin()
        {
            if (RootAnimateTree == null) return;
            if (RootAnimateTree != null)
            {
                if (RootAnimateTree is IAyAnimateLifecycle animate)
                {
                    animate.Begin();
                    CurrentAyAnimate = RootAnimateTree;
                }
            }
        }
        /// <summary>
        /// 暂停当前正在进行的动画
        /// </summary>
        public void Pauze()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Pauze();
                }
            }
        }

        /// <summary>
        /// 继续当前正在进行的动画
        /// </summary>
        public void Resume()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Resume();
                }
            }
        }
        /// <summary>
        /// 当前正在进行的动画 跳到结束
        /// </summary>
        public void SkipToFill()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.SkipToFill();
                }
            }
            if (RootAnimateTree is IAyAnimateLifecycle animate1)
            {
                animate1.SkipToFill();
            }

            SkipToFillAll(RootAnimateTree);
        }
        private void SkipToFillAll(AyAnimateTreeBase animate)
        {
            if (animate.NextAnimateTreeItem != null)
            {
                if (animate.NextAnimateTreeItem is IAyAnimateLifecycle ani)
                {
                    ani.SkipToFill();
                }
                SkipToFillAll(animate.NextAnimateTreeItem);
            }
        }
        /// <summary>
        /// 停止当前正在进行的动画
        /// </summary>
        public void Stop()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Stop();
                }
            }
            if (RootAnimateTree is IAyAnimateLifecycle animate1)
            {
                animate1.Stop();
            }

            StopAll(RootAnimateTree);
        }
        private void InitializeAll(AyAnimateTreeBase animate)
        {
            if (animate.NextAnimateTreeItem != null)
            {
                if (animate.NextAnimateTreeItem is IAyAnimateLifecycle ani)
                {
                    ani.Initialize();
                }
                InitializeAll(animate.NextAnimateTreeItem);
            }
        }
        private void DestroyAll(AyAnimateTreeBase animate)
        {
            if (animate.NextAnimateTreeItem != null)
            {
                if (animate.NextAnimateTreeItem is IAyAnimateLifecycle ani)
                {
                    ani.Destroy();
                }
                DestroyAll(animate.NextAnimateTreeItem);
            }
        }
        private void StopAll(AyAnimateTreeBase animate)
        {
            if (animate.NextAnimateTreeItem != null)
            {
                if (animate.NextAnimateTreeItem is IAyAnimateLifecycle ani)
                {
                    ani.Stop();
                }
                StopAll(animate.NextAnimateTreeItem);
            }
        }

        /// <summary>
        /// 销毁动画，然后清空树
        /// </summary>
        public void ClearPad()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Destroy();
                }
                RootAnimateTree = null;
                AyCommon.MemoryGC();
            }
        }
        /// <summary>
        /// 停止和销毁动画，然后清空树
        /// </summary>
        public void ClearPadAndStop()
        {
            if (RootAnimateTree == null) return;
            if (CurrentAyAnimate != null)
            {
                if (CurrentAyAnimate is IAyAnimateLifecycle animate)
                {
                    animate.Stop();
                    animate.Destroy();
                }
                RootAnimateTree = null;
                AyCommon.MemoryGC();
            }
        }

    }
}
