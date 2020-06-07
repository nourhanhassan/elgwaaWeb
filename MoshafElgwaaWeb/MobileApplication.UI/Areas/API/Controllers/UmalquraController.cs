using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.API.Controllers
{
    public class UmalquraController : Controller
    {
        UmAlQuraCalendar umalqura = new UmAlQuraCalendar();

        public JsonResult Convert(string convertDirection, string gregDate, string hijriDate)
        {
            if (gregDate == "0") gregDate = "";

            string returnedGregDate = null;
            string returnedHijriDate = null;

            if (convertDirection == "gth")
            {
                returnedGregDate = gregDate;
                if (gregDate != "")
                {
                    returnedHijriDate = GetHijriDateString(DateTime.Parse(gregDate));
                }
                else returnedHijriDate = "";

            }
            else if (convertDirection == "htg")
            {
                returnedGregDate = QvLib.QVUtil.Date.HijriToGreg(hijriDate, "yyyy-MM-dd");
                returnedHijriDate = hijriDate;
            }

            return Json(new { gregDate = returnedGregDate, hijriDate = returnedHijriDate }, JsonRequestBehavior.AllowGet);
        }

        private string GetHijriDateString(DateTime gregDate)
        {
            return string.Format("{0}-{1:00}-{2:00}",
                       umalqura.GetYear(gregDate),
                       umalqura.GetMonth(gregDate),
                       umalqura.GetDayOfMonth(gregDate)
                   );
        }

        public string GetHijriDate(string gregDate)
        {
            var dateGreg = DateTime.ParseExact(gregDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return string.Format("{0}-{1:00}-{2:00}",
                          umalqura.GetYear(dateGreg),
                          umalqura.GetMonth(dateGreg),
                          umalqura.GetDayOfMonth(dateGreg)
                      );
     
        }
    }
}