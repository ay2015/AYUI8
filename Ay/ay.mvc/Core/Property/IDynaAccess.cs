
using System;

namespace Ay.MvcFramework
{

    /// <summary>
    /// 支持动态访问能力接口
    /// ***暂时没有使用***
    /// </summary>
    public interface IDynaAccessible
    {
        /// <summary>
        /// 得到可以动态访问的属性名称
        /// </summary>
        string[] GetPropertyNames();

        /// <summary>
        /// 设置动态属性值
        /// </summary>
        void SetProperty(string propertyName, object propertyValue);

        /// <summary>
        /// 得到动态属性值
        /// </summary>
        object GetProperty(string propertyName);

        /// <summary>
        /// 判断是否存在指定名称的属性
        /// </summary>
        bool HasProperty(string propertyName);

        /// <summary>
        /// 得到指定动态属性的类型
        /// </summary>
        Type GetPropertyType(string propertyName);
    }

    /// <summary>
    /// 动态访问的代理接口
    /// </summary>
    public interface IDynaAccessProxy
    {
        /// <summary>
        /// 得到指定对象的属性名称数组
        /// 永远不抛出异常
        /// </summary>
        string[] GetPropertyNames(object target);

        /// <summary>
        /// 设置指定对象实例的动态属性值
        /// </summary>
        /// <param name="target">对象实例</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        /// <exception cref="Framework.Commons.Property.DynaAccessException">产生任何错误时</exception>
        void SetProperty(object target, string propertyName, object propertyValue);

        /// <summary>
        /// 获取指定对象实例的动态属性值
        /// </summary>
        /// <param name="target">对象实例</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        /// <exception cref="Framework.Commons.Property.DynaAccessException">产生任何错误时</exception>
        object GetProperty(object target, string propertyName);

        /// <summary>
        /// 判断是否存在指定名称的属性
        /// 永远不抛出异常
        /// </summary>
        bool HasProperty(object target, string propertyName);

        /// <summary>
        /// 得到指定动态属性的类型
        /// </summary>
        /// <exception cref="Framework.Commons.Property.DynaAccessException">产生任何错误时</exception>
        Type GetPropertyType(object target, string propertyName);

        /// <summary>
        /// 得到代理的类型
        /// 永远不抛出异常
        /// </summary>
        Type ProxyType { get; }
    }
}
