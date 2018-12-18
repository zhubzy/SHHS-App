using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using SHHS.Model;
using System.Collections.Generic;

namespace SHHS.Controller
{
    public partial class CalenderPage : ContentPage
    {

         int currentYear = DateTime.Today.Date.Year;
         int currentMonth = DateTime.Today.Date.Month;
         List<SHHSEvent> annoucements;



        public CalenderPage()
        {
            InitializeComponent();

            //Calender Initalization
            for (int row = 0; row < 6; row++)
                for (int col = 0; col < 7; col++){

                    var label = new Button { Text = $"{row*7 + col}", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, CornerRadius = 20, WidthRequest = 40, HeightRequest = 40, BackgroundColor = Color.Transparent, Opacity = 1 };
                    Calendar.Children.Add(label, col, row);
                }
            RefreshDate();


            //Annoucement List View Initalization
            annoucements = new List<SHHSEvent>
            {
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"},
                new SHHSEvent{Title = "Light Up The Night", Time = "5:30PM - 7:30PM", DaysLeft = "In 2 days", Location = "South Hills High School"}

            };

            AnnoucementList.ItemsSource = annoucements;

            
        }



          void RefreshDate()
        {


        


            int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int daysNeedToBeDisplayFromPreviousMonth = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek - 1;

            DateTime lastDayOfpreviousMonth = currentMonth == 1
                ? new DateTime(currentYear - 1, 12, DateTime.DaysInMonth(currentYear - 1, 12))
                          : new DateTime(currentYear, currentMonth - 1, DateTime.DaysInMonth(currentYear, currentMonth - 1));

            int daysInPreviousMonth = DateTime.DaysInMonth(lastDayOfpreviousMonth.Year, lastDayOfpreviousMonth.Month);


            int column = 0;
            int row = 0;

            MonthLabel.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth);
            YearLabel.Text = currentYear.ToString();
         


            //Display Dates From Previous Month
            for (int i = daysInPreviousMonth - daysNeedToBeDisplayFromPreviousMonth; i < daysInPreviousMonth; i++)
            {
                Button lb = ((Button)(Calendar.Children[row * 7 + column]));
                lb.Opacity = 0.5;
                lb.Text = $"{i}";

                column++;


            }

            //Displays Dates From Current Month
            for (int i = 1; i <= daysInMonth; i++)
            {
                Button lb = ((Button)(Calendar.Children[row * 7 + column]));
                lb.Text = $"{i}";
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
                lb.Opacity = 0.5;
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
                button.BackgroundColor = Color.Red;
                button.Opacity = 0.3;


            }
        }






        void DateClicked(object sender, System.EventArgs e){

            var button = (Button)sender;
            button.BackgroundColor = Color.Green;
            button.Opacity = 0.2;


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



    }
}





