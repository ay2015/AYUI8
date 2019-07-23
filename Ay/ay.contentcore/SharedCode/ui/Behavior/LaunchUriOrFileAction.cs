using System.Diagnostics;
using System.Windows;
using System.Windows.Interactivity;

namespace ay.contentcore
{
    public class LaunchUriOrFileAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(LaunchUriOrFileAction));

        public string Path
        {
            get
            {
                return (string)GetValue(PathProperty);
            }
            set
            {
                SetValue(PathProperty, value);
            }
        }

        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject != null && !string.IsNullOrEmpty(Path))
            {
                Process.Start(Path);
            }
        }
    }
}
