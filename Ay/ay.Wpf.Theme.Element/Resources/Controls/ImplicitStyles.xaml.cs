using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ay.Wpf.Theme.Element
{
    public partial class ImplicitStyles : ElementStylesBase
    {
        internal static readonly List<Type> _mergedDataList = new List<Type>
        {
            typeof(ExplicitStyles)
        };


        public ImplicitStyles()
            : base(_mergedDataList)
        {
        }

        public ImplicitStyles(ColorModeEnum colorMode)
            : base(_mergedDataList)
        {
        }

        public override void Initialize()
        {
            InitializeComponent();
        }


    }
}
