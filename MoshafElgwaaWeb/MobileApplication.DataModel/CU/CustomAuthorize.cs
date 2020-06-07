//using OperationalPlanning_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


namespace Service.Contracts.Models.Security
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private class Http401Result : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                // Set the response code to 401.
                var myvar = MvcApplication.UserData;
                context.HttpContext.Response.StatusCode = (myvar == null) ? 402 : 401;
                context.HttpContext.Response.Write("AuthorizationLostPleaseLogOutAndLogInAgainToContinue");
                context.HttpContext.Response.End();
            }
        }

        /// <summary>
        /// an function or controller wraped by [CustomAuthorize] attrbute
        /// redirect the user to this function before any excutions to check for user Authentication&Authroization.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ///get Rout Data
            var routData = httpContext.Request.RequestContext.RouteData;
            string currentAction = routData.GetRequiredString("action");//current controller.
            string currentController = routData.GetRequiredString("controller");//current action.

            bool result = false;
            var user = (Security.UserData)HttpContext.Current.Session["User"];

            if (user != null) // the session has expired
            {
                
                //check user permission.
                result = CheckPermission(currentAction, currentController, httpContext);
            }

            else if (CheckCookie())//check user Cookie.
            {
                //check user permission.
                result = CheckPermission(currentAction, currentController, httpContext);
            }
            return result;
        }

        /// <summary>
        /// Check User Permission.
        /// over Current Controller and Action.
        /// </summary>
        /// <param name="currentAction"></param>
        /// <param name="currentController"></param>
        /// <returns></returns>
        private bool CheckPermission(string currentAction, string currentController,HttpContextBase httpContext)
        {
            bool result = false;
       
            // Check User permission.
            var actionSecurety = new ActionSecurity(currentController);
            try
            {
              
                result = actionSecurety.Permission[(QVEnterprise.ActionType)Enum.Parse(typeof(QVEnterprise.ActionType), currentAction.ToLower() == "Index".ToLower()  ? "View": currentAction,true)];
              //  result = true;
            }
            catch (Exception)
            {
                result = true;
            }
            HttpContext.Current.Items["Permission"] = actionSecurety.Permission;
            if (currentController == "Authen") return true;
            return result;
        }

        /// <summary>
        /// handel Unauthorized user
        /// eather redirect the user to the login page if he doesn,t logged in
        /// or to Unauthorized page if he has no permission over curren action.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //For Rederict To Error Page
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
              //  filterContext.Result = new Http401Result();
                filterContext.Result = new JavaScriptResult { Script = "window.location.href='/Security/Login'" };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
                var myvar = MvcApplication.UserData;
                if (myvar == null)// if user dosen't logged in. 
                {
                    //use pase handdling to solve return url and some extera isssues.
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Security" }, { "action", "Login" } });
                    //base.HandleUnauthorizedRequest(filterContext);
                }
                else // user has no permission to do this action.
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Security" }, { "action", "UnAuthorizedUser" } });
                }
            }
        }

        /// <summary>
        /// Set user Name and user Password in Cookie
        /// to remember thr user data 
        /// in next requists.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        public void SetCookie(string userName, string pass)
        {
            HttpCookie aCookie = new HttpCookie("UserData");
            aCookie.Values.Add("userName", userName);
            aCookie.Values.Add("password", pass);
            aCookie.Expires = DateTime.Now.AddDays(1);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        /// <summary>
        /// Check for user Cookie
        /// and fill the settion by
        /// the current user ata if he has valid cookie.
        /// </summary>
        /// <returns></returns>
        public bool CheckCookie()
        {
            bool result = false;
            if (HttpContext.Current.Request.Cookies["UserData"] != null)
            {
                string userName = QvLib.Security.DataProtection.Decrypt(HttpContext.Current.Request.Cookies["UserData"]["userName"]);
                string password = HttpContext.Current.Request.Cookies["UserData"]["password"];
                QvLib.Identity.LogIn(userName, QvLib.Security.DataProtection.Decrypt(password), false);
                var user = new EDLQ_AppEntities().CU_Employee.AsQueryable().Where(e => e.LoginName == userName && e.Password == password).FirstOrDefault();
                if (user != null)
                {
                    var userData = new Security.UserData
                    {
                        userId = user.ID,
                        userName = user.LoginName,
                        password = user.Password
                    };

                    // fill session
                    MvcApplication.UserData = userData;

                    result = true;
                }

            }
            return result;
        }
    }
}