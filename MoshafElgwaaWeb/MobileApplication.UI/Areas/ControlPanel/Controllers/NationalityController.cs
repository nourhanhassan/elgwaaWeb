
using MobileApplication.DataService;
using MobileApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileApplication.UI.ControlPanel.Controllers;
using MobileApplication.DataModel.ControlPanel;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class NationalityController : ViewGridBaseController<NationalityModel>
    {

        public NationalityController()
        //  : base(new TestService())      // just in case of changing somr thing in the service but the inserted service must inherit from IManageBaseService<[same model]>
        {
           
        }



	}
}