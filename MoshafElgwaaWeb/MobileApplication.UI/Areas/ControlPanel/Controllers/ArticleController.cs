using MobileApplication.DataModel.ControlPanel.ArticleModels;
using MobileApplication.DataService.ControlPanel;
using MobileApplication.UI.ControlPanel.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.UI.Areas.ControlPanel.Controllers
{
    public class ArticleController : ViewGridBaseController<ArticleModel>
    {
        private readonly ArticleService _articleService;
        public ArticleController()
            : base()
        {
            _articleService = new ArticleService();
        }
	}
}