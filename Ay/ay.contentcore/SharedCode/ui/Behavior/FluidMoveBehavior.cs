using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ay.contentcore
{
	public sealed class FluidMoveBehavior : FluidMoveBehaviorBase
	{
		public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(FluidMoveBehavior), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1.0))));

		public static readonly DependencyProperty InitialTagProperty = DependencyProperty.Register("InitialTag", typeof(TagType), typeof(FluidMoveBehavior), new PropertyMetadata(TagType.Element));

		public static readonly DependencyProperty InitialTagPathProperty = DependencyProperty.Register("InitialTagPath", typeof(string), typeof(FluidMoveBehavior), new PropertyMetadata(string.Empty));

		private static readonly DependencyProperty InitialIdentityTagProperty = DependencyProperty.RegisterAttached("InitialIdentityTag", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		public static readonly DependencyProperty FloatAboveProperty = DependencyProperty.Register("FloatAbove", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(true));

		public static readonly DependencyProperty EaseXProperty = DependencyProperty.Register("EaseX", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		public static readonly DependencyProperty EaseYProperty = DependencyProperty.Register("EaseY", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		private static readonly DependencyProperty OverlayProperty = DependencyProperty.RegisterAttached("Overlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		private static readonly DependencyProperty CacheDuringOverlayProperty = DependencyProperty.RegisterAttached("CacheDuringOverlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));

		private static readonly DependencyProperty HasTransformWrapperProperty = DependencyProperty.RegisterAttached("HasTransformWrapper", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(false));

		private static Dictionary<object, Storyboard> TransitionStoryboardDictionary = new Dictionary<object, Storyboard>();

		public Duration Duration
		{
			get
			{
				return (Duration)GetValue(DurationProperty);
			}
			set
			{
				SetValue(DurationProperty, value);
			}
		}

		public TagType InitialTag
		{
			get
			{
				return (TagType)GetValue(InitialTagProperty);
			}
			set
			{
				SetValue(InitialTagProperty, value);
			}
		}

		public string InitialTagPath
		{
			get
			{
				return (string)GetValue(InitialTagPathProperty);
			}
			set
			{
				SetValue(InitialTagPathProperty, value);
			}
		}

		public bool FloatAbove
		{
			get
			{
				return (bool)GetValue(FloatAboveProperty);
			}
			set
			{
				SetValue(FloatAboveProperty, value);
			}
		}

		public IEasingFunction EaseX
		{
			get
			{
				return (IEasingFunction)GetValue(EaseXProperty);
			}
			set
			{
				SetValue(EaseXProperty, value);
			}
		}

		public IEasingFunction EaseY
		{
			get
			{
				return (IEasingFunction)GetValue(EaseYProperty);
			}
			set
			{
				SetValue(EaseYProperty, value);
			}
		}

		protected override bool ShouldSkipInitialLayout
		{
			get
			{
				if (!base.ShouldSkipInitialLayout)
				{
					return InitialTag == TagType.DataContext;
				}
				return true;
			}
		}

		private static object GetInitialIdentityTag(DependencyObject obj)
		{
			return obj.GetValue(InitialIdentityTagProperty);
		}

		private static void SetInitialIdentityTag(DependencyObject obj, object value)
		{
			obj.SetValue(InitialIdentityTagProperty, value);
		}

		private static object GetOverlay(DependencyObject obj)
		{
			return obj.GetValue(OverlayProperty);
		}

		private static void SetOverlay(DependencyObject obj, object value)
		{
			obj.SetValue(OverlayProperty, value);
		}

		private static object GetCacheDuringOverlay(DependencyObject obj)
		{
			return obj.GetValue(CacheDuringOverlayProperty);
		}

		private static void SetCacheDuringOverlay(DependencyObject obj, object value)
		{
			obj.SetValue(CacheDuringOverlayProperty, value);
		}

		private static bool GetHasTransformWrapper(DependencyObject obj)
		{
			return (bool)obj.GetValue(HasTransformWrapperProperty);
		}

		private static void SetHasTransformWrapper(DependencyObject obj, bool value)
		{
			obj.SetValue(HasTransformWrapperProperty, value);
		}

		protected override void EnsureTags(FrameworkElement child)
		{
			base.EnsureTags(child);
			if (InitialTag == TagType.DataContext)
			{
				object obj = child.ReadLocalValue(InitialIdentityTagProperty);
				if (!(obj is BindingExpression))
				{
					child.SetBinding(InitialIdentityTagProperty, new Binding(InitialTagPath));
				}
			}
		}

		internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData)
		{
			bool flag = false;
			bool flag2 = false;
			object initialIdentityTag = GetInitialIdentityTag(child);
			TagData value;
			bool flag3 = FluidMoveBehaviorBase.TagDictionary.TryGetValue(tag, out value);
			if (flag3 && value.InitialTag != initialIdentityTag)
			{
				flag3 = false;
				FluidMoveBehaviorBase.TagDictionary.Remove(tag);
			}
			Rect rect;
			if (!flag3)
			{
				TagData value2;
				if (initialIdentityTag != null && FluidMoveBehaviorBase.TagDictionary.TryGetValue(initialIdentityTag, out value2))
				{
					rect = FluidMoveBehaviorBase.TranslateRect(value2.AppRect, root, newTagData.Parent);
					flag = true;
					flag2 = true;
				}
				else
				{
					rect = Rect.Empty;
				}
				TagData tagData = new TagData();
				tagData.ParentRect = Rect.Empty;
				tagData.AppRect = Rect.Empty;
				tagData.Parent = newTagData.Parent;
				tagData.Child = child;
				tagData.Timestamp = DateTime.Now;
				tagData.InitialTag = initialIdentityTag;
				value = tagData;
				FluidMoveBehaviorBase.TagDictionary.Add(tag, value);
			}
			else if (value.Parent != VisualTreeHelper.GetParent(child))
			{
				rect = FluidMoveBehaviorBase.TranslateRect(value.AppRect, root, newTagData.Parent);
				flag = true;
			}
			else
			{
				rect = value.ParentRect;
			}
			if ((!IsEmptyRect(rect) && !IsEmptyRect(newTagData.ParentRect) && (!IsClose(rect.Left, newTagData.ParentRect.Left) || !IsClose(rect.Top, newTagData.ParentRect.Top))) || (child != value.Child && TransitionStoryboardDictionary.ContainsKey(tag)))
			{
				Rect currentRect = rect;
				bool flag4 = false;
				Storyboard value3 = null;
				if (TransitionStoryboardDictionary.TryGetValue(tag, out value3))
				{
					object overlay2 = GetOverlay(value.Child);
					AdornerContainer adornerContainer = (AdornerContainer)overlay2;
					flag4 = (overlay2 != null);
					FrameworkElement child2 = value.Child;
					if (overlay2 != null)
					{
						Canvas canvas = adornerContainer.Child as Canvas;
						if (canvas != null)
						{
							child2 = (canvas.Children[0] as FrameworkElement);
						}
					}
					if (!flag2)
					{
						Transform transform = GetTransform(child2);
						currentRect = transform.TransformBounds(currentRect);
					}
					TransitionStoryboardDictionary.Remove(tag);
					value3.Stop();
					value3 = null;
					RemoveTransform(child2);
					if (overlay2 != null)
					{
						AdornerLayer.GetAdornerLayer(root).Remove(adornerContainer);
						TransferLocalValue(value.Child, CacheDuringOverlayProperty, UIElement.RenderTransformProperty);
						SetOverlay(value.Child, null);
					}
				}
				object overlay = null;
				if (flag4 || (flag && FloatAbove))
				{
					Canvas canvas2 = new Canvas();
					canvas2.Width = newTagData.ParentRect.Width;
					canvas2.Height = newTagData.ParentRect.Height;
					canvas2.IsHitTestVisible = false;
					Canvas canvas3 = canvas2;
					Rectangle rectangle = new Rectangle();
					rectangle.Width = newTagData.ParentRect.Width;
					rectangle.Height = newTagData.ParentRect.Height;
					rectangle.IsHitTestVisible = false;
					Rectangle rectangle2 = rectangle;
					rectangle2.Fill = new VisualBrush(child);
					canvas3.Children.Add(rectangle2);
					AdornerContainer adornerContainer2 = new AdornerContainer(child);
					adornerContainer2.Child = canvas3;
					AdornerContainer adorner = (AdornerContainer)(overlay = adornerContainer2);
					SetOverlay(child, overlay);
					AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(root);
					adornerLayer.Add(adorner);
					TransferLocalValue(child, UIElement.RenderTransformProperty, CacheDuringOverlayProperty);
					child.RenderTransform = new TranslateTransform(-10000.0, -10000.0);
					canvas3.RenderTransform = new TranslateTransform(10000.0, 10000.0);
					child = rectangle2;
				}
				Rect layoutRect = newTagData.ParentRect;
				Storyboard transitionStoryboard = CreateTransitionStoryboard(child, flag2, ref layoutRect, ref currentRect);
				TransitionStoryboardDictionary.Add(tag, transitionStoryboard);
				transitionStoryboard.Completed += delegate
				{
					Storyboard value4;
					if (TransitionStoryboardDictionary.TryGetValue(tag, out value4) && value4 == transitionStoryboard)
					{
						TransitionStoryboardDictionary.Remove(tag);
						transitionStoryboard.Stop();
						RemoveTransform(child);
						child.InvalidateMeasure();
						if (overlay != null)
						{
							AdornerLayer.GetAdornerLayer(root).Remove((AdornerContainer)overlay);
							TransferLocalValue(child, CacheDuringOverlayProperty, UIElement.RenderTransformProperty);
							SetOverlay(child, null);
						}
					}
				};
				transitionStoryboard.Begin();
			}
			value.ParentRect = newTagData.ParentRect;
			value.AppRect = newTagData.AppRect;
			value.Parent = newTagData.Parent;
			value.Child = newTagData.Child;
			value.Timestamp = newTagData.Timestamp;
		}

		private Storyboard CreateTransitionStoryboard(FrameworkElement child, bool usingBeforeLoaded, ref Rect layoutRect, ref Rect currentRect)
		{
			Duration duration = Duration;
			Storyboard storyboard = new Storyboard();
			storyboard.Duration = duration;
			double num = (!usingBeforeLoaded || layoutRect.Width == 0.0) ? 1.0 : (currentRect.Width / layoutRect.Width);
			double num2 = (!usingBeforeLoaded || layoutRect.Height == 0.0) ? 1.0 : (currentRect.Height / layoutRect.Height);
			double num3 = currentRect.Left - layoutRect.Left;
			double num4 = currentRect.Top - layoutRect.Top;
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(new ScaleTransform
			{
				ScaleX = num,
				ScaleY = num2
			});
			transformGroup.Children.Add(new TranslateTransform
			{
				X = num3,
				Y = num4
			});
			AddTransform(child, transformGroup);
			string text = "(FrameworkElement.RenderTransform).";
			TransformGroup transformGroup2 = child.RenderTransform as TransformGroup;
			if (transformGroup2 != null && GetHasTransformWrapper(child))
			{
				object obj = text;
				text = obj + "(TransformGroup.Children)[" + (transformGroup2.Children.Count - 1) + "].";
			}
			if (usingBeforeLoaded)
			{
				if (num != 1.0)
				{
					DoubleAnimation doubleAnimation = new DoubleAnimation();
					doubleAnimation.Duration = duration;
					doubleAnimation.From = num;
					doubleAnimation.To = 1.0;
					DoubleAnimation doubleAnimation2 = doubleAnimation;
					Storyboard.SetTarget(doubleAnimation2, child);
					Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(text + "(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
					doubleAnimation2.EasingFunction = EaseX;
					storyboard.Children.Add(doubleAnimation2);
				}
				if (num2 != 1.0)
				{
					DoubleAnimation doubleAnimation3 = new DoubleAnimation();
					doubleAnimation3.Duration = duration;
					doubleAnimation3.From = num2;
					doubleAnimation3.To = 1.0;
					DoubleAnimation doubleAnimation4 = doubleAnimation3;
					Storyboard.SetTarget(doubleAnimation4, child);
					Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath(text + "(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
					doubleAnimation4.EasingFunction = EaseY;
					storyboard.Children.Add(doubleAnimation4);
				}
			}
			if (num3 != 0.0)
			{
				DoubleAnimation doubleAnimation5 = new DoubleAnimation();
				doubleAnimation5.Duration = duration;
				doubleAnimation5.From = num3;
				doubleAnimation5.To = 0.0;
				DoubleAnimation doubleAnimation6 = doubleAnimation5;
				Storyboard.SetTarget(doubleAnimation6, child);
				Storyboard.SetTargetProperty(doubleAnimation6, new PropertyPath(text + "(TransformGroup.Children)[1].(TranslateTransform.X)"));
				doubleAnimation6.EasingFunction = EaseX;
				storyboard.Children.Add(doubleAnimation6);
			}
			if (num4 != 0.0)
			{
				DoubleAnimation doubleAnimation7 = new DoubleAnimation();
				doubleAnimation7.Duration = duration;
				doubleAnimation7.From = num4;
				doubleAnimation7.To = 0.0;
				DoubleAnimation doubleAnimation8 = doubleAnimation7;
				Storyboard.SetTarget(doubleAnimation8, child);
				Storyboard.SetTargetProperty(doubleAnimation8, new PropertyPath(text + "(TransformGroup.Children)[1].(TranslateTransform.Y)"));
				doubleAnimation8.EasingFunction = EaseY;
				storyboard.Children.Add(doubleAnimation8);
			}
			return storyboard;
		}

		private static void AddTransform(FrameworkElement child, Transform transform)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup == null)
			{
				transformGroup = new TransformGroup();
				transformGroup.Children.Add(child.RenderTransform);
				child.RenderTransform = transformGroup;
				SetHasTransformWrapper(child, true);
			}
			transformGroup.Children.Add(transform);
		}

		private static Transform GetTransform(FrameworkElement child)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup != null && transformGroup.Children.Count > 0)
			{
				return transformGroup.Children[transformGroup.Children.Count - 1];
			}
			return new TranslateTransform();
		}

		private static void RemoveTransform(FrameworkElement child)
		{
			TransformGroup transformGroup = child.RenderTransform as TransformGroup;
			if (transformGroup != null)
			{
				if (GetHasTransformWrapper(child))
				{
					child.RenderTransform = transformGroup.Children[0];
					SetHasTransformWrapper(child, false);
				}
				else
				{
					transformGroup.Children.RemoveAt(transformGroup.Children.Count - 1);
				}
			}
		}

		private static void TransferLocalValue(FrameworkElement element, DependencyProperty source, DependencyProperty dest)
		{
			object obj = element.ReadLocalValue(source);
			BindingExpressionBase bindingExpressionBase = obj as BindingExpressionBase;
			if (bindingExpressionBase != null)
			{
				element.SetBinding(dest, bindingExpressionBase.ParentBindingBase);
			}
			else if (obj == DependencyProperty.UnsetValue)
			{
				element.ClearValue(dest);
			}
			else
			{
				element.SetValue(dest, element.GetAnimationBaseValue(source));
			}
			element.ClearValue(source);
		}

		private static bool IsClose(double a, double b)
		{
			return Math.Abs(a - b) < 1E-07;
		}

		private static bool IsEmptyRect(Rect rect)
		{
			if (!rect.IsEmpty && !double.IsNaN(rect.Left))
			{
				return double.IsNaN(rect.Top);
			}
			return true;
		}
	}
}
