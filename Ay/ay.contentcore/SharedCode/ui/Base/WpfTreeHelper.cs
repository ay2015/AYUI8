using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
public static class TreeHelper
{
    public static DependencyObject GetParent(DependencyObject element)
    {
        return GetParent(element, true);
    }

    private static DependencyObject GetParent(DependencyObject element, bool recurseIntoPopup)
    {
        if (recurseIntoPopup)
        {
            Popup popup = element as Popup;
            if (popup != null && popup.PlacementTarget != null)
            {
                return popup.PlacementTarget;
            }
        }
        Visual visual = element as Visual;
        DependencyObject dependencyObject = (visual == null) ? null : VisualTreeHelper.GetParent(visual);
        if (dependencyObject == null)
        {
            FrameworkElement frameworkElement = element as FrameworkElement;
            if (frameworkElement != null)
            {
                dependencyObject = frameworkElement.Parent;
                if (dependencyObject == null)
                {
                    dependencyObject = frameworkElement.TemplatedParent;
                }
            }
            else
            {
                FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
                if (frameworkContentElement != null)
                {
                    dependencyObject = frameworkContentElement.Parent;
                    if (dependencyObject == null)
                    {
                        dependencyObject = frameworkContentElement.TemplatedParent;
                    }
                }
            }
        }
        return dependencyObject;
    }

    public static T FindParent<T>(DependencyObject startingObject) where T : DependencyObject
    {
        return FindParent<T>(startingObject, false, null);
    }

    public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject) where T : DependencyObject
    {
        return FindParent<T>(startingObject, checkStartingObject, null);
    }

    public static T FindParent<T>(DependencyObject startingObject, bool checkStartingObject, Func<T, bool> additionalCheck) where T : DependencyObject
    {
        for (DependencyObject dependencyObject = checkStartingObject ? startingObject : GetParent(startingObject, true); dependencyObject != null; dependencyObject = GetParent(dependencyObject, true))
        {
            T val = dependencyObject as T;
            if (val != null)
            {
                if (additionalCheck == null)
                {
                    return val;
                }
                if (additionalCheck(val))
                {
                    return val;
                }
            }
        }
        return null;
    }

    public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
    {
        return FindChild<T>(parent, null);
    }

    public static T FindChild<T>(DependencyObject parent, Func<T, bool> additionalCheck) where T : DependencyObject
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            T val = VisualTreeHelper.GetChild(parent, i) as T;
            if (val != null)
            {
                if (additionalCheck == null)
                {
                    return val;
                }
                if (additionalCheck(val))
                {
                    return val;
                }
            }
        }
        for (int j = 0; j < childrenCount; j++)
        {
            T val = FindChild(VisualTreeHelper.GetChild(parent, j), additionalCheck);
            if (val != null)
            {
                return val;
            }
        }
        return null;
    }

    public static bool IsDescendantOf(DependencyObject element, DependencyObject parent)
    {
        return IsDescendantOf(element, parent, true);
    }

    public static bool IsDescendantOf(DependencyObject element, DependencyObject parent, bool recurseIntoPopup)
    {
        while (element != null)
        {
            if (element == parent)
            {
                return true;
            }
            element = GetParent(element, recurseIntoPopup);
        }
        return false;
    }
}
public class WpfTreeHelper
{

    //2016-4-12 22:17:05 用于截图
    //public static void CreateBitmapFromVisual(Visual target, string fileName)
    //{
    //    CreateBitmapFromVisual(target, fileName, PixelFormats.Pbgra32);
    //}
    ////2016-4-12 22:17:05 用于截图
    //public static void CreateBitmapFromVisual(Visual target, string fileName, PixelFormat fileType)
    //{
    //    if (target == null || string.IsNullOrEmpty(fileName))
    //    {
    //        return;
    //    }

    //    Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

    //    RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, fileType);

    //    DrawingVisual visual = new DrawingVisual();

    //    using (DrawingContext context = visual.RenderOpen())
    //    {
    //        VisualBrush visualBrush = new VisualBrush(target);
    //        context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
    //    }

    //    renderTarget.Render(visual);
    //    PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
    //    bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
    //    using (Stream stm = File.Create(fileName))
    //    {
    //        bitmapEncoder.Save(stm);
    //    }
    //}


