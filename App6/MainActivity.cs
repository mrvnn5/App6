using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace App6
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        private List<MealParentItem> mealParentItems;
        private RecyclerDataAdapter recyclerDataAdapter;
        private RecyclerView recyclerView;
        private RecyclerView.LayoutManager mLayoutManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            mealParentItems = new List<MealParentItem>
            {
                new MealParentItem("Завтрак", new List<MealChildItem>
                {
                    new MealChildItem("rebenok1"),
                    new MealChildItem("rebenok2"),
                }),
                new MealParentItem("Обед"),
                new MealParentItem("Ужин"),
                new MealParentItem("Перекус")
            };

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

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
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}