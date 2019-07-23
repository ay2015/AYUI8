using Ay.MvcFramework;
using RDS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RDS.Controllers
{
    public interface IViewStartView
    {

    }

    public class ViewStartController : Controller
    {
        #region Initialize
        IViewStartView View { get; set; }
        public ViewStartModel Model { get; set; } = new ViewStartModel();
        public ViewStartController() : base()
        {
        }
        public ViewStartController(IViewStartView view) : base()
        {
            View = view;
        }
        #endregion

        public override void Initialize()
        {


        }

    }
}
