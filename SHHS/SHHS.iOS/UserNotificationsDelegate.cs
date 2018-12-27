using System;
using ObjCRuntime;
using UserNotifications;


namespace SHHS.iOS
{
    public class UserNotificationsDelegate:UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
            completionHandler(UNNotificationPresentationOptions.Sound);

        }
    }
}
