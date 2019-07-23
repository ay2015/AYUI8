
using ay.Animate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ay.Controls
{

   
    public class ObjectReference : MarkupExtension
    {

        public static string GetKKK(DependencyObject obj)
        {
            return (string)obj.GetValue(KKKProperty);
        }

        public static void SetKKK(DependencyObject obj, string value)
        {
            obj.SetValue(KKKProperty, value);
        }
        private static void ddddddd(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement ttt)
            {
                References[e.NewValue] = new WeakReference(ttt);
            }
        }

        // Using a DependencyProperty as the backing store for KKK.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KKKProperty =
            DependencyProperty.RegisterAttached("KKK", typeof(string), typeof(FrameworkElement), new PropertyMetadata(null, new PropertyChangedCallback(ddddddd)));

        public ObjectReference()
        {
        }

        public ObjectReference(object key)
        {
            _key = key;
        }






        public ObjectReference(object key, bool isDeclaration)
        {
            _key = key;
            _isDeclaration = isDeclaration;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object result = _key;

            // ensure that a key was specified
            if (_key == null)
            {
                throw new InvalidOperationException("The Key has not been specified for the ObjectReference.");
            }

            // determine whether this is a declaration or a reference
            bool isDeclaration = false;
            if (serviceProvider != null)
            {
                IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
                if (provideValueTarget != null)
                {
                    // treat the reference as a declaration if the IsDeclaration property is true
                    // or if its being set on the Declaration attached property
                    isDeclaration = _isDeclaration
                        || (provideValueTarget.TargetProperty == ObjectReference.DeclarationProperty);
                    if (isDeclaration)
                    {
                        References[_key] = new WeakReference(provideValueTarget.TargetObject);
                    }
                }
            }

            if (!isDeclaration)
            {
                WeakReference targetReference;
                if (References.TryGetValue(_key, out targetReference))
                {
                    result = targetReference.Target;
                }
            }

            return result;
        }

        #region Declaration Attached Property

        public static readonly DependencyProperty DeclarationProperty =
            DependencyProperty.RegisterAttached("Declaration", typeof(object), typeof(ObjectReference),
                new FrameworkPropertyMetadata(null));

        public static object GetDeclaration(DependencyObject d)
        {
            return (object)d.GetValue(DeclarationProperty);
        }

        public static void SetDeclaration(DependencyObject d, object value)
        {
            d.SetValue(DeclarationProperty, value);
        }

        #endregion

        [ThreadStatic]
        private static Dictionary<object, WeakReference> _references = null;

        private static Dictionary<object, WeakReference> References
        {
            get
            {
                if (_references == null)
                {
                    _references = new Dictionary<object, WeakReference>();
                }
                return _references;
            }
        }

        private object _key = null;
        public object Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private bool _isDeclaration = false;
        public bool IsDeclaration
        {
            get { return _isDeclaration; }
            set { _isDeclaration = value; }
        }
    }
}
