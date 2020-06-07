using MobileApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public interface INotificationHub
    {
         void SendNotificationToUser(NotificationModel notification, int userID);
         void SendNotificationToUser(NotificationModel notification, int[] userIDs);
     
    }
}
