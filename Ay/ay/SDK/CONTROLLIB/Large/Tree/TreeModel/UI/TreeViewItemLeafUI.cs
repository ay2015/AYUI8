using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ay.Controls
{
    public class TreeViewItemLeafUI : AyPropertyChanged, ITreeViewItemUI
    {
        public TreeViewItemLeafUI(TreeViewItemData data, ITreeViewItemUI parent)
        {
            this.parent = parent;
            this.data = data;
            this.Depth = parent == null ? 0 : (parent.Depth + 1);
        }

        private TreeViewItemData data = null;
        public TreeViewItemData Data
        {
            set
            {
                if (this.data != value)
                {
                    this.data = value;
                    OnPropertyChanged("Title");
                }
            }
            get { return this.data; }
        }

        #region 目录treeviewitem的基础模板属性
        private bool? isChecked = false;
        /// <summary>
        /// 是否选中,用于CheckBox
        /// </summary>
        public bool? IsChecked
        {
            set { Set(ref isChecked, value); }
            get { return this.isChecked; }
        }

        private ITreeViewItemUI parent = null;
        public ITreeViewItemUI Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private bool _IsExpanded;

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set { Set(ref _IsExpanded, value); }
        }


        public ObservableCollection<ITreeViewItemUI> Children
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this.data.Header; }
            set { }
        }
        private bool isInList = false;
        public bool IsInList
        {
            set
            {
                Set(ref isInList, value);
            }
            get { return this.isInList; }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
            get { return this.isSelected; }
        }

        public bool IsLeaf
        {
            get { return true; }
            set { }
        }

        public int OrderID
        {
            get { return this.data.OrderID; }
            set { }
        }

        public int Depth { get; set; }
        #endregion

    }
}
