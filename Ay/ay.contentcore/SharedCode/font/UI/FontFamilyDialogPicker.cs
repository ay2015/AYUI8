
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using System.Windows.Input;
using i = System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Interactivity;

namespace ay.contentcore
{
    [DefaultTrigger(typeof(ButtonBase), typeof(i.EventTrigger), new object[] { "Click" })]
    [DefaultTrigger(typeof(Shape), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    [DefaultTrigger(typeof(UIElement), typeof(i.EventTrigger), new object[] { "MouseLeftButtonDown" })]
    public class FontFamilyDialogPicker : TriggerAction<FrameworkElement>
    {

        protected override void Invoke(object parameter)
        {
            ColorFontFamilyDialog fntDialog = new ColorFontFamilyDialog();
            FontInfo f = new FontInfo();
            string fontFamilyName = Properties.Settings.Default.LastFontFamily;
            string fontFamilyStretch = Properties.Settings.Default.LastFontStretch;
            string fontFamilyStyle = Properties.Settings.Default.LastFontStyle;
            string fontFamilyWeight = Properties.Settings.Default.LastFontWeight;
            if (string.IsNullOrWhiteSpace(fontFamilyName))
            {
                //读取资源
                var _1 = Application.Current.Resources["NormalFontFamily"] as FontFamily;
                fontFamilyName = _1.Source;
            }
            if (string.IsNullOrWhiteSpace(fontFamilyStretch))
            {
                var _2 = (FontStretch)Application.Current.Resources["NormalFontStretch"];
                fontFamilyStretch = _2.ToString();

            }
            if (string.IsNullOrWhiteSpace(fontFamilyStyle))
            {
                var _3 = (FontStyle)Application.Current.Resources["NormalFontStyle"];
                fontFamilyStyle = _3.ToString();
            }
            if (string.IsNullOrWhiteSpace(fontFamilyWeight))
            {
                var _4 = (FontWeight)Application.Current.Resources["NormalFontWeight"];
                fontFamilyWeight = _4.ToString();
            }
            f.Family = new FontFamily(fontFamilyName);
            f.Weight = fontFamilyWeight.ToFontWeight();
            f.Stretch = fontFamilyStretch.ToFontStretch();
            f.Style = fontFamilyStyle.ToFontStyle();
            var _win = Window.GetWindow(this.AssociatedObject) as Window;
            fntDialog.Owner = _win;
            f.Size = (double)Application.Current.Resources["NormalFontSize"];
            fntDialog.Font = f;
            if (fntDialog.ShowDialog() == true)
            {
                FontInfo selectedFont = fntDialog.Font;
                if (selectedFont != null)
                {
                    Application.Current.Resources["NormalFontFamily"] = selectedFont.Family;
                    Application.Current.Resources["NormalFontStretch"] = selectedFont.Stretch;
                    Application.Current.Resources["NormalFontStyle"] = selectedFont.Style;
                    Application.Current.Resources["NormalFontWeight"] = selectedFont.Weight;
                    Properties.Settings.Default.LastFontFamily = selectedFont.Family.Source;
                    Properties.Settings.Default.LastFontStretch = selectedFont.Stretch.ToString();
                    Properties.Settings.Default.LastFontStyle = selectedFont.Style.ToString();
                    Properties.Settings.Default.LastFontWeight = selectedFont.Weight.ToString();
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}


