/**----------------------------------------------- 
 *  ====================www.ayjs.net       杨洋    wpfui.com        ayui      ay  aaronyang======使用代码请注意侵权========= 
 * 
 * 作者：ay 
 * 联系QQ：875556003 
 * 时间2016-6-24 15:27:39 
 * 最后修改时间：2019-06-14 增加名字记录集合和取消注册，因为注册的名字会很多，如果n个，则外侧可能需要声明N个字符串去存储
 * -----------------------------------------*/
using System.Collections.Generic;
using System.Windows;

namespace ay.Animate
{
    /// <summary>
    /// 命名服务注册器，Ay设计给WPF控件 注册名字和删除名字，还有注册资源
    /// </summary>
    public class NameRegister
    {
        public NameRegister()
        {

        }
        /// <summary>
        /// 前缀，因为wpf中，控件的name属性不能数字开头 
        /// </summary>
        internal string namePrix = "ay2020";

        public string GetName(DependencyObject obj)
        {
            var tex = obj.GetValue(FrameworkElement.NameProperty);
            if (tex != null && tex.ToString() != "")
            {
                return tex.ToString();
            }
            return namePrix + obj.GetHashCode().ToString();
        }
        List<string> Names = new List<string>();
        public NameRegister(FrameworkElement Element)
        {
            var win = Window.GetWindow(Element);
            if (win != null)
            {
                this.Win = win;
            }

            if (Element != null)
            {
                WinFramework = Element;
                return;
            }
            //win = GetLogicalAncestor<>(element);   
            DependencyObject item = LogicalTreeHelper.GetParent(Element);
            if (item is FrameworkElement && item != null)
            {
                WinFramework = item as FrameworkElement;
            }
            else
            {
                DependencyObject item2 = LogicalTreeHelper.GetParent(item);
                if (item2 is FrameworkElement && item2 != null)
                {
                    WinFramework = item2 as FrameworkElement;
                }
                else
                {
                    DependencyObject item3 = LogicalTreeHelper.GetParent(item2);
                    if (item3 is FrameworkElement && item3 != null)
                    {
                        WinFramework = item3 as FrameworkElement;
                    }
                }

            }
  
        }
        internal FrameworkElement WinFramework { get; set; }

        internal Window Win { get; set; }

        public ResourceDictionary Resources
        {
            get
            {
                if (Win != null)
                {
                    return Win.Resources;
                }
                else if (WinFramework != null)
                {
                    return WinFramework.Resources;
                }
                return null;
            }
        }
        public void RegisterName(string name, object scopedElement)
        {
      
             if (WinFramework != null)
            {
                name = namePrix + name;
                WinFramework.RegisterName(name, scopedElement);
                if (!Names.Contains(name))
                {
                    Names.Add(name);
                }
            }
            else if (Win != null)
            {
                name = namePrix + name;
                Win.RegisterName(name, scopedElement);
                if (!Names.Contains(name))
                {
                    Names.Add(name);
                }
            }

        }

        public void RegisterName(object scopedElement)
        {
        
            if (WinFramework != null)
            {
                string name = scopedElement.GetHashCode().ToString();
                name = namePrix + name;
                WinFramework.RegisterName(name, scopedElement);
                if (!Names.Contains(name))
                {
                    Names.Add(name);
                }
            }
            else if (Win != null)
            {
                string name = scopedElement.GetHashCode().ToString();
                name = namePrix + name;
                Win.RegisterName(name, scopedElement);
                if (!Names.Contains(name))
                {
                    Names.Add(name);
                }
            }

        }

        public void UnregisterName(string name, bool isDelete = true)
        {
            if (Win != null)
            {
                Win.UnregisterName(name);
                if (isDelete)
                    if (Names.Contains(name))
                    {
                        Names.Remove(name);
                    }
            }
            else if (WinFramework != null)
            {
                WinFramework.UnregisterName(name);
                if (isDelete)
                    if (Names.Contains(name))
                    {
                        Names.Remove(name);
                    }
            }
        }

        /// <summary>
        /// 取消本次所有注册的名字
        /// </summary>
        public void UnRegisterNameAll()
        {
            foreach (var item in Names)
            {
                UnregisterName(item, false);
            }
            Names.Clear();
        }

    }

}
