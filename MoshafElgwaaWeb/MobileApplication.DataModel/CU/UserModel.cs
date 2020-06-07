using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApplication.DataModel.QvDataAnnotation;
using GenericEngine.ServiceContract.Infrastructure;


namespace MobileApplication.DataModel
{
    public class UserModel:GenericModel
    {
        public int? ID { get; set; }

        [Required]
        [String150]
        public string Name { get; set; }
        public int IdJob { get; set; }

        [Mobile]
        public string Mobile { get; set; }
        public bool Gender { get; set; }

        [Email530]
        [Required]
        [System.Web.Mvc.Remote("CheckDuplicateEmail", "User", AdditionalFields = "ID", ErrorMessage = "البريد الإلكتروني مستخدم من قبل")]
        public string Email { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }

        [Required]
        [LoginName]
        [System.Web.Mvc.Remote("DuplicateUserName", "User", AdditionalFields = "ID", ErrorMessageResourceName = "NameExists", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string LoginName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsActive { get; set; }
        public int IdDepartment { get; set; }
        public int IdHeadDepartment { get; set; }
        public string Phone { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? LastVisitTime { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> IdEmployeeCategory { get; set; }
        public Nullable<int> BranchID { get; set; }

    }

    public class UserRoleModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsUserExist { get; set; }
    }




}


