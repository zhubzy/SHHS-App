using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotifications;

namespace SHHS.Model
{
    public class SHHSScheduleManager
    {


        public List<SHHSSchedule> ScheduleList { get; set; }
        int CurrentSchedule { get; set; }
        public int ScheduleOfTheDay { get; set; }
        public int MinutesLeft { get; set; }
        public int SecondsLeft { get; set; }
        public int Offset { get; set; }
        public int PeriodLength { get; set; }
        public string ScheduleName { get ; set;}
        public string CurrentMessage { get; set; }
        public Boolean IsActive { get; set; }

        Dictionary<string, string> scheduleExceptions;



        public SHHSScheduleManager()
        {

            ScheduleList = new List<SHHSSchedule>();
            scheduleExceptions = new Dictionary<string, string>();
            
        }


        public async Task<bool> GetScheduleException(){

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

            try{

                var exceptions = await firebase.Child("South Hills Schedule Exception").OnceAsync<SHHSScheduleExceptionManager>();
                foreach (var e in exceptions)
                {

                    scheduleExceptions = e.Object.ScheduleExceptionLists;
                    Offset = e.Object.Offset;

                }

                return true;

            }
            catch (FirebaseException e){
                

                
                Console.WriteLine(e.StackTrace);

                return false;
            }

        }

        public async Task RefreshData()
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");
            Console.WriteLine("Before");
            // add new item 
            try
            {

                var schedules = await firebase.Child("South Hills Schedule").OnceAsync<SHHSSchedule>();
                ScheduleList.RemoveRange(0, ScheduleList.Count);
                foreach (var s in schedules)
                {

                    ScheduleList.Add(s.Object);


                }

                for (int i = 0; i < ScheduleList.Count; i++)
                {
                    for (int s = 0; s < ScheduleList[i].Schedule.Count; s++)
                    {
                        ScheduleList[i].Schedule[s].StartDateTime = DateTime.Parse(ScheduleList[i].Schedule[s].StartTime);
                        ScheduleList[i].Schedule[s].EndDateTime = DateTime.Parse(ScheduleList[i].Schedule[s].EndTime);
                        ScheduleList[i].Schedule[s].Length = CalcTime(ScheduleList[i].Schedule[s].StartDateTime, ScheduleList[i].Schedule[s].EndDateTime);
                    }
                }


                Console.WriteLine($"{ScheduleList[0].Schedule[0]}");
                Console.WriteLine($"{ScheduleList[0].ScheduleName}");


            }
            catch (FirebaseException e)
            {


                Console.WriteLine(e.StackTrace);

            }
            // add new item to list of data and let the client generate new key for you (done offline)

            Console.WriteLine("After");




        }





        private int CalcTime(DateTime x, DateTime y)
        {
           return (y.Hour - x.Hour) * 3600 + (y.Minute - x.Minute) * 60 + (y.Second - x.Second);

     

        }

        private int FindCurrentSchedule(DateTime currentTime, List<SHHSPeriod> datetimes)
        {
            int[] difference = new int[datetimes.Count];

            for (int i = 0; i < datetimes.Count; i++)
            {
                difference[i] = CalcTime(datetimes[i].EndDateTime, currentTime);

                //Console.WriteLine(difference[i]);
            }

            int min = difference[0];
            int index = 0;
            for (int ctr = 1; ctr < difference.Length; ctr++)
            {
                if (difference[ctr] < min && min >= 0)
                    min = difference[ctr];

                index = Array.IndexOf(difference, min);

            }


            //Return the closest datetime to the current time
            return index;

        }

