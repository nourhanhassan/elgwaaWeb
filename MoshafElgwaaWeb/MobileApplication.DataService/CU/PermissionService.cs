using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Service;
using AutoMapper;


using GenericEngine.Service.SharedService;
using MobileApplication.DataModel;
using MobileApplication.Context;


namespace MobileApplication.DataService
{
    public class PermissionService
    {
        private readonly CU_Role_PageService _CU_Role_PageService;
        private readonly CU_PageService _CU_PageService;
        private readonly CU_ActionService _CU_ActionService;
        private readonly CU_LogService _CU_LogService;
        protected ILogService _logService;


        public PermissionService()
        {
            _CU_Role_PageService = new CU_Role_PageService();
            _CU_PageService = new CU_PageService();
            _CU_ActionService = new CU_ActionService();
            _CU_LogService = new CU_LogService();
        }

        public IEnumerable<PermissionModel> Permission(Dictionary<string, string> sorting, Dictionary<string, string> filter)
        {
            int RoleID = filter.ContainsKey("RoleID") ? int.Parse(filter["RoleID"]) : new CU_RoleService().RolesList.FirstOrDefault().ID;

            List<CU_Role_Page> RolePagesList = _CU_Role_PageService.GetRolePageByRoleID(RoleID).Where(a => a.CU_Page.IsDeleted == false /*&& a.CU_Page.IsActive == true*/).ToList();
            List<CU_Page> PagesList = _CU_PageService.CU_PageList.Where(p => !RolePagesList.Select(r => r.IdPage).Contains(p.ID)).ToList();

            if (PagesList.Count > 0)
            {
                foreach (CU_Page p in PagesList)
                {

                    CU_Role_Page OldObj = new CU_Role_Page() { ID = 0, IdPage = p.ID, IdRoleProgram = RoleID, Permission = new System.Text.StringBuilder().Insert(0, "0", _CU_ActionService.CU_ActionList.Count()).ToString() };
                    _CU_Role_PageService.Save(OldObj);


                    //_CU_Role_PageService.Save(0, RoleID, p.ID, new System.Text.StringBuilder().Insert(0, "0", _CU_ActionService.CU_ActionList.Count()).ToString());
                }
                RolePagesList = _CU_Role_PageService.GetRolePageByRoleID(RoleID);
            }

            if (RolePagesList.Count == 0)
            {
                _CU_PageService.CU_PageList.ToList().ForEach(delegate(CU_Page p)
                {
                    if (_CU_Role_PageService.CU_RolepageList.Where(i => i.IdPage == p.ID && i.IdRoleProgram == RoleID).Count() == 0)
                    {
                        CU_Role_Page OldObj = new CU_Role_Page() { ID = 0, IdPage = p.ID, IdRoleProgram = RoleID, Permission = new System.Text.StringBuilder().Insert(0, "0", _CU_ActionService.CU_ActionList.Count()).ToString() };
                        _CU_Role_PageService.Save(OldObj);
                        //_CU_Role_PageService.Save(0, RoleID, p.ID, new System.Text.StringBuilder().Insert(0, "0", _CU_ActionService.CU_ActionList.Count()).ToString());
                    }
                });
                RolePagesList = _CU_Role_PageService.GetRolePageByRoleID(RoleID);
            }


            var data = RolePagesList/*.Where(r => r.CU_Page.IsActive == true).OrderBy(x => x.CU_Page.PageOrder)*/.Select(i => new PermissionModel
             {
                 ID = i.ID,
                 PageName = i.CU_Page.Name,
                 IdPage = i.CU_Page.ID,
                 View = (_CU_ActionService.Action("View").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("View").Order, 1) == "1"),
                 Insert = (_CU_ActionService.Action("Insert").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Insert").Order, 1) == "1"),
                 Update = (_CU_ActionService.Action("Update").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Update").Order, 1) == "1"),
                 Delete = (_CU_ActionService.Action("Delete").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Delete").Order, 1) == "1"),
                 Password = (_CU_ActionService.Action("Password").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Password").Order, 1) == "1"),
                 Admin = (_CU_ActionService.Action("Admin").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Admin").Order, 1) == "1"),
                 Report = (_CU_ActionService.Action("Report").Order < i.Permission.Length && i.Permission.Substring(_CU_ActionService.Action("Report").Order, 1) == "1"),
                 All = i.Permission.Contains("1"),
                 PagePermissions = (_CU_PageService.GetCU_PageById(i.IdPage).Actions),
                 Permission = i.Permission,
                 SubPagesCount =0// i.CU_Page.CU_Page1.Count
             }).ToList();

            var groupedData = data.GroupBy(d => d.IdPage).Select(p=>p.First()).Where(p=>!(p.SubPagesCount>0));
            return groupedData.ToList();
        }

        public int Save(int ID, string PageName, bool View, bool Insert, bool Update, bool Delete, bool Password, bool Admin, bool Report, int? UserId, string ActionName)
        {

            char[] permission = new char[8];
            permission[0] = View ? '1' : '0';
            permission[1] = Insert ? '1' : '0';
            permission[2] = Update ? '1' : '0';
            permission[3] = Delete ? '1' : '0';
            permission[4] = '0';
            permission[5] = Admin ? '1' : '0';
            permission[6] = Password ? '1' : '0';
            permission[7] = Report ? '1' : '0';



            var obj = _CU_Role_PageService.GetRolePageById(ID);
            CU_Role_Page OldObj = ((CU_Role_Page)obj).Clone();
            //if(obj.CU_Page.ParentID > 0)
            //{
            //  var rolePages=  _CU_Role_PageService.GetRolePageByRoleID(obj.IdRoleProgram);
            //    var pages =rolePages.Select(rg=>rg.IdPage);
            //    if(pages.Contains((int)obj.CU_Page.ParentID))
            //    {
            //        var rolePage = rolePages.First(p=>p.IdPage==(int)obj.CU_Page.ParentID);
            //        rolePage.Permission = "1000000";
            //        _CU_Role_PageService.Save(rolePage);
            //    }
            //    else
            //    {
            //        var rolePage = new CU_Role_Page()
            //        {
            //            IdPage = (int)obj.CU_Page.ParentID,
            //            IdRoleProgram = obj.IdRoleProgram,
            //            Permission = "1000000",
                   
            //        };
            //        _CU_Role_PageService.Save(rolePage);
            //    }
            //}
            obj.Permission = new string(permission);

            _CU_Role_PageService.Save(obj);
            _CU_LogService.Update(obj.CU_Role_Program.CU_Role.ID, obj.CU_Role_Program.CU_Role.Name + DataServiceArabicResource.In+ obj.CU_Page.Name, ParsePermissions(OldObj), ParsePermissions(obj), UserId,"/ControlPanel/"+obj.CU_Page.URL+"/"+ActionName);

            return obj.ID;
        }

        private CU_Role_Page ParsePermissions(CU_Role_Page obj)
        {

            CU_Role_Page retObj = obj.Clone<CU_Role_Page>();

            //  _CU_ActionRepository.GetList(); 
            retObj.Permission = "";
            for (int i = 0; i < obj.Permission.Length; i++)
            {
                if (obj.Permission[i] == '1')
                {
                    retObj.Permission += "," + _CU_ActionService.GetAllActions.Where(a => a.Order == i).FirstOrDefault().Name;
                }

            }

            return retObj;

        }


    }
}
