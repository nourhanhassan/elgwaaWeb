using QV.Service.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.AutoMapper;
using Service;
using Service.Contracts.Models_API;

namespace MomtazExpress.UI.Areas.API.Controllers
{
    public class LoginnController : Controller
    {
        //
        // GET: /API/Login/

        readonly private AppUsersService _appUsersService;
        readonly private LoginService _loginService;
        readonly private AccountService _AccountService;
        readonly private SendSMSService _sendSMSService;
        public LoginnController()
        {
            _appUsersService = new AppUsersService();
            _loginService = new LoginService();
            _AccountService = new AccountService();
            _sendSMSService = new SendSMSService();
        }

        //[HttpPost]
        public ActionResult checklogin(string loginMobile, string loginPassword, string registerationId, string registerationType)
        {

            int usrret = new LoginService().checklogin(loginMobile, loginPassword);
            if (usrret == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            //else if (usrret == -1)//not verified
            //{
                //generate verification code (this part of sending smsm is after registration)
                //Random rnd = new Random();
                //int code = rnd.Next(1000, 9999); //generate randome code from 4 digits
                //send message with it

                //insert record in AppUser_SMSMessage table in db
                //check in the app on "not verified" if exist don't login and open the verification page

               // _sendSMSService.CheckCanSendVerificationCodeToAppUser()(this check will be when click generate another code)

            //    return Json("not verified", JsonRequestBehavior.AllowGet);
            //}
            else
            {
                //Set the registeration id for the user's device
                _appUsersService.setRegisterationID(usrret, registerationId, registerationType);

                MvcApplication.UserData = _appUsersService.GetAppUserDataByID(usrret);

                return Json(_loginService.GetLoginModel(usrret), JsonRequestBehavior.AllowGet);
            }
        }

        public void SetUserRegisterationID(string registerationId, string registerationType)
        {
            _appUsersService.setRegisterationID(MvcApplication.UserData.ID, registerationId, registerationType);
        }
       // [HttpPost]
        public void Logout(string registerationType,int appUserID)
        {
            //if (MvcApplication.UserData != null)
            //{
                //int userID = MvcApplication.UserData.ID;
            _appUsersService.RemoveUserRegisterationID(appUserID, registerationType);

            //}
        }

        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            var IsSuccess = false;
            if (ModelState.IsValid)
            {
                var appUser = _appUsersService.ValidateMail(email);
                if (appUser != null)
                {
                    string UserGUID = _appUsersService.ResetPasswordRequest(appUser.ID);
                    IsSuccess = MailService.ForgotPasswordMail(HttpContext.Request.Url.Host, Request.Url.Scheme + "://" + Request.Url.Authority + "/API/AppUser/ResetPassword?id=" + UserGUID, email);
                    //IsSuccess = true;
                }
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            return Json(IsSuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyCode(int appUserID,string code)
        {
           var ret= _sendSMSService.verifyCode(appUserID, code);
            if(ret==null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
        }

       //  [HttpPost]
        public JsonResult ResendCode(int appUserID)
        {
            //var ret = _sendSMSService.CheckCanSendVerificationCodeToAppUser(appUserID);
            // if(ret)
            // {
                var ret= _sendSMSService.sendVerificationMessageToUser(appUserID);
             //}
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        ////Get Permissions for specific UserID for Mobile Pages
        //public ActionResult Getpermissions(int userID)
        //{
        //    UsersService user = new UsersService();
        //    NotificationService notificationService = new NotificationService();
        //    LoginModel login = new LoginModel(userID);
        //    MobileConfigurationService configser = new MobileConfigurationService();
        //    CU_User usr = user.Select(userID);
        //    //   login.UserID = userID;
        //    login.LoginName = usr.LoginName;
        //    login.Name = usr.Name;
        //    login.DateTimeNowHijri = QvLib.QVUtil.Date.GregToHijri(DateTime.Now);
        //    var UmAlQuraCalendar = new UmAlQuraCalendar();
        //    CultureInfo Hijri = new CultureInfo("ar-SA") { DateTimeFormat = { Calendar = UmAlQuraCalendar } };
        //    login.TimeSAOnly = (DateTime.Now).ToString("hh:mm tt", Hijri);
        //    login.UserTypeId = usr.UserTypeID;
        //    login.AssistantTo = usr.AssistantTo;
        //    Dictionary<string, string> allConfig = new Dictionary<string, string>();
        //    foreach (var config in configser.AllConfigurations)
        //    {
        //        allConfig.Add(config.Name, config.Value);
        //    }
        //    login.config = allConfig;
        //    login.TotalUnreadNotificationsCount = notificationService.GetUserUnreadNotificationsCount(usr.ID);
        //    return Json(login, JsonRequestBehavior.AllowGet);
        //}
	}
}