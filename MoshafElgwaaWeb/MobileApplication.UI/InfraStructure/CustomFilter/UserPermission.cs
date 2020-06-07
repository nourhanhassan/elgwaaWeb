//using QV.Service.Security;
//using Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MobileApplication.DataService;
//using QV.Service.Security;

namespace MobileApplication.UI.InfraStructure
{
    public class UserPermission : AuthorizeAttribute
    {
        public QVEnterprise.ActionType[] Actions { get; set; }
        public string Controller { get; set; }

        public AuthorizationContext Context { get; protected set; }

        public bool IsControllerNotDefine { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.Context = filterContext;
            base.OnAuthorization(filterContext);
        }

        
public UserPermission(params QVEnterprise.ActionType[] actions)
        {
            Actions = actions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            base.AuthorizeCore(httpContext);

            var profile = (UserProfile)httpContext.Session["_Profile"];

            //--Get Permission
            var result = GetPermission(profile);

            //--Check if controller is define in dataBase
            if (result == null)
            {
                IsControllerNotDefine = true;
                return false;
            }

            foreach (var action in Actions)
            {
                var permission = profile.Permission[action];
                if (!permission)
                {
                    return false;
                }
            }

            //-- for pass Permission to view by ViewData
            profile.Permission.Keys.ToList().ForEach(i => this.Context.Controller.ViewData.Add(i.ToString(), profile.Permission[i]));

            return true;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.Result = new ViewResult
            {
                ViewName = IsControllerNotDefine ? "~/Areas/ControlPanel/Views/Shared/ControllerNotDefine.cshtml" : "~/Areas/ControlPanel/Views/Shared/UnAuthorizedUser.cshtml"
            };
        }


        private bool? GetPermission(UserProfile profile)
        
        
        
        {
            var actionService = new CU_ActionService();
            bool result = false;
            try
            {
                string currentController = string.Empty;
                if (string.IsNullOrEmpty(Controller))
                {

                    //--get Rout Data
                    var routData = HttpContext.Current.Request.RequestContext.RouteData;

                    //--current action.
                    currentController = routData.GetRequiredString("controller");
                }
                else
                {
                    currentController = Controller;
                }
                //--Get User permission.
                profile.Permission = actionService.GetPermission(currentController, profile.Id.Value);

                result = true;
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }
    }
}