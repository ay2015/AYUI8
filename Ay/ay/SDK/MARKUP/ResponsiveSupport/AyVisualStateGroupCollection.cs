using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ay.MARKUP.ResponsiveSupport
{
    public class AyVisualStateGroupCollection : FreezableCollection<AyVisualStateGroup>
    {
        public DependencyObject Owner { get; set; }

        protected override Freezable CreateInstanceCore()
        {
            return new AyVisualStateGroupCollection();
        }
    }

    public class AyVisualStateCollection : Collection<AyVisualState>
    {


    }

    public class AySetterCollection : Collection<IDataSetter>
    {

    }

}
