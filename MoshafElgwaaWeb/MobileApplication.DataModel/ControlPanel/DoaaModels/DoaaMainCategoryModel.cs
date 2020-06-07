using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel.ControlPanel.DoaaModels
{
    public class DoaaMainCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DoaaCategoryModel> DoaaCategoryList { get; set; }
    }
}
