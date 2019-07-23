using System.Windows;
using System.Windows.Controls;

namespace ay.contentcore
{

    public class TemplateBorder : Border
    {
        static TemplateBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TemplateBorder), new FrameworkPropertyMetadata(typeof(TemplateBorder)));
        }
    }
}
