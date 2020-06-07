using GenericEngine.ServiceContract.Infrastructure;
using MobileApplication.DataModel.QvDataAnnotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.ArticleModels
{
    [LogName("الفصول")]
    public class ArticleModel : GenericModel
    {
        public ArticleModel()
        {
            List<GridLink> links = new List<GridLink>
            {
                new GridLink("تعديل", "fa fa-edit", "", "", "/ControlPanel/Article/Edit?Id={{item.ID}}"),
                new DeleteGridLink("حذف","fa fa-times-circle delete","Delete($event,item,$index)","","",""),
            };
            SetPublicSettings("الفصل", "الفصول", links, null, "ID", "Name");
            //AddBtn = new GridLink("فصل", "fa fa-edit", "", "", "/ControlPanel/Article/Add");
        
        }
        public int ID { get; set; }
        [GridViewShown(true, "اسم الفصل")]
        public string Name { get; set; }
        [Required]
        public string ArticleContent { get; set; }
        public bool HasAddBtn { get; set; }

    }
}
