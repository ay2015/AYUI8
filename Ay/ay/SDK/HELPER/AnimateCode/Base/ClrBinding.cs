
using System;
using System.Windows;
using System.Windows.Data;

namespace ay.Animate
{
    public class ClrBinding : DependencyObject
    {
        private readonly object _owner;
        private readonly DependencyProperty _attachedProperty;
        private readonly Action<object, object> _valueChangeCallback;

        public ClrBinding(object owner, DependencyProperty attachedProperty,
            Action<object, object> valueChangeCallback = null)
        {
            _owner = owner;
            _attachedProperty = attachedProperty;
            _valueChangeCallback = valueChangeCallback;
        }

        public object GetValue()
        {
            return GetValue(_attachedProperty);
        }

        public void SetValue(object value)
        {
            if (value is Binding binding)
            {
                BindingOperations.SetBinding(this, _attachedProperty, binding);
            }
            else
            {
                SetValue(_attachedProperty, value);
            }
        }

        public static void ValueChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ClrBinding)d)._valueChangeCallback?.Invoke(e.OldValue, e.NewValue);
        }
    }

}
