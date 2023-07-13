using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;

namespace App6.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class EnterActivity : Activity
    {
        private bool isSignIn = true;
        private EditText username;
        private EditText password;
        private EditText passwordRepeat;
        private TextView passwordRepeatText;
        private Button signIn;
        private Button signUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            /*Task<string> r = CheckUser();
            r.Wait();
            string result = r.Result;*/

            var request = HttpWebRequest.Create(@"https://cdn.jsdelivr.net/gh/goodwin74/prod_rus@latest/products.json");
            request.ContentType = "application/json";
            request.Headers.Add("ngrok-skip-browser-warning", "1");
            request.Method = "GET";
            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
               delegate { return true; }
            );

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        //Toast.MakeText(BaseContext, content, ToastLength.Long).Show();
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }
                }
            }


            if (false)
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(0, 0);
            }
            base.SetTheme(Resource.Style.AppTheme); 
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enter_activity);

            username = FindViewById<EditText>(Resource.Id.usernameEditText);
            password = FindViewById<EditText>(Resource.Id.passEditText);
            passwordRepeat = FindViewById<EditText>(Resource.Id.repeatpassEditText);
            signIn = FindViewById<Button>(Resource.Id.signInButton);
            signUp = FindViewById<Button>(Resource.Id.signUpButton);
            passwordRepeatText = FindViewById<TextView>(Resource.Id.repeatpass);

            signUp.Click += (sender, args) => ChangeIsSignIn();
            signIn.Click += (sender, args) => Continue();

            //Toast.MakeText(BaseContext, result, ToastLength.Long).Show();
        }

        public void Continue()
        {
            if(isSignIn)
            {
                //TODO server

                Intent intentMain = new Intent(BaseContext, typeof(MainActivity));
                intentMain.PutExtra("username", Intent.GetStringExtra("username"));
                StartActivity(intentMain);
                return;
            }
            if(password.Text != passwordRepeat.Text)
            {
                Toast.MakeText(BaseContext, "Пароли не совпадают", ToastLength.Long).Show();
                return;
            }
            Intent intentUser = new Intent(BaseContext, typeof(UserActivity));
            intentUser.PutExtra("isSignIn", isSignIn);
            intentUser.PutExtra("username", username.Text);
            intentUser.PutExtra("password", password.Text);
            StartActivity(intentUser);
            OverridePendingTransition(0, 0);
        }

        public void ChangeIsSignIn()
        {
            isSignIn = !isSignIn;

            if(isSignIn)
            {
                passwordRepeatText.Visibility = ViewStates.Invisible;
                passwordRepeat.Visibility = ViewStates.Invisible;
                signIn.Text = "Вход";
                signUp.Text = "Я новый пользователь";
            } 
            else
            {
                passwordRepeatText.Visibility = ViewStates.Visible;
                passwordRepeat.Visibility = ViewStates.Visible;
                signIn.Text = "Регистрация";
                signUp.Text = "У меня уже есть аккаунт";
            }
        }

        public async Task<string> CheckUser()
        {
            /*var client = new RestClient();
            var request = new RestRequest("https://jsonplaceholder.typicode.com/posts");
            request.RequestFormat = DataFormat.Json;
            Toast.MakeText(BaseContext, "gjlujnjdrf", ToastLength.Long).Show();
            var response = await client.GetAsync(request);
            Toast.MakeText(BaseContext, "ura", ToastLength.Long).Show();
            return response.ToString();*/

            string URL = "https://jsonplaceholder.typicode.com/posts";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(URL);

            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }
}