using System;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Controls;


namespace ay.MARKUP.ResponsiveSupport
{
    [ContentProperty("Setters")]
    public class AyVisualState : DependencyObject
    {

        public double? MinWindowHeight { get; set; }
        public double? MinWindowWidth { get; set; }

        public double? MaxWindowHeight { get; set; }
        public double? MaxWindowWidth { get; set; }
        /// <summary>
        /// 默认终止，执行方式
        /// </summary>
        public VisualStateSetteMode SetterMode { get; set; } = VisualStateSetteMode.End;

        private AySetterCollection _setters = null;
        public AySetterCollection Setters
        {
            get
            {

                VerifyAccess();

                if (_setters == null)
                {
                    _setters = new AySetterCollection();
                }
                return _setters;
            }
        }
    }

    public enum VisualStateSetteMode
    {
        End,
        Continue
    }
}
