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
        Boolean isAnimating;

        public MainPage()
        {
            InitializeComponent();
            //timer = new SHHSTimer();
            //HomePage.Children.Add(timer,null,null,
            //Constraint.RelativeToParent((parent) => {
            //    if(parent.Width > parent.Height){

                    
            //        return parent.Width;
            //    }
            //    return parent.Width ;
            //}),
            //Constraint.RelativeToParent((parent) => {
            //    if (parent.Width > parent.Height)
            //    {

            //        return parent.Height;
            //    }
            //    return parent.Height /3;
            //}));

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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if(width > height && HomePage.Children.Count > 0){
                timer.canvasView.InvalidateSurface();
            }

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
                timer.isActive = false;

                //First time starting
            } else if (!timer.isActive) {

                StartTime();


                //Resuming from app
            } else {



            }

        }


        void StartTime()
        {
            timer.isActive = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {


                if (timer.seconds == 0)
                {

                    if (timer.minutes == 0)
                    {
                        isAnimating = true;
                        playAnimation(360);
                        timer.isActive = false;           
                        RefreshSchedule();
                        return false;
                    }

                    timer.minutes--;
                    timer.seconds = 60;

                }

                    timer.seconds--;
                    timer.TimeLeft = timer.minutes.ToString("00") + ":" + timer.seconds.ToString("00");
                    int remainingsecs = (timer.minutes * 60 + timer.seconds);
                    float newAngle = (360.0F * (remainingsecs * 1.0F / timer.length));
                if(Math.Abs(timer.SweepAngleSlider - newAngle) > 5 && isAnimating == false){
                    isAnimating = true;
                    playAnimation(newAngle);
                } else {
                    timer.SweepAngleSlider = newAngle;
                }

                return true; // True = Repeat again, False = Stop the timer
            });

        }

        void playAnimation(float dgr){


            Device.StartTimer(TimeSpan.FromMilliseconds(0.1), () =>
            {

                if (timer.SweepAngleSlider > dgr)
                {
                    timer.SweepAngleSlider -= 3;
                } else {

                    timer.SweepAngleSlider += 3;

                }

                if (Math.Abs(timer.SweepAngleSlider - dgr)  <= 5){
                    timer.SweepAngleSlider = dgr;
                    isAnimating = false;
                    return false;

                }

                return true; // True = Repeat again, False = Stop the timer
            });


        }






    }
}