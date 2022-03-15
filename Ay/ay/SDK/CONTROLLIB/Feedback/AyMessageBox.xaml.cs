using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ay.Controls;
using ay.contents;
using ay.Animate;
using static ay.Controls.AyWindow;
using System.Windows.Media.Effects;
using System.Windows.Media;

public partial class AyMessageBox : Window
{

    /// <summary>
    /// 添加一个构造函数
    /// </summary>
    /// <param name="owner">TMMessageBox拥有者</param>
    /// <param name="message"></param>
    /// <param name="details"></param>
    /// <param name="button"></param>
    /// <param name="icon"></param>
    /// <param name="defaultResult"></param>
    /// <param name="options"></param>
    /// <param name="promtcallback">promt回调</param>
    public AyMessageBox(Window owner, string message, string title, MessageBoxButton button, AyMessageBoxImage icon,
                  MessageBoxResult defaultResult, MessageBoxOptions options, Action closeAction, Action<AyFormInput> Init, int _1, Action<string> promtcallback = null)
    {
        InitializeComponent();
        this.Topmost = true;
        this.CloseOverride = closeAction;
        try
        {
            Owner = owner ?? Application.Current.MainWindow;
        }
        catch
        {


        }

        if (promtcallback != null)
        {
            AyFormInput afi = new AyFormInput();
            afi.MinWidth = 160;
            afi.Width = double.NaN;
            afi.Margin = new Thickness(24, 20, 24, 20);
            afi.Rule = "required";
            afi.VerticalAlignment = VerticalAlignment.Center;
            afi.HorizontalAlignment = HorizontalAlignment.Stretch;
            layout.Child = afi;
            if (Init.IsNotNull())
            {
                Init(afi);
            }
            this.MinWidth = 360;
            var okButton = new Button
            {
                Name = "okButton",
                Content = ay.contents.Langs.share_ok.Lang(),
                IsDefault = defaultResult == MessageBoxResult.OK,
                Tag = MessageBoxResult.OK,
                Margin = new Thickness(10, 0, 0, 0)
            };
            okButton.SetResourceReference(Button.StyleProperty, "Button.Primary");
            okButton.Click += (ss, ee) =>
            {
                if (afi.Validate())
                {
                    MessageBoxResult = (MessageBoxResult)(ss as Button).Tag;
                    CloseTMMessageBox();
                    if (afi.IsPasswordBox)
                    {
                        promtcallback(afi.Password);
                    }
                    else
                    {
                        promtcallback(afi.Text);
                    }

                }
                else
                {
                    afi.Focus();
                }
            };
            ButtonsPanel.Children.Add(okButton);

            var cancelButton = new Button
            {
                Name = "cancelButton",
                Content = Langs.share_cancel.Lang(),
                IsDefault = defaultResult == MessageBoxResult.Cancel,
                IsCancel = true,
                Tag = MessageBoxResult.Cancel,
                Margin = new Thickness(6, 0, 4, 0)
            };
            cancelButton.SetResourceReference(Button.StyleProperty, "Button.Default");
            cancelButton.Click += ButtonClick;
            ButtonsPanel.Children.Add(cancelButton);
            ButtonsPanel.Margin = new Thickness(0, 0, 20, 16);
        }
        else
        {
            CreateButtons(button, defaultResult);
            CreateImage(icon);
            MessageText.Text = message;
        }


        if (title.IsEmptyAndNull())
        {
            this.Title = Langs.share_tip.Lang();
        }
        else
        {
            this.Title = title ?? "";
        }


        this.Closed += Ay_Closed;
        this.SourceInitialized += new EventHandler(win_SourceInitialized);
        Showwindow();
        this.Topmost = false;


    }


    //public AyMessageBox(Window owner, List<LinkModel> messages, string title, MessageBoxButton button, AyMessageBoxImage icon,
    //           MessageBoxResult defaultResult, MessageBoxOptions options, Action closeAction, Action<AyFormInput> Init, int _1, Action<string> promtcallback = null)
    //{
    //    InitializeComponent();
    //    this.Topmost = true;
    //    this.CloseOverride = closeAction;
    //    try
    //    {
    //        Owner = owner ?? Application.Current.MainWindow;
    //    }
    //    catch
    //    {


