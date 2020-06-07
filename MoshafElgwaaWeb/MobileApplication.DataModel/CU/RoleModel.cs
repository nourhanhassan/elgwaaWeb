using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel
{
    public class RoleModel
    {
        public int? ID { get; set; }

        [Required]
        [String150]
        [System.Web.Mvc.Remote("DuplicateName", "Role", AdditionalFields = "ID", ErrorMessageResourceName = "NameExists", ErrorMessageResourceType = typeof(ValidationMessages))]     
        public string Name { get; set; }

        [Required]
        [String350]
        public string Description { get; set; }
      
        public bool AllowDelete { get; set; }

        public int? UserId { get; set; }

        public string ActionName { get; set; }
    }

}
