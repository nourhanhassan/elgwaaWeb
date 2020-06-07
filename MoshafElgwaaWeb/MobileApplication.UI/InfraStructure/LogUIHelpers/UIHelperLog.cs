//using Resources;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//using QV.Service.Security;
using Resources;
using MobileApplication.DataModel;
using DataModel.Enum;
using MobileApplication.DataService;




namespace MobileApplication.UI.InfraStructure
{

    public class UIHelperLog
    {
        /// <summary>
        /// Template for General Log
        /// </summary>
        /// <param name="lstLogs"></param>
        /// <returns></returns>
        //public static String GetLogTempaltes(List<LogModel> lstLogs)
        //{
        //    string LogssTempaltes = string.Empty;
        //    if (lstLogs != null & lstLogs.Count() > 0)
        //    {
        //        foreach (LogModel objLog in lstLogs)
        //        {
        //            string strlogDetails = string.Empty;
        //            string strActionText = string.Empty;
        //            string strActionDetails = string.Empty;
        //            string strAddedEditedObjLnk = string.Empty;
        //            string strAddedEditedPageLnk = string.Empty;
        //            string strAddEditObjName = string.Empty;

        //            string strPageLink = string.Empty;

        //            string strClassIcon = string.Empty;

        //            string strEitPossibleLink = string.Empty;
        //            string strDefaultPossibleLink = string.Empty;
        //            string strArea = string.Empty;
        //            Dictionary<string[], List<string>> items = new Dictionary<string[], List<string>>();

        //            #region IconClass
        //            switch (objLog.IdAction)
        //            {
        //                case (int)ActionType.View:
        //                    strClassIcon = "fa fa-eye";
        //                    strActionText = LogTemplate.ViewText;
        //                    break;

        //                case (int)ActionType.Insert:
        //                    strClassIcon = "fa fa-pencil";
        //                    strActionText = LogTemplate.InsertText;
        //                    items = new CU_LogService().GetInsertItems(objLog.Json, objLog.Page_URL);
        //                    break;
        //                case (int)ActionType.Update:
        //                    strClassIcon = "fa fa-pencil";
        //                    strActionText = LogTemplate.EditText;
        //                    items = new CU_LogService().GetEditItems(objLog.Json, objLog.Page_URL);
        //                    break;

        //                case (int)ActionType.Password:
        //                    strClassIcon = "fa fa-lock";
        //                    strActionText = LogTemplate.PasswordText;
        //                    strActionDetails = objLog.Json;
        //                    break;
        //                case (int)ActionType.Delete:
        //                    strClassIcon = "fa fa-eraser";
        //                    strActionText = LogTemplate.DeleteText;
        //                    strActionDetails = objLog.Json;
        //                    break;

        //                case (int)ActionType.Logout:
        //                    strClassIcon = "fa fa-sign-out";
        //                    strActionText = LogTemplate.LogOutText;
        //                    break;

        //                case (int)ActionType.Login:
        //                    strClassIcon = "fa fa-sign-in";
        //                    strActionText = LogTemplate.LoginText;
        //                    strActionDetails = objLog.Json;
        //                    break;

        //                default:
        //                    break;
        //            }
        //            #endregion


        //            #region logDetails
        //            //log record details redirection
        //            HandleLogRecordsLinks(objLog.Page_URL,objLog.Action, out strEitPossibleLink, out strDefaultPossibleLink);
        //            strPageLink = LogTemplate.PageLink.Replace(LogTemplate.PageName, objLog.PageName).Replace(LogTemplate.PageURL, strDefaultPossibleLink);
        //            strlogDetails += strActionText;

        //            if ((objLog.IdAction == (int)ActionType.Insert || objLog.IdAction == (int)ActionType.Update ) && items.Keys.FirstOrDefault()[0] != null)
        //            {


        //                strAddedEditedPageLnk = strEitPossibleLink;//+ "?Id=" + items.Keys.FirstOrDefault()[0];
        //                strAddEditObjName = items.Keys.FirstOrDefault()[1];
        //                //.Replace("@PageLink", strPageLink).
        //                strAddedEditedObjLnk = LogTemplate.AddedEditedObjLnk.Replace(LogTemplate.AddEditPageLink, strAddedEditedPageLnk).Replace(LogTemplate.AddEditObjName, strAddEditObjName);

