using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using App6.Singleton;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace App6.Activities
{
    [Activity(Label = "UserActivity", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class UserActivity : Activity
    {
        private RequestService requestService;

        private TextView userGreeting;
        private EditText editHeight;
        private EditText editWeight;
        private TextView editAge;
        private Spinner spinnerGender;
        private Spinner spinnerActivity;
        private Spinner spinnerPlan;
        private TextView rci;
        private TextView rciValue;
        private Button changeData;
        private ImageButton homeButton;
        private ImageButton logOutButton;
        private ProgressBar progressBar;

        private bool isChangeMode;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            requestService = RequestService.getInstance();
            if (requestService == null)
            {
                StartActivity(typeof(EnterActivity));
            }

            DateTime currentDate = requestService.User.BirthDate;
            
            base.OnCreate(savedInstanceState); 
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_user);

            userGreeting = FindViewById<TextView>(Resource.Id.userGreeting);
            editHeight = FindViewById<EditText>(Resource.Id.editHeight);
            editWeight = FindViewById<EditText>(Resource.Id.editWeight);
            editAge = FindViewById<TextView>(Resource.Id.editAge);
            spinnerGender = FindViewById<Spinner>(Resource.Id.spinnerGender);
            spinnerActivity = FindViewById<Spinner>(Resource.Id.spinnerActivity);
            spinnerPlan = FindViewById<Spinner>(Resource.Id.spinnerPlan);
            rci = FindViewById<TextView>(Resource.Id.rci);
            rciValue = FindViewById<TextView>(Resource.Id.rciValue);
            changeData = FindViewById<Button>(Resource.Id.changeData);
            homeButton = FindViewById<ImageButton>(Resource.Id.homeButton);
            logOutButton = FindViewById<ImageButton>(Resource.Id.logOutButton);
            progressBar = FindViewById<ProgressBar>(Resource.Id.activityIndicator);

            progressBar.Visibility = ViewStates.Gone;

            userGreeting.Text = "Привет, " + requestService.User.Username + "!";

            if (Intent.GetBooleanExtra("isSignIn", true))
            {
                SetChageMode(false);
                homeButton.Visibility = ViewStates.Visible;
                logOutButton.Visibility = ViewStates.Visible;
                editHeight.Text = requestService.User.Height.ToString();
                editWeight.Text = requestService.User.Weight.ToString();
                editAge.Text = requestService.User.BirthDate.Date.ToString("dd/MM/yyyy");
                spinnerGender.SetSelection((int)requestService.User.Sex);
                spinnerActivity.SetSelection((int)requestService.User.Activity);
                spinnerPlan.SetSelection((int)requestService.User.Plan);

                rciValue.Text = requestService.GetRCI().ToString();
            }
            else
            {
                SetChageMode(true);
                rci.Visibility = ViewStates.Invisible;
                rciValue.Visibility = ViewStates.Invisible;
                homeButton.Visibility = ViewStates.Invisible;
                logOutButton.Visibility = ViewStates.Invisible;
            }

            changeData.Click += (s, e) =>
            {
                changeData.Enabled = false;
                changeData.Visibility = ViewStates.Gone;
                progressBar.Visibility = ViewStates.Visible;
                if (isChangeMode)
                {
                    requestService.User.Height = Convert.ToInt32(editHeight.Text);
                    requestService.User.Weight = Convert.ToInt32(editWeight.Text);
                    requestService.User.BirthDate = DateTime.ParseExact(editAge.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    requestService.User.Sex = (Models.Sex)spinnerGender.SelectedItemPosition;
                    requestService.User.Activity = (Models.Activity)spinnerActivity.SelectedItemPosition;
                    requestService.User.Plan = (Models.Plan)spinnerPlan.SelectedItemPosition;

                    requestService.UpdateUser();

                    rciValue.Text = requestService.GetRCI().ToString();

                    if (Intent.GetBooleanExtra("isSignIn", true))
                    {
                        SetChageMode(false);
                    }
                    else
                    {
                        StartActivity(typeof(MainActivity));
                    }
                }
                else
                {
                    SetChageMode(true);
                }
                changeData.Enabled = true;
                changeData.Visibility = ViewStates.Gone;
                progressBar.Visibility = ViewStates.Visible;
            };

            homeButton.Click += (s, e) =>
            {
                StartActivity(typeof(MainActivity));
            };

            logOutButton.Click += (s, e) =>
            {
                requestService.ClearToken().Wait();
                requestService.User = null;
                StartActivity(typeof(EnterActivity));
            };



            editAge.Click += (sender, args) =>
            {
                if (isChangeMode)
                {
                    DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                    {
                        currentDate = time.Date;
                        editAge.Text = currentDate.ToString("dd/MM/yyyy");
                    });

                    frag.Show(FragmentManager, DatePickerFragment.TAG);
                }
            };
        }

        public void SetChageMode(bool newChangeMode)
        {
            if(!newChangeMode)
            {
                editHeight.Clickable = false;
                editHeight.Focusable = false;
                editWeight.Clickable = false;
                editWeight.Focusable = false;
                spinnerGender.Clickable = false;
                spinnerActivity.Clickable = false;
                spinnerPlan.Clickable = false;

                editHeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                editWeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                editAge.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditText);
                spinnerGender.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                spinnerActivity.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                spinnerPlan.SetBackgroundResource(Resource.Drawable.spinnerStyle);
                changeData.Text = "Изменить данные";

                isChangeMode = false;
            }
            else
            {
                editHeight.Clickable = true;
                editHeight.FocusableInTouchMode = true;
                editWeight.Clickable = true;
                editWeight.FocusableInTouchMode = true;
                spinnerGender.Clickable = true;
                spinnerActivity.Clickable = true;
                spinnerPlan.Clickable = true;

                editHeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                editWeight.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                editAge.SetBackgroundResource(Resource.Drawable.RoundedBorder_EditTextSelected);
                spinnerGender.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                spinnerActivity.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                spinnerPlan.SetBackgroundResource(Resource.Drawable.spinnerStyleSelected);
                changeData.Text = "Сохранить данные";

                isChangeMode = true;
            }
        }
    }
}