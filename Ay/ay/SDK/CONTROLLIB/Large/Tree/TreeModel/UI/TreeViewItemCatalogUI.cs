using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ay.Controls
{
    public class TreeViewItemCatalogUI : AyPropertyChanged, ITreeViewItemUI
    {
        public TreeViewItemCatalogUI(TreeViewItemCatalogData category, ITreeViewItemUI parent)
        {
            this.data = category;
            this.parent = parent;
            this.Depth = parent == null ? 0 : (parent.Depth + 1);
            foreach (var d in this.Data.Leafs)
            {
                this.Children.Add(new TreeViewItemLeafUI(d, this));
            }
            foreach (TreeViewItemCatalogData c in this.data.Catagory)
            {
                this.Children.Add(new TreeViewItemCatalogUI(c, this));
            }
        }

        #region 目录treeviewitem的基础模板属性
        private ITreeViewItemUI parent = null;
        public ITreeViewItemUI Parent
        {
            get { return parent; }
            set { parent = value; }
        }



        private ObservableCollection<ITreeViewItemUI> _Children = new ObservableCollection<ITreeViewItemUI>();

        /// <summary>
        /// 子元素
        /// </summary>
        public ObservableCollection<ITreeViewItemUI> Children
        {
            get { return _Children; }
            set { Set(ref _Children, value); }
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this.data.Header; }
            set { }
        }
        public int OrderID
        {
            get { return this.data.OrderID; }
            set { }
        }
        public bool IsInList
        {
            set;
            get;
        }
        private bool? isChecked = false;
        /// <summary>
        /// 是否选中,用于CheckBox
        /// </summary>
        public bool? IsChecked
        {
            set { Set(ref isChecked, value); }
            get { return this.isChecked; }
        }
        private bool isSelected = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            set { Set(ref isSelected, value); }
            get { return this.isSelected; }
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

        /// <summary>
        /// 是否是子节点
        /// </summary>
        public bool IsLeaf
        {
            set { }
            get { return false; }
        }

        public int Depth { get; set; }

        #endregion

        private TreeViewItemCatalogData data = null;

        public TreeViewItemCatalogData Data
        {
            set
            {
                Set(ref data, value);
            }
            get { return this.data; }
        }

    }
}
