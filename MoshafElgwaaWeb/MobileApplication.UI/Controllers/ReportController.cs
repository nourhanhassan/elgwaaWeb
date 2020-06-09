using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApplication.UI.Areas.ControlPanel.Controllers;
using MobileApplication.UI.Models;

namespace MobileApplication.Controllers
{
    public class ReportController : BaseController
    {
        public ActionResult LoadReport(string reportName)
        {
            string reportPageUrl = "/Report/ReportPage.aspx?reportName=" + reportName;

            // return view
            return View("Views/Shared/LayoutReport.cshtml", new ReportModel { ReportPageUrl = reportPageUrl });
			/////btanch test
        }
    }
}