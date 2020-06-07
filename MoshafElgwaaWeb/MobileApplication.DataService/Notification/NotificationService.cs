using Repository;
using MobileApplication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApplication.DataModel;
using AutoMapper;

namespace MobileApplication.DataService
{
    public class NotificationService : BaseService
    {
        private  Repository<Notification> _NotificationRepository;
        private  Repository<NotificationUser> _NotificationUserRepository;
        public  static INotificationHub _NotificationHub;

        public NotificationService()
        {
            Initialize();
        }

        public NotificationService(IUnitOfWork unitofWork)
        {
            this._unitOfWork = unitofWork;
            Initialize();
        }

        private void Initialize()
        {
            _NotificationRepository = new Repository<Notification>(_unitOfWork);
            _NotificationUserRepository = new Repository<NotificationUser>(_unitOfWork);
        }
        public IEnumerable<NotificationUser> AllNotifications
        {
            get { return _NotificationUserRepository.GetList(); }
        }


        public IEnumerable<NotificationUser> NotificationsbyUserId(int UserId)
        {
            return _NotificationUserRepository.Get(u => u.AppUserID == UserId).ToList();
        }

        public int NotificationUserCount(int UserId)
        {
            return _NotificationUserRepository.Get(u => u.AppUserID == UserId).Count();
        }


        public IEnumerable<NotificationModel> GetUnseenNotifications(int userId)
        {
            var notificationsList = _NotificationUserRepository.GetByFilter(a => a.IsSeen == false && a.AppUserID == userId).Select(a => a.Notification);
            return Mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationModel>>(notificationsList);
        }

        public IEnumerable<NotificationUserModel> Notifications(int UserId, Dictionary<string, string> sorting, Dictionary<string, string> filter)
        {
            int IDType = filter.ContainsKey("IDType") ? int.Parse(filter["IDType"]) : 0;
            IEnumerable<NotificationUser> Notifications = NotificationsbyUserId(UserId).Where(x => (IDType == 0)).OrderByDescending(x => x.NotificationID);

            if (sorting.ContainsKey("ID"))
            {
                if (sorting["ID"] == "asc")
                {
                    Notifications = Notifications.OrderBy(c => c.NotificationID);
                }
                else { Notifications = Notifications.OrderByDescending(c => c.NotificationID); }
            }

            var data = Mapper.Map<IEnumerable<NotificationUser>, IEnumerable<NotificationUserModel>>(Notifications);
            return data;
        }

        public List<NotificationModel> SelectByUserId(int iUserId, int skip, int take, out int total)
        {
            List<NotificationModel> notificationModelList = new List<NotificationModel>();
            var lst = _NotificationUserRepository.Get(nu => nu.AppUserID == iUserId).Select(nu => new {notification= nu.Notification,isSeen=nu.IsSeen }).OrderByDescending(o => o.notification.CreateDate).Skip(skip).Take(take).ToList();

            total = _NotificationUserRepository.Get(nu => nu.AppUserID == iUserId).Count();
          
            foreach (var item in lst)
            {
                var obj = Mapper.Map<Notification, NotificationModel>(item.notification);
                obj.IsSeen = item.isSeen;
                notificationModelList.Add(obj);
            }
            return notificationModelList;

        }


        public int Save(NotificationModel model)
        {
            var data = Mapper.Map<NotificationModel, Notification>(model);
            _NotificationRepository.Save(data);
            _unitOfWork.Submit();
            return (int)data.ID;
        }

        /// <summary>
        /// Saves the notification Then sets the notification to a specific user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userID">The user to receive the notification</param>
        /// <returns></returns>
        public int Save(NotificationModel model, int userID)
        {
            int notificationID = this.Save(model);
            if (notificationID > 0)
            {
                NotificationUserModel notificationUserModel = new NotificationUserModel()
                {
                    NotificationID = notificationID,
                    AppUserID = userID,
                    IsSeen = false,

                };

                var notificationUser = Mapper.Map<NotificationUserModel,NotificationUser>(notificationUserModel);
                _NotificationUserRepository.Save(notificationUser);
                _unitOfWork.Submit();
            }
            return notificationID;
        }

        /// <summary>
        /// Saves the notification then sets the notification to multiple users
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userIDs">list of users to receive the notification</param>
        /// <returns></returns>
        public int Save(NotificationModel model, IEnumerable<int> userIDs)
        {
            int notificationID = this.Save(model);
            if (notificationID > 0)
            {
                foreach (int userID in userIDs)
                {
                    NotificationUserModel notificationUserModel = new NotificationUserModel()
                    {
                        NotificationID = notificationID,
                        AppUserID = userID,
                        IsSeen = false,
                    };

                    var notificationUser = Mapper.Map<NotificationUserModel, NotificationUser>(notificationUserModel);
                    _NotificationUserRepository.Save(notificationUser);
                }

                _unitOfWork.Submit();

            }

            return notificationID;
        }


        //Save in User Notification  After check if Notification already Saved before in previouse method   
        public int SaveInNotificationUser(NotificationUserModel model)
        {
            var data = Mapper.Map<NotificationUserModel, NotificationUser>(model);
            _NotificationUserRepository.Save(data);
            _unitOfWork.Submit();
            return (int)data.ID;
        }



        /// <summary>
        /// Change the status of the notification to "seen"
        /// </summary>
        /// <param name="UserId">The User Id</param>
        /// <param name="NotificationId">The notification Id</param>
        public void SetIsSeen(int userID, int notificationID)
        {
            var obj = _NotificationUserRepository.Get(a => a.AppUserID == userID && a.NotificationID == notificationID).FirstOrDefault();
            obj.IsSeen = true;
            _NotificationUserRepository.Save(obj);
            _unitOfWork.Submit();
        }

        public void SetAllSeenForUser(int userID)
        {
            var lst = _NotificationUserRepository.Get(a => a.AppUserID == userID).ToList();
            foreach (var item in lst)
            {
                item.IsSeen = true;
                _NotificationUserRepository.Save(item);
            }

            _unitOfWork.Submit();
        }

    }
}
