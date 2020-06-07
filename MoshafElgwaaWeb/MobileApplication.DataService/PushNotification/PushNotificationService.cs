using AutoMapper;
using MomtazExpress.Context;
using MomtazExpress.DataModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomtazExpress.DataService
{

    public class PushNotificationService : BaseService
    {
        private readonly Repository<Notification> _NotificationRepository;
        private readonly Repository<NotificationUser> _NotificationUserRepository;
        PushSharpService _pushSharpService;

        public PushNotificationService()
        {
            _NotificationRepository = new Repository<Notification>(_unitOfWork);
            _NotificationUserRepository = new Repository<NotificationUser>(_unitOfWork);
            _pushSharpService = new PushSharpService();
        }

        public IEnumerable<Notification> NotificationList
        {

            get
            {
                return
                    _NotificationRepository.GetList();
            }
        }

        public IEnumerable<NotificationUser> NotificationUserList
        {

            get
            {
                return
                    _NotificationUserRepository.GetList();
            }
        }

        public object GetUserAllNotifications(int userID, int skip, int take)
        {
            IEnumerable<Notification> notifications = NotificationUserList.Where(x => x.AppUserID == userID).Select(x => x.Notification).OrderByDescending(x => x.ID).Skip(skip).Take(take);
            IEnumerable<NotificationModel> notificationsModel_lst = Mapper.Map<IEnumerable<Notification>, IEnumerable<NotificationModel>>(notifications);

            foreach (var item in notificationsModel_lst)
            {
                item.IsSeen = NotificationUserList.FirstOrDefault(x => x.NotificationID == item.ID).IsSeen;
              //  item.TotalCount = NotificationUserList.Where(x => x.AppUserID == userID).Count();
            }

            int TotalCount = NotificationUserList.Where(x => x.AppUserID == userID).Count();
            object obj = new { TotalCount = TotalCount, notifications = notificationsModel_lst };
            return obj;
          //  return notificationsModel_lst;
        }
        /// <summary>
        /// method to insert the notification in database
        /// one record foe notification table and one record in notificationuser table
        /// </summary>
        /// <param name="NotificationTypeID"></param>
        /// <param name="FromAppUserID">if its value is null then its the case of approving post from control panel</param>
        /// <param name="Message"></param>
        /// <param name="Link">in our case its just postID</param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool InsertNotification(int NotificationTypeID, int? FromAppUserID, string Message, string Link,int ToAppUserID )
        {
            if (FromAppUserID != ToAppUserID)
            {
                Notification newNotif = new Notification()
                {
                    NotificationTypeID = NotificationTypeID,
                    FromAppUserID = FromAppUserID,
                    Message = Message,
                    Link = Link,
                    CreateDate = DateTime.Now
                };
                _NotificationRepository.Save(newNotif);

                NotificationUser newNotifUser = new NotificationUser()
                {
                    Notification = newNotif,
                    AppUserID = ToAppUserID,
                    IsSeen = false
                };
                _NotificationUserRepository.Save(newNotifUser);

                int row = _unitOfWork.Submit();

                if (row > 0)
                {
                  //  var user = new AppUserService().GetAppUserContextByID(ToAppUserID);
                  //  //if the user logged in from 2 devices with different platforms send notif to both
                  //  string GCMID = "";
                  //  string APNID = "";
                  ////  string registrationType="";
                  //  if(!string.IsNullOrEmpty(user.GCMID))
                  //  {
                  //      GCMID = user.GCMID;
                  //     // registrationType="gcm";
                  //  }
                  //  if(!string.IsNullOrEmpty(user.APNID))
                  //  {
                  //      APNID = user.APNID;
                  //    //  registrationType = "apns";
                  //  }
                  //  int userID = user.ID;
                  //  int notificationsCount = UnSeenNotificationsCount(userID);
                  //  if (!string.IsNullOrEmpty(user.GCMID) || !string.IsNullOrEmpty(user.APNID))
                  //  {
                  //      SendNotification(GCMID, APNID, Message, notificationsCount,int.Parse(Link));
                  //  }
                }

                return row > 0;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// if the user logged in from 2 devices with different platforms send notif to both
        /// </summary>
        /// <param name="receiverGCMID"></param>
        /// <param name="receiverAPNID"></param>
        /// <param name="Message"></param>
        /// <param name="notificationsCount"></param>
        /// <returns></returns>
        public bool SendNotification(string receiverGCMID, string receiverAPNID, string Message, int notificationsCount, int postID)
        {
            if (!string.IsNullOrEmpty(receiverGCMID))
            {
                List<string> ids = new List<string>();
                ids.Add(receiverGCMID);
                Dictionary<string, object> appData = new Dictionary<string, object>();
                appData.Add("PostID", postID.ToString());
                _pushSharpService.SendFCMNotification(ids, Message, Message, notificationsCount, appData);
            }
            if (!string.IsNullOrEmpty(receiverAPNID))
            {
                List<string> ids = new List<string>();
                ids.Add(receiverAPNID);
                _pushSharpService.SendAPNSNotification(ids, Message, Message, notificationsCount,null);
            }
            return true;
        }

        public int UnSeenNotificationsCount(int userID)
        {
           return _NotificationUserRepository.GetList().Where(x => x.AppUserID == userID && x.IsSeen == false).Count();
        }

        public bool SetAllNotificationsSeen(int appUserID)
        {
            var userNotifications = _NotificationUserRepository.GetList().Where(x => x.AppUserID == appUserID);
           foreach (var item in userNotifications)
           {
               item.IsSeen = true;
               _NotificationUserRepository.Save(item);
           }
           int affectedRows = _unitOfWork.Submit();
           return affectedRows > 0;
        }

        public bool DeleteNotificationsSentByUser(int appUserID)
        {
            var notificationsSentByUser = _NotificationRepository.Get(n => n.FromAppUserID == appUserID).Select(n=>n.ID).ToList();
            foreach (var notificationID in notificationsSentByUser)
            {
                _NotificationRepository.Delete(notificationID);
            }
            return _unitOfWork.Submit()>0;
        }
    }
}
