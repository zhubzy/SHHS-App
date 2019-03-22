using System;
using System.ComponentModel;
using SQLite;
namespace SHHS.Model
{
    public class SHHSEvent: BaseViewModel, IComparable
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }


        //Fields
        private string _daysLeft;
        private string _title;
        private string _location;
        private string _time;

        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isCountdown;



        //Accessors


        public string LocationText { get; set; }
        public string Title { get { return _title; } set { SetValue(ref _title, value); } }
        public string Time { get { return _time; } set { SetValue(ref _time, value); } }
        public string Location { get { return _location; } set { SetValue(ref _location, value); } }
        public string DaysLeft { get { return _daysLeft; } set { SetValue(ref _daysLeft, value); } }
        public bool IsOnline { get; set; }
        public bool IsCountDown { get { return _isCountdown; } set { SetValue(ref _isCountdown, value); } }
        public string IsCountDownLabel { get { if (!IsCountDown) return "Use CD"; return "Stop CD"; } }


        //Firebase
        public string FireBaseID { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public string DateString { get; set; }



        public TimeSpan StartTime { get { return _startTime; } set { SetValue(ref _startTime, value); } }
        public TimeSpan EndTime { get { return _endTime; } set { SetValue(ref _endTime, value); } }
        public DateTime StartDate { get { return _startDate; } set { SetValue(ref _startDate, value); } }
        public DateTime EndDate { get { return _endDate; } set { SetValue(ref _endDate, value); } }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;


            if (obj is SHHSEvent eventA)
            {

                if (eventA.DaysLeft.Equals("Today") && !this.DaysLeft.Equals("Today"))
                    return 1;
                else if (!eventA.DaysLeft.Equals("Today") && this.DaysLeft.Equals("Today"))
                    return -1;
                else if (this.StartDate.CompareTo(DateTime.Now) > 0 && eventA.StartDate.CompareTo(DateTime.Now) < 0)
                    return -1;
                else if (this.StartDate.CompareTo(DateTime.Now) < 0 && eventA.StartDate.CompareTo(DateTime.Now) > 0)
                    return 1;
                else if (this.StartDate.CompareTo(DateTime.Now) < 0 && eventA.StartDate.CompareTo(DateTime.Now) < 0)
                    return -1 * this.StartDate.CompareTo(eventA.StartDate);

                return this.StartDate.CompareTo(eventA.StartDate);
            }
            else
                return 0;
        }

       

    }
}
