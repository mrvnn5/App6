using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Android.Views.View;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace App6
{
    public class RecyclerDataAdapter : RecyclerView.Adapter
    {
        public List<MealParentItem> mealParentItems;
        public override int ItemCount
        { 
            get { return mealParentItems.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MealParentItem mealParentItem = mealParentItems[position];
            MealViewHolder vh = holder as MealViewHolder;
            vh.ParentName.Text = mealParentItem.getParentName();
            //
            int noOfChildTextViews = vh.linearLayout_childItems.ChildCount;
            for (int index = 0; index < noOfChildTextViews; index++)
            {
                TextView currentTextView = (TextView)vh.linearLayout_childItems.GetChildAt(index);
                currentTextView.Visibility = ViewStates.Visible;
            }

            int noOfChild = mealParentItem.getChildDataItems().Count;
            if (noOfChild < noOfChildTextViews)
            {
                for (int index = noOfChild; index < noOfChildTextViews; index++)
                {
                    TextView currentTextView = (TextView)vh.linearLayout_childItems.GetChildAt(index);
                    currentTextView.Visibility = ViewStates.Gone;
                }
            }
            for (int textViewIndex = 0; textViewIndex < noOfChild; textViewIndex++)
            {
                TextView currentTextView = (TextView)vh.linearLayout_childItems.GetChildAt(textViewIndex);
                currentTextView.Text = mealParentItem.getChildDataItems()[textViewIndex].getChildName();
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.meal_item, parent, false);
            return new MealViewHolder(itemView, mealParentItems);
        }

        public RecyclerDataAdapter(List<MealParentItem> mealParentItems)
        {
            this.mealParentItems = mealParentItems;
        }
    }

    public class MealViewHolder : RecyclerView.ViewHolder
    {
        public List<MealParentItem> mealParentItems;
        private Android.Content.Context context;
        public LinearLayout linearLayout_childItems;
        public TextView ParentName { get; private set; }
        public MealViewHolder(View itemView, List<MealParentItem> mealParentItems) : base(itemView)
        {
            context = itemView.Context;
            this.mealParentItems = mealParentItems;
            linearLayout_childItems = itemView.FindViewById<LinearLayout>(Resource.Id.ll_child_items);
            ParentName = itemView.FindViewById<LinearLayout>(Resource.Id.linearLayoutHTop).FindViewById<TextView>(Resource.Id.tv_parentName);

            linearLayout_childItems.Visibility = ViewStates.Gone;
            int intMaxNoOfChild = 0;
            for (int index = 0; index < mealParentItems.Count; index++)
            {
                int intMaxSizeTemp = mealParentItems[index].getChildDataItems().Count;
                if (intMaxSizeTemp > intMaxNoOfChild) intMaxNoOfChild = intMaxSizeTemp;
            }
            for (int indexView = 0; indexView < intMaxNoOfChild; indexView++)
            {
                TextView textView = new TextView(context);
                textView.Id = indexView;
                textView.SetPadding(0, 20, 0, 20);
                textView.Gravity = GravityFlags.Center;
                //textView.SetBackground(ContextCompat.getDrawable(context, R.drawable.background_sub_module_text));
                //LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WRAP_CONTENT);
                //textView.SetOnClickListener(this);
                linearLayout_childItems.AddView(textView);
            }
            itemView.FindViewById<LinearLayout>(Resource.Id.linearLayoutHBot).Click += (sender, args) =>
            {
                /*if ((sender as View).Id == Resource.Id.tv_parentName)
                {
                    if (linearLayout_childItems.Visibility == ViewStates.Visible)
                    {
                        linearLayout_childItems.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        linearLayout_childItems.Visibility = ViewStates.Visible;
                    }
                }
                else
                {
                    TextView textViewClicked = (TextView)(sender as View);
                }*/
                if (linearLayout_childItems.Visibility == ViewStates.Visible)
                {
                    linearLayout_childItems.Visibility = ViewStates.Gone;
                }
                else
                {
                    linearLayout_childItems.Visibility = ViewStates.Visible;
                }
            };
        }
    }
}
