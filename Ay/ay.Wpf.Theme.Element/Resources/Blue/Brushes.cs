using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace ay.Wpf.Theme.Element.Blue
{
	public partial class Brushes : ElementStylesBase
	{
		internal static readonly List<Type> MergedDataList = new List<Type>();

		public Brushes()
			: base(MergedDataList)
		{
		}

		public Brushes(ColorModeEnum colorMode)
			: base(MergedDataList)
		{
		}

		public override void Initialize()
		{
			InitializeComponent();
		}

	}
}
