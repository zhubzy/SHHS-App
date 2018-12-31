
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using SHHS.Model;
using System.Threading.Tasks;
using System;

namespace SHHS.View
{
    public partial class SHHSEventLayout
    {
        public SHHSEventLayout()
        {
            InitializeComponent();
        }

        void DiscardChanges(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(false);
        }
        async void SaveEvent(object sender, System.EventArgs e)
        {
            SHHSEvent a = new SHHSEvent { Title = EventNameEntry.Text, Location = LoacationEntry.Text , StartDate = StartDateEntry.Date, EndDate = EndDateEntry.Date, StartTime = StartTimeEntry.Time,  EndTime = EndTimeEntry.Time  };

            DateTime now = DateTime.Now;
            DateTime today = DateTime.Now;
            DateTime startDate = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, a.StartTime.Hours, a.StartTime.Minutes, a.StartTime.Seconds);
            DateTime endDate = new DateTime(a.EndDate.Year, a.EndDate.Month, a.EndDate.Day, a.EndTime.Hours, a.EndTime.Minutes, a.EndTime.Seconds);

            if (AllDaySwitch.IsToggled)
            {
                a.Time = "All Day";

            }
            else
            {
                a.Time = "" + a.StartTime + " - " + a.EndTime;
            }

            //For one time events 
            int daysDifferenceS = (int)Math.Abs(startDate.Subtract(now).Days);
            int hoursDifferenceS = (int)Math.Abs(startDate.Subtract(now).Hours);
            int minsDifferenceS = (int)Math.Abs(startDate.Subtract(now).Minutes);
            int daysDifferenceE = (int)Math.Abs(endDate.Subtract(now).Days);
            int hoursDifferenceE = (int)Math.Abs(endDate.Subtract(now).Hours);
            int minsDifferenceE = (int)Math.Abs(endDate.Subtract(now).Minutes);
            String timeDifferencialS;
            String timeDifferencialE;

            //TODO: Simplify into method
            if (daysDifferenceS > 0)
            {
                timeDifferencialS = "" + daysDifferenceS + " days";

            } else if (hoursDifferenceS  == 0) {

                timeDifferencialS = "" + minsDifferenceS + " mins";



            }
            else if (daysDifferenceE == 0) {

                timeDifferencialS = "" + hoursDifferenceS + " hours";


            }
            else {

                timeDifferencialS = "Error";
            }

            if (daysDifferenceE > 0)
            {
                timeDifferencialE = "" + daysDifferenceE + " days";

            }
            else if (hoursDifferenceE == 0)
            {

                timeDifferencialE = "" + minsDifferenceE + " mins";

            }
            else if (daysDifferenceE == 0)
            {
                timeDifferencialE = "" + hoursDifferenceE + " hours";



            }
            else
            {

                timeDifferencialE = "Error";
            }








            if (startDate.Subtract(now).TotalSeconds > 0 ) {
                //Event hasn't begin yet

                a.DaysLeft = "In " + timeDifferencialS;

            
            } else {
                //Event has past or in between

                if (endDate.Subtract(now).TotalSeconds > 0)
                {
                    //Event hasn't ended yet
                    a.DaysLeft = timeDifferencialE + " remaining";
                }
                else
                {
                    a.DaysLeft = timeDifferencialE + " remaing";
                }
            }
         


            await ((App)Application.Current).shhsEventManager.AddEvent(a);
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
