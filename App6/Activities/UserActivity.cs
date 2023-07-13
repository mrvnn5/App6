using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace App6.Activities
{
    [Activity(Label = "UserActivity", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class UserActivity : Activity
    {
        private TextView userGreeting;
        private EditText editHeight;
        private EditText editWeight;
        private EditText editAge;
        private Spinner spinnerGender;
        private Spinner spinnerActivity;
        private Spinner spinnerPlan;
        private TextView rci;
        private TextView rciValue;
        private Button changeData;
        private ImageButton homeButton;
        private ImageButton logOutButton;

        private bool isChangeMode;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState); 
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_user);

            userGreeting = FindViewById<TextView>(Resource.Id.userGreeting);
            editHeight = FindViewById<EditText>(Resource.Id.editHeight);
            editWeight = FindViewById<EditText>(Resource.Id.editWeight);
            editAge = FindViewById<EditText>(Resource.Id.editAge);
            spinnerGender = FindViewById<Spinner>(Resource.Id.spinnerGender);
            spinnerActivity = FindViewById<Spinner>(Resource.Id.spinnerActivity);
            spinnerPlan = FindViewById<Spinner>(Resource.Id.spinnerPlan);
            rci = FindViewById<TextView>(Resource.Id.rci);
            rciValue = FindViewById<TextView>(Resource.Id.rciValue);
            changeData = FindViewById<Button>(Resource.Id.changeData);
            homeButton = FindViewById<ImageButton>(Resource.Id.homeButton);
            logOutButton = FindViewById<ImageButton>(Resource.Id.logOutButton);

            userGreeting.Text = "Привет, " + Intent.GetStringExtra("username") + "!";

            if (Intent.GetBooleanExtra("isSignIn", true))
            {
                SetChageMode(false);
                homeButton.Visibility = ViewStates.Visible;
                logOutButton.Visibility = ViewStates.Visible;
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
                if(isChangeMode)
                {
                    if (Intent.GetBooleanExtra("isSignIn", true))
                    {
                        //TODO

                        SetChageMode(false);
                    }
                    else
                    {
                        //TODO

                        Intent intentMain = new Intent(BaseContext, typeof(MainActivity));
                        intentMain.PutExtra("username", Intent.GetStringExtra("username"));
                        StartActivity(intentMain);
                    }
                }
                else
                {
                    SetChageMode(true);
                }
            };

            homeButton.Click += (s, e) =>
            {
                Intent intentMain = new Intent(BaseContext, typeof(MainActivity));
                intentMain.PutExtra("username", Intent.GetStringExtra("username"));
                StartActivity(intentMain);
            };
        }

        public void Register()
        {

        }

        public void SetChageMode(bool newChangeMode)
        {
            if(!newChangeMode)
            {
                editHeight.Clickable = false;
                editHeight.Focusable = false;
                editWeight.Clickable = false;
                editWeight.Focusable = false;
                editAge.Clickable = false;
                editAge.Focusable = false;
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
                editAge.Clickable = true;
                editAge.FocusableInTouchMode = true;
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