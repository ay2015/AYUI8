using Ay.MvcFramework;
using RDS.Controllers;
using System.Windows.Controls;

namespace RDS.Views
{
    public partial class IndexView : Page, IIndexView
    {
        #region 框架通用基础
        IndexController Controller = null;
        private Actions<IndexController> _mvc;
        public Actions<IndexController> Mvc
        {
            get
            {
                if (_mvc == null)
                {
                    _mvc = new Actions<IndexController>(DataContext as IndexController);
                }
                return _mvc;
            }
        }
        public IndexView()
        {
            InitializeComponent();
            Loaded += IndexView_Loaded;
        }
        #endregion
        private void IndexView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Loaded -= IndexView_Loaded;
            #region 初始化绑定
            Controller = new IndexController(this);
            Controller.Initialize();
            DataContext = Controller;
            #endregion

        }


    }
}