        private void TimeTilEndOfPeriod(DateTime now, SHHSSchedule schedule)
        {
            DateTime closestPeriodEnd = schedule.Schedule[CurrentSchedule].EndDateTime;
            DateTime closestPeriodStart = schedule.Schedule[CurrentSchedule].StartDateTime;

            //IF the peiord occurs after the current time
            if (closestPeriodStart.Hour * 60 + closestPeriodStart.Minute > now.Hour * 60 + now.Minute)
            {
                MinutesLeft = CalcTime(now, closestPeriodStart) / 60;
                SecondsLeft = CalcTime(now, closestPeriodStart) % 60;

                if(CurrentSchedule != 0){
                PeriodLength = CalcTime(schedule.Schedule[CurrentSchedule - 1].EndDateTime, closestPeriodStart) ;
                } else {
                PeriodLength = CalcTime(DateTime.Today, closestPeriodStart);
                }
                IsActive = true;
                CurrentMessage = "Before " + schedule.Schedule[CurrentSchedule].PeriodName + " Starts";

            } else if (closestPeriodEnd.Hour * 60 + closestPeriodEnd.Minute >= now.Hour * 60 + now.Minute) {

                MinutesLeft = CalcTime(now, closestPeriodEnd) / 60;
                SecondsLeft = CalcTime(now, closestPeriodEnd) % 60;
                PeriodLength = schedule.Schedule[CurrentSchedule].Length;
                IsActive = true;
                CurrentMessage = "Left Of " + schedule.Schedule[CurrentSchedule].PeriodName;


            }
            else
            {
                IsActive = false;
                MinutesLeft = 0;
                SecondsLeft = 0;
                CurrentMessage = "Class is Over";

            }


        }

        public void GetTimeUntilEndOfPeriod(){

            ScheduleOfTheDay = FindScheduleType();

            if(ScheduleOfTheDay == -1){
                    IsActive = false;
                    MinutesLeft = 0;
                    SecondsLeft = 0;
                    CurrentMessage = "Class is Over";
                    ScheduleName = "No School";
            } else {
            CurrentSchedule = FindCurrentSchedule(DateTime.Now, ScheduleList[ScheduleOfTheDay].Schedule);
            TimeTilEndOfPeriod(DateTime.Now, ScheduleList[ScheduleOfTheDay]);
                 ScheduleName = ScheduleList[ScheduleOfTheDay].ScheduleName ;

            }
        }



        public int FindScheduleType(){

     

            String[] scheduleName = new String[scheduleExceptions.Count];
            scheduleExceptions.Keys.CopyTo(scheduleName, 0);

            String[] time = new String[scheduleExceptions.Count];
            scheduleExceptions.Values.CopyTo(time, 0);

            for (int i = 0; i < scheduleExceptions.Count; i++)
            {
                bool isFound = DateTime.Equals(DateTime.Today, DateTime.Parse(scheduleName[i]));
                for (int s = 0; s < ScheduleList.Count; s++)
                    if(string.Equals(time[i],ScheduleList[s].ScheduleName) && isFound)
                        return s;
            }


            return (int)DateTime.Today.DayOfWeek == 6 || (int)DateTime.Today.DayOfWeek == 0 ? -1 : 0;
        }
        
        public void PushLocalNotifications()
        {

            if (ScheduleOfTheDay != -1) {

                CrossLocalNotifications.Current.CancelAll();

                var notID = 0;

                foreach(var s in ScheduleList[ScheduleOfTheDay].Schedule) {

                    if(DateTime.Compare(DateTime.Now,s.EndDateTime) < 0){
                    CrossLocalNotifications.Current.Show("2 Minute Warning!", s.PeriodName + " is about to end in 2 minutes", notID, s.EndDateTime.AddSeconds(-120) ,true, true);
                        notID++;
                    }
                }

            }
        }



        public void LocalJson()
        {


            ScheduleList.RemoveRange(0, ScheduleList.Count);

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(SHHSScheduleManager)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("SHHS.shhsdata.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsondata = reader.ReadToEnd();
                JObject root = JObject.Parse(jsondata);
                var a = root["South Hills Schedule"]["Regular Schedule"].ToString();

                foreach (var schedule in root["South Hills Schedule"].Children().Children()){

                    SHHSSchedule s = JsonConvert.DeserializeObject<SHHSSchedule>(schedule.ToString());
                    ScheduleList.Add(s);

                    for (int i = 0; i < s.Schedule.Count; i++)
                    {
                        s.Schedule[i].StartDateTime = DateTime.Parse(s.Schedule[i].StartTime).AddSeconds(Offset);
                        s.Schedule[i].EndDateTime = DateTime.Parse(s.Schedule[i].EndTime).AddSeconds(Offset);
                        s.Schedule[i].Length = CalcTime(s.Schedule[i].StartDateTime, s.Schedule[i].EndDateTime);
                    }

                }


            }




        }




    }

}



