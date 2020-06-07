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
    public class DoaaItemSourceController : ViewGridBaseController<DoaaItemSourceModel>
    {
        //
        // GET: /ControlPanel/DoaaItemSource/
        private readonly DoaaService _doaaService;
        public DoaaItemSourceController()
            : base()
        {
            _doaaService = new DoaaService();
        }
	}
}