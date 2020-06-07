
using AutoMapper;
using DataModel.Enum;
using DataModel.hashSaltProtection;
using MobileApplication.Context;
using MobileApplication.DataModel;
using MobileApplication.DataService;
using QvLib.Security;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class CU_EmployeeService :BaseService
   {

       private readonly Repository<CU_Employee> _CU_EmployeeRepository;
       private readonly Repository<CU_Role_Page> _CU_RolePageRepository;
       
        private CU_LogService _logService;
        private CU_Role_ProgramService _CU_Role_ProgramService;
        public CU_EmployeeService()
        {
            _CU_EmployeeRepository = new Repository<CU_Employee>(_unitOfWork);
            
            _logService = new CU_LogService();
            _CU_Role_ProgramService = new CU_Role_ProgramService();
            //int num = 0;
            //var x = new NeighborhodModel().filterExpProgram<Poor_Member>(null).Compile();

            //var m = _supportProgramRepository.GetGridList(out num, 1, 1, x);
        }

        public IEnumerable<CU_Employee> EmployeeList
        {
            get { return _CU_EmployeeRepository.GetList(); }
        }
        public IEnumerable<CU_Role_Page> RolePageList
        {
            get { return _CU_RolePageRepository.GetList(); }
        }
        public UserModel CheckValidateMail(string Mail)
        {
            var emp = _CU_EmployeeRepository.GetByFilter(x => (x.Email.Trim().ToLower() == Mail.Trim().ToLower())).FirstOrDefault();
            if (emp != null)
            {
                var data = Mapper.Map<CU_Employee, UserModel>(emp);
                return data;
            }
            else
            {
                return null;
            }
        }
        public bool ValidatePassword(int id, string passWord)
        {
            var emp = _CU_EmployeeRepository.GetByFilter(x => x.ID == id).FirstOrDefault();
            if (emp != null)
            {
                bool isPasswordMatch = false;

                return isPasswordMatch = hashSaltProtection.IsPasswordMatch(passWord, emp.Salt, emp.Password);
            }
            else return false;
        }

        public CU_Employee GetByID(int id)
        {
            var employee = _CU_EmployeeRepository.GetById(id);
            return employee;
        }

        public EmployeeModel GetModelByID(int id)
        {
            var user = _CU_EmployeeRepository.GetById(id);
            var data = Mapper.Map<CU_Employee, EmployeeModel>(user);
            return data;
        }

        public List<int> GetUserRole(int idUser)
        {
            var user = _CU_EmployeeRepository.GetList().Where(a => a.ID == idUser).First();
            return user.CU_Employee_RoleProgram.Where(e => e.CU_Role_Program.IdProgram == int.Parse(ConfigurationManager.AppSettings["ProgramID"])).Select(x => x.CU_Role_Program.IdRole).ToList();
        }

        public List<int> GetUserRoleProgram(int idUser)
        {
            var user = _CU_EmployeeRepository.GetList().Where(a => a.ID == idUser).First();
            return user.CU_Employee_RoleProgram.Where(e => e.CU_Role_Program.IdProgram == int.Parse(ConfigurationManager.AppSettings["ProgramID"])).Select(x => x.CU_Role_Program.ID).ToList();
        }
        public IEnumerable<UserRoleModel> GetUserRole(EmployeeModel userModel)
        {
            List<CU_Role_Program> rolePrograms = _CU_Role_ProgramService.RolesList.Where(r => r.IdProgram == int.Parse(ConfigurationManager.AppSettings["ProgramID"])).GroupBy(o => o.IdRole).Select(g => g.First()).ToList();
            var UserRoles = GetUserRole(userModel.ID);
            var data = (from R in rolePrograms
                        join UR in UserRoles on R.ID equals UR into temp
                        from subpet in temp.DefaultIfEmpty()
                        select new UserRoleModel
                        {
                            RoleID = R.ID,
                            RoleName = R.CU_Role.Name,
                            IsUserExist = UserRoles == null ? false : (UserRoles.Contains(R.IdRole))
                        });
            return data;
        }
        public int Save(EmployeeModel obj)
        {

            CU_Employee oldObj = new CU_Employee();
            CU_Employee oldObj2 = new CU_Employee();
            if (obj.ID > 0)
            {
                oldObj2 = GetByID((int)obj.ID);
                oldObj = oldObj2.Clone();
            }

            // decrypt password to prevent double ecryption
            //    string decryptedPassword = DataProtection.Decrypt(obj.Password);
            //   if (!decryptedPassword.Contains("The input is not")) 

            //    obj.Password = string.IsNullOrEmpty(obj.Password) ? null : contract_Extention.testDecrypt(obj.Password);
            if (string.IsNullOrEmpty(obj.Password) != null)
            {
                string strsalt = string.Empty;
                obj.Password = hashSaltProtection.GeneratePasswordHash(obj.Password, out strsalt);
                obj.Salt = strsalt;
            }
            var data = Mapper.Map<EmployeeModel, CU_Employee>(obj);

            int? parentLogId = 0;
            if (obj.ID > 0)
            {
                oldObj2.Name = data.Name;
                oldObj2.Mobile = data.Mobile;
                oldObj2.IsActive = data.IsActive;

                _CU_EmployeeRepository.Save(oldObj2);
                _unitOfWork.Submit();
                parentLogId = _logService.Update(data.ID, DataServiceArabicResource.User, oldObj, oldObj2, obj.UserId.Value, obj.ActionName);
            }
            else
            {

                _CU_EmployeeRepository.Save(data);
                _unitOfWork.Submit();
                obj.ID = data.ID;
             
                parentLogId = _logService.Insert(data.ID, DataServiceArabicResource.User, obj, obj.UserId.Value, obj.ActionName.ToLower().Replace("add", "edit").Replace("0", data.ID.ToString()));
            }

            int? logId;
            if (obj.UserRoles != null)
            {
                new CU_Employee_RoleProgramService().Save(obj.UserRoles, data.ID, parentLogId, out logId, obj.UserId.Value, "AddEdit");
            }
            return (int)data.ID;
        }
        public int[] getUsersByPermissionOnPageAndRole(string strController, ActionType actionType, int RoleID, int SecondRoleID=0)
        {
            int[] employeeIDs = null;
           
            int programID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ProgramID"]);
            CU_Page pageObj = new CU_PageService().GetCU_PageByPageURL(strController);
            Expression<Func<CU_Employee, bool>> filterExpression = x => (x.CU_Employee_RoleProgram.Any(y => (y.CU_Role_Program.CU_Program.ID == programID)
               && y.CU_Role_Program.CU_Role_Page.Any()
                && y.CU_Role_Program.CU_Role_Page.Any(z => z.IdPage == pageObj.ID &&
                    ((int)actionType - 1 < z.Permission.Length && z.Permission.Substring((int)actionType - 1, 1) == "1"))));

            var employeeList = _CU_EmployeeRepository.GetByFilter(filterExpression).Where(y => y.CU_Employee_RoleProgram.Any(x => x.CU_Role_Program.IdRole == RoleID || x.CU_Role_Program.IdRole==SecondRoleID));

            if (employeeList != null) employeeIDs = employeeList.Select(i => i.ID).ToArray();
            return employeeIDs;

        }
   }
}



