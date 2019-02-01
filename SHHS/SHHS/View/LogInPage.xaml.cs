
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using SHHS.Model;
using Xamarin.Forms;

namespace SHHS.View
{
    public partial class LogInPage : ContentPage
    {
        public List<SHHSLogin> a = new List<SHHSLogin>();

        public LogInPage()
        {
            InitializeComponent();
            spinner.IsVisible = false;
        }



        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            spinner.IsVisible = true;
            spinner.IsVisible = false;
            string loginSuccess = await SHHSFirebaseLogin.SignIn(userName.Text, passWord.Text);


                if (loginSuccess == null)
                {
                 ((App)Application.Current).isAdmin = true;
                await Navigation.PushAsync(new AdminPage());

            }
            else
                    await DisplayAlert("Failed to log in", loginSuccess, "OK");

            
        }

        public async Task GetData()
        {

            a.Clear();
            var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

            try
            {
                var adminstrator = await firebase.Child("South Hills Login").OnceAsync<SHHSLogin>();

                foreach (var e in adminstrator)
                {

                    a.Add(e.Object);

                }
            }
            catch (FirebaseException e)
            {

                await DisplayAlert("No Internet", e.Message, "OK");

            }


        }
    }
}
