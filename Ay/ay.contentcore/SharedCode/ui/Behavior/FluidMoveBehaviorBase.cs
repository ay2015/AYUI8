
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ay.contentcore
{
	public abstract class FluidMoveBehaviorBase : Behavior<FrameworkElement>
	{
		internal class TagData
		{
			public FrameworkElement Child
			{
				get;
				set;
			}

			public FrameworkElement Parent
			{
				get;
				set;
			}

			public Rect ParentRect
			{
				get;
				set;
			}

			public Rect AppRect
			{
				get;
				set;
			}

			public DateTime Timestamp
			{
				get;
				set;
			}

			public object InitialTag
			{
				get;
				set;
			}
		}

		public static readonly DependencyProperty AppliesToProperty = DependencyProperty.Register("AppliesTo", typeof(FluidMoveScope), typeof(FluidMoveBehaviorBase), new PropertyMetadata(FluidMoveScope.Self));

		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(FluidMoveBehaviorBase), new PropertyMetadata(true));

		public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(TagType), typeof(FluidMoveBehaviorBase), new PropertyMetadata(TagType.Element));

		public static readonly DependencyProperty TagPathProperty = DependencyProperty.Register("TagPath", typeof(string), typeof(FluidMoveBehaviorBase), new PropertyMetadata(string.Empty));

		protected static readonly DependencyProperty IdentityTagProperty = DependencyProperty.RegisterAttached("IdentityTag", typeof(object), typeof(FluidMoveBehaviorBase), new PropertyMetadata(null));

		internal static Dictionary<object, TagData> TagDictionary = new Dictionary<object, TagData>();

		private static DateTime NextToLastPurgeTick = DateTime.MinValue;

		private static DateTime LastPurgeTick = DateTime.MinValue;

		private static TimeSpan MinTickDelta = TimeSpan.FromSeconds(0.5);

		public FluidMoveScope AppliesTo
		{
			get
			{
				return (FluidMoveScope)GetValue(AppliesToProperty);
			}
			set
			{
				SetValue(AppliesToProperty, value);
			}
		}

		public bool IsActive
		{
			get
			{
				return (bool)GetValue(IsActiveProperty);
			}
			set
			{
				SetValue(IsActiveProperty, value);
			}
		}

		public TagType Tag
		{
			get
			{
				return (TagType)GetValue(TagProperty);
			}
			set
			{
				SetValue(TagProperty, value);
			}
		}

		public string TagPath
		{
			get
			{
				return (string)GetValue(TagPathProperty);
			}
			set
			{
				SetValue(TagPathProperty, value);
			}
		}

		protected virtual bool ShouldSkipInitialLayout
		{
			get
			{
				return Tag == TagType.DataContext;
			}
		}

		protected static object GetIdentityTag(DependencyObject obj)
		{
			return obj.GetValue(IdentityTagProperty);
		}

		protected static void SetIdentityTag(DependencyObject obj, object value)
		{
			obj.SetValue(IdentityTagProperty, value);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			base.AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			base.AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
		}

		private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
		{
			if (IsActive)
			{
				if (DateTime.Now - LastPurgeTick >= MinTickDelta)
				{
					List<object> list = null;
					foreach (KeyValuePair<object, TagData> item in TagDictionary)
					{
						if (item.Value.Timestamp < NextToLastPurgeTick)
						{
							if (list == null)
							{
								list = new List<object>();
							}
							list.Add(item.Key);
						}
					}
					if (list != null)
					{
						foreach (object item2 in list)
						{
							TagDictionary.Remove(item2);
						}
					}
					NextToLastPurgeTick = LastPurgeTick;
					LastPurgeTick = DateTime.Now;
				}
				if (AppliesTo == FluidMoveScope.Self)
				{
					UpdateLayoutTransition(base.AssociatedObject);
				}
				else
				{
					Panel panel = base.AssociatedObject as Panel;
					if (panel != null)
					{
						foreach (FrameworkElement child in panel.Children)
						{
							UpdateLayoutTransition(child);
						}
					}
				}
			}
		}

		private void UpdateLayoutTransition(FrameworkElement child)
		{
			if ((child.Visibility != Visibility.Collapsed && child.IsLoaded) || !ShouldSkipInitialLayout)
			{
				FrameworkElement visualRoot = GetVisualRoot(child);
				TagData tagData = new TagData();
				tagData.Parent = (VisualTreeHelper.GetParent(child) as FrameworkElement);
				tagData.ParentRect = ExtendedVisualStateManager.GetLayoutRect(child);
				tagData.Child = child;
				tagData.Timestamp = DateTime.Now;
				try
				{
					tagData.AppRect = TranslateRect(tagData.ParentRect, tagData.Parent, visualRoot);
				}
				catch (ArgumentException)
				{
					if (ShouldSkipInitialLayout)
					{
						return;
					}
				}
				EnsureTags(child);
				object obj = GetIdentityTag(child);
				if (obj == null)
				{
					obj = child;
				}
				UpdateLayoutTransitionCore(child, visualRoot, obj, tagData);
			}
		}

		internal abstract void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData);

		protected virtual void EnsureTags(FrameworkElement child)
		{
			if (Tag == TagType.DataContext)
			{
				object obj = child.ReadLocalValue(IdentityTagProperty);
				if (!(obj is BindingExpression))
				{
					child.SetBinding(IdentityTagProperty, new Binding(TagPath));
				}
			}
		}

		private static FrameworkElement GetVisualRoot(FrameworkElement child)
		{
			while (true)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(child) as FrameworkElement;
				if (frameworkElement == null)
				{
					return child;
				}
				if (AdornerLayer.GetAdornerLayer(frameworkElement) == null)
				{
					break;
				}
				child = frameworkElement;
			}
			return child;
		}

		internal static Rect TranslateRect(Rect rect, FrameworkElement from, FrameworkElement to)
		{
			if (from == null || to == null)
			{
				return rect;
			}
			Point point = new Point(rect.Left, rect.Top);
			point = from.TransformToVisual(to).Transform(point);
			return new Rect(point.X, point.Y, rect.Width, rect.Height);
		}
	}
}
