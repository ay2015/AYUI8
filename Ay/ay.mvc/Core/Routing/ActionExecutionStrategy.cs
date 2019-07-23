using System;
using System.Collections.Generic;
using System.Linq;
using Ay.MvcFramework;
using Ay.MvcFramework.AyMarkupExtension;

namespace Ay.MvcFramework
{
    public class ActionExecutionStrategy : IExecutionStrategy
    {
         
        #region IExecutionStrategy Members

        public CommandBehaviorBinding Route { get; set; }



        public void Execute(object parameter)
        {
            //拦截
            if (Route!=null)
            {
                //System.Windows.FrameworkElement _1 = Route.Owner as System.Windows.FrameworkElement;
                //Controller _getC 

                //设置controller 2017-9-21 13:22:59
                if (Route.Controller == null)
                {


                    System.Windows.FrameworkElement _100 = Route.Owner as System.Windows.FrameworkElement;
                    if (_100!=null)
                    {
                        Route.Controller = _100.DataContext as Controller;
                        if (Route.Controller == null)
                        {
                            var _1011 = _100.GetVisualAncestor<System.Windows.Controls.UserControl>();
                            if (_1011!=null)
                            {
                                Route.Controller = _1011.DataContext as Controller;
                                if (Route.Controller == null)
                                {
                                    var _101 = _100.GetVisualAncestor<System.Windows.Controls.Page>();
                                    if (_101!=null)
                                    {
                                        Route.Controller = _101.DataContext as Controller;
                                    }

                                    if (Route.Controller == null)
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
                //if (Route.Controller == null) return;
                //var _1 = Route.Action.Target.ToString();
                var objType = Route.Action.Target.GetType().DeclaringType;
                if (objType == null)
                {
                    objType = Route.Action.Target.GetType();
                    if (objType == null)
                    {
                        return;
                    }
                }
                //var _3123 = _122.Assembly;
                //Type objType = null;
                //int dw = _1.IndexOf("+<>c");
                //string _2 = null;
                //if (dw > -1)
                //{
                //     _2 = _1.Substring(0, dw);
                //    objType = Type.GetType(_2 + "," + ClientApplicationInfo.ClientAssemblyName);
                //}
                //else
                //{
                //    objType = Type.GetType(_1 + "," + ClientApplicationInfo.ClientAssemblyName);
                //}


                //var _controller=Activator.CreateInstance(controllerType);
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

                    if (Route.ActionName != null)
                    {


                        var propertyInfo = objType.GetProperty(Route.ActionName);

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
                    }
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
                            bool isOK = item.OnAuthorization(this);
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
                                item.OnActionExecuting(this);
                            }
                            Route.Action(parameter);

                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilter.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilter[i].OnActionExecuted(this);
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
                            bool isOK = item.OnAuthorization(this);
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
                                item.OnActionExecuting(this);
                            }
                            Route.Action(parameter);

                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilter.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilter[i].OnActionExecuted(this);
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
                            bool isOK = item.OnAuthorization(this);
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
                                item.OnActionExecuting(this);
                            }
                            Route.Action(parameter);
                            //2017-8-18 11:58:45  从 Controller级别执行前-> action级别执行前 -> 执行  -> action级别执行后 ->  Controller级别 执行后
                            int maxLength = actionFilterWilling.Count();
                            for (int i = maxLength - 1; i >= 0; i--)
                            {
                                actionFilterWilling[i].OnActionExecuted(this);
                            }

                        }
                    }
                }
                else
                {
                    throw new Exception("这个类型不存在");
                }
            }
        }

        #endregion
    }

}
