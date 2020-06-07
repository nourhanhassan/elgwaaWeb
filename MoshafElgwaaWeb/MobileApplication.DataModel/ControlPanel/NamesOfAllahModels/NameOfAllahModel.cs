using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.NamesOfAllahModels
{
    public class NameOfAllahModel : GenericModel
    {
        public NameOfAllahModel()
        {
            List<GridLink> links = new List<GridLink>
            {
                new GridLink("تعديل", "fa fa-edit", "", "", "/ControlPanel/NamesOfAllah/Edit?Id={{item.ID}}"),
                new DeleteGridLink("حذف","fa fa-times-circle delete","Delete($event,item,$index)","","",""),
            };
            SetPublicSettings("الأسم", "أسماء الله الحسني", links, null, "ID", "Name");
            AddBtn = new GridLink("اسم", "fa fa-edit", "", "", "/ControlPanel/NamesOfAllah/Add");
        }
        public int ID { get; set; }
        //[Required]
        public string NameOfAllahImage { get; set; }
        [GridViewShown(true, "الاسم")]
        [Required]
        public string NameOfAllah { get; set; }
        [Required]
        public string NameOfAllahMeaning { get; set; }
        [GridViewShown(true, "الرقم")]
        [Required]
        [Number]
        public int? Number { get; set; }
    }
}
