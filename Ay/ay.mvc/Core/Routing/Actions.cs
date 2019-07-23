using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ay.MvcFramework.AyMarkupExtension;

namespace Ay.MvcFramework
{
    /// <summary>
    /// 生日 2017-09-06 14:38:13 
    /// 增加 xaml.cs 对 Controller的访问控制
    /// AY ay www.ayjs.net
    /// </summary>
    /// <typeparam name="T">AYUI控制器</typeparam>
    public class Actions<T> where T : Controller
    {
        public Actions()
        {

        }
        /// <summary>
        /// 当前窗体对应的控制器类型
        /// </summary>
        /// <param name="_controllerInstance"></param>
        public Actions(T _controllerInstance)
        {
            this.Controller = _controllerInstance;
        }


        private T _controller;

        public T Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }


        /// <summary>
        ///直接执行action，不通过Filter
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        public void Use(Expression<Func<T, ActionResult>> action)
        {
            Use(action,null);
        }
        /// <summary>
        ///直接执行action，不通过Filter
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        /// <param name="sender">当前事件的Sender参数</param>
        /// <param name="eventArgs">当前事件的args参数</param>
        public void Use(Expression<Func<T, ActionResult>> action, object parameter, object sender, object eventArgs)
        {
            Use(action, new object[] { parameter, sender, eventArgs });
        }

        /// <summary>
        ///直接执行action，不通过Filter
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        public void Use(Expression<Func<T, ActionResult>> action, object parameter)
        {
            Func<T, ActionResult> result = action.Compile();
            ActionResult _actionResult = result(Controller);
            if (_actionResult!=null)
            {
                _actionResult(parameter);
            }
        }


        /// <summary>
        /// 过滤器的方式执行 Action
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        public void UseFilter(Expression<Func<T, ActionResult>> action)
        {
            UseFilter(action,null);
        }
        /// <summary>
        /// 过滤器的方式执行 Action
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        public void UseFilter(Expression<Func<T, ActionResult>> action, object parameter)
        {
            UseFilter(action,parameter, null);
        }
        /// <summary>
        /// 过滤器的方式执行 Action
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        /// <param name="sender">当前事件的Sender参数</param>
        /// <param name="eventArgs">当前事件的args参数</param>
        public void UseFilter(Expression<Func<T, ActionResult>> action, object parameter,object sender, EventArgs eventArgs)
        {
            UseFilter(action, new object[] { parameter, sender, eventArgs }, null);
        }

        /// <summary>
        /// 过滤器的方式执行 Action
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        /// <param name="sender">当前事件的Sender参数</param>
        /// <param name="eventArgs">当前事件的args参数</param>
        /// <param name="Route">Mvc路由信息模型，无需设置Action和ActionName,如果你在filter中需要信息，你可以传递此参数,主要是EventName,Owner值设置</param>
        public void UseFilter(Expression<Func<T, ActionResult>> action, object parameter, object sender, EventArgs eventArgs, CommandBehaviorBinding Route)
        {
            UseFilter(action, new object[] { parameter, sender, eventArgs },Route);
        }
        /// <summary>
        /// 过滤器的方式执行 Action
        /// </summary>
        /// <param name="action">Controller中的Action</param>
        /// <param name="parameter">Controller中的Action的参数</param>
        /// <param name="Route">Mvc路由信息模型，无需设置Action和ActionName,如果你在filter中需要信息，你可以传递此参数,主要是EventName,Owner值设置</param>
        public void UseFilter(Expression<Func<T, ActionResult>> action, object parameter, CommandBehaviorBinding Route)
        {
            Func<T, ActionResult> result = action.Compile();
            ActionResult _actionResult = result(Controller);
            if (_actionResult!=null)
            {
                var memberExpression = action.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", "propertyExpression");
                }

                var propertyInfo = memberExpression.Member as System.Reflection.PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", "propertyExpression");
                }

                //获取控制器类型

