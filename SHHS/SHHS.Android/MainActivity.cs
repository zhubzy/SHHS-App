using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Plugin.LocalNotifications;
using Plugin.CurrentActivity;

namespace SHHS.Droid
{
    [Activity(Label = "SHHS", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();

            CrossCurrentActivity.Current.Activity = this;
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.plugin_lc_smallicon;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }
}