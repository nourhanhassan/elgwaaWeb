using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.DataModel
{
    public class PageModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Actions { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public bool IsVisible { get; set; }


        [System.Web.Mvc.Remote("DuplicateName", "Pages", AdditionalFields = "ID", ErrorMessageResourceName = "OrderExists", ErrorMessageResourceType = typeof(ValidationMessages))]
        public int PageOrder { get; set; }
        public int PageOrder1 { get; set; }

        public string Icon { get; set; }
        public string Class { get; set; }
        public int? ParentID { get; set; }
        public int IdProgram { get; set; }
        public List<Role_PageModel> CU_Role_Page { get; set; }
        public List<PageModel> CU_Page1 { get; set; }

        public bool HasPermission { get; set; }


    }
}