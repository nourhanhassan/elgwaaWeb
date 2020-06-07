using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.NotificationModels
{
    public class NotificationActionModel
    { 
        public string icon { get; set; }
        public string title { get; set; }
        public string callback { get; set; }
        public bool foreground { get; set; }
    }
}
