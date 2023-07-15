using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App6.Singleton
{
    public class ProductService
    {
        private static ProductService instance;

        public List<Product> Products;

        public ProductService(List<Product> products)
        {
            Products = products;
        }

        public static ProductService getInstance(List<Product> products = null)
        {
            if (instance == null || instance.Products == null || instance.Products.Count == 0)
            {
                if (products == null || products.Count == 0) return null;
                instance = new ProductService(products);
            }
            return instance;
        }
    }
}