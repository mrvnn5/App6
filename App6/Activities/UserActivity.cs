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
        private RequestService RequestService;

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
        public DateTime dt = DateTime.Now.Date.AddYears(-12); 

        private bool isChangeMode;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestService = RequestService.GetInstance();
            if (RequestService.Status != RequestService.StatusCode.SUCCESS)
            {
                StartActivity(typeof(EnterActivity));
            }

            DateTime currentDate = RequestService.User.BirthDate;
            
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

            userGreeting.Text = "Привет, " + RequestService.User.Username + "!";

            if (Intent.GetBooleanExtra("isSignIn", true))
            {
                SetChageMode(false);
                homeButton.Visibility = ViewStates.Visible;
                logOutButton.Visibility = ViewStates.Visible;
                editHeight.Text = RequestService.User.Height.ToString();
                editWeight.Text = RequestService.User.Weight.ToString();
                editAge.Text = RequestService.User.BirthDate.Date.ToString("dd/MM/yyyy");
                spinnerGender.SetSelection((int)RequestService.User.Sex);
                spinnerActivity.SetSelection((int)RequestService.User.Activity);
                spinnerPlan.SetSelection((int)RequestService.User.Plan);

                rciValue.Text = RequestService.GetRCI().ToString();
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
                if ((Convert.ToInt32(editHeight.Text) < 100) || (Convert.ToInt32(editHeight.Text) > 300) || (editHeight.Text.Equals(null)))
                {
                    Toast.MakeText(BaseContext, "Рост введён некорректно", ToastLength.Long).Show();
                    return;
                }
                if ((Convert.ToInt32(editWeight.Text) < 35) || (Convert.ToInt32(editWeight.Text) > 300) || (editWeight.Text.Equals(null)))
                {
                    Toast.MakeText(BaseContext, "Вес введён некорректно", ToastLength.Long).Show();
                    return;
                }
               
                if (Convert.ToDateTime(editAge.Text).Date > dt)
                {
                    Toast.MakeText(BaseContext, "Упс! Кажется, вам ещё нет 12 лет. Приложение может быть опасным для вас!", ToastLength.Long).Show();
                    return;
                }
                changeData.Enabled = false;
                if (isChangeMode)
                {
                    RequestService.User.Height = Convert.ToInt32(editHeight.Text);
                    RequestService.User.Weight = Convert.ToInt32(editWeight.Text);
                    RequestService.User.BirthDate = DateTime.ParseExact(editAge.Text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    RequestService.User.Sex = (Models.Sex)spinnerGender.SelectedItemPosition;
                    RequestService.User.Activity = (Models.Activity)spinnerActivity.SelectedItemPosition;
                    RequestService.User.Plan = (Models.Plan)spinnerPlan.SelectedItemPosition;

                    RequestService.UpdateUser();

                    rciValue.Text = RequestService.GetRCI().ToString();

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
            };

            homeButton.Click += (s, e) =>
            {
                StartActivity(typeof(MainActivity));
            };

            logOutButton.Click += (s, e) =>
            {
                RequestService.LogOut();
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