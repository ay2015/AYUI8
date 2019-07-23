
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;


namespace ay.Controls
{
//    <i:Interaction.Behaviors>
//    <Behaviours:ResourceChangeNotifierBehavior
//                Resource = "{DynamicResource MyDynamicResourceKey}"
//                ResourceChanged="OnResourceChanged"/>
//</i:Interaction.Behaviors>
    public class ResourceChangeNotifierBehavior
  : System.Windows.Interactivity.Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty ResourceProperty
                = DependencyProperty.Register("Resource",
                       typeof(object),
                       typeof(ResourceChangeNotifierBehavior),
                       new PropertyMetadata(default(object), ResourceChangedCallback));

        public event EventHandler ResourceChanged;

        public object Resource
        {
            get { return GetValue(ResourceProperty); }
            set { SetValue(ResourceProperty, value); }
        }

        private static void ResourceChangedCallback(DependencyObject dependencyObject,
                                                    DependencyPropertyChangedEventArgs args)
        {
            var resourceChangeNotifier = dependencyObject as ResourceChangeNotifierBehavior;
            if (resourceChangeNotifier == null)
                return;

            resourceChangeNotifier.OnResourceChanged();
        }

        private void OnResourceChanged()
        {
            EventHandler handler = ResourceChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public class DynamicResourceBinding : MarkupExtension
    {
        public DynamicResourceBinding(string path)
        {
            binding = new Binding(path);
        }

        #region Binding Members

        public PropertyPath Path
        {
            get { return binding.Path; }
            set { binding.Path = value; }
        }
        public string XPath
        {
            get { return binding.XPath; }
            set { binding.XPath = value; }
        }
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode
        {
            get { return binding.Mode; }
            set { binding.Mode = value; }
        }
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return binding.UpdateSourceTrigger; }
            set { binding.UpdateSourceTrigger = value; }
        }
        public IValueConverter Converter
        {
            get { return binding.Converter; }
            set { binding.Converter = value; }
        }
        public object ConverterParameter
        {
            get { return binding.ConverterParameter; }
            set { binding.ConverterParameter = value; }
        }
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture
        {
            get { return binding.ConverterCulture; }
            set { binding.ConverterCulture = value; }
        }
        public object Source
        {
            get { return binding.Source; }
            set { binding.Source = value; }
        }
        public string ElementName
        {
            get { return binding.ElementName; }
            set { binding.ElementName = value; }
        }
        public RelativeSource RelativeSource
        {
            get { return binding.RelativeSource; }
            set { binding.RelativeSource = value; }
        }
        public object FallbackValue
        {
            get { return binding.FallbackValue; }
            set { binding.FallbackValue = value; }
        }

        private readonly Binding binding;

