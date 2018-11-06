using System;

using SHHS.View;
using SHHS.Model;
using Xamarin.Forms;
namespace SHHS.Controller
{
    public partial class MainPage : TabbedPage
    {


        public SHHSScheduleManager scheduleManager;
        SHHSTimer timer;

        public MainPage()
        {
            InitializeComponent();
            timer = new SHHSTimer();
            HomePage.Children.Add(timer,  
            widthConstraint:Constraint.RelativeToParent(parent=>parent.Width),
            heightConstraint:Constraint.RelativeToParent(parent=>parent.Height / 3));
            scheduleManager = new SHHSScheduleManager();
            scheduleManager.LocalJson();



    }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //await scheduleManager.RefreshData();
            //Tells the the timer that schedule has benn loaded
            RefreshSchedule();
            Console.WriteLine( scheduleManager.MinutesLeft +  " Minutes " + scheduleManager.SecondsLeft + " Seconds.");

        }





        public void RefreshSchedule()
        {

            scheduleManager.GetTimeUntilEndOfPeriod();
            timer.minutes = scheduleManager.MinutesLeft;
            timer.seconds = scheduleManager.SecondsLeft;
            timer.TimeLeft = "Loading";
            timer.PeriodInfo = scheduleManager.CurrentMessage;
            timer.length = scheduleManager.PeriodLength;
 
            if (timer.minutes == 0 && timer.seconds == 0){

                timer.TimeLeft = "0:00";

            } else if (!timer.isActive) {

                StartTime();
                timer.isActive = true;


            }

        }


        void StartTime()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {


                if (timer.seconds == 0)
                {

                    if (timer.minutes == 0)
                    {
                        timer.isActive = false;
                        RefreshSchedule();
                        playAnimation();
                        return false;
                    }

                    timer.minutes--;
                    timer.seconds = 60;

                }

                    timer.seconds--;
                    timer.TimeLeft = timer.minutes.ToString("00") + ":" + timer.seconds.ToString("00");
                    int remainingsecs = (timer.minutes * 60 + timer.seconds);
                    timer.SweepAngleSlider = (float)(360.0 * (remainingsecs * 1.0 / timer.length));

                return true; // True = Repeat again, False = Stop the timer
            });

        }

        void playAnimation(){


            Device.StartTimer(TimeSpan.FromTicks(1), () =>
            {
                timer.SweepAngleSlider += 5;

                if(timer.SweepAngleSlider >= 360){
                    timer.SweepAngleSlider = 360;
                    return false;

                }

                return true; // True = Repeat again, False = Stop the timer
            });


        }






    }
}