
using MobileApplication.DataService;
using QvLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApplication.DataModel;
using QvLib.Security;
using DataModel.hashSaltProtection;
using MobileApplication.UI.InfraStructure;


namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly CU_AccountService _AccountService;
          private readonly CU_LogService _logService;

        
        
        
        
        public AccountController()
        
        
        {
            _AccountService = new CU_AccountService();
           _logService = new CU_LogService();
         
        }

        [HttpGet]
        public ActionResult Default(UserProfile profile, string returnUrl)
        {
            //check Cookie
            if (UserProfile.CheckUserCookie(profile))
            {
                return returnUrl != null ? (ActionResult)Redirect(returnUrl) : RedirectToAction("Default", "Dashboard", new { area = "ControlPanel" });
            }
            if ( Identity.UserID!=null) // login from cu
            {
                profile.Id = Identity.UserID;
            
                profile.LoginName = Identity.UserFullName;
                profile.Name = Identity.UserFullName;
                profile.LastVisitTime = Identity.LastVisit;

                HttpContext.Session["_Profile"] = profile;
                
            }

            //Check Seesion
            if (profile != null && profile.Id != null)
            {



                return returnUrl != null ? (ActionResult)Redirect(returnUrl) : RedirectToAction("Default", "Dashboard", new { area = "ControlPanel" });
            }
          
          
            //Login page
            return View();
        }

        [HttpPost]
        public ActionResult SubmitLogin(LoginFormModel model, UserProfile profile)
        {

            string strsalt = string.Empty;
            var Password = hashSaltProtection.GeneratePasswordHash(model.Password, out strsalt);
            var Salt = strsalt;
            if (ModelState.IsValid)
            {
                var user = _AccountService.ValidateLoginData(model.UserName, model.Password);

                //--Not vaild user
                if (user == null)
                {
                    ModelState.AddModelError("error", "يوجد خطأ فى اسم المستخدم أو كلمة المرور");
                    return View("Default", model);
                }

                //--Create Session
                UserProfile.CreateUserSession(profile, user);
                _AccountService.SetLastVisitTime(user.ID.Value);

                //--Create Cookie
                if (model.RememberMe)
                {
                    UserProfile.CreateUserCookie(profile, user,model.Password);
                }

                //--Check For RequestedUrl
                if (model.returnUrl != null)
                {
                    _logService.Login(profile.Id);

                    return Redirect(model.returnUrl);
                }
            }
             _logService.Login(profile.Id);

             return RedirectToAction("Default", "DashBoard", new { area = "ControlPanel" });
        }


        #region forgetPassword
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SubmitForgetPassword(ForgetPasswordModel model)
        {
            var IsSuccess = false;
            if (ModelState.IsValid)
            {
                var emp = _AccountService.ValidateMail(model.Email);
                if (emp != null)
                {
                    string UserGUID =_AccountService.ResetPasswordRequest(emp.ID.Value);
                    IsSuccess = false; //MailService.ForgotPasswordMail(HttpContext.Request.Url.Host, Request.Url.Scheme + "://" + Request.Url.Authority + "/ControlPanel/Account/ResetPassword/" + UserGUID, model.Email);
                }
                return Json(new { IsSuccess = IsSuccess, emp = emp, model = model }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = IsSuccess, model = model, ModelStateIsValid = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckMailExistance(string Email, UserProfile profile)
        {
            var emp = _AccountService.ValidateMail(Email);
            bool IsEmailExist = emp != null ? true : false;
            return Json(IsEmailExist, JsonRequestBehavior.AllowGet);
        }
        #endregion forgetPassword

        #region ResetPassword
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            var ResetModel = _AccountService.GetResetPasswordLink(id);
            if (ResetModel.IsExpired)
                return View("ExpiredLink");
            else
            {
                return View(ResetModel);
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model, UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                _AccountService.SaveNewPassword(model.EmployeeID, model.NewPassword, model.GUID);
                return RedirectToAction("Default");
            }
            return View("ResetPassword", model);
        }
        #endregion ResetPassword



        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitChangePassword(ChangePassWordModel model, UserProfile profile)
        {
            _AccountService.ChangePassword(profile.Id.Value, model.NewPassword);
           // MailService.ChangePasswordMail(HttpContext.Request.Url.Host, profile.Email, profile.LoginName, model.NewPassword);
            return RedirectToAction("Default");
        }



        [HttpGet]
        public JsonResult Checkcorrectpassword(string OldPassword, UserProfile profile)
        {
            if (profile != null && profile.Id.HasValue)
            {
                return Json(_AccountService.ValidatePassword(profile.Id.Value, OldPassword), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Logout(UserProfile profile)
        {
            if (profile != null && profile.Id.HasValue)
            {
               // _logService.LogOut(profile.Id);
                _AccountService.SetLastVisitTime(profile.Id.Value);
            }
            UserProfile.ClearCookie();
            UserProfile.ClearSession();
            Identity.LogOut();
          //  return RedirectToAction("Default");

          return View("~/Views/Account/Default.cshtml");
        }

        public ViewResult UnAuthorizedUser()
        {
            return View("~/Views/Shared/UnAuthorizedUser.cshtml");
        }

    }
}
