using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ay.Controls
{
    /// <summary>
    /// 树定义基础
    /// </summary>
    public interface ITreeViewItemUI
    {
        ITreeViewItemUI Parent { get; set; }  
        ObservableCollection<ITreeViewItemUI> Children { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        bool? IsChecked { get; set; }
        bool IsInList { get; set; }
        bool IsLeaf { get; set; }
        string Title { get; set; }
        int Depth { get; set; }
        int OrderID { get; set; }
    }
}
