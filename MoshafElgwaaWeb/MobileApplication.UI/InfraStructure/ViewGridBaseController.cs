using MobileApplication.DataService;
using MobileApplication.UI.InfraStructure;
using Service.Contracts;
using MobileApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using GenericEngine.Service.SharedService;
using MobileApplication.UI.Areas.ControlPanel.Controllers;

namespace MobileApplication.UI.ControlPanel.Controllers
{
    public class ViewGridBaseController<T> : BaseController
      where T : class //,GenericEngine.ServiceContract.Infrastructure.IGenericModel
    {
        protected IManageBaseService<T> _CalledService;     
        protected string _Title;
        public ViewGridBaseController()
        {
            _CalledService = new ManageBaseService<T>();
        }
        public ViewGridBaseController(string title)
        {
            _CalledService = new ManageBaseService<T>();
            _Title = title;
        }
        public ViewGridBaseController(IManageBaseService<T> baseService)
        {

            _CalledService = baseService;
        }



        //edited
        [HttpGet]
        [UserPermission(QVEnterprise.ActionType.View)]
        public virtual ActionResult Default(UserProfile profile)
        {
            ViewBag.pagingMethodName = "/ControlPanel/" + HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString() + "/GetDataList";

       
            if (string.IsNullOrEmpty(_Title))
                return View("Default", Activator.CreateInstance(typeof(T)));
            else
                return View("Default", Activator.CreateInstance(typeof(T), _Title));

        }

        [HttpGet]
        public virtual JsonResult GetDataList(int page, int count, Dictionary<string, string> sorting, Dictionary<string, string> filter, UserProfile profile)
        {
            var URL = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
            GridModel gridModel = _CalledService.GetDataList(sorting, filter, page, count, URL, (int)profile.Id);

            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual bool Delete(int id, UserProfile profile, string tableName = "")//, ControlPanelProfile profile
        {
            var URL = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
            return _CalledService.Delete(id, (profile.Id.HasValue ? profile.Id.Value : 0), URL);//, (int)profile.Id, URL
        }


        protected virtual ActionResult AddEdit(int Id)
        {
            T AddEditModel;
            if (Id != 0)
            {
                AddEditModel = _CalledService.GetById(Id);

                return View("AddEdit", AddEditModel);
            }
            else
            {
                AddEditModel = (T)Activator.CreateInstance(typeof(T));
            }
            return View("AddEdit", AddEditModel);
        }

        [HttpGet]
        [UserPermission(QVEnterprise.ActionType.Insert)]
        public virtual ActionResult Add()
        {
            return AddEdit(0);
        }
        [HttpGet]
        [UserPermission(QVEnterprise.ActionType.Update)]

        public virtual ActionResult Edit(int Id)
        {
            return AddEdit(Id);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual JsonResult Save(T Model, string command, UserProfile profile)
        {
            if (ModelState.IsValid) 
            {
                var URL = this.ControllerContext.RouteData.Values;

                var curControllerName = URL["controller"];
                var ActionName = this.ControllerContext.HttpContext.Request.UrlReferrer.ToString();


                if (Request.QueryString["logActionName"] != null)			 
                {
                    ActionName = this.ControllerContext.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority + Request.QueryString["logActionName"];
                }
                else
                {
                    ActionName = this.ControllerContext.HttpContext.Request.UrlReferrer.ToString();
                }

                var ID = _CalledService.Save(Model, (profile.Id.HasValue ? profile.Id.Value : 0), (string)ActionName);
                return Json(new { ID, command }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var errors = ModelState.Where(m => m.Value.Errors.Count > 0);
                return Json(new { ID = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public virtual ActionResult DuplicateName(string ColName, string Value, int ID = 0, string tableName = "")
        {
            bool Return = false;

            Return = _CalledService.CheckDuplicate(ColName, Value, ID, tableName);// ,"CU_Employee"
            return Json(!Return, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckDuplicate(int? ID)
        {
            string Name = (string)Request.Params[0];
            var Return = _CalledService.CheckDuplicate(Request.Params.AllKeys[0].ToString(), Name, ID == null ? 0 : (int)ID);
            return Json(!Return, JsonRequestBehavior.AllowGet);
        }

        public virtual IEnumerable<SelectListItem> GetlookupList(Type Lookuptype, string textCoulumn = "Name", string valueCoulomn = "ID")
        {

            return (IEnumerable<SelectListItem>)typeof(LookupManger<>).MakeGenericType(Lookuptype).GetMethod("GetSelectListItem").Invoke(null, new object[] { textCoulumn, valueCoulomn });
        }
    }
}
