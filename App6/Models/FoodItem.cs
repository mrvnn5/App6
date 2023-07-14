using System;

namespace App6.Models
{
    public class FoodItem
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        public int Weight { get; set; }
        public DateTime Date { get; set; }
        public MealType MealType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return Id == (obj as FoodItem).Id;
        }
    }

    public enum MealType
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner = 2,
        Snack = 3
    }
}
