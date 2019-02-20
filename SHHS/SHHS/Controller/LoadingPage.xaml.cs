using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SHHS.Controller
{
    public partial class LoadingPage : ContentPage
    {

        private int numberOfTasks;
        private int completedTasks;

        public LoadingPage(int tasks)
        {
            InitializeComponent();
            LoadingBar.ProgressColor = Color.White;
            numberOfTasks = tasks;
            completedTasks = 0;
        }

        public void ProgressComplete() {

            completedTasks++;
            LoadingBar.ProgressTo((completedTasks * 1.0) / (numberOfTasks * 1.0));
        
        }
        public async System.Threading.Tasks.Task FadeAwayAsync() {
            LoadingBar.IsVisible = false;
            await StackLayoutLoad.FadeTo(0, 300, Easing.SinOut);
            //await Logo.ScaleTo(150, 900, Easing.SinOut);


        }
    }

    public class CustomProgressBar: ProgressBar {
        public void ProgressTo(double value)
        {

            Device.StartTimer(TimeSpan.FromMilliseconds(0.1), () =>
            {

                if (Progress > value)
                {
                    Progress -= 0.00005;
                }
                else
                {

                    Progress += 0.00005;

                }

                if (Math.Abs(Progress - value) < 0.001)
                {
              
                    return false;

                }

                return true; // True = Repeat again, False = Stop the timer
            });


        }
    }



}
