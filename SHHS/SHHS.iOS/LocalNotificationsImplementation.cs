﻿using System;
using System.Linq;
using Foundation;
using UIKit;
using UserNotifications;
using Plugin.LocalNotifications;
using Plugin.LocalNotifications.Abstractions;


[assembly: Xamarin.Forms.Dependency(typeof(LocalNotificationsImplementation))]
namespace Plugin.LocalNotifications
{
    /// <summary>
    /// Local Notifications implementation for iOS
    /// </summary>
    public class LocalNotificationsImplementation : ILocalNotifications
    {
        private const string NotificationKey = "LocalNotificationKey";

        /// <summary>
        /// Show a local notification
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        public void Show(string title, string body, bool hasSound, bool hasVibrate, int id)
        {

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(.1, false);
                ShowUserNotification(title, body, id, hasSound, trigger);
            }
            else
            {
                Show(title, body, id, DateTime.Now, true, true);

            }


        }

        /// <summary>
        /// Show a local notification at a specified time
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        /// <param name="notifyTime">Time to show notification</param>
        public void Show(string title, string body, int id, DateTime notifyTime, bool hasSound, bool hasVibration)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponentsFromDateTime(notifyTime), false);
                ShowUserNotification(title, body, id, hasSound, trigger);
            }
            else
            {
                var notification = new UILocalNotification
                {
                    FireDate = (NSDate)notifyTime,
                    AlertTitle = title,
                    AlertBody = body,
                    UserInfo = NSDictionary.FromObjectAndKey(NSObject.FromObject(id), NSObject.FromObject(NotificationKey)),
                    SoundName = hasSound ? UILocalNotification.DefaultSoundName : null
                    

                };

                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            }
        }

        /// <summary>
        /// Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the notification to cancel</param>
        public void Cancel(int id)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RemovePendingNotificationRequests(new string[] { id.ToString() });
                UNUserNotificationCenter.Current.RemoveDeliveredNotifications(new string[] { id.ToString() });
            }
            else
            {
                var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
                var notification = notifications.Where(n => n.UserInfo.ContainsKey(NSObject.FromObject(NotificationKey)))
                    .FirstOrDefault(n => n.UserInfo[NotificationKey].Equals(NSObject.FromObject(id)));

                if (notification != null)
                {
                    UIApplication.SharedApplication.CancelLocalNotification(notification);
                }

            }
        }

        public void CancelAll() {

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            }
            else
            {

                UIApplication.SharedApplication.CancelAllLocalNotifications();
                

            }


        }


        // Show local notifications using the UNUserNotificationCenter using a notification trigger (iOS 10+ only)
        void ShowUserNotification(string title, string body, int id, bool hasSound, UNNotificationTrigger trigger)
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                return;
            }

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Body = body,
                Sound = hasSound ? UNNotificationSound.Default : null

            };
            
            var request = UNNotificationRequest.FromIdentifier(id.ToString(), content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (error) => { });
        }

        NSDateComponents GetNSDateComponentsFromDateTime(DateTime dateTime)
        {
            return new NSDateComponents
            {
                Month = dateTime.Month,
                Day = dateTime.Day,
                Year = dateTime.Year,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }


    }
}
