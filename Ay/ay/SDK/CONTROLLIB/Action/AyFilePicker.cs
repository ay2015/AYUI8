using Ay.Framework.WPF.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Winform = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using i = System.Windows.Interactivity;
using ay.contentcore;

namespace ay.Controls
{
    public class AyFilePickerExt
    {
        public static string GetFIleNameList(string[] filePaths,bool IsLongPath)
        {
            StringBuilder b = new StringBuilder();
            foreach (var item in filePaths)
            {
                if (IsLongPath)
                {
                    b.Append(item + ",");
                }
                else
                {
                    b.Append(System.IO.Path.GetFileName(item) + ",");
                }
            }

            return b.ToString().TrimEnd(',');
        }
    }

    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    public class AyFilePicker : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// 文件大小限制
        /// </summary>
        public double MaxFileLength
        {
            get { return (double)GetValue(MaxFileLengthProperty); }
            set { SetValue(MaxFileLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxFileLengthProperty =
            DependencyProperty.Register("MaxFileLength", typeof(double), typeof(AyFilePicker), new PropertyMetadata(-1.00));

        public static Dictionary<string, string> FileExtensionMappers = new Dictionary<string, string>
        {
            {"image","Jpeg 文件 (*.jpg)|*.jpg|位图文件 (*.bmp)|*.bmp|Png 文件 (*.png)|*.png|所有 (*.bmp/*.jpg/*.png)|*.bmp;*.jpg;*.png"},
            {"audio","wav文件 (*.wav)|*.wav|aac文件 (*.aac)|*.aac|mp3文件 (*.mp3)|*.mp3|所有 (*.wav/*.aac/*.mp3)|*.wav;*.aac;*.mp3" }
        };
        /// <summary>
        /// 当前文件数量，建议跟某个变量绑定
        /// </summary>
        public int CurrentFileCount
        {
            get { return (int)GetValue(CurrentFileCountProperty); }
            set { SetValue(CurrentFileCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentFileCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentFileCountProperty =
            DependencyProperty.Register("CurrentFileCount", typeof(int), typeof(AyFilePicker), new PropertyMetadata(0));


        public object ObjectBind { get; set; }
        protected override void OnAttached()
        {
            base.OnAttached();
            ObjectBind = this.AssociatedObject;
        }
        /// <summary>
        /// 文件选择数量限制
        /// </summary>
        public int MaxFileCount
        {
            get { return (int)GetValue(MaxFileCountProperty); }
            set { SetValue(MaxFileCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxFileCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxFileCountProperty =
            DependencyProperty.Register("MaxFileCount", typeof(int), typeof(AyFilePicker), new PropertyMetadata(-1));



        /// <summary>
        /// 文件后缀限制
        /// 内置：image,audio,其他的单独自己写，默认所有文件
        /// </summary>
        public string FileExtension
        {
            get { return (string)GetValue(FileExtensionProperty); }
            set { SetValue(FileExtensionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileExtension.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileExtensionProperty =
            DependencyProperty.Register("FileExtension", typeof(string), typeof(AyFilePicker), new PropertyMetadata("所有文件 (*.*)|*.*"));



        public delegate void Handler(object sender, RoutedEventArgs e);
        public event Handler Selected=null;

        /// <summary>
        /// 是否多选文件
        /// </summary>
        public bool IsMultiply
        {
            get { return (bool)GetValue(IsMultiplyProperty); }
            set { SetValue(IsMultiplyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMultiply.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMultiplyProperty =
            DependencyProperty.Register("IsMultiply", typeof(bool), typeof(AyFilePicker), new PropertyMetadata(false));



        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(AyFilePicker), new PropertyMetadata(null));



        /// <summary>
        /// 路径模式
        /// </summary>
        public bool IsLongPath
        {
            get { return (bool)GetValue(IsLongPathProperty); }
            set { SetValue(IsLongPathProperty, value); }
        }

        public static readonly DependencyProperty IsLongPathProperty =
            DependencyProperty.Register("IsLongPath", typeof(bool), typeof(AyFilePicker), new PropertyMetadata(false));


        public string DefaultFolderPath
        {
            get { return (string)GetValue(DefaultFolderPathProperty); }
            set { SetValue(DefaultFolderPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultFolderPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultFolderPathProperty =
            DependencyProperty.Register("DefaultFolderPath", typeof(string), typeof(AyFilePicker), new PropertyMetadata(null));



        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCommandProperty =
            DependencyProperty.Register("SelectedCommand", typeof(ICommand), typeof(AyFilePicker), new PropertyMetadata(null));

        internal string GetSize(double length)
        {
            return AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(length);
        }



        protected override void Invoke(object parameter)
        {
            if (MaxFileCount > 0 && CurrentFileCount >= MaxFileCount)
            {
                apErrorToolTip.IsOpen = true;
                _tb.Text = "最多只能选择" + MaxFileCount + "个文件";
                AyTime.setTimeout(3000, () =>
                    {
                        apErrorToolTip.IsOpen = false;
                    });
                return;
            }
            var d = new System.Windows.Forms.OpenFileDialog();
            if (DefaultFolderPath.IsNotNull())
            {
                d.InitialDirectory = DefaultFolderPath;
            }
            d.Multiselect = IsMultiply;

            if (FileExtensionMappers.ContainsKey(FileExtension))
            {
                d.Filter = FileExtensionMappers[FileExtension];
            }
            else
            {
                d.Filter = FileExtension;
            }



            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (IsMultiply == false)
                {
                    string filePath = d.FileName;
                    FileInfo fi = new FileInfo(filePath);
                    if (MaxFileLength > 0 && fi.Length > MaxFileLength)
                    {
                        AyMessageBox.ShowWarning("你选择的文件过大,目前仅支持小于" + GetSize(MaxFileLength) + "的文件", "文件选择错误");
                        return;
                    }
                    if (Selected != null)
                    {
                        Selected(filePath, new RoutedEventArgs(Button.ClickEvent,this) );
                    }
                    if (SelectedCommand != null)
                    {
                        SelectedCommand.Execute(filePath);
                    }
                    if (Target.IsNotNull())
                    {
                        var _11 = Target as TextBox;
                        if (_11 != null)
                        {
                            if (IsLongPath)
                            {
                                _11.Text = filePath;
                            }
                            else
                            {
                                _11.Text = System.IO.Path.GetFileName(filePath);
                            }

                        }
                        else
                        {
                            var _12 = Target as TextBlock;
                            if (_12.IsNotNull())
                            {
                                if (IsLongPath)
                                {
                                    _12.Text = filePath;
                                }
                                else
                                {
                                    _12.Text = System.IO.Path.GetFileName(filePath);
                                }
                            }
                            else
                            {
                                var _13 = Target as Label;
                                if (_13.IsNotNull())
                                {
                                    if (IsLongPath)
                                    {
                                        _13.Content = filePath;
                                    }
                                    else
                                    {
                                        _13.Content = System.IO.Path.GetFileName(filePath);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string[] filePaths = d.FileNames;
                    bool hasNo = false;
                    if (MaxFileLength > 0)
                    {
                        foreach (var filePath in filePaths)
                        {
                            FileInfo fi = new FileInfo(filePath);
                            if (fi.Length > MaxFileLength)
                            {
                                AyMessageBox.ShowWarning("你选择的文件中有文件过大,目前仅支持小于" + GetSize(MaxFileLength) + "的文件。\r\n" + fi.Name + "(" + GetSize(fi.Length) + ")", "文件选择错误");
                                hasNo = true;
                                break;
                            }
                        }
                    }
                    if (hasNo)
                    {
                        return;
                    }

                    if (Selected != null)
                    {
                        Selected(filePaths, new RoutedEventArgs(Button.ClickEvent, this));
                    }
                    if (SelectedCommand != null)
                    {
                        SelectedCommand.Execute(filePaths);
                    }
                    if (Target.IsNotNull())
                    {
                        var _11 = Target as TextBox;
                        if (_11 != null)
                        {
                            _11.Text = GetFIleNameList(filePaths);
                        }
                        else
                        {
                            var _12 = Target as TextBlock;
                            if (_12.IsNotNull())
                            {
                                _12.Text = GetFIleNameList(filePaths);
                            }
                            else
                            {

                                var _13 = Target as Label;
                                if (_13.IsNotNull())
                                {
                                    _13.Content = GetFIleNameList(filePaths);
                                }
                            }
                        }
                    }
                }
            }
            //arl.FileLength = fi.Length;
            //arl.FilePath = filePath;
            //arl.FileName = System.IO.Path.GetFileName(filePath);
            //arl.Duration = AyExtension.GetGuidNoSplit;


        }

        public string GetFIleNameList(string[] filePaths)
        {
            StringBuilder b = new StringBuilder();
            foreach (var item in filePaths)
            {
                if (IsLongPath)
                {
                    b.Append(item + ",");
                }
                else
                {
                    b.Append(System.IO.Path.GetFileName(item) + ",");
                }
            }

            return b.ToString().TrimEnd(',');
        }




        #region 错误提示
        ToolTip _apErrorToolTip = null;
        public ToolTip apErrorToolTip
        {
            get
            {
                if (_apErrorToolTip.IsNull())
                {
                    CreatePopupEx();
                }
                return _apErrorToolTip;
            }
            set { _apErrorToolTip = value; }
        }


        AyTooltip at = null;

        TextBlock _tb = null;
        private void CreatePopupEx()
        {
            if (_apErrorToolTip.IsNull())
            {
                _apErrorToolTip = new ToolTip();
                _apErrorToolTip.BorderThickness = new Thickness(0);
                _apErrorToolTip.Background = new SolidColorBrush(Colors.Transparent);
                _apErrorToolTip.Padding = new Thickness(0);
                _apErrorToolTip.Placement = PlacementMode.Top;
                _apErrorToolTip.Padding = new Thickness(0, 0, 0, 10);
                _apErrorToolTip.HorizontalOffset = 0;
                _apErrorToolTip.VerticalOffset = 0;
                _apErrorToolTip.Opened += popup_Opened;
                _apErrorToolTip.PlacementTarget = this.AssociatedObject;
                _apErrorToolTip.VerticalContentAlignment = VerticalAlignment.Center;

                if (at.IsNull())
                {
                    at = new AyTooltip();
                    at.BorderBrush = SolidColorBrushConverter.From16JinZhi("#B2C9DE");
                    at.Foreground = SolidColorBrushConverter.From16JinZhi("#FF3737");
                    at.Background = SolidColorBrushConverter.From16JinZhi("#F3FAFD");
                    at.Placement = Dock.Bottom;
                    _tb = new TextBlock();
                    _tb.TextWrapping = TextWrapping.Wrap;
                    _tb.FontSize = 12;
                    _tb.Foreground = at.Foreground;

                    at.TooltipContent = _tb;
                    _apErrorToolTip.Content = at;


                }
            }
        }


        void popup_Opened(object sender, EventArgs e)

        {
            var p = sender as ToolTip;
            if (p != null)
            {
                UpdateToolTipStyle();
            }
        }

        public void UpdateToolTipStyle()
        {
            Point relativeLocation = at.TranslatePoint(new Point(0, 0), this.AssociatedObject);

            if (relativeLocation.Y > 0)
            {
                at.Placement = Dock.Top;
                apErrorToolTip.Padding = new Thickness(0, 10, 0, 0);
            }
            else if (relativeLocation.Y < 0)
            {
                at.Placement = Dock.Bottom;
                apErrorToolTip.Padding = new Thickness(0, 0, 0, 10);
            }
        }

        #endregion
    }


}


