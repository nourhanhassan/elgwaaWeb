using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel
{
    public class PushNotificationLogModel
    {
        public int ID { get; set; }
        public string NotificationPayload { get; set; }
        public Nullable<bool> IsSuccess { get; set; }

        public string Error { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }

    }
}
