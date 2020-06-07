using Repository;
using MobileApplication.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MobileApplication.DataModel;

namespace MobileApplication.DataService
{
   public class NotificationUserService:BaseService
    {
        private  Repository<NotificationUser> _NotificationUserRepository;

        public NotificationUserService()
       {
           Initialize();
       }

        public NotificationUserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            Initialize();
        }

        private void Initialize()
        {
            _NotificationUserRepository = new Repository<NotificationUser>(_unitOfWork);
        }

       public IEnumerable<NotificationUser> NotificationUserList
       {
           get { return _NotificationUserRepository.GetList(); }
       }

       /// <summary>
       /// Set is seen true
       /// </summary>
       /// <param name="iNotificationUserId"></param>
       /// <returns></returns>
       public int SetIsSeen(int? iUserId, int? iNotificationId)
       {
           int Return = 0;

           NotificationUser objnotification = NotificationUserList.FirstOrDefault(i => i.NotificationID == iNotificationId && i.AppUserID == iUserId);
           objnotification.IsSeen = true;
           _NotificationUserRepository.Save(objnotification);
           _unitOfWork.Submit();

           //_NotificationUserRepository.CommitChanges();
           Return = objnotification.ID;
           return Return;
       }

       /// <summary>
       /// Set all notificatioin IsSeen true for this user
       /// </summary>
       /// <param name="iUserId"></param>
       /// <returns></returns>
       public int SetIsSeenForAll(int? iUserId)
       {

           int Return = 0;
           var objnotification = NotificationUserList.Where(i => i.AppUserID == iUserId);
               foreach (var item in objnotification)
               {
                   item.IsSeen = true;
                   _NotificationUserRepository.Save(item);
               }
               Return = _unitOfWork.Submit();
               // _NotificationUserRepository.CommitChanges();
         return Return;
       }

       public int Insert(int iNotificationId, int iUserId, bool bIsSeen)
       {
           int Return = 0;
         
               NotificationUser objnotificationuser = new NotificationUser();
               objnotificationuser.NotificationID = iNotificationId;
               objnotificationuser.AppUserID = iUserId;
               objnotificationuser.IsSeen = bIsSeen;
               _NotificationUserRepository.Save(objnotificationuser);
               _unitOfWork.Submit();

             //  _NotificationUserRepository.CommitChanges();
               Return = objnotificationuser.ID;

           
           return Return;
       }


       /// <summary>
       /// Select some notifications for the specified user id
       /// </summary>
       /// <param name="iUserId">the userid to get norificaitons for</param>
       /// <param name="iTake">howmany notitifcations to return</param>
       /// <param name="iSkip">howmany notifications to skip (default is 0)</param>
       /// <returns>List of notificationuser object that matches the requested parameters</returns>
       public IEnumerable<NotificationModel> SelectByUserId(int iUserId, int iTake, int iSkip = 0)
       {
         
           //To load the notification object with the NotificationUser Object
           //var dlo = new System.Data.Linq.DataLoadOptions();
           //dlo.LoadWith<NotificationUser>(p => p.Notification);
           //db.LoadOptions = dlo;

            var data= NotificationUserList.Where(nu => nu.AppUserID == iUserId).OrderByDescending(nu => nu.ID).Skip(iSkip).Take(iTake).ToList();
            return Mapper.Map<IEnumerable<NotificationUser>, IEnumerable<NotificationModel>>(data);
       }

       /// <summary>
       /// Get the Unseen notifications count for a user
       /// </summary>
       /// <param name="iUserId"></param>
       /// <returns></returns>
       public int GetUnseenNotificationsCount(int iUserId)
       {

           return NotificationUserList.Count(nu => nu.AppUserID == iUserId && nu.IsSeen == false);
           
       }
    }
}
