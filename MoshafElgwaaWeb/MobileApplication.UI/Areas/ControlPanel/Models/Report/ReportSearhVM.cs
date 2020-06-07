using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.Areas.ControlPanel.Models
{
    public class ReportSearhVM
    {
        public int? CityID { get; set; }
        public bool HasSearchCity { get; set; }
        
    }

    public class ReportResultVM
    {
        public bool Succeeded { get; set; }
        public string ReportResultHTML { get; set; }
    }
}