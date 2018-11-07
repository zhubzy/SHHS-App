using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SHHS.Controller;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SHHS
{
    public partial class App : Application
    {

        MainPage shhsMain;
        
        public App()
        {
            InitializeComponent();
            shhsMain = new MainPage();
            MainPage = shhsMain;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

            // Handle when your app resumes
            shhsMain.RefreshSchedule();
        }
    }
}
