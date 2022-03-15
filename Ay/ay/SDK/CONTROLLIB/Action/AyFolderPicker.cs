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
using ay.FuncFactory;

namespace ay.Controls
{



    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    ///生日：2016-10-12 16:01:03
    public class AyFolderPicker : TriggerAction<FrameworkElement>
    {

        /// <summary>
        /// 文件要求的最低所需剩余空间大小，单位byte
        /// </summary>
        public double? MinSpaceUsage
        {
            get { return (double?)GetValue(MinSpaceUsageProperty); }
            set { SetValue(MinSpaceUsageProperty, value); }
        }

        public static readonly DependencyProperty MinSpaceUsageProperty =
            DependencyProperty.Register("MinSpaceUsage", typeof(double?), typeof(AyFolderPicker), new PropertyMetadata(null));


        /// <summary>
        /// 打开时候默认路径
        /// </summary>
        public string DefaultPath
        {
            get { return (string)GetValue(DefaultPathProperty); }
            set { SetValue(DefaultPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultPathProperty =
            DependencyProperty.Register("DefaultPath", typeof(string), typeof(AyFolderPicker), new PropertyMetadata(null));


        /// <summary>
        /// 选择后的所在驱动器剩余可用容量,只读
        /// </summary>
        public double? SelectFolderCapacity
        {
            get { return (double?)GetValue(SelectFolderCapacityProperty); }
            private set { SetValue(SelectFolderCapacityPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey SelectFolderCapacityPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectFolderCapacity", typeof(double?), typeof(AyFolderPicker), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectFolderCapacityProperty = SelectFolderCapacityPropertyKey.DependencyProperty;

        public object ObjectBind { get; set; }
        protected override void OnAttached()
        {
            base.OnAttached();
            ObjectBind = this.AssociatedObject;
        }

        /// <summary>
        /// 选择后的所在驱动器总容量,只读
        /// </summary>
        public double? SelectTotalFolderCapacity
        {
            get { return (double?)GetValue(SelectTotalFolderCapacityProperty); }
            private set { SetValue(SelectTotalFolderCapacityPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey SelectTotalFolderCapacityPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectTotalFolderCapacity", typeof(double?), typeof(AyFolderPicker), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectTotalFolderCapacityProperty = SelectTotalFolderCapacityPropertyKey.DependencyProperty;

        /// <summary>
        /// 支持 textblock，支持textbox，支持label自动会填入所选路径
        /// </summary>
        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(AyFolderPicker), new PropertyMetadata(null));

        public object TotalSizeTarget
        {
            get { return (object)GetValue(TotalSizeTargetProperty); }
            set { SetValue(TotalSizeTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalSizeTargetProperty =
            DependencyProperty.Register("TotalSizeTarget", typeof(object), typeof(AyFolderPicker), new PropertyMetadata(null));

        /// <summary>
        /// 支持 textblock，支持textbox，支持label自动会填入 文件夹剩余空间
        /// </summary>
        public object SizeTarget
        {
            get { return (object)GetValue(SizeTargetProperty); }
            set { SetValue(SizeTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeTargetProperty =
            DependencyProperty.Register("SizeTarget", typeof(object), typeof(AyFolderPicker), new PropertyMetadata(null));


        /// <summary>
        /// 可用空间：{0}
        /// </summary>
        public string SizeStringFormat
        {
            get { return (string)GetValue(SizeStringFormatProperty); }
            set { SetValue(SizeStringFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SizeStringFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeStringFormatProperty =
            DependencyProperty.Register("SizeStringFormat", typeof(string), typeof(AyFolderPicker), new PropertyMetadata("可用空间：{0}"));

        /// <summary>
        /// 总空间：{0}
        /// </summary>
        public string TotalSizeStringFormat
        {
            get { return (string)GetValue(TotalSizeStringFormatProperty); }
            set { SetValue(TotalSizeStringFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SizeStringFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalSizeStringFormatProperty =
            DependencyProperty.Register("TotalSizeStringFormat", typeof(string), typeof(AyFolderPicker), new PropertyMetadata("总空间：{0}"));

        /// <summary>
        /// 标题描述
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(AyFolderPicker), new PropertyMetadata("请选择文件夹"));



        /// <summary>
        /// 成功选择后触发的命令
        /// </summary>
        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCommandProperty =
            DependencyProperty.Register("SelectedCommand", typeof(ICommand), typeof(AyFolderPicker), new PropertyMetadata(null));

        internal string GetSize(double length)
        {
            return AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(length);
        }

        public delegate void Handler(object sender, RoutedEventArgs e);
        /// <summary>
        /// 成功选择后，后台事件
        /// </summary>
        public event Handler Selected;



        protected override void Invoke(object parameter)
        {
            var d = new System.Windows.Forms.FolderBrowserDialog();
            if (DefaultPath.IsNotNull())
            {
                d.SelectedPath = AyFuncIO.Instance.GetDirectory(DefaultPath);
            }
            d.ShowNewFolderButton = true;
            d.Description = Description;
            if (d.ShowDialog() == Winform.DialogResult.OK)
            {
                string dirPath = d.SelectedPath;
                string diskNumber = dirPath[0].ToString();
                double canUsage = AyFuncDisk.Instance.GetHardDiskFreeSpace1(diskNumber);//单位B
                double totalUsage = AyFuncDisk.Instance.GetHardDiskSpace1(diskNumber);//单位B
                if (MinSpaceUsage.HasValue && MinSpaceUsage.Value > canUsage)
                {
                    apErrorToolTip.IsOpen = true;
                    _tb.Text = "磁盘可用空间不足,至少需要" + AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(MinSpaceUsage.Value) + "可用空间";
                    AyTime.setTimeout(3000, () =>
                    {
                        apErrorToolTip.IsOpen = false;
                    });
                    return;
                }
                SelectTotalFolderCapacity = totalUsage;
                SelectFolderCapacity = canUsage;
                var _1 = TotalSizeStringFormat.StringFormat(AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(totalUsage));
                var _2 = SizeStringFormat.StringFormat(AyFuncDisk.Instance.GetFileOrDirectoryFormatedSize(canUsage));

                if (SizeTarget.IsNotNull())
                {
                    var _11 = SizeTarget as TextBox;
                    if (_11 != null)
                    {
                        _11.Text = _2;
                    }
                    else
                    {
                        var _12 = SizeTarget as TextBlock;
                        if (_12.IsNotNull())
                        {
                            _12.Text = _2;
                        }
                        else
                        {

                            var _13 = SizeTarget as Label;
                            if (_13.IsNotNull())
                            {
                                _13.Content = _2;
                            }
                        }
                    }
                }

                if (TotalSizeTarget.IsNotNull())
                {
                    var _11 = TotalSizeTarget as TextBox;
                    if (_11 != null)
                    {
                        _11.Text = _1;
                    }
                    else
                    {
                        var _12 = TotalSizeTarget as TextBlock;
                        if (_12.IsNotNull())
                        {
                            _12.Text = _1;
                        }
                        else
                        {

                            var _13 = TotalSizeTarget as Label;
                            if (_13.IsNotNull())
                            {
                                _13.Content = _1;
                            }
                        }
                    }
                }


                if (Selected != null)
                {
                    Selected(dirPath, new RoutedEventArgs(Button.ClickEvent, this));
                }
                if (SelectedCommand != null)
                {
                    SelectedCommand.Execute(dirPath);
                }
                if (Target.IsNotNull())
                {
                    var _11 = Target as TextBox;
                    if (_11 != null)
                    {
                        _11.Text = dirPath;
                    }
                    else
                    {
                        var _12 = Target as TextBlock;
                        if (_12.IsNotNull())
                        {
                            _12.Text = dirPath;
                        }
                        else
                        {

                            var _13 = Target as Label;
                            if (_13.IsNotNull())
                            {
                                _13.Content = dirPath;
                            }
                        }
                    }
                }
            }

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


