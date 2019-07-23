using ay.contents;
using ay.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using Winform = System.Windows.Forms;

/// <summary>
/// 一些 packuri 模板，一些window操作，屏幕最大化，获得窗口位置
/// </summary>
public static class AyExtension
{

    #region 2016-9-23 22:42:30 增加控件关闭所在窗体
    /// <summary>
    /// ay 2016-9-23 增加: 控件关闭  所在窗体，调用窗体的Close方法
    /// <param name="element">控件</param>
    public static void CloseParentAyWindow(this UIElement element)
    {
        var a = Window.GetWindow(element) as AyWindow;
        if (a.IsNotNull()) a.DoCloseWindow();
    }

    public static void CloseAyLayerNotTriggerClosed(this FrameworkElement element)
    {
        //查找aylayer 
        var _2 = element.GetVisualAncestor<AyLayer>();
        if (_2.IsNotNull())
        {
            _2.CloseAyLayerNotTriggerClosed(_2._options.LayerId, _2);
        }
    }
    public static void CloseParentAyLayer(this FrameworkElement element)
    {
        //查找aylayer 
        var _2 = element.GetVisualAncestor<AyLayer>();
        if (_2!=null)
        {
            _2.CloseAyLayer(_2._options.LayerId, _2);
        }
    }
    public static void CloseParentLiAyLayer(this FrameworkElement element)
    {
        //查找aylayer 
        var _2 = element.GetVisualAncestor<AyLayer>();
        if (_2.IsNotNull())
        {
            _2.CloseLIAyLayer(_2._options.LayerId, _2);
        }
    }

    public static void ShowMessageBox<T>(this CommonReturnDTO<T> returnDTO,Window owner=null) where T:class
    {
        //查找aylayer 
        if (returnDTO.IsSuccess)
        {
            AyMessageBox.ShowRight(owner,GetMessageByType<T>(returnDTO), Langs.share_tip.Lang());
        }
        else if (returnDTO.Error != null)
        {
            AyMessageBox.ShowError(owner, GetMessageByType<T>(returnDTO), Langs.share_tip.Lang());
        }
        else
        {
            AyMessageBox.ShowInformation(owner, GetMessageByType<T>(returnDTO), Langs.share_tip.Lang());
        }
    }
    public static void ShowMessageBox(this CommonReturnDTO returnDTO, Window owner = null)
    {
        //查找aylayer 
        if (returnDTO.IsSuccess)
        {
            AyMessageBox.ShowRight(owner, GetMessageByType(returnDTO), Langs.share_tip.Lang());
        }
        else if (returnDTO.Error != null)
        {
            AyMessageBox.ShowError(owner, GetMessageByType(returnDTO), Langs.share_tip.Lang());
        }
        else
        {
            AyMessageBox.ShowInformation(owner, GetMessageByType(returnDTO), Langs.share_tip.Lang());
        }
    }
    public static string GetMessageByType<T>(CommonReturnDTO<T> dto)
    {
        if (!dto.IsSuccess && dto.Error != null)
        {
            return dto.Error;
        }
        if (dto.Type == 1 && dto.IsSuccess)
        {
            return Langs.ay_returnaddsuccess.Lang();
        }
        else if (dto.Type == 1 && !dto.IsSuccess)
        {
            return Langs.ay_returnaddfail.Lang();
        }
        else if (dto.Type == 2 && dto.IsSuccess)
        {
            return Langs.ay_returndelsuccess.Lang();
        }
        else if (dto.Type == 2 && !dto.IsSuccess)
        {
            return Langs.ay_returndelfail.Lang();
        }
        else if (dto.Type == 3 && dto.IsSuccess)
        {
            return Langs.ay_returneditsuccess.Lang();
        }
        else if (dto.Type == 3 && !dto.IsSuccess)
        {
            return Langs.ay_returneditfail.Lang();
        }
        else if (dto.Type == 4 && dto.IsSuccess)
        {
            return Langs.ay_returnsavesuccess.Lang();
        }
        else if (dto.Type == 4 && !dto.IsSuccess)
        {
            return Langs.ay_returnsavefail.Lang();
        }
        else if (dto.Type == 5 && dto.IsSuccess)
        {
            return Langs.ay_returnactionsuccess.Lang();
        }
        else if (dto.Type == 5 && !dto.IsSuccess)
        {
            return Langs.ay_returnactionfail.Lang();
        }
        else if (dto.Type == 6 && dto.IsSuccess)
        {
            return Langs.ay_returnsearchsuccess.Lang();
        }
        else if (dto.Type == 6 && !dto.IsSuccess)
        {
            return Langs.ay_returnsearchfail.Lang();
        }
        else if (dto.Type == 7 && dto.IsSuccess)
        {
            return Langs.ay_returngetsuccess.Lang();
        }
        else if (dto.Type == 7 && !dto.IsSuccess)
        {
            return Langs.ay_returngetfail.Lang();
        }
        return "";
    }
    public static string GetMessageByType(CommonReturnDTO dto)
    {
        if (!dto.IsSuccess && dto.Error != null)
        {
            return dto.Error;
        }
        if (dto.Type == 1 && dto.IsSuccess)
        {
            return Langs.ay_returnaddsuccess.Lang();
        }
        else if (dto.Type == 1 && !dto.IsSuccess)
        {
            return Langs.ay_returnaddfail.Lang();
        }
        else if (dto.Type == 2 && dto.IsSuccess)
        {
            return Langs.ay_returndelsuccess.Lang();
        }
        else if (dto.Type == 2 && !dto.IsSuccess)
        {
            return Langs.ay_returndelfail.Lang();
        }
        else if (dto.Type == 3 && dto.IsSuccess)
        {
            return Langs.ay_returneditsuccess.Lang();
        }
        else if (dto.Type == 3 && !dto.IsSuccess)
        {
            return Langs.ay_returneditfail.Lang();
        }
        else if (dto.Type == 4 && dto.IsSuccess)
        {
            return Langs.ay_returnsavesuccess.Lang();
        }
        else if (dto.Type == 4 && !dto.IsSuccess)
        {
            return Langs.ay_returnsavefail.Lang();
        }
        else if (dto.Type == 5 && dto.IsSuccess)
        {
            return Langs.ay_returnactionsuccess.Lang();
        }
        else if (dto.Type == 5 && !dto.IsSuccess)
        {
            return Langs.ay_returnactionfail.Lang();
        }
        else if (dto.Type == 6 && dto.IsSuccess)
        {
            return Langs.ay_returnsearchsuccess.Lang();
        }
        else if (dto.Type == 6 && !dto.IsSuccess)
        {
            return Langs.ay_returnsearchfail.Lang();
        }
        else if (dto.Type == 7 && dto.IsSuccess)
        {
            return Langs.ay_returngetsuccess.Lang();
        }
        else if (dto.Type == 7 && !dto.IsSuccess)
        {
            return Langs.ay_returngetfail.Lang();
        }
        return "";
    }
    public static void CloseParentWindow(this FrameworkElement element)
    {
        var a = Window.GetWindow(element);
        if (a.IsNotNull()) a.Close();
    }
    #endregion

