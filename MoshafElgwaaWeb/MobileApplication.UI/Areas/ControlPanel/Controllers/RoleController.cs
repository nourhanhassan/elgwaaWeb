using System.Web.Mvc;
using MobileApplication.UI.ControlPanel.Controllers;
using MobileApplication.DataModel;
using MobileApplication.DataService;
using MobileApplication.UI.InfraStructure;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class RoleController : ViewGridBaseController<RoleModelLookup>
    {
        private readonly CU_RoleService _roleService;
        public RoleController()
        {
            _roleService = new CU_RoleService();
        }

        public override JsonResult Save(RoleModelLookup Model, string command, UserProfile profile)
        {
            if (ModelState.IsValid)  //true )//
            {
                var actionName = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
                int result =  _roleService.Save(Model, profile.Id ?? 0, actionName);
                return Json(new { ID = result, command }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ID = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        public override bool Delete(int id, UserProfile profile, string tableName = "")
        {
            var url = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
            return _roleService.Delete(id, profile.Id ?? 0, url);

        }

        //[HttpGet]
        //public override JsonResult GetDataList(int page, int count, Dictionary<string, string> sorting, Dictionary<string, string> filter, UserProfile profile)
        //{

        //    filter.Add("IsDeleted", "False");
        //    var URL = this.ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery;
        //    GridModel gridModel = _CalledService.GetDataList(sorting, filter, page, count, URL, (int)profile.Id);
        // //   System.Linq.Expressions.ParameterExpression fieldName = System.Linq.Expressions.Expression.Parameter(typeof(object), "IsDeleted");
        // //   System.Linq.Expressions.Expression fieldExpr = System.Linq.Expressions.Expression.PropertyOrField(System.Linq.Expressions.Expression.Constant(gridModel.data), "IsDeleted");

        // //   System.Linq.Expressions.Expression<Func<object, bool>> exp2 = System.Linq.Expressions.Expression.Lambda<Func<object, bool>>(fieldExpr, fieldName);
        // ////   PropertyInfo val = RoleModelLookup.GetProperty("IsDeleted").GetValue(gridModel);
        // //    var result = gridModel.data.ToList().Where(exp2);

        //   //Expression<Func<bool, PropertyInfo>> d = (Expression<Func<bool, PropertyInfo>>)(x => x.);

        //    // var DataList = IQuarableDataList.Skip((page - 1) * count).Take(count).ToList();
        //    //  var x = _CalledService.GetById(1);
        //    //var xx=  _CalledService.Delete(1);
        //    return Json(gridModel, JsonRequestBehavior.AllowGet);
        //}  


    }
}