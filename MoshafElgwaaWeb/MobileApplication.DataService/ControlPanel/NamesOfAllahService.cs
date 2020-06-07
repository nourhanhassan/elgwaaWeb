using MobileApplication.Context;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService.ControlPanel
{
    public class NamesOfAllahService : BaseService
    {
        private readonly Repository<NamesOfAllah> _namesOfAllahRepository;
        public NamesOfAllahService()
        {
            _namesOfAllahRepository = new Repository<NamesOfAllah>(_unitOfWork);
        }
    }
}
