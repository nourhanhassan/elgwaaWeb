using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.Areas.ControlPanel.Models
{
    public class MultiAttachmentsVM
    {
        //The element id to be created (should be unique if you have multiple uploaders in the page
        public string ElementID { get; set; }

     
        public string AllowedExtensions { get; set; }
      
        //The foreign key name to be added to the object
        public string ForeignKeyName { get; set; }
        public string ForeignKeyValue { get; set; }

        //The folder to upload the images into
        public string Folder { get; set; }
        //The property name in the scope object that holds the attachments array
        public string AttachmentsPropertyName { get; set; }

        public string IsUploadingPropertyName { get; set; }

        public int MaxAllowedSize { get; set; }

        public int MaxFiles { get; set; }

        public MultiAttachmentsVM()
        {
            MaxAllowedSize = 2;
            AllowedExtensions = ".png,.jpg,.jpeg";
            AttachmentsPropertyName = "attachments";
            IsUploadingPropertyName = "isUploading";
            Folder = "/DataUpload/";
            MaxFiles = 5;
        }


    }
}