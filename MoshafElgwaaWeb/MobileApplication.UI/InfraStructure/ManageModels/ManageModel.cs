using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.UI.InfraStructure
{
    public class ManageModel
    {
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ModelName { get; set; }
        public string ModalPopUpId { get; set; }
    }

    public class FileUploadModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public string Ext { get; set; }
        public string NgModel { get; set; }
    }

}