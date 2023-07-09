using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6
{
    public class MealParentItem
    {
        private String parentName;
        private List<MealChildItem> childDataItems = new List<MealChildItem>();

        public MealParentItem(String parentName, List<MealChildItem> childDataItems)
        {
            this.parentName = parentName;
            this.childDataItems = childDataItems;
        }

        public MealParentItem(String parentName)
        {
            this.parentName = parentName;
        }

        public String getParentName()
        {
            return parentName;
        }

        public void setParentName(String parentName)
        {
            this.parentName = parentName;
        }

        public List<MealChildItem> getChildDataItems()
        {
            return childDataItems;
        }

        public void setChildDataItems(List<MealChildItem> childDataItems)
        {
            this.childDataItems = childDataItems;
        }
    }
}