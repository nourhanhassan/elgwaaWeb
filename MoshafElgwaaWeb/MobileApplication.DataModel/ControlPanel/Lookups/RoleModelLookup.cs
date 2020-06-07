using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GenericEngine.ServiceContract.Infrastructure;
using System.Linq.Expressions;
using GenericEngine.Service.Enum;
using MobileApplication.DataModel.QvDataAnnotation;

namespace MobileApplication.DataModel
{
    [LogName("المجموعات")]
    public class RoleModelLookup : GenericModel
    {
        public RoleModelLookup()

            
        {
          
            List<GridLink> links = new List<GridLink>() { 
            new GridLink("الصلاحيات","fa fa-edit","","","/ControlPanel/Permission/Default?Id={{item.RoleProgramId}}"),
            new EditGridLink(),
            new DeleteGridLink(),
            };

            SetPublicSettings("المجموعات", "المجموعات", links);
            AddBtn = new GridLink("مجموعه", "fa fa-edit", "AddEdit()", "", "");
        }
        [Equals]
        public int ID { get; set; }
        [GridViewShown(true)]
        [SearchNameShownAttribute("Textbox")]
        [Contains]
        [DisplayField("اسم المجموعة")]
        [Required]
        [String150]
        [System.Web.Mvc.Remote("CheckDuplicate", "Role", AdditionalFields = "ID", ErrorMessage = "الاسم مستخدم من قبل")]
        [LogName("المجموعات")]
        public string Name { get; set; }


        public int RoleProgramId { get; set; }
        [String350]

        
        [GridViewShown(true)]
        [DisplayField("الوصف")]
        public string Description { get; set; }

        public bool CanDelete { get; set; }

        public int? UserId { get; set; }

        public string ActionName { get; set; }
        //[Const("False")]
        //public bool IsDeleted { get; set; }

        public bool CanEdit { get; set; }



           }

}
