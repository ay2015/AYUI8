using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using ay.contents;
using System.Windows.Shapes;

namespace ay.Controls
{
    [TemplatePart(Name = "PART_MENU", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MIN", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MAX", Type = typeof(Button))]
    [TemplatePart(Name = "PART_RESTORE", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CLOSE", Type = typeof(Button))]
    [TemplatePart(Name = "contentBorder", Type = typeof(Border))]
    public class AyWindowSimple : AyWindowBase, INotifyPropertyChanged
    {
   
        static AyWindowSimple()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AyWindowSimple),
                new FrameworkPropertyMetadata(typeof(AyWindowSimple)));
        }
        public AyWindowSimple():base()
        {
         
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CloseWindowMethodOverride != null)
                {
                    CloseWindowMethodOverride();
                }
                else
                {
                    DoCloseWindow();
                }
            }
            catch
            {

            }

        }

        public override void DoCloseWindow()
        {
            if (CloseIsHideWindow)
            {
                this.Hide();
            }
            else
            {
                try
                {
                    if (!ComfirmBeforeClose)
                    {
                        this.Close();
                    }
                    else
                    {
                        if (MessageBoxResult.OK == AyMessageBox.ShowQuestionOkCancel(Langs.ay_ConfirmWhenExitApp.Lang(), Langs.share_remind.Lang()))
                        {
                            this.Close();
                        }
                    }

                }
                catch
                {


                }

            }

        }


        public void MinButton_Click(object sender, RoutedEventArgs e)
        {
            if (MinWindowMethodOverride == null)
            {
                DoMinWindow();
            }
            else
            {
                MinWindowMethodOverride();
            }
        }

        public override void DoMinWindow()
        {
            if (this.ShowInTaskbar)
            {
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.Hide();
            }
        }

        public void ReMaxButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaxWindowMethodOverride == null)
            {
                DoRestoreOrMax();
            }
            else
            {
                MaxWindowMethodOverride();
            }

        }



        public override void DoRestoreOrMax()
        {
            if (this.WindowState == WindowState.Normal)
            {
                restoreWindowVisibility = Visibility.Visible;
                maxWindowVisibility = Visibility.Collapsed;
            }
            else
            {
                restoreWindowVisibility = Visibility.Collapsed;
                maxWindowVisibility = Visibility.Visible;
            }
            this.WindowState = (this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
            //特殊纠正 2018-10-31 05:02:44
            if (this.WindowState == WindowState.Maximized)
            {
                Size OutTaskBarSize = new Size(System.Windows.Forms.SystemInformation.WorkingArea.Width, System.Windows.Forms.SystemInformation.WorkingArea.Height);
                var _b = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                Size ScreenSize = new Size(_b.Width, _b.Height);
                Size TaskBarSize;
                TaskBarSize = new Size(
                                (ScreenSize.Width - (ScreenSize.Width - OutTaskBarSize.Width)),
                                (ScreenSize.Height - OutTaskBarSize.Height)
                                );
                var _44 = GetTaskBarLocation();
                if (TaskBarSize.Width > TaskBarSize.Height)
                {
                    //上下排列

                    if (_44 == TaskBarLocation.BOTTOM)
                    {
                        //下排列
                        ContentBorder.BorderThickness = new Thickness(6, 6, 6, TaskBarSize.Height + 6);
                        AllContentMargin = new Thickness(0, 0, 0, 0);
                    }
                    else if (_44 == TaskBarLocation.TOP)
                    {
                        //上排列
                        ContentBorder.BorderThickness = new Thickness(6, TaskBarSize.Height + 6, 6, 6);
                        AllContentMargin = new Thickness(0, 0, 0, 0);
                    }
                    else if (_44 == TaskBarLocation.LEFT)
                    {

                        ContentBorder.BorderThickness = new Thickness(TaskBarSize.Height + 6, 6, 6, 6);
                        AllContentMargin = new Thickness(ScreenSize.Width - OutTaskBarSize.Width, 0, 0, 0);
                    }
                    else if (_44 == TaskBarLocation.RIGHT)
                    {

                        ContentBorder.BorderThickness = new Thickness(6, 6, TaskBarSize.Height + 6, 6);
                        AllContentMargin = new Thickness(0, 0, ScreenSize.Width - OutTaskBarSize.Width, 0);
                    }
                }
            }
            else
            {
                ContentBorder.BorderThickness = new Thickness(0);

                AllContentMargin = new Thickness(0, 0, 0, 0);
            }
        }



        internal Thickness AllContentMargin
        {
            get { return (Thickness)GetValue(AllContentMarginProperty); }
            set { SetValue(AllContentMarginProperty, value); }
        }

        internal static readonly DependencyProperty AllContentMarginProperty =
            DependencyProperty.Register("AllContentMargin", typeof(Thickness), typeof(AyWindowSimple), new PropertyMetadata(new Thickness(0, 0, 0, 0)));


        private enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }

        private TaskBarLocation GetTaskBarLocation()
        {
            TaskBarLocation taskBarLocation = TaskBarLocation.BOTTOM;
            bool taskBarOnTopOrBottom = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);
            if (taskBarOnTopOrBottom)
            {
                if (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Top > 0) taskBarLocation = TaskBarLocation.TOP;
            }
            else
            {
                if (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Left > 0)
                {
                    taskBarLocation = TaskBarLocation.LEFT;
                }
                else
                {
                    taskBarLocation = TaskBarLocation.RIGHT;
                }
            }
            return taskBarLocation;
        }
        public override void DoShowWindowMenu()
        {
            if (WindowMenu != null)
            {
                WindowMenu.IsOpen = true;
            }
        }
        public Border ContentBorder = null;
        public override void OnApplyTemplate()
        {
            //Button PART_MENU = null;
            Button PART_MIN = null;
            Button PART_MAX = null;
            Button PART_RESTORE = null;
            Button PART_CLOSE = null;
            ayLayerAboveArea = GetTemplateChild("AyLayerAboveArea") as Grid;
            AyWindowMaskArea = GetTemplateChild("AyWindowMaskArea") as Rectangle;
            //PART_MENU = GetTemplateChild("PART_MENU") as Button;
            PART_MIN = GetTemplateChild("PART_MIN") as Button;
            PART_MAX = GetTemplateChild("PART_MAX") as Button;
            PART_RESTORE = GetTemplateChild("PART_RESTORE") as Button;
            PART_CLOSE = GetTemplateChild("PART_CLOSE") as Button;

            Button menuWindow = GetTemplateChild("PART_MENU") as Button;
            if (menuWindow != null)
            {
                if (WindowMenu != null)
                {
                    WindowMenuVisibility = Visibility.Visible;
                    WindowMenu.Placement = PlacementMode.Bottom;
                    WindowMenu.PlacementTarget = menuWindow;
                }

                menuWindow.Click += delegate
                {
                    if (MenuWindowMethodOverride == null)
                    {
                        DoShowWindowMenu();
                    }
                    else
                    {
                        MenuWindowMethodOverride();
                    }

                };
            }
            if (PART_RESTORE != null)
                PART_RESTORE.Click += ReMaxButton_Click;
            if (PART_MAX != null)
                PART_MAX.Click += ReMaxButton_Click;
            if (PART_MIN != null)
                PART_MIN.Click += MinButton_Click;
            if (PART_CLOSE != null)
                PART_CLOSE.Click += CloseButton_Click;

            ContentBorder = GetTemplateChild("contentBorder") as Border;
            //if (moveBarButton != null)
            //    moveBarButton.MouseLeftButtonDown += MoveBarButton_MouseLeftButtonDown;
            base.OnApplyTemplate();
        }

      
    }
}