        //                //append to logDetails
        //                strlogDetails += strAddedEditedObjLnk;

        //                //fields details
        //                strActionDetails = "<ul>";
        //                foreach (string item in items.Values.FirstOrDefault())
        //                {
        //                    strActionDetails += "<li>" + item + "</li>";
        //                }
        //                strActionDetails += "</ul>";

        //            }
        //            else
        //            {
        //                if (objLog.IdAction == (int)ActionType.Delete)
        //                {
        //                    strlogDetails += strActionDetails;
        //                }
        //                //page default link
        //                strlogDetails += strPageLink;

        //            }



        //            if (objLog.IdAction != (int)ActionType.Delete)
        //            {
        //                strlogDetails += strActionDetails;
        //            }
        //            #endregion

        //            string template = LogTemplate.LogRecordItem.Replace(LogTemplate.logID, objLog.LogID.ToString())
        //            .Replace(LogTemplate.Action, objLog.ActionName.ToString())
        //            .Replace(LogTemplate.ClassIcon, strClassIcon.ToString())
        //            .Replace(LogTemplate.IpAddress, objLog.IpAddress)
        //            .Replace(LogTemplate.logDate, objLog.ArabicDateTime.ToString())
        //            .Replace(LogTemplate.HourAction, objLog.ActionTime.ToString("HH"))
        //            .Replace(LogTemplate.MinAction, objLog.ActionTime.ToString("mm"))
        //            .Replace(LogTemplate.UserName, objLog.EmployeeName)
        //            .Replace("@logDetails", strlogDetails);
        //            ////Add Log Tempalte to All Tempaltes 
        //            LogssTempaltes += template;

        //        }
        //    }
        //    else
        //    {
        //        LogssTempaltes = LogTemplate.NotFoundLog;
        //    }
        //    return LogssTempaltes;
        //}




