using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class QuickAddExampleController : BaseController
    {
        //
        // GET: /ControlPanel/testquickAdd/
        public ActionResult Index()
        
        {
            return View();
        }

        public ActionResult TestAttachments()
        {

            return View();
        }
	}
}