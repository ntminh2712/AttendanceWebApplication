using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MusicBox.Entity;
using Windows.Storage;
using Newtonsoft.Json.Linq;

namespace MusicBox.Entity
{
    class APIHandle
    {
        private static String API_REGISTER = "https://2-dot-backup-server-002.appspot.com/_api/v2/members";
        private static String API_LOGIN = "https://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication";
        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        public async static Task<HttpResponseMessage> Sign_Up(Member member)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(member), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_REGISTER, content);
            return response.Result;
        }
        public static Task<HttpResponseMessage> Sign_In(string email, string password)
        {
            Dictionary<String, String> memberLogin = new Dictionary<string, string>();
            memberLogin.Add("email", "xuanhung2401@gmail.com");
            memberLogin.Add("password", "123");
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(memberLogin), System.Text.Encoding.UTF8, "application/json");

            Debug.WriteLine(httpClient.PostAsync(API_LOGIN, content).Result.Content);
            return null;
        }
        public async static Task<string> Create_Song(Song song)
        {
            HttpClient httpClient = new HttpClient();
            StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String info = await FileIO.ReadTextAsync(config_login);
            JObject data = JObject.Parse(info);
            var token = ((data.SelectToken("token")));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            return contents;
        }
    }
}