        public static String GetLogTempaltes(List<LogModel> lstLogs)
        {
            string LogssTempaltes = string.Empty;
            if (lstLogs != null & lstLogs.Count() > 0)
            {
                foreach (LogModel objLog in lstLogs)
                {
                    string strlogDetails = string.Empty;
                    string strActionText = string.Empty;
                    string strActionDetails = string.Empty;
                    string strAddedEditedObjLnk = string.Empty;
                    string strAddedEditedPageLnk = string.Empty;
                    string strAddEditObjName = string.Empty;

                    string strPageLink = string.Empty;

                    string strClassIcon = string.Empty;

                    string strEitPossibleLink = string.Empty;
                    string strDefaultPossibleLink = string.Empty;
                    string strArea = string.Empty;
                    Dictionary<string[], List<string>> items = new Dictionary<string[], List<string>>();

                    #region IconClass
                    switch (objLog.IdAction)
                    {
                        case (int)ActionType.View:
                            strClassIcon = "fa fa-eye";
                            strActionText = LogTemplate.ViewText;
                            break;

                        case (int)ActionType.Insert:
                            strClassIcon = "fa fa-pencil";
                            strActionText = LogTemplate.InsertText;
                            items = new CU_LogService().GetInsertItems(objLog.Json, objLog.Page_URL);
                            break;
                        case (int)ActionType.Update:
                            strClassIcon = "fa fa-pencil";
                            strActionText = LogTemplate.EditText;
                            items = new CU_LogService().GetEditItems(objLog.Json, objLog.Page_URL);
                            break;

                        case (int)ActionType.Password:
                            strClassIcon = "fa fa-lock";
                            strActionText = LogTemplate.PasswordText;
                            strActionDetails = objLog.Json;
                            break;
                        case (int)ActionType.Delete:
                            strClassIcon = "fa fa-eraser";
                            strActionText = LogTemplate.DeleteText;
                            strActionDetails = objLog.Json;
                            break;

                        case (int)ActionType.Logout:
                            strClassIcon = "fa fa-sign-out";
                            strActionText = LogTemplate.LogOutText;
                            break;

                        case (int)ActionType.Login:
                            strClassIcon = "fa fa-sign-in";
                            strActionText = LogTemplate.LoginText;
                            strActionDetails = objLog.Json;
                            break;

                        default:
                            break;
                    }
                    #endregion


                    #region logDetails
                    //log record details redirection
                 
                    strlogDetails += strActionText;

                    if ((objLog.IdAction == (int)ActionType.Insert || objLog.IdAction == (int)ActionType.Update) && items.Keys.FirstOrDefault()[0] != null)
                    {

                        HandleLogRecordsLinks(objLog.Page_URL, objLog.Action, out strEitPossibleLink, out strDefaultPossibleLink);

                        strPageLink = LogTemplate.PageLink.Replace(LogTemplate.PageName, objLog.PageName).Replace(LogTemplate.PageURL, strDefaultPossibleLink);
                     //   HandleLogRecordsLinks(objLog.Page_URL, objLog.Action, out strEitPossibleLink, out strDefaultPossibleLink);

                        strAddedEditedPageLnk = strEitPossibleLink;//+ "?Id=" + items.Keys.FirstOrDefault()[0];
                        strAddEditObjName = items.Keys.FirstOrDefault()[1];
                        //.Replace("@PageLink", strPageLink).
                        strAddedEditedObjLnk = LogTemplate.AddedEditedObjLnk.Replace(LogTemplate.AddEditPageLink, strAddedEditedPageLnk).Replace(LogTemplate.AddEditObjName, strAddEditObjName);

                        //append to logDetails
                        strlogDetails += strAddedEditedObjLnk;

                        //fields details
                        strActionDetails = "<ul>";
                        foreach (string item in items.Values.FirstOrDefault())
                        {
                            strActionDetails += "<li>" + item + "</li>";
                        }
                        strActionDetails += "</ul>";

                    }
                    else
                    {
                       // HandleLogRecordsLinks("", objLog.Action, out strEitPossibleLink, out strDefaultPossibleLink);
                        HandleLogRecordsLinks("", objLog.Action, out strEitPossibleLink, out strDefaultPossibleLink);

                        strPageLink = LogTemplate.PageLink.Replace(LogTemplate.PageName, objLog.PageName).Replace(LogTemplate.PageURL, strDefaultPossibleLink);

                        if (objLog.IdAction == (int)ActionType.Delete)
                        {
                            strlogDetails += strActionDetails;
                        }
                        //page default link
                        strlogDetails += strPageLink;

                    }



                    if (objLog.IdAction != (int)ActionType.Delete)
                    {
                        strlogDetails += strActionDetails;
                    }
                    #endregion

                    string template = LogTemplate.LogRecordItem.Replace(LogTemplate.logID, objLog.LogID.ToString())
                    .Replace(LogTemplate.Action, objLog.ActionName.ToString())
                    .Replace(LogTemplate.ClassIcon, strClassIcon.ToString())
                    .Replace(LogTemplate.IpAddress, objLog.IpAddress)
                    .Replace(LogTemplate.logDate, objLog.ArabicDateTime.ToString())
                    .Replace(LogTemplate.HourAction, objLog.ActionTime.ToString("HH"))
                    .Replace(LogTemplate.MinAction, objLog.ActionTime.ToString("mm"))
                    .Replace(LogTemplate.UserName, objLog.EmployeeName)
                    .Replace("@logDetails", strlogDetails);
                    ////Add Log Tempalte to All Tempaltes 
                    LogssTempaltes += template;

                }
            }
            else
            {
                LogssTempaltes = LogTemplate.NotFoundLog;
            }
            return LogssTempaltes;
        }





        public static void HandleLogRecordsLinks(string strControllerName,string strActionName, out string strEitPossibleLink, out string strDefaultPossibleLink)
        {
            strEitPossibleLink = string.Empty;
            strDefaultPossibleLink = string.Empty;
           if(strActionName != "")
           {
               strEitPossibleLink =strActionName ;//strControllerName == "" ?strActionName : "/ControlPanel/" + strControllerName + "/" + strActionName;
               strDefaultPossibleLink = strActionName;//strActionName == "" ? strActionName : "/ControlPanel/" + strControllerName + "/" + strActionName;
           }
           else
           {
               strEitPossibleLink =  "/ControlPanel/Log/Default"; //:strActionName;
               strDefaultPossibleLink =  "/ControlPanel/Log/Default" ;//: "/ControlPanel/" + strControllerName + "/" + strActionName;
           }
          
               // }

           // }
            if (string.IsNullOrEmpty(strEitPossibleLink))
            {
                strEitPossibleLink = strDefaultPossibleLink;
            }
        }
    }


}