    //    }

    //    if (promtcallback != null)
    //    {
    //        AyFormInput afi = new AyFormInput();
    //        afi.Height = 38;
    //        afi.MinWidth = 160;
    //        afi.Margin = new Thickness(24, 30, 28, 20);
    //        afi.Rule = "required";
    //        afi.VerticalAlignment = VerticalAlignment.Center;
    //        afi.HorizontalAlignment = HorizontalAlignment.Center;
    //        layout.Child = afi;
    //        if (Init.IsNotNull())
    //        {
    //            Init(afi);
    //        }
    //        this.MinWidth = 400;
    //        var okButton = new Button
    //        {
    //            Name = "okButton",
    //            Content = Langs.share_ok.Lang(),
    //            IsDefault = defaultResult == MessageBoxResult.OK,
    //            Tag = MessageBoxResult.OK,
    //            Margin = new Thickness(10, 0, 0, 0)
    //        };
    //        okButton.SetResourceReference(Button.StyleProperty, "Button.Primary");
    //        okButton.Click += (ss, ee) =>
    //        {
    //            if (afi.Validate())
    //            {
    //                MessageBoxResult = (MessageBoxResult)(ss as Button).Tag;
    //                CloseTMMessageBox();
    //                if (afi.IsPasswordBox)
    //                {
    //                    promtcallback(afi.Password);
    //                }
    //                else
    //                {
    //                    promtcallback(afi.Text);
    //                }

    //            }
    //            else
    //            {
    //                afi.Focus();
    //            }
    //        };
    //        ButtonsPanel.Children.Add(okButton);

    //        var cancelButton = new Button
    //        {
    //            Name = "cancelButton",
    //            Content = Langs.share_cancel.Lang(),
    //            IsDefault = defaultResult == MessageBoxResult.Cancel,
    //            IsCancel = true,
    //            Tag = MessageBoxResult.Cancel,
    //            Margin = new Thickness(6, 0, 4, 0)
    //        };
    //        cancelButton.SetResourceReference(Button.StyleProperty, "{ Button.Default");
    //        cancelButton.Click += ButtonClick;
    //        ButtonsPanel.Children.Add(cancelButton);
    //        ButtonsPanel.Margin = new Thickness(0, 0, 20, 16);
    //    }
    //    else
    //    {
    //        CreateButtons(button, defaultResult);
    //        CreateImage(icon);
    //        var _13 = SolidColorBrushConverter.From16JinZhi("#000000");
    //        var _2 = SolidColorBrushConverter.From16JinZhi("#2A72C5");
    //        MessageText.Inlines.Clear();
    //        foreach (var item in messages)
    //        {
    //            if (item.Link.IsNullAndTrimAndEmpty())
    //            {
    //                Run r = new Run();
    //                r.Text = item.Content;
    //                r.Foreground = _13;
    //                MessageText.Inlines.Add(r);
    //            }
    //            else
    //            {
    //                Hyperlink hy = new Hyperlink();
    //                hy.Foreground= _2;
    //                hy.Inlines.Add(item.Content);
    //                hy.NavigateUri = new Uri(item.Link);
    //                hy.RequestNavigate += Hy_RequestNavigate;
    //                MessageText.Inlines.Add(hy);
    //            }
    //        }
    //    }


    //    if (title.IsEmptyAndNull())
    //    {
    //        this.Title = "提示";
    //    }
    //    else
    //    {
    //        this.Title = title ?? "";
    //    }


    //    this.Closed += Ay_Closed;
    //    this.SourceInitialized += new EventHandler(win_SourceInitialized);
    //    Showwindow();
    //    this.Topmost = false;

    //    try
    //    {
    //        SourceHelper.SetImage(this.login_logo, GlobalHelper.AgentLianXiFangShi.clientLoginPageLogo);
    //    }
    //    catch (Exception ex)
    //    {


    //    }

