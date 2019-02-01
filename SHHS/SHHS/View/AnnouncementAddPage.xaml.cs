using System;
using System.Collections.Generic;
using SHHS.Model;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace SHHS.View
{
    public partial class AnnouncementAddPage{ 

    public string Announcer { get; set; }
    public string Detail { get; set; }

    
        public AnnouncementAddPage()
        {
            InitializeComponent();
        }




        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var app = (((App)Application.Current)).shhsMain;
            await app.announcementManager.AddAnnoucements(new SHHSAnnouncement { Announcer = this.Announcer, Info = this.Detail });
            app.RefreshCarsoulView();
            await PopupNavigation.Instance.PopAsync(true);

        }

       async void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
