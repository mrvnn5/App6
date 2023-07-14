using App6.Models;
using System;
using System.Collections.Generic;

namespace App6.Adapters
{
    public class MealParentItem
    {
        private String parentName;
        private List<FoodItem> childDataItems = new List<FoodItem>();
        private int imageId;

        public MealParentItem(String parentName, List<FoodItem> childDataItems)
        {
            this.parentName = parentName;
            this.childDataItems = childDataItems;
        }

        public MealParentItem(String parentName)
        {
            this.parentName = parentName;
        }
        public MealParentItem(String parentName, List<FoodItem> childDataItems, int imageId)
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

        public List<FoodItem> getChildDataItems()
        {
            return childDataItems;
        }

        public void setChildDataItems(List<FoodItem> childDataItems)
        {
            this.childDataItems = childDataItems;
        }
    }
}