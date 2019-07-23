using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Reflection;
using System.Windows;
using Ay.MvcFramework.Internal.Attributes;

namespace Ay.MvcFramework.AyMarkupExtension
{
    /// <summary>
    /// Defines the command behavior binding
    /// </summary>
    public class CommandBehaviorBinding : IDisposable
    {
        #region Properties
        public Controller Controller { get; set; }

        /// <summary>
        /// Get the owner of the CommandBinding ex: a Button
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public DependencyObject Owner { get; set; }
        /// <summary>
        /// The event name to hook up to
        /// This property can only be set from the BindEvent Method
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// The event info of the event
        /// </summary>
        public EventInfo Event { get; set; }
        /// <summary>
        /// Gets the EventHandler for the binding with the event
        /// </summary>
        public Delegate EventHandler { get; set; }
        /// <summary>
        /// 2017-9-1 10:17:33 ay  是否发送事件参数
        /// </summary>
        public bool IsSendEventArgs { get; set; }

        public bool IsLockOwner { get; set; } = false;
        #region Execution
        //stores the strategy of how to execute the event handler
        public IExecutionStrategy strategy;

        /// <summary>
        /// Gets or sets a CommandParameter
        /// </summary>
        public object CommandParameter { get; set; }

        //ICommand command;
        ///// <summary>
        ///// The command to execute when the specified event is raised
        ///// </summary>
        //public ICommand Command
        //{
        //    get { return command; }
        //    set
        //    {
        //        command = value;
        //        strategy = new CommandExecutionStrategy { Route = this };
        //    }
        //}

        public string ActionName { get; set; }

        ActionResult action;

        public ActionResult Action
        {
            get { return action; }
            set
            {
                action = value;
                strategy = new ActionExecutionStrategy { Route = this };
            }
        }
        #endregion

        #endregion
        public void BindEvent(DependencyObject owner, string eventName)
        {
            EventName = eventName;
            Owner = owner;
            Event = Owner.GetType().GetEvent(EventName, BindingFlags.Public | BindingFlags.Instance);
            if (Event == null)
                throw new InvalidOperationException(String.Format("你绑定的事件名有错误: {0}", EventName));
            var _methodInfo = typeof(CommandBehaviorBinding).GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance);
            //Create an event handler for the event that will call the ExecuteCommand method
            //EventHandler = EventHandlerGenerator.CreateDelegate(
            //    Event.EventHandlerType, _methodInfo, this);
            EventHandler = Delegate.CreateDelegate(Event.EventHandlerType, this, _methodInfo);

            //Register the handler to the Event
            Event.AddEventHandler(Owner, EventHandler);



        }
        #region 给Mvc的Event改变时候用的
        public void AppFirstLoad()
        {
            if (Controller == null) return;
            var objType = Controller.GetType().DeclaringType;
            if (objType == null)
            {
                objType = Controller.GetType();
                if (objType == null)
                {
                    return;
                }
            }
            //if (_1.IndexOf("+<>c") > -1)
            //{
            //    var _2 = _1.Substring(0, _1.IndexOf("+<>c"));
            //    objType = Type.GetType(_2 + "," + ClientApplicationInfo.ClientAssemblyName);
            //}else
            //{
            //    objType = Type.GetType(_1 + "," + ClientApplicationInfo.ClientAssemblyName);
            //}

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
                if (ActionName != null)
                {
                    var propertyInfo = objType.GetProperty(ActionName);
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
                        if (attribute.Count()>0)
                        {
                            bool isOK = item.OnAuthorization(strategy);
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
                            bool isOK = item.OnAuthorization(strategy);
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
                            bool isOK = item.OnAuthorization(strategy);
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
        #endregion



        /// <summary>
        /// Executes the strategy
        /// </summary>
        public void Execute(object sender, EventArgs args)
        {
            if (strategy!=null)
            {
                if (strategy.Route.IsSendEventArgs)
                {
                    strategy.Execute(new object[] { CommandParameter, sender, args });
                }
                else
                {
                    //注入
                    strategy.Execute(CommandParameter);
                }

            }

        }

        #region IDisposable Members
        bool disposed = false;
        /// <summary>
        /// Unregisters the EventHandler from the Event
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Event.RemoveEventHandler(Owner, EventHandler);
                disposed = true;
            }
        }
        ///// <summary>
        ///// AY拓展的，方便用户在ActionFilter里面可以设置，给用户机会决定是否
        ///// </summary>
        //public bool CanExecuted { get; set; } = true;
        #endregion
    }
}
