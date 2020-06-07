using DataModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel{
    public class BaseNotificationModel
    {
        public NotificationTypeEnum NotificationType { get; set; }
        
        public Dictionary<string, object> ConvertToDictionary( )
        {

              var  dict = new Dictionary<string, object>();
            
            
                var properties = this.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    var propertyName = prop.Name;
                    var propertyValue = prop.GetValue(this);
                    if (dict.ContainsKey(propertyName))
                    {
                        dict[propertyName] += " " + propertyValue;
                    }
                    else
                    {
                        dict.Add(propertyName, propertyValue);
                    }
                }
            

            return dict;
        }

    }
}
