using ay.Controls;
using Ay.MvcFramework;
using RDS.Controllers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RDS
{

    public partial class _ViewStart : AyWindow, IViewStartView
    {
        #region 框架通用基础
        ViewStartController Controller = null;
        private Actions<ViewStartController> _mvc;
        public Actions<ViewStartController> Mvc
        {
            get
            {
                if (_mvc == null)
                {
                    _mvc = new Actions<ViewStartController>(DataContext as ViewStartController);
                }
                return _mvc;
            }
        }
        public _ViewStart()
        {
            InitializeComponent();
            Loaded += ViewStartView_Loaded;
        }
        #endregion
        private void ViewStartView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Loaded -= ViewStartView_Loaded;
            Application.Current.MainWindow = this;
            App.IndexWindow = this;
            #region 初始化绑定
            Controller = new ViewStartController(this);
            Controller.Initialize();
            DataContext = Controller;
            #endregion

        }

        #region 通用导航
        public void NavPage(string uri)
        {
            frame.Source = new Uri(uri, UriKind.RelativeOrAbsolute);
        }
        public void NavPage(Page page)
        {
            frame.NavigationService.Navigate(page);
        }
        public void NavBack()
        {
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
        public void NavForward()
        {
            if (frame.CanGoForward)
            {
                frame.GoForward();
            }
        }

        //private void back2_Click(object sender, RoutedEventArgs e)
        //{
        //    App.IndexWindow.NavPage(App.IndexView);
        //}
        #endregion


    }
}
