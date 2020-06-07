using DataModel.Enum;
using MobileApplication.DataModel;
using MobileApplication.DataModel.NotificationModels;
using MobileApplication.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class DashBoardController : BaseController
    {
        //
        // GET: /ControlPanel/DashBoard/
        public ActionResult Default()
        {
             //this is for test
            
           // List<string> ids = new List<string>();
           // ids.Add("95638a7422e88ebed76d68b9d42dffb965c46814fe9f34c0eea69ade1320da6a");
            /*
            Dictionary<string, object> test = new Dictionary<string, object>();
            test.Add("test", "data");

            List<NotificationActionModel> actions = new List<NotificationActionModel>();
            actions.Add(new NotificationActionModel() { icon = "emailGuests", title = "ACCEPT", callback = "window.acceptCallbackName", foreground = true });
            actions.Add(new NotificationActionModel() { icon = "snooze", title = "REJECT", callback = "window.rejectCallbackName", foreground = true });
            */
           // PushSharpService _pushSharpService = new PushSharpService();
          //  _pushSharpService.SendFCMNotification(ids, "This is my notification","notification titleeeee",null,test,actions,true);
           // _pushSharpService.SendAPNSNotification(ids, "This is my notification to ios", "", null, null, (int)ReceiverTypeEnum.Merchant, true);

       //    new SearchDriverService().SearchDrivers(new ShipmentService().GetShipmentModelByID(20));
            

            //List<SearchDriversModel> x= new List<SearchDriversModel>();
            //x.Add(new DriverService().)
            //new SearchDriverService().SendRequestToMerchantAndDrivers(ids, new ShipmentService().GetShipmentModelByID(2))
            return View();
        }
	}
}