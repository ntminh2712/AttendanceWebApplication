using MusicBox.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicBox.View
{
   
    public sealed partial class Account : Page
    {
        public ObservableCollection<Song> listMySongs;
        public ObservableCollection<Song> ListMySongs { get => listMySongs; set => listMySongs = value; }
        public Account()
        {
            this.ListMySongs = new ObservableCollection<Song>();

            getMySong();
            this.InitializeComponent();
        }
        public async void getMySong()
        {
            StorageFile config_login = await ApplicationData.Current.LocalFolder.GetFileAsync("token.txt");
            String info = await FileIO.ReadTextAsync(config_login);
            JObject data = JObject.Parse(info);
            var token = ((data.SelectToken("token")));
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
            StringContent aut = new StringContent(JsonConvert.SerializeObject("Basic" + token), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.GetAsync("https://2-dot-backup-server-002.appspot.com/_api/v2/songs/get-mine").Result;
            string content = response.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(content);
            if (true)
            {
                ObservableCollection<Song> songResponse = JsonConvert.DeserializeObject<ObservableCollection<Song>>(content);
                foreach (var song in songResponse)
                {
                    this.ListMySongs.Add(song);
                }
            }
            if (ListMySongs.Count == 0)
            {
                ErrorRenctly.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorRenctly.Visibility = Visibility.Collapsed;
            }

        }


        private void PlayMusic(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            Song song = panel.Tag as Song;
            Debug.WriteLine(song.link);
            Recently.listRecently.Add(song);
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).Source = MediaSource.CreateFromUri(new Uri(song.link));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).AutoPlay = true;
            ((Parent as Frame).FindName("ThumnailPlaying") as Image).Source = new BitmapImage(new Uri(song.thumbnail, UriKind.Absolute));

        }
    }
}
