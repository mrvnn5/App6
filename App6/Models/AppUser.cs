using System.Collections.Generic;

namespace App6.Models
{
    public class AppUser
    {
        public string Username { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sex Sex { get; set; }
        public Activity Activity { get; set; }
        public Plan Plan { get; set; }
        public int Age { get; set; }
        public string PasswordHash { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        public AppUser(string username, int height, int weight, Sex sex, Activity activity, Plan plan, int age, string passwordHash)
        {
            Username = username;
            Height = height;
            Weight = weight;
            Sex = sex;
            Activity = activity;
            Plan = plan;
            Age = age;
            PasswordHash = passwordHash;
        }
    }
    public enum Sex
    {
        Male = 0,
        Female = 1
    }
    public enum Activity
    {
        Low = 0,
        Middle = 1,
        High = 2,
        VeryHigh = 3
    }
    public enum Plan
    {
        Loss = 0,
        Support = 1,
        Gain = 2
    }
}
