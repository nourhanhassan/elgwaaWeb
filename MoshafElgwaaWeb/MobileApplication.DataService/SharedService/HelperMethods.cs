using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace MobileApplication.DataService
{
    public class HelperMethods
    {
        public static IEnumerable<SelectListItem> GetOnTheFlySelectListItem()
        {
            List<string> listOfItems = new List<string>() { "True:فعال", "False:غير فعال" };
            // var listOfItems = GetList();
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var item in listOfItems)
            {
                
                result.Add(new SelectListItem
                {
                    Value = item.Split(':')[0],
                    Text = item.Split(':')[1]
                }
                );
            }
            result.FirstOrDefault(x => x.Value == "True").Selected = true;
            return result;
        }
    }
}