using MobileApplication.UI.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MobileApplication.DataModel.QvDataAnnotation;
using MobileApplication.DataService;
using MobileApplication.DataModel;
using DataModel.Enum;


namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    [ControlPanelUserAuthorize]
    public class BaseController : Controller
    {
        //  [CustomAuthorize]  
        public string controllerName { get; set; }
        public IEnumerable<NotificationModel> Notifications { get; set; }
        protected  UserProfile profile { get; set; }
        private readonly CU_ActionService _ActionService;

        public BaseController()
        {
             profile = (UserProfile)System.Web.HttpContext.Current.Session["_Profile"];
            _ActionService = new CU_ActionService();
           
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            profile = (UserProfile)HttpContext.Session["_Profile"];
            controllerName = this.ControllerContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();


            //-- for layout data----------//
            ViewBag.UserID = profile.Id;
            ViewBag.UserName = GetSubStringUserName(profile.Name);
            ViewBag.CurTime = UmAlQura.GetHijriDateFullString(DateTime.Now);
            //ViewBag.CurTime = QvLib.QVUtil.Date.GetHijriDate();
            ViewBag.LastVisit = profile.LastVisitTime;

            //-- for log method------------//
            var URL = this.ControllerContext.HttpContext.Request.RawUrl;
            var isAngular = this.ControllerContext.HttpContext.Request.Headers["FROM-ANGULAR"] == null ? false : true;
            var isNotLog = (filterContext.ActionParameters.ContainsKey("bNotLog") && filterContext.ActionParameters["bNotLog"] != null) ? Convert.ToBoolean(filterContext.ActionParameters["bNotLog"]) : false;
            var IsChildAction = this.ControllerContext.IsChildAction ? this.ControllerContext.ParentActionViewContext.Controller.ControllerContext.HttpContext.Request.CurrentExecutionFilePath == this.ControllerContext.Controller.ControllerContext.HttpContext.Request.CurrentExecutionFilePath ? true : false : false;

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                IEnumerable<PageModel> pages = new List<PageModel>();
                //  List<PageModel> pages = new List<PageModel>();

                var allPages = new CU_PageService().ParentPageList.Where(p => p.IsVisible && p.IsDeleted == false && (p.CU_Role_Page.Where(s => QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0 || p.CU_Page1.Any(g => g.CU_Role_Page.Where(s => QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0)));
                pages = new CU_PageService().ParentPageList.Where(p => p.IsVisible && p.IsDeleted == false && p.ParentID == null && (p.CU_Role_Page.Where(s => QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0 || p.CU_Page1.Any(g => g.CU_Role_Page.Where(s => QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0)));

                var pagesPermissions = _ActionService.GetPermission(allPages.Select(a => a.URL.ToLower()).ToList(), profile.Id.Value);

                foreach (var i in pages)
                {
                    List<PageModel> childPages = new List<PageModel>();

                    // var parentPagePermission = _ActionService.GetPermission(i.URL, profile.Id.Value);
                    var parentPagePermission = pagesPermissions[i.URL.ToLower()];
                    if (parentPagePermission.ContainsValue(true)) i.HasPermission = true;


                    var allChildPages = i.CU_Page1.Where(p => p.CU_Role_Page.Where(s => p.IsVisible && p.IsDeleted == false && QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0).ToList();
                    if (allChildPages.Count != 0)
                    {
                        foreach (var item in allChildPages)
                        {
                            //var childPagePermission = _ActionService.GetPermission(item.URL, profile.Id.Value);
                            var childPagePermission = pagesPermissions[item.URL.ToLower()];
                            if (childPagePermission.ContainsValue(true))
                            {
                                item.HasPermission = true;
                                childPages.Add(item);
                            }
                        }
                        if (childPages.Count != 0)
                        {
                            i.CU_Page1 = childPages;
                            i.HasPermission = true;
                        }

                        else i.CU_Page1 = new List<PageModel>();
                    }
                }


                ViewBag.Pages = pages;
                ViewBag.ParentPages = pages;

                //----------------------------//
               // ViewBag.UserType = profile.UserType;
               // ViewBag.Currency = new SettingService().GetCurrency();
               // ViewBag.CountryID = new SettingService().GetDefaultCountryID();
                ViewBag.CurrentPage = new CU_PageService().GetPageModelByPageURL(controllerName);
                //------------------------Notifications--------------------------------------------------//
                var unseenNotificationsModelList = new NotificationService().GetUnseenNotifications(profile.Id.Value).OrderByDescending(a => a.CreateDate);

                var clientNotificationsList = new List<NotificationClientObject>();

                foreach (NotificationModel not in unseenNotificationsModelList)
                {
                    NotificationClientObject notification = new NotificationClientObject();

                    notification.Message = not.Message;
                    notification.ID = not.ID.ToString();
                    notification.Time = not.CreateDate.ToString();
                    notification.Link = not.Link;
                    clientNotificationsList.Add(notification);

                }

                ViewBag.UnSeenNotifications = clientNotificationsList;
                //--------------------------------------------------------------//
            }
        }

        public ViewResult Error()
        {
            return View("~/Areas/ControlPanel/Views/Shared/Error.cshtml");
        }

        public ViewResult UnAuthorizedUser()
        {
            return View("~/Areas/ControlPanel/Views/Shared/UnAuthorizedUser.cshtml");
        }
        private string GetSubStringUserName(string UserName)
        {

            if (UserName.Length > 20)
            {
                int pos = UserName.LastIndexOf(" ", 20);
                if (pos == -1)   //if no space exist
                {
                    pos = 20;
                }
                UserName = UserName.Substring(0, pos) + " ...";
            }
            return UserName;
        }
        protected List<PageModel> getChildPages(int? profileID)
        {
           string controllerName = HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var allChildPages = new CU_PageService().GetPageChilds(controllerName).Where(p => p.CU_Role_Page.Where(s => QvLib.Identity.Roles.Contains(s.IdRoleProgram)).Count() > 0);
            List<PageModel> childPages = new List<PageModel>();
            foreach (var item in allChildPages)
            {
                var childPagePermission = _ActionService.GetPermission(item.URL, profileID.Value);
                if (childPagePermission.ContainsValue(true))
                {
                    item.HasPermission = true;
                    childPages.Add(item);
                }
            }
            return childPages.OrderBy(p => p.PageOrder).ToList();
        }
        // to be continued.....
        //public JsonResult FilterDropDownList()//string FunctionName, object ForignKey
        //{

        //    Globals _Globals = new Globals();
        //    IEnumerable<SelectListItem> dbObjectList = (IEnumerable<SelectListItem>)_Globals.GetType().GetMethod("").Invoke(_Globals, null);

        //    //  MvcHtmlString DropDownListHtml = System.Web.Mvc.Html.SelectExtensions.DropDownList(html,prop.Name, dbObjectList, new { ng_model = "searchObject." + prop.Name + "", @class = " form-control" });

        //    return Json(new { data = dbObjectList }, JsonRequestBehavior.AllowGet);

        //}
    }
}
