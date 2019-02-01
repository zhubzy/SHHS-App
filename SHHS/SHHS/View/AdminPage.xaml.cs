using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using SHHS.Model;
using Xamarin.Forms;

namespace SHHS.View
{
    public partial class AdminPage : ContentPage
    {

        List<SHHSDateTime> dateTimes;
        SHHSScheduleExceptionManager exceptionManager;
        public List<String> ScheduleNames { get; set; }


        async protected override void OnAppearing()
        {
            base.OnAppearing();
            await GetScheduleException();
            await RefreshData();




            for (int i = 0; i < 30; i++)
            {


                SHHSDateTime date = new SHHSDateTime(DateTime.Today.AddDays(i));
                if (!(date.Date.DayOfWeek == DayOfWeek.Saturday || date.Date.DayOfWeek == DayOfWeek.Sunday))
                {
                    foreach (var scheudleName in exceptionManager.ScheduleExceptionLists.Keys)
                    {
                        date.CurrentSchedule = "Regular Schedule";
                        if (date.Date.Equals(DateTime.Parse(scheudleName)))
                        {
                            date.CurrentSchedule = exceptionManager.ScheduleExceptionLists[scheudleName];
                            break;
                        }
                    }

                    dateTimes.Add(date);
                }



            }
            ScheduleExcpetionList.ItemsSource = dateTimes;


        }


        public AdminPage()
        {
            InitializeComponent();
            dateTimes = new List<SHHSDateTime>();
            ScheduleNames = new List<string>();
            exceptionManager = new SHHSScheduleExceptionManager();
        }
        public async Task RefreshData()
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");
            Console.WriteLine("Before");
            // add new item 
            try
            {

                var schedules = await firebase.Child("South Hills Schedule").OnceAsync<SHHSSchedule>();
                ScheduleNames.Clear();
                foreach (var s in schedules)
                {
                    ScheduleNames.Add(s.Object.ScheduleName);

                }
                ScheduleNames.Add("No School");
            }
            catch (FirebaseException e)
            {


                Console.WriteLine(e.StackTrace);

            }
            // add new item to list of data and let the client generate new key for you (done offline)

            Console.WriteLine("After");




        }
        public async Task<bool> GetScheduleException()
        {
            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");
            try
            {

                var exceptions = await firebase.Child("South Hills Schedule Exception").OnceAsync<SHHSScheduleExceptionManager>();
                foreach (var e in exceptions)
                {
                    exceptionManager = e.Object;
                }
                return true;
            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            var firebase = (Application.Current as App).Client;
            try
            {

                await firebase.Child("South Hills Schedule Exception").Child("Exception").PutAsync(new SHHSScheduleExceptionManager { Offset = 0, ScheduleExceptionLists = exceptionManager.ScheduleExceptionLists });

            }
            catch (FirebaseException error)
            {
                Console.WriteLine(error.StackTrace);
            }

            await (Application.Current as App).shhsMain.ReInitMainPage();

        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string dateInString = ((sender as Picker).BindingContext as SHHSDateTime).Date.ToString("yyyy MM dd");
            string name = (sender as Picker).SelectedItem as string;
            exceptionManager.ScheduleExceptionLists[dateInString] = name;
           

        }
    }


    public class SHHSDateTime
    {
        public SHHSDateTime(DateTime d)
        {
            Date = d;
        }
        private string _displayDate;
        public DateTime Date { get; set; }
        public string DisplayDate { get { return Date.ToString("D"); } set { _displayDate = value; } }
        public string CurrentSchedule { get; set; }

    }
}

