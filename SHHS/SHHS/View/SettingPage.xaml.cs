using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SHHS.View
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {

            InitializeComponent();
            BindingContext = Application.Current;
        }
        void Handle_Tapped(object sender, System.EventArgs e)
        {

            Navigation.PushAsync(new FeedBackPage());

        }

 

        void Handle_Tapped_1(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CreditPage());
        }

        void Handle_ToLogInPage(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LogInPage());
        }

        void GoToScheduleDisplay(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ScheduleDisplayPage());

        }
        void GoToAeries(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("https://aeries.c-vusd.org/parentportal/LoginParent.aspx?page=default.aspx"));
        }
        void ViewLunchMenu(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("https://schoolnutritionandfitness.com/mobile/#/school/1305082248163119/3054/4896"));
        }

        void ViewSports(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("http://www.maxpreps.com/high-schools/south-hills-huskies-(west-covina,ca)/sports.htm"));
        }


    }
}
