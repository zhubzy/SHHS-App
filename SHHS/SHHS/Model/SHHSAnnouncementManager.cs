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
        public List<SHHSAnnouncementView> AnnouncementList { get; set; }

        public SHHSAnnouncementManager()
        {


            AnnouncementList = new List<SHHSAnnouncementView>();
        
            MyCommand = new Command(() =>
            {
                Debug.WriteLine("Position selected.");
            });
            MyItemsSource = new ObservableCollection<Xamarin.Forms.View>();
            SHHSAnnouncementView view = new SHHSAnnouncementView { Announcer = "Loading", Info = "Loading" };
            SHHSAnnouncementView view2 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Your Internet May Be Slow" };
            SHHSAnnouncementView view3 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Check Your Wifi Connection" };
            SHHSAnnouncementView view4 = new SHHSAnnouncementView { Announcer = "Loading", Info = "Please Be Patient" };


            MyItemsSource.Add(CreateStackLayout(view));
            MyItemsSource.Add(CreateStackLayout(view2));
            MyItemsSource.Add(CreateStackLayout(view3));
            MyItemsSource.Add(CreateStackLayout(view4));
         
        }

        public async Task GetAnnoucements()
        {

            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

            try
            {

                var annoucements = await firebase.Child("South Hills Announcements").OnceAsync<SHHSAnnouncementView>();
                foreach (var e in annoucements)
                {
                    AnnouncementList.Add(e.Object);

                }

            }
            catch (FirebaseException e)
            {



                Console.WriteLine(e.StackTrace);


            }
            RefreshData();

        }

        ObservableCollection<Xamarin.Forms.View> _myItemsSource;
        public ObservableCollection<Xamarin.Forms.View> MyItemsSource
        {
            set
            {
                _myItemsSource = value;
                OnPropertyChanged("MyItemsSource");
            }
            get
            {
                return _myItemsSource;
            }
        }

        public Command MyCommand { protected set; get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshData(){

            MyItemsSource = new ObservableCollection<Xamarin.Forms.View>();


            for (int i = 0; i < AnnouncementList.Count; i ++){

                MyItemsSource.Add(CreateStackLayout(AnnouncementList[i]));

            }



        }




        public StackLayout CreateStackLayout(SHHSAnnouncementView v)
        {

            var layout = new StackLayout { Orientation = StackOrientation.Horizontal };
            var ownerlayout = new StackLayout { Orientation = StackOrientation.Vertical };
            var annoucerLabel = new Label
            {
                Text = v.Announcer,
                FontFamily = "OpenSans-Bold",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var infoLabel = new Label
            {
                Text = v.Info,
                FontFamily = "OpenSans-Regular",
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
