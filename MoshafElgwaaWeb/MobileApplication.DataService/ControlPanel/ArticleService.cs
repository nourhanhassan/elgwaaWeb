using MobileApplication.Context;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService.ControlPanel
{
    public class ArticleService : BaseService
    {
        private readonly Repository<Article> _articleRepository;
        private readonly Repository<ArticleItemExplanation> _articleItemExplanationRepository;
        public ArticleService()
        {
            _articleRepository = new Repository<Article>(_unitOfWork);
            _articleItemExplanationRepository = new Repository<ArticleItemExplanation>(_unitOfWork);
        }
    }
}
