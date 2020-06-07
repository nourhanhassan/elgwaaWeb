
using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel
{
    public class SettingModel
    {
        public int SettingID { get; set; }
        
        [Required]
        public string ServerName { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        //[Number]
        //[RangeNum(int.MinValue, int.MaxValue, ErrorMessageResourceName = "ValiedPortNo", ErrorMessageResourceType = typeof(ValidationMessages))]
            [PortNo]
        public int PortNo { get; set; }
        
        [Required]
        [Email530]
        public string FromEmail { get; set; }

        [Required]
        [Email530]
        public string ContactUsResponsableEmail { get; set; }

        public bool SSL { get; set; }

        public int? OnlineVisiter { get; set; }

        public int? VisitorCounter { get; set; }

        public int? UserId { get; set; }

        public string ActionName { get; set; }

    }
}
