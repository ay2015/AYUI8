using System.Collections.Generic;

namespace ay.Controls
{
    public class TreeViewItemCatalogData : TreeViewItemData
    {

        private List<TreeViewItemCatalogData> _Catagory = new List<TreeViewItemCatalogData>();

        /// <summary>
        /// 第一种类型
        /// </summary>
        public List<TreeViewItemCatalogData> Catagory
        {
            get { return _Catagory; }
            set { Set(ref _Catagory, value); }
        }

        private List<TreeViewItemData> _Leafs = new List<TreeViewItemData>();

        /// <summary>
        /// 第二种类型
        /// </summary>
        public List<TreeViewItemData> Leafs
        {
            get { return _Leafs; }
            set { Set(ref _Leafs, value); }
        }
    }



}