    //}
    private void Ay_Closed(object sender, EventArgs e)
    {
        this.Closed -= Ay_Closed;
        if (Owner == null) return;
        Owner.Focus();
    }
    private void Showwindow()
    {

        var _1 = Owner as AyWindowBase;
        if (_1 != null)
        {
            _1.MaskColorAnimate.Begin();
        }
        var sc = new AyAniScale(body, () =>
        {
            isShowAnimatedOK = true;
            //DropShadowEffect de = new DropShadowEffect();
            ////de.BlurRadius = options.ShadowRadius;
            //body.Effect = de;
            //de.ShadowDepth = 2;
            //de.Opacity = 0.3;
            //de.Color = Colors.Black;
            //AyAniDouble _1 = new AyAniDouble(body);
            //_1.AniPropertyPath = new PropertyPath("(FrameworkElement.Effect).(DropShadowEffect.BlurRadius)");
            //_1.FromDouble = 0;
            //_1.ToDouble = 12;
            //_1.AniEasingMode = 2;
            //_1.AnimateSpeed = 200;
            //_1.Begin();
        });
        sc.AnimateSpeed = 450;
        sc.ScaleXFrom = 0;

        sc.ScaleYFrom = 0;
        sc.ScaleXTo = 1;
        sc.ScaleYTo = 1;
        sc.EasingFunction = new System.Windows.Media.Animation.CubicEase { EasingMode = EasingMode.EaseOut };
        sc.AutoDestory = true;

        sc.Begin();

    }
    /// <summary>
    /// 展示指定图像和文本
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="button"></param>
    /// <param name="iconUrl">图片地址,建议png格式</param>
    /// <param name="defaultResult"></param>
    /// <param name="options"></param>
    public AyMessageBox(Window owner, string message, string title, MessageBoxButton button, string iconUrl,
                MessageBoxResult defaultResult, MessageBoxOptions options)
    {

        InitializeComponent();

        try
        {
            Owner = owner ?? Application.Current.MainWindow;
        }
        catch
        {
        }
        CreateButtons(button, defaultResult);
        if (string.IsNullOrEmpty(iconUrl))
        {
            ImagePlaceholder.Visibility = Visibility.Collapsed;
        }
        else
        {
            ImagePlaceholder.Icon = iconUrl;
        }
        MessageText.Text = message;
        this.Title = title ?? "";
        this.SourceInitialized += new EventHandler(win_SourceInitialized);
        Showwindow();
    }

    //public AyMessageBox(Window owner, List<LinkModel> messages, string title, MessageBoxButton button, string iconUrl, MessageBoxResult defaultResult,  MessageBoxOptions options)
    //{
    //    InitializeComponent();

    //    try
    //    {
    //        Owner = owner ?? Application.Current.MainWindow;
    //    }
    //    catch
    //    {
    //    }
    //    CreateButtons(button, defaultResult);
    //    if (string.IsNullOrEmpty(iconUrl))
    //    {
    //        ImagePlaceholder.Visibility = Visibility.Collapsed;
    //    }
    //    else
    //    {
    //        ImagePlaceholder.Source = new BitmapImage(new Uri(iconUrl, UriKind.RelativeOrAbsolute));
    //    }
    //    var _1= SolidColorBrushConverter.From16JinZhi("#000000"); 
    //    var _2= SolidColorBrushConverter.From16JinZhi("#2A72C5");
    //    MessageText.Inlines.Clear();
    //    foreach (var item in messages)
    //    {
    //        if (item.Link.IsNullAndTrimAndEmpty()) {
    //            Run r = new Run();
    //            r.Text = item.Content;
    //            r.Foreground = _1;
    //            MessageText.Inlines.Add(r);
    //        }
    //        else
    //        {

    //            Hyperlink hy = new Hyperlink();
    //            hy.Foreground = _2;
    //            hy.Inlines.Add(item.Content);
    //            hy.NavigateUri = new Uri(item.Link);
    //            hy.RequestNavigate += Hy_RequestNavigate;
    //            MessageText.Inlines.Add(hy);
    //        }
    //    }
    //    this.Title = title ?? "";
    //    this.SourceInitialized += new EventHandler(win_SourceInitialized);
    //    Showwindow();
    //}


