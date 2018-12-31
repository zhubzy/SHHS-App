﻿
using System;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using Foundation;
using UIKit;
using UserNotifications;

namespace SHHS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            UITabBar.Appearance.TintColor = UIColor.White;
            CarouselViewRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                        UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                        (approved, error) => { });

                UNUserNotificationCenter.Current.Delegate = new UserNotificationsDelegate();
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                        new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            return base.FinishedLaunching(app, options);
        }

        [Export("application:supportedInterfaceOrientationsForWindow:")] 
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, IntPtr forWindow) 
        { 
        return Plugin.DeviceOrientation.DeviceOrientationImplementation.SupportedInterfaceOrientations;
         }
    }
}
