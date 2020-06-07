using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericEngine.Service.SharedService;
using MobileApplication.DataService;
using MobileApplication.Context;

namespace MobileApplication.DataService
{
    public class CU_Role_PageService :BaseService
    {
        private readonly Repository<CU_Role_Page> _CU_Role_PageServiceRepository;

        public CU_Role_PageService()
        {
            _CU_Role_PageServiceRepository = new Repository<CU_Role_Page>(_unitOfWork);

        }

        public IEnumerable<CU_Role_Page> CU_RolepageList
        {
            get { return _CU_Role_PageServiceRepository.GetList(); }
        }

        public IEnumerable<CU_Role_Page> CU_Rolepages()
        {
            return CU_RolepageList;
        }
        public List<CU_Role_Page> GetRolePageByRoleID(int iRoleId)
        {

            return CU_RolepageList.Where(i => i.IdRoleProgram == iRoleId).ToList();
        }

        public CU_Role_Page GetRolePageById(int RolePageId)
        {
            return _CU_Role_PageServiceRepository.GetById(RolePageId);
        }

        public int Save(CU_Role_Page obj)
        {
            _CU_Role_PageServiceRepository.Save(obj);
            return _unitOfWork.Submit();
        }

    }
}