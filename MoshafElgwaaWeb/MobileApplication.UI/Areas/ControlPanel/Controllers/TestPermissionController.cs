using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MomtazExpress.UI.InfraStructure;

namespace MomtazExpress.UI.Areas.ControlPanel.Controllers
{
    public class LogController : BaseController
    {

       [UserPermission(QVEnterprise.ActionType.View)]

        public ActionResult Default()
        {
            return View();
        }
	}
}