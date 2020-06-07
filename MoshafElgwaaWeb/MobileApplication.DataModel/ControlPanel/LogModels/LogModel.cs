using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobileApplication.DataModel.QvDataAnnotation;
using Service;
using DataModel.Enum;

namespace MobileApplication.DataModel
{
    public class LogModel
    {
        public int LogID { get; set; }
        public string ActionName { get; set; }
        public string ArabicDateTime { get; set; }

        [String150]
        public string EmployeeName { get; set; }
        public string PageName { get; set; }
        public string Json { get; set; }
        public int IdAction { get; set; }
        public DateTime ActionTime { get; set; }
        public int IdUser { get; set; }
        public int? IdPage { get; set; }
        public string Page_URL { get; set; }
        public string Value { get; set; }
        public string IpAddress { get; set; }
        public bool? IsMobile { get; set; }

        public string startDate { get; set; }
        public string endDate { get; set; }
        public string Action { get; set; }

        public LogModel(LogRecord dbLog)
        {
            this.ActionName = dbLog.ActionName;
            this.ArabicDateTime = dbLog.ArabicDateTime;
            this.EmployeeName = dbLog.EmployeeName;
            this.Json = dbLog.Json;
            this.LogID = dbLog.LogID;
            this.PageName = dbLog.PageName;
            this.IdAction = dbLog.IdAction;
            this.ActionTime = dbLog.ActionTime;
            this.IdUser = dbLog.IdUser;
            this.IdPage = dbLog.IdPage;
            this.Page_URL = dbLog.Page_URL;
            this.IpAddress = dbLog.IpAddress;
            this.ActionTime = dbLog.ActionTime;
            this.Page_URL = dbLog.Page_URL;
            this.IsMobile = dbLog.IsMobile;
            this.Action = dbLog.Action;
        }
    }

    #region helper Classes log

    public class LogPattren
    {
        public int LogID { get; set; }
        public int IdAction { get; set; }
        public DateTime ActionTime { get; set; }
        public int IdUser { get; set; }
        public int? IdPage { get; set; }
        public string Value { get; set; }
        public string IpAddress { get; set; }
        public bool? IsMobile { get; set; }
        public string Action { get; set; }
       
    }

    public class LogRecord
    {
        public int LogID { get; set; }
        public string ActionName { get; set; }
        public string ArabicDateTime { get; set; }
        public string EmployeeName { get; set; }
        public string PageName { get; set; }
        public string Json { get; set; }
        public int IdAction { get; set; }
        public DateTime ActionTime { get; set; }
        public int IdUser { get; set; }
        public int? IdPage { get; set; }
        public string Page_URL { get; set; }
        //public string Value { get; set; }
        public string IpAddress { get; set; }
        public bool? IsMobile { get; set; }
        public string Action { get; set; }
    }

    //<summary>
    //this class is use in serialization  json of update row whitch have the id and title and the rows updated
    //</summary>
    public class LogEdit
    {
        public string ID { get; set; }
        public string itemText { get; set; }
        public List<LogEditItem> updates { get; set; }
        public List<ManyToManyItem> ManyToManyItems { get; set; }
    }

    /// <summary>
    /// this class is use in serialization  json of  updated items
    /// </summary>
    public class LogEditItem
    {
        public string columnName { get; set; }
        public string text { get; set; }
        public string old_value { get; set; }
        public string new_value { get; set; }
    }

    public class ManyToManyItem
    {
        public ActionType Type { get; set; }
        public string text { get; set; }
        public string value { get; set; }
        public string old_value { get; set; }
    }
   
    /// <summary>
    /// this class is use in serialization  json of inserted item
    /// </summary>
    public class LogInsertItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
    }

    #endregion
}