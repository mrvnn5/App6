using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace App6
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme.Splash", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private List<MealParentItem> mealParentItems;
        private RecyclerDataAdapter recyclerDataAdapter;
        private RecyclerView recyclerView;
        private TextView dateTextView;
        private DateTime currentDate = DateTime.Today;
        private RecyclerView.LayoutManager mLayoutManager;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            base.SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "";


            mealParentItems = new List<MealParentItem>
            {
                new MealParentItem("Завтрак", new List<MealChildItem>
                {
                    new MealChildItem("rebenok1"),
                    new MealChildItem("rebenok2"),
                }, Resource.Drawable.morning),
                new MealParentItem("Обед", new List<MealChildItem>
                {
                    new MealChildItem("rebenok1"),
                    new MealChildItem("rebenok2"),
                }, Resource.Drawable.day),
                new MealParentItem("Ужин", new List<MealChildItem>
                {
                    new MealChildItem("rebenok1"),
                    new MealChildItem("rebenok2"),
                }, Resource.Drawable.night),
                new MealParentItem("Перекус", new List<MealChildItem>
                {
                    new MealChildItem("rebenok1"),
                    new MealChildItem("rebenok2"),
                }, Resource.Drawable.other)
            };

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            dateTextView = FindViewById<TextView>(Resource.Id.dateTextView);

            mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);

            recyclerDataAdapter = new RecyclerDataAdapter(mealParentItems);
            recyclerView.SetAdapter(recyclerDataAdapter);

            

            /*items = new List<string>();

            items.Add("asdasdad");
            items.Add("sdaffdgfds");
            items.Add("lkfghdfd");


            expandableListView = FindViewById<ExpandableListView>(Resource.Id.expandableListView1);
            expandableListView.SetAdapter(new ExpandableListAdapter(this, items));*/

            dateTextView.Click += (sender, args) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(currentDate, delegate (DateTime time)
                {
                    currentDate = time.Date;
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