using System;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Controls;


namespace ay.MARKUP.ResponsiveSupport
{
    public class DataSetter : Setter, IDataSetter
    {
        /// <summary>
        /// 指定名字
        /// </summary>
        public new string TargetName { get; set; }
        /// <summary>
        /// 资源范围
        /// </summary>
        public DataSetterScope Scope { get; set; } = DataSetterScope.Current;


    }
    /// <summary>
    /// 数据的 name查找方式
    /// </summary>
    public enum DataSetterScope
    {
        Current,
        ParentWindow,
        ParentPage,
    }
    /// <summary>
    /// 资源字典 的 key查找方式
    /// </summary>
    public enum ResourceSetterScope
    {
        FindInWindowResource,
        FindInPageResource,
        FindInUserControlResource,
        FindInApplicationResource
    }

}