using System;
using System.Threading.Tasks;
using Firebase.Auth;

using Firebase.Database.Query;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace SHHS.Model
{
    public static class SHHSFirebaseLogin
    {
        private const string FirebaseApiKey = "AIzaSyARjlx-Xld8Wcj1ivoE_Q5fFOKOuz3nbY8";


     

        public static async Task<string> SignUp() {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));

            try
            {
                
                var app = (App)(Application.Current);
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(CrossDeviceInfo.Current.Id + "@gmail.com".ToLower(), CrossDeviceInfo.Current.Id);
                app.AuthLink = auth;
                var user = new SHHSUser { DeviceName = CrossDeviceInfo.Current.DeviceName, Email = CrossDeviceInfo.Current.Id + "@gmail.com".ToLower(), Password = CrossDeviceInfo.Current.Id, Manufacture = CrossDeviceInfo.Current.Manufacturer, Model = CrossDeviceInfo.Current.Model, UID = auth.User.LocalId, Version = CrossDeviceInfo.Current.Version };
                await app.Client.Child("Devices").Child($"{user.UID}").PutAsync(user);
                return null;
            }
            catch (FirebaseAuthException e) {
                Console.WriteLine(e.Message);
                return e.Reason.ToString();

            }
        }
                  public static async Task<string> SignIn(string userName, string password)
        {
            try
            {
                var app = (App)(Application.Current);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
                app.AuthLink = await authProvider.SignInWithEmailAndPasswordAsync(userName, password);
                return null;
              }
            catch (FirebaseAuthException e)
         
                  {
                Console.WriteLine(e.Reason);
                return e.Reason.ToString();

            }


        }

        public static async Task<string> SignIn()
        {
            try
            {
                var app = (App)(Application.Current);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
                app.AuthLink = await authProvider.SignInWithEmailAndPasswordAsync(CrossDeviceInfo.Current.Id + "@gmail.com".ToLower(), CrossDeviceInfo.Current.Id);
                return null;
              }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine(e.Reason);
                return e.Reason.ToString();

            }


        }


   

        public static async Task<int> CheckForUpdate()
        {


            var app = (App)(Application.Current);

            if (app.Client != null)
            {

                var a = await app.Client.Child("App Info").OnceAsync<SHHSAppInfo>();
                foreach (var info in a)
                {


                    if (Device.RuntimePlatform == Device.iOS)
                    {
                            return int.Parse(info.Object.iOSBuild);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {

                            return int.Parse(info.Object.andriodBuild);

                    }
                }

            }

            return 0;


        }


    }
}

   

