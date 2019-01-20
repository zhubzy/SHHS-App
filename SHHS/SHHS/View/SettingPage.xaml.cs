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
    }
}
