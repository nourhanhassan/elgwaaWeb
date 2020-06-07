using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApplication.DataModel
{
    public class PermissionModel
    {
        public int ID { get; set; }
        public string PageName { get; set; }
        public int IdPage { get; set; }
        public int? SubPagesCount { get; set; }
        public bool View { get; set; }
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Admin { get; set; }

        public bool Report { get; set; }

        public bool Delete { get; set; }
        public bool Password { get; set; }
        public bool All { get; set; }
        public string PagePermissions { get; set; }
        public string Permission { get; set; }


    }
}