using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MobileApplication.DataService;
using MobileApplication.DataService.AutoMapper;
using MobileApplication.UI.InfraStructure;
using QvSMS;

namespace MobileApplication.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(UserProfile), new UserProfileModelBinder());
            AutoMapperConfig.RegisterMappers();


            new MailEngineService().Initialize();
            new SMSEngine().Initialize();


            AppSettingService.HasNotification = Convert.ToBoolean(new AppSettingService().GetHasNotificationAppSetting().Value);
            if (AppSettingService.HasNotification)
            {
                NotificationService._NotificationHub = new NotificationHub();

            }      
        }
    }
}
