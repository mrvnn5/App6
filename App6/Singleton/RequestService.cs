﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Systems;
using Android.Views;
using App6.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System;
using App6.Activities;

namespace App6.Singleton
{
    public class RequestService
    {
        public StatusCode Status { get; private set; }
        private static RequestService instance;
        private ProductService productService;

        private string TCoYBServerURL;
        private string ProductBaseURL;
        public AppUser User;

        public enum StatusCode
        {
            SUCCESS = 0,
            JSON_LOAD_ERROR = 1,
            LOCAL_TOKEN_ERROR = 2,
            BAD_TOKEN = 3
        }

        protected RequestService()
        {
            TCoYBServerURL = @"https://0c8a8d23a263-14839808304840164421.ngrok-free.app/";
            ProductBaseURL = @"https://cdn.jsdelivr.net/gh/goodwin74/prod_rus@latest/products.json";

            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
               delegate { return true; }
            );

            if(!LoadProducts())
            {
                Status = StatusCode.JSON_LOAD_ERROR; 
                return;
            }

            Task<UserToken> task = GetToken();
            task.Wait();
            if (task.Result == null)
            {
                Status = StatusCode.LOCAL_TOKEN_ERROR; 
                return;
            }

            if(!GetUser(task.Result))
            {
                Status = StatusCode.BAD_TOKEN;
                return;
            }

            Status = StatusCode.SUCCESS;
        }

        public static RequestService GetInstance()
        {
            if (instance == null)
                instance = new RequestService();
            return instance;
        }

        public void LogOut()
        {
            ClearToken().Wait();
            User = null;
            Status = RequestService.StatusCode.BAD_TOKEN;
        }

        public async Task<bool> SetToken()
        {
            try
            {
                await SecureStorage.SetAsync("username", User.UserToken.Username);
                await SecureStorage.SetAsync("accessToken", User.UserToken.AccessToken);
                await SecureStorage.SetAsync("tokenExpiredAt", User.UserToken.ExpiredAt.ToString());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ClearToken()
        {
            try
            {
                await SecureStorage.SetAsync("username", "");
                await SecureStorage.SetAsync("accessToken", "");
                await SecureStorage.SetAsync("tokenExpiredAt", "");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<UserToken> GetToken()
        {
            try
            {
                UserToken userToken = new UserToken()
                {
                    Username = await SecureStorage.GetAsync("username"),
                    AccessToken = await SecureStorage.GetAsync("accessToken"),
                    ExpiredAt = Convert.ToDateTime(await SecureStorage.GetAsync("tokenExpiredAt"))
                };

                return userToken;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool LoadProducts()
        {
            var request = HttpWebRequest.Create(ProductBaseURL);
            request.Method = "GET";

            request.ContentType = "application/json";

            HttpWebResponse response;

            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                return false;
            }

            using (response)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return false;
                    }
                    else
                    {
                        productService = ProductService.getInstance(JsonConvert.DeserializeObject<List<Product>>(content));
                        if (productService == null) return false;
                        return true;
                    }
                }
            }
        }

        public int GetRCI()
        {
            return (int)Math.Round((User.Weight * 10
                    + User.Height * 6.25
                    - ((new DateTime(1, 1, 1) + (DateTime.Now - User.BirthDate)).Year - 1) * 5
                    + (User.Sex == Models.Sex.Male ? 5 : -161))
                    * (User.Activity == Models.Activity.Low ? 1.375 :
                        User.Activity == Models.Activity.Middle ? 1.55 :
                        User.Activity == Models.Activity.High ? 1.7 : 1.9)
                    * (User.Plan == Models.Plan.Loss ? 0.9 :
                        User.Plan == Models.Plan.Support ? 1 : 1.1));
        }

        public bool SendRequest(WebRequest request)
        {
            request.ContentType = "application/json";
            request.Headers.Add("ngrok-skip-browser-warning", "1");

            HttpWebResponse response;

            try
            {
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                return false;
            }
            
            using (response)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return false;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return false;
                    }
                    else
                    {
                        User = JsonConvert.DeserializeObject<AppUser>(content);
                        SetToken().Wait();
                        Status = StatusCode.SUCCESS;
                        return true;
                    }
                }
            }
        }

        public bool CreateUser(string username, string password)
        {
            var request = HttpWebRequest.Create(TCoYBServerURL + "User/createUser?username=" + username + "&password=" + password);
            request.Method = "POST";
            return SendRequest(request);
        }

        public bool GetUser(string username, string password)
        {
            var request = HttpWebRequest.Create(TCoYBServerURL + "User/byUsername?username=" + username + "&password=" + password);
            request.Method = "GET";
            return SendRequest(request);
        }

        public bool GetUser(UserToken userToken)
        {
            var request = HttpWebRequest.Create(TCoYBServerURL + "User/byToken");
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(userToken));
            }
            return SendRequest(request);
        }

        public bool UpdateUser()
        {
            var request = HttpWebRequest.Create(TCoYBServerURL + "User/updateUser");
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(User));
            }
            return SendRequest(request);
        }
    }
}