using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MobileApplication.UI.InfraStructure
{
    public class PagePath
    {
        public string PageTitle { get; set; } //The Page title to be displayed
        public string ParentPath { get; set; } //The parent path text comma separated (Main,Branches,Add)
        public string ParentURLs { get; set; } //The parent URLs comma separated 
        public Dictionary<string, string> ParentPathDic { get; set; }

        public PagePath()
        {
            ParentPathDic = new Dictionary<string, string>();
        }

    }

}