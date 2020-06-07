using AutoMapper;
using MobileApplication.Context;
using MobileApplication.DataModel.ControlPanel.DoaaModels;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService.ControlPanel
{
   public class DoaaService :BaseService
    {
       private readonly Repository<Doaa> _DoaaRepository;
       private readonly Repository<DoaaItemSource> _doaaItemSourceRepository;
        public DoaaService()
        {
            _DoaaRepository = new Repository<Doaa>(_unitOfWork);
            _doaaItemSourceRepository = new Repository<DoaaItemSource>(_unitOfWork);
        }
        public IEnumerable<Doaa> DoaaList
        {
            get { return _DoaaRepository.GetList(); }
        }
        public IEnumerable<DoaaItemSource> DoaaCategoryList
        {
            get { return _doaaItemSourceRepository.GetList(); }
        }
        public Doaa GetByID(int id)
        {
            var Doaa = _DoaaRepository.GetById(id);
            return Doaa;
        }

        public DoaaModel GetModelByID(int id)
        {
            var doaa = _DoaaRepository.GetById(id);
            var data = Mapper.Map<Doaa, DoaaModel>(doaa);
            return data;
        }
       //public List<DoaaCategoryModel> GetDoaaCategoryList(int DoaaMainCategoryID)
       // {
       //     var doaaList =  DoaaCategoryList.Where(x => x.DoaaMainCategoryID == DoaaMainCategoryID).ToList();
       //     return Mapper.Map<List<DoaaCategory>, List<DoaaCategoryModel>>(doaaList);
       // }
    }
}
