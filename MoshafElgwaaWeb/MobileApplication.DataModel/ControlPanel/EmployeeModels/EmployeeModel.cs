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
    [LogName("المستخدمين")]

    public class EmployeeModel : GenericModel
    {
        public EmployeeModel()
        {
            SetPublicSettings("المستخدمين","المستخدمين");
            CanEdit = true;

        }
        public int ID { get; set; }
        [GridViewShown(true)]
        [SearchNameShownAttribute()]
        [Contains]
        [DisplayField("اسم المستخدم")]
        [String150]
        [QvDataAnnotation.Required]

        public string Name { get; set; }

    [QvDataAnnotation.Required]
        [Mobile]
        [DisplayField("رقم الجوال")]
        public string Mobile { get; set; }
       [Email530]
       [QvDataAnnotation.Required]
        [SearchNameShownAttribute()]
        [Equals]
        [System.Web.Mvc.Remote("CheckDuplicate", "Employee", AdditionalFields = "ID", ErrorMessage = "البريد الإلكتروني مستخدم من قبل")]
        public string Email { get; set; }

        public Nullable<System.DateTime> BirthDate { get; set; }
        [QvDataAnnotation.Required]
        [SearchNameShownAttribute()]
        [Contains]
        [System.Web.Mvc.Remote("CheckDuplicate", "Employee", AdditionalFields = "ID", ErrorMessage = "الإسم مستخدم من قبل")]


        [String50]
        public string LoginName { get; set; }
        
        public string Password { get; set; }
        public string Salt { get; set; }
        [Equals]
        public bool IsActive { get; set; }
        public int? IdDepartment { get; set; }
        public int? IdHeadDepartment { get; set; }
       
          
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
        public Nullable<System.DateTime> LastVisitTime { get; set; }
        public bool IsTrainee { get; set; }
        public string strUserRoles { get; set; }
        public List<int> UserRoles { get; set; }
        public string ActionName { get; set; }
        public int? UserId { get; set; }
       
    
    }
}
