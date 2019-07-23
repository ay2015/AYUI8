using System.Windows.Media;

namespace ay.Wpf.Theme.Element
{
    public class ElementBlueThemeResourceDictionary : ElementThemeResourceDictionaryBase
    {
        public ElementBlueThemeResourceDictionary()
            : base(ColorModeEnum.Blue)
        {
        }

        public ElementBlueThemeResourceDictionary(Brush accentBrush)
            : base(accentBrush, ColorModeEnum.Blue)
        {
        }

        public void ChangeTheme()
        {

        }
    }
}
