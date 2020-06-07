using AutoMapper;
using MobileApplication.Context;
using MobileApplication.DataModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataService
{
    public class PushNotificationLogService:BaseService
    {
        private Repository<PushNotificationLog> _PushNoitificationLogRepository;

        public PushNotificationLogService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            Initialize();
        }

        public PushNotificationLogService()
        {
            Initialize();
        }

        private void Initialize()
        {
            _PushNoitificationLogRepository = new Repository<PushNotificationLog>(this._unitOfWork);
        }

        public void Insert(PushNotificationLogModel notification)
        {
            var notificationLog = Mapper.Map<PushNotificationLogModel,PushNotificationLog >(notification);
            _PushNoitificationLogRepository.Save(notificationLog);
            this._unitOfWork.Submit();
        }




    }
}
