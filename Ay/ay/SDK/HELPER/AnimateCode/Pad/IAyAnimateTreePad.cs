using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.Animate
{
    /// <summary>
    /// Ay动画树
    /// </summary>
    public interface IAyAnimateTreePad : IAyAnimateLifecycle
    {
        /// <summary>
        /// 清空
        /// </summary>
        void ClearPad();
        /// <summary>
        /// 增加一个动画执行
        /// </summary>
        /// <param name="ayAnimate"></param>
        /// <returns></returns>
        IAyAnimateTreePad Add(AyAnimateBase ayAnimate);

        /// <summary>
        /// 添加一组同时执行的Ay动画
        /// </summary>
        /// <param name="ayAnimate">Ay动画</param>
        /// <returns></returns>
        IAyAnimateTreePad AddSameBegin(params AyAnimateBase[] ayAnimate);
        /// <summary>
        /// 延迟多少毫秒后执行 一个动画
        /// </summary>
        /// <param name="millSeconds">毫秒</param>
        /// <param name="ayAnimate">Ay动画</param>
        /// <returns></returns>
        IAyAnimateTreePad DelayAdd(int millSeconds, AyAnimateBase ayAnimate);

        /// <summary>
        /// 延迟多少毫秒后执行一组同时执行的Ay动画
        /// </summary>
        /// <param name="millSeconds">毫秒</param>
        /// <param name="ayAnimate">一组Ay动画</param>
        /// <returns></returns>
        IAyAnimateTreePad DelayAddSameBegin(int millSeconds, params AyAnimateBase[] ayAnimate);
    }
}
