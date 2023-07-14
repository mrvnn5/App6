using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using App6.Adapters;
using App6.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App6.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class MainActivity : Activity
    {
        private RequestService requestService;

        private RecyclerDataAdapter recyclerDataAdapter;
        private RecyclerView recyclerView;
        private TextView dateTextView;
        private DateTime currentDate = DateTime.Today;
        private RecyclerView.LayoutManager mLayoutManager;
        private ImageButton userButton;

        private TextView proteinText;
        private TextView fatText;
        private TextView carbText;
        private TextView rciText;
        private TextView calText;
        private TextView calUsedTextView;
        private TextView calLeftTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            requestService = RequestService.getInstance();
            if (requestService == null)
            {
                StartActivity(typeof(EnterActivity));
            }

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";

            proteinText = FindViewById<TextView>(Resource.Id.proteinText);
            fatText = FindViewById<TextView>(Resource.Id.fatText);
            carbText = FindViewById<TextView>(Resource.Id.carbText);
            rciText = FindViewById<TextView>(Resource.Id.rciText);
            calText = FindViewById<TextView>(Resource.Id.caloriesText);
            calUsedTextView = FindViewById<TextView>(Resource.Id.calUsedTextView);
            calLeftTextView = FindViewById<TextView>(Resource.Id.calLeftTextView);

            userButton = FindViewById<ImageButton>(Resource.Id.userButton);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            dateTextView = FindViewById<TextView>(Resource.Id.dateTextView);

            mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);

            UpdateView();

            userButton.Click += (sender, e) =>
            {
                Intent intent = new Intent(BaseContext, typeof(UserActivity));
                intent.PutExtra("username", Intent.GetStringExtra("username"));
                StartActivity(intent);
            };

            dateTextView.Click += (sender, args) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                {
                    currentDate = time.Date;
                    UpdateView();
                    if (time.Date == DateTime.Now.Date) 
                    {
                        dateTextView.Text = "Сегодня";
                    } else if (time.Date == DateTime.Now.AddDays(1).Date)
                    {
                        dateTextView.Text = "Завтра";
                    } else if (time.Date == DateTime.Now.AddDays(-1).Date)
                    {
                        dateTextView.Text = "Вчера";
                    }
                    else
                        dateTextView.Text = time.ToLongDateString();
                });
                
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };
        }

        public void UpdateView()
        {
            List<MealParentItem> mealParentItems = new List<MealParentItem>
            {
                new MealParentItem("Завтрак", 
                        requestService.User.FoodItems.Where(f => f.Date.Date == currentDate && f.MealType == Models.MealType.Breakfast).ToList(), 
                        Resource.Drawable.morning),
                new MealParentItem("Обед",
                        requestService.User.FoodItems.Where(f => f.Date.Date == currentDate && f.MealType == Models.MealType.Lunch).ToList(),
                        Resource.Drawable.day),
                new MealParentItem("Ужин",
                        requestService.User.FoodItems.Where(f => f.Date.Date == currentDate && f.MealType == Models.MealType.Dinner).ToList(),
                        Resource.Drawable.night),
                new MealParentItem("Перекус",
                        requestService.User.FoodItems.Where(f => f.Date.Date == currentDate && f.MealType == Models.MealType.Snack).ToList(),
                        Resource.Drawable.other)
            };

            recyclerDataAdapter = new RecyclerDataAdapter(mealParentItems);
            recyclerView.SetAdapter(recyclerDataAdapter);

            var items = requestService.User.FoodItems
                .Where(f => f.Date.Date == currentDate.Date).ToList()
                .Join(requestService.Products, 
                    f => f.ProductId, 
                    p => p.Id, 
                    (f, p) => new { Weight = f.Weight,
                    Protein = p.GetProtein(), 
                    Fat = p.GetFat(),
                    Carb = p.GetCarb(),
                    Cal = p.Kcal})
                .ToList();

            proteinText.Text = items.Sum(i => i.Protein * i.Weight / 100).ToString();
            fatText.Text = items.Sum(i => i.Fat * i.Weight / 100).ToString();
            carbText.Text = items.Sum(i => i.Carb * i.Weight / 100).ToString();
            calText.Text = calUsedTextView.Text = items.Sum(i => i.Cal * i.Weight / 100).ToString();
            rciText.Text = items.Sum(i =>
                (int)Math.Round(
                (i.Cal * i.Weight / 100)
                / requestService.GetRCI() * 100)) + "%";
            calLeftTextView.Text = (requestService.GetRCI() - items.Sum(i => i.Cal * i.Weight / 100)).ToString();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    [Obsolete]
    public class DatePickerFragment : DialogFragment,
                                  DatePickerDialog.IOnDateSetListener
    {
        public static DateTime currently;
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(DateTime current, Action<DateTime> onDateSelected)
        {
            currently = current;
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }



        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            
            DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                           this,
                                                           currently.Year,
                                                           currently.Month - 1,
                                                           currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}