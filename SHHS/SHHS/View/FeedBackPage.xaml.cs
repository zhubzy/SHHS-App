using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

using Xamarin.Forms;

namespace SHHS.View
{


    public partial class FeedBackPage : ContentPage
    {
        public Suggestion currentSuggestion;
        public FeedBackPage()
        {
            InitializeComponent();
            currentSuggestion = new Suggestion();
            BindingContext = currentSuggestion;
            
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            string withoutSpaces = currentSuggestion.Feedback.Replace(" ", "");
            if (withoutSpaces.Length >= 10)
            {

                var firebase = new FirebaseClient("https://shhs-45632.firebaseio.com/");

                //Write feedback
                currentSuggestion.Time = DateTime.Now.ToString();


                try
                {

                    await firebase.Child("Suggestions").PostAsync(currentSuggestion);
                    await DisplayAlert("Got it!", "We thank you for your feedback!", "OK");
                    currentSuggestion = new Suggestion();
                    BindingContext = currentSuggestion;
                }
                catch (FirebaseException exception)
                {

                    await DisplayAlert("Fail to submit", exception.Message, "OK");


                }


            } else {

                await DisplayAlert("Invalid", "Your feedback must be longer than 10 characters", "OK");
            
            }


        }
    }

    public class Suggestion {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Feedback { get; set; }

        public string Time { get; set; }



    }
}
