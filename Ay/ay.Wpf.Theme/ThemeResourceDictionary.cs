
using System;
using System.Windows;

public abstract class ThemeResourceDictionary : ResourceDictionary
{
    private StyleUsageMode m_styleUsageMode;

    public StyleUsageMode StyleUsageMode
    {
        get
        {
            return m_styleUsageMode;
        }
        set
        {
            if (value != m_styleUsageMode)
            {
                m_styleUsageMode = value;
                UpdateResources();
            }
        }
    }

    public ThemeResourceDictionary()
    {
    }
    /// <summary>
    /// 重点实现这个方法
    /// </summary>
    protected abstract void UpdateResources();
}

public static class ThemeResourceDictionaryExtensionMethod
{
    /// <summary>
    /// 应用主题
    /// </summary>
    /// <param name="resource">主题资源字典</param>
    /// <param name="element">控件级别的对象</param>
    public static void ApplyTheme(this ThemeResourceDictionary resource, FrameworkElement element)
    {
        if (resource != null)
        {
            element.Resources.MergedDictionaries.Clear();
            if (resource != null)
            {
                element.Resources.MergedDictionaries.Add(resource);
            }
        }
    }
    public static void AddTheme(this ThemeResourceDictionary resource, Application application)
    {
        if (resource != null)
        {
            application.Resources.MergedDictionaries.Add(resource);
        }
    }
    /// <summary>
    /// 应用主题
    /// </summary>
    /// <param name="resource">主题资源字典</param>
    /// <param name="element">App级别的对象</param>
    public static void ApplyTheme(this ThemeResourceDictionary resource, Application application)
    {
        if (resource != null)
        {
            application.Resources.MergedDictionaries.Clear();
            if (resource != null)
            {
                application.Resources.MergedDictionaries.Add(resource);
            }
        }
    }

}