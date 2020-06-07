using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using QvLib.QVUtil;
using Repository;
using GenericEngine.Service.SharedService;
using System.Globalization;
using MobileApplication.Context;
using MobileApplication.DataModel;
using DataModel.Enum;



namespace MobileApplication.DataService
{
    public class CU_LogService :BaseService, ILogService
    {
        #region Memebr Data
        //  private readonly UnitOfWork _unitOfWork;
        private readonly Repository<CU_Log> _CU_LogServiceRepository;
        private CU_PageService pageServiceObj;
        private CU_Page pageObj;
        private int? PageID = null;
        //private string ActionName;
        private string PageName;

        private bool IsMobile;

        public IEnumerable<CU_Log> CU_LogServiceList
        {
            get { return _CU_LogServiceRepository.GetList(); }
        }
        public CU_Log CU_LogServiceByID(int id)
        {
              return _CU_LogServiceRepository.GetList().Where(x=>x.LogID==id).FirstOrDefault();
        }
        #endregion

        #region Constructors

        public CU_LogService()
        {

            //  _unitOfWork = new UnitOfWork(new QVControlPanelEntities());
            _CU_LogServiceRepository = new Repository<CU_Log>(_unitOfWork);
            pageServiceObj = new CU_PageService();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                //string strControllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                string strAreaName = Convert.ToString(HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"]);
                if (strAreaName == "API")//mobie request
                {
                    //IsMobile = true;
                    //PageID = int.Parse(HttpContext.Current.Request.QueryString["LogPageID"].ToString());
                    //pageObj = pageServiceObj.GetCU_PageById(PageID.Value);
                    //PageName = pageObj == null ? PageName : pageObj.URL;
                }
                else// web request
                {
                    IsMobile = false;
                    PageName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                    pageObj = pageServiceObj.GetCU_PageByPageURL(PageName);
                    PageID = pageObj == null ? PageID : pageObj.ID;
                }
                if (PageID == null)
                {
                    var page = pageServiceObj.GetCU_PageByPageURLEvenIfDeleted(PageName);
                    PageID = page == null ? PageID : page.ID;
                }
            }
        }
        #endregion

        #region DAL

        public LogPattren SelectById(int LogID)
        {
            LogPattren pattren = null;

            var record = CU_LogServiceList.Where(i => i.LogID == LogID).FirstOrDefault();
            if (record != null)
            {
                pattren = new LogPattren();
                pattren.LogID = record.LogID;
                pattren.ActionTime = record.ActionTime;
                pattren.IdAction = record.IdAction;
                pattren.IdUser = record.IdEmployee;
                pattren.IdPage = record.IdPage;
                pattren.Value = record.Value;
                pattren.IpAddress = record.IpAddress;
                pattren.Action = record.CU_Action.Name;
            }

            return pattren;
        }


        public IEnumerable<LogRecord> Select(int skip, int take, DateTime? from, DateTime? to, int iPageID, int iActionID, string strEmpName, int iUserId, bool bIsAdmin, out int total)
        {
            List<LogRecord> ret = new List<LogRecord>();

            var data = CU_LogServiceList.Where(i =>
                 (bIsAdmin || i.IdEmployee == iUserId)
                && (strEmpName == string.Empty || i.CU_Employee.Name.ToLower().Contains(strEmpName.ToLower()))
                   && (iActionID == 0 || i.IdAction == iActionID)
                   && (iPageID == 0 || i.IdPage == iPageID));
            if (from.HasValue)
                data = data.Where(i => i.ActionTime.Date >= from.Value.Date);
            if (to.HasValue)
                data = data.Where(i => i.ActionTime.Date <= to.Value.Date);

            total = data.Count();
            data.OrderByDescending(i => i.ActionTime).Skip(skip).Take(take).ToList().ForEach(delegate(CU_Log i)
            {
                ret.Add(new LogRecord()
                {
                    ActionName = i.CU_Action.Name,
                    ArabicDateTime = i.ActionTime.ToHijriArabicDate(), //i.ActionTime.ToGregorianArabicDate()
                    EmployeeName = i.CU_Employee.Name,
                    Json = i.Value,
                    LogID = i.LogID,
                    PageName = i.IdPage.HasValue ? i.CU_Page.Name : "",
                    IdAction = i.IdAction,
                    ActionTime = i.ActionTime,
                    IdUser = i.IdEmployee,
                    IdPage = i.IdPage.HasValue ? i.IdPage.Value : 0,
                    Page_URL = i.IdPage.HasValue ? i.CU_Page.URL : "",
                    IpAddress = i.IpAddress,
                    //IsMobile = i.IsMobile,
                    Action = i.ActionName
                });
            });


            return ret.AsEnumerable();
        }


