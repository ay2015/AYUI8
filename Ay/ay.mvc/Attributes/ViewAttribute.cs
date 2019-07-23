using System;
namespace Ay.MvcFramework
{
    /// <summary>
    /// 视图数据共享过滤器
    /// AY 2017-9-21 14:15:24
    /// 设计，目的： 页面共享读取数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class ViewAttribute : Attribute, IViewFilter
    {
        public int Order { get; set; }

        public virtual void OnRendered(Controller controller) { }

    }


}
