using AutoMapper;
using GenericEngine.Service.SharedService;
using MobileApplication.Context;
using MobileApplication.DataModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MobileApplication.DataService
{
    public class CU_Role_ProgramService : BaseService
    {
       private readonly Repository<CU_Role_Program> _Role_ProgramRepository;
        //private readonly CU_LogService _logService;


         public CU_Role_ProgramService()
        {
            _Role_ProgramRepository = new Repository<CU_Role_Program>(_unitOfWork);
            //_logService = new CU_LogService();

        }

         public IEnumerable<CU_Role_Program> RolesList
        {
            get { return _Role_ProgramRepository.GetList(); }
        }

        /// <summary>
       /// get roles expext role of institution and return list of context roles  
       /// </summary>
        //public IEnumerable<CU_Role> RolesListExpectInstitution
        //{
        //    get { return _Role_ProgramRepository.GetList().Where(I => I.IsInstitution == false); }
        //}
        /// <summary>
       /// get roles expext role of institution and return list of model roles  
       /// </summary>
       /// <returns></returns>
        //public IEnumerable<RoleModel> RolesListExpectInstitutionModel()
        //{
        //    var data = Mapper.Map<IEnumerable<CU_Role>, IEnumerable<RoleModel>>(RolesListExpectInstitution);
        //    return data;
          
        //}


        public int RolesListCount
        {
            get { return _Role_ProgramRepository.GetList().Count(); }
        }

        //public IEnumerable<RoleModel> GetRoles(Dictionary<string, string> sorting, Dictionary<string, string> filter, bool IncludedIsInstitution)
        //{
        //    string Name = filter.ContainsKey("Name") ? filter["Name"] : string.Empty;
        //    IEnumerable<CU_Role> Roles = RolesList.Where
        //        (x => x.Name.Trim().ToLower().Contains(string.IsNullOrEmpty(Name.Trim().ToLower()) ? "" : Name.Trim().ToLower())
        //        && (IncludedIsInstitution==true||x.IsInstitution==IncludedIsInstitution)
        //        ).OrderByDescending(x => x.ID);
            
        //    if (sorting.ContainsKey("ID"))
        //    {
        //        if (sorting["ID"] == "asc")
        //        {
        //            Roles = Roles.OrderBy(c => c.ID);
        //        }
        //        else { Roles = Roles.OrderByDescending(c => c.ID); }
        //    }

        //    if (sorting.ContainsKey("Name"))
        //    {
        //        if (sorting["Name"] == "asc")
        //        {
        //            Roles = Roles.OrderBy(c => c.Name);
        //        }
        //        else { Roles = Roles.OrderByDescending(c => c.Name); }
        //    }

        //    var data = Mapper.Map<IEnumerable<CU_Role>, IEnumerable<RoleModel>>(Roles);
        //    return data;
        //}



        public CU_Role_Program GetRoleContextById(int Id)
        {
            return _Role_ProgramRepository.GetById(Id);
        }

        public RoleProgramModel GetRoleProgramByRoleAndProgramId()
        {
           var ProgramID = System.Configuration.ConfigurationManager.AppSettings["ProgramID"];
           var RoleID = System.Configuration.ConfigurationManager.AppSettings["ResearcherID"];

           var data = RolesList.Where(x => x.IdProgram == int.Parse(ProgramID) && x.IdRole == int.Parse(RoleID)).FirstOrDefault();
            var result = Mapper.Map<CU_Role_Program, RoleProgramModel>(data);
            return result;
        }

    }
}
