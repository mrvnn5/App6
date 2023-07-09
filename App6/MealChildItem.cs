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
    public class MealChildItem
    {
        private String childName;

        public MealChildItem(String childName)
        {
            this.childName = childName;
        }

        public String getChildName()
        {
            return childName;
        }

        public void setChildName(String childName)
        {
            this.childName = childName;
        }
    }
}