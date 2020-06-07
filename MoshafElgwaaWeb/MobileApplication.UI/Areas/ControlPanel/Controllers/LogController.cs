using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApplication.DataService;
using MobileApplication.DataModel;
using MobileApplication.UI.ControlPanel.Controllers;
using MobileApplication.UI.InfraStructure;
using DataModel.Extention;


namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class LogController : BaseController
    {
       private readonly CU_LogService _logService;
       
        bool IsAdmin = true;
        public LogController()
        {
            _logService = new CU_LogService();
        }

        [HttpGet]
        [UserPermission(QVEnterprise.ActionType.View)]
        public ActionResult Default(UserProfile profile)
        {
            IsAdmin = true;
            ViewBag.CurrUserID = profile.Id;
            return View();
        }

        public ActionResult GetLogs(UserProfile profile, string startDate, string endDate, int IdPage = 0, int IdAction = 0, string EmployeeName = "", int skip = 0, int take = 10, bool bNotLog = true)
        {
            ViewBag.CurrUserID = profile.Id;
            IsAdmin = true;
            int total = 0;
            DateTime? datefrom = startDate.HijriToGregDateObj();
            DateTime? dateto = endDate.HijriToGregDateObj();
           
            List<LogModel> logs = _logService.Select(skip, take, datefrom, dateto, IdPage, IdAction, EmployeeName, (int)ViewBag.CurrUserID, IsAdmin, out total).Select(eq => new LogModel(eq)).ToList();
            ViewBag.total = total;
            return PartialView(logs);
        }

    }












}