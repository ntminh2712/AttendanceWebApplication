using MusicBox.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
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
    /// 
    
    public sealed partial class Recently : Page
    {
        //public static Recently currentList = new Recently();
        public static ObservableCollection<Song> listRecently = new ObservableCollection<Song>();
        public ObservableCollection<Song> ListRecently { get => listRecently; set => listRecently = value; }
        public Recently()
        {
            this.ListRecently = listRecently;
            this.InitializeComponent();
        }
        private void PlayMusic(object sender, TappedRoutedEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            Song song = panel.Tag as Song;
            Debug.WriteLine(song.link);
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).Source = MediaSource.CreateFromUri(new Uri(song.link));
            ((Parent as Frame).FindName("mediaPlayer") as MediaPlayerElement).AutoPlay = true;
        }
    }
}
