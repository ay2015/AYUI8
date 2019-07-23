using ay.contentcore.Mgr;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ay.contentcore
{
    /// <summary>
    /// 目标:支持各种图像展示的一个控件
    /// </summary>
    public partial class AyIconAll : ContentControl, IIconSupport
    {
        public AyIconAll()
        {
            Loaded += Canvas_Loaded;
            this.Focusable = false;
            this.FocusVisualStyle = null;
        }
        public bool IsBitmapImage
        {
            get { return (bool)GetValue(IsBitmapImageProperty); }
            set { SetValue(IsBitmapImageProperty, value); }
        }

        public static readonly DependencyProperty IsBitmapImageProperty =
            DependencyProperty.Register("IsBitmapImage", typeof(bool), typeof(AyIconAll), new PropertyMetadata(false));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

    
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AyIconAll), new PropertyMetadata(Stretch.Uniform));


        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Canvas_Loaded;
            LoadIcon();
        }

        internal AyIconAllType IconType { get; set; }



        /// <summary>
        /// 控制path或者fontawesome的Stroke
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(AyIconAll), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));


        /// <summary>
        /// 当多组path时候，有的path没有fill，这里设置默认 2018-5-29 13:48:37
        /// </summary>
        public Brush EFil
        {
            get { return (Brush)GetValue(EFilProperty); }
            set { SetValue(EFilProperty, value); }
        }
        public static readonly DependencyProperty EFilProperty =
            DependencyProperty.Register("EFil", typeof(Brush), typeof(AyIconAll), new PropertyMetadata(null));

        /// <summary>
        /// 是否描边字体
        /// </summary>
        public bool IsFontStrokeLabel
        {
            get { return (bool)GetValue(IsFontStrokeLabelProperty); }
            set { SetValue(IsFontStrokeLabelProperty, value); }
        }

        public static readonly DependencyProperty IsFontStrokeLabelProperty =
            DependencyProperty.Register("IsFontStrokeLabel", typeof(bool), typeof(AyIconAll), new PropertyMetadata(false));



        /// <summary>
        /// 控制path或者fontawesome的StrokeThickness
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(AyIconAll), new PropertyMetadata(0.00));

        AyStrokeLabel tb;
        TextBlock tblock;
        AyPath btn1;
        AyGifControl imageGif;
        public Viewbox GetMoreIcon()
        {
            var xc = (from c in PathIcon.Instance.xmlDoc.Descendants(Icon)
                      select c).FirstOrDefault();
            if (xc != null)
            {
                //拆分path
                string nv = $"<ay>{xc.Value}</ay>";
                var xd = XDocument.Parse(nv);
                var results = from c in xd.Descendants("path")
                              select c;
                Viewbox vb = new Viewbox();
                Binding bindingWidth = new Binding { Path = new PropertyPath("Width"), Source = this, Mode = BindingMode.TwoWay, TargetNullValue = 16.00 };
                vb.SetBinding(Viewbox.WidthProperty, bindingWidth);
                Binding bindingHeight = new Binding { Path = new PropertyPath("Height"), Source = this, Mode = BindingMode.TwoWay, TargetNullValue = 16.00 };
                vb.SetBinding(Viewbox.HeightProperty, bindingHeight);
                Grid g = new Grid();
                vb.Child = g;
                Brush defaultFil = null;

                var _hasbg = xc.Attribute("main");
                if (_hasbg != null && _hasbg.Value == "1")
                {
                    Foreground = null;
                }
                if (Foreground == null)
                {
                    var _hasFill = xc.Attribute("fil");
                    if (_hasFill != null && _hasFill.Value != "")
                    {
                        defaultFil = HexToBrush.FromHex(_hasFill.Value);
                    }

                    foreach (var result in results)
                    {
                        if (!string.IsNullOrEmpty(result.Attribute("d").Value))
                        {
                            Path p = new Path();
                            p.Data = PathGeometry.Parse(result.Attribute("d").Value);
                            if (defaultFil != null)
                            {
                                p.Fill = defaultFil;
                            }
                            else
                            {
                                var yanse = result.Attribute("fill");
                                if (yanse != null && yanse.Value != "")
                                {
                                    p.Fill = HexToBrush.FromHex(yanse.Value);
                                }
                                else
                                {

                                    var _hasEFill = xc.Attribute("efil");
                                    if (_hasEFill != null && _hasEFill.Value != "")
                                    {
                                        p.Fill = HexToBrush.FromHex(_hasEFill.Value);
                                    }
                                    else
                                    {
                                        if (EFil != null)
                                        {
                                            Binding bindingEfill = new Binding { Path = new PropertyPath("EFil"), Source = this, Mode = BindingMode.TwoWay };
                                            p.SetBinding(Path.FillProperty, bindingEfill);
                                        }
                                    }


                                }
                            }
                            g.Children.Add(p);
                        }
                    }
                }
                else
                {
                    foreach (var result in results)
                    {
                        if (!string.IsNullOrEmpty(result.Attribute("d").Value))
                        {
                            Path p = new Path();
                            p.Data = PathGeometry.Parse(result.Attribute("d").Value);
                            Binding bindingFill = new Binding { Path = new PropertyPath("Foreground"), Source = this, Mode = BindingMode.TwoWay };
                            p.SetBinding(Path.FillProperty, bindingFill);
                            Binding bindingStroke = new Binding { Path = new PropertyPath("Stroke"), Source = this, Mode = BindingMode.TwoWay };
                            p.SetBinding(Path.StrokeProperty, bindingStroke);

                            Binding bindingStrokeThickness = new Binding { Path = new PropertyPath("StrokeThickness"), Source = this, Mode = BindingMode.TwoWay };
                            p.SetBinding(Path.StrokeThicknessProperty, bindingStrokeThickness);

                            g.Children.Add(p);
                        }
                    }
                }
                Binding bindingStretch = new Binding { Path = new PropertyPath("Stretch"), Source = this, Mode = BindingMode.TwoWay };
                vb.SetBinding(Viewbox.StretchProperty, bindingStretch);
                return vb;
            }
            return null;
        }
        public void LoadIcon()
        {
            //判断Icon方式
            if (!string.IsNullOrEmpty(Icon))
            {
                string ext = System.IO.Path.GetExtension(Icon);
                if (ext == "")
                {
                    //<control:AyStrokeLabel Text="&#xf007;" Style="{StaticResource FontAwesome}" Fill="Yellow" Stroke="Black" StrokeThickness="0.3" FontWeight="Bold" FontSize="50"/>
                    if (Icon.IndexOf("fa_") == 0 || Icon.IndexOf("fa-") == 0)
                    {
                        if (IsFontStrokeLabel)
                        {
                            tb = new AyStrokeLabel
                            {
                                VerticalAlignment = VerticalAlignment.Center,
                                Stroke = Stroke,
                                StrokeThickness = StrokeThickness,
                                FontWeight = FontWeight,
                                FontStretch = FontStretch,
                                FontStyle = FontStyle,
                                UseLayoutRounding = true
                            };
                            Binding bindingFont = new Binding { Path = new PropertyPath("FontSize"), Source = this, Mode = BindingMode.TwoWay };
                            tblock.SetBinding(TextBlock.FontSizeProperty, bindingFont);
                            //设置绑定
                            Binding binding = new Binding { Path = new PropertyPath("Foreground"), Source = this, Mode = BindingMode.TwoWay };
                            tb.SetBinding(AyStrokeLabel.FillProperty, binding);

                            tb.Style = Application.Current.TryFindResource("FontAwesome") as Style;
                            Icon = Icon.Replace(" ", "").Replace("-", "_");
                            tb.Text = FontAweSomeHelper.GetUnicode(Icon);
                            //tb.Text = "&#xf1b9;";
                            this.Content = tb;
                        }
                        else
                        {
                            tblock = new TextBlock
                            {
                                VerticalAlignment = VerticalAlignment.Center,
                                FontWeight = FontWeight,
                                FontStretch = FontStretch,
                                FontStyle = FontStyle,
                                UseLayoutRounding = true
                            };
                            Binding bindingFont = new Binding { Path = new PropertyPath("FontSize"), Source = this, Mode = BindingMode.TwoWay };
                            tblock.SetBinding(TextBlock.FontSizeProperty, bindingFont);
                            //设置绑定
                            Binding binding = new Binding { Path = new PropertyPath("Foreground"), Source = this, Mode = BindingMode.TwoWay };
                            tblock.SetBinding(TextBlock.ForegroundProperty, binding);

                            tblock.Style = Application.Current.TryFindResource("FontAwesome") as Style;
                            Icon = Icon.Replace(" ", "").Replace("-", "_");
                            tblock.Text = FontAweSomeHelper.GetUnicode(Icon);
                            this.Content = tblock;
                        }

                        IconType = AyIconAllType.Font;
                    }
                    else if (Icon.IndexOf("path_") == 0)
                    {

                        //2015-5-27 16:57:15   联系qq 875556003   更新,改为性能更好的path
                        btn1 = new AyPath
                        {
                            SnapsToDevicePixels = true,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            //PathStrokeBrush = Stroke,
                            //Foreground=Foreground,
                            //PathStrokeThickness = StrokeThickness,
                            Stretch = Stretch,
                            UseLayoutRounding = true
                        };
                        PathIcon.SetIcon(btn1, Icon);
                        //设置绑定
                        Binding binding = new Binding { Path = new PropertyPath("Foreground"), Source = this, Mode = BindingMode.TwoWay };
                        btn1.SetBinding(AyPath.ForegroundProperty, binding);

                        Binding binding2 = new Binding { Path = new PropertyPath("Stroke"), Source = this, Mode = BindingMode.TwoWay };
                        btn1.SetBinding(AyPath.PathStrokeBrushProperty, binding2);

                        Binding binding3 = new Binding { Path = new PropertyPath("StrokeThickness"), Source = this, Mode = BindingMode.TwoWay };
                        btn1.SetBinding(AyPath.PathStrokeThicknessProperty, binding3);


                        this.Content = btn1;
                        IconType = AyIconAllType.Path;
                    }
                    else if (Icon.IndexOf("more_") == 0)
                    {
                        this.Content = GetMoreIcon();
                        IconType = AyIconAllType.More;
                    }

                }
                else if (ext == ".gif")
                {
                    try
                    {
                        imageGif = new AyGifControl
                        {
                            Width = this.Width,
                            Height = this.Height,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Stretch = ImageStretch
                        };
                        imageGif.InitControl(Icon);
                        imageGif.StartAnimate();
                        this.Content = imageGif;
                        IconType = AyIconAllType.Gif;
                    }
                    catch
                    {
                        //AyMessageBox.ShowError(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        if (IsBitmapImage)
                        {
                            AyImage image = new AyImage();
                            this.IsBitmapImage = false;
                            if (Icon.IndexOf("pack://") == 0)
                            {

                            }
                            image.Source = new BitmapImage(new Uri(Icon, UriKind.RelativeOrAbsolute));
                            this.Content = image;
                        }
                        else
                        {
                            Image image = new Image
                            {
                                VerticalAlignment = VerticalAlignment.Stretch,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Stretch = ImageStretch
                            };
                            image.UseLayoutRounding = true;
                            if (Icon.IndexOf("pack://") == 0)
                            {

                            }
                            image.Source = new BitmapImage(new Uri(Icon, UriKind.RelativeOrAbsolute));
                            this.Content = image;
                        }
                        IconType = AyIconAllType.Image;

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("文件格式不正确" + ex.Message);
                    }

                }
            }
        }
        public void StopGif()
        {
            if (imageGif != null && AyIconAllType.Gif == IconType)
            {
                imageGif.StopAnimate();
            }
        }


        /// <summary>
        /// ayui 3.5增加
        /// </summary>
        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageStretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageStretchProperty =
            DependencyProperty.Register("ImageStretch", typeof(Stretch), typeof(AyIconAll), new PropertyMetadata(Stretch.UniformToFill));


        /// <summary>
        /// 支持jpg,png
        /// 支持gif
        /// 支持@开头的fontawesome字体
        /// 支持path_   此名字来自application.xml中
        /// </summary>
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(AyIconAll), new PropertyMetadata("", IconLoadChanged));

        private static void IconLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            AyIconAll aia = d as AyIconAll;

            var vv = e.NewValue as string;
            var vv_Old = e.OldValue as string;
            if (!string.IsNullOrEmpty(vv_Old) && System.IO.Path.GetExtension(vv_Old) == ".gif")
            { //如果上次gif，则关闭，防止泄露
                aia.StopGif();
            }
            aia.LoadIcon();

        }
    }
}
