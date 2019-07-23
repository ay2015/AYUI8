using System.Collections.ObjectModel;

namespace ay.Controls
{
    public class AuthTreeViewItemModel : AyTreeViewItemModel
    {
        public AuthTreeViewItemModel()
        {

        }
        public AuthTreeViewItemModel(string text, string icon, AyTreeViewItemModel parent, bool isExpanded, object extValue) : base(text, icon, parent, isExpanded, extValue)
        {

        }
        private ObservableCollection<AyTreeViewItemModel> _Auths = new ObservableCollection<AyTreeViewItemModel>();

        /// <summary>
        /// 权限items
        /// </summary>
        public ObservableCollection<AyTreeViewItemModel> Auths
        {
            get { return _Auths; }
            set { Set(ref _Auths, value); }
        }

    }
}
