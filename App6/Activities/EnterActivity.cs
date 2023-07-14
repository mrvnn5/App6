using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Singleton;
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
        private RequestService requestService;

        private bool isSignIn = true;
        private EditText username;
        private EditText password;
        private EditText passwordRepeat;
        private TextView passwordRepeatText;
        private Button signIn;
        private Button signUp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            requestService = RequestService.getInstance();
            if(requestService == null)
            {
                Toast.MakeText(BaseContext, "Сервер сейчас недоступен\nМы уже работаем над этим", ToastLength.Long).Show();
                base.OnCreate(savedInstanceState);
                return;
            }

            if (requestService.User != null)
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
            signIn.Enabled = false;
            if (isSignIn)
            {
                if (requestService.GetUser(username.Text, password.Text))
                {
                    StartActivity(typeof(MainActivity));
                    return;
                } 
                else
                {
                    Toast.MakeText(BaseContext, "Неправильный никнейм или пароль", ToastLength.Long).Show();
                    return;
                }
            }

            if(password.Text != passwordRepeat.Text)
            {
                Toast.MakeText(BaseContext, "Пароли не совпадают", ToastLength.Long).Show();
                return;
            } 
            else if (requestService.CreateUser(username.Text, password.Text))
            {
                Intent intentUser = new Intent(BaseContext, typeof(UserActivity));
                intentUser.PutExtra("isSignIn", isSignIn);
                StartActivity(intentUser);
                OverridePendingTransition(0, 0);
            }
            else Toast.MakeText(BaseContext, "Пользователь с таким никнеймом уже существует", ToastLength.Long).Show();
            signIn.Enabled = false;
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
    }
}