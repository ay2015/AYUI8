using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace ay.contentcore
{
    public class ExtendedVisualStateManager : VisualStateManager
    {
        internal class WrapperCanvas : Canvas
        {
            internal static readonly DependencyProperty SimulationProgressProperty = DependencyProperty.Register("SimulationProgress", typeof(double), typeof(WrapperCanvas), new PropertyMetadata(0.0, SimulationProgressChanged));

            public Rect OldRect
            {
                get;
                set;
            }

            public Rect NewRect
            {
                get;
                set;
            }

            public Dictionary<DependencyProperty, object> LocalValueCache
            {
                get;
                set;
            }

            public Visibility DestinationVisibilityCache
            {
                get;
                set;
            }

            public double SimulationProgress
            {
                get
                {
                    return (double)GetValue(SimulationProgressProperty);
                }
                set
                {
                    SetValue(SimulationProgressProperty, value);
                }
            }

            private static void SimulationProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                WrapperCanvas wrapperCanvas = d as WrapperCanvas;
                double num = (double)e.NewValue;
                if (wrapperCanvas != null && wrapperCanvas.Children.Count > 0)
                {
                    FrameworkElement frameworkElement = wrapperCanvas.Children[0] as FrameworkElement;
                    frameworkElement.Width = Math.Max(0.0, wrapperCanvas.OldRect.Width * num + wrapperCanvas.NewRect.Width * (1.0 - num));
                    frameworkElement.Height = Math.Max(0.0, wrapperCanvas.OldRect.Height * num + wrapperCanvas.NewRect.Height * (1.0 - num));
                    Canvas.SetLeft(frameworkElement, num * (wrapperCanvas.OldRect.Left - wrapperCanvas.NewRect.Left));
                    Canvas.SetTop(frameworkElement, num * (wrapperCanvas.OldRect.Top - wrapperCanvas.NewRect.Top));
                }
            }
        }

        internal class OriginalLayoutValueRecord
        {
            public FrameworkElement Element
            {
                get;
                set;
            }

            public DependencyProperty Property
            {
                get;
                set;
            }

            public object Value
            {
                get;
                set;
            }
        }

        private class DummyEasingFunction : EasingFunctionBase
        {
            public static readonly DependencyProperty DummyValueProperty = DependencyProperty.Register("DummyValue", typeof(double), typeof(DummyEasingFunction), new PropertyMetadata(0.0));

            public double DummyValue
            {
                get
                {
                    return (double)GetValue(DummyValueProperty);
                }
                set
                {
                    SetValue(DummyValueProperty, value);
                }
            }

            protected override Freezable CreateInstanceCore()
            {
                return new DummyEasingFunction();
            }

            protected override double EaseInCore(double normalizedTime)
            {
                return DummyValue;
            }
        }

        public static readonly DependencyProperty UseFluidLayoutProperty = DependencyProperty.RegisterAttached("UseFluidLayout", typeof(bool), typeof(ExtendedVisualStateManager), new PropertyMetadata(false));

        public static readonly DependencyProperty RuntimeVisibilityPropertyProperty = DependencyProperty.RegisterAttached("RuntimeVisibilityProperty", typeof(DependencyProperty), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty OriginalLayoutValuesProperty = DependencyProperty.RegisterAttached("OriginalLayoutValues", typeof(List<OriginalLayoutValueRecord>), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty LayoutStoryboardProperty = DependencyProperty.RegisterAttached("LayoutStoryboard", typeof(Storyboard), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty CurrentStateProperty = DependencyProperty.RegisterAttached("CurrentState", typeof(VisualState), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        public static readonly DependencyProperty TransitionEffectProperty = DependencyProperty.RegisterAttached("TransitionEffect", typeof(TransitionEffect), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty TransitionEffectStoryboardProperty = DependencyProperty.RegisterAttached("TransitionEffectStoryboard", typeof(Storyboard), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty DidCacheBackgroundProperty = DependencyProperty.RegisterAttached("DidCacheBackground", typeof(bool), typeof(ExtendedVisualStateManager), new PropertyMetadata(false));

        internal static readonly DependencyProperty CachedBackgroundProperty = DependencyProperty.RegisterAttached("CachedBackground", typeof(object), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        internal static readonly DependencyProperty CachedEffectProperty = DependencyProperty.RegisterAttached("CachedEffect", typeof(Effect), typeof(ExtendedVisualStateManager), new PropertyMetadata(null));

        private static List<FrameworkElement> MovingElements;

        private static Storyboard LayoutTransitionStoryboard;

        private static List<DependencyProperty> LayoutProperties = new List<DependencyProperty>
        {
            Grid.ColumnProperty,
            Grid.ColumnSpanProperty,
            Grid.RowProperty,
            Grid.RowSpanProperty,
            Canvas.LeftProperty,
            Canvas.TopProperty,
            FrameworkElement.WidthProperty,
            FrameworkElement.HeightProperty,
            FrameworkElement.MinWidthProperty,
            FrameworkElement.MinHeightProperty,
            FrameworkElement.MaxWidthProperty,
            FrameworkElement.MaxHeightProperty,
            FrameworkElement.MarginProperty,
            FrameworkElement.HorizontalAlignmentProperty,
            FrameworkElement.VerticalAlignmentProperty,
            UIElement.VisibilityProperty,
            StackPanel.OrientationProperty
        };

        private static List<DependencyProperty> ChildAffectingLayoutProperties = new List<DependencyProperty>
        {
            StackPanel.OrientationProperty
        };

        private bool changingState;

        public static bool IsRunningFluidLayoutTransition
        {
            get
            {
                return LayoutTransitionStoryboard != null;
            }
        }

        public static bool GetUseFluidLayout(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseFluidLayoutProperty);
        }

        public static void SetUseFluidLayout(DependencyObject obj, bool value)
        {
            obj.SetValue(UseFluidLayoutProperty, value);
        }

        public static DependencyProperty GetRuntimeVisibilityProperty(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(RuntimeVisibilityPropertyProperty);
        }

        public static void SetRuntimeVisibilityProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(RuntimeVisibilityPropertyProperty, value);
        }

        internal static List<OriginalLayoutValueRecord> GetOriginalLayoutValues(DependencyObject obj)
        {
            return (List<OriginalLayoutValueRecord>)obj.GetValue(OriginalLayoutValuesProperty);
        }

        internal static void SetOriginalLayoutValues(DependencyObject obj, List<OriginalLayoutValueRecord> value)
        {
            obj.SetValue(OriginalLayoutValuesProperty, value);
        }

        internal static Storyboard GetLayoutStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(LayoutStoryboardProperty);
        }

        internal static void SetLayoutStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(LayoutStoryboardProperty, value);
        }

        internal static VisualState GetCurrentState(DependencyObject obj)
        {
            return (VisualState)obj.GetValue(CurrentStateProperty);
        }

        internal static void SetCurrentState(DependencyObject obj, VisualState value)
        {
            obj.SetValue(CurrentStateProperty, value);
        }

        public static TransitionEffect GetTransitionEffect(DependencyObject obj)
        {
            return (TransitionEffect)obj.GetValue(TransitionEffectProperty);
        }

        public static void SetTransitionEffect(DependencyObject obj, TransitionEffect value)
        {
            obj.SetValue(TransitionEffectProperty, value);
        }

        internal static Storyboard GetTransitionEffectStoryboard(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(TransitionEffectStoryboardProperty);
        }

        internal static void SetTransitionEffectStoryboard(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(TransitionEffectStoryboardProperty, value);
        }

        internal static bool GetDidCacheBackground(DependencyObject obj)
        {
            return (bool)obj.GetValue(DidCacheBackgroundProperty);
        }

        internal static void SetDidCacheBackground(DependencyObject obj, bool value)
        {
            obj.SetValue(DidCacheBackgroundProperty, value);
        }

        internal static object GetCachedBackground(DependencyObject obj)
        {
            return obj.GetValue(CachedBackgroundProperty);
        }

        internal static void SetCachedBackground(DependencyObject obj, object value)
        {
            obj.SetValue(CachedBackgroundProperty, value);
        }

        internal static Effect GetCachedEffect(DependencyObject obj)
        {
            return (Effect)obj.GetValue(CachedEffectProperty);
        }

        internal static void SetCachedEffect(DependencyObject obj, Effect value)
        {
            obj.SetValue(CachedEffectProperty, value);
        }

        private static bool IsVisibilityProperty(DependencyProperty property)
        {
            if (property != UIElement.VisibilityProperty)
            {
                return property.Name == "RuntimeVisibility";
            }
            return true;
        }

        private static DependencyProperty LayoutPropertyFromTimeline(Timeline timeline, bool forceRuntimeProperty)
        {
            PropertyPath targetProperty = Storyboard.GetTargetProperty(timeline);
            if (targetProperty == null || targetProperty.PathParameters == null || targetProperty.PathParameters.Count == 0)
            {
                return null;
            }
            DependencyProperty dependencyProperty = targetProperty.PathParameters[0] as DependencyProperty;
            if (dependencyProperty != null)
            {
                if (dependencyProperty.Name == "RuntimeVisibility" && dependencyProperty.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(dependencyProperty))
                    {
                        LayoutProperties.Add(dependencyProperty);
                    }
                    if (!forceRuntimeProperty)
                    {
                        return UIElement.VisibilityProperty;
                    }
                    return dependencyProperty;
                }
                if (dependencyProperty.Name == "RuntimeWidth" && dependencyProperty.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(dependencyProperty))
                    {
                        LayoutProperties.Add(dependencyProperty);
                    }
                    if (!forceRuntimeProperty)
                    {
                        return FrameworkElement.WidthProperty;
                    }
                    return dependencyProperty;
                }
                if (dependencyProperty.Name == "RuntimeHeight" && dependencyProperty.OwnerType.Name.EndsWith("DesignTimeProperties", StringComparison.Ordinal))
                {
                    if (!LayoutProperties.Contains(dependencyProperty))
                    {
                        LayoutProperties.Add(dependencyProperty);
                    }
                    if (!forceRuntimeProperty)
                    {
                        return FrameworkElement.HeightProperty;
                    }
                    return dependencyProperty;
                }
                if (LayoutProperties.Contains(dependencyProperty))
                {
                    return dependencyProperty;
                }
            }
            return null;
        }

        protected override bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            if (changingState)
            {
                return false;
            }
            if (group == null || state == null)
            {
                return false;
            }
            VisualState currentState = GetCurrentState(group);
            if (currentState == state)
            {
                return true;
            }
            VisualTransition transition = FindTransition(group, currentState, state);
            bool animateWithTransitionEffect = PrepareTransitionEffectImage(stateGroupsRoot, useTransitions, transition);
            if (!GetUseFluidLayout(group))
            {
                return TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions, transition, animateWithTransitionEffect, currentState);
            }
            Storyboard storyboard = ExtractLayoutStoryboard(state);
            List<OriginalLayoutValueRecord> list = GetOriginalLayoutValues(group);
            if (list == null)
            {
                list = new List<OriginalLayoutValueRecord>();
                SetOriginalLayoutValues(group, list);
            }
            if (!useTransitions)
            {
                if (LayoutTransitionStoryboard != null)
                {
                    StopAnimations();
                }
                bool result = TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions, transition, animateWithTransitionEffect, currentState);
                SetLayoutStoryboardProperties(control, stateGroupsRoot, storyboard, list);
                return result;
            }
            if (storyboard.Children.Count == 0 && list.Count == 0)
            {
                return TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions, transition, animateWithTransitionEffect, currentState);
            }
            try
            {
                changingState = true;
                stateGroupsRoot.UpdateLayout();
                List<FrameworkElement> list2 = FindTargetElements(control, stateGroupsRoot, storyboard, list, MovingElements);
                Dictionary<FrameworkElement, Rect> rectsOfTargets = GetRectsOfTargets(list2, MovingElements);
                Dictionary<FrameworkElement, double> oldOpacities = GetOldOpacities(control, stateGroupsRoot, storyboard, list, MovingElements);
                if (LayoutTransitionStoryboard != null)
                {
                    stateGroupsRoot.LayoutUpdated -= control_LayoutUpdated;
                    StopAnimations();
                    stateGroupsRoot.UpdateLayout();
                }
                TransitionEffectAwareGoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions, transition, animateWithTransitionEffect, currentState);
                SetLayoutStoryboardProperties(control, stateGroupsRoot, storyboard, list);
                stateGroupsRoot.UpdateLayout();
                Dictionary<FrameworkElement, Rect> rectsOfTargets2 = GetRectsOfTargets(list2, null);
                MovingElements = new List<FrameworkElement>();
                foreach (FrameworkElement item in list2)
                {
                    if (rectsOfTargets[item] != rectsOfTargets2[item])
                    {
                        MovingElements.Add(item);
                    }
                }
                foreach (FrameworkElement key in oldOpacities.Keys)
                {
                    if (!MovingElements.Contains(key))
                    {
                        MovingElements.Add(key);
                    }
                }
                WrapMovingElementsInCanvases(MovingElements, rectsOfTargets, rectsOfTargets2);
                stateGroupsRoot.LayoutUpdated += control_LayoutUpdated;
                LayoutTransitionStoryboard = CreateLayoutTransitionStoryboard(transition, MovingElements, oldOpacities);
                LayoutTransitionStoryboard.Completed += delegate
                {
                    stateGroupsRoot.LayoutUpdated -= control_LayoutUpdated;
                    StopAnimations();
                };
                LayoutTransitionStoryboard.Begin();
            }
            finally
            {
                changingState = false;
            }
            return true;
        }

        private static void control_LayoutUpdated(object sender, EventArgs e)
        {
            if (LayoutTransitionStoryboard != null)
            {
                foreach (FrameworkElement movingElement in MovingElements)
                {
                    WrapperCanvas wrapperCanvas = movingElement.Parent as WrapperCanvas;
                    if (wrapperCanvas != null)
                    {
                        Rect layoutRect = GetLayoutRect(wrapperCanvas);
                        Rect newRect = wrapperCanvas.NewRect;
                        TranslateTransform translateTransform = wrapperCanvas.RenderTransform as TranslateTransform;
                        double num = (translateTransform == null) ? 0.0 : translateTransform.X;
                        double num2 = (translateTransform == null) ? 0.0 : translateTransform.Y;
                        double num3 = newRect.Left - layoutRect.Left;
                        double num4 = newRect.Top - layoutRect.Top;
                        if (num != num3 || num2 != num4)
                        {
                            if (translateTransform == null)
                            {
                                translateTransform = (TranslateTransform)(wrapperCanvas.RenderTransform = new TranslateTransform());
                            }
                            translateTransform.X = num3;
                            translateTransform.Y = num4;
                        }
                    }
                }
            }
        }

        private static void StopAnimations()
        {
            if (LayoutTransitionStoryboard != null)
            {
                LayoutTransitionStoryboard.Stop();
                LayoutTransitionStoryboard = null;
            }
            if (MovingElements != null)
            {
                UnwrapMovingElementsFromCanvases(MovingElements);
                MovingElements = null;
            }
        }

        private static bool PrepareTransitionEffectImage(FrameworkElement stateGroupsRoot, bool useTransitions, VisualTransition transition)
        {
            TransitionEffect transitionEffect = (transition == null) ? null : GetTransitionEffect(transition);
            bool result = false;
            if (transitionEffect != null)
            {
                transitionEffect = transitionEffect.CloneCurrentValue();
                if (useTransitions)
                {
                    result = true;
                    int pixelWidth = (int)Math.Max(1.0, stateGroupsRoot.ActualWidth);
                    int pixelHeight = (int)Math.Max(1.0, stateGroupsRoot.ActualHeight);
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                    renderTargetBitmap.Render(stateGroupsRoot);
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = renderTargetBitmap;
                    transitionEffect.OldImage = imageBrush;
                }
                Storyboard transitionEffectStoryboard = GetTransitionEffectStoryboard(stateGroupsRoot);
                if (transitionEffectStoryboard != null)
                {
                    transitionEffectStoryboard.Stop();
                    FinishTransitionEffectAnimation(stateGroupsRoot);
                }
                if (useTransitions)
                {
                    TransferLocalValue(stateGroupsRoot, UIElement.EffectProperty, CachedEffectProperty);
                    stateGroupsRoot.Effect = transitionEffect;
                }
            }
            return result;
        }

        private bool TransitionEffectAwareGoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions, VisualTransition transition, bool animateWithTransitionEffect, VisualState previousState)
        {
            IEasingFunction generatedEasingFunction = null;
            if (animateWithTransitionEffect)
            {
                generatedEasingFunction = transition.GeneratedEasingFunction;
                transition.GeneratedEasingFunction = new DummyEasingFunction
                {
                    DummyValue = (FinishesWithZeroOpacity(control, stateGroupsRoot, state, previousState) ? 0.01 : 0.0)
                };
            }
            bool flag = base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
            if (animateWithTransitionEffect)
            {
                transition.GeneratedEasingFunction = generatedEasingFunction;
                if (flag)
                {
                    AnimateTransitionEffect(stateGroupsRoot, transition);
                }
            }
            SetCurrentState(group, state);
            return flag;
        }

        private static bool FinishesWithZeroOpacity(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualState state, VisualState previousState)
        {
            if (state.Storyboard != null)
            {
                foreach (Timeline child in state.Storyboard.Children)
                {
                    if (TimelineIsAnimatingRootOpacity(child, control, stateGroupsRoot))
                    {
                        bool gotValue;
                        object valueFromTimeline = GetValueFromTimeline(child, out gotValue);
                        return gotValue && valueFromTimeline is double && (double)valueFromTimeline == 0.0;
                    }
                }
            }
            if (previousState != null && previousState.Storyboard != null)
            {
                foreach (Timeline child2 in previousState.Storyboard.Children)
                {
                    TimelineIsAnimatingRootOpacity(child2, control, stateGroupsRoot);
                }
                double num = (double)stateGroupsRoot.GetAnimationBaseValue(UIElement.OpacityProperty);
                return num == 0.0;
            }
            return stateGroupsRoot.Opacity == 0.0;
        }

        private static bool TimelineIsAnimatingRootOpacity(Timeline timeline, FrameworkElement control, FrameworkElement stateGroupsRoot)
        {
            if (GetTimelineTarget(control, stateGroupsRoot, timeline) != stateGroupsRoot)
            {
                return false;
            }
            PropertyPath targetProperty = Storyboard.GetTargetProperty(timeline);
            if (targetProperty != null && targetProperty.PathParameters != null && targetProperty.PathParameters.Count != 0)
            {
                return targetProperty.PathParameters[0] == UIElement.OpacityProperty;
            }
            return false;
        }

        private static void AnimateTransitionEffect(FrameworkElement stateGroupsRoot, VisualTransition transition)
        {
            Effect effect = stateGroupsRoot.Effect;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = transition.GeneratedDuration;
            doubleAnimation.EasingFunction = transition.GeneratedEasingFunction;
            doubleAnimation.From = 0.0;
            doubleAnimation.To = 1.0;
            Storyboard sb = new Storyboard();
            sb.Duration = transition.GeneratedDuration;
            sb.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, stateGroupsRoot);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(0).(1)", UIElement.EffectProperty, TransitionEffect.ProgressProperty));
            Panel panel = stateGroupsRoot as Panel;
            if (panel != null && panel.Background == null)
            {
                SetDidCacheBackground(panel, true);
                TransferLocalValue(panel, Panel.BackgroundProperty, CachedBackgroundProperty);
                panel.Background = Brushes.Transparent;
            }
            sb.Completed += delegate
            {
                Storyboard transitionEffectStoryboard = GetTransitionEffectStoryboard(stateGroupsRoot);
                if (transitionEffectStoryboard == sb)
                {
                    FinishTransitionEffectAnimation(stateGroupsRoot);
                }
            };
            SetTransitionEffectStoryboard(stateGroupsRoot, sb);
            sb.Begin();
        }

        private static void FinishTransitionEffectAnimation(FrameworkElement stateGroupsRoot)
        {
            SetTransitionEffectStoryboard(stateGroupsRoot, null);
            TransferLocalValue(stateGroupsRoot, CachedEffectProperty, UIElement.EffectProperty);
            if (GetDidCacheBackground(stateGroupsRoot))
            {
                TransferLocalValue(stateGroupsRoot, CachedBackgroundProperty, Panel.BackgroundProperty);
                SetDidCacheBackground(stateGroupsRoot, false);
            }
        }

        private static VisualTransition FindTransition(VisualStateGroup group, VisualState previousState, VisualState state)
        {
            string b = (previousState != null) ? previousState.Name : string.Empty;
            string b2 = (state != null) ? state.Name : string.Empty;
            int num = -1;
            VisualTransition result = null;
            if (group.Transitions != null)
            {
                foreach (VisualTransition transition in group.Transitions)
                {
                    int num2 = 0;
                    if (transition.From == b)
                    {
                        num2++;
                    }
                    else if (!string.IsNullOrEmpty(transition.From))
                    {
                        continue;
                    }
                    if (transition.To == b2)
                    {
                        num2 += 2;
                    }
                    else if (!string.IsNullOrEmpty(transition.To))
                    {
                        continue;
                    }
                    if (num2 > num)
                    {
                        num = num2;
                        result = transition;
                    }
                }
                return result;
            }
            return result;
        }

        private static Storyboard ExtractLayoutStoryboard(VisualState state)
        {
            Storyboard storyboard = null;
            if (state.Storyboard != null)
            {
                storyboard = GetLayoutStoryboard(state.Storyboard);
                if (storyboard == null)
                {
                    storyboard = new Storyboard();
                    for (int num = state.Storyboard.Children.Count - 1; num >= 0; num--)
                    {
                        Timeline timeline = state.Storyboard.Children[num];
                        if (LayoutPropertyFromTimeline(timeline, false) != null)
                        {
                            state.Storyboard.Children.RemoveAt(num);
                            storyboard.Children.Add(timeline);
                        }
                    }
                    SetLayoutStoryboard(state.Storyboard, storyboard);
                }
            }
            if (storyboard == null)
            {
                return new Storyboard();
            }
            return storyboard;
        }

        private static List<FrameworkElement> FindTargetElements(FrameworkElement control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
        {
            List<FrameworkElement> list = new List<FrameworkElement>();
            if (movingElements != null)
            {
                list.AddRange(movingElements);
            }
            foreach (Timeline child in layoutStoryboard.Children)
            {
                FrameworkElement frameworkElement = (FrameworkElement)GetTimelineTarget(control, templateRoot, child);
                if (frameworkElement != null)
                {
                    if (!list.Contains(frameworkElement))
                    {
                        list.Add(frameworkElement);
                    }
                    if (ChildAffectingLayoutProperties.Contains(LayoutPropertyFromTimeline(child, false)))
                    {
                        Panel panel = frameworkElement as Panel;
                        if (panel != null)
                        {
                            foreach (FrameworkElement child2 in panel.Children)
                            {
                                if (!list.Contains(child2) && !(child2 is WrapperCanvas))
                                {
                                    list.Add(child2);
                                }
                            }
                        }
                    }
                }
            }
            foreach (OriginalLayoutValueRecord originalValueRecord in originalValueRecords)
            {
                if (!list.Contains(originalValueRecord.Element))
                {
                    list.Add(originalValueRecord.Element);
                }
                if (ChildAffectingLayoutProperties.Contains(originalValueRecord.Property))
                {
                    Panel panel2 = originalValueRecord.Element as Panel;
                    if (panel2 != null)
                    {
                        foreach (FrameworkElement child3 in panel2.Children)
                        {
                            if (!list.Contains(child3) && !(child3 is WrapperCanvas))
                            {
                                list.Add(child3);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                FrameworkElement frameworkElement4 = list[i];
                FrameworkElement frameworkElement5 = VisualTreeHelper.GetParent(frameworkElement4) as FrameworkElement;
                if (movingElements != null && movingElements.Contains(frameworkElement4) && frameworkElement5 is WrapperCanvas)
                {
                    frameworkElement5 = (VisualTreeHelper.GetParent(frameworkElement5) as FrameworkElement);
                }
                if (frameworkElement5 != null)
                {
                    if (!list.Contains(frameworkElement5))
                    {
                        list.Add(frameworkElement5);
                    }
                    for (int j = 0; j < VisualTreeHelper.GetChildrenCount(frameworkElement5); j++)
                    {
                        FrameworkElement frameworkElement6 = VisualTreeHelper.GetChild(frameworkElement5, j) as FrameworkElement;
                        if (frameworkElement6 != null && !list.Contains(frameworkElement6) && !(frameworkElement6 is WrapperCanvas))
                        {
                            list.Add(frameworkElement6);
                        }
                    }
                }
            }
            return list;
        }

        private static object GetTimelineTarget(FrameworkElement control, FrameworkElement templateRoot, Timeline timeline)
        {
            string targetName = Storyboard.GetTargetName(timeline);
            if (string.IsNullOrEmpty(targetName))
            {
                return null;
            }
            if (control is UserControl)
            {
                return control.FindName(targetName);
            }
            return templateRoot.FindName(targetName);
        }

        private static Dictionary<FrameworkElement, Rect> GetRectsOfTargets(List<FrameworkElement> targets, List<FrameworkElement> movingElements)
        {
            Dictionary<FrameworkElement, Rect> dictionary = new Dictionary<FrameworkElement, Rect>();
            foreach (FrameworkElement target in targets)
            {
                Rect value;
                if (movingElements != null && movingElements.Contains(target) && target.Parent is WrapperCanvas)
                {
                    WrapperCanvas wrapperCanvas = target.Parent as WrapperCanvas;
                    value = GetLayoutRect(wrapperCanvas);
                    TranslateTransform translateTransform = wrapperCanvas.RenderTransform as TranslateTransform;
                    double left = Canvas.GetLeft(target);
                    double top = Canvas.GetTop(target);
                    value = new Rect(value.Left + (double.IsNaN(left) ? 0.0 : left) + ((translateTransform == null) ? 0.0 : translateTransform.X), value.Top + (double.IsNaN(top) ? 0.0 : top) + ((translateTransform == null) ? 0.0 : translateTransform.Y), target.ActualWidth, target.ActualHeight);
                }
                else
                {
                    value = GetLayoutRect(target);
                }
                dictionary.Add(target, value);
            }
            return dictionary;
        }

        internal static Rect GetLayoutRect(FrameworkElement element)
        {
            double num = element.ActualWidth;
            double num2 = element.ActualHeight;
            if (element is Image || element is MediaElement)
            {
                if (element.Parent.GetType() == typeof(Canvas))
                {
                    num = (double.IsNaN(element.Width) ? num : element.Width);
                    num2 = (double.IsNaN(element.Height) ? num2 : element.Height);
                }
                else
                {
                    num = element.RenderSize.Width;
                    num2 = element.RenderSize.Height;
                }
            }
            num = ((element.Visibility == Visibility.Collapsed) ? 0.0 : num);
            num2 = ((element.Visibility == Visibility.Collapsed) ? 0.0 : num2);
            Thickness margin = element.Margin;
            Rect layoutSlot = LayoutInformation.GetLayoutSlot(element);
            double x = 0.0;
            double y = 0.0;
            switch (element.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    x = layoutSlot.Left + margin.Left;
                    break;
                case HorizontalAlignment.Center:
                    x = (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - num / 2.0;
                    break;
                case HorizontalAlignment.Right:
                    x = layoutSlot.Right - margin.Right - num;
                    break;
                case HorizontalAlignment.Stretch:
                    x = Math.Max(layoutSlot.Left + margin.Left, (layoutSlot.Left + margin.Left + layoutSlot.Right - margin.Right) / 2.0 - num / 2.0);
                    break;
            }
            switch (element.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    y = layoutSlot.Top + margin.Top;
                    break;
                case VerticalAlignment.Center:
                    y = (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - num2 / 2.0;
                    break;
                case VerticalAlignment.Bottom:
                    y = layoutSlot.Bottom - margin.Bottom - num2;
                    break;
                case VerticalAlignment.Stretch:
                    y = Math.Max(layoutSlot.Top + margin.Top, (layoutSlot.Top + margin.Top + layoutSlot.Bottom - margin.Bottom) / 2.0 - num2 / 2.0);
                    break;
            }
            return new Rect(x, y, num, num2);
        }

        private static Dictionary<FrameworkElement, double> GetOldOpacities(FrameworkElement control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<OriginalLayoutValueRecord> originalValueRecords, List<FrameworkElement> movingElements)
        {
            Dictionary<FrameworkElement, double> dictionary = new Dictionary<FrameworkElement, double>();
            if (movingElements != null)
            {
                foreach (FrameworkElement movingElement in movingElements)
                {
                    WrapperCanvas wrapperCanvas = movingElement.Parent as WrapperCanvas;
                    if (wrapperCanvas != null)
                    {
                        dictionary.Add(movingElement, wrapperCanvas.Opacity);
                    }
                }
            }
            for (int num = originalValueRecords.Count - 1; num >= 0; num--)
            {
                OriginalLayoutValueRecord originalLayoutValueRecord = originalValueRecords[num];
                double value;
                if (IsVisibilityProperty(originalLayoutValueRecord.Property) && !dictionary.TryGetValue(originalLayoutValueRecord.Element, out value))
                {
                    value = (((Visibility)originalLayoutValueRecord.Element.GetValue(originalLayoutValueRecord.Property) == Visibility.Visible) ? 1.0 : 0.0);
                    dictionary.Add(originalLayoutValueRecord.Element, value);
                }
            }
            foreach (Timeline child in layoutStoryboard.Children)
            {
                FrameworkElement frameworkElement = (FrameworkElement)GetTimelineTarget(control, templateRoot, child);
                DependencyProperty dependencyProperty = LayoutPropertyFromTimeline(child, true);
                double value2;
                if (frameworkElement != null && IsVisibilityProperty(dependencyProperty) && !dictionary.TryGetValue(frameworkElement, out value2))
                {
                    value2 = (((Visibility)frameworkElement.GetValue(dependencyProperty) == Visibility.Visible) ? 1.0 : 0.0);
                    dictionary.Add(frameworkElement, value2);
                }
            }
            return dictionary;
        }

        private static void SetLayoutStoryboardProperties(FrameworkElement control, FrameworkElement templateRoot, Storyboard layoutStoryboard, List<OriginalLayoutValueRecord> originalValueRecords)
        {
            foreach (OriginalLayoutValueRecord originalValueRecord in originalValueRecords)
            {
                ReplaceCachedLocalValueHelper(originalValueRecord.Element, originalValueRecord.Property, originalValueRecord.Value);
            }
            originalValueRecords.Clear();
            foreach (Timeline child in layoutStoryboard.Children)
            {
                FrameworkElement frameworkElement = (FrameworkElement)GetTimelineTarget(control, templateRoot, child);
                DependencyProperty dependencyProperty = LayoutPropertyFromTimeline(child, true);
                if (frameworkElement != null && dependencyProperty != null)
                {
                    bool gotValue;
                    object valueFromTimeline = GetValueFromTimeline(child, out gotValue);
                    if (gotValue)
                    {
                        originalValueRecords.Add(new OriginalLayoutValueRecord
                        {
                            Element = frameworkElement,
                            Property = dependencyProperty,
                            Value = CacheLocalValueHelper(frameworkElement, dependencyProperty)
                        });
                        frameworkElement.SetValue(dependencyProperty, valueFromTimeline);
                    }
                }
            }
        }

        private static object GetValueFromTimeline(Timeline timeline, out bool gotValue)
        {
            ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = timeline as ObjectAnimationUsingKeyFrames;
            if (objectAnimationUsingKeyFrames != null)
            {
                gotValue = true;
                return objectAnimationUsingKeyFrames.KeyFrames[0].Value;
            }
            DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = timeline as DoubleAnimationUsingKeyFrames;
            if (doubleAnimationUsingKeyFrames != null)
            {
                gotValue = true;
                return doubleAnimationUsingKeyFrames.KeyFrames[0].Value;
            }
            DoubleAnimation doubleAnimation = timeline as DoubleAnimation;
            if (doubleAnimation != null)
            {
                gotValue = true;
                return doubleAnimation.To;
            }
            ThicknessAnimationUsingKeyFrames thicknessAnimationUsingKeyFrames = timeline as ThicknessAnimationUsingKeyFrames;
            if (thicknessAnimationUsingKeyFrames != null)
            {
                gotValue = true;
                return thicknessAnimationUsingKeyFrames.KeyFrames[0].Value;
            }
            ThicknessAnimation thicknessAnimation = timeline as ThicknessAnimation;
            if (thicknessAnimation != null)
            {
                gotValue = true;
                return thicknessAnimation.To;
            }
            Int32AnimationUsingKeyFrames int32AnimationUsingKeyFrames = timeline as Int32AnimationUsingKeyFrames;
            if (int32AnimationUsingKeyFrames != null)
            {
                gotValue = true;
                return int32AnimationUsingKeyFrames.KeyFrames[0].Value;
            }
            Int32Animation int32Animation = timeline as Int32Animation;
            if (int32Animation != null)
            {
                gotValue = true;
                return int32Animation.To;
            }
            gotValue = false;
            return null;
        }

        private static void WrapMovingElementsInCanvases(List<FrameworkElement> movingElements, Dictionary<FrameworkElement, Rect> oldRects, Dictionary<FrameworkElement, Rect> newRects)
        {
            foreach (FrameworkElement movingElement in movingElements)
            {
                FrameworkElement frameworkElement = VisualTreeHelper.GetParent(movingElement) as FrameworkElement;
                WrapperCanvas wrapperCanvas = new WrapperCanvas();
                wrapperCanvas.OldRect = oldRects[movingElement];
                wrapperCanvas.NewRect = newRects[movingElement];
                object value = CacheLocalValueHelper(movingElement, FrameworkElement.DataContextProperty);
                movingElement.DataContext = movingElement.DataContext;
                bool flag = true;
                Panel panel = frameworkElement as Panel;
                if (panel != null && !panel.IsItemsHost)
                {
                    int index = panel.Children.IndexOf(movingElement);
                    panel.Children.RemoveAt(index);
                    panel.Children.Insert(index, wrapperCanvas);
                }
                else
                {
                    Decorator decorator = frameworkElement as Decorator;
                    if (decorator != null)
                    {
                        decorator.Child = wrapperCanvas;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    wrapperCanvas.Children.Add(movingElement);
                    CopyLayoutProperties(movingElement, wrapperCanvas, false);
                    ReplaceCachedLocalValueHelper(movingElement, FrameworkElement.DataContextProperty, value);
                }
            }
        }

        private static void UnwrapMovingElementsFromCanvases(List<FrameworkElement> movingElements)
        {
            foreach (FrameworkElement movingElement in movingElements)
            {
                WrapperCanvas wrapperCanvas = movingElement.Parent as WrapperCanvas;
                if (wrapperCanvas != null)
                {
                    object value = CacheLocalValueHelper(movingElement, FrameworkElement.DataContextProperty);
                    movingElement.DataContext = movingElement.DataContext;
                    FrameworkElement frameworkElement = VisualTreeHelper.GetParent(wrapperCanvas) as FrameworkElement;
                    wrapperCanvas.Children.Remove(movingElement);
                    Panel panel = frameworkElement as Panel;
                    if (panel != null)
                    {
                        int index = panel.Children.IndexOf(wrapperCanvas);
                        panel.Children.RemoveAt(index);
                        panel.Children.Insert(index, movingElement);
                    }
                    else
                    {
                        Decorator decorator = frameworkElement as Decorator;
                        if (decorator != null)
                        {
                            decorator.Child = movingElement;
                        }
                    }
                    CopyLayoutProperties(wrapperCanvas, movingElement, true);
                    ReplaceCachedLocalValueHelper(movingElement, FrameworkElement.DataContextProperty, value);
                }
            }
        }

        private static void CopyLayoutProperties(FrameworkElement source, FrameworkElement target, bool restoring)
        {
            WrapperCanvas wrapperCanvas = (restoring ? source : target) as WrapperCanvas;
            if (wrapperCanvas.LocalValueCache == null)
            {
                wrapperCanvas.LocalValueCache = new Dictionary<DependencyProperty, object>();
            }
            foreach (DependencyProperty layoutProperty in LayoutProperties)
            {
                if (!ChildAffectingLayoutProperties.Contains(layoutProperty))
                {
                    if (restoring)
                    {
                        ReplaceCachedLocalValueHelper(target, layoutProperty, wrapperCanvas.LocalValueCache[layoutProperty]);
                    }
                    else
                    {
                        object value = target.GetValue(layoutProperty);
                        object value2 = CacheLocalValueHelper(source, layoutProperty);
                        wrapperCanvas.LocalValueCache[layoutProperty] = value2;
                        if (IsVisibilityProperty(layoutProperty))
                        {
                            wrapperCanvas.DestinationVisibilityCache = (Visibility)source.GetValue(layoutProperty);
                        }
                        else
                        {
                            target.SetValue(layoutProperty, source.GetValue(layoutProperty));
                        }
                        source.SetValue(layoutProperty, value);
                    }
                }
            }
        }

        private static Storyboard CreateLayoutTransitionStoryboard(VisualTransition transition, List<FrameworkElement> movingElements, Dictionary<FrameworkElement, double> oldOpacities)
        {
            Duration duration = (transition != null) ? transition.GeneratedDuration : new Duration(TimeSpan.Zero);
            IEasingFunction easingFunction = (transition != null) ? transition.GeneratedEasingFunction : null;
            Storyboard storyboard = new Storyboard();
            storyboard.Duration = duration;
            foreach (FrameworkElement movingElement in movingElements)
            {
                WrapperCanvas wrapperCanvas = movingElement.Parent as WrapperCanvas;
                if (wrapperCanvas != null)
                {
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = 1.0;
                    doubleAnimation.To = 0.0;
                    doubleAnimation.Duration = duration;
                    DoubleAnimation doubleAnimation2 = doubleAnimation;
                    doubleAnimation2.EasingFunction = easingFunction;
                    Storyboard.SetTarget(doubleAnimation2, wrapperCanvas);
                    Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(WrapperCanvas.SimulationProgressProperty));
                    storyboard.Children.Add(doubleAnimation2);
                    wrapperCanvas.SimulationProgress = 1.0;
                    Rect newRect = wrapperCanvas.NewRect;
                    if (!IsClose(wrapperCanvas.Width, newRect.Width))
                    {
                        DoubleAnimation doubleAnimation3 = new DoubleAnimation();
                        doubleAnimation3.From = newRect.Width;
                        doubleAnimation3.To = newRect.Width;
                        doubleAnimation3.Duration = duration;
                        DoubleAnimation doubleAnimation4 = doubleAnimation3;
                        Storyboard.SetTarget(doubleAnimation4, wrapperCanvas);
                        Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath(FrameworkElement.WidthProperty));
                        storyboard.Children.Add(doubleAnimation4);
                    }
                    if (!IsClose(wrapperCanvas.Height, newRect.Height))
                    {
                        DoubleAnimation doubleAnimation5 = new DoubleAnimation();
                        doubleAnimation5.From = newRect.Height;
                        doubleAnimation5.To = newRect.Height;
                        doubleAnimation5.Duration = duration;
                        DoubleAnimation doubleAnimation6 = doubleAnimation5;
                        Storyboard.SetTarget(doubleAnimation6, wrapperCanvas);
                        Storyboard.SetTargetProperty(doubleAnimation6, new PropertyPath(FrameworkElement.HeightProperty));
                        storyboard.Children.Add(doubleAnimation6);
                    }
                    if (wrapperCanvas.DestinationVisibilityCache == Visibility.Collapsed)
                    {
                        Thickness margin = wrapperCanvas.Margin;
                        if (!IsClose(margin.Left, 0.0) || !IsClose(margin.Top, 0.0) || !IsClose(margin.Right, 0.0) || !IsClose(margin.Bottom, 0.0))
                        {
                            ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames = new ObjectAnimationUsingKeyFrames();
                            objectAnimationUsingKeyFrames.Duration = duration;
                            ObjectAnimationUsingKeyFrames objectAnimationUsingKeyFrames2 = objectAnimationUsingKeyFrames;
                            DiscreteObjectKeyFrame discreteObjectKeyFrame = new DiscreteObjectKeyFrame();
                            discreteObjectKeyFrame.KeyTime = TimeSpan.Zero;
                            discreteObjectKeyFrame.Value = default(Thickness);
                            DiscreteObjectKeyFrame keyFrame = discreteObjectKeyFrame;
                            objectAnimationUsingKeyFrames2.KeyFrames.Add(keyFrame);
                            Storyboard.SetTarget(objectAnimationUsingKeyFrames2, wrapperCanvas);
                            Storyboard.SetTargetProperty(objectAnimationUsingKeyFrames2, new PropertyPath(FrameworkElement.MarginProperty));
                            storyboard.Children.Add(objectAnimationUsingKeyFrames2);
                        }
                        if (!IsClose(wrapperCanvas.MinWidth, 0.0))
                        {
                            DoubleAnimation doubleAnimation7 = new DoubleAnimation();
                            doubleAnimation7.From = 0.0;
                            doubleAnimation7.To = 0.0;
                            doubleAnimation7.Duration = duration;
                            DoubleAnimation doubleAnimation8 = doubleAnimation7;
                            Storyboard.SetTarget(doubleAnimation8, wrapperCanvas);
                            Storyboard.SetTargetProperty(doubleAnimation8, new PropertyPath(FrameworkElement.MinWidthProperty));
                            storyboard.Children.Add(doubleAnimation8);
                        }
                        if (!IsClose(wrapperCanvas.MinHeight, 0.0))
                        {
                            DoubleAnimation doubleAnimation9 = new DoubleAnimation();
                            doubleAnimation9.From = 0.0;
                            doubleAnimation9.To = 0.0;
                            doubleAnimation9.Duration = duration;
                            DoubleAnimation doubleAnimation10 = doubleAnimation9;
                            Storyboard.SetTarget(doubleAnimation10, wrapperCanvas);
                            Storyboard.SetTargetProperty(doubleAnimation10, new PropertyPath(FrameworkElement.MinHeightProperty));
                            storyboard.Children.Add(doubleAnimation10);
                        }
                    }
                }
            }
            foreach (FrameworkElement key in oldOpacities.Keys)
            {
                WrapperCanvas wrapperCanvas2 = key.Parent as WrapperCanvas;
                if (wrapperCanvas2 != null)
                {
                    double num = oldOpacities[key];
                    double num2 = (wrapperCanvas2.DestinationVisibilityCache == Visibility.Visible) ? 1.0 : 0.0;
                    if (!IsClose(num, 1.0) || !IsClose(num2, 1.0))
                    {
                        DoubleAnimation doubleAnimation11 = new DoubleAnimation();
                        doubleAnimation11.From = num;
                        doubleAnimation11.To = num2;
                        doubleAnimation11.Duration = duration;
                        DoubleAnimation doubleAnimation12 = doubleAnimation11;
                        doubleAnimation12.EasingFunction = easingFunction;
                        Storyboard.SetTarget(doubleAnimation12, wrapperCanvas2);
                        Storyboard.SetTargetProperty(doubleAnimation12, new PropertyPath(UIElement.OpacityProperty));
                        storyboard.Children.Add(doubleAnimation12);
                    }
                }
            }
            return storyboard;
        }

        private static void TransferLocalValue(FrameworkElement element, DependencyProperty sourceProperty, DependencyProperty destProperty)
        {
            object value = CacheLocalValueHelper(element, sourceProperty);
            ReplaceCachedLocalValueHelper(element, destProperty, value);
        }

        private static object CacheLocalValueHelper(DependencyObject dependencyObject, DependencyProperty property)
        {
            return dependencyObject.ReadLocalValue(property);
        }

        private static void ReplaceCachedLocalValueHelper(FrameworkElement element, DependencyProperty property, object value)
        {
            if (value == DependencyProperty.UnsetValue)
            {
                element.ClearValue(property);
            }
            else
            {
                BindingExpressionBase bindingExpressionBase = value as BindingExpressionBase;
                if (bindingExpressionBase != null)
                {
                    element.SetBinding(property, bindingExpressionBase.ParentBindingBase);
                }
                else
                {
                    element.SetValue(property, value);
                }
            }
        }

        private static bool IsClose(double a, double b)
        {
            return Math.Abs(a - b) < 1E-07;
        }
    }
}