                var objTyp1 = Controller.GetType();
                var objType = Controller.GetType().DeclaringType;
                if(objType==null && objTyp1 != null)
                {
                    objType = objTyp1;
                }
                if (objType == null && objTyp1==null)
                {
                    objType = Route.Action.Target.GetType();
                    if (objType == null)
                    {
                        return;
                    }
                }
                if (objType != null)
                {
                    //获得控制器是否有filter AY 2017-8-17 14:06:01
                    var controllerAttribute =
                              objType.GetCustomAttributes(typeof(FilterAttribute), false);

                    List<FilterAttribute> controllerFilters = null;
                    List<FilterAttribute> actionFilters = null;

                    if (controllerAttribute!=null && controllerAttribute.Length > 0)
                    { 
                        controllerFilters = new List<FilterAttribute>();
                        foreach (object ta in controllerAttribute)
                        {
                            controllerFilters.Add(ta as FilterAttribute);
                        }
                        //排序筛选器 AY 2017-8-17 14:05:56
                        //同类型过滤器 controller和action只会执行1次，或者2个都执行
                        controllerFilters = controllerFilters.OrderBy(a => a.Order).ToList();
                    }



                    if (propertyInfo != null)
                    {
                        var attribute =
                              propertyInfo.GetCustomAttributes(typeof(FilterAttribute), false);

                        if (attribute!=null && attribute.Length > 0)
                        {
                            actionFilters = new List<FilterAttribute>();
                            foreach (object ta in attribute)
                            {
                                actionFilters.Add(ta as FilterAttribute);
                            }
                        }
                    }


                    var _this = new ActionExecutionStrategy();
                    if (Route == null)
                    {
                        Route = new CommandBehaviorBinding();
                    }
                    Route.Action = _actionResult;
                    Route.ActionName = propertyInfo.Name;
                    Route.CommandParameter = parameter;

                    //设置controller 2017-9-21 13:35:36
                    if (Route.Owner!=null)
                    {
                        System.Windows.FrameworkElement _100 = Route.Owner as System.Windows.FrameworkElement;
                        if (_100!=null)
                        {
                            Route.Controller = _100.DataContext as Controller;
                            if (Controller == null)
                            {
                                var _1011 = _100.GetVisualAncestor<System.Windows.Controls.UserControl>();
                                if (_1011!=null)
                                {
                                    Route.Controller = _1011.DataContext as Controller;
                                    if (Controller == null)
                                    {
                                        var _101 = _100.GetVisualAncestor<System.Windows.Controls.Page>();
                                        if (_101!=null)
                                        {
                                            Route.Controller = _101.DataContext as Controller;
                                        }

                                        if (Controller == null)
                                        {
                                            var _102 = System.Windows.Window.GetWindow(_100);
                                            if (_102!=null)
                                            {
                                                Route.Controller = _102.DataContext as Controller;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    var _testIsArray = parameter as object[];
                    if (_testIsArray != null)
                    {
                        Route.IsSendEventArgs = true;
                    }
                    _this.Route = Route;
                    

                    if (controllerFilters == null && actionFilters == null)
                    {
                        Route.Action(parameter);
                    }
                    else if (controllerFilters == null && actionFilters != null)
                    {
                        bool isForbid = false;
                        List<AuthorizeAttribute> authorizationFilter = new List<AuthorizeAttribute>();
                        List<ActionFilterAttribute> actionFilter = new List<ActionFilterAttribute>();
                        for (int i = 0; i < actionFilters.Count(); i++)
                        {
                            var _temp = actionFilters[i] as AuthorizeAttribute;
                            if (_temp!=null) authorizationFilter.Add(_temp);
                            else
                            {
                                var _temp2 = actionFilters[i] as ActionFilterAttribute;
                                if (_temp2!=null)
                                {
                                    actionFilter.Add(_temp2);
                                }
                            }
                        }
                        authorizationFilter = authorizationFilter.OrderBy(a => a.Order).ToList();
                        //优先执行IAuthorizationFilter
                        foreach (var item in authorizationFilter)
                        {
                            bool isOK = item.OnAuthorization(_this);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                        if (isForbid)
                        {
                            return;
                        }
                        else
                        {
                            actionFilter = actionFilter.OrderBy(a => a.Order).ToList();
                            foreach (var item in actionFilter)
                            {
                                item.OnActionExecuting(_this);
                            }
                            _actionResult(parameter);

                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilter.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilter[i].OnActionExecuted(_this);
                            }
                        }
                    }
                    else if (controllerFilters != null && actionFilters == null)  //控制器全局
                    {
                        bool isForbid = false;
                        List<AuthorizeAttribute> authorizationFilter = new List<AuthorizeAttribute>();
                        List<ActionFilterAttribute> actionFilter = new List<ActionFilterAttribute>();
                        for (int i = 0; i < controllerFilters.Count(); i++)
                        {
                            var _temp = controllerFilters[i] as AuthorizeAttribute;
                            if (_temp!=null) authorizationFilter.Add(_temp);
                            else
                            {
                                var _temp2 = controllerFilters[i] as ActionFilterAttribute;
                                if (_temp2!=null)
                                {
                                    actionFilter.Add(_temp2);
                                }
                            }
                        }
                        authorizationFilter = authorizationFilter.OrderBy(a => a.Order).ToList();
                        //优先执行IAuthorizationFilter
                        foreach (var item in authorizationFilter)
                        {
                            bool isOK = item.OnAuthorization(_this);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                        if (isForbid)
                        {
                            return;
                        }
                        else
                        {
                            actionFilter = actionFilter.OrderBy(a => a.Order).ToList();
                            foreach (var item in actionFilter)
                            {
                                item.OnActionExecuting(_this);
                            }
                            Route.Action(parameter);

                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilter.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilter[i].OnActionExecuted(_this);
                            }

                        }
                    }
                    else if (controllerFilters != null && actionFilters != null)
                    {
                        //第一步洗牌：排序
                        //第二步洗牌：归类
                        //第三步洗牌：筛选
                        //第四步洗牌：挑拣
                        //第五步洗牌：为了执行
                        bool isForbid = false;
                        //归类
                        List<AuthorizeAttribute> authorizationControllerFilter = new List<AuthorizeAttribute>();
                        List<ActionFilterAttribute> actionControllerFilter = new List<ActionFilterAttribute>();
                        for (int i = 0; i < controllerFilters.Count(); i++)
                        {
                            var _temp = controllerFilters[i] as AuthorizeAttribute;
                            if (_temp!=null) authorizationControllerFilter.Add(_temp);
                            else
                            {
                                var _temp2 = controllerFilters[i] as ActionFilterAttribute;
                                if (_temp2!=null)
                                {
                                    actionControllerFilter.Add(_temp2);
                                }
                            }
                        }
                        //当前要被执行的的过滤器
                        List<AuthorizeAttribute> authorizationFilterWilling = new List<AuthorizeAttribute>();
                        List<ActionFilterAttribute> actionFilterWilling = new List<ActionFilterAttribute>();
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
                            else
                            {
                                var _temp2 = actionFilters[i] as ActionFilterAttribute;
                                if (_temp2!=null)
                                {
                                    var _afilterType = _temp2.GetType();
                                    bool hasSame = false;
                                    foreach (var item in actionControllerFilter)
                                    {
                                        if (_afilterType == item.GetType())
                                        {
                                            hasSame = true;
                                            //查看策略
                                            switch (_temp2.FilterScope)
                                            {
                                                case FilterScope.Controller:
                                                    actionFilterWilling.Add(item);
                                                    break;
                                                case FilterScope.Action:
                                                    actionFilterWilling.Add(_temp2);
                                                    break;
                                                case FilterScope.Both:
                                                    actionFilterWilling.Add(item);
                                                    actionFilterWilling.Add(_temp2);
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    if (!hasSame)
                                    {
                                        actionFilterWilling.Add(_temp2);
                                    }
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


                        int alength = actionControllerFilter.Count();
                        int awlength = actionFilterWilling.Count();
                        for (int i = 0; i < aclength; i++)
                        {
                            bool hasSame = false;
                            for (int j = 0; j < awlength; j++)
                            {
                                if (actionControllerFilter[i].GetType() == actionFilterWilling[j].GetType())
                                {
                                    hasSame = true;
                                    break;
                                }
                            }
                            if (!hasSame)
                            {
                                actionFilterWilling.Add(actionControllerFilter[i]);
                            }
                        }

                        //第四步 挑拣完成↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
                        //优先执行IAuthorizationFilter
                        authorizationFilterWilling = authorizationFilterWilling.OrderBy(a => a.Order).ToList();
                        foreach (var item in authorizationFilterWilling)
                        {
                            bool isOK = item.OnAuthorization(_this);
                            if (!isOK)
                            {
                                isForbid = true;
                                break;
                            }
                        }
                        if (isForbid)
                        {
                            return;
                        }
                        else
                        {
                            actionFilterWilling = actionFilterWilling.OrderBy(a => a.Order).ToList();
                            foreach (var item in actionFilterWilling)
                            {
                                item.OnActionExecuting(_this);
                            }
                            Route.Action(parameter);
                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilterWilling.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilterWilling[i].OnActionExecuted(_this);
                            }

                        }
                    }
                }
            }




            //var getMethod = property.GetGetMethod(true);
            //if (getMethod.IsStatic)
            //{
            //    throw new ArgumentException("PropertySupport_StaticExpression_Exception", "propertyExpression");
            //}
        }

    }
}