        #endregion Binding Members

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                var targetObject = provideValueTarget.TargetObject as FrameworkElement;
                if (targetObject != null)
                {
                    var targetProperty = provideValueTarget.TargetProperty as DependencyProperty;
                    if (targetProperty != null)
                    {
                        targetObject.SetBinding(EnsureResourceKeyProperty(targetProperty), binding);
                    }
                }
            }

            return null;
        }

        private static readonly object locker = new object();

        public static DependencyProperty EnsureResourceKeyProperty(DependencyProperty targetProperty)
        {
            DependencyProperty resourceKeyProperty;
            lock (locker)
            {
                if (!DirectMap.TryGetValue(targetProperty, out resourceKeyProperty))
                {
                    resourceKeyProperty = RegisterResourceKeyProperty(targetProperty);
                    DirectMap.Add(targetProperty, resourceKeyProperty);
                    ReverseMap.Add(resourceKeyProperty, targetProperty);
                }
            }
            return resourceKeyProperty;
        }

        private static DependencyProperty RegisterResourceKeyProperty(DependencyProperty targetProperty)
        {
            return DependencyProperty.RegisterAttached(targetProperty.Name + "_ResourceKey", typeof(object), typeof(DynamicResourceBinding),
                new PropertyMetadata(ResourceKeyChanged));
        }

        private static void ResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null)
            {
                lock (locker)
                {
                    DependencyProperty targetProperty;
                    if (ReverseMap.TryGetValue(e.Property, out targetProperty))
                    {
                        fe.SetResourceReference(targetProperty, e.NewValue);
                    }
                }
            }
        }

        private static readonly Dictionary<DependencyProperty, DependencyProperty> DirectMap = new Dictionary<DependencyProperty, DependencyProperty>();
        private static readonly Dictionary<DependencyProperty, DependencyProperty> ReverseMap = new Dictionary<DependencyProperty, DependencyProperty>();
    }
    public class ResourceBinding : MarkupExtension
    {
        #region Helper properties

        public static object GetResourceBindingKeyHelper(DependencyObject obj)
        {
            return (object)obj.GetValue(ResourceBindingKeyHelperProperty);
        }

        public static void SetResourceBindingKeyHelper(DependencyObject obj, object value)
        {
            obj.SetValue(ResourceBindingKeyHelperProperty, value);
        }

        // Using a DependencyProperty as the backing store for ResourceBindingKeyHelper.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResourceBindingKeyHelperProperty =
            DependencyProperty.RegisterAttached("ResourceBindingKeyHelper", typeof(object), typeof(ResourceBinding), new PropertyMetadata(null, ResourceKeyChanged));

        static void ResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as FrameworkElement;
            var newVal = e.NewValue as Tuple<object, DependencyProperty>;

            if (target == null || newVal == null)
                return;

            var dp = newVal.Item2;

            if (newVal.Item1 == null)
            {
                target.SetValue(dp, dp.GetMetadata(target).DefaultValue);
                return;
            }

            target.SetResourceReference(dp, newVal.Item1);

        }

        #endregion

        public ResourceBinding()
        {

        }

        public ResourceBinding(string path)
        {
            this.Path = new PropertyPath(path);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTargetService = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (provideValueTargetService == null)
                return null;

            if (provideValueTargetService.TargetObject != null &&
                provideValueTargetService.TargetObject.GetType().FullName == "System.Windows.SharedDp")
                return this;


            var targetObject = provideValueTargetService.TargetObject as FrameworkElement;
            var targetProperty = provideValueTargetService.TargetProperty as DependencyProperty;
            if (targetObject == null || targetProperty == null)
                return null;



            var binding = new Binding();

            #region binding

            binding.Path = this.Path;
            binding.XPath = this.XPath;
            binding.Mode = this.Mode;
            binding.UpdateSourceTrigger = this.UpdateSourceTrigger;
            binding.Converter = this.Converter;
            binding.ConverterParameter = this.ConverterParameter;
            binding.ConverterCulture = this.ConverterCulture;

            if (this.RelativeSource != null)
                binding.RelativeSource = this.RelativeSource;

            if (this.ElementName != null)
                binding.ElementName = this.ElementName;

            if (this.Source != null)
                binding.Source = this.Source;

            binding.FallbackValue = this.FallbackValue;

            #endregion

            var multiBinding = new MultiBinding();
            multiBinding.Converter = HelperConverter.Current;
            multiBinding.ConverterParameter = targetProperty;

            multiBinding.Bindings.Add(binding);

            multiBinding.NotifyOnSourceUpdated = true;

            targetObject.SetBinding(ResourceBindingKeyHelperProperty, multiBinding);

            return null;

        }


        #region Binding Members

        /// <summary> The source path (for CLR bindings).</summary>
        public object Source
        {
            get;
            set;
        }

        /// <summary> The source path (for CLR bindings).</summary>
        public PropertyPath Path
        {
            get;
            set;
        }

        /// <summary> The XPath path (for XML bindings).</summary>
        [DefaultValue(null)]
        public string XPath
        {
            get;
            set;
        }

        /// <summary> Binding mode </summary>
        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode
        {
            get;
            set;
        }

        /// <summary> Update type </summary>
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get;
            set;
        }

        /// <summary> The Converter to apply </summary>
        [DefaultValue(null)]
        public IValueConverter Converter
        {
            get;
            set;
        }

        /// <summary>
        /// The parameter to pass to converter.
        /// </summary>
        /// <value></value>
        [DefaultValue(null)]
        public object ConverterParameter
        {
            get;
            set;
        }

        /// <summary> Culture in which to evaluate the converter </summary>
        [DefaultValue(null)]
        [TypeConverter(typeof(System.Windows.CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture
        {
            get;
            set;
        }

        /// <summary>
        /// Description of the object to use as the source, relative to the target element.
        /// </summary>
        [DefaultValue(null)]
        public RelativeSource RelativeSource
        {
            get;
            set;
        }

        /// <summary> Name of the element to use as the source </summary>
        [DefaultValue(null)]
        public string ElementName
        {
            get;
            set;
        }


        #endregion

        #region BindingBase Members

        /// <summary> Value to use when source cannot provide a value </summary>
        /// <remarks>
        ///     Initialized to DependencyProperty.UnsetValue; if FallbackValue is not set, BindingExpression
        ///     will return target property's default when Binding cannot get a real value.
        /// </remarks>
        public object FallbackValue
        {
            get;
            set;
        }

        #endregion



        #region Nested types

        private class HelperConverter : IMultiValueConverter
        {
            public static readonly HelperConverter Current = new HelperConverter();

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return Tuple.Create(values[0], (DependencyProperty)parameter);
            }
            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
