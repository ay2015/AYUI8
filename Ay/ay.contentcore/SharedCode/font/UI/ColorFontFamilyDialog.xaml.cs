using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ay.contentcore
{

    public partial class ColorFontFamilyDialog : Window
    {
        private FontInfo selectedFont;

        public ColorFontFamilyDialog()
        {
            this.selectedFont = null; // Default
            InitializeComponent();
            this.Loaded += Window_Loaded_1;
        }

        public FontInfo Font
        {
            get
            {
                return this.selectedFont;
            }

            set
            {
                FontInfo fi = value;
                this.selectedFont = fi;
            }
        }

        private void SyncFontName()
        {
            string fontFamilyName = this.selectedFont.Family.Source;
 
            int idx = 0;
            foreach (var item in this.ColorFontChooser.lstFamily.Items)
            {
                string itemName = item.ToString();
                if (fontFamilyName == itemName)
                {
                    break;
                }
                idx++;
            }
            if(idx >= this.ColorFontChooser.lstFamily.Items.Count)
            {
                idx = 0;
            }
            this.ColorFontChooser.lstFamily.SelectedIndex = idx;
            this.ColorFontChooser.lstFamily.ScrollIntoView(this.ColorFontChooser.lstFamily.Items[idx]);
        }


        private void SyncFontSize()
        {
            double fontSize = this.selectedFont.Size;
            this.ColorFontChooser.fontSizeSlider.Value = fontSize;
        }
        private void SyncFontTypeface()
        {
            string fontTypeFaceSb = FontInfo.TypefaceToString(this.selectedFont.Typeface);
            int idx = 0;
            foreach (var item in this.ColorFontChooser.lstTypefaces.Items)
            {
                FamilyTypeface face = item as FamilyTypeface;
                if (fontTypeFaceSb == FontInfo.TypefaceToString(face))
                {
                    break;
                }
                idx++;
            }
            this.ColorFontChooser.lstTypefaces.SelectedIndex = idx;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Font = this.ColorFontChooser.SelectedFont;

            this.DialogResult = true;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Loaded -= Window_Loaded_1;
            SyncFontSize();
            this.SyncFontName();
            this.SyncFontTypeface();
        }
    }
}
