using System;
using SHHS.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]

namespace SHHS.Droid
{
    public class AppVersionProvider : IAppVersionProvider
    {
        public string ApplicationsPublicVersion { get; set; }
        public string ApplicationsPrivateVersion { get; set; }

        public AppVersionProvider()
        {
            var context = Android.App.Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);

            ApplicationsPublicVersion = info.VersionName;
            ApplicationsPrivateVersion = info.VersionCode.ToString();
        }
    }
}
