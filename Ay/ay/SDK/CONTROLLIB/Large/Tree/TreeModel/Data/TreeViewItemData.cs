namespace ay.Controls
{
    public class TreeViewItemData : AyPropertyChanged
    {
        private string _Header;

        /// <summary>
        /// 树的内容
        /// </summary>
        public string Header
        {
            get { return _Header; }
            set { Set(ref _Header, value); }
        }

        private int _OrderID;

        /// <summary>
        /// 排序ID，
        /// </summary>
        public int OrderID
        {
            get { return _OrderID; }
            set { Set(ref _OrderID, value); }
        }

    }



}
