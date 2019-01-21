using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
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

                    var Stack = new StackLayout { Orientation = StackOrientation.Vertical, Spacing = -10 };
                    var dot = new Label { Text = "", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White };
                    var label = new Button { Text = $"{row*7 + col}", FontFamily="OpenSans-Light", HorizontalOptions = LayoutOptions.Center, TextColor = Color.White, WidthRequest = 40, HeightRequest = 40, BackgroundColor = Color.Transparent, Opacity = 1 };
                    Stack.Children.Add(label);
                    Stack.Children.Add(dot);

                    Calendar.Children.Add(Stack, col, row);
                }



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
            RefreshDate();

        }








        public void SetDataSource(ObservableCollection<SHHSEvent> s) {

            AnnoucementList.ItemsSource = s;

        }


        public void RefreshDate()
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
            for (int i = daysInPreviousMonth - daysNeedToBeDisplayFromPreviousMonth + 1; i <= daysInPreviousMonth; i++)
            {
                Button lb = (Button)((StackLayout)(Calendar.Children[row * 7 + column])).Children[0];
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.BorderWidth = 0;
                lb.Opacity = 0.5;
                lb.BindingContext =  SubtraceOneMonth(currentYear,currentMonth,i);

                int n = ((App)Application.Current).shhsEventManager.FindEvent((DateTime)lb.BindingContext);
                Label b = (Label)((StackLayout)(Calendar.Children[row * 7 + column])).Children[1];
                b.Text = "";


                for (int a =0; a<n; a++)
                {

                    b.Text += ".";
                    b.Opacity = 1;

                }



                column++;


            }

            //Displays Dates From Current Month
            for (int i = 1; i <= daysInMonth; i++)
            {
                Button lb = (Button)((StackLayout)(Calendar.Children[row * 7 + column])).Children[0];
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.Opacity = 1;
                lb.BorderWidth = 0;
                lb.BindingContext = new DateTime(currentYear, currentMonth, i);
                lb.Clicked += DateClicked;


                int n = ((App)Application.Current).shhsEventManager.FindEvent((DateTime)lb.BindingContext);
                Label b = (Label)((StackLayout)(Calendar.Children[row * 7 + column])).Children[1];
                b.Text = "";

                for (int a = 0; a < n; a++)
                {

                    b.Text += ".";
                    b.Opacity = 1;

                }

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
                Button lb = (Button)((StackLayout)(Calendar.Children[row * 7 + column])).Children[0];
                lb.Text = $"{i}";
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.Opacity = 0.5;
                lb.BorderWidth = 0;
                lb.BindingContext = AddOneMonth(currentYear, currentMonth, i);



                int n = ((App)Application.Current).shhsEventManager.FindEvent((DateTime)lb.BindingContext);
                Label b = (Label)((StackLayout)(Calendar.Children[row * 7 + column])).Children[1];
                b.Text = "";

                for (int a = 0; a < n; a++)
                {

                    b.Text += ".";
                    b.Opacity = 0.5;

                }


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
                Button lb = (Button)((StackLayout)(Calendar.Children[DateTime.Today.Day + daysNeedToBeDisplayFromPreviousMonth - 1])).Children[0];
                lb.BorderColor = Color.FromHex("e6e600");
                lb.BorderWidth = 5;
                lb.Opacity = 1;

            }
        }

  





        void DateClicked(object sender, System.EventArgs e){

             
            if (currentSelected != 0) {
                int daysNeedToBeDisplayFromPreviousMonth = (int)new DateTime(currentYear, currentMonth, 1).DayOfWeek - 1;
                var lb = (Button)((StackLayout)(Calendar.Children[currentSelected + daysNeedToBeDisplayFromPreviousMonth - 1])).Children[0];
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;


            }//reset

            Button button = (Button)sender;

            currentSelected = Convert.ToInt32(button.Text);
            button.TextColor = Color.Black;
            button.BackgroundColor = Color.FromHex("e6e600");



        }















        /// Display and refesh the calander in previous month.
        void PreviousMonth(object sender, System.EventArgs e)
        {
            AdvanceToPreviousMonth();
        }

         void NextMonth(object sender, System.EventArgs e)
        {
            AdvanceToNextMonth();
        }


        DateTime SubtraceOneMonth(int yr, int mt, int day) {

        
            if(mt == 1) {

                yr -= 1;
                mt = 12;
            
            } else {

                mt -= 1;
            }

            return new DateTime(yr, mt, day);

        }



        DateTime AddOneMonth(int yr, int mt, int day)
        {
       

            if (mt == 12)
            {

                yr += 1;
                mt = 1;

            }
            else
            {

                mt += 1;

            }

            return new DateTime(yr,mt,day);

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
        //async void MakeFavorite(object sender, System.EventArgs e)
        //{

        //}

        async void AddEvent(object sender, System.EventArgs e)
        {

            await PopupNavigation.Instance.PushAsync(new SHHSEventLayout());

        }

        void OnSwipedL(object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            AdvanceToPreviousMonth();        
           }


        void OnSwipedR(object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            AdvanceToNextMonth();
        }



        private void AdvanceToPreviousMonth() {

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
        private void AdvanceToNextMonth()
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





        public void ClearLabel()
        {

            for (int i = 0; i <= Calendar.Children.Count; i++)
            {
                Button lb = (Button)((StackLayout)(Calendar.Children[i])).Children[0];
                lb.BackgroundColor = Color.Transparent;
                lb.TextColor = Color.White;
                lb.BorderWidth = 0;
                lb.Opacity = 0.5;
            }

        }
            void EditEvent(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var shhsEvent = (e.Item as SHHSEvent);
             PopupNavigation.Instance.PushAsync(new SHHSEventLayout(shhsEvent));
            AnnoucementList.SelectedItem = null;
        }
    }
}





