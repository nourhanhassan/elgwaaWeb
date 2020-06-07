
using MobileApplication.DataService;
using MobileApplication.DataModel ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApplication.UI.InfraStructure;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class NotificationController : BaseController
    {
        //
        // GET: /ControlPanel/Notification/

        readonly private NotificationService _NotificationService;
        readonly private NotificationUserService _NotificationUserService;

        public NotificationController()
        {
            _NotificationService = new NotificationService();
            _NotificationUserService = new NotificationUserService();
        }

        public ActionResult Default(UserProfile Up)
        {
            //List<NotificationModel> notifications = _NotificationService.SelectByUserId((int)Up.Id).Select(x => new NotificationModel(x)).ToList();
            //ViewBag.pagingMethodName = "GetNotifications";
            return View();
        }

        public ActionResult GetNotifications(UserProfile profile, int skip = 0, int take = 10)
       {
            ViewBag.CurrUserID = profile != null ? profile.Id : 0;
            int total = 0;

            List<NotificationModel> notifications = _NotificationService.SelectByUserId((int)profile.Id, skip, take, out total);
            ViewBag.total = total;
            return PartialView(notifications);

        }

        //public JsonResult GetNotifications(int page, int count, Dictionary<string, string> sorting, Dictionary<string, string> filter)
        //{
        //    //List<NotificationModel> notifications = _NotificationService.SelectByUserId((int)Up.Id).Select(x => new NotificationModel(x)).ToList();
        //    List<NotificationModel> notifications = new List<NotificationModel>();
        //    var d = new { count = notifications.Count(), data = notifications };
        //    return Json(d, JsonRequestBehavior.AllowGet);

        //}

    }
}
