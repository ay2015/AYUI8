namespace ay.Animate
{
    /// <summary>
    /// Ay动画树父类
    /// 2019-6-18 09:26:56
    /// </summary>
    public class AyAnimateTreeBase
    {
        /// <summary>
        /// 当前的在末端的动画,时长最长的
        /// </summary>
        public AyAnimateBase LastAyAnimateBase { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public AyAnimateBase[] AyAnimateBases { get; set; }
        /// <summary>
        /// 是否是第一个
        /// </summary>
        public bool IsFirst { get; set; }
        /// <summary>
        /// 是否是最后一个，如果addgroup，则时长最长的为End，不按照添加顺序。如果是Add，则为当前
        /// </summary>
        public bool IsEnd { get; set; }
        /// <summary>
        /// 上一个动画Item
        /// </summary>
        public AyAnimateTreeBase LastAnimateTreeItem { get; set; }
        /// <summary>
        /// 下一个动画Item
        /// </summary>
        public AyAnimateTreeBase NextAnimateTreeItem { get; set; }
        /// <summary>
        /// 耗时多久
        /// </summary>
        public double Time { get; set; }
    }
}
