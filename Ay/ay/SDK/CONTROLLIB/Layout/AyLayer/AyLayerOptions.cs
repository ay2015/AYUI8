
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ay.Controls
{
    public interface IAyLayerSupport
    {
        Border AyBackgroundBehindLayer { get; set; }
        Border AyBackgroundLayer { get; set; }
        ContentPresenter AllCP { get; set; }
    }
    public class AyLayerOptions : ICloneable
    {
        private static readonly object sync = new object();
        public static AyLayerOptions _defaultAyLayerOptions;
        public static AyLayerOptions DefaultAyLayerOptions
        {
            get
            {
                if (_defaultAyLayerOptions == null)
                {
                    lock (sync)
                    {
                        if (_defaultAyLayerOptions == null)
                        {
                            _defaultAyLayerOptions = new AyLayerOptions();
                            _defaultAyLayerOptions.CanDrag = true;
                            _defaultAyLayerOptions.HasShadow = false;
                            _defaultAyLayerOptions.MaskBrush = SolidColorBrushConverter.From16JinZhi("#8C000000");
                        }
                    }
                }
                return _defaultAyLayerOptions;
            }
        }

        private Border titleBar;
        /// <summary>
        /// 2017-10-20 15:39:01 增加
        /// </summary>
        public Border TitleBar
        {
            get { return titleBar; }
            set { titleBar = value; }
        }
        public bool WhenShowDialogNeedShake { get; set; } = true;

        private bool canDrag = true;
        /// <summary>
        /// 是否可以移动
        /// </summary>
        public bool CanDrag
        {
            get { return canDrag; }
            set { canDrag = value; }
        }
        private bool canClose = true;
        /// <summary>
        /// 右上角按钮，是否可以关闭
        /// </summary>
        public bool CanClose
        {
            get { return canClose; }
            set { canClose = value; }
        }



        private string _layerId = AyCommon.GetGuidNoSplit;
        /// <summary>
        /// 层Id
        /// </summary>
        public string LayerId
        {
            get { return _layerId; }
            set { _layerId = value; }
        }

        private bool isShowTitleBar = true;
        /// <summary>
        /// 是否包含标题栏
        /// </summary>
        public bool IsContainsTitleBar
        {
            get { return isShowTitleBar; }
            set { isShowTitleBar = value; }
        }
        public Thickness TitleBarBorderThickness { get; set; } = new Thickness(0, 0, 0, 1);

        private Brush maskBrush = null;
        /// <summary>
        /// 遮盖画刷
        /// </summary>
        public Brush MaskBrush
        {
            get { return maskBrush; }
            set { maskBrush = value; }
        }


        private bool hasShadow = false;
        /// <summary>
        /// 是否有阴影
        /// </summary>
        public bool HasShadow
        {
            get { return hasShadow; }
            set { hasShadow = value; }
        }


        private double _width;
        /// <summary>
        /// 宽度,废弃
        /// </summary>
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private double _height;
        /// <summary>
        /// 高度,废弃
        /// </summary>
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }


        private double _minWidth;
        /// <summary>
        /// 最小宽度,废弃
        /// </summary>
        public double MinWidth
        {
            get { return _minWidth; }
            set { _minWidth = value; }
        }


        private double _minHeight;
        /// <summary>
        /// 最小高度,废弃
        /// </summary>
        public double MinHeight
        {
            get { return _minHeight; }
            set { _minHeight = value; }
        }

        private double _maxWidth;
        /// <summary>
        /// 最大宽度,废弃
        /// </summary>
        public double MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }


        private double _maxHeight;
        /// <summary>
        /// 最大高度,废弃
        /// </summary>
        public double MaxHeight
        {
            get { return _maxHeight; }
            set { _maxHeight = value; }
        }

        private int showAnimateIndex = 1;
        /// <summary>
        /// 填入1-15，目前15种开场动画
        /// </summary>
        public int ShowAnimateIndex
        {
            get { return showAnimateIndex; }
            set { showAnimateIndex = value; }
        }
        /// <summary>
        /// 是否有关闭动画
        /// </summary>
        public bool HasCloseAnimation { get; set; } = true;

        private AyLayerDockDirect? _direction = AyLayerDockDirect.CC;
        /// <summary>
        /// 弹窗第一次方向，默认居中。
        /// </summary>
        public AyLayerDockDirect? Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private double shadowRadius = 12.00;

        public double ShadowRadius
        {
            get { return shadowRadius; }
            set { shadowRadius = value; }
        }
        private Color shodowColor = SolidColorBrushConverter.ToColor("#000000");

        public Color ShadowColor
        {
            get { return shodowColor; }
            set { shodowColor = value; }
        }
        private double shadowDepth = 2;

        public double ShadowDepth
        {
            get { return shadowDepth; }
            set { shadowDepth = value; }
        }

        private bool isShowLayerBorder = false;

        public bool IsShowLayerBorder
        {
            get { return isShowLayerBorder; }
            set { isShowLayerBorder = value; }
        }

        private Thickness? layerBorderThickness;

        public Thickness? LayerBorderThickness
        {
            get { return layerBorderThickness; }
            set { layerBorderThickness = value; }
        }

        public CornerRadius? LayerCornerRadius { get; set; } = new CornerRadius(0);

        private Brush layerBackground = new SolidColorBrush(Colors.White);

        public Brush LayerBackground
        {
            get { return layerBackground; }
            set { layerBackground = value; }
        }

        public object Clone()
        {
            AyLayerOptions _defaultAyLayerOptions = new AyLayerOptions();
            _defaultAyLayerOptions = new AyLayerOptions();
            _defaultAyLayerOptions.CanDrag = true;
            _defaultAyLayerOptions.HasShadow = false;
            _defaultAyLayerOptions.MaskBrush = HexToBrush.FromHex("#8C000000");
            return _defaultAyLayerOptions;
        }

        #region 打开和关闭触发 2016-12-5 20:15:03
        public Action Opened { get; set; }
        public Action Closed { get; set; }
        #endregion


    }
    public enum AyLayerDockDirect
    {
        LT,
        CT,
        RT,
        LC,
        CC,
        RC,
        LB,
        CB,
        RB
    }
}
