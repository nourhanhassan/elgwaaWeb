using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using MobileApplication.UI.InfraStructure;

namespace MobileApplication.UI.Report
{
    public partial class ReportPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string reportPath = Server.MapPath("~/Reports/") + Convert.ToString(Request.QueryString["reportName"]) + ".rdlc";
                if (File.Exists(reportPath))
                {
                    // var umalqura = new System.Globalization.UmAlQuraCalendar();
                    var umalqura = new System.Globalization.GregorianCalendar();
                    string currentDateTime = string.Format("{0}-{1}-{2} {3:00}:{4:00}",
                                                        umalqura.GetYear(DateTime.Now),
                                                        umalqura.GetMonth(DateTime.Now),
                                                        umalqura.GetDayOfMonth(DateTime.Now),
                                                        umalqura.GetHour(DateTime.Now),
                                                        umalqura.GetMinute(DateTime.Now)
                                                    );
                    // report viewer
                    repv.Reset();
                    repv.LocalReport.ReportPath = reportPath;


                    string imagePath = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + "/Content/Report/report_header.png";

                    ReportParameterCollection param = new ReportParameterCollection();
                    param.Add(new ReportParameter("ImagePath", imagePath));
                    param.Add(new ReportParameter("ReportDateTime", currentDateTime));

                    //string headerFontColor = HttpContext.GetGlobalResourceObject("Thabat", "HeaderFontColor").ToString();
                    //string headerBackgroungColor = HttpContext.GetGlobalResourceObject("Thabat", "HeaderBackgroundColor").ToString();

                    //param.Add(new ReportParameter("HeaderFontColor", headerFontColor));
                    //param.Add(new ReportParameter("HeaderBackgroundColor", headerBackgroungColor));

                    //string titleFontColor = HttpContext.GetGlobalResourceObject("Thabat", "TitleFontColor").ToString();
                    //string titleBackgroungColor = HttpContext.GetGlobalResourceObject("Thabat", "TitleBackgroundColor").ToString();

                    //param.Add(new ReportParameter("TitleFontColor", titleFontColor));
                    //param.Add(new ReportParameter("TitleBackgroundColor", titleBackgroungColor));
                    // repv.LocalReport.SetParameters(new ReportParameter("ImagePath", imagePath));

                    //string headerSubreportBackground = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBackground").ToString();
                    //string headerSubreportBGLevel_1 = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBGLevel_1").ToString();
                    //string headerSubreportBGLevel_2 = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBGLevel_2").ToString();
                    //string headerSubreportBGLevel_3 = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBGLevel_3").ToString();
                    //string headerSubreportBGLevel_4 = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBGLevel_4").ToString();
                    //string headerSubreportBGLevel_5 = HttpContext.GetGlobalResourceObject("Thabat", "HeaderSubreportBGLevel_5").ToString();

                    //param.Add(new ReportParameter("HeaderSubreportBackground", headerSubreportBackground));
                    //param.Add(new ReportParameter("HeaderSubreportBGLevel_1", headerSubreportBGLevel_1));
                    //param.Add(new ReportParameter("HeaderSubreportBGLevel_2", headerSubreportBGLevel_2));
                    //param.Add(new ReportParameter("HeaderSubreportBGLevel_3", headerSubreportBGLevel_3));
                    //param.Add(new ReportParameter("HeaderSubreportBGLevel_4", headerSubreportBGLevel_4));
                    //param.Add(new ReportParameter("HeaderSubreportBGLevel_5", headerSubreportBGLevel_5));

                    repv.LocalReport.EnableExternalImages = true;
                    repv.LocalReport.SetParameters(param);


                    // parameters
                    if (MyApp.ReportParameters != null) foreach (var prm in MyApp.ReportParameters)
                        {
                            //string x = prm.Values[0].ToString();
                            repv.LocalReport.SetParameters(prm);


                        }
                    // datasources
                    repv.LocalReport.DataSources.Clear();
                    if (MyApp.ReportDataSources != null) foreach (var rds in MyApp.ReportDataSources) repv.LocalReport.DataSources.Add(rds);
                    repv.LocalReport.SubreportProcessing += MyApp.SubreportProcEventHandler;
                    repv.LocalReport.Refresh();
                }
                else if (!reportPath.Contains(".gif"))
                {
                    //Response.Write("Check report path.");
                }

            }
        }
    }
}