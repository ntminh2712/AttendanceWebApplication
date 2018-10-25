using MusicBox.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Playlist : Page
    {
        private Song currentSong;

        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        public Playlist()
        {
            this.currentSong = new Song();
            this.InitializeComponent();

        }


        private async void SignUp(object sender, RoutedEventArgs e)
        {
            this.currentSong.name = this.Name.Text;
            this.currentSong.description = this.Description.Text;
            this.currentSong.singer = this.Singer.Text;
            this.currentSong.author = this.Author.Text;
            this.currentSong.thumbnail = this.Thumbnail.Text;
            this.currentSong.link = this.Link.Text;
            HttpClient httpClient = new HttpClient();
            StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String info = await FileIO.ReadTextAsync(config_login);
            JObject data = JObject.Parse(info);
            var token = ((data.SelectToken("token")));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
            var content = new StringContent(JsonConvert.SerializeObject(this.currentSong), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content).Result;
            var contents = response.StatusCode; ;
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {

                var messageDialog = new MessageDialog("Add song unsuccess !.");
                await messageDialog.ShowAsync();
            }
            else
            {
                // Xu ly loi.
                var messageDialog = new MessageDialog("Add song unsuccess !.");
                await messageDialog.ShowAsync();
            }

        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Name.Text = "";
            this.Description.Text = "";
            this.Author.Text = "";
            this.Thumbnail.Text = "";
            this.Singer.Text = "";
            this.Link.Text = "";
        }
    }
}

