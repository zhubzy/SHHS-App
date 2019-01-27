using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Firebase.Database;
using Xamarin.Forms;

namespace SHHS.Model
{
    public class SHHSAnnouncementManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<Xamarin.Forms.View> _annoucementList;
        public ObservableCollection<Xamarin.Forms.View> AnnouncementList
        {
            set
            {
                _annoucementList = value;
                OnPropertyChanged("MyItemsSource");
            }
            get
            {
                return _annoucementList;
            }
        }

        public Command MyCommand { protected set; get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        public SHHSAnnouncementManager()
        {


        
            MyCommand = new Command(() =>
            {
                Debug.WriteLine("Position selected.");
            });
            AnnouncementList = new ObservableCollection<Xamarin.Forms.View>();
            SHHSAnnouncementView view = new SHHSAnnouncementView { Announcer = "Loading", Info = "Loading" };
            SHHSAnnouncementView view2 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Your Internet May Be Slow" };
            SHHSAnnouncementView view3 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Check Your Wifi Connection" };
            SHHSAnnouncementView view4 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Please Be Patient" };


            AnnouncementList.Add(CreateStackLayout(view));
            AnnouncementList.Add(CreateStackLayout(view2));
            AnnouncementList.Add(CreateStackLayout(view3));
            AnnouncementList.Add(CreateStackLayout(view4));
         
        }

        public async Task GetAnnoucements()
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

       

            try
            {

                var annoucements = await firebase.Child("South Hills Announcements").OnceAsync<SHHSAnnouncementView>();
                AnnouncementList.Clear();
                foreach (var e in annoucements)
                {
                    AnnouncementList.Add(CreateStackLayout(e.Object));

                }

            }
            catch (FirebaseException e)
            {



                Console.WriteLine(e.StackTrace);


            }

        }

   




        public StackLayout CreateStackLayout(SHHSAnnouncementView v)
        {

            var layout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var ownerlayout = new StackLayout { Orientation = StackOrientation.Vertical };
            var annoucerLabel = new Label
            {

                Text = v.Announcer,
                FontFamily = Device.RuntimePlatform == Device.iOS ? "OpenSans-Bold": "OpenSans-Bold.ttf#OpenSans-Bold",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var infoLabel = new Label
            {
                Text = v.Info,
                FontFamily = Device.RuntimePlatform == Device.iOS ? "OpenSans-Regular" : "OpenSans-Regular.ttf#OpenSans-Regular",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };



            var icon = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Gold, HorizontalOptions = LayoutOptions.FillAndExpand };

            ownerlayout.Children.Add(annoucerLabel);
            ownerlayout.Children.Add(icon);
            layout.Children.Add(ownerlayout);
            layout.Children.Add(infoLabel);



            return layout;

        }

    }


}
