using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SHHS.Controller;
using System.Globalization;
using System.Threading;
using SQLite;
using System.IO;
using SHHS.Model;
using System.Collections.ObjectModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SHHS
{
    public partial class App : Application
    {

        MainPage shhsMain;
        CalenderPage shhsCalender;
        private SQLiteAsyncConnection _connection;
        public ObservableCollection<SHHSEvent> events;

   
        public App()
        {
            InitializeComponent();
            var userSelectedCulture = new CultureInfo("en-US");

            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
            shhsMain = new MainPage();
            shhsCalender = new CalenderPage { Title = "Calendar", Icon = "calendar.png" };
            MainPage = shhsMain;
            shhsMain.Children.Add(shhsCalender);
            Current = this;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");
            _connection = new SQLiteAsyncConnection(databasePath);
            

        }

        override protected async void OnStart()
        {
            // Handle when your app starts
            await _connection.CreateTableAsync<SHHSEvent>();
            var data = await _connection.Table<SHHSEvent>().ToListAsync();
            events = new ObservableCollection<SHHSEvent>(data);
            shhsCalender.SetDataSource(events);

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
