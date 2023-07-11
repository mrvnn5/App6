using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class EnterActivity : Activity
    {
        private ImageButton enterButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (false)
            {
                StartActivity(typeof(MainActivity));
                OverridePendingTransition(0, 0);
            }
            base.SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.enter_activity);
        }
    }
}