using GenericEngine.Service.SharedService;
using MobileApplication.Context;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class CU_Employee_RoleService :BaseService
    {
        private readonly Repository<CU_Employee_Role> _CU_Employee_RoleServiceRepository;
        private readonly Repository<CU_Employee> _EmployeeRepository;
        private CU_RoleService roleServiceObj;
      //  private CU_LogService logServiceObj;

        public CU_Employee_RoleService()
        {
            _CU_Employee_RoleServiceRepository = new Repository<CU_Employee_Role>(_unitOfWork);
            _EmployeeRepository = new Repository<CU_Employee>(_unitOfWork);
            roleServiceObj = new CU_RoleService();
         //   logServiceObj = new CU_LogService();
        }

        public IEnumerable<CU_Employee_Role> CU_Employee_RoleServiceList
        {
            get { return _CU_Employee_RoleServiceRepository.GetList(); }
        }

        public IEnumerable<CU_Employee_Role> CU_Employee_Roles()
        {
            return CU_Employee_RoleServiceList;
        }

        /// <summary>
        /// handle many to many relationship between Employees and roles BLL and Log logic
        /// </summary>
        /// <param name="SelectedRolesIds"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="parentId"></param>
        /// <param name="logId"></param>
        /// <returns></returns>
        public bool Save(List<int> SelectedRolesIds, int EmployeeID, int? parentId, out int? logId, int userId, string ActionName)
        {
            bool Return = false;

            List<CU_Employee_Role> exsistingRows = CU_Employee_RoleServiceList.Where(x => x.IdEmployee == EmployeeID).ToList();

            List<int> toBeAddedIds = SelectedRolesIds.Where(x => !exsistingRows.Exists(y => y.IdRole == x)).ToList();

            List<CU_Employee_Role> toBeRemovedRows = exsistingRows.Where(x => !SelectedRolesIds.Exists(y => x.IdRole == y)).ToList();

            List<CU_Employee_Role> toBeAddedRows = new List<CU_Employee_Role>();
            CU_Employee EmployeeObj = _EmployeeRepository.GetList().Where(x => x.ID == EmployeeID).FirstOrDefault();

            foreach (var item in toBeAddedIds)
            {
                CU_Employee_Role obj = new CU_Employee_Role()
                {
                    IdRole = item,
                    IdEmployee = EmployeeID
                };
                toBeAddedRows.Add(obj);
            }

            if (toBeRemovedRows.Count > 0)
            {

                toBeRemovedRows.ForEach(d => _CU_Employee_RoleServiceRepository.Delete(d));

            }
            if (toBeAddedRows.Count > 0)
            {

                toBeAddedRows.ForEach(a => _CU_Employee_RoleServiceRepository.Save(a));
            }

            List<string> lstWillBeDeleted = toBeRemovedRows.Select(x => EmployeeObj.Name + DataServiceArabicResource.FromGroup + roleServiceObj.RolesList.Where(a => a.ID == x.IdRole).FirstOrDefault().Name).ToList<string>();

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
