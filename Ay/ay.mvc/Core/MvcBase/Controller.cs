using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Ay.MvcFramework.Internal.Attributes;

namespace Ay.MvcFramework
{
    public class Controller : ControllerBase
    {
        public Controller()
        {
            var attribute = this.GetType().GetCustomAttributes(typeof(ViewAttribute), false);

            if (attribute!=null && attribute.Length > 0)
            {
                List<ViewAttribute> v = new List<ViewAttribute>();

                foreach (var item in attribute)
                {
                    var _1 = item as ViewAttribute;
                    v.Add(_1);
                }
                var _2 = v.OrderBy(a => a.Order).ToList();
                foreach (var item in _2)
                {
                    item.OnRendered(this);
                }
            }

            //IAuzhorizationFilter、IActionFilter、IResultFilter、IExceptionFilter

            //protected internal ViewResult View() {

            //}


        }

        public virtual void Initialize() { }
    }


}
