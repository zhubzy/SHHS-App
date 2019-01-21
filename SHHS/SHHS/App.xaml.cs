using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SHHS.View;
using SHHS.Controller;
using System.Globalization;
using System.Threading;

using SHHS.Model;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SHHS
{
    public partial class App : Application
    {

        MainPage shhsMain;
        public CalenderPage shhsCalender;
        public SHHSEventManager shhsEventManager;
        public SettingPage shhsSetting;
        private const string notificationKey = "Notification";
        private const string soundKey = "Sound";
        private const string minutesKey = "Minutes";

     

        public App()
        {
            InitializeComponent();
            var userSelectedCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;

            shhsMain = new MainPage();
            MainPage = new NavigationPage(shhsMain);
            shhsCalender = new CalenderPage { Title = "Calendar", Icon = "calendaricon.png" };
            shhsSetting =  new SettingPage { Title = "Setting", Icon = "setting.png", BackgroundColor = Color.FromHex("#EFFACB") };
            shhsMain.Children.Add(shhsCalender);
            shhsMain.Children.Add(shhsSetting);
            shhsEventManager = new SHHSEventManager();
            Current = this;



        }

        override protected async void OnStart()
        {
            // Handle when your app starts

            await shhsEventManager.InitalizeEventTable();
            shhsCalender.SetDataSource(shhsEventManager.events);
            await shhsEventManager.RefreshEvent();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        override protected async void OnResume()
        {
            // Handle when your app resumes
            shhsMain.RefreshSchedule();
            await shhsEventManager.RefreshEvent();
        }
        public bool NotificationEnabled { 
            get {
                if (Properties.ContainsKey(notificationKey))
                    return (bool)Properties[notificationKey];
                return false;
            }
            set {
                Properties[notificationKey] = value; 
            }
        }
        public bool SoundEnabled
        {

            get
            {

                if (Properties.ContainsKey(soundKey))
                    return (bool)Properties[soundKey];
                return false;

            }

            set
            {

                Properties[soundKey] = value;


            }


        }

        public string MinutesToSendNotification
        {

            get
            {

                if (Properties.ContainsKey(minutesKey))
                    return Properties[minutesKey].ToString();

                return "2";
            }

            set
            {

                Properties[minutesKey] = value;

                if(shhsSetting!= null)
                shhsMain.scheduleManager.PushLocalNotifications();
            }


        }
    }
}
