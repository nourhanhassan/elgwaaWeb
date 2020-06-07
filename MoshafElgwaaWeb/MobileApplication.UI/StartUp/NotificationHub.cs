using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MobileApplication.DataService;
using MobileApplication.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

/// <summary>
/// Summary description for NotifcicationHub
/// </summary>

public class NotificationClientObject
{
    public string ID { get; set; }
    public string Time { get; set; }
    public string Message { get; set; }
    public string Link { get; set; }
    public bool IsSeen { get; set; }

    public int  NotificationTypeId { get; set; }
    public string TimePassed
    {
        get
        {
            var currentTime = DateTime.Now;
            var notificationTime = DateTime.Parse(this.Time);
            var difference = currentTime - notificationTime;

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
}

public class NotificationHub : Hub, IRequiresSessionState,INotificationHub
{
    private static IHubContext Instance;
    /// <summary>
    /// the instance of hub context enable us to send notification from server side
    /// </summary>
    private IHubContext HubContext
    {
        get
        {
            if (Instance == null)
            {
                Instance = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            }
            return Instance;
        }
    }

    private NotificationService _NotificationService { get; set; }

    static Dictionary<string, List<string>> ConnectionIds = new Dictionary<string, List<string>>();

    public NotificationHub()
    {
        _NotificationService = new NotificationService();
    }

    /// <summary>
    /// on connected ... update connection id
    /// </summary>
    /// <returns></returns>

    public override System.Threading.Tasks.Task OnConnected()
    {
        //If the userid is already connected (i.e still in the ConnectionIds dictionary), then update his connection id with the new connection id.
        if (ConnectionIds.ContainsKey(Context.QueryString["UserID"]))
        {
            ConnectionIds[Context.QueryString["UserID"]].Add(Context.ConnectionId);
        }
        else //If the userid is not contained in the dictionary, then insert a new record.
        {
            ConnectionIds[Context.QueryString["UserID"]] = new List<string>();
            ConnectionIds[Context.QueryString["UserID"]].Add(Context.ConnectionId);
        }
        return base.OnConnected();

    }



    /// <summary>
    /// On Disconnected, remove the user id from the connectionids pool
    /// </summary>
    /// <param name="stopCalled"></param>
    /// <returns></returns>
    public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
    {
        string userID = Context.QueryString["UserID"];
        if (ConnectionIds.ContainsKey(userID))
        {
            if (ConnectionIds[userID].Contains(Context.ConnectionId))
            {
                ConnectionIds[userID].Remove(Context.ConnectionId);
            }
        }

        return base.OnDisconnected(stopCalled);
    }



    /// <summary>
    /// on OnReconnected ... update connection id
    /// </summary>
    /// <returns></returns>

    public override System.Threading.Tasks.Task OnReconnected()
    {

        //ConnectionId.Add(Context.ConnectionId, Context.QueryString["qvUserId"]);
        return base.OnReconnected();
    }

    /////////////////////////////////////////////////////////// start signalr website Methods

    public void UpdateNotificationCount(int count, string clintID)
    {


        if (ConnectionIds.ContainsKey(clintID))
        {
            foreach (var i in ConnectionIds[clintID])
            {
                HubContext.Clients.Client(i).receiveNotificationCount(count);
            }

        }
    }

    ///////////////////////////////////////////////////////////end signalr website Methods

    /// <summary>
    /// Change the status of the notification to "seen"
    /// </summary>
    /// <param name="UserId">The User Id</param>
    /// <param name="NotificationId">The notification Id</param>
    public void SetIsSeen(int UserId, int NotificationId)
    {

        _NotificationService.SetIsSeen(UserId, NotificationId);
        if (ConnectionIds.ContainsKey(UserId.ToString()))
        {
            foreach (var i in ConnectionIds[UserId.ToString()])
            {
                HubContext.Clients.Client(i).applyIsSeen(NotificationId);
            }

        }

    }

    public void SeeAllNotifcation(int UserId)
    {
        _NotificationService.SetAllSeenForUser(UserId);
        if (ConnectionIds.ContainsKey(UserId.ToString()))
        {
            foreach (var i in ConnectionIds[UserId.ToString()])
            {
                HubContext.Clients.Client(i).applyAllIsSeen();
            }

        }
    }
    /// <summary>
    /// Send notification to a specific userid
    /// </summary>
    /// <param name="iUserID"> the user id that needs to receive the notification</param>
    /// <param name="NotiType">the notification type</param>
    /// <param name="Message">The notification message</param>
    /// <param name="Link">the url of the link to go to</param>
    public void SendNotificationToUser(NotificationModel notification, int userID)
    {

        int notificationID = _NotificationService.Save(notification, userID);

        var objectToSend = new NotificationClientObject()
        {
            ID = notificationID.ToString(),
            Time = notification.CreateDate.ToString(),
            Link = notification.Link,
            Message = notification.Message
        };


        if (ConnectionIds.ContainsKey(userID.ToString()))
        {
            foreach (var connectionID in ConnectionIds[userID.ToString()])
            {
                HubContext.Clients.Client(connectionID).receiveNotification(objectToSend);
            }
        }
    }

    /// <summary>
    /// Sends notification to multiple users
    /// </summary>
    /// <param name="iUserIDs">Array of the userids to receive the notification</param>
    /// <param name="NotiType"></param>
    /// <param name="Message"></param>
    /// <param name="Link"></param>
    public void SendNotificationToUser(NotificationModel notification, int[] userIDs)
    {
        int notificationID = _NotificationService.Save(notification, userIDs);

        var objectToSend = new NotificationClientObject()
        {
            ID = notificationID.ToString(),
            Time = DateTime.Now.ToString(),
            Link = notification.Link,
            Message = notification.Message
        };

        List<string> recipientConnectionIDs = new List<string>();

        foreach (int userID in userIDs)
        {
            if (ConnectionIds.ContainsKey(userID.ToString()))
            {
                recipientConnectionIDs.AddRange(ConnectionIds[userID.ToString()]);
            }

        }

        HubContext.Clients.Clients(recipientConnectionIDs).receiveNotification(objectToSend);



    }
    
}