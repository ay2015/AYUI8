using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Interactivity
{
	public static class DependencyObjectHelper
	{
		public static IEnumerable<DependencyObject> GetSelfAndAncestors(this DependencyObject dependencyObject)
		{
			while (dependencyObject != null)
			{
				yield return dependencyObject;
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
		}
	}
}