    private void Hy_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        e.Handled = true;
    }

    public MessageBoxResult MessageBoxResult { get; set; }

    private void CreateButtons(MessageBoxButton button, MessageBoxResult defaultResult)
    {
        switch (button)
        {
            case MessageBoxButton.OK:
                ButtonsPanel.Children.Add(CreateOkButton(defaultResult));
                break;
            case MessageBoxButton.OKCancel:
                ButtonsPanel.Children.Add(CreateOkButton(defaultResult));
                ButtonsPanel.Children.Add(CreateCancelButton(defaultResult));
                break;
            case MessageBoxButton.YesNoCancel:
                ButtonsPanel.Children.Add(CreateYesButton(defaultResult));
                ButtonsPanel.Children.Add(CreateNoButton(defaultResult));
                ButtonsPanel.Children.Add(CreateCancelButton(defaultResult));
                break;
            case MessageBoxButton.YesNo:
                ButtonsPanel.Children.Add(CreateYesButton(defaultResult));
                ButtonsPanel.Children.Add(CreateNoButton(defaultResult));
                break;
            default:
                throw new ArgumentOutOfRangeException("button");
        }
    }


    private Button CreateOkButton(MessageBoxResult defaultResult)
    {
        var okButton = new Button
        {
            Name = "okButton",
            Content = Langs.share_ok.Lang(),
            IsDefault = defaultResult == MessageBoxResult.OK,
            Tag = MessageBoxResult.OK,
            Margin = new Thickness(10, 0, 0, 0)
        };
        okButton.SetResourceReference(Button.StyleProperty, "Button.Primary");
        okButton.Click += ButtonClick;

        return okButton;
    }

    private Button CreateCancelButton(MessageBoxResult defaultResult)
    {
        var cancelButton = new Button
        {
            Name = "cancelButton",
            Content = Langs.share_cancel.Lang(),
            IsDefault = defaultResult == MessageBoxResult.Cancel,
            IsCancel = true,
            Tag = MessageBoxResult.Cancel,
            Margin = new Thickness(10, 0, 0, 0)
        };
        cancelButton.SetResourceReference(Button.StyleProperty, "Button.Default");
        cancelButton.Click += ButtonClick;

        return cancelButton;
    }


    private Button CreateYesButton(MessageBoxResult defaultResult)
    {
        var yesButton = new Button
        {
            Name = "yesButton",
            Content = Langs.share_yes.Lang(),
            IsDefault = defaultResult == MessageBoxResult.Yes,
            Tag = MessageBoxResult.Yes,
            Margin = new Thickness(10, 0, 0, 0)
        };
        yesButton.SetResourceReference(Button.StyleProperty, "Button.Primary");
        yesButton.Click += ButtonClick;

        return yesButton;
    }


    private Button CreateNoButton(MessageBoxResult defaultResult)
    {
        var noButton = new Button
        {
            Name = "noButton",
            Content = Langs.share_no.Lang(),
            IsDefault = defaultResult == MessageBoxResult.No,
            Tag = MessageBoxResult.No,
            Margin = new Thickness(10, 0, 0, 0)
        };
        noButton.SetResourceReference(Button.StyleProperty, "Button.Default");
        noButton.Click += ButtonClick;

        return noButton;
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = (MessageBoxResult)(sender as Button).Tag;
        CloseTMMessageBox();
    }

    public Action CloseOverride { get; set; }
    bool isShowAnimatedOK = false;


    private void CloseTMMessageBox()
    {
        if (isShowAnimatedOK)
        {
            var _12 = Owner as AyWindowBase;
            if (_12 != null)
            {
                _12.ResetMaskColor();
            }
            if (CloseOverride.IsNotNull()) CloseOverride();
            var bn = new AyAniZoomBounceOut(body, () =>
            {
 
                this.Close();
            });
            bn.AutoDestory = true;
            bn.Begin();
            //AyAniDouble _1 = new AyAniDouble(body);
            //_1.AniPropertyPath = new PropertyPath("(FrameworkElement.Effect).(DropShadowEffect.BlurRadius)");
            //_1.FromDouble = 12;
            //_1.ToDouble = 0;
            //_1.AniEasingMode = 2;
            //_1.AnimateSpeed = 200;
            //_1.AutoDestory = true;
            //_1.Begin();
         

        }
    }

    /// <summary>
    /// Create the image from the system's icons
    /// </summary>
    /// <param name="icon"></param>
    private void CreateImage(AyMessageBoxImage icon)
    {
        switch (icon)
        {
            case AyMessageBoxImage.None:
                ImagePlaceholder.Visibility = Visibility.Collapsed;
                break;
            case AyMessageBoxImage.Information:
                ImagePlaceholder.Icon = "path_ay_msg_info";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#909399");
                break;
            case AyMessageBoxImage.Question:
                ImagePlaceholder.Icon = "path_ay_msg_question";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#409EFF");
                break;
            case AyMessageBoxImage.Warning:
                ImagePlaceholder.Icon = "path_ay_msg_warning";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#E6A23C");
                break;
            case AyMessageBoxImage.Error:
                ImagePlaceholder.Icon = "path_ay_msg_error";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#F56C6C");
                break;
            case AyMessageBoxImage.Delete:
                ImagePlaceholder.Icon = "path_ay_msg_delete";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#F56C6C");
                break;
            case AyMessageBoxImage.Right:
                ImagePlaceholder.Icon = "path_ay_msg_right";
                ImagePlaceholder.Foreground = HexToBrush.FromHex("#67C473");

                break;
            default:
                throw new ArgumentOutOfRangeException("icon");
        }
    }



    #region  Delete 删除提示

    public static MessageBoxResult ShowDelete(string message, string title = "",
                                                     MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowDelete(null, message, title, options);
    }


    public static MessageBoxResult ShowDelete(Window owner, string message, string title = "",
                                                MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, MessageBoxButton.OKCancel,
                    AyMessageBoxImage.Delete, MessageBoxResult.OK, options);
    }


    #endregion

    #region  Right

    public static MessageBoxResult ShowRight(string message, string title = "", bool showCancel = false,
                                                 MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowRight(null, message, title, showCancel, options);
    }


    public static MessageBoxResult ShowRight(Window owner, string message, string title = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                    AyMessageBoxImage.Right, MessageBoxResult.OK, options);
    }


    #endregion

    #region Show Information

    public static MessageBoxResult ShowInformation(string message, string title = "", bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowInformation(null, message, title, showCancel, options);
    }


    public static MessageBoxResult ShowInformation(Window owner, string message, string title = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                    AyMessageBoxImage.Information, MessageBoxResult.OK, options);
    }

    #endregion

    #region Show Question


    public static MessageBoxResult ShowQuestion(string message, string title = "",
                                                bool showCancel = false,
                                                MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowQuestion(null, message, title, showCancel, options);
    }


    public static MessageBoxResult ShowQuestion(Window owner, string message, string title = "",
                                                bool showCancel = false,
                                                MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                    AyMessageBoxImage.Question, MessageBoxResult.Yes, options);
    }

    public static MessageBoxResult ShowQuestion(Window owner, string message, Action closeAction, string title = "",
                                           bool showCancel = false,
                                           MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, closeAction, title, showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                    AyMessageBoxImage.Question, MessageBoxResult.Yes, options);
    }

    public static MessageBoxResult ShowQuestionOkCancel(string message, string title = "",
                                          MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowQuestionOkCancel(null, message, title, options);
    }

    public static MessageBoxResult ShowQuestionOkCancel(Window owner, string message, string title = "",
                                           MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, MessageBoxButton.OKCancel,
                    AyMessageBoxImage.Question, MessageBoxResult.OK, options);
    }



    #endregion

    #region Show Warning


    public static MessageBoxResult ShowWarning(string message, string title = "",
                                               bool showCancel = false,
                                               MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowWarning(null, message, title, showCancel, options);
    }


    public static MessageBoxResult ShowWarning(Window owner, string message, string title = "",
                                               bool showCancel = false,
                                               MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                    AyMessageBoxImage.Warning, MessageBoxResult.OK, options);
    }

    #endregion

    #region Show Error

    public static MessageBoxResult ShowError(string message, string title = "",
                                                     bool showCancel = false,
                                                     MessageBoxOptions options = MessageBoxOptions.None)
    {
        return ShowError(null, message, title, showCancel, options);
    }


    public static MessageBoxResult ShowError(Window owner, string message, string title = "",
                                               bool showCancel = false,
                                               MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, title, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                    AyMessageBoxImage.Error, MessageBoxResult.OK, options);
    }


    #endregion

    #region Show
    public static MessageBoxResult Show(string message)
    {
        return Show(null, message, null, MessageBoxButton.OK, AyMessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.RightAlign);
    }
    //public static MessageBoxResult Show(List<LinkModel> messages)
    //{
    //    return Show(null, messages,null, null, MessageBoxButton.OK, AyMessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.RightAlign);
    //}

    //public static MessageBoxResult Show(List<LinkModel> messages, string title)
    //{
    //    return Show(null, messages, null, title, MessageBoxButton.OK, AyMessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.RightAlign);
    //}

    //public static MessageBoxResult Show(Window owner, List<LinkModel> messages, Action closeAction, string title = "",
    //                                 MessageBoxButton button = MessageBoxButton.OK,
    //                                 AyMessageBoxImage icon = AyMessageBoxImage.None,
    //                                 MessageBoxResult defaultResult = MessageBoxResult.None,
    //                                 MessageBoxOptions options = MessageBoxOptions.None)
    //{
    //    var result = Application.Current.Dispatcher.Invoke(new Func<MessageBoxResult>(() =>
    //    {
    //        var messageBox = new AyMessageBox(owner, messages, title, button, icon, defaultResult, options, closeAction, null, 1);
    //        messageBox.ShowDialog();

    //        return messageBox.MessageBoxResult;
    //    }));

    //    return (MessageBoxResult)result;
    //}





    public static MessageBoxResult Show(string message, string title)
    {
        return Show(null, message, title, MessageBoxButton.OK, AyMessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.RightAlign);
    }

    public static MessageBoxResult Show(string message, string title = "",
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        AyMessageBoxImage icon = AyMessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None,
                                        MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(null, message, title, button, icon, defaultResult, options);
    }
    public static MessageBoxResult Show(string message,
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        AyMessageBoxImage icon = AyMessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None,
                                        MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(message, string.Empty, button, icon, defaultResult, options);
    }

    public static MessageBoxResult Show(Window owner, string message,
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        AyMessageBoxImage icon = AyMessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None,
                                        MessageBoxOptions options = MessageBoxOptions.None)
    {
        return Show(owner, message, string.Empty, button, icon, defaultResult, options);
    }
    public static MessageBoxResult Show(Window owner, string message, Action closeAction, string title = "",
                                     MessageBoxButton button = MessageBoxButton.OK,
                                     AyMessageBoxImage icon = AyMessageBoxImage.None,
                                     MessageBoxResult defaultResult = MessageBoxResult.None,
                                     MessageBoxOptions options = MessageBoxOptions.None)
    {
        var result = Application.Current.Dispatcher.Invoke(new Func<MessageBoxResult>(() =>
        {
            var messageBox = new AyMessageBox(owner, message, title, button, icon, defaultResult, options, closeAction, null, 1);
            messageBox.ShowDialog();

            return messageBox.MessageBoxResult;
        }));

        return (MessageBoxResult)result;
    }

    public static MessageBoxResult Show(Window owner, string message, string title = "",
                                        MessageBoxButton button = MessageBoxButton.OK,
                                        AyMessageBoxImage icon = AyMessageBoxImage.None,
                                        MessageBoxResult defaultResult = MessageBoxResult.None,
                                        MessageBoxOptions options = MessageBoxOptions.None)
    {
        var result = Application.Current.Dispatcher.Invoke(new Func<MessageBoxResult>(() =>
        {
            var messageBox = new AyMessageBox(owner, message, title, button, icon, defaultResult, options, null, null, 1);
            messageBox.ShowDialog();

            return messageBox.MessageBoxResult;
        }));

        return (MessageBoxResult)result;
    }

    internal static MessageBoxResult Promt(Window owner, Action<AyFormInput> init, Action<string> callBack, string title = "",
                                MessageBoxButton button = MessageBoxButton.OKCancel,
                                AyMessageBoxImage icon = AyMessageBoxImage.None,
                                MessageBoxResult defaultResult = MessageBoxResult.None,
                                MessageBoxOptions options = MessageBoxOptions.None)
    {
        try
        {
            var result = Application.Current.Dispatcher.Invoke(new Func<MessageBoxResult>(() =>
            {
                string a = null;
                var messageBox = new AyMessageBox(owner, a, title, button, icon, defaultResult, options, null, init, 1, callBack);
                messageBox.ShowDialog();
                return messageBox.MessageBoxResult;
            }));
            return (MessageBoxResult)result;
        }
        catch
        {

            return MessageBoxResult.None;
        }



    }

    /// <summary>
    /// 指定图标的自定义弹出消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="iconUri">图标地址,建议png格式</param>
    /// <param name="title"></param>
    /// <param name="button"></param>
    /// <param name="defaultResult"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static MessageBoxResult ShowCus(string message, string title = "", string iconUri = "",
                                MessageBoxButton button = MessageBoxButton.OK,
                                MessageBoxResult defaultResult = MessageBoxResult.None
                                )
    {
        return ShowCus(null, message, title, iconUri, button, defaultResult);
    }

    public static MessageBoxResult ShowCus(Window owner, string message, string title = "", string iconUri = "",
                                  MessageBoxButton button = MessageBoxButton.OK,
                                  MessageBoxResult defaultResult = MessageBoxResult.None)
    {
        var result = Application.Current.Dispatcher.Invoke(new Func<MessageBoxResult>(() =>
        {
            var messageBox = new AyMessageBox(owner, message, title, button, iconUri, defaultResult, MessageBoxOptions.None);

            messageBox.ShowDialog();

            return messageBox.MessageBoxResult;
        }));

        return (MessageBoxResult)result;
    }

    private void closewindow_Click(object sender, RoutedEventArgs e)
    {
        CloseTMMessageBox();
    }

    #region Promt
    public static MessageBoxResult Promt(Action<string> callBack, string title = "")
    {
        return Promt(null, null, callBack, title, MessageBoxButton.OKCancel, AyMessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }
    public static MessageBoxResult Promt(Action<AyFormInput> init, Action<string> callBack, string title = "")
    {
        return Promt(null, init, callBack, title, MessageBoxButton.OKCancel, AyMessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }

    public static MessageBoxResult Promt(Window owner, Action<AyFormInput> init, Action<string> callBack, string title = "")
    {
        return Promt(owner, init, callBack, title, MessageBoxButton.OKCancel, AyMessageBoxImage.None, MessageBoxResult.OK, MessageBoxOptions.None);
    }
    #endregion

    //layout
    #endregion

    //private void AyPopupWindow_Loaded(object sender, RoutedEventArgs e)
    //{
    //    //var hei=layout.ActualHeight + 76;
    //    //this.Height = hei;

    //}



    #region Avoid hiding task bar upon maximalisation
    [DllImport("user32")]
    internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("User32")]
    internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

    private static System.IntPtr WindowProc(
          System.IntPtr hwnd,
          int msg,
          System.IntPtr wParam,
          System.IntPtr lParam,
          ref bool handled)
    {
        switch (msg)
        {
            case 0x0024:
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
                break;
        }

        return (System.IntPtr)0;
    }

    void win_SourceInitialized(object sender, EventArgs e)
    {
        System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
        WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
    }

    private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
    {

        MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

        // Adjust the maximized size and position to fit the work area of the correct monitor
        int MONITOR_DEFAULTTONEAREST = 0x00000002;
        System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

        if (monitor != System.IntPtr.Zero)
        {

            MONITORINFO monitorInfo = new MONITORINFO();
            GetMonitorInfo(monitor, monitorInfo);
            RECT rcWorkArea = monitorInfo.rcWork;
            RECT rcMonitorArea = monitorInfo.rcMonitor;
            mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
            mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
            mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
            mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
        }

        Marshal.StructureToPtr(mmi, lParam, true);
    }
    #endregion
}

