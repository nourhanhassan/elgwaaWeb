using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Compilation;
using MobileApplication.UI.InfraStructure;

namespace MobileApplication.UI.Areas.API.Controllers
{
    public class SelectListController : Controller
    {
        //
        // GET: /API/SelectList/


        public ActionResult GetSelectListItems(string TName,  object[] Params)
        {
            
            var ModelType = BuildManager.GetType("Service.Contracts.Models" + TName, false);
            var lookupManager = typeof(LookupManger<>).MakeGenericType(ModelType);
           var ret = lookupManager.GetMethod("GetSelectListItem").Invoke(lookupManager, new object[] { "Name", "ID" });

          return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetGlobalsSelectListItems(string FuncName , object[] args,UserProfile profile)
        {
            var GlobalsType = typeof(Globals);
            if (FuncName == "GetPropertiesByBenefactorID")
            {
               var argsList= args.ToList();
               argsList.Add(profile);
               args = argsList.ToArray();
            }
            var ret = GlobalsType.GetMethod(FuncName).Invoke(GlobalsType, args);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}