using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using QVEnterprise;
using QvLib.Security;
using MobileApplication.DataModel;
using MobileApplication.DataService;



namespace MobileApplication.UI.InfraStructure
{
    public class UserProfile
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string LastVisitTime { get; set; }
        public int IdDepartment { get; set; } 
        public int IdHeadDepartment { get; set; }
        public int? BranchID { get; set; }

        public Dictionary<QVEnterprise.ActionType, bool> Permission;

        public static void CreateUserSession(UserProfile profile, UserModel employee)
        {
            profile.Id = employee.ID;
            profile.Name = employee.Name;
            profile.LoginName = employee.LoginName;
            profile.Password = employee.Password;
            profile.Email = employee.Email;
            profile.IdDepartment = employee.IdDepartment;
            profile.IdHeadDepartment = employee.IdHeadDepartment;
          
            if (employee.LastVisitTime.HasValue)
            {
                profile.LastVisitTime = QvLib.QVUtil.Date.GregToHijri(employee.LastVisitTime.Value);
            }
            profile.BranchID = employee.BranchID;
        }

        public static void CreateUserCookie(UserProfile profile, UserModel employee,string password=null)
        {
            var cookie = new HttpCookie("UserProfileCookie");
            cookie["Id"] = DataProtection.Encrypt(profile.Id.ToString());
            cookie["Name"] = DataProtection.Encrypt(profile.Name);
            cookie["LoginName"] = DataProtection.Encrypt(profile.LoginName);
            cookie["Password"] = DataProtection.Encrypt(password);
            cookie["Email"] = DataProtection.Encrypt(profile.Email);
            cookie["LastVisitTime"] = DataProtection.Encrypt(profile.LastVisitTime);
            cookie["BranchID"] = DataProtection.Encrypt(profile.BranchID.ToString());
            cookie.Expires = DateTime.Now.AddDays(7);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static bool CheckUserCookie(UserProfile profile)
        {
            if (HttpContext.Current.Request.Cookies["UserProfileCookie"] != null)
            {
                string strId = DataProtection.Decrypt(HttpContext.Current.Request.Cookies["UserProfileCookie"]["Id"]);
                if (string.IsNullOrWhiteSpace(strId)) return false;
                UserModel employee = new CU_AccountService().ValidateLoginData(DataProtection.Decrypt(HttpContext.Current.Request.Cookies["UserProfileCookie"]["LoginName"])
                    , DataProtection.Decrypt(HttpContext.Current.Request.Cookies["UserProfileCookie"]["Password"]));
                   if (employee != null)
                    {
	                     CreateUserSession(profile,employee);
                          return true;
    	            }
                    else return false;
               


                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ClearCookie()
        {
            try
            {
                HttpContext.Current.Response.Cookies["UserProfileCookie"].Expires = DateTime.Now.AddDays(-1);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool ClearSession()
        {
            try
            {
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.Clear(); 
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}