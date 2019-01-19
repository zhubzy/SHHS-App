
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using SHHS.Model;
using System;
using System.Globalization;

namespace SHHS.View
{
    public partial class SHHSEventLayout
    {

        private SHHSEvent eventEditing;


        public SHHSEventLayout(SHHSEvent e)
        {
            InitializeComponent();

            if(Device.RuntimePlatform == Device.Android) {

                ConfirmButton.IsVisible = false;
            
            }


            EventNameEntry.Text = e.Title;
            LoacationEntry.Text = e.Location;
            StartDateEntry.Date = e.StartDate;
            StartTimeEntry.Time = e.StartTime;
            EndTimeEntry.Time = e.EndTime;
            AllDaySwitch.IsToggled |= e.Time.Equals("All Day");
            ConfirmButton.Text = "Confirm Changes";
            eventEditing = e;
        }


        public SHHSEventLayout()
        {
            InitializeComponent();

        }
        void DiscardChanges(object sender, System.EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        async void SaveEvent(object sender, System.EventArgs e)
        {
            //Check if information is valid
            if (string.IsNullOrEmpty(EventNameEntry.Text))
            {
                await DisplayAlert("Invalid Input", "Event name required", "OK");
            }
            else if (StartTimeEntry.Time > EndTimeEntry.Time)
            {
                await DisplayAlert("Invalid Input", "The event cannot take place after it has ended!", "OK");
            }
            else
            {



                if (eventEditing!= null)
                {

                    eventEditing.Title  = EventNameEntry.Text;
                    eventEditing.Location = LoacationEntry.Text;
                    eventEditing.StartDate = StartDateEntry.Date; 
                    eventEditing.StartTime = StartTimeEntry.Time;
                    eventEditing.EndTime = EndTimeEntry.Time;

                    DateTime startDate = new DateTime(eventEditing.StartDate.Year, eventEditing.StartDate.Month, eventEditing.StartDate.Day, eventEditing.StartTime.Hours, eventEditing.StartTime.Minutes, eventEditing.StartTime.Seconds);
                    DateTime endDate = new DateTime(eventEditing.StartDate.Year, eventEditing.StartDate.Month, eventEditing.StartDate.Day, eventEditing.EndTime.Hours, eventEditing.EndTime.Minutes, eventEditing.EndTime.Seconds);
                    eventEditing.LocationText = startDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US"));

                     //Refresh events
                    if (AllDaySwitch.IsToggled)
                    {
                        eventEditing.Time = "All Day";
                    }
                    else
                    {
                        eventEditing.Time = "" + startDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US")) + " - " + endDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    if (!string.IsNullOrEmpty(LoacationEntry.Text)) { 
                    eventEditing.LocationText += "\n@ " + LoacationEntry.Text; 
                    }



                }
                else
                {


                    SHHSEvent a = new SHHSEvent { Title = EventNameEntry.Text, Location = LoacationEntry.Text, StartDate = StartDateEntry.Date, StartTime = StartTimeEntry.Time, EndTime = EndTimeEntry.Time };

                    DateTime startDate = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, a.StartTime.Hours, a.StartTime.Minutes, a.StartTime.Seconds);
                    DateTime endDate = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, a.EndTime.Hours, a.EndTime.Minutes, a.EndTime.Seconds);
                    a.LocationText = startDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US"));

                    if (AllDaySwitch.IsToggled)
                    {
                        a.Time = "All Day";

                    }
                    else
                    {
                        a.Time = "" + startDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US")) + " - " + endDate.ToString("t", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    if (!string.IsNullOrEmpty(LoacationEntry.Text))
                    {
                        a.LocationText += " @" + LoacationEntry.Text;
                    }
                    //Add the event to SQLite
                    await ((App)Application.Current).shhsEventManager.AddEvent(a);
                }




                await ((App)Application.Current).shhsEventManager.RefreshEvent();

                //Dismiss the popup page
                await PopupNavigation.Instance.PopAsync(true);
            }

        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {

            var t = sender as Switch;
            if (t.IsToggled)
            {

                StartTimeEntry.Opacity = 0.5;
                EndTimeEntry.Opacity = 0.5;
                StartTimeEntry.InputTransparent = true;
                EndTimeEntry.InputTransparent = true;


            }
            else
            {

                StartTimeEntry.Opacity = 1;
                EndTimeEntry.Opacity = 1;
                StartTimeEntry.InputTransparent = false;
                EndTimeEntry.InputTransparent = false;


            }

        }


    }
}
