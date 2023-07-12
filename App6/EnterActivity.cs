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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Android.Provider.Contacts.Intents;

namespace App6
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class EnterActivity : Activity
    {
        private ImageButton enterButton;
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
                        Toast.MakeText(BaseContext, content, ToastLength.Long).Show();
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }
                }
            }


            if (true)
            {
                StartActivity(typeof(RegistrationActivity));
                OverridePendingTransition(0, 0);
            }
            base.SetTheme(Resource.Style.AppTheme); 
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enter_activity);

            //Toast.MakeText(BaseContext, result, ToastLength.Long).Show();
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