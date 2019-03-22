using System;
using ObjCRuntime;
using UserNotifications;
using SHHS;
using Plugin.LocalNotifications;

namespace SHHS.iOS
{
    public class UserNotificationsDelegate:UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
            completionHandler(UNNotificationPresentationOptions.Sound);

        }



        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            // Take action based on Action ID
            switch (response.ActionIdentifier)
            {

                case "IDYes":
                    Console.WriteLine("Yes");
                    break;
                case "IDNo":
                    (Xamarin.Forms.Application.Current as App).RemindToday = DateTime.Today.ToString();
                    break;
                case "IDInput":
                    (Xamarin.Forms.Application.Current as App).MinutesToSendNotification = (response as UNTextInputNotificationResponse).UserText;
                    break;
                default:
                    // Take action based on identifier
                    if (response.IsDefaultAction)
                    {
                        // Handle default action...
                    }
                    else if (response.IsDismissAction)
                    {
                        // Handle dismiss action
                    }
                    break;
            }

            // Inform caller it has been handled
            completionHandler();
        }
    }


}
