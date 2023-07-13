using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;

namespace App6.Adapters
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
            vh.ParentImage.SetImageResource(mealParentItem.getImageId());
            //
            int noOfChildTextViews = vh.linearLayout_childItems.ChildCount;
            for (int index = 0; index < noOfChildTextViews; index++)
            {
                View currentView = (View)vh.linearLayout_childItems.GetChildAt(index);
                currentView.Visibility = ViewStates.Visible;
            }

            int noOfChild = mealParentItem.getChildDataItems().Count;
            if (noOfChild < noOfChildTextViews)
            {
                for (int index = noOfChild; index < noOfChildTextViews; index++)
                {
                    View currentView = (View)vh.linearLayout_childItems.GetChildAt(index);
                    currentView.Visibility = ViewStates.Gone;
                }
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
        public ImageView ParentImage { get; private set; }
        public ImageView ExpandButton { get; private set; }
        public MealViewHolder(View itemView, List<MealParentItem> mealParentItems) : base(itemView)
        {
            context = itemView.Context;
            this.mealParentItems = mealParentItems;
            linearLayout_childItems = itemView.FindViewById<LinearLayout>(Resource.Id.ll_child_items);
            ParentName = itemView.FindViewById<LinearLayout>(Resource.Id.linearLayoutHTop).FindViewById<TextView>(Resource.Id.tv_parentName);
            ParentImage = itemView.FindViewById<LinearLayout>(Resource.Id.linearLayoutHTop).FindViewById<ImageView>(Resource.Id.imageView);
            ExpandButton = itemView.FindViewById<LinearLayout>(Resource.Id.linearLayoutHBot).FindViewById<ImageView>(Resource.Id.expandButton);


            linearLayout_childItems.Visibility = ViewStates.Gone;
            int intMaxNoOfChild = 0;
            for (int index = 0; index < mealParentItems.Count; index++)
            {
                int intMaxSizeTemp = mealParentItems[index].getChildDataItems().Count;
                if (intMaxSizeTemp > intMaxNoOfChild) intMaxNoOfChild = intMaxSizeTemp;
            }
            for (int indexView = 0; indexView < intMaxNoOfChild; indexView++)
            {
                View view = LayoutInflater.From(context).Inflate(Resource.Layout.food_item, linearLayout_childItems, false);
                view.Id = indexView;
                //textView.SetBackground(ContextCompat.getDrawable(context, R.drawable.background_sub_module_text));
                //LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WRAP_CONTENT);
                //textView.SetOnClickListener(this);
                linearLayout_childItems.AddView(view);
            }
            itemView.FindViewById<CardView>(Resource.Id.mealCardView).Click += (sender, args) =>
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
                    ExpandButton.SetImageResource(Resource.Drawable.round_expand_more_24);
                    linearLayout_childItems.Visibility = ViewStates.Gone;
                }
                else
                {
                    ExpandButton.SetImageResource(Resource.Drawable.round_expand_less_24);
                    linearLayout_childItems.Visibility = ViewStates.Visible;
                }
            };
        }
    }
}
