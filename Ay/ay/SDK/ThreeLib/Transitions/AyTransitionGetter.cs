using Ay.Framework.WPF.Controls.Transitions;
using System;
using System.Windows;

namespace ay.Controls.Helper
{
    public static class AyTransitionGetter
    {
        public static ResourceDictionary dictionary = null;
        public static Transition[] AyTransitionOneWay()
        {
            if (dictionary == null)
            {
                dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("/ay;component/Themes/Common.xaml"), UriKind.RelativeOrAbsolute);
            }
            return dictionary["Transitions"] as Transition[];
        }
        public static Transition[] AyTransitionTwoWay()
        {
            if (dictionary == null)
            {
                dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("/ay;component/Themes/Common.xaml"), UriKind.RelativeOrAbsolute);
            }
            return dictionary["ForwardBackTransitions"] as Transition[];
        }

    }
}
