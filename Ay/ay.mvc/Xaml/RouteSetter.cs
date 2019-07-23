using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ay.MvcFramework.AyMarkupExtension
{
    /// <summary>
    /// Defines a Command Binding
    /// This inherits from freezable so that it gets inheritance context for DataBinding to work
    /// </summary>
    public class RouteSetter : Freezable
    {
        CommandBehaviorBinding behavior;
        /// <summary>
        /// Stores the Command Behavior Binding
        /// </summary>
        internal CommandBehaviorBinding Behavior
        {
            get
            {
                if (behavior == null)
                    behavior = new CommandBehaviorBinding();
                return behavior;
            }
        }

        bool rightExecuteLock = false;
        DependencyObject owner;
        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                ResetEventBinding();
                if (owner != null)
                {
                    if (!rightExecuteLock)
                    {
                        AppFirstLoad();
                    }
                    rightExecuteLock = true;
                }
            }
        }

        //#region Command

        ///// <summary>
        ///// Command Dependency Property
        ///// </summary>
        //public static readonly DependencyProperty CommandProperty =
        //    DependencyProperty.Register("Command", typeof(ICommand), typeof(RouteSetter),
        //        new FrameworkPropertyMetadata((ICommand)null,
        //            new PropertyChangedCallback(OnCommandChanged)));

        ///// <summary>
        ///// Gets or sets the Command property.  
        ///// </summary>
        //public ICommand Command
        //{
        //    get { return (ICommand)GetValue(CommandProperty); }
        //    set { SetValue(CommandProperty, value); }
        //}

        ///// <summary>
        ///// Handles changes to the Command property.
        ///// </summary>
        //private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((RouteSetter)d).OnCommandChanged(e);
        //}

        ///// <summary>
        ///// Provides derived classes an opportunity to handle changes to the Command property.
        ///// </summary>
        //protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    Behavior.Command = Command;
        //}

        //#endregion

        #region Action

        /// <summary>
        /// Action Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(ActionResult), typeof(RouteSetter),
                new FrameworkPropertyMetadata((ActionResult)null,
                    new PropertyChangedCallback(OnActionChanged)));

        /// <summary>
        /// Gets or sets the Action property. 
        /// </summary>
        public ActionResult Action
        {
            get { return (ActionResult)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Action property.
        /// </summary>
        private static void OnActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _32 = d.GetBindingExpression(RouteSetter.ActionProperty);
            var _33 = ((RouteSetter)d);
            if (_32!=null)
            {
                var _bindname = _32.ParentBinding.Path.Path;
                ((RouteSetter)d).Behavior.ActionName = _bindname.Substring(_bindname.LastIndexOf('.') + 1);
                // 初始化执行
                //dynamic d3 = ((dynamic)_32).ContextElement;
                //_33.Behavior.Owner = null;
            }
            _33.OnActionChanged(e);

        }


        public void AppFirstLoad()
        {
            if (Action==null) return;
            //var _1 = Action.Target.ToString();
            //var _2 = _1.Substring(0, _1.IndexOf("+<>c"));
            //var objType = Type.GetType(_2 + "," + ClientApplicationInfo.ClientAssemblyName);
            if (Action.Target == null) return;
            var objType = Action.Target.GetType().DeclaringType;
            if (objType == null)
            {
                objType = Action.Target.GetType();
                if (objType == null)
                {
                    return;
                }
            }
            if (objType != null)
            {
                var controllerAttribute =
                          objType.GetCustomAttributes(typeof(AuthorizeAttribute), false);

                List<AuthorizeAttribute> controllerFilters = null;
                List<AuthorizeAttribute> actionFilters = null;

                if (controllerAttribute!=null && controllerAttribute.Length > 0)
                {
                    controllerFilters = new List<AuthorizeAttribute>();
                    foreach (object ta in controllerAttribute)
                    {
                        controllerFilters.Add(ta as AuthorizeAttribute);
                    }
                    //排序筛选器 AY 2017-8-17 14:05:56
                    //同类型过滤器 controller和action只会执行1次，或者2个都执行
                    controllerFilters = controllerFilters.OrderBy(a => a.Order).ToList();
                }
                if (Behavior.ActionName!=null)
                {
                    var propertyInfo = objType.GetProperty(Behavior.ActionName);
                    if (propertyInfo != null)
                    {
                        var attribute =
                              propertyInfo.GetCustomAttributes(typeof(AuthorizeAttribute), false);

                        if (attribute!=null && attribute.Length > 0)
                        {
                            actionFilters = new List<AuthorizeAttribute>();
                            foreach (object ta in attribute)
                            {
                                actionFilters.Add(ta as AuthorizeAttribute);
                            }
                        }
                    }
                }


                if (controllerFilters == null && actionFilters == null)
                {
                    return;
                }
                else if (controllerFilters == null && actionFilters != null)
                {
                    var authorizationFilter = actionFilters.OrderBy(a => a.Order).ToList();
                    bool isForbid = false;
                    foreach (var item in authorizationFilter)
                    {
                        var attribute = (item.GetType()).GetCustomAttributes(typeof(AppStartLoadAttribute), false);
                        if (attribute.Count() > 0)
                        {
                            bool isOK = item.OnAuthorization(Behavior.strategy);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                    }
                    if (isForbid)
                    {
                        return;
                    }
                }
                else if (controllerFilters != null && actionFilters == null)  //控制器全局
                {
                    bool isForbid = false;
                    var authorizationFilter = controllerFilters.OrderBy(a => a.Order).ToList();
                    //优先执行IAuthorizationFilter
                    foreach (var item in authorizationFilter)
                    {
                        var attribute = (item.GetType()).GetCustomAttributes(typeof(AppStartLoadAttribute), false);
                        if (attribute.Count() > 0)
                        {
                            bool isOK = item.OnAuthorization(Behavior.strategy);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                    }
                    if (isForbid)
                    {
                        return;
                    }
                }
                else if (controllerFilters != null && actionFilters != null)
                {

                    bool isForbid = false;
                    List<AuthorizeAttribute> authorizationControllerFilter = new List<AuthorizeAttribute>();
                    for (int i = 0; i < controllerFilters.Count(); i++)
                    {
                        var _temp = controllerFilters[i] as AuthorizeAttribute;
                        if (_temp!=null) authorizationControllerFilter.Add(_temp);
                    }

                    //当前要被执行的的过滤器
                    List<AuthorizeAttribute> authorizationFilterWilling = new List<AuthorizeAttribute>();
                    for (int i = 0; i < actionFilters.Count(); i++)
                    {
                        var _temp = actionFilters[i] as AuthorizeAttribute;
                        if (_temp!=null)
                        {
                            var _afilterType = _temp.GetType();
                            bool hasSame = false;
                            foreach (var item in authorizationControllerFilter)
                            {
                                if (_afilterType == item.GetType())
                                {
                                    hasSame = true;
                                    //查看策略
                                    switch (_temp.FilterScope)
                                    {
                                        case FilterScope.Controller:
                                            authorizationFilterWilling.Add(item);
                                            break;
                                        case FilterScope.Action:
                                            authorizationFilterWilling.Add(_temp);
                                            break;
                                        case FilterScope.Both:
                                            authorizationFilterWilling.Add(item);
                                            authorizationFilterWilling.Add(_temp);
                                            break;
                                    }
                                    break;
                                }
                            }
                            if (!hasSame)
                            {
                                authorizationFilterWilling.Add(_temp);
                            }
                        }
                    }

                    //执行时，action是同类别的filter，如果有，获得FilterScope，决定再怎么执行
                    //优先执行授权，再action
                    //优先执行IAuthorizationFilter  ============筛选AuthorizeAttribute
                    //第四步 挑拣 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ AY2017-8-18 11:41:01 
                    int aclength = authorizationControllerFilter.Count();
                    int aalength = authorizationFilterWilling.Count();
                    for (int i = 0; i < aclength; i++)
                    {
                        bool hasSame = false;
                        for (int j = 0; j < aalength; j++)
                        {
                            if (authorizationControllerFilter[i].GetType() == authorizationFilterWilling[j].GetType())
                            {
                                hasSame = true;
                                break;
                            }
                        }
                        if (!hasSame)
                        {
                            authorizationFilterWilling.Add(authorizationControllerFilter[i]);
                        }
                    }

                    //第四步 挑拣完成↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
                    //优先执行IAuthorizationFilter
                    authorizationFilterWilling = authorizationFilterWilling.OrderBy(a => a.Order).ToList();
                    foreach (var item in authorizationFilterWilling)
                    {
                        var attribute = (item.GetType()).GetCustomAttributes(typeof(AppStartLoadAttribute), false);
                        if (attribute.Count() > 0)
                        {
                            bool isOK = item.OnAuthorization(Behavior.strategy);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                    }
                    if (isForbid)
                    {
                        return;
                    }

                }


            }

        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Action property.
        /// </summary>
        protected virtual void OnActionChanged(DependencyPropertyChangedEventArgs e)
        {
            Behavior.Action = Action;
        }

        #endregion

        #region Parameter

        /// <summary>
        /// CommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object), typeof(RouteSetter),
                new FrameworkPropertyMetadata((object)null,
                    new PropertyChangedCallback(OnParameterChanged)));

        /// <summary>
        /// Gets or sets the CommandParameter property.  
        /// </summary>
        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RouteSetter)d).OnParameterChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CommandParameter property.
        /// </summary>
        protected virtual void OnParameterChanged(DependencyPropertyChangedEventArgs e)
        {
            Behavior.CommandParameter = Parameter;
        }

        #endregion


        #region IsSendEventArgs
        public static readonly DependencyProperty IsSendEventArgsProperty =
         DependencyProperty.Register("IsSendEventParameter", typeof(bool), typeof(RouteSetter),
             new FrameworkPropertyMetadata(false,
                 new PropertyChangedCallback(OnIsSendEventArgsChanged)));


        public bool IsSendEventArgs
        {
            get { return (bool)GetValue(IsSendEventArgsProperty); }
            set { SetValue(IsSendEventArgsProperty, value); }
        }

        private static void OnIsSendEventArgsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RouteSetter)d).OnIsSendEventParameterChanged(e);
        }

        protected virtual void OnIsSendEventParameterChanged(DependencyPropertyChangedEventArgs e)
        {
            Behavior.IsSendEventArgs = IsSendEventArgs;
        }

        #endregion
        #region Event

        /// <summary>
        /// Event Dependency Property
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.Register("Event", typeof(string), typeof(RouteSetter),
                new FrameworkPropertyMetadata((string)null,
                    new PropertyChangedCallback(OnEventChanged)));

        /// <summary>
        /// Gets or sets the Event property.  
        /// </summary>
        public string Event
        {
            get { return (string)GetValue(EventProperty); }
            set { SetValue(EventProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Event property.
        /// </summary>
        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RouteSetter)d).OnEventChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Event property.
        /// </summary>
        protected virtual void OnEventChanged(DependencyPropertyChangedEventArgs e)
        {
            ResetEventBinding();
        }

        #endregion

        static void OwnerReset(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RouteSetter)d).ResetEventBinding();
        }

        private void ResetEventBinding()
        {
            if (Owner != null) //only do this when the Owner is set
            {
                //check if the Event is set. If yes we need to rebind the Command to the new event and unregister the old one
                if (Behavior.Event != null && Behavior.Owner != null)
                    Behavior.Dispose();

                //bind the new event to the command
                Behavior.BindEvent(Owner, Event);
            }
        }

        //pack://application:,,,/Ay.Framework.WPF;component/

        /// <summary>
        /// This is not actually used. This is just a trick so that this object gets WPF Inheritance Context
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new RouteSetter();
        }
    }
}
