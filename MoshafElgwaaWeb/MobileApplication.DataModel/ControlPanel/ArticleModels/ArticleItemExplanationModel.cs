using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.ArticleModels
{
    public class ArticleItemExplanationModel : GenericModel
    {
        public ArticleItemExplanationModel()
        {
            List<GridLink> links = new List<GridLink>
            {
                new GridLink("تعديل", "fa fa-edit", "", "", "/ControlPanel/ArticleItemExplanation/Edit?Id={{item.ID}}"),
                new DeleteGridLink("حذف","fa fa-times-circle delete","Delete($event,item,$index)","","",""),
            };
            SetPublicSettings("الحاشية", "الحواشي", links, null, "ID", "Name");
            AddBtn = new GridLink("حاشية", "fa fa-edit", "", "", "/ControlPanel/ArticleItemExplanation/Add");
        }
        public int ID { get; set; }
        [GridViewShown(true, "اسم الحاشية")]
        public string ArticleName { get; set; }
        [Required]
        public int ArticleID { get; set; }

        [Required]
        public string ItemExplanation { get; set; }
        [GridViewShown(true, "رقم الحاشية")]
        [Required]
        public int ExplanationNumber { get; set; }
        

    }
}
