using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Service
{
    public class WebApiService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly WebClient wclient = new WebClient();
        public T Get<T>(string url)
        {
            try
            {
                var data = wclient.DownloadString("http://" + url);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default(T);
            }
        }
        public string Get(string url)
        {
            try
            {
                var data = wclient.DownloadString(url);
                return data;
            }
            catch
            {
                return null;
            }
        }
        public void Read(string url)
        {
            try
            {
                var data = wclient.OpenRead("http://" + url);
            }
            catch
            {

            }
        }
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(responseString);
                return data;
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<dynamic, dynamic> values)
        {
            try
            {
                url = url + "?";
                foreach (var item in values)
                {
                    url = url + item.Key + "=" + item.Value + "&";
                }
                var response = await client.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<T> GetPostAsync<T>(string url)
        {
            try
            {
                var response = await client.PostAsync(url, null);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
                return default(T);
            }

        }

        public async Task<T> GetPostAsync<T>(string url, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
                return default(T);
            }
        }
        public async Task<string> GetPostAsync(string url, Dictionary<string, string> values, string ip = "127.0.0.1")
        {
            try
            {
                values.Add("ip", ip);
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> GetPostAsync(string url, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
