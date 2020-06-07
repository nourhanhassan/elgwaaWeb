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
    public class ArticleItemExplanationController : ViewGridBaseController<ArticleItemExplanationModel>
    {
        private readonly ArticleService _articleItemExplanationService;
        public ArticleItemExplanationController()
            : base()
        {
            _articleItemExplanationService = new ArticleService();
        }
        public ActionResult Default()
        {
            return View();
        }
	}
}