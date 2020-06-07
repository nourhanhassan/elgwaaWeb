using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using MobileApplication.Areas.ControlPanel.Models;
using MobileApplication.UI.InfraStructure;
using MobileApplication.DataService;


namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public abstract class _ReportBaseController : BaseController
    {
        protected readonly ReportService _ReportService;

        public _ReportBaseController()
        {
            _ReportService = new ReportService();
        }

        public virtual ActionResult Default(UserProfile profile)
        {
            return View("~/Areas/Deduction/Views/Report/_ReportSearch.cshtml");
        }

        public abstract PartialViewResult Submit(UserProfile profile, ReportSearhVM searchModel);

        protected PartialViewResult ReturnReportResult()
        {
            string reportViewerName = "~/Views/Shared/_ReportViewer.cshtml";
            string reportName = this.RouteData.GetRequiredString("controller");

            return PartialView(reportViewerName, reportName);
        }
    }
}