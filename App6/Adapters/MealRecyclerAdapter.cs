using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using App6.Activities;
using App6.Models;
using App6.Singleton;
using Org.W3c.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Adapters
{
    public class MealRecyclerAdapter : RecyclerView.Adapter
    {
        private RequestService RequestService;
        private MainActivity MainActivity;

        public MealRecyclerAdapter(MainActivity mainActivity)
        {
            MainActivity = mainActivity;
            RequestService = RequestService.GetInstance();
        }

        public override int ItemCount => 4;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MealRecyclerAdapterViewHolder vh = holder as MealRecyclerAdapterViewHolder;
            vh.FoodItemsLinearLayout.RemoveAllViews();
            vh.UpdateTotal(MainActivity.currentDate, position);

            switch (position)
            {
                case 0:
                    vh.ParentImage.SetImageResource(Resource.Drawable.morning);
                    vh.MealName.Text = "Завтрак";
                    break;
                case 1:
                    vh.ParentImage.SetImageResource(Resource.Drawable.day);
                    vh.MealName.Text = "Обед";
                    break;
                case 2:
                    vh.ParentImage.SetImageResource(Resource.Drawable.night);
                    vh.MealName.Text = "Ужин";
                    break;
                default:
                    vh.ParentImage.SetImageResource(Resource.Drawable.other);
                    vh.MealName.Text = "Перекус";
                    break;
            }

            int i = 0;
            foreach (FoodItem item in RequestService.User.FoodItems.Where(f => f.Date.Date == MainActivity.currentDate.Date && (int)f.MealType == position))
            {
                View currentView = LayoutInflater.FromContext(vh.ItemView.Context).Inflate(Resource.Layout.food_item, vh.FoodItemsLinearLayout, false);
                vh.FoodItemsLinearLayout.AddView(currentView);

                currentView.FindViewById<TextView>(Resource.Id.productName).Text = item.Product.Name;

                double cal = item.Product.Kcal * item.Weight / 100;
                currentView.FindViewById<TextView>(Resource.Id.cal).Text = cal.ToString();

                currentView.FindViewById<TextView>(Resource.Id.weight).Text = item.Weight.ToString() + " г";

                currentView.FindViewById<ImageButton>(Resource.Id.deleteProductButton).Click += (s, e) =>
                {
                    vh.FoodItemsLinearLayout.RemoveView(currentView);
                    RequestService.User.FoodItems.Remove(item);
                    RequestService.UpdateUser();
                    vh.UpdateTotal(MainActivity.currentDate, position);
                    MainActivity.UpdateTotal();
                };

                double protein = item.Product.GetProtein() * item.Weight / 100;
                currentView.FindViewById<TextView>(Resource.Id.productProtein).Text = protein.ToString();

                double fat = item.Product.GetFat() * item.Weight / 100;
                currentView.FindViewById<TextView>(Resource.Id.productFat).Text = fat.ToString();

                double carb = item.Product.GetCarb() * item.Weight / 100;
                currentView.FindViewById<TextView>(Resource.Id.productCarb).Text = carb.ToString();

                int rci = (int)Math.Round(
                    (item.Product.Kcal * item.Weight / 100)
                    / RequestService.GetRCI() * 100);
                currentView.FindViewById<TextView>(Resource.Id.productRci).Text = rci + "%";

                i++;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.meal_item, parent, false);
            return new MealRecyclerAdapterViewHolder(itemView);
        }
    }

    internal class MealRecyclerAdapterViewHolder : RecyclerView.ViewHolder
    {
        private RequestService RequestService;

        public LinearLayout FoodItemsLinearLayout;
        public TextView MealName { get; private set; }
        public TextView TotalMealProtein { get; private set; }
        public TextView TotalMealFat { get; private set; }
        public TextView TotalMealCarb { get; private set; }
        public TextView TotalMealRci { get; private set; }
        public TextView TotalMealCal { get; private set; }
        public ImageView ParentImage { get; private set; }
        public ImageView ExpandButton { get; private set; }

        public MealRecyclerAdapterViewHolder(View itemView) : base(itemView)
        {
            RequestService = RequestService.GetInstance();

            FoodItemsLinearLayout = itemView.FindViewById<LinearLayout>(Resource.Id.ll_child_items);
            MealName = itemView.FindViewById<TextView>(Resource.Id.tv_parentName);
            ParentImage = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            ExpandButton = itemView.FindViewById<ImageView>(Resource.Id.expandButton);
            TotalMealProtein = itemView.FindViewById<TextView>(Resource.Id.totalMealProtein);
            TotalMealFat = itemView.FindViewById<TextView>(Resource.Id.totalMealFat);
            TotalMealCarb = itemView.FindViewById<TextView>(Resource.Id.totalMealCarb);
            TotalMealRci = itemView.FindViewById<TextView>(Resource.Id.totalMealRci);
            TotalMealCal = itemView.FindViewById<TextView>(Resource.Id.tCal);

            FoodItemsLinearLayout.Visibility = ViewStates.Gone;

            itemView.FindViewById<CardView>(Resource.Id.mealCardView).Click += (sender, args) =>
            {
                if (FoodItemsLinearLayout.Visibility == ViewStates.Visible)
                {
                    ExpandButton.SetImageResource(Resource.Drawable.round_expand_more_24);
                    FoodItemsLinearLayout.Visibility = ViewStates.Gone;
                }
                else
                {
                    ExpandButton.SetImageResource(Resource.Drawable.round_expand_less_24);
                    FoodItemsLinearLayout.Visibility = ViewStates.Visible;
                }
            };
        }

        public void UpdateTotal(DateTime currentDate, int position)
        {
            var items = RequestService.User.FoodItems
                .Where(f => f.Date.Date == currentDate.Date && (int)f.MealType == position).ToList();

            TotalMealProtein.Text = items.Sum(i => i.Product.GetProtein() * i.Weight / 100).ToString();
            TotalMealFat.Text = items.Sum(i => i.Product.GetFat() * i.Weight / 100).ToString();
            TotalMealCarb.Text = items.Sum(i => i.Product.GetCarb() * i.Weight / 100).ToString();
            TotalMealCal.Text = items.Sum(i => i.Product.Kcal * i.Weight / 100) + "\nКкал";
            TotalMealRci.Text = items.Sum(i =>
                (int)Math.Round(
                (i.Product.Kcal * i.Weight / 100)
                / RequestService.GetRCI() * 100)) + "%";
        }
    }
}