﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace SHHS.Controller
{
    public partial class CalenderPage : ContentPage
    {

        private int currentYear = DateTime.Today.Date.Year;
        private int currentMonth = DateTime.Today.Date.Month;




        public CalenderPage()
        {
            InitializeComponent();
            RefreshDate();



        }



        //RelativeView.Children.Add(new Button { Text = "Announcement1", BorderColor = Color.White, IsEnabled = false, CornerRadius = 35, BorderWidth = 2 },
        //                                                                Constraint.RelativeToParent(parent=>parent.X+35),
        //                                                                Constraint.RelativeToParent(parent=>parent.Width*0.8),
        //                                                                Constraint.RelativeToParent(parent=>monthLabel.Y+350),
        //                                                                Constraint.RelativeToParent(parent=>monthLabel.Height*2));
        //RelativeView.Children.Add(new Button { Text = "Announcement2", BorderColor = Color.White, CornerRadius = 35, BorderWidth = 2, IsEnabled = false },
        //Constraint.RelativeToParent(parent => parent.X + 35),
        //Constraint.RelativeToParent(parent => parent.Width * 0.8),
        //Constraint.RelativeToParent(parent => monthLabel.Y + 500),
        //Constraint.RelativeToParent(parent => monthLabel.Height * 2));


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
            //Delete all labels
            for (int i = 0; i < Calendar.Children.Count; i++)
            {

                Calendar.Children.RemoveAt(i);
                i--;


            }



            //Display Dates From Previous Month
            for (int i = daysInPreviousMonth - daysNeedToBeDisplayFromPreviousMonth; i < daysInPreviousMonth; i++)
            {
                var label = new Button { Text = $"{i}", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, CornerRadius = 20, WidthRequest = 40, HeightRequest = 40 ,BackgroundColor = Color.Transparent, Opacity = 0.5 };
                Calendar.Children.Add(label, column, row);
                column++;


            }

            //Displays Dates From Current Month
            for (int i = 1; i <= daysInMonth; i++)
            {
                var label = new Button { Text = $"{i}", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, CornerRadius = 20, WidthRequest = 40, HeightRequest = 40 ,BackgroundColor = Color.Transparent, Opacity = 1};
                label.Clicked += DateClicked;
                Calendar.Children.Add(label, column, row);
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
                var label = new Button { Text = $"{i}", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, CornerRadius = 20, WidthRequest =40, HeightRequest = 40 ,BackgroundColor = Color.Transparent,  Opacity = 0.5 };
                Calendar.Children.Add(label, column, row);
                column++;
                if (column != 0 && column % 7 == 0)
                {
                    column = 0;
                    row++;


                }



            }
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

        private async void StartAnimation(Button button)
        {
            await Task.Delay(20);
            await button.FadeTo(0, 250);
            await Task.Delay(20);
            await button.FadeTo(1, 250);
        }



    }
}




