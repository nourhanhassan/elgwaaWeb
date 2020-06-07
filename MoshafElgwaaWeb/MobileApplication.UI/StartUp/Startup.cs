using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Web.Configuration;
using MobileApplication.DataService;

[assembly: OwinStartup(typeof(SignalRNotification.Startupp))]
namespace SignalRNotification
{
    public class Startupp
    {
        public void Configuration(IAppBuilder app)
        {
            if (AppSettingService.HasNotification)
            {
                // Any connection or hub wire up and configuration should go here
                app.MapSignalR();
            }
        
          
        }
    }
}