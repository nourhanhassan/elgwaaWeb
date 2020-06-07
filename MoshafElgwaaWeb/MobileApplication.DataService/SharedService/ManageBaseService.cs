using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericEngine.Service.AutoMapper;
using Service.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using GenericEngine.ServiceContract.Infrastructure;
using Repository;
using System.Data.Entity;
using MobileApplication.DataService;
using GenericEngine.Service.SharedService;
//using System.Web.Mvc;



namespace MobileApplication.DataService
{
    public partial class ManageBaseService<TModel> : BaseService, IManageBaseService<TModel>
        where TModel : class
    {

        protected ILogService _logService;
        //protected IUnitOfWork _unitOfWork
        //{
        //    get;
        //    set;
        //}
        private dynamic _ObjectRepository { get; set; } //Repository for the DBContext Object type
        private Type ObjectContextType { get; set; } //The type of the type in the DBContext that matches the model type 
        public string UniqueIDColumName;
        public string LogRepresintitiveColumName;
        public Type repositoryType { get; set; }
        // protected SuperBaseService BaseService = null;
        public ManageBaseService()//IUnitOfWork BaseUnitOfWork = null, ILogService logService = null
        {
            //This is a kofta method
            // Type GenericBaseType=Type.GetType("INV.Context."+T+", INV.Context");
            //Type RepositoryBaseType = typeof(Repository<>).MakeGenericType(GenericBaseType);
            //_ObjectRepository = Activator.CreateInstance(RepositoryBaseType, this._unitOfWork);

            //get the matching type 
            ObjectContextType = Generic_Configurations.GetMatchingType(typeof(TModel));

            //get the type of the repository
            repositoryType = typeof(Repository<>).MakeGenericType(ObjectContextType);

            //initialize the repository
            _ObjectRepository = Activator.CreateInstance(repositoryType, _unitOfWork);

            UniqueIDColumName = "ID";
            LogRepresintitiveColumName = "Name";

            _logService = base._LogService;


        }

        #region methods for log transaction
        public static string getTableNameForLog(object Model)
        {
            string TableName = null;
            var SelectedClass = Model.GetType();
            var attr = SelectedClass.GetCustomAttributes(typeof(LogName), true);
            if (attr.Count() > 0)
            {
                LogName logNameAttr = (LogName)(attr.First());
                TableName = logNameAttr.tableName;
            }
            else if (Model.GetType().GetProperty("PageName") != null)
            {
                TableName = Model.GetType().GetProperty("PageName").GetValue(Model).ToString();
            }

            return TableName;
        }
        public static List<PropertyInfo> getPropertyWithSpesificAttr(object Model, Type SelectedType)
        {
            var SelectedClass = Model.GetType();
            // var nnn=SelectedClass.GetCustomAttributes(SelectedType, true);
            List<PropertyInfo> props = SelectedClass.GetProperties().Where(i => i.GetCustomAttributes(SelectedType, true).Any()).ToList();

            return props;
        }
        public static object[] getPropertyAttr(PropertyInfo prop, Type SelectedType)
        {
            var result = prop.GetCustomAttributes(SelectedType, true);
            return result;
        }

        #endregion

        public virtual GridModel GetDataList<TGridModel>(Dictionary<string, string> sortings, Dictionary<string, string> filter, int page, int count, string ActionName, int UserId = 0)
            where TGridModel : class
        {
            var FuncType = typeof(SearchandSort<>).MakeGenericType(ObjectContextType);
            var obj = Activator.CreateInstance(FuncType, _unitOfWork);
            //var SortQuery = FuncType.GetMethod("GetOrderByQuery").Invoke(obj, new object[] { sortings});

            //The grid used to carry list of objects
            GridModel returnModel = new GridModel();

            int Out_Count = 0;
            //return list of objects after sending to the facilator which manage to both filter based on constant attribute
            //and filter based on search criteria
            object[] dataToBeSentToFunction = new object[] { Out_Count, filter, sortings, typeof(TGridModel), page, count };
            var ObjectList = FuncType.GetMethod("manageSearch").Invoke(obj, dataToBeSentToFunction);
            Out_Count = Convert.ToInt32(dataToBeSentToFunction[0]);
            returnModel.count = Out_Count;

            //List of the objects to be mapped
            List<TGridModel> modelList = new List<TGridModel>();
            foreach (var dbObject in (IEnumerable<dynamic>)ObjectList)
            {
                TGridModel mappedObject = (TGridModel)Mapper.Map(dbObject, ObjectContextType, typeof(TGridModel));
                modelList.Add(mappedObject);
            }
            //logservice
            //  _logService.Read(ActionName, UserId);
            CanDelete(modelList);
            returnModel.data = modelList;
            return returnModel;
        }

        public virtual GridModel GetDataList<TGridModel>(Dictionary<string, string> sortings, Dictionary<string, string> filter, int page, int count, string ActionName, Dictionary<string, string> k)
           where TGridModel : class
        {
            var FuncType = typeof(SearchandSort<>).MakeGenericType(ObjectContextType);
            var obj = Activator.CreateInstance(FuncType, _unitOfWork);
            //var SortQuery = FuncType.GetMethod("GetOrderByQuery").Invoke(obj, new object[] { sortings});

            //The grid used to carry list of objects
            GridModel returnModel = new GridModel();

            int Out_Count = 0;
            //return list of objects after sending to the facilator which manage to both filter based on constant attribute
            //and filter based on search criteria
            object[] dataToBeSentToFunction = new object[] { Out_Count, filter, sortings, typeof(TGridModel), page, count };
            var ObjectList = FuncType.GetMethod("manageSearch").Invoke(obj, dataToBeSentToFunction);
            Out_Count = Convert.ToInt32(dataToBeSentToFunction[0]);
            returnModel.count = Out_Count;

            //List of the objects to be mapped
            List<TGridModel> modelList = new List<TGridModel>();
            foreach (var dbObject in (IEnumerable<dynamic>)ObjectList)
            {
                TGridModel mappedObject = (TGridModel)Mapper.Map(dbObject, ObjectContextType, typeof(TGridModel));
                modelList.Add(mappedObject);
            }
            //logservice
            //  _logService.Read(ActionName, UserId);
            CanDelete(modelList);
            returnModel.data = modelList;
            return returnModel;
        }
        public virtual void CanDelete(IEnumerable<object> modelList)

        {

        }

        public virtual GridModel GetDataList(Dictionary<string, string> sortings, Dictionary<string, string> filter, int page, int count, string ActionName, int UserId = 0)
        {
            Dictionary<string, string> sortings2 = new Dictionary<string, string>();
            sortings2.Add("ID", "asc");
            return GetDataList<TModel>(sortings2, filter, page, count, ActionName, UserId);
        }
        /// <summary>
        /// Get Object by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TModel GetById(int id)
        {
            /*
            var dbObject = _ObjectRepository.GetById(id);
            TModel mappedObject = (TModel)Mapper.Map(dbObject, ObjectContextType, typeof(TModel));
            return mappedObject;
             * */

            return GetById<TModel>(id);
        }

        public virtual TAnotherModel GetById<TAnotherModel>(int id)
        {
            var dbObject = _ObjectRepository.GetById(id);
            TAnotherModel mappedObject = (TAnotherModel)Mapper.Map(dbObject, ObjectContextType, typeof(TAnotherModel));
            return mappedObject;
        }

        public virtual object GetobjectById(int id)
        {
            return _ObjectRepository.GetById(id);
        }
        /// <summary>
        /// Get all the items of a type
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TModel> GetList()
        {

            IEnumerable<dynamic> dbObjectList = (IEnumerable<dynamic>)_ObjectRepository.GetList();
            // return (IEnumerable<TModel>)_ObjectRepository.GetList();

            List<TModel> modelList = new List<TModel>();
            foreach (var dbObject in dbObjectList)
            {

                TModel mappedObject = (TModel)Mapper.Map(dbObject, ObjectContextType, typeof(TModel));

                modelList.Add(mappedObject);
            }
            return modelList;
        }

        //public virtual IEnumerable<dynamic> GetLookupList(bool IncludePermanent = true)
        //{
        //    IEnumerable<dynamic> dbObjectList = (IEnumerable<dynamic>)_ObjectRepository.GetList();
        //    //var rolesList = new LookUpService().ContentCategoriesList(IncludePermanent);
        //    var obj = new SelectListItem { Value = "", Text ="اختر", Selected = true };
        //    yield return obj;
        //    foreach (var item in dbObjectList)
        //    {
        //        obj = new SelectListItem { Value = item.ID.ToString(), Text = item.Name };
        //        yield return obj;
        //    }
        //}

        public virtual bool Delete(int id, int userId, string ActionName)
        {
            try
            {
                var obj = GetById(id);
                _ObjectRepository.Delete(id);
                var deleteResult = _unitOfWork.Submit();
                var propertyInfo = obj.GetType().GetProperty(LogRepresintitiveColumName);
                if (propertyInfo != null)
                {
                    string tableNameforLog = getTableNameForLog(obj) ?? "";
                    string name = propertyInfo.GetValue(obj, null).ToString();
                    _logService.Delete(name + tableNameforLog + " من ", ActionName, userId);
                }
                var flag = deleteResult != 0;
                return flag;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public virtual bool CheckDuplicate(String ColumnName, string KeyWord, int id = 0, string tableName = "")
        {
            Type FuncType;
            if (string.IsNullOrEmpty(tableName))
            {
                FuncType = typeof(facilitator<>).MakeGenericType(ObjectContextType);
            }
            else
            {

                var _assembly = ObjectContextType.Assembly;
                var _namespace = ObjectContextType.Namespace;
                Type ty = Type.GetType(_namespace + "." + tableName + ", " + _assembly.GetName().Name);
                FuncType = typeof(facilitator<>).MakeGenericType(ty);
            }
            var obj = Activator.CreateInstance(FuncType, _unitOfWork);
            return (bool)FuncType.GetMethod("CheckDuplicate").Invoke(obj, new object[] { ColumnName, KeyWord, id });
        }

        public virtual int Save(TModel obj, int UserId, string ActionName, ref int? logID)
        {
            return this.Save<TModel>(obj, UserId, ActionName, ref logID);
        }

        public virtual int Save<TAnotherModel>(TAnotherModel obj, int UserId, string ActionName, ref int? logID)
        {
            //Create the Generics Main Class 
            var FuncType = typeof(facilitator<>).MakeGenericType(ObjectContextType);
            //Create an object from the Generic <ObjectContextType> class
            var Callobj = Activator.CreateInstance(FuncType, _unitOfWork);

            //Get the identity Coulmn name "ObjectContextType" value to check with 
            int UniqueId = (int)obj.GetType().GetProperty(UniqueIDColumName).GetValue(obj, null);
            //create an new defult instance from the ObjectContextType class
            var oldObj = FuncType.GetMethod("Create_TModel_Instance").Invoke(Callobj, new object[] { });
            //Map the sent TModel class to it's own dbclass to be apple to save it
            var data = Mapper.Map(obj, typeof(TAnotherModel), ObjectContextType);


            if (UniqueId > 0)
            {
                oldObj = GetobjectById(UniqueId).Clone();
                oldObj = Mapper.Map(oldObj, ObjectContextType, typeof(TAnotherModel));

            }



            //Because of the repository is dinamic saving a dinamic type throw a dinamic class is not valid that make us go to the Generics to transmite the dinamic object to geneiric one to be apple to save it
            var result = (int)FuncType.GetMethod("Call_Save").Invoke(Callobj, new object[] { data });

            //Id after saving  
            int IdItem = (int)data.GetType().GetProperty(UniqueIDColumName).GetValue(data, null);

            //namelog
            // string namelog = obj.GetType().GetProperty("LogName").GetValue(obj, null).ToString();
            string TableNameforLog = getTableNameForLog(obj) ?? "";

            if (UniqueId > 0)
            {
                if (logID != null)
                {
                    // data
                    logID = _logService.Update(logID, IdItem, TableNameforLog, oldObj, obj, UserId, ActionName);

                }
                // data
                else
                    logID = _logService.Update(IdItem, TableNameforLog, oldObj, obj, UserId, ActionName);
            }
            else
            {
                //  data.GetType().GetProperty("ID").SetValue(data, IdItem);
                obj.GetType().GetProperty("ID").SetValue(obj, IdItem);

                logID = _logService.Insert(IdItem, TableNameforLog, obj, UserId, ActionName.ToLower().Replace("add", "edit").Replace("0", IdItem.ToString()));

            }
            return result;
        }



        public virtual int Save(TModel obj, int UserId, string ActionName)
        {
            //Create the Generics Main Class 
            return this.Save<TModel>(obj, UserId, ActionName);

        }

        public virtual int Save<TAnotherModel>(TAnotherModel obj, int UserId, string ActionName)
        {
            var FuncType = typeof(facilitator<>).MakeGenericType(ObjectContextType);
            //Create an object from the Generic <ObjectContextType> class
            var Callobj = Activator.CreateInstance(FuncType, _unitOfWork);

            //Get the identity Coulmn name "ObjectContextType" value to check with 
            int UniqueId = (int)obj.GetType().GetProperty(UniqueIDColumName).GetValue(obj, null);
            //create an new defult instance from the ObjectContextType class
            var oldObj = FuncType.GetMethod("Create_TModel_Instance").Invoke(Callobj, new object[] { });
            //Map the sent TModel class to it's own dbclass to be apple to save it
            var data = Mapper.Map(obj, typeof(TAnotherModel), ObjectContextType);


            if (UniqueId > 0)
            {
                oldObj = GetobjectById(UniqueId).Clone();
            }



            //Because of the repository is dinamic saving a dinamic type throw a dinamic class is not valid that make us go to the Generics to transmite the dinamic object to geneiric one to be apple to save it
            var result = (int)FuncType.GetMethod("Call_Save").Invoke(Callobj, new object[] { data });

            //Id after saving  
            int IdItem = (int)data.GetType().GetProperty(UniqueIDColumName).GetValue(data, null);

            //namelog
            // string namelog = obj.GetType().GetProperty("LogName").GetValue(obj, null).ToString();
            string TableNameforLog = getTableNameForLog(obj) ?? "";

            if (UniqueId > 0)
            {
                _logService.Update(IdItem, TableNameforLog, oldObj, data, UserId, ActionName);
            }
            else
            {
                //  data.GetType().GetProperty("ID").SetValue(data, IdItem);
                obj.GetType().GetProperty("ID").SetValue(obj, IdItem);

                _logService.Insert(IdItem, TableNameforLog, obj, UserId, ActionName.ToLower().Replace("add", "edit").Replace("0", IdItem.ToString()));

            }
            return result;
        }

        #region oldPart 
        //Delete if you wish this functionalities has been transfered to BuildExpression.cs
        //# region ManageModelAttributes

        //public static Dictionary<string, object[]> GetModelAttributesList(Type attribute)
        //{
        //    Dictionary<string, object[]> myList = new Dictionary<string, object[]>();
        //    PropertyInfo[] props = typeof(TModel).GetProperties();
        //    foreach (PropertyInfo prop in props)
        //    {
        //        var x = prop.Attributes.GetType();
        //        var y = prop.CustomAttributes;
        //        if (prop.GetCustomAttributes(attribute, true).ToList().Count > 0)
        //        {
        //            object[] result = prop.GetCustomAttributes(attribute, true);
        //            myList.Add(prop.Name, result);
        //        }
        //    }
        //    return myList;
        //}

        //public Expression<Func<T, bool>> GetSearchedListByFilter<T>(Dictionary<string, string> filter, Dictionary<string, object[]> modelAttributesList)
        //{
        //    // IEnumerable<dynamic> dbObjectList = (IEnumerable<dynamic>)_ObjectRepository.GetList();
        //    //List<dynamic> dbObjectListSearched = new List<dynamic>();
        //    Expression<Func<T, bool>> sumExps = null;
        //    foreach (var obj in filter)
        //    {
        //        string propertyName = obj.Key;
        //        string filterValue = obj.Value;
        //        var thisAttr = modelAttributesList.FirstOrDefault(t => t.Key == obj.Key).Value.ToList();
        //        if (thisAttr != null)
        //        {
        //            foreach (var objAttr in thisAttr)
        //            {
        //                Expression<Func<T, bool>> filterExp = null;
        //                if (ObjectContextType.GetProperties().Any(a => a.Name == propertyName))
        //                {
        //                    filterExp = ((SearchAttribute)objAttr).Operator<T>(propertyName, filterValue, filter);
        //                    if (sumExps == null)
        //                    {
        //                        sumExps = filterExp;
        //                    }
        //                    else
        //                    {
        //                        var sum = Expression.Or(sumExps.Body, Expression.Invoke(filterExp, sumExps.Parameters[0]));
        //                        sumExps = Expression.Lambda<Func<T, bool>>(sum, sumExps.Parameters);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    ParameterExpression param = Expression.Parameter(typeof(T), "d");
        //    var sx = sumExps.Body;
        //    var data = Expression.Lambda<Func<T, bool>>(sx, sumExps.Parameters[0]);
        //    var dataCompiled = data.Compile();
        //    return data;
        //}
        //when first time to search,add to search list from main list and when search property has more than attribute 
        //if (thisAttr.Count > 1)
        //{
        //    if (dbObjectListSearched.Count == 0)
        //        dbObjectListSearched.AddRange(dbObjectList.Where(filterExp).ToList());
        //    else
        //        dbObjectListSearched = dbObjectListSearched.Where(filterExp).ToList();

        //    if (dbObjectList.Where(filterExp).ToList().Count == 0 || dbObjectListSearched.Count == 0)
        //        return dbObjectListSearched;
        //}
        ////when first time to search,add to search list from main list and when search property has more than attribute 
        //else
        //{
        //    if (dbObjectListSearched.Count >= 1 && thisAttr.Count <= 1)
        //    {
        //        dbObjectListSearched = dbObjectListSearched.Where(filterExp).ToList();
        //        if (dbObjectListSearched.Count == 0)
        //            return dbObjectListSearched;
        //    }
        //    else
        //    {
        //        dbObjectListSearched.AddRange(dbObjectList.Where(filterExp).ToList());
        //        if (dbObjectListSearched.Count == 0)
        //            return dbObjectListSearched;
        //    }
        //}



        //public IEnumerable<dynamic> GetConstantList(IEnumerable<dynamic> dbObjectList)
        //{
        //    var constAttributesList = GetModelAttributesList(typeof(ConstAttribute));
        //    foreach (var objAttr in constAttributesList)
        //    {
        //        Func<dynamic, bool> filterExp = null;
        //        foreach (var thisAttr in objAttr.Value)
        //        {
        //            var col = ((ConstAttribute)thisAttr).Name;
        //            var colName = string.IsNullOrEmpty(col) ? objAttr.Key.ToString() : col;

        //            //filterExp = ((ConstAttribute)thisAttr).Operator(colName, ((ConstAttribute)thisAttr).value);
        //            dbObjectList = dbObjectList.Where(filterExp);

        //        }
        //    }
        //    return dbObjectList;
        //}
        //# endregion


        //public Expression<Func<T, bool>> GetConstantList<T>()
        //{
        //    var constAttributesList = GetModelAttributesList(typeof(ConstAttribute));
        //    //ParameterExpression param = Expression.Parameter(typeof(T), "t");
        //    //Expression<Func<T, bool>> exp = null;
        //    Expression<Func<T, bool>> sumExps = null;
        //    //string name = result[0].GetType().GetProperty("Name").GetValue(result[0], null).ToString();
        //    var SortedList = constAttributesList.OrderBy(o => o.Value[0].GetType().GetProperty("Name").GetValue(o.Value[0], null).ToString() == "" ? o.Key : o.Value[0].GetType().GetProperty("Name").GetValue(o.Value[0], null).ToString()).ToList();

        //    foreach (var objAttr in SortedList.Select((value, i) => new { i, value }))
        //    {
        //        int x = objAttr.i - 1;
        //        Expression<Func<T, bool>> filterExp = null;
        //        int flag = 0;
        //        foreach (var thisAttr in objAttr.value.Value)
        //        {
        //            var col = ((ConstAttribute)thisAttr).Name;
        //            var colName = string.IsNullOrEmpty(col) ? objAttr.value.Key.ToString() : col;
        //            if (x >= 0)
        //            {
        //                flag = 1;
        //            }
        //            var col2 = flag==1?((ConstAttribute)SortedList[x].Value[0]).Name:"undefined";
        //            var colName2 = flag==1?(string.IsNullOrEmpty(col2) ? SortedList[x].Key.ToString() : col2):"undefined";

        //            filterExp = ((ConstAttribute)thisAttr).Operator<T>(colName, ((ConstAttribute)thisAttr).value);
        //            if (sumExps == null)
        //            {
        //                sumExps = filterExp;
        //            }
        //            else
        //            {
        //                if (colName == colName2)
        //                {
        //                    var sum = Expression.Or(sumExps.Body, Expression.Invoke(filterExp, sumExps.Parameters[0]));
        //                    sumExps = Expression.Lambda<Func<T, bool>>(sum, sumExps.Parameters);
        //                    //sumExps = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(sumExps.Body, filterExp.Body), sumExps.Parameters.Single());
        //                }
        //                //concatinate previous value to new one 
        //                else
        //                {
        //                    var sum = Expression.And(sumExps.Body, Expression.Invoke(filterExp, sumExps.Parameters[0]));
        //                    sumExps = Expression.Lambda<Func<T, bool>>(sum, sumExps.Parameters);
        //                    //sumExps = Expression.Lambda<Func<T, bool>>(Expression.Or(sumExps.Body, filterExp.Body), sumExps.Parameters.Single());
        //                }
        //                //Expression.AndAlso(exp, filterExp);
        //            }

        //        }
        //    }
        //    //return Expression.Lambda<Func<T, bool>>(exp, param);
        //    //return Expression.Lambda<Func<T, bool>>(exp, param);
        //    ParameterExpression param = Expression.Parameter(typeof(T), "d");
        //    var sx = sumExps.Body;
        //    var data = Expression.Lambda<Func<T, bool>>(sx, sumExps.Parameters[0]);
        //    var dataCompiled =data.Compile();
        //    return data;
        //}
        #endregion
    }

}
