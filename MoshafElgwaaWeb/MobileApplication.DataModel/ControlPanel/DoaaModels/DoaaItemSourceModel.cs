using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.DoaaModels
{
    public class DoaaItemSourceModel : GenericModel
    {
        public DoaaItemSourceModel()
        {
            List<GridLink> links = new List<GridLink>
            {
                new GridLink("تعديل", "fa fa-edit", "", "", "/ControlPanel/DoaaItemSource/Edit?Id={{item.ID}}"),
                new DeleteGridLink("حذف","fa fa-times-circle delete","Delete($event,item,$index)","","",""),
            };
            SetPublicSettings("مصدر الدعاء", "مصادر الأدعية", links, null, "ID", "Name");
            AddBtn = new GridLink("مصدر دعاء", "fa fa-edit", "", "", "/ControlPanel/DoaaItemSource/Add");
        }
        public int ID { get; set; }
        [GridViewShown(true, "رقم المصدر")]
        [Required]
        public int SourceNumber { get; set; }
        [Required]
        public int DoaaID { get; set; }
        [Required]
        public string ItemSource { get; set; }
        public string ItemSynonyms { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        [GridViewShown(true, "التصنيف الرئيسي")]
        public string DoaaName { get; set; }
    }
}
