using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.DataModel.QvDataAnnotation;

namespace MobileApplication.DataModel
{
    [LogName("إعدادات")]
    public class AppSettingModel 
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string KeyArabicName { get; set; }

        [QvDataAnnotation.Required]
        public string Value { get; set; }
        public bool IsMobile { get; set; }
    }
}