    public static T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(element);
        var children = new FrameworkElement[childrenCount];

        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
            children[i] = child;
            if (child is T)
                return (T)child;
        }

        for (int i = 0; i < childrenCount; i++)
            if (children[i] != null)
            {
                var subChild = FindFirstChild<T>(children[i]);
                if (subChild != null)
                    return subChild;
            }

        return null;
    }

    public static T FindParentControl<T>(DependencyObject outerDepObj) where T : DependencyObject
    {
        if (outerDepObj == null) return null;
        DependencyObject dObj = VisualTreeHelper.GetParent(outerDepObj);
        if (dObj == null)
            return null;

        if (dObj is T)
            return dObj as T;

        while ((dObj = VisualTreeHelper.GetParent(dObj)) != null)
        {
            if (dObj is T)
                return dObj as T;
        }

        return null;
    }

    /// <summary>
    /// 查找某种类型的子控件，并返回一个List集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="typename"></param>
    /// <returns></returns>
    public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
    {
        DependencyObject child = null;
        List<T> childList = new List<T>();

        for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);

            if (child is T && (((T)child).GetType() == typename))
            {
                childList.Add((T)child);
            }
            childList.AddRange(GetChildObjects<T>(child, typename));
        }

        return childList;
    }
    /// <summary>
    /// 通过名称查找子控件，并返回一个List集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject child = null;
        List<T> childList = new List<T>();

        for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);

            if (child is T && (((T)child).GetType().ToString() == name | string.IsNullOrEmpty(name)))
            {
                childList.Add((T)child);
            }
            childList.AddRange(GetChildObjects<T>(child, name));
        }
        return childList;
    }

    /// <summary>
    /// 通过名称查找某子控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject child = null;
        T grandChild = null;

        for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);

            if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
            {
                return (T)child;
            }
            else
            {
                grandChild = GetChildObject<T>(child, name);
                if (grandChild != null)
                    return grandChild;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过名称查找父控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject parent = VisualTreeHelper.GetParent(obj);

        while (parent != null)
        {
            if (parent is T && (((T)parent).Name == name | string.IsNullOrEmpty(name)))
            {
                return (T)parent;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }

    /// <summary>
    /// 复制一个节点
    /// </summary>
    /// <param name="elementToClone"></param>
    /// <returns></returns>
    public static object CloneElement(object elementToClone)
    {
        string xaml = XamlWriter.Save(elementToClone);
        return XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
    }

    /// <summary>
    /// 查找可视子元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="visual"></param>
    /// <returns></returns>
    public static T FindVisualChild<T>(Visual visual) where T : Visual
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
        {
            Visual child = (Visual)VisualTreeHelper.GetChild(visual, i);
            if (child != null)
            {
                T correctlyTyped = child as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }
                T descendent = FindVisualChild<T>(child);
                if (descendent != null)
                {
                    return descendent;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 从数据模板中获得数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public T FindFirstVisualChildFromDataTemplate<T>(DependencyObject obj, string childName) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is T && child.GetValue(FrameworkElement.NameProperty).ToString() == childName)
            {
                return (T)child;
            }
            else
            {
                T childOfChild = FindFirstVisualChildFromDataTemplate<T>(child, childName);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
        }
        return null;
    }

    ///遍历Listbox绑定模板中的控件
    //var lbCurrent = Template.FindName("lb_ConfigViewPropertys", this) as ListBox;

    //      if (lbCurrent != null)
    //      {
    //          foreach (object o in lbCurrent.Items)
    //          {
    //              ListBoxItem lbi = lbCurrent.ItemContainerGenerator.ContainerFromItem(o) as ListBoxItem;
    //              if (lbi != null)
    //              {
    //                  Button btnSendValue = FindFirstVisualChild2<Button>(lbi, "btnSendValue");
    //                  if (btnSendValue != null)
    //                  {
    //                      btnSendValue.Click += btnSendValue_Click;
    //                  }

    //              }
    //          }
    //      }


    public static T FindChild<T>(DependencyObject reference) where T : class
    {
        // Do a breadth first search.
        var queue = new Queue<DependencyObject>();
        queue.Enqueue(reference);
        while (queue.Count > 0)
        {
            DependencyObject child = queue.Dequeue();
            T obj = child as T;
            if (obj != null)
            {
                return obj;
            }

            // Add the children to the queue to search through later.
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(child); i++)
            {
                queue.Enqueue(VisualTreeHelper.GetChild(child, i));
            }
        }
        return null; // Not found.
    }



    private static bool? _isInDesignMode;
    /// <summary>
    /// Gets a value indicating whether the control is in design mode (running in Blend
    /// or Visual Studio).
    /// </summary>
    public static bool IsInDesignMode
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

    //private static Action EmptyDelegate = delegate () { };

    //public static void Refresh(this UIElement uiElement)
    //{
    //    uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
    //}



}