    public static System.Windows.Media.ImageSourceConverter _ImageSourceConverter;
    /// <summary>
    /// ImageSourceConverter
    /// </summary>
    public static System.Windows.Media.ImageSourceConverter ImageSourceConverter
    {
        get
        {
            if (_ImageSourceConverter == null)
            {
                _ImageSourceConverter = new System.Windows.Media.ImageSourceConverter();
            }
            return _ImageSourceConverter;
        }
    }


    #region windows的拓展方法

    #region 2016-6-20 00:53:04 增加窗体移动和双击最大化，单击移动
    /// <summary>
    /// 用于AyWindow，快速创建单击拖动，双击最大化或者还原窗体
    /// 添加时间：2016-6-20 01:13:22
    /// 作者：AY
    /// <param name="element"></param>
    /// <param name="e">鼠标对象</param>
    /// <param name="ClickTwoAction">双击执行的代码</param>
    /// <returns></returns>
    public static void SetAyWindowMouseLeftButtonCommonClick(UIElement element, Action ClickTwoAction = null)
    {
        if (element == null) return;
        ay.Controls.AyWindowBase aywindow = Window.GetWindow(element) as ay.Controls.AyWindowBase;
        if (aywindow == null)
        {
            return;
        }
        element.MouseLeftButtonDown += (sender, e) =>
        {
            if (e.ClickCount == 2)
            {
                if (ClickTwoAction == null)
                {
                    if (aywindow.MaxWindowMethodOverride == null)
                    {
                        aywindow.DoRestoreOrMax();
                    }
                    else
                    {
                        aywindow.MaxWindowMethodOverride();
                    }

                }
                else
                {
                    ClickTwoAction();
                }
                e.Handled = true;
            }
            if (e.ClickCount == 1)
            {
                if (aywindow.WindowState == WindowState.Normal)
                {
                    aywindow.DragMove();
                }
            }
            e.Handled = true;
        };
    }

    /// <summary>
    /// 用于AyWindow，快速创建单击拖动，双击最大化或者还原窗体
    /// 添加时间：2016-6-20 01:13:58
    /// 作者：AY
    /// </summary>
    /// <param name="element"></param>
    /// <param name="e">鼠标对象</param>
    /// <param name="ClickTwoAction">双击执行的代码</param>
    /// <returns></returns>
    public static void SetAyWindowMouseLeftButtonCommonClick(ay.Controls.AyWindowBase aywindow, Action ClickTwoAction = null)
    {
        if (aywindow == null)
        {
            return;
        }
        aywindow.MouseLeftButtonDown += (sender, e) =>
          {
              if (e.ClickCount == 2)
              {
                  if (ClickTwoAction == null)
                  {
                      if (aywindow.MaxWindowMethodOverride == null)
                      {
                          aywindow.DoRestoreOrMax();
                      }
                      else
                      {
                          aywindow.MaxWindowMethodOverride();
                      }

                  }
                  else
                  {
                      ClickTwoAction();
                  }
                  e.Handled = true;
              }
              if (e.ClickCount == 1)
              {
                  if (aywindow.WindowState == WindowState.Normal)
                  {
                      aywindow.DragMove();
                  }
              }
              e.Handled = true;
          };
    }

