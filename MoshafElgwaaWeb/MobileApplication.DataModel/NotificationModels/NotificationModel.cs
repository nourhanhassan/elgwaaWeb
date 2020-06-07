using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.DataModel
{
   public class NotificationModel
   {
       public int ID { get; set; }
       public Nullable<int> NotificationTypeID { get; set; }
       public Nullable<int> FromAppUserID { get; set; }
       public string Title { get; set; }
       public string Message { get; set; }
       public string Link { get; set; }
       public Nullable<System.DateTime> CreateDate { get; set; }

       public string TimePassed
       {
           get
           {
               var currentTime = DateTime.Now;
               var postTime = DateTime.Parse(this.CreateDate.ToString());
               var difference = currentTime - postTime;

               if (difference.Days > 0)
               {
                   return difference.Days.ToString() + " يوم";
               }
               else if (difference.Hours > 0)
               {
                   return difference.Hours.ToString() + "ساعة";
               }
               else if (difference.Minutes > 0)
               {
                   return difference.Minutes.ToString() + "دقيقة";
               }
               else
               {
                   return "الآن";
               }
           }
       }

       public string FromAppUserName { get; set; }
       public string FromAppUserImageUrl { get; set; }

       public bool IsSeen { get; set; }
       //  public int TotalCount { get; set; }

       //public virtual AppUser AppUser { get; set; }
       //public virtual NotificationType NotificationType { get; set; }
       //public virtual ICollection<NotificationUser> NotificationUsers { get; set; }
   }
}
