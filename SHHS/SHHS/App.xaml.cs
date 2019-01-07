using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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


        public App()
        {
            InitializeComponent();
            var userSelectedCulture = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
            shhsMain = new MainPage();
            shhsCalender = new CalenderPage { Title = "Calendar", Icon = "calendar.png" };
            MainPage = shhsMain;
            shhsMain.Children.Add(shhsCalender);
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
    }
}
