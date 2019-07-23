using System;
using System.Globalization;
using System.Windows;
using System.Windows.Interactivity;

namespace ay.contentcore
{
    public class GoToStateAction : TargetedTriggerAction<FrameworkElement>
    {
        public static readonly DependencyProperty UseTransitionsProperty = DependencyProperty.Register("UseTransitions", typeof(bool), typeof(GoToStateAction), new PropertyMetadata(true));

        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register("StateName", typeof(string), typeof(GoToStateAction), new PropertyMetadata(string.Empty));

        public bool UseTransitions
        {
            get
            {
                return (bool)GetValue(UseTransitionsProperty);
            }
            set
            {
                SetValue(UseTransitionsProperty, value);
            }
        }

        public string StateName
        {
            get
            {
                return (string)GetValue(StateNameProperty);
            }
            set
            {
                SetValue(StateNameProperty, value);
            }
        }

        private FrameworkElement StateTarget
        {
            get;
            set;
        }

        private bool IsTargetObjectSet
        {
            get
            {
                return ReadLocalValue(TargetedTriggerAction.TargetObjectProperty) != DependencyProperty.UnsetValue;
            }
        }

        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);
            FrameworkElement resolvedControl = null;
            if (string.IsNullOrEmpty(base.TargetName) && !IsTargetObjectSet)
            {
                if (!VisualStateUtilities.TryFindNearestStatefulControl(base.AssociatedObject as FrameworkElement, out resolvedControl) && resolvedControl != null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "GoToStateActionTargetHasNoStateGroups", new object[1]
                    {
                        resolvedControl.Name
                    }));
                }
            }
            else
            {
                resolvedControl = base.Target;
            }
            StateTarget = resolvedControl;
        }

        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject != null)
            {
                InvokeImpl(StateTarget);
            }
        }

        internal void InvokeImpl(FrameworkElement stateTarget)
        {
            if (stateTarget != null)
            {
                VisualStateUtilities.GoToState(stateTarget, StateName, UseTransitions);
            }
        }
    }
}
