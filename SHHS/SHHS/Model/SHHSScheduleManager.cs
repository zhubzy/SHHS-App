﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.LocalNotifications;
using Xamarin.Forms;

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

            var firebase = (Application.Current as App).Client;
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
                

                
                Console.WriteLine(e.Message);

                return false;
            }

        }

        public List<SHHSPeriod> GetCurrentPeriodList() {

            if(ScheduleOfTheDay >= 0) 
            return ScheduleList[ScheduleOfTheDay].Schedule;
                return ScheduleList[0].Schedule;
            

        }

        public List<SHHSPeriod> GetCurrentPeriodList(int s)
        {

          
            return ScheduleList[s].Schedule;


        }


        public List<String> GetScheduleNames()
        {
            List<String> schedules = new List<string>();
           foreach(var sch in ScheduleList) {

                schedules.Add(sch.ScheduleName);
           }

            return schedules;

        }

        public async Task RefreshData()
        {

            var firebase = (Application.Current as App).Client;
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

            } else if (closestPeriodEnd.Hour * 60 + closestPeriodEnd.Minute > now.Hour * 60 + now.Minute) {

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
                CurrentMessage = GetScheduleName(DateTime.Today.AddDays(1)) + " tomorrow";

            }


        }

        public void GetTimeUntilEndOfPeriod(){

            ScheduleOfTheDay = FindScheduleType(DateTime.Today);
     

            if (ScheduleOfTheDay == -1){
                    IsActive = false;
                    MinutesLeft = 0;
                    SecondsLeft = 0;
                    CurrentMessage = GetScheduleName(DateTime.Today.AddDays(1)) + " tomorrow";
                    ScheduleName = "No School";
            } else {
                CurrentSchedule = FindCurrentSchedule(DateTime.Now, ScheduleList[ScheduleOfTheDay].Schedule);
                TimeTilEndOfPeriod(DateTime.Now, ScheduleList[ScheduleOfTheDay]);
                 ScheduleName = ScheduleList[ScheduleOfTheDay].ScheduleName ;

            }
        }


        private string GetScheduleName(DateTime d) {
            int tommorowShedule = FindScheduleType(d);
            string tommorowScheduleName;
            if (tommorowShedule < 0)
            {
                tommorowScheduleName = "No School";
            }
            else
            {

                tommorowScheduleName = ScheduleList[tommorowShedule].ScheduleName;
            }

            return tommorowScheduleName;
        }

        public int GetScheduleForDay(DateTime d)
        {
            int tommorowShedule = FindScheduleType(d);
            string tommorowScheduleName;
            if (tommorowShedule < 0)
            {
                tommorowScheduleName = "No School";
            }
            else
            {

                tommorowScheduleName = ScheduleList[tommorowShedule].ScheduleName;
            }

            return tommorowShedule;
        }


        public int FindScheduleType(DateTime d){

     

            String[] scheduleName = new String[scheduleExceptions.Count];
            scheduleExceptions.Keys.CopyTo(scheduleName, 0);

            String[] time = new String[scheduleExceptions.Count];
            scheduleExceptions.Values.CopyTo(time, 0);

            for (int i = 0; i < scheduleExceptions.Count; i++)
            {
                bool isFound = DateTime.Equals(d, DateTime.Parse(scheduleName[i]));


                for (int s = 0; s < ScheduleList.Count; s++)
                {
                    if (string.Equals(time[i],"No School") && isFound)
                        return -1;
                    if (string.Equals(time[i], ScheduleList[s].ScheduleName) && isFound)
                        return s;
                }
            }


            return (int)d.DayOfWeek == 6 || (int)d.DayOfWeek == 0 ? -1 : 0;
        }
        
        public void PushLocalNotifications()
        {


            

            var app = Application.Current as App;

            int mins;
            if(int.TryParse(app.MinutesToSendNotification, out mins) == false) {

                mins = 2;
            }
            CrossLocalNotifications.Current.CancelAll();

            var notID = 0;

            for (int i = 0; i < 14; i++)
            {
                DateTime date = DateTime.Today.AddDays(i * 1.0);
                int scheduleIndex = GetScheduleForDay(date);

                if (scheduleIndex != -1 && app.NotificationEnabled && (!date.ToString().Equals(app.RemindToday)))
                {



                    foreach (var s in ScheduleList[scheduleIndex].Schedule)
                    {
                        DateTime notDate = new DateTime(date.Year, date.Month, date.Day, s.EndDateTime.Hour, s.EndDateTime.Minute,0);
                        if (DateTime.Compare(DateTime.Now, notDate) < 0)
                        {
                            
                            if (app.SoundEnabled)
                                CrossLocalNotifications.Current.Show(mins + " Minute Warning!", s.PeriodName + " is about to end in " + mins + " minutes", notID, notDate.AddMinutes(-1.0 * mins), true, true);
                            else
                                CrossLocalNotifications.Current.Show(mins + " Minute Warning!", s.PeriodName + " is about to end in " + mins + " minutes", notID, notDate.AddMinutes(-1.0 * mins), false, false);

                            notID++;
                        }
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



