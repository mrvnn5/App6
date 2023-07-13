using System;

namespace App6.Adapters
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