using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ay.contentcore
{
    public class PropertyChangedTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register("Binding", typeof(object), typeof(PropertyChangedTrigger), new PropertyMetadata(OnBindingChanged));

        public object Binding
        {
            get
            {
                return GetValue(BindingProperty);
            }
            set
            {
                SetValue(BindingProperty, value);
            }
        }

        protected virtual void EvaluateBindingChange(object args)
        {
            InvokeActions(args);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            base.PreviewInvoke += OnPreviewInvoke;
        }

        protected override void OnDetaching()
        {
            base.PreviewInvoke -= OnPreviewInvoke;
            OnDetaching();
        }

        private void OnPreviewInvoke(object sender, PreviewInvokeEventArgs e)
        {
            DataBindingHelper.EnsureDataBindingOnActionsUpToDate(this);
        }

        private static void OnBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PropertyChangedTrigger propertyChangedTrigger = (PropertyChangedTrigger)sender;
            propertyChangedTrigger.EvaluateBindingChange(args);
        }
    }

    internal static class DataBindingHelper
    {
        private static Dictionary<Type, IList<DependencyProperty>> DependenciesPropertyCache = new Dictionary<Type, IList<DependencyProperty>>();

        public static void EnsureDataBindingUpToDateOnMembers(DependencyObject dpObject)
        {
            IList<DependencyProperty> value = null;
            if (!DependenciesPropertyCache.TryGetValue(dpObject.GetType(), out value))
            {
                value = new List<DependencyProperty>();
                Type type = dpObject.GetType();
                while (type != null)
                {
                    FieldInfo[] fields = type.GetFields();
                    FieldInfo[] array = fields;
                    foreach (FieldInfo fieldInfo in array)
                    {
                        if (fieldInfo.IsPublic && fieldInfo.FieldType == typeof(DependencyProperty))
                        {
                            DependencyProperty dependencyProperty = fieldInfo.GetValue(null) as DependencyProperty;
                            if (dependencyProperty != null)
                            {
                                value.Add(dependencyProperty);
                            }
                        }
                    }
                    type = type.BaseType;
                }
                DependenciesPropertyCache[dpObject.GetType()] = value;
            }
            if (value != null)
            {
                foreach (DependencyProperty item in value)
                {
                    EnsureBindingUpToDate(dpObject, item);
                }
            }
        }

        public static void EnsureDataBindingOnActionsUpToDate(TriggerBase<DependencyObject> trigger)
        {
            foreach (System.Windows.Interactivity.TriggerAction action in trigger.Actions)
            {
                EnsureDataBindingUpToDateOnMembers(action);
            }
        }

        public static void EnsureBindingUpToDate(DependencyObject target, DependencyProperty dp)
        {
            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(target, dp);
            if (bindingExpression != null)
            {
                bindingExpression.UpdateTarget();
            }
        }
    }
}
