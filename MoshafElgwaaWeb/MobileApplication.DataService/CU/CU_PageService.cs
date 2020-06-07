using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using Repository;
using GenericEngine.Service.SharedService;
using MobileApplication.DataService;
using MobileApplication.Context;
using MobileApplication.DataModel;


namespace MobileApplication.DataService
{
    public class CU_PageService : BaseService
    {
        
        CU_Role_PageService _RolePageServiceObj;
        public int progID = Extention.GeKeyValue<int>("ProgramID");
        private readonly Repository<CU_Page> _CU_PageServiceRepository;

        public CU_PageService()
        {

            _CU_PageServiceRepository = new Repository<CU_Page>(_unitOfWork);
            _RolePageServiceObj = new CU_Role_PageService();
        } 

        public IEnumerable<CU_Page> CU_PageList
        {
            get { return _CU_PageServiceRepository.GetList().Where(x => !x.IsDeleted).Where(x => x.IdProgram == progID); }
        }

        public IEnumerable<PageModel> ParentPageList
        {
            get
            {
                
              //  var temp = _CU_PageServiceRepository.GetList().Where(x => !x.IsDeleted && x.IsActive.HasValue && x.IsActive.Value).OrderBy(x => x.PageOrder);
                var data = Mapper.Map<IEnumerable<CU_Page>, IEnumerable<PageModel>>(CU_PageList);
                return data;
            }
        }

        public Dictionary<int, string> GetPageIdsByURLs(IEnumerable<string> pageUrls)
        {

            pageUrls = pageUrls.Select(a => a.ToLower());

            var pages = _CU_PageServiceRepository.GetByFilter(a => pageUrls.Contains(a.URL)).Select(a => new { ID = a.ID, URL = a.URL.ToLower() }).ToDictionary(a => a.ID, b => b.URL);
            return pages;
        }
        public PageModel GetPageModelByPageURL(string strPageURL)
        {
            var page = CU_PageList.Where(a => a.URL.ToLower() == strPageURL.ToLower()).FirstOrDefault();
            return Mapper.Map<CU_Page, PageModel>(page);
        }

        public CU_Page GetCU_PageById(int Id)
        {
            return _CU_PageServiceRepository.GetById(Id);
        }

        public PageModel GetPageById(int Id)
        {
            var data=_CU_PageServiceRepository.GetById(Id);
            var result=Mapper.Map<CU_Page,PageModel>(data);
            return result;
        }

        public CU_Page GetCU_PageByPageURL(string strPageURL)
        {
            return CU_PageList.Where(a => a.URL.ToLower() == strPageURL.ToLower()).FirstOrDefault();
        }

        public CU_Page GetCU_PageByPageURLEvenIfDeleted(string strPageURL)
        {
            if (strPageURL != null)
                return _CU_PageServiceRepository.GetList().Where(x => x.IdProgram == progID).Where(a => a.URL.ToLower() == strPageURL.ToLower()).FirstOrDefault();
            else return null;
        }

        public IEnumerable<PageModel> GetPageChilds(string url)
        {
           
            var parent = GetCU_PageByPageURL(url);
            var pages = CU_PageList.Where(p=>p.ParentID==parent.ID).OrderBy(x => x.PageOrder);

            var data = Mapper.Map<IEnumerable<CU_Page>, IEnumerable<PageModel>>(pages);
            return data;
        }
        public IEnumerable<PageModel> GetPages(Dictionary<string, string> filter)
        {
            var pages = CU_PageList;
            //.OrderBy(x => x.PageOrder);

            var data = Mapper.Map<IEnumerable<CU_Page>, IEnumerable<PageModel>>(pages);
            return data;
        }


        public IEnumerable<PageModel> GetAuthorizedPagesByUserID(int UserID)
        {
            var RolePages = _RolePageServiceObj.CU_Rolepages();

            var temp = (from page in CU_PageList
                        join rolePage in RolePages
                            on page.ID equals rolePage.IdPage
                          where(rolePage.CU_Role_Program.CU_Employee_RoleProgram.Where(a=>a.IdEmployee == UserID)).Count() > 0
                          && (rolePage.Permission.Contains("1"))
                          select page).Distinct().AsEnumerable();
            //where (rolePage.CU_Role.CU_Employee_Role.Where(a => a.IdUser == UserID)).Count() > 0
            //&& (rolePage.Permission.Contains("1")
            //|| (rolePage.CU_Page.CU_Log.Where(l => l.IdUser == UserID).Count() > 0))
            //select page).Distinct().AsEnumerable();


            var data = Mapper.Map<IEnumerable<CU_Page>, IEnumerable<PageModel>>(temp);
            return data;
        }
        public IEnumerable<PageModel> GetAuthorizedParentPagesByUserID(int UserID)
        {
            var RolePages = _RolePageServiceObj.CU_Rolepages();

            var temp = (from page in CU_PageList
                        join rolePage in RolePages
                            on page.ID equals rolePage.IdPage
                        where (rolePage.CU_Role_Program.CU_Employee_RoleProgram.Where(a => a.IdEmployee == UserID)).Count() > 0
                        && (rolePage.Permission.Contains("1"))
                        && (page.ParentID != null)
                        select page.CU_Page2).Distinct().AsEnumerable();


            var data = Mapper.Map<IEnumerable<CU_Page>, IEnumerable<PageModel>>(temp);
            return data;
        }

        public int Save(PageModel obj)
        {
            var data = Mapper.Map<PageModel, CU_Page>(obj);
           // data.PageOrder = obj.PageOrder1;
            _CU_PageServiceRepository.Save(data);
            _unitOfWork.Submit();
            return (int)data.ID;
        }


        public bool CheckPageOrder(int? Id, int PageOrder)
        {
            bool Return = false;

            if (!Id.HasValue)
                Id = 0;
            Return = !CU_PageList.Any(i => (Id == 0 || i.ID != Id));
            return Return;
        }
   
    }
    }
