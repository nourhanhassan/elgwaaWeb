using MobileApplication.DataModel.ControlPanel.DoaaModels;
using MobileApplication.DataService.ControlPanel;
using MobileApplication.UI.ControlPanel.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class DoaaController : ViewGridBaseController<DoaaModel>
    {
        private readonly DoaaService _doaaService;
        public DoaaController()
            : base()
        {
            _doaaService = new DoaaService();
        }
        //public JsonResult GetDoaaCategoryList(int DoaaMainCategoryID)
        //{
        //    return Json(_doaaService.GetDoaaCategoryList(DoaaMainCategoryID), JsonRequestBehavior.AllowGet);
        //}
	}
}