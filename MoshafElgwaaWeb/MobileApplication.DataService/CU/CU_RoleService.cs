using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Service;
using AutoMapper;
using GenericEngine.Service.SharedService;
using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.Context;
using MobileApplication.DataModel;
//using CU_Role_Program = QVEnterprise.CU_Role_Program;

namespace MobileApplication.DataService
{
    public class CU_RoleService : BaseService
    {
        private readonly Repository<CU_Role> _RoleRepository;
        private readonly Repository<CU_Role_Program> _RoleProgramRepository;
        private readonly Repository<CU_Role_Page> _RolePageRepository;
        protected ILogService _logService;
        public string LogRepresintitiveColumName;
        //    private readonly CU_LogService _logService;


        public CU_RoleService()
        {
            _RoleRepository = new Repository<CU_Role>(_unitOfWork);
            _RoleProgramRepository = new Repository<CU_Role_Program>(_unitOfWork);
            _RolePageRepository = new Repository<CU_Role_Page>(_unitOfWork);
            _logService = base._LogService;
            LogRepresintitiveColumName = "Name";
            // _logService = new CU_LogService();
        }

        public IEnumerable<CU_Role> RolesList
        {
            get { return _RoleRepository.GetList(); }
        }

        public int RolesListCount
        {
            get { return _RoleRepository.GetList().Count(); }
        }

        public IEnumerable<RoleModel> GetRoles(Dictionary<string, string> sorting, Dictionary<string, string> filter)
        {
            string Name = filter.ContainsKey("Name") ? filter["Name"] : string.Empty;
            IEnumerable<CU_Role> Roles = RolesList.Where(x => x.Name.Trim().ToLower().Contains(string.IsNullOrEmpty(Name.Trim().ToLower()) ? "" : Name.Trim().ToLower())).OrderByDescending(x => x.ID);

            if (sorting.ContainsKey("ID"))
            {
                if (sorting["ID"] == "asc")
                {
                    Roles = Roles.OrderBy(c => c.ID);
                }
                else { Roles = Roles.OrderByDescending(c => c.ID); }
            }

            if (sorting.ContainsKey("Name"))
            {
                if (sorting["Name"] == "asc")
                {
                    Roles = Roles.OrderBy(c => c.Name);
                }
                else { Roles = Roles.OrderByDescending(c => c.Name); }
            }

            var data = Mapper.Map<IEnumerable<CU_Role>, IEnumerable<RoleModel>>(Roles);
            return data;
        }

        public bool CheckRoleName(string Name, int? Id)
        {
            bool Return = false;

            if (!Id.HasValue)
                Id = 0;
            Return = !RolesList.Any(i => (i.Name.Trim().ToLower() == Name.Trim().ToLower()) && (Id == 0 || i.ID != Id));
            return Return;
        }

        public CU_Role GetRoleContextById(int Id)
        {
            return _RoleRepository.GetById(Id);
        }

        public int Save(RoleModelLookup obj, int userId ,string url)
        {
            CU_Role role = Mapper.Map<RoleModelLookup, CU_Role>(obj);
            _RoleRepository.Save(role);
            _unitOfWork.Submit();
            string value = System.Configuration.ConfigurationManager.AppSettings["ProgramID"];
            CU_Role_Program roleProgramObj = new CU_Role_Program
            {
                IdRole = role.ID,
                IdProgram = int.Parse(value)
            };
            _RoleProgramRepository.Save(roleProgramObj);
            _unitOfWork.Submit();
            _logService.Insert(role.ID, role.Name + DataServiceArabicResource.UserAdded, obj, userId, url);
            return role.ID;
        }

        public bool Delete(int id, int userId, string actionName)
        {
            try
            {
                var role = _RoleRepository.GetById(id);
                var rolePrograms = role.CU_Role_Program.ToList();
                foreach (var roleProgram in rolePrograms)
                {
                    var rolePages = roleProgram.CU_Role_Page.ToList();
                    foreach (var rolePage in rolePages)
                    {
                        _RolePageRepository.Delete(rolePage.ID);
                    }
                    var test = _RoleProgramRepository.GetById(roleProgram.ID);
                    _RoleProgramRepository.Delete(test.ID);
                }
                _RoleRepository.Delete(id);
                var deleteResult = _unitOfWork.Submit();
                _logService.Delete(role.Name + DataServiceArabicResource.FromGroups, actionName, userId);
                var flag = deleteResult != 0;
                return flag;

            }
            catch (Exception )
            {
                return false;
            }

        }
    }
}