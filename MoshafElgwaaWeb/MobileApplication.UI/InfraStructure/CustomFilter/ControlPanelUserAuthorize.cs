using QvLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MobileApplication.UI.InfraStructure;

namespace MobileApplication.UI.InfraStructure
{
    public class ControlPanelUserAuthorize : AuthorizeAttribute
    {
        private class Http401Result : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                // Set the response code to 403.
                context.HttpContext.Response.StatusCode = 403;
                context.HttpContext.Response.Write("AuthorizationLostPleaseLogOutAndLogInAgainToContinue");
                context.HttpContext.Response.End();
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
            if (httpContext.Session != null)
            {
                var profile = (UserProfile)httpContext.Session["_Profile"];
                HttpContext.Current.Session.Timeout = 120;
                //--Check Seesion
                if (profile != null && profile.Id != null)
                {                   
                    return true;
                }
                else
                {
                  
                     profile = new UserProfile();
                    if (CheckCookie())//check user Cookie login from cu.
                    {
                        profile.Id = Identity.UserID;
                        profile.LoginName = Identity.UserFullName;
                        profile.Name = Identity.UserFullName;
                        profile.LastVisitTime = Identity.LastVisit;
                        profile.IdDepartment = Identity.DepartmentID.HasValue ? Identity.DepartmentID.Value : 0;
                        profile.IdHeadDepartment = Identity.HeadDepartmentID.HasValue ? Identity.HeadDepartmentID.Value : 0;
                        HttpContext.Current.Session["_Profile"] = profile;
                        HttpContext.Current.Session.Timeout = 120;
                        return true;
                    }
                    else return false;
                    
                }
            }

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new Http401Result();
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary(
                                       new { area = "ControlPanel", controller = "Account", action = "Default", returnUrl = filterContext.RequestContext.HttpContext.Request.Url }));
            }
        }

        public bool CheckCookie()
        {

            if (Extention.GeKeyValue<bool>("IN_DEV")) // development mode
            {
                return false; // no cookie is set for this project
            }
            else
            {
                bool bIsLoggedIn = QvLib.Identity.CheckLogin();
                if (!bIsLoggedIn)
                {
                    HttpContext.Current.Response.Redirect(Extention.GeKeyValue<string>("ERPMainLogin"));
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
    }
}