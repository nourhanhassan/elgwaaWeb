using System;
using System.Linq;
using AutoMapper;
using System.Web;
using QvLib.Security;
using System.Data.EntityClient;
using QvLib;
using Repository;
using GenericEngine.Service.SharedService;
using QV.Service.CU;
using MobileApplication.DataService;
using MobileApplication.DataModel;
using MobileApplication.Context;
using DataModel.hashSaltProtection;


namespace MobileApplication.DataService
{
    public class CU_AccountService : ManageBaseService<UserModel>
    {
        private readonly Repository<ResetPasswordRequest> _ResetPasswordRequestRepositry;
        private readonly CU_LogService _logService;
        private readonly CU_EmployeeService _employeeService;
        private readonly Repository<CU_Employee> _Employee;


        public CU_AccountService()
        {
            _ResetPasswordRequestRepositry = new Repository<ResetPasswordRequest>(_unitOfWork);
            _logService = new CU_LogService();
            _employeeService = new CU_EmployeeService();
            _Employee = new Repository<CU_Employee>(_unitOfWork);

        }

        public UserModel ValidateLoginData(string loginName, string passWord)
        {

          //  passWord = QvLib.Security.DataProtection.Encrypt(passWord);
            var xx = _employeeService.EmployeeList;
            var emp = _employeeService.EmployeeList.Where(x => x.LoginName.Trim().ToLower() == loginName.Trim().ToLower() && x.IsActive == true).FirstOrDefault();

            if (emp != null)
            {
                bool isPasswordMatch = false;

                isPasswordMatch = hashSaltProtection.IsPasswordMatch(passWord, emp.Salt, emp.Password);

                if (isPasswordMatch)
                {


                    CU_Role_ProgramService progServ = new CU_Role_ProgramService();

                    int[] roles = progServ.RolesList.Where(i => i.CU_Employee_RoleProgram.Where(s => s.IdEmployee == emp.ID).Count() > 0).Select(i => i.ID).ToArray();
                    if (roles.Count() == 0)
                    {
                        Identity.UserID = null;

                    }
                    Identity.UserFullName = emp.Name;
                    Identity.UserID = emp.ID;
                    Identity.Roles = roles;
                    Identity.DepartmentID = emp.IdDepartment;
                    Identity.LastVisit = emp.LastVisitTime.HasValue ? QvLib.QVUtil.Date.GregToHijri(emp.LastVisitTime.Value) : DataServiceArabicResource.NoSignInBefore;
                    Identity.HeadDepartmentID = emp.IdHeadDepartment;
                    Identity.BranchID = emp.BranchID == null ? 0 : emp.BranchID;
                    //update last visit
                }
                else emp = null;
            }


            var data = Mapper.Map<CU_Employee, UserModel>(emp);
            return data;
            //return emp;
        }

        public UserModel ValidateMail(string mail)
        {
            return _employeeService.CheckValidateMail(mail);
           
        }

        public bool ValidatePassword(int id, string passWord)
        {

            var emp = _Employee.Get(x => x.ID == id).FirstOrDefault();

            if (emp != null)
            {
                bool isPasswordMatch = false;

                return isPasswordMatch = hashSaltProtection.IsPasswordMatch(passWord, emp.Salt, emp.Password);
            }
            else return false;
        }    

        public void SetLastVisitTime(int id)
        {
            var emp = _Employee.GetById(id);
            emp.LastVisitTime = DateTime.Now.Date;
            _Employee.Save(emp);
            _unitOfWork.Submit();
        }

        public string ResetPasswordRequest(int UserId)
        {
            ResetPasswordRequest Reset = new ResetPasswordRequest();
            Reset.EmployeeID = UserId;
            Reset.GUID = Guid.NewGuid().ToString();
            Reset.ResetRequestDateTime = DateTime.Now;
            Reset.IsExpired = false;
            _ResetPasswordRequestRepositry.Save(Reset);
            _unitOfWork.Submit();
            return Reset.GUID;
        }

        public ResetPasswordModel GetResetPasswordLink(string Guid)
        {
            var resetPassword = _ResetPasswordRequestRepositry.Get(r => r.GUID == Guid).Single();
            var data = Mapper.Map<ResetPasswordRequest, ResetPasswordModel>(resetPassword);
            return data;
        }

        private void SetResetPasswordExpired(string Guid)
        {
            var resetPassword =_ResetPasswordRequestRepositry.Get(r => r.GUID == Guid).Single();
            resetPassword.IsExpired = true;
            _ResetPasswordRequestRepositry.Save(resetPassword);
            _unitOfWork.Submit();
        }

        public bool SaveNewPassword(int userid, string newPassword, string userguid)
        {
            CU_Employee emp = _Employee.Get(x => x.ID == userid).First();
          //  emp.Password = DataProtection.Encrypt(newPassword);
            string strsalt = string.Empty;
            emp.Password = hashSaltProtection.GeneratePasswordHash(newPassword, out strsalt);
            emp.Salt = strsalt;
            _Employee.Save(emp); 
            _unitOfWork.Submit();
            _logService.ChangePassword(emp.ID);
            SetResetPasswordExpired(userguid);
            return true;
        }
        public void ChangePassword(int id, string NewpassWord)
        {

            CU_Employee emp = _Employee.Get(x => x.ID == id).First();
          //  emp.Password = DataProtection.Encrypt(NewpassWord);
            string strsalt = string.Empty;
            emp.Password = hashSaltProtection.GeneratePasswordHash(NewpassWord, out strsalt);
            emp.Salt = strsalt;
            _Employee.Update(emp);
            _unitOfWork.Submit();

           CU_Employee beforUpdate = emp.Clone();
            if (emp != null)
            {
                _logService.ChangePassword(emp.ID);

            }



        }



    }
}