    public static void SetAyWindowMouseLeftButtonMove(Window aywindow)
    {
        if (aywindow == null)
        {
            return;
        }
        aywindow.MouseLeftButtonDown += (sender, e) =>
        {

            if (e.ClickCount == 1)
            {
                if (aywindow.WindowState == WindowState.Normal)
                {
                    aywindow.DragMove();
                }
            }
            e.Handled = true;
        };
    }
    public static void SetAyWindowMouseLeftButtonMove(ay.Controls.AyWindowBase aywindow, UIElement element)
    {
        if (element == null) return;
        if (aywindow == null)
        {
            return;
        }
        element.MouseLeftButtonDown += (sender, e) =>
        {

            if (e.ClickCount == 1)
            {
                if (aywindow.WindowState == WindowState.Normal)
                {
                    aywindow.DragMove();
                }
            }
            e.Handled = true;
        };
    }
    public static void SetWindowMouseLeftButtonMove(Window aywindow, UIElement element)
    {
        if (element == null) return;
        if (aywindow == null)
        {
            return;
        }
        element.MouseLeftButtonDown += (sender, e) =>
        {

            if (e.ClickCount == 1)
            {
                if (aywindow.WindowState == WindowState.Normal)
                {
                    aywindow.DragMove();
                }
            }
            e.Handled = true;
        };
    }
    public static void SetWindowMouseLeftButtonCommonClick(Window window, UIElement element)
    {
        if (element == null) return;
        if (window == null)
        {
            return;
        }
        element.MouseLeftButtonDown += (sender, e) =>
        {
            if (e.ClickCount == 2)
            {
                if (window.WindowState == WindowState.Normal)
                {
                    window.WindowState = (window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                }
                else
                {

                    window.WindowState = (window.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
                }
                e.Handled = true;
            }
            if (e.ClickCount == 1)
            {
                if (window.WindowState == WindowState.Normal)
                {
                    window.DragMove();
                }
            }
            e.Handled = true;
        };
    }

    public static void SetAyWindowMouseLeftButtonCommonClick(ay.Controls.AyWindowBase aywindow, UIElement element, Action ClickTwoAction = null)
    {
        if (element == null) return;
        if (aywindow == null)
        {
            return;
        }
        element.MouseLeftButtonDown += (sender, e) =>
        {
            if (e.ClickCount == 2)
            {
                if (ClickTwoAction == null)
                {
                    if (aywindow.MaxWindowMethodOverride == null)
                    {
                        aywindow.DoRestoreOrMax();
                    }
                    else
                    {
                        aywindow.MaxWindowMethodOverride();
                    }

                }
                else
                {
                    ClickTwoAction();
                }
                e.Handled = true;
            }
            if (e.ClickCount == 1)
            {
                if (aywindow.WindowState == WindowState.Normal)
                {
                    aywindow.DragMove();
                }
            }
            e.Handled = true;
        };
    }


    #endregion


    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern IntPtr GetForegroundWindow();
    [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowRect")]
    public static extern int GetWindowRect(IntPtr hwnd, ref System.Drawing.Rectangle lpRect);

    public static Window ToExtensionMaxScreen(this Window window, Winform.Screen extScreen)
    {
        if (extScreen.Primary)
        {
            window.WindowState = WindowState.Maximized;
        }
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        System.Drawing.Rectangle mswa = extScreen.WorkingArea;
        window.Left = mswa.Left;
        window.Top = mswa.Top;
        window.Width = mswa.Width;
        window.Height = mswa.Height;

        return window;
    }
    public static Window ToExtensionNormalScreen(this Window window, Winform.Screen extScreen, bool isNeedMax = true)
    {
        if (isNeedMax)
        {
            window.WindowState = WindowState.Maximized;
        }
        window.WindowStartupLocation = WindowStartupLocation.Manual;
        System.Drawing.Rectangle mswa = extScreen.WorkingArea;
        window.Left = mswa.Left;
        window.Top = mswa.Top;
        window.Width = mswa.Width;
        window.Height = mswa.Height;
        return window;
    }
    /// <summary>
    /// 根据激活窗口句柄获得窗体位置
    /// </summary>
    /// <returns></returns>
    public static System.Drawing.Rectangle GetWindowRectangle(this Window w)
    {
        if (w.WindowState == WindowState.Maximized)
        {
            var handle = new System.Windows.Interop.WindowInteropHelper(w).Handle;
            var screen = System.Windows.Forms.Screen.FromHandle(handle);
            return screen.WorkingArea;
        }
        else
        {
            return new System.Drawing.Rectangle(
                (int)w.Left, (int)w.Top,
                (int)w.ActualWidth, (int)w.ActualHeight);
        }
    }

    /// <summary>
    /// 根据激活窗口句柄获得窗体位置
    /// </summary>
    /// <returns></returns>
    public static System.Drawing.Rectangle GetWindowRectHandle()
    {
        IntPtr hForeground = GetForegroundWindow();
        System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
        GetWindowRect(hForeground, ref rect);
        return rect;
    }




    #endregion


}







