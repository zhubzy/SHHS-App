using System;
using Foundation;
using SHHS.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]
namespace SHHS.iOS
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string ApplicationsPublicVersion { get; set; }
        public string ApplicationsPrivateVersion { get; set; }

        public AppVersionProvider()
        {
            ApplicationsPublicVersion = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleShortVersionString")].ToString();
            ApplicationsPrivateVersion = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
        }
    }
   
}
