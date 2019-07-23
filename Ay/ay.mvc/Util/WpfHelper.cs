using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

internal static class WpfHelper
{

    internal static BindingExpression GetBindingExpression(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
    {
        return (dependencyObject.ReadLocalValue(dependencyProperty) as BindingExpression);
    }

    private static bool? _isInDesignMode;
    /// <summary>
    /// 是否涉及模式
    /// </summary>
    internal static bool IsInDesignMode
    {
        get
        {
            if (!_isInDesignMode.HasValue)
            {
#if SILVERLIGHT
            _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                _isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
#endif
            }
            return _isInDesignMode.Value;
        }
    }


    internal static DependencyObject FindChild(this DependencyObject reference, string childName, Type childType)
    {
        DependencyObject foundChild = null;
        if (reference != null)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(reference, i);
                // If the child is not of the request child type child
                if (child.GetType() != childType)
                {
                    // recursively drill down the tree
                    foundChild = FindChild(child, childName, childType);
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = child;
                    break;
                }
            }
        }
        return foundChild;
    }

    internal static T GetVisualAncestor<T>(this DependencyObject d) where T : class
    {
        DependencyObject item = VisualTreeHelper.GetParent(d);

        while (item != null)
        {
            T itemAsT = item as T;
            if (itemAsT != null) return itemAsT;
            item = VisualTreeHelper.GetParent(item);
        }

        return null;
    }
    /// <summary>
    /// 从逻辑树找控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="d"></param>
    /// <returns></returns>
    internal static T GetLogicalAncestor<T>(this DependencyObject d) where T : class
    {
        DependencyObject item = LogicalTreeHelper.GetParent(d);

        while (item != null)
        {
            T itemAsT = item as T;
            if (itemAsT != null) return itemAsT;
            item = LogicalTreeHelper.GetParent(item);
        }

        return null;
    }



    internal static DependencyObject GetVisualAncestor(this DependencyObject d, Type type)
    {
        DependencyObject item = VisualTreeHelper.GetParent(d);

        while (item != null)
        {
            if (item.GetType() == type) return item;
            item = VisualTreeHelper.GetParent(item);
        }

        return null;
    }

    internal static T GetVisualDescendent<T>(this DependencyObject d) where T : DependencyObject
    {
        return d.GetVisualDescendents<T>().FirstOrDefault();
    }

    internal static IEnumerable<T> GetVisualDescendents<T>(this DependencyObject d) where T : DependencyObject
    {
        int childCount = VisualTreeHelper.GetChildrenCount(d);

        for (int n = 0; n < childCount; n++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(d, n);

            if (child is T)
            {
                yield return (T)child;
            }

            foreach (T match in GetVisualDescendents<T>(child))
            {
                yield return match;
            }
        }

        yield break;
    }



}
