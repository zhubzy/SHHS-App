﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;

namespace SHHS
{
    public partial class CalenderPage : ContentPage
    {

        private int currentYear = DateTime.Today.Date.Year;
        private int currentMonth = DateTime.Today.Date.Month;



        public CalenderPage()
        {
            InitializeComponent();

            RefreshDate();
            Console.WriteLine($"Count is {Calendar.Children.Count}");



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
            var firstDay = new DateTime(currentYear, currentMonth, 1);
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            int column = ((int)firstDay.DayOfWeek + 6) % 7 ;
            int row = 0;
            MonthLabel.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonth);

            for (int i = 0; i < Calendar.Children.Count; i++)
            {

                Calendar.Children.RemoveAt(i);
                i--;


            }

            Console.WriteLine($"month {currentMonth} days in month {daysInMonth}");

            for (int i = 1; i <= daysInMonth; i++)
            {
                var label = new Label { Text = $"{i}", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Calendar.Children.Add(label, column, row);

                if (i % 7 == 0)
                {
                    column = 0;
                    row++;
                }
                else
                {

                    column ++ ;

                }


            }
        }

        //backend programmer: start here
        void PreviousMonth(object sender, System.EventArgs e)
        {


            /*To Get Information about date*/
            /*To Get Information about date*
            //DateTime.DaysInMonth(2018,11)

            /*To Create date object*/
            //new DateTime(2018, 11, 2);

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



    }
}





