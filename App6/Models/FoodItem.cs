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
    }

    public enum MealType
    {
        None = 0,
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3,
        Snack = 4
    }
}
