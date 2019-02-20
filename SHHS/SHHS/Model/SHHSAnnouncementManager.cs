using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Firebase.Database;
using Firebase.Database.Query;
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
            SHHSAnnouncement view = new SHHSAnnouncement { Announcer = "Loading", Info = "Loading" };
         


            AnnouncementList.Add(CreateStackLayout(view));

         
        }

        public async Task GetAnnoucements()
        {

            var firebase = (Application.Current as App).Client;



            try
            {

                var annoucements = await firebase.Child("South Hills Announcements").OnceAsync<SHHSAnnouncement>();
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

        public async Task AddAnnoucements(SHHSAnnouncement s)
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");



            try
            {

                await firebase.Child("South Hills Announcements").PostAsync(s);
                AnnouncementList.Add(CreateStackLayout(s));


            }
            catch (FirebaseException e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }






        public StackLayout CreateStackLayout(SHHSAnnouncement v)
        {

            var layout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var ownerlayout = new StackLayout { Orientation = StackOrientation.Vertical, WidthRequest = 100  };
            var annoucerLabel = new Label
            {

                Text = v.Announcer,
                FontFamily = Device.RuntimePlatform == Device.iOS ? "OpenSans-Bold": "OpenSans-Bold.ttf#OpenSans-Bold",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            var infoLabel = new Label
            {
                Text = v.Info,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = Device.RuntimePlatform == Device.iOS ? "OpenSans-Regular" : "OpenSans-Regular.ttf#OpenSans-Regular",
               
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
