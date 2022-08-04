using System;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Linq;

namespace ay.MARKUP.ResponsiveSupport
{
    [ContentProperty("AyVisualStateGroups")]
    public class AyVisualStateManager
    {

        public static AyOrientation ScreenDirection { get; set; }

        //static void SystemEvents_DisplaySettingsChanged(object sender, System.EventArgs e)
        //{
        //    SetDeviceScreenDirectChanged();
        //}

        //private static void SetDeviceScreenDirectChanged()
        //{
        //    if (System.Windows.SystemParameters.PrimaryScreenWidth > System.Windows.SystemParameters.PrimaryScreenHeight)
        //    {
        //        ScreenDirection = Orientation.Horizontal;
        //    }
        //    else
        //    {
        //        ScreenDirection = Orientation.Vertical;
        //    }
        //    if (!isFirst)
        //    {

        //    }
        //    isFirst = false;
        //}

        private static readonly DependencyPropertyKey AyVisualStateGroupsPropertyKey
    = DependencyProperty.RegisterAttachedReadOnly("BehaviorsInternal", typeof(AyVisualStateGroupCollection), typeof(AyVisualStateManager),
        new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty AyVisualStateGroupsProperty
            = AyVisualStateGroupsPropertyKey.DependencyProperty;


        [AttachedPropertyBrowsableForType(typeof(AyVisualStateGroup))]
        public static AyVisualStateGroupCollection GetAyVisualStateGroups(DependencyObject d)
        {
            if (d == null)
                throw new InvalidOperationException("The dependency object trying to attach to is set to null");
            if (!WpfTreeHelper.IsInDesignMode)
            {
                AyVisualStateGroupCollection collection = d.GetValue(AyVisualStateManager.AyVisualStateGroupsProperty) as AyVisualStateGroupCollection;
                if (collection == null)
                {
                    collection = new AyVisualStateGroupCollection();
                    collection.Owner = d;
                    var _2 = d as FrameworkElement;
                    if (_2 != null)
                    {

                        Microsoft.Win32.SystemEvents.DisplaySettingsChanged += (sen, eargs) =>
                        {
                            if (System.Windows.SystemParameters.PrimaryScreenWidth > System.Windows.SystemParameters.PrimaryScreenHeight)
                            {
                                ScreenDirection = AyOrientation.Horizontal;
                            }
                            else
                            {
                                ScreenDirection = AyOrientation.Vertical;
                            }
                            WhenOwnerSizeChanged(new Size(_2.ActualWidth, _2.ActualHeight), collection, _2);

                        };
                        if (System.Windows.SystemParameters.PrimaryScreenWidth > System.Windows.SystemParameters.PrimaryScreenHeight)
                        {
                            ScreenDirection = AyOrientation.Horizontal;
                        }
                        else
                        {
                            ScreenDirection = AyOrientation.Vertical;
                        }
                        _2.SizeChanged += (sender, args) =>
                        {
                            var _nowSize = args.NewSize;
                            WhenOwnerSizeChanged(_nowSize, collection, _2);
                        };

                    }
                    SetAyVisualStateGroups(d, collection);
                }
                return collection;
            }
            return new AyVisualStateGroupCollection();
        }

        private static void WhenOwnerSizeChanged(Size _nowSize, AyVisualStateGroupCollection collection, FrameworkElement _2)
        {
            var _31 = collection.FirstOrDefault(x => x.Orientation == AyOrientation.Both);
            if (_31 != null)
            {
                //执行
                var _4 = _31.VisualStates;
                foreach (var item in _4)
                {
                    if (item.MinWindowWidth.HasValue && !item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Width <= item.MinWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    else if (!item.MinWindowWidth.HasValue && item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Height <= item.MinWindowHeight.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (item.MinWindowWidth.HasValue && item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Height <= item.MinWindowHeight.Value && _nowSize.Width <= item.MinWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    else
                    if (item.MaxWindowWidth.HasValue && !item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Width > item.MaxWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (!item.MaxWindowWidth.HasValue && item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Height > item.MaxWindowHeight.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (item.MaxWindowWidth.HasValue && item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Height > item.MaxWindowHeight.Value && _nowSize.Width > item.MaxWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            var _3 = collection.FirstOrDefault(x => x.Orientation == ScreenDirection);
            if (_3 != null)
            {
                var _4 = _3.VisualStates;
                foreach (var item in _4)
                {
                    if (item.MinWindowWidth.HasValue && !item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Width <= item.MinWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (!item.MinWindowWidth.HasValue && item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Height <= item.MinWindowHeight.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (item.MinWindowWidth.HasValue && item.MinWindowHeight.HasValue)
                    {
                        if (_nowSize.Height <= item.MinWindowHeight.Value && _nowSize.Width <= item.MinWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    else
                    if (item.MaxWindowWidth.HasValue && !item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Width > item.MaxWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (!item.MaxWindowWidth.HasValue && item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Height > item.MaxWindowHeight.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (item.MaxWindowWidth.HasValue && item.MaxWindowHeight.HasValue)
                    {
                        if (_nowSize.Height > item.MaxWindowHeight.Value && _nowSize.Width > item.MaxWindowWidth.Value)
                        {
                            ExecuteSetters(item, _2);
                            if (item.SetterMode == VisualStateSetteMode.Continue)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static void ExecuteSetters(AyVisualState item, FrameworkElement w)
        {
            Window _parentWindow = null;
            if (w is Window)
            {
                _parentWindow = w as Window;
            }
            else
            {
                _parentWindow = Window.GetWindow(w);
            }
            UserControl _parentUserControl = null;
            if (w is UserControl)
            {
                _parentUserControl = w as UserControl;
            }
            else
            {
                _parentUserControl = WpfTreeHelper.FindParentControl<UserControl>(w);
            }

            Page _parentPage = null;
            if (w is Page)
            {
                _parentPage = w as Page;
            }
            else
            {
                _parentPage = WpfTreeHelper.FindParentControl<Page>(w);
            }

            foreach (var setter2 in item.Setters)
            {
                if (setter2 is DataSetter)
                {
                    var setter = setter2 as DataSetter;

                    if (setter.TargetName != null)
                    {
                        switch (setter.Scope)
                        {
                            case DataSetterScope.Current:
                                var _CurrentFind = w.FindName(setter.TargetName) as UIElement;
                                if (_CurrentFind != null)
                                {
                                    _CurrentFind.SetValue(setter.Property, setter.Value);
                                }
                                break;
                            case DataSetterScope.ParentWindow:
                                if (_parentWindow != null)
                                {
                                    var _ParentWindowFind = _parentWindow.FindName(setter.TargetName) as UIElement;
                                    if (_ParentWindowFind != null)
                                    {
                                        _ParentWindowFind.SetValue(setter.Property, setter.Value);
                                    }
                                }
                                break;
                            case DataSetterScope.ParentPage:
                                if (_parentPage != null)
                                {
                                    var _ParentPageFind = _parentPage.FindName(setter.TargetName) as UIElement;
                                    if (_ParentPageFind != null)
                                    {
                                        _ParentPageFind.SetValue(setter.Property, setter.Value);
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (setter2 is ResourceSetter)
                {
                    var setter = setter2 as ResourceSetter;

                    if (setter.TargetName != null)
                    {
                        switch (setter.Scope)
                        {
                            case ResourceSetterScope.FindInPageResource:
                                if (_parentPage != null)
                                {
                                    var _resources = _parentPage.Resources;
                                    if (setter.ResourceType == typeof(Thickness))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToThickness();
                                    }
                                    else if (setter.ResourceType == typeof(CornerRadius))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToCornerRadius();
                                    }
                                    else
                                    {
                                        _resources[setter.TargetName] = Convert.ChangeType(setter.ResourceValue, setter.ResourceType);
                                    }
                                }
                                break;
                            case ResourceSetterScope.FindInApplicationResource:
                                { 
                                var _resources = Application.Current.Resources;
                                    if (setter.ResourceType == typeof(Thickness))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToThickness();
                                    }
                                    else if (setter.ResourceType == typeof(CornerRadius))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToCornerRadius();
                                    }
                                    else
                                    {
                                        _resources[setter.TargetName] = Convert.ChangeType(setter.ResourceValue, setter.ResourceType);
                                    }
                                }
                                break;
                            case ResourceSetterScope.FindInWindowResource:
                                if (_parentWindow != null)
                                {
                                    var _resources = _parentWindow.Resources;
                                    if (setter.ResourceType == typeof(Thickness))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToThickness();
                                    }
                                    else if (setter.ResourceType == typeof(CornerRadius))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToCornerRadius();
                                    }
                                    else
                                    {
                                        _resources[setter.TargetName] = Convert.ChangeType(setter.ResourceValue, setter.ResourceType);
                                    }
                                   
                                }
                                break;
                            case ResourceSetterScope.FindInUserControlResource:
                                if (_parentUserControl != null)
                                {
                                    var _resources = _parentUserControl.Resources;
                                    if (setter.ResourceType == typeof(Thickness))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToThickness();
                                    }
                                    else if (setter.ResourceType == typeof(CornerRadius))
                                    {
                                        _resources[setter.TargetName] = setter.ResourceValue.ToString().ToCornerRadius();
                                    }
                                    else
                                    {
                                        _resources[setter.TargetName] = Convert.ChangeType(setter.ResourceValue, setter.ResourceType);
                                    }
                                }
                                break;
                        }
                    }

                }


            }
        }


        /// <summary>
        /// Provides a secure method for setting the Behaviors property.  
        /// This dependency property indicates ....
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(AyVisualStateGroup))]
        private static void SetAyVisualStateGroups(DependencyObject d, AyVisualStateGroupCollection value)
        {
            d.SetValue(AyVisualStateGroupsPropertyKey, value);
            INotifyCollectionChanged collection = (INotifyCollectionChanged)value;
            collection.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        static void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AyVisualStateGroupCollection sourceCollection = (AyVisualStateGroupCollection)sender;
            switch (e.Action)
            {

                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                        foreach (AyVisualStateGroup item in e.NewItems)
                            item.Owner = sourceCollection.Owner;
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (AyVisualStateGroup item in e.OldItems)
                        {
                            //item.Behavior.Dispose();
                        }
                    break;


                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null)
                        foreach (AyVisualStateGroup item in e.NewItems)
                            item.Owner = sourceCollection.Owner;

                    if (e.OldItems != null)
                        foreach (AyVisualStateGroup item in e.OldItems)
                        {
                            //item.Behavior.Dispose();
                        }
                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)
                        foreach (AyVisualStateGroup item in e.OldItems)
                        {
                            //item.Behavior.Dispose();
                        }
                    break;

                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }

    }


}
