using System;
using SHHS.View;
using SHHS.Model;
using Xamarin.Forms;
using CarouselView.FormsPlugin.Abstractions;
using Plugin.DeviceOrientation;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

namespace SHHS.Controller
{
    public partial class MainPage : TabbedPage
    {


        public SHHSScheduleManager scheduleManager;
        SHHSTimer timer;
        CarouselViewControl myCarousel;
        public SHHSCountDown countdown;

        public SHHSAnnouncementManager announcementManager;
        public SHHSAnnouncementManager countdownManager;
        ImageButton addAnnouncement;
        Boolean isAnimating;

        public MainPage()
        {
            InitializeComponent();
            timer = new SHHSTimer();

            countdown = new SHHSCountDown(new DateTime(DateTime.Today.Year + 1,1,1),"New Year");
            countdownManager = new SHHSAnnouncementManager();
            announcementManager = new SHHSAnnouncementManager();
            addAnnouncement = new ImageButton { BackgroundColor = Color.Transparent, WidthRequest = 50, Opacity = 0.75, Source = "Add.png", Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, IsVisible = false };
            addAnnouncement.Clicked += AddAnnouncement_Clicked;







            myCarousel = new CarouselViewControl
            {
                ItemsSource = announcementManager.AnnouncementList,  // ADD/REMOVE PAGES FROM CAROUSEL ADDING/REMOVING ELEMENTS FROM THE COLLECTION
                Position = 0, //default
                InterPageSpacing = 10,
                Orientation = CarouselViewOrientation.Horizontal,
                PositionSelectedCommand = announcementManager.MyCommand,
                ShowArrows = false,
                ShowIndicators = true,
                BackgroundColor = Color.FromHex("#E5EDCD")
            };

            myCarousel.PositionSelected += Handle_PositionSelected;
            myCarousel.Scrolled += Handle_Scrolled;
        
            //x,y,w,h
            //Adding timer

            HomePage.Children.Add(timer, null, null,
            Constraint.RelativeToParent((parent) =>
            {
                if (parent.Width > parent.Height)
                {


                    return parent.Width;
                }
                return parent.Width;
            }),
            Constraint.RelativeToParent((parent) =>
            {
                if (parent.Width > parent.Height)
                {

                    return parent.Height;
                }
                return parent.Height / 3;
            }));



            HomePage.Children.Add(countdown, null, Constraint.RelativeToParent((parent) =>
            {

                return timer.Height + 10;
            }),
            Constraint.RelativeToParent((parent) =>
            {

                return parent.Width;
            }),
            Constraint.RelativeToParent((parent) =>
            {

                return Height / 4;
            }));




            //Adding news scroll
            HomePage.Children.Add(myCarousel, null,Constraint.RelativeToParent((parent) =>
            {
              
                return countdown.Y + countdown.Height - 10;
            }),
            Constraint.RelativeToParent((parent) =>
            {
               
                return countdown.Width;
            }),
            Constraint.RelativeToParent((parent) =>
            {
               
                return Height / 4;
            }));




              HomePage.Children.Add(addAnnouncement, null,Constraint.RelativeToParent((parent) =>
            {
              
                return myCarousel.Y + myCarousel.Height + 30;
            }),
            Constraint.RelativeToParent((parent) =>
            {
               
                return parent.Width;
            })
           );

      


            CurrentPageChanged += CurrentPageChangedEvent;

            scheduleManager = new SHHSScheduleManager();


            //Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            //{
            //    if (myCarousel.ItemsSource.GetCount() == 0)
            //    {
            //        return false;
            //    }

            //    if (myCarousel.Position == myCarousel.ItemsSource.GetCount() - 1)
            //    {

            //        myCarousel.Position = 0;
            //    }
            //    else
            //    {
            //        myCarousel.Position += 1;


            //    }
            //    return true; // True = Repeat again, False = Stop the timer
            //});

        }


  
        void CurrentPageChangedEvent(object sender, EventArgs e)
        {

            var i = Children.IndexOf(CurrentPage);

            if(i == 0)
            CrossDeviceOrientation.Current.UnlockOrientation();


        }






        public async Task ReInitMainPage() {


            if ((Application.Current as App).Client == null)
            {
                return;
            }

            ShowAddButton();

            base.OnAppearing();
            if (await scheduleManager.GetScheduleException())
            {
                await scheduleManager.RefreshData();
                await announcementManager.GetAnnoucements();
                RefreshCarsoulView();
            }

            RefreshSchedule();
            scheduleManager.PushLocalNotifications();

        }



        public void RefreshCarsoulView() {


            myCarousel.ItemsSource = announcementManager.AnnouncementList;


        }




        public void RefreshSchedule()
        {

            scheduleManager.GetTimeUntilEndOfPeriod();
            timer.minutes = scheduleManager.MinutesLeft;
            timer.seconds = scheduleManager.SecondsLeft;
            timer.TimeLeft = "Loading";
            timer.PeriodInfo = scheduleManager.CurrentMessage;
            timer.length = scheduleManager.PeriodLength;
            string scheduleName = scheduleManager.ScheduleName;
       

            if( scheduleName.Contains(" ")){
                timer.line1 = scheduleName.Substring(0, scheduleName.IndexOf(" ", StringComparison.CurrentCulture));
                timer.line2 = scheduleName.Substring(scheduleName.IndexOf(" ", StringComparison.CurrentCulture) + 1);

            } else {

                timer.line1 = scheduleName;
                timer.line2 = "Schedule";

            }


            if (timer.minutes == 0 && timer.seconds == 0)
            {

                timer.TimeLeft = "0:00";
                timer.isActive = false;

                //First time starting
            }
            else if (!timer.isActive)
            {

                StartTime();


                //Resuming from app
            }
            else
            {



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
                if (Math.Abs(timer.SweepAngleSlider - newAngle) > 5 && isAnimating == false)
                {
                    isAnimating = true;
                    playAnimation(newAngle);
                }
                else
                {
                    timer.SweepAngleSlider = newAngle;
                }

                return true; // True = Repeat again, False = Stop the timer
            });

        }

        void playAnimation(float dgr)
        {


            Device.StartTimer(TimeSpan.FromMilliseconds(0.1), () =>
            {

                if (timer.SweepAngleSlider > dgr)
                {
                    timer.SweepAngleSlider -= 3;
                }
                else
                {

                    timer.SweepAngleSlider += 3;

                }

                if (Math.Abs(timer.SweepAngleSlider - dgr) <= 5)
                {
                    timer.SweepAngleSlider = dgr;
                    isAnimating = false;
                    return false;

                }

                return true; // True = Repeat again, False = Stop the timer
            });


        }

        private void ShowAddButton()
        {
            if (((App)Application.Current).isAdmin)
            {
                addAnnouncement.IsVisible = true;
            }
        }

        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {

            Console.WriteLine("Position " + e.NewValue + " selected.");

        }

        void Handle_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {
            Console.WriteLine("Scrolled to " + e.NewValue + " percent.");
            Console.WriteLine("Direction = " + e.Direction);
        }


        void AddAnnouncement_Clicked(object sender, EventArgs e)
        {

            PopupNavigation.Instance.PushAsync(new AnnouncementAddPage());


        }

    }
}