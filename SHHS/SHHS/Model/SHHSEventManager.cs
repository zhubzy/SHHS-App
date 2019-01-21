using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using SQLite;
using Xamarin.Forms;

namespace SHHS.Model
{
    public class SHHSEventManager
    {

        private SQLiteAsyncConnection _connection;
        public ObservableCollection<SHHSEvent> events;

        public SHHSEventManager()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db");
            _connection = new SQLiteAsyncConnection(databasePath);


        }
        public async Task InitalizeEventTable()
        {

            await _connection.CreateTableAsync<SHHSEvent>();
            var data = await _connection.Table<SHHSEvent>().ToListAsync();
            events = new ObservableCollection<SHHSEvent>(data);
            events.CollectionChanged += Events_CollectionChanged;

            await GetEvents();
        }

        public async Task AddEvent(SHHSEvent e) {

            await _connection.InsertAsync(e);
            events.Add(e);
        }

        public async Task RemoveEvent(SHHSEvent e)
        {

            await _connection.DeleteAsync(e);
            events.Remove(e);

        }
        public async Task GetEvents()
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

            try
            {
                var annoucements = await firebase.Child("South Hills Events").OnceAsync<SHHSEvent>();
                foreach (var e in annoucements)
                {

                    e.Object.StartDate = DateTime.Parse(e.Object.DateString);
                    e.Object.StartTime = TimeSpan.Parse(e.Object.StartTimeString);
                    e.Object.EndTime = TimeSpan.Parse(e.Object.EndTimeString);



                    DateTime startDate = new DateTime(e.Object.StartDate.Year, e.Object.StartDate.Month, e.Object.StartDate.Day, e.Object.StartTime.Hours, e.Object.StartTime.Minutes, e.Object.StartTime.Seconds);
                    DateTime endDate = new DateTime(e.Object.StartDate.Year, e.Object.StartDate.Month, e.Object.StartDate.Day, e.Object.EndTime.Hours, e.Object.EndTime.Minutes, e.Object.EndTime.Seconds);
                    e.Object.LocationText = startDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US"));
                    if (e.Object.StartTime.Equals(e.Object.EndTime))
                    {
                        e.Object.Time = "All Day";
                    }
                    else
                    {
                        e.Object.Time = "" + e.Object.StartDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US")) + " - " + e.Object.EndDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US"));
                    }

                    if (!string.IsNullOrEmpty(e.Object.Location))
                    {
                        e.Object.LocationText += "\n@ " + e.Object.Location;
                    }
                    events.Add(e.Object);
                }


            }
            catch (FirebaseException e)
            {



                Console.WriteLine(e.StackTrace);


            }

        }

        public async Task RefreshEvent()
        {

            foreach (var a in events)
            {

                DateTime startDate = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, a.StartTime.Hours, a.StartTime.Minutes, a.StartTime.Seconds);
                DateTime endDate = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, a.EndTime.Hours, a.EndTime.Minutes, a.EndTime.Seconds);
                string timeDifferencialS = DifferencialInString(startDate, DateTime.Now);
                string timeDifferencialE = DifferencialInString(endDate, DateTime.Now);

                //Event hasn't begin yet
                if (startDate.Subtract(DateTime.Now).TotalSeconds > 0)
                {
                    a.DaysLeft = "In " + timeDifferencialS;
                }

                else

                {

                    //Event has past or in between
                    if (endDate.Subtract(DateTime.Now).TotalSeconds < 0)
                    {
                        a.DaysLeft = timeDifferencialE + " ago";

                    }
                    else
                    {
                        //Event hasn't ended yet, use remaining time
                        a.DaysLeft = timeDifferencialE + " remaining";

                    }
                }

                if (a.Time.Equals("All Day") && DateTime.Today.Equals(a.StartDate))
                    a.DaysLeft = "Today";

                await _connection.UpdateAsync(a);

            }
            ObservableCollection<SHHSEvent> eventSorted = new ObservableCollection<SHHSEvent>(
                events.OrderBy(person => person)
            );


            foreach (var a in eventSorted)
            {

                Console.WriteLine(a.StartDate);

            }

            events = eventSorted;
            ((App)Application.Current).shhsCalender.SetDataSource(events);
            ((App)Application.Current).shhsCalender.RefreshDate();



        }

        public int FindEvent(DateTime a)
        {
            int times = 0;

            foreach (var e in events)
            {
                if (e.StartDate.Equals(a))
                    times++;
            }

            return times;
        }


      





        void Events_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) { 


            }

        }



        public String DifferencialInString(DateTime dateA, DateTime dateB)
        {


            //Total difference will be negative if dateB occurs after dateA
            //Total difference will be positive if dataB occurs before dateA
            //For one time events 
            int daysDifference = (int)Math.Abs(dateA.Subtract(dateB).Days);
            int hoursDifference = (int)Math.Abs(dateA.Subtract(dateB).Hours);
            int minsDifference = (int)Math.Abs(dateA.Subtract(dateB).Minutes);

            String timeDifferencial = "";

            //TODO: Simplify into method

            if (daysDifference >= 365)
            {

                double totalYears = daysDifference * 1.0 / 365.0;
                int years = daysDifference / 365;
                if ( totalYears - years >= 0.5)
                {

                    timeDifferencial += years + 1;

                }


                else
                {

                    timeDifferencial += years;

                }
                timeDifferencial += " years";

            }

            else if (daysDifference > 0)
            {


                if ( Math.Abs(dateA.Subtract(dateB).TotalDays) - daysDifference >= 0.75)
                {

                    timeDifferencial += daysDifference + 1;

                }

                else if (Math.Abs(dateA.Subtract(dateB).TotalDays) - daysDifference >= 0.25)
                {

                    timeDifferencial += daysDifference + " 1/2";


                }
                else
                {

                    timeDifferencial += daysDifference;

                }


                timeDifferencial += " days";
            }
            else if (hoursDifference == 0)
            {

                timeDifferencial = "" + minsDifference + " mins";
            }
            else if (daysDifference == 0)
            {
                timeDifferencial = "" + hoursDifference + " hours";
            }
            else
            {
                timeDifferencial = "Error";
            }




            return timeDifferencial;
        }
    

    }
}
