using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace Ay.MvcFramework.AyMarkupExtension
{
    public class RouteExtension : MarkupExtension
    {
        readonly string _controllerName, _actionName;

        public RouteExtension(string controllerName, string actionName)
        {
            _controllerName = controllerName;
            _actionName = actionName;
        }

        public RouteExtension(string actionName) : this("", actionName)
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {

            return "";
        }
    }
}
