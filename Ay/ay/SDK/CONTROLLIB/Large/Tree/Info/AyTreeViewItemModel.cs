using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace ay.Controls
{

    /// <summary>
    /// 基本tree 杨洋
    /// 2015-5-20 17:17:58  增加深度Depth 从0开始
    /// </summary>
    public class AyTreeViewItemModel : AyPropertyChanged
    {
        public void CollapseAllChildren()
        {
            IsExpanded = false;
            foreach (var item in Children)
            {
                item.IsExpanded = false;
                if (item.Children.Count > 0)
                {
                    CollapseAllChildren1(item);
                }
            }
        }
        private void CollapseAllChildren1(AyTreeViewItemModel model)
        {
            foreach (var item in model.Children)
            {
                item.IsExpanded = false;
                if (item.Children.Count > 0)
                {
                    CollapseAllChildren1(item);
                }
            }
        }

        public void ExpandAllChildren()
        {
            IsExpanded = true;
            foreach (var item in Children)
            {
                item.IsExpanded = true;
                if (item.Children.Count > 0)
                {
                    ExpandAllChildren1(item);
                }
            }
        }
        private void ExpandAllChildren1(AyTreeViewItemModel model)
        {
            foreach (var item in model.Children)
            {
                item.IsExpanded = true;
                if (item.Children.Count > 0)
                {
                    ExpandAllChildren1(item);
                }
            }
        }

        /// <summary>
        /// 展开自己的父节点
        /// </summary>
        public void ExpandParent()
        {
            ExpandParent(this);
        }
        private void ExpandParent(AyTreeViewItemModel model)
        {
            if (model.ParentCategory != null)
            {
                model.ParentCategory.IsExpanded = true;
                ExpandParent(model.ParentCategory);
            }
        }
        /// <summary>
        /// 只移除下一级的子，不会递归移除
        /// </summary>
        /// <param name="id"></param>
        public void RemoveChild(string id)
        {
            AyTreeViewItemModel d = null;
            foreach (var item in Children)
            {
                if (item.id == id)
                {
                    d = item;
                    break;
                }
            }
            Children.Remove(d);
            d = null;
        }
        #region 2017-3-28 16:46:22 增加功能 此功能不推荐大数据，大数据推荐异步加载，单击节点加载数据
        //常规树，自定义图标树
        //
        private string _SwitchOpenIcon = "path_treeFolderOpen";

        /// <summary>
        /// 动态切换的图标
        /// </summary>
        public string SwitchOpenIcon
        {
            get { return _SwitchOpenIcon; }
            set { Set(ref _SwitchOpenIcon, value); }
        }
        private string _SwitchCloseIcon = "path_treeFolderClose";

        /// <summary>
        /// 未填写
        /// </summary>
        public string SwitchCloseIcon
        {
            get { return _SwitchCloseIcon; }
            set { Set(ref _SwitchCloseIcon, value); }
        }



        #endregion
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string pid;
        public string Pid
        {
            get { return pid; }
            set { pid = value; }
        }
        public AyTreeViewItemModel() { }
        private readonly ObservableCollection<AyTreeViewItemModel> children = new ObservableCollection<AyTreeViewItemModel>();

        private string text = String.Empty;




        private Guid uid;
        /// <summary>
        /// 默认生成的guid，唯一标识符
        /// </summary>
        public Guid Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private int _NodeType;

        /// <summary>
        /// 节点类型，默认0表示 普通  1代表文件夹，自己可以拓展
        /// </summary>
        public int NodeType
        {
            get { return _NodeType; }
            set { Set(ref _NodeType, value); }
        }



        public ObservableCollection<AyTreeViewItemModel> Children
        {
            get { return children; }
        }
        /// <summary>
        /// 此属性用于绑定显示的Text值
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }

            }
        }

        private bool subIsSelected;

        /// <summary>
        /// 子类中是否有选中的
        /// 此属性：用于 父节点使用 2017-3-28 16:49:47
        /// </summary>
        public bool SubIsSelected
        {
            get { return subIsSelected; }
            set
            {
                if (value != subIsSelected)
                {
                    subIsSelected = value;
                    OnPropertyChanged("SubIsSelected");
                }
            }
        }

        private bool parentIsSelf;
        /// <summary>
        /// 父节点是不是自己，如果是自己，则自己是选中的状态，当然选中状态不是selected的触发器触发的，是自己的这个属性触发
        /// 此属性：用于 子节点使用 2017-3-28 16:49:36
        /// </summary>
        public bool ParentIsSelf
        {
            get { return parentIsSelf; }
            set
            {
                if (value != parentIsSelf)
                {
                    parentIsSelf = value;
                    OnPropertyChanged("ParentIsSelf");
                }
            }
        }

        private AyTreeViewItemModel parent;
        public AyTreeViewItemModel ParentCategory
        {
            get { return parent; }
            set
            {
                Set(ref parent, value);
            }
        }


        public void UpdateDepth(bool addparent = false)
        {
            if (parent != null)
            {
                this.Depth = parent.Depth + 1;
                if (addparent)
                {
                    parent.children.Add(this);
                }
            }
        }
        #region 构造函数

        public AyTreeViewItemModel(string text)
        {
            this.text = text;
            UpdateDepth();
            Uid = Guid.NewGuid();

        }
        public AyTreeViewItemModel(string text, string icon) : this(text)
        {
            this.Icon = icon;
            //UpdateDepth();
            //Uid = Guid.NewGuid();
        }


        public AyTreeViewItemModel(string text, AyTreeViewItemModel parent)
            : this(text, null, parent)
        {
        }
        public AyTreeViewItemModel(string text, AyTreeViewItemModel parent, int nodetype)
        : this(text, null, parent)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent)
        {
            this.text = text;
            this.parent = parent;
            this.Icon = icon;
            UpdateDepth(true);
            Uid = Guid.NewGuid();
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, int nodetype) : this(text, icon, parent)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, int nodetype) : this(text, icon, parent, isExpanded)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded) : this(text, icon, parent)
        {
            this.IsExpanded = isExpanded;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object extValue, int nodetype)
            : this(text, icon, parent, isExpanded, extValue)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object extValue)
                 : this(text, icon, parent, isExpanded)
        {
            this.ExtValue = extValue;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object extValue, object[] extValues, int nodetype) : this(text, icon, parent, isExpanded, extValue, extValues)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object extValue, object[] extValues)
            : this(text, icon, parent, isExpanded, extValue)
        {
            this.ExtValues = extValues;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object[] extValues, int nodetype) : this(text, icon, parent, isExpanded, extValues)
        {
            this.NodeType = nodetype;
        }
        public AyTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object[] extValues)
            : this(text, icon, parent, isExpanded)
        {
            this.ExtValues = extValues;
        }

        #endregion

        public override string ToString()
        {
            return "aytree: " + text;
        }


        #region 其他补充

        //private string icon;
        ///// <summary>
        ///// 头像, AyIconAll结构
        ///// </summary>
        //public string Icon
        //{
        //    get { return icon; }
        //    set
        //    {
        //        icon = value;
        //        OnPropertyChanged("Icon");
        //    }
        //}


        private string icon;

        /// <summary>
        /// 未填写
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { Set(ref icon, value); }
        }

        private int depth = 0;

        /// <summary>
        /// 未填写
        /// </summary>
        public int Depth
        {
            get { return depth; }
            set { Set(ref depth, value); }
        }

        //private int depth = 0;
        //public int Depth
        //{
        //    get { return depth; }
        //    set
        //    {
        //        depth = value;
        //        OnPropertyChanged("Depth");
        //    }
        //}

        //展开时候,从集合拿数据
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;

                    OnPropertyChanged("IsExpanded");

                }

                //if (!HasChildren())
                //{
                //    Children.Remove(_temp);
                //    //LoadChildren();
                //}
            }
        }
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected && !value && Depth > 0)
                {
                    //如果原来选中，现在取消
                    this.ParentCategory.SubIsSelected = value;
                }
                if (!isSelected && value && Depth > 0)
                {
                    //如果原来未选中，现在选中
                    this.ParentCategory.SubIsSelected = value;
                }

                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        //public void UpdateParentChecked(bool _ck, AyTreeViewItemModel p)
        //{
        //    if (p.parent != null)
        //    {
        //        foreach (var item in child)
        //        {
        //            if (item.Children.Count > 0)
        //            {
        //                UpdateChecked(_ck, item.Children);
        //            }
        //            item.IsChecked = _ck;
        //        }
        //    }
        //}
        public void UpdateChecked(bool? _ck, ObservableCollection<AyTreeViewItemModel> child)
        {
            foreach (var item in child)
            {
                //if (item.Children.Count > 0)
                //{
                //    UpdateChecked(_ck, item.Children);
                //}
                item.IsNotifyParentSetChecked = false;
                item.IsChecked = _ck;
                item.IsNotifyParentSetChecked = true;
            }

        }
        internal bool IsNotifyChildSetChecked = true;
        internal bool IsNotifyParentSetChecked = true;
        internal bool IsHandCheckNull = true;
        private bool? _IsChecked = false;

        /// <summary>
        /// 是否checkbox选中
        /// </summary>
        public bool? IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (_IsChecked != value)
                {
                    if (IsHandCheckNull == true)
                    {
                        if (value == null) { value = false; }
                    }
                    _IsChecked = value;

                    OnPropertyChanged(nameof(IsChecked));
                    if (IsNotifyChildSetChecked)
                    {
                        UpdateChecked(value, Children);
                    }

                    if (IsNotifyParentSetChecked)
                    {
                        if (parent != null)
                        {

                            if (value == true)
                            {
                                var _1 = parent.Children.Where(x => x.IsChecked == true);
                                if (_1.Count() == parent.Children.Count)
                                {
                                    parent.IsNotifyChildSetChecked = false;
                                    parent.IsChecked = true;
                                    parent.IsNotifyChildSetChecked = true;
                                    return;
                                }
                                if (_1.Count() > 0)
                                {
                                    parent.IsNotifyChildSetChecked = false;
                                    parent.IsHandCheckNull = false;
                                    parent.IsChecked = null;
                                    parent.IsNotifyChildSetChecked = true;
                                    parent.IsHandCheckNull = true;
                                    return;
                                }
                            }
                            else
                            {
                                var _21 = parent.Children.Any(x => x.IsChecked == null);
                                if (_21)
                                {
                                    parent.IsNotifyChildSetChecked = false;
                                    parent.IsHandCheckNull = false;
                                    parent.IsChecked = null;
                                    parent.IsNotifyChildSetChecked = true;
                                    parent.IsHandCheckNull = true;
                                    return;
                                }
                                var _1 = parent.Children.Any(x => x.IsChecked == true);
                                if (_1)
                                {
                                    parent.IsNotifyChildSetChecked = false;
                                    parent.IsHandCheckNull = false;
                                    parent.IsChecked = null;
                                    parent.IsNotifyChildSetChecked = true;
                                    parent.IsHandCheckNull = true;
                                }
                                else
                                {
                                    parent.IsNotifyChildSetChecked = false;
                                    parent.IsChecked = false;
                                    parent.IsNotifyChildSetChecked = true;
                                }
                            }
                        }

                    }
                    IsNotifyChildSetChecked = true;
                }
            }
        }


        private TreeViewItem relativeItem;
        /// <summary>
        /// 相关item对象
        /// </summary>
        public TreeViewItem RelativeItem
        {
            get { return relativeItem; }
            set { relativeItem = value; }
        }

        #endregion

        #region 拓展属性 2015-6-12 13:56:12 增加
        private object extValue;
        /// <summary>
        /// 用于放置单个属性值
        /// </summary>
        public object ExtValue
        {
            get { return extValue; }
            set
            {
                Set(ref extValue, value);
            }
        }


        private object[] extValues;
        /// <summary>
        /// 用于放置多个值
        /// </summary>
        public object[] ExtValues
        {
            get { return extValues; }
            set
            {
                Set(ref extValues, value);
            }
        }

        #endregion
    }
}
