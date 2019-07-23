using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.Wpf.Theme.Element
{
    public partial class AllControlsResourceDictionary : ElementStylesBase
    {
        internal static readonly List<Type> BlueMergedDataList = new List<Type>
        {
            typeof(Blue.Brushes)
        };


        public AllControlsResourceDictionary()
            : base(BlueMergedDataList)
        {
        }

        public AllControlsResourceDictionary(ColorModeEnum colorMode)
            : base((colorMode == ColorModeEnum.Blue) ? BlueMergedDataList : null)
        {
        }

        public override void Initialize()
        {
            InitializeComponent();
        }


    }
}
