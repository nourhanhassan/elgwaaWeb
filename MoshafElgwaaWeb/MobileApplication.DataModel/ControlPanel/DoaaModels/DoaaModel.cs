using GenericEngine.ServiceContract.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.DoaaModels
{
    public class DoaaModel : GenericModel
    {
        public DoaaModel()
        {
            List<GridLink> links = new List<GridLink>
            {
                new GridLink("تعديل", "fa fa-edit", "", "", "/ControlPanel/Doaa/Edit?Id={{item.ID}}"),
                new DeleteGridLink("حذف","fa fa-times-circle delete","Delete($event,item,$index)","","",""),
            };
            SetPublicSettings("الدعاء", "الأدعية", links, null, "ID", "Name");
            //AddBtn = new GridLink("دعاء", "fa fa-edit", "", "", "/ControlPanel/Doaa/Add");
        }

        public int ID { get; set; }
        [GridViewShown(true, "التصنيف الرئيسي")]
        public string Name { get; set; }
        public int DoaaMainCategoryID { get; set; }
        public string StrDoaaMainCategory { get; set; }
        //[QvDataAnnotation.Required]
        //public int DoaaCategoryID { get; set; }
        //[GridViewShown(true, "التصنيف الفرعي")]
        //public string StrDoaaCategory { get; set; }
        [QvDataAnnotation.Required]
        public string DoaaContent { get; set; }
        public string DoaaViewContent { get; set; }
        public string DoaaMeanings { get; set; }
        public bool HasAddBtn { get; set; }
    }
}
