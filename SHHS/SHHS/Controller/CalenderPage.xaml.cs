using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using Plugin.LocalNotifications;
using System.Collections.ObjectModel;
using SHHS.Model;
using SHHS.View;
using Rg.Plugins.Popup.Services;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;

namespace SHHS.Controller
{
    public partial class CalenderPage : ContentPage
    {

         int currentYear = DateTime.Today.Date.Year;
         int currentMonth = DateTime.Today.Date.Month;
        int currentSelected;


        public CalenderPage()
        {
            InitializeComponent();

            //Calender Initalization
            for (int row = 0; row < 6; row++)
                for (int col = 0; col < 7; col++){

                    var label = new Button { Text = $"{row*7 + col}", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, WidthRequest = 40, HeightRequest = 40, BackgroundColor = Color.Transparent, Opacity = 1 };
                    label.CornerRadius = (int)(label.HeightRequest / 2);
                    Calendar.Children.Add(label, col, row);
                }
            RefreshDate();



        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            CalendarScroll.HeightRequest = height / 2.2;

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);

        }








        public void SetDataSource(ObservableCollection<SHHSEvent> s) {

            AnnoucementList.ItemsSource = s;

        }


       



        private void RefreshDate()
        {




            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int daysNeedToBeDisplayFromPreviousMonth = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek - 1;

            DateTime lastDayOfpreviousMonth = currentMonth == 1
                ? new DateTime(currentYear - 1, 12, DateTime.DaysInMonth(currentYear - 1, 12))
                          : new DateTime(currentYear, currentMonth - 1, DateTime.DaysInMonth(currentYear, currentMonth - 1));

            int daysInPreviousMonth = DateTime.DaysInMonth(lastDayOfpreviousMonth.Year, lastDayOfpreviousMonth.Month);


            int column = 0;
            int row = 0;

            YearLabel.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth) + " " + currentYear.ToString();
         


            //Display Dates From Previous Month
            for (int i = daysInPreviousMonth - daysNeedToBeDisplayFromPreviousMonth; i < daysInPreviousMonth; i++)
            {
                Button lb = ((Button)(Calendar.Children[row * 7 + column]));
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                button.BorderWidth = 0;
                lb.Opacity = 0.5;

                column++;


            }

            //Displays Dates From Current Month
            for (int i = 1; i <= daysInMonth; i++)
            {
                Button lb = ((Button)(Calendar.Children[row * 7 + column]));
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.Opacity = 1;
                button.BorderWidth = 0;
                lb.Clicked += DateClicked;

                column++;
                if (column != 0 && column % 7 == 0)
                {
                    column = 0;
                    row++;
                }
            


            }


            //Displays Dates From Next Month
            for (int i = 1; row < 6; i++)
            {
                Button lb = ((Button)(Calendar.Children[row * 7 + column]));
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.Opacity = 0.5;
                button.BorderWidth = 0;

                column++;
                if (column != 0 && column % 7 == 0)
                {
                    column = 0;
                    row++;


                }



            }

            //Highlight the current date
            if (currentMonth == DateTime.Today.Month && currentYear == DateTime.Today.Year)
            {
                
                var button = ((Button)Calendar.Children[DateTime.Today.Day + daysNeedToBeDisplayFromPreviousMonth - 1]);
                button.BorderColor = Color.Green;
                button.BorderWidth = 5;
                button.Opacity = 1;

            }
        }

        void AddEvent(object sender, System.EventArgs e)
        {

            PopupNavigation.Instance.PushAsync(new SHHSEventLayout());

        }





        void DateClicked(object sender, System.EventArgs e){


            if(currentSelected != 0) {
                int daysNeedToBeDisplayFromPreviousMonth = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek - 1;
                var lb = ((Button)(Calendar.Children[currentSelected + daysNeedToBeDisplayFromPreviousMonth - 1]));
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;


            }

            var button = (Button)sender;

            currentSelected = Convert.ToInt32(button.Text);

            button.BackgroundColor = Color.Green;
            


            CrossLocalNotifications.Current.Show("Test","Test Message",0, DateTime.Now.AddSeconds(5),true,true);

        }

        /// Display and refesh the calander in previous month.
        void PreviousMonth(object sender, System.EventArgs e)
        {


            StartAnimation((Button)sender);

            if (currentMonth == 1)
            {

                currentYear--;
                currentMonth = 12;
            }
            else
            {
                currentMonth--;
            }

            RefreshDate();

        }

         void NextMonth(object sender, System.EventArgs e)
        {
            StartAnimation((Button)sender);
            if (currentMonth == 12)
            {

                currentYear++;
                currentMonth = 1;
            }
            else
            {
                currentMonth++;
            }

             RefreshDate();
        }

         async void StartAnimation(Button button)
        {
            await Task.Delay(20);
            await button.FadeTo(0, 250);
            await Task.Delay(20);
            await button.FadeTo(1, 250);
        }

 
        async void DeleteEvent(object sender, System.EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var shhsEvent = menuItem.CommandParameter as SHHSEvent;
                       await ((App)Application.Current).shhsEventManager.RemoveEvent(shhsEvent);
        }
    }
}





