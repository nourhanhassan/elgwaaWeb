using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.InfraStructure
{
    public class UserProfileModelBinder : IModelBinder
    {
        private const string ProfileSessionKey = "_Profile";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Some modelbinders can update properties on existing model instances. This
            // one doesn't need to - it's only used to supply action method parameters.
            if (bindingContext.Model != null)
            {
                throw new InvalidOperationException("Cannot update instances");
            }

            // Return the profile from Session[] (creating it first if necessary)
            var profile = (UserProfile)controllerContext.HttpContext.Session[ProfileSessionKey];
            if (profile == null)
            {
                profile = new UserProfile();
                controllerContext.HttpContext.Session[ProfileSessionKey] = profile;
            }
          
            return profile;
        }
    }
}