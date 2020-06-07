using DataModel.Enum;
using MobileApplication.DataModel;
using MobileApplication.DataModel.NotificationModels;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace MobileApplication.DataService
{
    public class PushSharpService : BaseService
    {
        public GcmServiceBroker gcmBroker;
        public ApnsServiceBroker apnsBroker;

        public PushSharpService()
        {
            ////GCM Configuration   360398778620    AIzaSyDIhMdVOY_LO0GoiP8kX3_N8D_YxtuLcEE
            //var config = new GcmConfiguration(QvLib.QVUtil.AppSetting.GetAppSetting("FCM_SENDER_ID"), QvLib.QVUtil.AppSetting.GetAppSetting("FCM_TOKEN_KEY"), null);
            //config.GcmUrl = "https://fcm.googleapis.com/fcm/send";

            //// Create a new broker
            // gcmBroker = new GcmServiceBroker(config);

            ////APNS Configuration
            // var applecert = File.ReadAllBytes(HttpContext.Current.Server.MapPath(QvLib.QVUtil.AppSetting.GetAppSetting("APNS_Cert_File")));
            // var apnsConfig = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
            // applecert, "111@admin");

            // // Create a new broker
            // apnsBroker = new ApnsServiceBroker(apnsConfig);
        }

        public void InitializeGcmBroker()
        {
            //GCM Configuration   360398778620    AIzaSyDIhMdVOY_LO0GoiP8kX3_N8D_YxtuLcEE
            var config = new GcmConfiguration(QvLib.QVUtil.AppSetting.GetAppSetting("FCM_SENDER_ID"), QvLib.QVUtil.AppSetting.GetAppSetting("FCM_TOKEN_KEY"), null);
            config.GcmUrl = "https://fcm.googleapis.com/fcm/send";
            // Create a new broker
            gcmBroker = new GcmServiceBroker(config);
        }

        /// <summary>
        /// to wire the success and failure events with the Gcm broker
        /// </summary>
        public void WireUpGCMEvents ()
        {
            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is GcmNotificationException)
                    {
                        Debugger.Break();
                        var notificationException = (GcmNotificationException)ex;

                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;

                        new PushNotificationLogService().Insert(new PushNotificationLogModel()
                        {
                            CreateDate=DateTime.Now,
                            NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(notificationException.Notification),
                            IsSuccess=false,
                            Error = notificationException.ToString(),
                        });

                        //    Console.WriteLine ($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        Debugger.Break();
                        var multicastException = (GcmMulticastResultException)ex;

                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            //     Console.WriteLine ($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
                        }

                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var successNotification = failedKvp.Key;
                            var notificationException = failedKvp.Value;

                            new PushNotificationLogService().Insert(new PushNotificationLogModel()
                            {
                                CreateDate = DateTime.Now,
                                NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(successNotification.Notification),
                                IsSuccess = false,
                                Error = notificationException.ToString(),
                            });

                            //          Console.WriteLine ($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
                        }

                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        //Debugger.Break();
                        var expiredException = (DeviceSubscriptionExpiredException)ex;

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        //   Console.WriteLine ($"Device RegistrationId Expired: {oldId}");

                        new PushNotificationLogService().Insert(new PushNotificationLogModel()
                        {
                            CreateDate = DateTime.Now,
                         NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(expiredException.Notification),
                            IsSuccess = false,
                            Error = ex.ToString()
                        });

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                          //  Debugger.Break();
                            // If this value isn't null, our subscription changed and we should update our database
                            //  Console.WriteLine ($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException)
                    {

                        var retryException = (RetryAfterException)ex;
                        new PushNotificationLogService().Insert(new PushNotificationLogModel()
                        {
                            CreateDate = DateTime.Now,
                            NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(retryException.Notification),
                            IsSuccess = false,
                            Error = ex.ToString()
                        });

                        //Debugger.Break();
                        
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        //   Console.WriteLine ($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        var unknownException = (NotificationException)ex;
                        new PushNotificationLogService().Insert(new PushNotificationLogModel()
                        {
                            CreateDate = DateTime.Now,
                            NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(unknownException.Notification),
                            IsSuccess = false,
                            Error = ex.ToString()
                        });
                        //Debugger.Break();
                        //Console.WriteLine("GCM Notification Failed for some unknown reason");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            gcmBroker.OnNotificationSucceeded += (notification) =>
            {
                new PushNotificationLogService().Insert(new PushNotificationLogModel()
                {
                    CreateDate = DateTime.Now,
                    NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(notification),
                    IsSuccess = true,
                    
                });
                //Console.WriteLine("GCM Notification Sent!");
            };
        }

        /// <summary>
        /// Queue notification to android GCM reigsteration ids
        /// </summary>
        /// <param name="registerationID"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public void QueueGcmNotification(List<string> registerationIDs, string message, string title, int? unSeenNotificationsCount, Dictionary<string, object> addedData, List<NotificationActionModel> actions, bool withAlert = true)
        {
            //set notification data
            var data = new Dictionary<string, object>();
            if (withAlert)
            {
                data.Add("message", message);
                data.Add("title", title);
                data.Add("soundName", "ringtone");
            }
            // if no count sent don't send the badge number
            if (unSeenNotificationsCount != null)
                data.Add("count", unSeenNotificationsCount.ToString());

            //to fire on notification event while the application on background
            data.Add("content-available", "1");

            //add action buttons if exist
            if (actions != null)
            {
                //object[] x ={ new { icon = "emailGuests", title = "ACCEPT", callback = "window.acceptCallbackName", foreground = false},
                //        new{ icon = "snooze", title = "REJECT", callback = "window.rejectCallbackName", foreground = true}
                //        };

                data.Add("actions", actions);
            }
            //additional data to send with notification
            if (addedData != null)
            {
                foreach (var item in addedData)
                {
                    if (data.ContainsKey(item.Key))//update the value to last one
                    {
                        data[item.Key] = item.Value;
                    }
                    else//add it
                    {
                        data.Add(item.Key, item.Value);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(data);

            foreach (var regId in registerationIDs)
            {
                // Queue a notification to send
                gcmBroker.QueueNotification(new GcmNotification
                {
                    RegistrationIds = new List<string> { regId },
                    Data = JObject.Parse(jsonString)

                });
            }
        }

        /// <summary>
        /// Process the notification queue
        /// notificationsCount: if null don't updare badge
        /// addedData: if null, no extra data other than title and message sent with the notification
        /// actions: for notification actions if not sent, we don;t have actions in notification
        /// withAlert: if false then send sient notification to the application with no alert to the user
        /// </summary>
        public void SendFCMNotification(List<string> registerationIDs, string message, string title, int? notificationsCount, Dictionary<string, object> addedData,bool withAlert=true)
        {
            SendFCMNotification(registerationIDs, message, title, notificationsCount, addedData, null, withAlert);

        }
        public void SendFCMNotification(List<string> registerationIDs, string message, string title, int? notificationsCount, Dictionary<string, object> addedData, List<NotificationActionModel> actions, bool withAlert = true)
        {
            //initialize the broker
            InitializeGcmBroker();
            //Wire Up Events
            WireUpGCMEvents();

            // Start the broker
            gcmBroker.Start();

            //prepare data 
            QueueGcmNotification(registerationIDs, message, title, notificationsCount, addedData, actions, withAlert);

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            gcmBroker.Stop();

        }

        public void InitializeAPNSBroker(int receiverType)
        {
            CompilationSection compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");
            bool isDebugEnabled = compilationSection.Debug;
            string certificateFile="";
            ApnsConfiguration.ApnsServerEnvironment apnServerEnvironmentMode;

            //debug mode
            if (isDebugEnabled)
            {
                apnServerEnvironmentMode = ApnsConfiguration.ApnsServerEnvironment.Sandbox;
                if(receiverType==(int)ReceiverTypeEnum.Driver)
                {
                    certificateFile = QvLib.QVUtil.AppSetting.GetAppSetting("Development_APNS_Driver_Cert_File");
                }
                else if (receiverType==(int)ReceiverTypeEnum.Merchant)
                {
                    certificateFile = QvLib.QVUtil.AppSetting.GetAppSetting("Development_APNS_Merchant_Cert_File");
                }
            }
                //production mode
            else
            {
                apnServerEnvironmentMode = ApnsConfiguration.ApnsServerEnvironment.Production;
                if (receiverType == (int)ReceiverTypeEnum.Driver)
                {
                    certificateFile = QvLib.QVUtil.AppSetting.GetAppSetting("Production_APNS_Driver_Cert_File");
                }
                else if (receiverType == (int)ReceiverTypeEnum.Merchant)
                {
                    certificateFile = QvLib.QVUtil.AppSetting.GetAppSetting("Production_APNS_Merchant_Cert_File");
                }
            }

            //APNS Configuration
            var applecert = File.ReadAllBytes(HttpContext.Current.Server.MapPath(certificateFile));
            var apnsConfig = new ApnsConfiguration(apnServerEnvironmentMode,
            applecert, "111@admin");

            // Create a new broker
            apnsBroker = new ApnsServiceBroker(apnsConfig);
        }

        /// <summary>
        /// to wire the success and failure events with the APNS broker
        /// </summary>
        public void WireUpAPNSEvents()
        {
            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        //Debugger.Break();
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        // Console.WriteLine ($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                        

                    }
                    else
                    {
                       // Debugger.Break();
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        //   Console.WriteLine ($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    new PushNotificationLogService().Insert(new PushNotificationLogModel()
                    {
                        CreateDate = DateTime.Now,
                        NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(notification),
                        IsSuccess = false,
                        Error = ex.ToString(),

                    });
                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                //Debugger.Break();
                // Console.WriteLine ("Apple Notification Sent!");

                new PushNotificationLogService().Insert(new PushNotificationLogModel()
                {
                    CreateDate = DateTime.Now,
                    NotificationPayload = Newtonsoft.Json.JsonConvert.SerializeObject(notification),
                    IsSuccess = true,

                });

            };
        }

        /// <summary>
        /// Queue notification to android APNS reigsteration ids
        /// </summary>
        /// <param name="registerationID"></param>
        /// <param name="message"></param>
        public void QueueAPNSNotification(List<string> registerationIDs, string message, int? unSeenNotificationsCount, Dictionary<string, object> addedData,bool withAlert=true)
        {
            //set notification data
            var data = new Dictionary<string, object>();
            if (withAlert)
            {
                data.Add("alert", message);
                data.Add("sound", "default");
            }
            // if no count sent don't send the badge number
            if (unSeenNotificationsCount != null)
                data.Add("badge", unSeenNotificationsCount.ToString());

            //to fire on notification event while the application on background
            data.Add("content-available", "1");

            //additional data to send with notification
            if (addedData != null)
            {
                foreach (var item in addedData)
                {
                    if (data.ContainsKey(item.Key))//update the value to last one
                    {
                        data[item.Key] = item.Value;
                    }
                    else//add it
                    {
                        data.Add(item.Key, item.Value);
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(data);

            
                 //List<string> ids=new List<string>();
                 //ids.Add("517c7ece787c198abc4224e01b023fff39b55351a775dc9390de425894577354");
                 foreach (var deviceToken in registerationIDs)
                 {
                // Queue a notification to send
                apnsBroker.QueueNotification (new ApnsNotification {
                    DeviceToken = deviceToken,
                   // Payload = JObject.Parse ("{\"aps\":{ \"alert\": \"Hello World\",\"sound\": \"default\",\"badge\":7}}")
                    Payload = JObject.Parse("{\"aps\":"+jsonString+"}")
                });
            }

        }

        /// <summary>
        /// Process the notification queue
        ///     /// notificationsCount: if null don't updare badge
        /// addedData: if null, no extra data other than title and message sent with the notification
        /// withAlert: if false then send sient notification to the application with no alert to the user
        /// </summary>
        public void SendAPNSNotification(List<string> registerationIDs, string message, string title, int? notificationsCount, Dictionary<string, object> addedData,int receiverType, bool withAlert = true)
        {
            //initialize the broker
            InitializeAPNSBroker(receiverType);
            //Wire Up Events
            WireUpAPNSEvents();

            // Start the broker
            apnsBroker.Start();

            //prepare data 
            QueueAPNSNotification(registerationIDs, message, notificationsCount,addedData,withAlert);

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();

        }

    }
}
