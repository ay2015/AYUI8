using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ay.contentcore
{

public partial class ColorFontDialog : Window
{
    private FontInfo selectedFont;

    public ColorFontDialog()
    {
        this.selectedFont = null; // Default
        InitializeComponent();
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
        this.ColorFontChooser.lstFamily.SelectedIndex = idx;
        this.ColorFontChooser.lstFamily.ScrollIntoView(this.ColorFontChooser.lstFamily.Items[idx]);
    }

    private void SyncFontSize()
    {
        double fontSize = this.selectedFont.Size;
        this.ColorFontChooser.fontSizeSlider.Value = fontSize;
    }

    private void SyncFontColor()
    {
        int colorIdx = AvailableColors.GetFontColorIndex(this.Font.Color);
        this.ColorFontChooser.colorPicker.superCombo.SelectedIndex = colorIdx;
        this.ColorFontChooser.txtSampleText.Foreground = this.Font.Color.Brush;
        this.ColorFontChooser.colorPicker.superCombo.BringIntoView();
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
        this.SyncFontColor();
        this.SyncFontName();
        this.SyncFontSize();
        this.SyncFontTypeface();
    }
}
}
