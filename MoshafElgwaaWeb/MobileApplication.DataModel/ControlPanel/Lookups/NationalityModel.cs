using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericEngine.ServiceContract.Infrastructure;
using System.Linq.Expressions;

namespace MobileApplication.DataModel.ControlPanel
{
    [LogName("الجنسيات")]
    public class NationalityModel : GenericModel //,IReplacableSort_GenericModel,IReplacableSearch_GenericModel,IAdaptedSearch_GenericModel
    {
        public NationalityModel()
        {
            List<GridLink> links = new List<GridLink>() { 
            new EditGridLink(),
            new DeleteGridLink(),
            };

            SetPublicSettings("جنسية", "الجنسيات", links, null, "ID", "Name", "/ControlPanel/settings/Default", "الاعدادات");
            AddBtn = new GridLink("جنسية", "fa fa-edit", "AddEdit()", "", "");
        }
        //  [Equals]
        [GridViewShown(true, "الكود")]
        public int ID { get; set; }
        [GridViewShown(true)]
        [Required]
        [String100]
        [Contains]
        [DisplayField("الاسم")]
        [System.Web.Mvc.Remote("CheckDuplicate", "Nationality", AdditionalFields = "ID", ErrorMessage = "الإسم مستخدم من قبل")]
        public string Name { get; set; }
        public bool CanDelete { get; set; }
        public bool CanEdit { get; set; }
    }
}
