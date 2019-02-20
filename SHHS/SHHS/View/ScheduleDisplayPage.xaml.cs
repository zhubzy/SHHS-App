using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SHHS.View
{
    public partial class ScheduleDisplayPage : ContentPage
    {
        public ScheduleDisplayPage()
        {

            InitializeComponent();
            ScheduleDisplayList.ItemsSource = (Application.Current as App).shhsMain.scheduleManager.GetCurrentPeriodList();
            SchedulePicker.ItemsSource = (Application.Current as App).shhsMain.scheduleManager.GetScheduleNames();
            SchedulePicker.SelectedIndex = (Application.Current as App).shhsMain.scheduleManager.ScheduleOfTheDay;


           }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ScheduleDisplayList.ItemsSource = (Application.Current as App).shhsMain.scheduleManager.GetCurrentPeriodList(SchedulePicker.SelectedIndex);
        }
    }
}
