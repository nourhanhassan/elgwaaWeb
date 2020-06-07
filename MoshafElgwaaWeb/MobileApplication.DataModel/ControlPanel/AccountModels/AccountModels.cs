using MobileApplication.DataModel.QvDataAnnotation;

namespace MobileApplication.DataModel
{
    public class LoginFormModel
    {     
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string returnUrl { get; set; }

        public string PageUrlNum { get; set; }
    }

    public class ForgetPasswordModel
    {

      [Required]
      [Email530]

        public string Email { get; set; }
    }

    public class ChangePassWordModel
    {
        [Required]
        [System.Web.Mvc.Remote("Checkcorrectpassword", "Account", ErrorMessageResourceName = "WrongOldPassword", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string OldPassword { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"((?=.*\d)(?=.*[A-z\u0600-\u06ff]).{8,12})", ErrorMessageResourceName = "ValidPassword", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string NewPassword { get; set; }

        [Required]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceName = "ValidComparerPassword", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string ConfirmNewPassword { get; set; }
    }

    public class ResetPasswordModel
    {
        public string GUID { get; set; }

        public int EmployeeID { get; set; }

        [System.ComponentModel.DataAnnotations.RegularExpression(@"((?=.*\d)(?=.*[A-z\u0600-\u06ff]).{8,12})", ErrorMessageResourceName = "ValidPassword", ErrorMessageResourceType = typeof(ValidationMessages))]
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceName = "ValidComparerPassword", ErrorMessageResourceType = typeof(ValidationMessages))]
        public string ConfirmPassword { get; set; }

        public bool IsExpired { get; set; }
    }
}
