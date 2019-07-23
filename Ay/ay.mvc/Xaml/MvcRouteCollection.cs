using System;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Markup;

namespace Ay.MvcFramework.AyMarkupExtension
{
    [ContentProperty("Routes")]
    public class MvcRouteCollection
    {
        #region Behaviors

        private static readonly DependencyPropertyKey RoutesPropertyKey
            = DependencyProperty.RegisterAttachedReadOnly("BehaviorsInternal", typeof(BehaviorBindingCollection), typeof(MvcRouteCollection),
                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty RoutesProperty
            = RoutesPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the Behaviors property.  
        /// Here we initialze the collection and set the Owner property
        /// 2017-09-01 11:37:04 ay 增加
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(RouteSetter))]
        public static BehaviorBindingCollection GetRoutes(DependencyObject d)
        {
            if (d == null)
                throw new InvalidOperationException("The dependency object trying to attach to is set to null");
            if (!WpfHelper.IsInDesignMode)
            {
                BehaviorBindingCollection collection = d.GetValue(MvcRouteCollection.RoutesProperty) as BehaviorBindingCollection;
                if (collection == null)
                {
                    collection = new BehaviorBindingCollection();
                    collection.Owner = d;
                    SetRoutes(d, collection);
                }
                return collection;
            }
            return new BehaviorBindingCollection();
        }

        /// <summary>
        /// Provides a secure method for setting the Behaviors property.  
        /// This dependency property indicates ....
        /// </summary>
        private static void SetRoutes(DependencyObject d, BehaviorBindingCollection value)
        {
            d.SetValue(RoutesPropertyKey, value);
            INotifyCollectionChanged collection = (INotifyCollectionChanged)value;
            collection.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        static void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BehaviorBindingCollection sourceCollection = (BehaviorBindingCollection)sender;
            switch (e.Action)
            {
                //when an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (RouteSetter item in e.NewItems)
                            item.Owner = sourceCollection.Owner;
                    break;
                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (RouteSetter item in e.OldItems)
                            item.Behavior.Dispose();
                    break;

                //here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null)
                        foreach (RouteSetter item in e.NewItems)
                            item.Owner = sourceCollection.Owner;

                    if (e.OldItems != null)
                        foreach (RouteSetter item in e.OldItems)
                            item.Behavior.Dispose();
                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)
                        foreach (RouteSetter item in e.OldItems)
                            item.Behavior.Dispose();
                    break;

                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }

        #endregion

    }

    /// <summary>
    /// Collection to store the list of behaviors. This is done so that you can intiniate it from XAML
    /// This inherits from freezable so that it gets inheritance context for DataBinding to work
    /// </summary>
    public class BehaviorBindingCollection : FreezableCollection<RouteSetter>
    {
        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner { get; set; }
    }
}