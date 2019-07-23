using Ay.MvcFramework;
using RDS.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RDS.Controllers
{
    public interface IIndexView
    {

    }

    public class IndexController : Controller
    {
        #region Initialize
        IIndexView View { get; set; }
        public IndexModel Model { get; set; } = new IndexModel();
        public IndexController() : base()
        {
        }
        public IndexController(IIndexView view) : base()
        {
            View = view;
        }
        #endregion

        public override void Initialize()
        {


        }

    }
}
