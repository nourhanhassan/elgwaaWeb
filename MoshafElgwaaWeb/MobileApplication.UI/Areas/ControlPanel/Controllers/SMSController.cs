using QvSMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class SMSController : Controller
    {
        //
        // GET: /ControlPanel/SMS/
        public ActionResult GetAll()
        {
            var list = new SMSEngine().GetSMSMessageModelList().OrderByDescending(a => a.CreationDate).ToList();
            return View(list);
        }
	}
}