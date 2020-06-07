

using MobileApplication.DataModel;
using MobileApplication.DataService;
using MobileApplication.UI.ControlPanel.Controllers;
using MobileApplication.UI.InfraStructure;
using QVEnterprise;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class EmployeeController : ViewGridBaseController<EmployeeModel>
    {
        private CU_EmployeeService _CU_EmployeeService;
        private CU_Role_ProgramService _CU_Role_ProgramService;
        private CU_Employee_RoleProgramService _CU_Employee_RoleProgramService;
        private CU_AccountService _CU_AcountService;
        public EmployeeController()
     //       : base(new CU_Employee_RoleProgramService())      // just in case of changing somr thing in the service but the inserted service must inherit from IManageBaseService<[same model]>
        {
            _CU_EmployeeService = new CU_EmployeeService();
           _CU_Role_ProgramService = new CU_Role_ProgramService();
           _CU_Employee_RoleProgramService = new CU_Employee_RoleProgramService();
           _CU_AcountService = new CU_AccountService();
        }

        public override JsonResult Save(EmployeeModel Model, string command, InfraStructure.UserProfile profile)
        {
            if (Model.ID != 0)
            {
              //  ModelState["password"].Errors.Clear();
                ModelState["LoginName"].Errors.Clear();
                ModelState["Email"].Errors.Clear();
                ModelState["strUserRoles"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                if (Model.strUserRoles != null)
                {
                    Model.UserRoles = js.Deserialize<List<int>>(Model.strUserRoles);
                }
                Model.UserId = profile.Id;
                //Model.ActionName = "AddEdit";
                Model.ActionName = this.ControllerContext.HttpContext.Request.UrlReferrer.ToString();

                var ID = _CU_EmployeeService.Save(Model);

                return Json(new { ID, command }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { ID = 0 }, JsonRequestBehavior.AllowGet); }

          
        }

        [HttpPost]
        public JsonResult GetUserRoles(EmployeeModel currModel)
        {
            var d = _CU_EmployeeService.GetUserRole(currModel);
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserRoles(string iUserID, string strRolesIds, UserProfile profile)
        {
            List<int> arrRolesToSave = new List<int>();
            int? logId;
            int iniUserID = int.Parse(iUserID);
            var arrStrRoles = strRolesIds.Trim().Split(',');
            for (int i = 0; i < arrStrRoles.Length; i++)
            {
                arrRolesToSave.Add(int.Parse(arrStrRoles[i]));
            }
            bool d = _CU_Employee_RoleProgramService.Save(arrRolesToSave, iniUserID, null, out logId, profile.Id.Value, "AddEdit");
            return Json(new { d }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(int iUserID, string newPassword)
        {
            var objUser = _CU_EmployeeService.GetModelByID(iUserID);
            _CU_AcountService.ChangePassword(iUserID, newPassword);
            var mailResult = MailService.ChangePasswordMailByAdmin(HttpContext.Request.Url.Host, objUser.Email, newPassword);
            return Json(new { d = mailResult }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckDuplicateEmail(string Email, int ID)
        {
         var   Return = _CalledService.CheckDuplicate("Email", Email, (int)ID);
         return Json(!Return, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckDuplicateLoginName(string LoginName, int ID)
        {
            var Return = _CalledService.CheckDuplicate("LoginName", LoginName, (int)ID);
            return Json(!Return, JsonRequestBehavior.AllowGet);
        }
	}
}