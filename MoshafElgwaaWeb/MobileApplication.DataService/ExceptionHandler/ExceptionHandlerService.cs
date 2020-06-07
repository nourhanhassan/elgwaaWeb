using MobileApplication.Context;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class ExceptionHandlerService : BaseService
    {
        Repository<ExceptionLog> _ExceptionLogRepository { get; set; }

        public ExceptionHandlerService()
        {
            _ExceptionLogRepository = new Repository<ExceptionLog>(_unitOfWork);
        }

        public void LogException(Exception exception, string url, string data)
        {
            ExceptionLog ex = new ExceptionLog();
            ex.CreatedDate = DateTime.Now;
            ex.Message = exception.ToString();
            ex.URL = url;
            ex.Data = data;
            _ExceptionLogRepository.Save(ex);
            _unitOfWork.Submit();

        }
    }
}
