using System;
using System.Collections.Generic;

namespace App6.Adapters
{
    public class MealParentItem
    {
        private String parentName;
        private List<MealChildItem> childDataItems = new List<MealChildItem>();
        private int imageId;

        public MealParentItem(String parentName, List<MealChildItem> childDataItems)
        {
            this.parentName = parentName;
            this.childDataItems = childDataItems;
        }

        public MealParentItem(String parentName)
        {
            this.parentName = parentName;
        }
        public MealParentItem(String parentName, List<MealChildItem> childDataItems, int imageId)
        {
            this.parentName = parentName;
            this.childDataItems = childDataItems;
            this.imageId = imageId;
        }

        public String getParentName()
        {
            return parentName;
        }

        public void setParentName(String parentName)
        {
            this.parentName = parentName;
        }

        public int getImageId()
        {
            return imageId;
        }

        public void setImageId(int imageId)
        {
            this.imageId = imageId;
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