        public int SaveCU_Log(LogPattren logObj)
        {
            CU_Log obj = new CU_Log()
            {
                IdEmployee = logObj.IdUser,
                IdAction = logObj.IdAction,
                ActionTime = logObj.ActionTime,
                IdPage = logObj.IdPage,
                Value = logObj.Value,
                IpAddress = logObj.IpAddress,
                //IsMobile = logObj.IsMobile,
                ActionName = logObj.Action
            };
            _CU_LogServiceRepository.Insert(obj);
            _unitOfWork.Submit();
            return obj.LogID;
        }

        public bool UpdateJson(int LogID, string ValueOfJSon)
        {
            bool ret = false;

            CU_Log obj = CU_LogServiceList.Where(i => i.LogID == LogID).FirstOrDefault();
            obj.Value = ValueOfJSon;
            _CU_LogServiceRepository.Save(obj);
            _unitOfWork.Submit();
            ret = true;

            return ret;
        }
        #endregion

        #region BLL Actions  Operations
        /// <summary>
        /// update for many to many relations
        /// </summary>
        /// <param name="old_value"></param>
        /// <param name="new_value"></param>
        public int? Update(int? iParentID, int ID, string strItemText, List<string> LstDeleted, Dictionary<string, string> LsUpdate, List<string> LstInserted, string strLocalResourceKey, int? UserId, string strLogPath = "")
        {
            int? iLogID = iParentID;
            LogPattren logRecord = null;
            if (iParentID.HasValue && iParentID.Value != 0)
            {

                logRecord = SelectById(iParentID.Value);
            }
            //get field name
            string field_name = "";
            field_name = HttpContext.GetGlobalResourceObject("ManyToManyResource", strLocalResourceKey).ToString();

            //generate class object of inserted and deleted items
            List<ManyToManyItem> items = new List<ManyToManyItem>();

            //if ((lstDeleted != null&&lstDeleted.Count!=0)&& LstInserted != null) // update case
            //{
            //    for (int i = 0; i < lstDeleted.Count(); i++)
            //        items.Add(new ManyToManyItem() { text = field_name, Type = ActionType.Update, value = LstInserted[i], old_value = lstDeleted[i] });
            //}

            //else
            //{
            if (LstDeleted != null)
                LstDeleted.ForEach(p => items.Add(new ManyToManyItem() { text = field_name, Type = ActionType.Delete, value = p }));
            if (LsUpdate != null)
                LsUpdate.ToList().ForEach(p => items.Add(new ManyToManyItem() { text = field_name, Type = ActionType.Update, value = p.Value, old_value = p.Key }));
            if (LstInserted != null)
                LstInserted.ForEach(p => items.Add(new ManyToManyItem() { text = field_name, Type = ActionType.Insert, value = p }));
            // }

            if (logRecord == null)
            {
                LogPattren logObj = new LogPattren();
                logObj.ActionTime = DateTime.UtcNow;
                logObj.IdUser = (int)UserId;  // ((UserData)HttpContext.Current.Session["User"]).userId; need handled----------------->
                logObj.IdAction = (int)ActionType.Update;
                logObj.IdPage = PageID;
                //generate LogEdit
                LogEdit logEditObj = new LogEdit();
                logEditObj.ID = ID.ToString();
                logEditObj.itemText = strItemText;
                logEditObj.ManyToManyItems = items;
                logObj.Value = new JavaScriptSerializer().Serialize(logEditObj);

                logObj.IpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                logObj.IsMobile = IsMobile;
                logObj.Action = strLogPath;

                //  logObj.ActionName=ActionName;
                iLogID = new CU_LogService().SaveCU_Log(logObj);

            }
            else
            {
                LogEdit logEditObj = new JavaScriptSerializer().Deserialize<LogEdit>(logRecord.Value);
                if (logEditObj.ManyToManyItems == null)
                    logEditObj.ManyToManyItems = items;
                else
                    logEditObj.ManyToManyItems = logEditObj.ManyToManyItems.Concat(items).ToList();

                logRecord.Value = new JavaScriptSerializer().Serialize(logEditObj);


                UpdateJson(logRecord.LogID, logRecord.Value);

            }
            return iLogID;

        }
        public int? Update(int? iParentID, int ID, string strTitle, object old_object, object new_object, int? UserId, string ActionName)
        {
            return 1;
        }
        /// <summary>
        /// The Login Action
        /// </summary>
        public void Login(int? UserId)
        {
            //get client ip address
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //save the log action and ip address
            New_Log(ActionType.Login, ip, "", UserId);
        }

        public void LogOut(int? UserId)
        {
            //get client ip address
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //save the log action and ip address
            New_Log(ActionType.Logout, "", ip, UserId);
        }

        public void ChangePassword(int? iUserID = 0)
        {
            //get client ip address

            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //save the log action and ip address
            New_Log(ActionType.Password, "", ip, iUserID);
        }
        /// <summary>
        /// The Read Action 
        /// </summary>
        public void Read(string ActionName, int? UserId)
        {
            New_Log(ActionType.View, String.Empty, ActionName, UserId);
        }

