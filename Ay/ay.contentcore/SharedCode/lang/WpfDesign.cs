using System.ComponentModel;
using System.Windows;

namespace ay.contentcore
{
    public class WpfDesign
    {
        private static bool? _isInDesignMode;
        /// <summary>
        /// 是否涉及模式
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
            _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    _isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
#endif
                }
                return _isInDesignMode.Value;
            }
        }

    }
}
