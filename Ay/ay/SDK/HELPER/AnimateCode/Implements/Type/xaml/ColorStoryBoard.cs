
using ay.Animate;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Controls
{
    public interface IBinding
    {
        void SetBinding(ClrBinding c, Binding binding);
    }
    [MarkupExtensionReturnType(typeof(Storyboard))]
    public class AyAniColorStoryBoard : MarkupExtension, IBinding
    {
        #region 共用属性
        private PropertyPath aniPath;

        public PropertyPath AniPath
        {
            get { return aniPath; }
            set { aniPath = value; }
        }

        public string AnimateKey { get; set; }


        #endregion
        #region 定制属性

        private readonly ClrBinding _FromColor;

        public static readonly DependencyProperty FromColorProperty = DependencyProperty.RegisterAttached(
            "FromColor", typeof(object), typeof(AyAniColorStoryBoard),
            new PropertyMetadata(Colors.Transparent, ClrBinding.ValueChangeCallback));

        public object FromColor
        {
            get => _FromColor.GetValue();
            set
            {
                //DynamicResourceExtensionConverter d = new DynamicResourceExtensionConverter();
                dynamic dd = value;
                //var _2 = d.ConvertFrom(dd.ResourceKey);
                var _2 = dd.ResourceKey;
                _FromColor.SetValue(value);

            }
        }
        public string FromColorKey { get; set; }
        public string ToColorKey { get; set; }

        private void OnFromColorChanged(object oldValue, object newValue)
        {
            Console.WriteLine("");
        }


        /// <summary>
        /// 无注释
        /// </summary>
        private readonly ClrBinding _ToColor;

        public static readonly DependencyProperty ToColorProperty = DependencyProperty.RegisterAttached(
            "ToColor", typeof(object), typeof(AyAniColorStoryBoard),
            new PropertyMetadata(Colors.Transparent, ClrBinding.ValueChangeCallback));

        public object ToColor
        {
            get => _ToColor.GetValue();
            set => _ToColor.SetValue(value);
        }

        private void OnToColorChanged(object oldValue, object newValue)
        {
            Console.WriteLine("");
        }

        public AyAniColor Ani { get; set; }

        #endregion
        public AyAniColorStoryBoard()
        {
            _FromColor = new ClrBinding(this, FromColorProperty, OnFromColorChanged);
            _ToColor = new ClrBinding(this, ToColorProperty, OnToColorChanged);
        }
        public AyAniColorStoryBoard(string animateKey)
        {
            this.AnimateKey = animateKey;
        }
        public AyAniColorStoryBoard(string animateKey, PropertyPath path,AyAniColor ani,string fckey,string tckey)
        {
            this.AnimateKey = animateKey;
            this.Ani = ani;
            this.FromColorKey = fckey;
            this.ToColorKey = tckey;
            this.AniPath = path;
            _FromColor = new ClrBinding(this, FromColorProperty, OnFromColorChanged);
            _ToColor = new ClrBinding(this, ToColorProperty, OnToColorChanged);
            //Ani = new ClrBinding(this, AniProperty, OnAniChanged);
        }



        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (AnimateKey == null)
            {
                throw new InvalidOperationException("AnimateKey Not Found");
            }

            if (serviceProvider != null)
            {
                var _a = Ani as AyAniColor;
                _a.Element= AyAnimateService.References[AnimateKey].Target as FrameworkElement;
                _a.SetResourceReference(AyAniColor.FromColorProperty, FromColor);
                //_a.SetResourceReference(AyAniColor.ToColorProperty, ToColor);
                //d.FromColor = (Color?)FromColor;
                //d.ToColor = (Color?)ToColor;
                _a.AniPropertyPath = aniPath;
                //, Colors.Red, Colors.Green, new PropertyPath("(Border.Background).(SolidColorBrush.Color)";

                return _a.Story;
            }
            return null;
        }

        public void SetBinding(ClrBinding c, Binding binding)
        {
            Console.WriteLine("");
        }
    }

}
