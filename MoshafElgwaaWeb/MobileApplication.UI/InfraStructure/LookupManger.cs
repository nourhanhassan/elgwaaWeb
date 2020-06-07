using MobileApplication.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobileApplication.UI.InfraStructure
{
    public static class LookupManger<t>// : LookupManger<t>
    {
        public static IEnumerable<t> GetList()
        {
            Type FuncType = typeof(ManageBaseService<>).MakeGenericType(typeof(t));
            var _ProjectTypeService = Activator.CreateInstance(FuncType);
            var listOfItems = (IEnumerable<t>)FuncType.GetMethod("GetList").Invoke(_ProjectTypeService, new object[] { });
            return listOfItems;
        }

        public static IEnumerable<SelectListItem> GetSelectListItem(string TextAttr = "Name", string ValueAttr = "ID")
        {
            var listOfItems = GetList();
            var obj = new SelectListItem { Value = "", Text = "اختر", Selected = true };
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(obj);
            if (typeof(t).GetProperty("IsDeleted") != null)
            {
                var IsDeletedvalue = typeof(t).GetProperty("IsDeleted");
                listOfItems = listOfItems.Where(i => IsDeletedvalue.GetValue(i, null).ToString().ToLower() != "true");
            }
            var Value = typeof(t).GetProperty(ValueAttr);
            var text = typeof(t).GetProperty(TextAttr);
            foreach (var item in listOfItems)
            {
                result.Add(new SelectListItem
                {
                    Value = Value.GetValue(item, null).ToString(),
                    Text = text.GetValue(item, null).ToString()
                }
               );
            }
            return result;
        }

        public static IEnumerable<SelectListItem> GetSelectListItemWithFilter(string FilterName, int FilterValue, string TextAttr = "Name", string ValueAttr = "ID")
        {
            var listOfItems = GetList();
            var obj = new SelectListItem { Value = "", Text = "اختر", Selected = true };
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(obj);
            var Value = typeof(t).GetProperty(ValueAttr);
            var text = typeof(t).GetProperty(TextAttr);
            var filter = typeof(t).GetProperty(FilterName);
            var boolFilterValue = false;
            var FlagVal = 0;
            if (FilterName == "IsDeleted")
            {
                FlagVal = 1;
                if (FilterValue == 1)
                {
                    boolFilterValue = true;
                }
                else
                {
                    boolFilterValue = false;
                }
            }
            foreach (var item in listOfItems)
            {
                if (FlagVal == 1)
                {
                    if (filter.GetValue(item, null).ToString().ToLower().Trim().Equals(boolFilterValue.ToString().ToLower().Trim()))
                    {
                        result.Add(new SelectListItem
                        {
                            Value = Value.GetValue(item, null).ToString(),
                            Text = text.GetValue(item, null).ToString()
                        }
                       );
                    }
                }
                else
                {
                    if (filter.GetValue(item, null).ToString().Equals(FilterValue.ToString()))
                    {
                        result.Add(new SelectListItem
                        {
                            Value = Value.GetValue(item, null).ToString(),
                            Text = text.GetValue(item, null).ToString()
                        }
                       );
                    }
                }

            }
            return result;
        }

        public static IEnumerable<SelectListItem> GetActiveSelectListItem(string TextAttr = "Name", string ValueAttr = "ID")
        {
            var listOfItems = GetList();
            var obj = new SelectListItem { Value = "", Text = "اختر", Selected = true };
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(obj);
            if (typeof(t).GetProperty("IsActive") != null)
            {
                var IsActivevalue = typeof(t).GetProperty("IsActive");
                listOfItems = listOfItems.Where(i => IsActivevalue.GetValue(i, null).ToString().ToLower() != "false");
            }
            var Value = typeof(t).GetProperty(ValueAttr);
            var text = typeof(t).GetProperty(TextAttr);
            foreach (var item in listOfItems)
            {
                result.Add(new SelectListItem
                {
                    Value = Value.GetValue(item, null).ToString(),
                    Text = text.GetValue(item, null).ToString()
                }
               );
            }
            return result;
        }
      
    }
}
