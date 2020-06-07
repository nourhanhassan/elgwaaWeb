using AutoMapper;
using GenericEngine.Service.SharedService;
using MobileApplication.Context;
using MobileApplication.DataModel;
using MobileApplication.DataService;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace MobileApplication.DataService
{
    public class CU_Employee_RoleProgramService : BaseService
    {
        private readonly Repository<CU_Employee_RoleProgram> _CU_Employee_RoleProgramRepository;
        private readonly Repository<CU_Employee> _EmployeeRepository;
        private readonly CU_Role_ProgramService _RoleProgramServece;
        private CU_RoleService roleServiceObj;
      //  private CU_LogService logServiceObj;

        public CU_Employee_RoleProgramService()
        {
            _CU_Employee_RoleProgramRepository = new Repository<CU_Employee_RoleProgram>(_unitOfWork);
            _EmployeeRepository = new Repository<CU_Employee>(_unitOfWork);
            roleServiceObj = new CU_RoleService();
            _RoleProgramServece = new CU_Role_ProgramService();

        }


        public IEnumerable<CU_Employee_RoleProgram> CU_Employee_RoleProgramList
        {
            get { return _CU_Employee_RoleProgramRepository.GetList(); }
        }
        public IEnumerable<CU_Employee_RoleProgram> GetEmployeeRoleProgram(int employeeId,int programId)
        {
            return CU_Employee_RoleProgramList.Where(i => i.IdEmployee == employeeId && (programId==0||i.CU_Role_Program.IdProgram==programId));
        }
        public IEnumerable<EmployeeRoleProgramModel> EmployeeRoleProgram()
        {
            var obj = _RoleProgramServece.GetRoleProgramByRoleAndProgramId();
            var data = CU_Employee_RoleProgramList.Where(x => x.IdRoleProgram == obj.ID );
            var result = Mapper.Map<IEnumerable<CU_Employee_RoleProgram>, IEnumerable<EmployeeRoleProgramModel>>(data);
            return result;

        }
        public List<int> GetEmployeeRoleProgramID(int employeeId, int programId)
        {
            var role = CU_Employee_RoleProgramList.Where(i => i.IdEmployee == employeeId && (programId == 0 || i.CU_Role_Program.IdProgram == programId));
            return role.Select(i=>i.CU_Role_Program.IdRole).ToList();
        }

        public bool Save(List<int> SelectedRolesIds, int EmployeeID, int? parentId, out int? logId, int userId, string ActionName)
        {
            bool Return = false;

            List<CU_Employee_RoleProgram> exsistingRows = CU_Employee_RoleProgramList.Where(x => x.IdEmployee == EmployeeID).ToList();

            List<int> toBeAddedIds = SelectedRolesIds.Where(x => !exsistingRows.Exists(y => y.IdRoleProgram == x)).ToList();

            List<CU_Employee_RoleProgram> toBeRemovedRows = exsistingRows.Where(x => !SelectedRolesIds.Exists(y => x.IdRoleProgram == y)).ToList();

            List<CU_Employee_RoleProgram> toBeAddedRows = new List<CU_Employee_RoleProgram>();
            CU_Employee EmployeeObj = _EmployeeRepository.GetList().Where(x => x.ID == EmployeeID).FirstOrDefault();

            foreach (var item in toBeAddedIds)
            {
                CU_Employee_RoleProgram obj = new CU_Employee_RoleProgram()
                {
                    IdRoleProgram = item,
                    IdEmployee = EmployeeID
                };
                toBeAddedRows.Add(obj);
            }

            if (toBeRemovedRows.Count > 0)
            {

                toBeRemovedRows.ForEach(d => _CU_Employee_RoleProgramRepository.Delete(d.ID));

            }
            if (toBeAddedRows.Count > 0) 
            {

                toBeAddedRows.ForEach(a => _CU_Employee_RoleProgramRepository.Save(a));
            }

            List<string> lstWillBeDeleted = toBeRemovedRows.Select(x => EmployeeObj.Name + DataServiceArabicResource.FromGroup + roleServiceObj.RolesList.Where(a => a.ID == x.CU_Role_Program.IdRole).FirstOrDefault().Name).ToList<string>();

            //x.CU_Role.Name
            _unitOfWork.Submit();

            ActionName = ActionName + "?Id=" + EmployeeID;
            //logId = logServiceObj.Update(parentId, EmployeeID, "صلاحيات المستخدم " + EmployeeObj.Name,null, null,toBeAddedRows.Select(x => EmployeeObj.Name + " إلى مجموعة " + roleServiceObj.RolesList.Where(a => a.ID == x.IdRole).FirstOrDefault().Name).ToList<string>(),"EmployeeRoles", userId, ActionName);
            //logId = logServiceObj.Update(logId, EmployeeID, "صلاحيات المستخدم " + EmployeeObj.Name, lstWillBeDeleted, null,null, "EmployeeRoles", userId, ActionName);
            logId = 0;
            Return = true;
            return Return;
        }

       

    }
}
