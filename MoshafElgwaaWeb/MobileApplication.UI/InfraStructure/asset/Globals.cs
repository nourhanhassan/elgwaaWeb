using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Linq;
using GenericEngine.QVControlPanel.InfraStructure;
using MobileApplication.DataModel.QvDataAnnotation;
using MobileApplication.DataService;


namespace MobileApplication.UI.InfraStructure
{
    public  class Globals
    {
        public static IEnumerable<SelectListItem> GetDefaultList()
        {
            yield return new SelectListItem { Value = "", Text = ValidationMessages.Choose, Selected = true };
        }
       

      
       
    }
}
