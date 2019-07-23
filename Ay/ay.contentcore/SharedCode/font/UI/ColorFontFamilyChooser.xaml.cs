using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ay.contentcore
{
    public partial class ColorFontFamilyChooser : UserControl
    {
        public ColorFontFamilyChooser()
        {
            InitializeComponent();
            this.txtSampleText.IsReadOnly = true;
        }

        public FontInfo SelectedFont
        {
            get
            {
                return new FontInfo(this.txtSampleText.FontFamily,
                                    this.txtSampleText.FontSize,
                                    this.txtSampleText.FontStyle,
                                    this.txtSampleText.FontStretch,
                                    this.txtSampleText.FontWeight,
                                    null);
            }

        }

     
    }
}
