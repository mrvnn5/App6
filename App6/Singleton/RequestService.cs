using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Systems;
using Android.Views;
using App6.Models;
using System.Collections.Generic;

namespace App6.Singleton
{
    public class RequestService
    {
        private static RequestService instance;

        private string TCoYBServerURL;
        private string ProductBaseURL;
        private AppUser User = null;
        List<Product> Products;

        protected RequestService()
        {
            TCoYBServerURL = "https://0c8a8d23a263-14839808304840164421.ngrok-free.app/";
            ProductBaseURL = "https://cdn.jsdelivr.net/gh/goodwin74/prod_rus@latest/products.json";


        }

        public static RequestService getInstance()
        {
            if (instance == null)
                instance = new RequestService();
            return instance;
        }

        /*public CreateUser(string username,  string password)
        {

        }*/
    }
}