        /// <summary>
        /// The Delete Action
        /// </summary>
        /// <param name="strTitle">The Title of item Deleted</param>
        public void Delete(string strTitle, string ActionName, int? UserId)
        {
            New_Log(ActionType.Delete, strTitle, ActionName, UserId);
        }


        public int? InsertCopy(int iIdentity, string strTitle, object InsertedObject, int? UserId, string ActionName)
        {


            //the list to save json
            List<string> JsonList = new List<string>();
            JsonList = InsertToLogCopy(InsertedObject);
            //if there're fields had changed insert new log for updated items.
            if (JsonList != null && JsonList.Count() > 0)
                return New_Log(ActionType.Insert, "{'ID':'" + iIdentity + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonList.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else
                return null;
        }


        /// <summary>
        /// The Insert Action
        /// </summary>
        /// <param name="iIdentity">The ID of item inserted</param>
        /// <param name="strTitle">The title of item inserted</param>
        /// <param name="InsertedObject"></param>
        public int? Insert(int iIdentity, string strTitle, object InsertedObject, int? UserId, string ActionName)
        {

            
            //the list to save json
            List<string> JsonList = new List<string>();
            JsonList = InsertToLog(InsertedObject);
            //if there're fields had changed insert new log for updated items.
            if (JsonList != null && JsonList.Count() > 0)
                return New_Log(ActionType.Insert, "{'ID':'" + iIdentity + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonList.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else
                return null;
        }

        /// <summary>
        /// The Update Action
        /// </summary>
        /// <param name="ID">The object updated identity</param>
        /// <param name="strTitle">The object updated title.(old title)</param>
        /// <param name="old_object">the old object</param>
        /// <param name="new_object">the new object</param>
        public int? Update(int ID, string strTitle, object old_object, object new_object, int? UserId, string ActionName)
        {

            List<string> JsonList = new List<string>();

            JsonList = UpdateLog(old_object, new_object);
            //if there're fields had changed insert new log for updated items.
            if (JsonList != null && JsonList.Count() > 0)
                return New_Log(ActionType.Update, "{'ID':'" + ID + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonList.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else
                return null;
        }


        public List<string> InsertToLogCopy(object InsertedObject)
        {
            var db = new QVMobileApplicationEntities();
            db.Database.Connection.Open();
            ////get the table type
            Type type = InsertedObject.GetType();

            //get table references prefernces
            IList<TableRef_Result> references = db.TableRef(HttpContext.GetGlobalResourceObject("TableResource", type.Name).ToString()).ToList();
            //IList<TableRef_Result> references = db.TableRef(type.Name.Replace("Model", " ").Trim()).ToList();

            //HttpContext.GetGlobalResourceObject("TableResource", type.Name).ToString()
            //get the table properties(fields)
            PropertyInfo[] Properties = type.GetProperties();

            //the list to save json
            List<string> JsonList = new List<string>();

            //fetch the properties of the table
            foreach (PropertyInfo property in Properties)
            {
                if (property.PropertyType.Name.ToLower().Contains("entityref") || property.PropertyType.Name.ToLower().Contains("entityset"))
                    continue;

                ///for navigation properties
                if (property.PropertyType.Name.ToLower().Contains("icollection"))
                    continue;

                var column = property.GetCustomAttributes(false)
                                .Where(x => x.GetType() == typeof(ColumnAttribute))
                                .FirstOrDefault(x =>
                                 ((ColumnAttribute)x).IsPrimaryKey &&
                                 ((ColumnAttribute)x).DbType.Contains("NOT NULL"));
                if (column != null)
                    continue;

                //get the current property value
                object value = property.GetValue(InsertedObject, null);
                value = value ?? String.Empty;
                if (value.Equals(String.Empty))
                    continue;

                ////if is datetime convert to arabic text
                //if (value.GetType() == typeof(DateTime))
                //{
                //    value = Convert.ToDateTime(value).ToGregorianArabicDate();// Date.GregToHijri(Convert.ToDateTime(value));

                //}
                //else 


                if (value.GetType() == typeof(bool))
                {
                    if (Convert.ToString(value) == "True")
                        value = DataServiceArabicResource.Yes;
                    else if (Convert.ToString(value) == "False")
                        value = DataServiceArabicResource.No;
                }
                

                //detect if it's a lookup value from another table
                TableRef_Result feild = references.Where(i => i.FK_COLUMN_NAME == property.Name).FirstOrDefault();
                if (feild != null) // if table have forien keys
                {
                    Type feildtype = feild.GetType();

                    //get table references prefernces
                    //get the table properties(fields)
                    PropertyInfo[] feildProperties = feildtype.GetProperties();
                    //foreach (PropertyInfo fproperty in feildProperties)
                    //{
                    //    var st = InnerLog(fproperty, feild);
                    //    JsonList.Add(st);
                    //}
                    if (db.Database.Connection.State == ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    if (value.GetType() == typeof(int))
                    {
                        if (Convert.ToString(value) == "0")
                            value = "";

                    }
                    using (var cmd = db.Database.Connection.CreateCommand()) //open DbCommand
                    {
                        if (value != "" )
                        {
                            //sql statement to select the row of refrenced table which equal the old value
                            cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + value;
                            //execute the sql statement and save it in datareader
                            DbDataReader reader = cmd.ExecuteReader();
                            reader.Read(); //read first row
                            //get the second field whitch usually be the Name field and save it as the value of the old_value
                            int index = 0;
                            try { index = reader.GetOrdinal("Name"); }
                            catch { }
                             if (index != 0)
                                 value = reader[index];
                                 //reader.GetOrdinal("ID");
                            
                            reader.Close();
                        }
                    }
                }
                try
                {
                    //get the controller name to get the local resource file object
                    //controller name after area name
                    // string vitural_path = HttpContext.Current.Request.Url.AbsolutePath.Split('/')[2];
                    string field_name = HttpContext.GetGlobalResourceObject(PageName + "Resource", property.Name).ToString();
                    //add the row updated to json list
                    JsonList.Add("{'columnName':'" + property.Name + "','text':'" + field_name + "','old_value':'','new_value':'" + value + "'}");
                }
                catch (Exception) { }
            }


            //if there're fields had changed insert new log for updated items.
            if (JsonList.Count() > 0)
                return JsonList;
            //return New_Log(ActionType.Insert, "{'ID':'" + iIdentity + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonList.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else
                return null;
        }

        public List<string> InsertToLog(object InsertedObject)
        {
            var db = new QVMobileApplicationEntities();
            db.Database.Connection.Open();
            ////get the table type
            //hadeer hassan updated from next to line number 498
            Type type = InsertedObject.GetType();
            int place = type.Name.LastIndexOf("Model");
            string table = type.Name.Remove(place, ("Model").Length).Insert(place, "");
            IList<TableRef_Result> references = db.TableRef(table).ToList();
            //get table references prefernces
            // IList<TableRef_Result> references = db.TableRef(type.Name.Replace("Model"," ").Trim()).ToList();
            //get the table properties(fields)
            PropertyInfo[] Properties = type.GetProperties();

            //the list to save json
            List<string> JsonList = new List<string>();

            //fetch the properties of the table
            foreach (PropertyInfo property in Properties)
            {
                if (property.PropertyType.Name.ToLower().Contains("entityref") || property.PropertyType.Name.ToLower().Contains("entityset"))
                    continue;

                ///for navigation properties
                if (property.PropertyType.Name.ToLower().Contains("icollection"))
                    continue;

                var column = property.GetCustomAttributes(false)
                                .Where(x => x.GetType() == typeof(ColumnAttribute))
                                .FirstOrDefault(x =>
                                 ((ColumnAttribute)x).IsPrimaryKey &&
                                 ((ColumnAttribute)x).DbType.Contains("NOT NULL"));
                if (column != null)
                    continue;

                //get the current property value
                object value = property.GetValue(InsertedObject, null);
                value = value ?? String.Empty;
                if (value.Equals(String.Empty))
                    continue;

                ////if is datetime convert to arabic text
                //if (value.GetType() == typeof(DateTime))
                //{
                //    value = Convert.ToDateTime(value).ToGregorianArabicDate();// Date.GregToHijri(Convert.ToDateTime(value));

                //}
                //else 


                if (value.GetType() == typeof(bool))
                {
                    if (Convert.ToString(value) == "True")
                        value = DataServiceArabicResource.Yes;
                    else if (Convert.ToString(value) == "False")
                        value = DataServiceArabicResource.No;
                }

                //detect if it's a lookup value from another table
                TableRef_Result feild = references.Where(i => i.FK_COLUMN_NAME == property.Name).FirstOrDefault();
                if (feild != null) // if table have forien keys
                {
                    Type feildtype = feild.GetType();

                    //get table references prefernces
                    //get the table properties(fields)
                    PropertyInfo[] feildProperties = feildtype.GetProperties();
                    //foreach (PropertyInfo fproperty in feildProperties)
                    //{
                    //    var st = InnerLog(fproperty, feild);
                    //    JsonList.Add(st);
                    //}
                    if (db.Database.Connection.State == ConnectionState.Closed)
                    {
                        db.Database.Connection.Open();
                    }
                    using (var cmd = db.Database.Connection.CreateCommand()) //open DbCommand
                    {
                        if (value != "")
                        {
                            //sql statement to select the row of refrenced table which equal the old value
                            cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + value;
                            //execute the sql statement and save it in datareader
                            DbDataReader reader = cmd.ExecuteReader();
                            reader.Read(); //read first row
                            //get the second field whitch usually be the Name field and save it as the value of the old_value
                            int index = 0;
                            try { index = reader.GetOrdinal("Name"); }
                            catch { }
                            // if (index == 0) reader.GetOrdinal("ID");
                            value = reader[index];
                            reader.Close();
                        }
                    }
                }
                try
                {
                    //get the controller name to get the local resource file object
                    //controller name after area name
                    // string vitural_path = HttpContext.Current.Request.Url.AbsolutePath.Split('/')[2];
                    string field_name = HttpContext.GetGlobalResourceObject(PageName + "Resource", property.Name).ToString();
                    //add the row updated to json list
                    JsonList.Add("{'columnName':'" + property.Name + "','text':'" + field_name + "','old_value':'','new_value':'" + value + "'}");
                }
                catch (Exception) { }
            }


            //if there're fields had changed insert new log for updated items.
            if (JsonList.Count() > 0)
                return JsonList;
            //return New_Log(ActionType.Insert, "{'ID':'" + iIdentity + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonList.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else
                return null;
        }


        public List<string> UpdateLog(object old_object, object new_object)
        {
            var db = new QVMobileApplicationEntities();
            //open connection to use it in ADO call in Linq
            db.Database.Connection.Open();
            //get the table type
            //            Type type = old_object.GetType();

            Type type = old_object.GetType();
            //Type type = old_object.GetType().BaseType;


            //get table references prefernces
            IList<TableRef_Result> references = db.TableRef(type.Name).ToList();

            //get the table properties(fields)
            PropertyInfo[] Properties = type.GetProperties();

            //the list to save json
            List<string> JsonList = new List<string>();

            //fetch the properties of the table
            foreach (PropertyInfo property in Properties)
            {

               

                //get the old value of property
                object old_value = property.GetValue(old_object, null);
                object new_value = old_value;
                //get the new value of property
                try
                {
                    new_value = property.GetValue(new_object, null);
                }
                catch (Exception ex)
                {
                    new_value = old_value;
                }
                //detect if not null and not empty and type is datetime convert to text arabic date format -----
                if (old_value != null && old_value.ToString() != "")
                {
                    //if is datetime convert to arabic text
                    //if (old_value.GetType() == typeof(DateTime))
                    //    old_value = Convert.ToDateTime(old_value).ToGregorianArabicDate();//ToHijriArabicDate();
                    if (old_value.GetType() == typeof(bool))
                    {
                        if (Convert.ToString(old_value) == "True")
                            old_value = DataServiceArabicResource.Yes;
                        else if (Convert.ToString(old_value) == "False")
                            old_value = DataServiceArabicResource.No;
                    }
                }

                if (new_value != null && new_value.ToString() != "")
                {
                    //if is datetime convert to arabic text
                    //if (new_value.GetType() == typeof(DateTime))
                    //    new_value = Convert.ToDateTime(new_value).ToGregorianArabicDate();//ToHijriArabicDate();
                    if (new_value.GetType() == typeof(bool))
                    {
                        if (Convert.ToString(new_value) == "True")
                            new_value = DataServiceArabicResource.Yes;
                        else if (Convert.ToString(new_value) == "False")  
                            new_value = DataServiceArabicResource.No;
                    }
                }
                //-------------------------------

                //to prevent null exception
                old_value = old_value ?? "";
                new_value = new_value ?? "";

                //detect if value changed
                if (!old_value.Equals(new_value))
                {
                    //detect if it's a lookup value from another table
                    TableRef_Result feild = references.Where(i => i.FK_COLUMN_NAME == property.Name).FirstOrDefault();
                    if (feild != null) // if table have forien keys
                    {
                        if (db.Database.Connection.State == ConnectionState.Closed)
                        {
                            db.Database.Connection.Open();
                        }
                        using (var cmd = db.Database.Connection.CreateCommand()) //open DbCommand
                        {
                            if (old_value != "")
                            {
                                //sql statement to select the row of refrenced table which equal the old value
                                cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + old_value;
                                //execute the sql statement and save it in datareader
                                DbDataReader reader = cmd.ExecuteReader();
                                reader.Read(); //read first row
                                //get the second field whitch usually be the Name field and save it as the value of the old_value
                                old_value = reader[1];
                                reader.Close();
                            }
                            if (new_value != "")
                            {
                                //sql statement to select the row of refrenced table which equal the new value
                                cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + new_value;
                                //execute the sql statement and save it in datareader for new value
                                DbDataReader reader = cmd.ExecuteReader();
                                reader.Read();
                                //get the second field whitch usually be the Name field and save it as the value of the new_value
                                new_value = reader[1];
                                reader.Close();
                            }
                        }
                    }
                    //get the vitural path to get the local resource file object

                    string field_name = String.Empty;
                    try
                    {
                        //controller name after area name
                        // string vitural_path = HttpContext.Current.Request.Url.AbsolutePath.Split('/')[2];
                        field_name = HttpContext.GetGlobalResourceObject(PageName + "Resource", property.Name).ToString();
                    }
                    catch (Exception e)
                    { }
                    //add the row updated to json list
                    if (!string.IsNullOrEmpty(field_name))
                        JsonList.Add("{'columnName':'" + property.Name + "','text':'" + field_name + "','old_value':'" + old_value + "','new_value':'" + new_value + "'}");
                }
            }
            if (JsonList.Count() > 0)
                return JsonList;
            else
                return null;
        }
        //public List<string> UpdateLog(object old_object, object new_object)
        //{
        //    var db = new QVControlPanelEntities();
        //    //open connection to use it in ADO call in Linq
        //    db.Database.Connection.Open();
        //    //get the table type
        //    //            Type type = old_object.GetType();

        //    Type type = old_object.GetType();
        //    //Type type = old_object.GetType().BaseType;


        //    //get table references prefernces
        //    IList<TableRef_Result> references = db.TableRef(type.Name).ToList();

        //    //get the table properties(fields)
        //    PropertyInfo[] Properties = type.GetProperties();

        //    //the list to save json
        //    List<string> JsonList = new List<string>();

        //    //fetch the properties of the table
        //    foreach (PropertyInfo property in Properties)
        //    {
        //        //get the old value of property
        //        object old_value = property.GetValue(old_object, null);
        //        //get the new value of property
        //        object new_value = property.GetValue(new_object, null);

        //        //detect if not null and not empty and type is datetime convert to text arabic date format -----
        //        if (old_value != null && old_value.ToString() != "")
        //        {
        //            //if is datetime convert to arabic text
        //            //if (old_value.GetType() == typeof(DateTime))
        //            //    old_value = Convert.ToDateTime(old_value).ToGregorianArabicDate();//ToHijriArabicDate();
        //            if (old_value.GetType() == typeof(bool))
        //            {
        //                if (Convert.ToString(old_value) == "True")
        //                    old_value = "نعم";
        //                else if (Convert.ToString(old_value) == "False")
        //                    old_value = "لا";
        //            }
        //        }

        //        if (new_value != null && new_value.ToString() != "")
        //        {
        //            //if is datetime convert to arabic text
        //            //if (new_value.GetType() == typeof(DateTime))
        //            //    new_value = Convert.ToDateTime(new_value).ToGregorianArabicDate();//ToHijriArabicDate();
        //            if (new_value.GetType() == typeof(bool))
        //            {
        //                if (Convert.ToString(new_value) == "True")
        //                    new_value = "نعم";
        //                else if (Convert.ToString(new_value) == "False")
        //                    new_value = "لا";
        //            }
        //        }
        //        //-------------------------------

        //        //to prevent null exception
        //        old_value = old_value ?? "";
        //        new_value = new_value ?? "";

        //        //detect if value changed
        //        if (!old_value.Equals(new_value))
        //        {
        //            //detect if it's a lookup value from another table
        //            TableRef_Result feild = references.Where(i => i.FK_COLUMN_NAME == property.Name).FirstOrDefault();
        //            if (feild != null) // if table have forien keys
        //            {
        //                if (db.Database.Connection.State == ConnectionState.Closed)
        //                {
        //                    db.Database.Connection.Open();
        //                }
        //                using (var cmd = db.Database.Connection.CreateCommand()) //open DbCommand
        //                {
        //                    if (old_value != "")
        //                    {
        //                        //sql statement to select the row of refrenced table which equal the old value
        //                        cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + old_value;
        //                        //execute the sql statement and save it in datareader
        //                        DbDataReader reader = cmd.ExecuteReader();
        //                        reader.Read(); //read first row
        //                        //get the second field whitch usually be the Name field and save it as the value of the old_value
        //                        old_value = reader[1];
        //                        reader.Close();
        //                    }
        //                    if (new_value != "")
        //                    {
        //                        //sql statement to select the row of refrenced table which equal the new value
        //                        cmd.CommandText = "SELECT * FROM [" + feild.REFERENCED_TABLE_NAME + "] WHERE " + feild.REFERENCED_COLUMN_NAME + " = " + new_value;
        //                        //execute the sql statement and save it in datareader for new value
        //                        DbDataReader reader = cmd.ExecuteReader();
        //                        reader.Read();
        //                        //get the second field whitch usually be the Name field and save it as the value of the new_value
        //                        new_value = reader[1];
        //                        reader.Close();
        //                    }
        //                }
        //            }
        //            //get the vitural path to get the local resource file object

        //            string field_name = String.Empty;
        //            try
        //            {
        //                //controller name after area name
        //                // string vitural_path = HttpContext.Current.Request.Url.AbsolutePath.Split('/')[2];
        //                field_name = HttpContext.GetGlobalResourceObject(PageName + "Resource", property.Name).ToString();
        //            }
        //            catch (Exception e)
        //            { }
        //            //add the row updated to json list
        //            if (!string.IsNullOrEmpty(field_name))
        //                JsonList.Add("{'columnName':'" + property.Name + "','text':'" + field_name + "','old_value':'" + old_value + "','new_value':'" + new_value + "'}");
        //        }
        //    }
        //    if (JsonList.Count() > 0)
        //        return JsonList;
        //    else
        //        return null;
        //}

        public int? InsertMulti(int iIdentity, string strTitle, List<string> JsonLists, string ActionName, int UserId)
        {
            if (JsonLists != null)

                return New_Log(ActionType.Insert, "{'ID':'" + iIdentity + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonLists.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else return null;
        }
        public int? UpdateMulti(int ID, string strTitle, List<string> JsonLists, string ActionName, int? UserId)
        {


            //if there're fields had changed insert new log for updated items.
            if (JsonLists != null)
                return New_Log(ActionType.Update, "{'ID':'" + ID + "','itemText':'" + strTitle + "','updates':[" + String.Join(",", JsonLists.ToArray()) + "]}", ActionName, UserId);//save old and new values of table updated
            else return null;
        }

        /// <summary>
        /// Insert New Log
        /// </summary>
        /// <param name="type">The ActionType Enum</param>
        /// <param name="JSON">The json or text to insert</param>
        private int New_Log(ActionType type, string JSON, string ActionName, int? iUserID = 0)
        {
            int iLogID = 0;

            LogPattren logObj = new LogPattren();
            //get the action type id form enum ActionType value
            logObj.IdAction = (int)(type); //get the enum value
            logObj.ActionTime = DateTime.UtcNow;
            logObj.IdUser = iUserID.Value;
            //assign json value
            logObj.Value = JSON;
            ////no page identity for login action
            if (type != ActionType.Login && type != ActionType.Logout && type != ActionType.Password)// && type != ActionType.Password 
                logObj.IdPage = PageID;

            logObj.IpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            logObj.IsMobile = IsMobile;
            logObj.Action = ActionName;
            iLogID = SaveCU_Log(logObj);

            return iLogID;
        }
        #endregion


        #region Json Deserialization
        public Dictionary<string[], List<string>> GetEditItems(string JSON, string strSectionPath)
        {
            strSectionPath = strSectionPath.Replace("Default.aspx", "").ToLower();
            List<string> updates = new List<string>();
            LogEdit data = new LogEdit();
            //data = new JavaScriptSerializer().Deserialize<LogEdit>(JSON);
            try
            {
                data = new JavaScriptSerializer().Deserialize<LogEdit>(JSON);
            }
            catch (Exception) { }

            if (data.updates != null)
            {
                foreach (LogEditItem item in data.updates)
                {
                    if (item.columnName.ToLower().Contains("path"))
                    {
                        if (item.old_value.Trim() != "")
                        {
                            if (isImage(item.old_value))
                                item.old_value = "<a type=\"pic\"  href=\"/" + "/" + item.old_value + "\">"+DataServiceArabicResource.ImageWidth+"</a>";
                            else
                                item.old_value = "<a target=\"_blank\" type=\"file\" href=\"" + item.old_value + "\">"+DataServiceArabicResource.FileLoad + "</a>";
                        }
                        else
                        {
                            item.old_value = "";
                        }
                        if (item.new_value.Trim() != "")
                        {

                            if (isImage(item.new_value))
                                item.new_value = "<a type=\"pic\"  href=\"/" + "/" + item.new_value + "\">"+DataServiceArabicResource.ImageWidth+"</a>";
                            else
                                item.new_value = "<a target=\"_blank\" type=\"file\" href=\"" + item.new_value + "\">"+DataServiceArabicResource.FileLoad+"</a>";
                        }
                        else
                        {
                            item.new_value = DataServiceArabicResource.NoFileExist;
                        }
                    }
                    else if (item.columnName.ToLower().Contains("password"))
                    {
                        item.old_value = "********";//QvLib.Security.DataProtection.Decrypt(item.old_value);
                        item.new_value = "********";//QvLib.Security.DataProtection.Decrypt(item.new_value);
                    }
                    else if (item.columnName.ToLower().Contains("date"))
                    {
                        //    contract_Extention
                        //This need to be repaired

                        item.new_value = item.new_value.Replace(DataServiceArabicResource.Morning, "AM");
                        item.new_value = item.new_value.Replace(DataServiceArabicResource.Evining, "PM");
                        item.new_value = DateTime.ParseExact(item.new_value, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).ToHijriArabicDate();
                        if (item.old_value != null && item.old_value != "")
                            item.old_value = Convert.ToDateTime(item.old_value).ToHijriArabicDate();
                    }
                    updates.Add( DataServiceArabicResource.HasDesignation+ " <span class='def_color'>" + item.text + "</span> " + DataServiceArabicResource.From + " \"" + item.old_value + "\"" + DataServiceArabicResource.To+ " \"" + item.new_value + "\"");
                }
            }

            if (data.ManyToManyItems != null)
            {
                foreach (ManyToManyItem item in data.ManyToManyItems)
                {
                    if (item.Type == ActionType.Insert)
                        updates.Add(DataServiceArabicResource.HasAdded+"<span class='def_color'>" + item.value + "</span> "+DataServiceArabicResource.In+" <span class='def_color'>" + item.text + "</span>");
                    else if (item.Type == ActionType.Delete)
                        updates.Add(DataServiceArabicResource.HasDeleted+"<span class='def_color'>" + item.value + "</span> "+DataServiceArabicResource.From+"<span class='def_color'>" + item.text + "</span>");
                    else if (item.Type == ActionType.Update)
                        updates.Add(DataServiceArabicResource.HasDesignation+ "<span class='def_color'>" + item.text + "</span> " + DataServiceArabicResource.From + "" + " \"" + item.old_value + "\"" + DataServiceArabicResource.To + " \"" + item.value + "\"");

                }
            }
            Dictionary<string[], List<string>> Return = new Dictionary<string[], List<string>>();
            Return.Add(new string[] { data.ID, data.itemText }, updates);
            return Return;
        }
        public Dictionary<string[], List<string>> GetInsertItems(string JSON, string strSectionPath)
        {
            strSectionPath = strSectionPath.Replace("Default.aspx", "").ToLower();
            List<string> updates = new List<string>();
            LogEdit data = new LogEdit();
            //data = new JavaScriptSerializer().Deserialize<LogEdit>(JSON);
            try
            {
                data = new JavaScriptSerializer().Deserialize<LogEdit>(JSON);
            }
            catch (Exception) { }

            if (data.updates != null)
            {
                foreach (LogEditItem item in data.updates)
                {
                    //if (item.columnName == "ContentTypeID") continue;

                    if (item.columnName.ToLower().Contains("path"))
                    {
                        if (item.new_value.Trim() != "")
                        {

                            if (isImage(item.new_value))
                                item.new_value = "<a type=\"pic\"  href=\"" + item.new_value + "\">"+DataServiceArabicResource.ImageWidth+"</a>";
                            else
                                item.new_value = "<a target=\"_blank\" type=\"file\" href=\"" + item.new_value + "\">"+DataServiceArabicResource.FileLoad+"</a>";
                        }
                        else
                        {
                            item.new_value = DataServiceArabicResource.NoFileExist;
                        }
                    }
                    else if (item.columnName.ToLower().Contains("password"))
                    {
                        item.new_value = "********";//QvLib.Security.DataProtection.Decrypt(item.new_value);
                    }
                    else if (item.columnName.ToLower().Contains("date"))
                    {
                        //  Extention.ToHijriArabicDate
                        //This need to be repaired
                        item.new_value = item.new_value.Replace(DataServiceArabicResource.Morning, "AM");
                        item.new_value = item.new_value.Replace(DataServiceArabicResource.Evining, "PM");
                        item.new_value = DateTime.ParseExact(item.new_value, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).ToHijriArabicDate();
                    }
                    updates.Add(DataServiceArabicResource.HasDesignation+" <span class='def_color'>" + item.text + "</span> "+DataServiceArabicResource.To+ " \"" + item.new_value + "\"");
                }
            }

            if (data.ManyToManyItems != null)
            {
                foreach (ManyToManyItem item in data.ManyToManyItems)
                {
                    if (item.Type == ActionType.Insert)
                        updates.Add(DataServiceArabicResource.HasAdded+" <span class='def_color'>" + item.value + "</span> "+DataServiceArabicResource.In+" <span class='def_color'>" + item.text + "</span>");
                    else if (item.Type == ActionType.Delete)
                        updates.Add(DataServiceArabicResource.HasDeleted+"<span class='def_color'>" + item.value + "</span> " + DataServiceArabicResource.From + " <span class='def_color'>" + item.text + "</span>");


                }
            }
            Dictionary<string[], List<string>> Return = new Dictionary<string[], List<string>>();
            Return.Add(new string[] { data.ID, data.itemText }, updates);
            return Return;
        }
        public LogInsertItem getInsertItem(string JSON)
        {
            LogInsertItem Return = new LogInsertItem();

            Return = new JavaScriptSerializer().Deserialize<LogInsertItem>(JSON);

            return Return;
        }
        private bool isImage(string filename)
        {
            string ext = filename.Substring(filename.LastIndexOf(".") + 1);
            string[] image_extentions = new string[] { "png", "gif", "jpg", "jpeg" };
            return image_extentions.Contains(ext);
        }

        #endregion

    }
}
