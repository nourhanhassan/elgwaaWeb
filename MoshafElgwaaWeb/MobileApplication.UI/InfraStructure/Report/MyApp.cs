using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.UI.InfraStructure
{
    public class MyApp
    {
        public static List<ReportParameter> ReportParameters
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["ReportParameters"] == null)
                    System.Web.HttpContext.Current.Session["ReportParameters"] = new List<ReportParameter>();
                return System.Web.HttpContext.Current.Session["ReportParameters"] as List<ReportParameter>;
            }
        }
        public static List<ReportDataSource> ReportDataSources
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["ReportDataSources"] == null)
                    System.Web.HttpContext.Current.Session["ReportDataSources"] = new List<ReportDataSource>();
                return System.Web.HttpContext.Current.Session["ReportDataSources"] as List<ReportDataSource>;
            }
        }
        public static SubreportProcessingEventHandler SubreportProcEventHandler
        {
            get { return System.Web.HttpContext.Current.Session["SubreportProcEventHandler"] as SubreportProcessingEventHandler; }
            set { System.Web.HttpContext.Current.Session["SubreportProcEventHandler"] = value; }
        }
    
    }
}