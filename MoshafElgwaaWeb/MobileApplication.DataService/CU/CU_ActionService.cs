using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Repository;

using GenericEngine.Service.SharedService;
using MobileApplication.DataService;
using MobileApplication.Context;

namespace MobileApplication.DataService
{
    public class CU_ActionService :BaseService
    {
        private readonly Repository<CU_Action> _CU_ActionRepository;
      //  private readonly CU_LogService logService;

        public CU_ActionService()
        {
            _CU_ActionRepository = new Repository<CU_Action>(_unitOfWork);
            //  logService = new CU_LogService();
        }

        public IEnumerable<CU_Action> CU_ActionList
        {
            get { return _CU_ActionRepository.GetList().Where(i => i.IsSecurity); }
        }

        public IEnumerable<CU_Action> GetAllActions
        {
            get { return _CU_ActionRepository.GetList(); }
        }

        public Dictionary<QVEnterprise.ActionType, bool> GetPermission(string currentController, int userID)
        {
           // CU_Employee_RoleService ser = new CU_Employee_RoleService();
             CU_Employee_RoleProgramService ser = new CU_Employee_RoleProgramService();

            var Permission = new Dictionary<QVEnterprise.ActionType, bool>();
            CU_PageService page = new CU_PageService();
            List<CU_Role_Page> rolePageLst = new List<CU_Role_Page>();
            CU_Role_PageService rolepage = new CU_Role_PageService();
            int progID = Extention.GeKeyValue<int>("ProgramID");

            var employeeRolesProgram = ser.GetEmployeeRoleProgram(userID, progID);
            foreach (var role in employeeRolesProgram)
            {
                var rolesPage = rolepage.CU_RolepageList.Where(r => r.IdRoleProgram == role.IdRoleProgram && r.IdPage == page.GetCU_PageByPageURL(currentController).ID).FirstOrDefault();
                if (rolesPage != null)
                {
                    rolePageLst.Add(rolesPage);
                }
            }
            //for each action get max permission allawed in logined user roles
            new CU_ActionService().CU_ActionList.ToList().ForEach(delegate(CU_Action i)
            {
                bool hasPermission = rolePageLst.Where(p => p.Permission.Length > i.Order && p.Permission.Substring(i.Order, 1) == "1").Count() > 0;
                Permission.Add((QVEnterprise.ActionType)System.Enum.Parse(typeof(QVEnterprise.ActionType), i.EnName), hasPermission);
            });
            return Permission;
        }

        public Dictionary<string, Dictionary<QVEnterprise.ActionType, bool>> GetPermission(IEnumerable<string> Controllers, int userID)
        {
            // CU_Employee_RoleService ser = new CU_Employee_RoleService();
            CU_Employee_RoleProgramService ser = new CU_Employee_RoleProgramService();

            Dictionary<int, string> pages = new CU_PageService().GetPageIdsByURLs(Controllers);

            //Get all permissions for all pages once and query them when needed instead of querying the database everytime
            Dictionary<string, Dictionary<QVEnterprise.ActionType, bool>> pagesPermissions = new Dictionary<string, Dictionary<QVEnterprise.ActionType, bool>>();


            CU_PageService page = new CU_PageService();

            CU_Role_PageService rolepageservice = new CU_Role_PageService();
            var rolepages = rolepageservice.CU_RolepageList;
            int progID = Extention.GeKeyValue<int>("ProgramID");

            var actionList = new CU_ActionService().CU_ActionList.ToList();

            var employeeRolesProgram = ser.GetEmployeeRoleProgram(userID, progID);

            foreach (int pageId in pages.Keys)
            {
                List<CU_Role_Page> rolePageLst = new List<CU_Role_Page>();
                var Permission = new Dictionary<QVEnterprise.ActionType, bool>();
                foreach (var role in employeeRolesProgram)
                {
                    var rolesPage = rolepages.Where(r => r.IdRoleProgram == role.IdRoleProgram && pageId == r.IdPage).FirstOrDefault();
                    if (rolesPage != null)
                    {
                        rolePageLst.Add(rolesPage);
                    }
                }
                //for each action get max permission allawed in logined user roles
                actionList.ForEach(delegate(CU_Action i)
                {
                    bool hasPermission = rolePageLst.Where(p => p.Permission.Length > i.Order && p.Permission.Substring(i.Order, 1) == "1").Count() > 0;
                    Permission.Add((QVEnterprise.ActionType)System.Enum.Parse(typeof(QVEnterprise.ActionType), i.EnName), hasPermission);
                });

                //we should not check for this.. but because there are 2 records in the page table having same url :(
                if (!pagesPermissions.ContainsKey(pages[pageId]))
                {
                    pagesPermissions.Add(pages[pageId], Permission);
                }

            }

            return pagesPermissions;
        }


        public CU_Action Action(string ActionName)
        {
            return CU_ActionList.Where(x => x.EnName == ActionName).FirstOrDefault();
        }

    }
}







