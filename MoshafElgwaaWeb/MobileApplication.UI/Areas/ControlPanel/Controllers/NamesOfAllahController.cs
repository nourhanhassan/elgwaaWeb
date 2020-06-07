using MobileApplication.DataModel.ControlPanel.NamesOfAllahModels;
using MobileApplication.DataService.ControlPanel;
using MobileApplication.UI.ControlPanel.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class NamesOfAllahController : ViewGridBaseController<NameOfAllahModel>
    {
        private readonly NamesOfAllahService _namesOfAllahService;
        public NamesOfAllahController()
            : base()
        {
            _namesOfAllahService = new NamesOfAllahService();
        }
	}
}