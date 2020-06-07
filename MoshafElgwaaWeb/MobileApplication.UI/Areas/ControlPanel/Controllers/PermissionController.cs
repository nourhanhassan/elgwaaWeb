

using MobileApplication.DataModel;
using MobileApplication.DataService;
using MobileApplication.UI.InfraStructure;

//using QV.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class PermissionController : BaseController
    {

        private readonly PermissionService _permissionService;
        private readonly CU_LogService _logService;


        public PermissionController()
        {
            _permissionService = new PermissionService();
            _logService = new CU_LogService();
        }

        [HttpGet]
        [UserPermission(QVEnterprise.ActionType.View)]
        public ActionResult Default()
        {
            ViewBag.pagingMethodName = "/ControlPanel/Permission/Get";
            return View();
        }

        [HttpGet]
        public JsonResult Get(int page, int count, Dictionary<string, string> sorting, Dictionary<string, string> filter,InfraStructure.UserProfile prof )
        {
            var result = _permissionService.Permission(sorting, filter);
            UserProfile profile = (UserProfile)HttpContext.Session["_Profile"];
            var d = new
            {
                count = 30,
                data = result
            };
            var URL = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
            _logService.Read(URL, profile.Id);

            return Json(d, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public int SetPermission(List<PermissionModel> lstModel, UserProfile profile)
        {
            int success = 0;
            if (lstModel != null)
            {
                foreach (var model in lstModel)
                {
                    success = _permissionService.Save(model.ID, model.PageName, model.View, model.Insert, model.Update, model.Delete, model.Password,model.Admin,model.Report, profile.Id, "Default");
                }
            }
            else
            {
                success = 1;
            }
            return success;
        }

    }
}