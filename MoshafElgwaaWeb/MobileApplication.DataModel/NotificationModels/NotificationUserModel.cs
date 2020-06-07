using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel
{
    public class NotificationUserModel
    {
        public int ID { get; set; }
        public int NotificationID { get; set; }
        public int AppUserID { get; set; }
        public bool IsSeen { get; set; }
    }
}
